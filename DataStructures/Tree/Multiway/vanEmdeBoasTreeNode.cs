using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced.Tree
{
    /// <summary>
    /// van Emde Boas Tree node
    /// van Emde Boas树的结点
    /// </summary>
    /// <typeparam name="TData">结点所存储的数据类型</typeparam>
    /// <remarks>
    /// Performance of the tree can be improved by dividing lsqrt/hsqrt through bit oprations
    /// vEB树的性能可以通过把lsqrt/hsqrt的除法改为位运算来提高
    /// </remarks>
    public class vanEmdeBoasTreeNode<TData> : IMultiwayTreeNode
    {
        readonly int u;
        readonly int lsqrt, hsqrt, lsbits;// low/high bit divider
        [ContractPublicPropertyName("Count")]
        int num;
        [ContractPublicPropertyName("Min")]
        int? min;
        [ContractPublicPropertyName("Max")]
        int? max;
        vanEmdeBoasTreeNode<TData>[] _clusters;
        vanEmdeBoasTreeNode<object> _summary; // summary doesn't need data
        static readonly Func<int, vanEmdeBoasTreeNode<object>> createsummary
            = (size) => new vanEmdeBoasTreeNode<object>(size);

        /// <summary>
        /// Min key
        /// 结点中最小的关键字
        /// </summary>
        public int? Min { get { return min; } protected set { min = value; } }
        /// <summary>
        /// Max key
        /// 结点中最大的关键字
        /// </summary>
        public int? Max { get { return max; } protected set { max = value; } }
        /// <summary>
        /// Data of min key
        /// 结点中最小关键字存储的数据
        /// </summary>
        public TData MinData { get; protected set; }
        /// <summary>
        /// 结点中最大关键字存储的数据
        /// </summary>
        public TData MaxData { get; protected set; }
        /// <summary>
        /// size of clusters stored in this node, also known as 'universal set size' of the node.
        /// 结点存储的簇的规模，也就是所谓“全集”的大小
        /// </summary>
        public int UniverseSize => 1 << u;
        /// <summary>
        /// UniverseSize described in binary bits
        /// 二进制位数表示的全集大小
        /// </summary>
        public int UniverseBits => u;
        public vanEmdeBoasTreeNode<TData>[] Clusters => _clusters;
        /// <summary>
        /// count of non-null elements
        /// 不为空的元素的个数
        /// </summary>
        public int Count => num;
        /// <summary>
        /// Summary of clusters
        /// 子簇的缩略结点
        /// </summary>
        public vanEmdeBoasTreeNode<object> Summary
        {
            get
            {
                if (_summary == null) _summary = new vanEmdeBoasTreeNode<object>(hsqrt);
                return _summary;
            }
        }
        /// <summary>
        /// this property returns <see cref="Clusters"/>
        /// </summary>
        IReadOnlyCollection<IMultiwayTreeNode> IMultiwayTreeNode.Children => Clusters;

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(Min == null || (Max != null && min <= max));
            Contract.Invariant(Count <= 1 || UniverseBits == 1 || Contract.Exists(Clusters, (cluster) => cluster?.Contains(max.Value & lsbits) ?? false)); // Max value is also in sub-clusters
            Contract.Invariant(_summary == null || _summary.UniverseSize == Clusters.Length);
            Contract.Invariant(Count >= 0 && Count <= UniverseSize);
        }

        /// <summary>
        /// 新建一个vEB树的节点
        /// </summary>
        /// <param name="bits">size必须是2的幂，尽管代码中没有进行检查，但如果不是2的幂则会运行出错</param>
        public vanEmdeBoasTreeNode(int bits)
        {
            Contract.Requires<ArgumentException>(bits > 0);

            u = bits;
            num = 0;
            if (bits > 1)
            {
                lsqrt = u >> 1;
                hsqrt = u - lsqrt;
                lsbits = (1 << lsqrt) - 1;
                _clusters = new vanEmdeBoasTreeNode<TData>[1 << hsqrt];
            }
        }

        public bool Contains(int key)
        {
            if (key == min || key == max)
                return true;
            else if (u == 1)
                return false;
            else
                return _clusters[key >> lsqrt]?.Contains(key & lsbits) ?? false;
        }
        public vanEmdeBoasTreeDataInfo<TData> Search(int key)
        {
            if (min == null || key < min || key > max)
                return vanEmdeBoasTreeDataInfo<TData>.Null;
            else if (key == min)
                return new vanEmdeBoasTreeDataInfo<TData>() { HitNode = this, HitMin = true };
            else if (u == 1)
                return new vanEmdeBoasTreeDataInfo<TData>() { HitNode = this, HitMin = false };
            else
                return _clusters[key >> lsqrt].Search(key & lsbits);
        }
        public vanEmdeBoasTreeDataInfo<TData> Create(int key, TData data, Func<int, vanEmdeBoasTreeNode<TData>> newNode)
        {
            Contract.Requires<ArgumentOutOfRangeException>(key >= 0 && key <= UniverseSize);
            Contract.Requires<ArgumentNullException>(newNode != null);

            vanEmdeBoasTreeDataInfo<TData> res;
            Create(key, data, out res, newNode);
            return res;
        }
        /// <summary>
        /// Insert a new node, if node with the key exists already, then update the data
        /// 插入新结点，若结点已存在则更新结点数据
        /// </summary>
        /// <param name="key">插入结点的关键字</param>
        /// <param name="data">需要插入的数据</param>
        /// <param name="info">返回插入的位置信息</param>
        /// <param name="newNode">创建新结点的方法， 参数是结点全域大小， 用以生成自定义的结点</param>
        /// <returns>如果关键字不存在则返回<c>true</c>，关键字已存在则返回<c>false</c></returns>
        public bool Create(int key, TData data, out vanEmdeBoasTreeDataInfo<TData> info, Func<int, vanEmdeBoasTreeNode<TData>> newNode)
        {
            Contract.Requires<ArgumentOutOfRangeException>(key >= 0 && key <= UniverseSize);
            Contract.Requires<ArgumentNullException>(newNode != null);

            bool updated = true;// update log, if node is new, then num will be updated.
            info = new vanEmdeBoasTreeDataInfo<TData>() { HitNode = this, HitMin = true };
            if (min == null)// the node is null
            {
                min = max = key;
                MinData = MaxData = data;
                num = 1;
            }
            else
            {
                bool replaced = false;
                if (key < min)// update min and insert min into sub-clusters 
                {
                    int temp = key; // swap data
                    key = min.Value;
                    min = temp;
                    TData tempdata = data;
                    data = MinData;
                    MinData = tempdata;
                    replaced = true;
                }
                else if (key == min) // update min
                {
                    MinData = data;
                    if (min == max) MaxData = data;
                    return false;
                }
                if (u > 1)
                {
                    int h = key >> lsqrt, l = key & lsbits;
                    if (_clusters[h] == null)
                        _clusters[h] = newNode(lsqrt);
                    if (_clusters[h].min == null) // sub-cluster is null 
                    {
                        Summary.Create(h, null, createsummary);
                        _clusters[h].min = _clusters[h].max = l;
                        _clusters[h].MinData = _clusters[h].MaxData = data;
                        _clusters[h].num = 1;
                        if (!replaced) info.HitNode = _clusters[h];
                    }
                    else
                    {
                        if (!replaced) updated = _clusters[h].Create(l, data, out info, newNode);
                        else _clusters[h].Create(l, data, newNode);
                    }
                }
                if (key > max)
                {
                    max = key;
                    MaxData = data;
                }
                else if (key == max) // update max
                {
                    MaxData = data;
                    if (!replaced) return false;
                }
                if (updated) num++;
            }
            return updated;
        }
        //移除val，移除前需判断是否包含 
        public vanEmdeBoasTreeDataInfo<TData> Remove(int key)
        {
            Contract.Requires<ArgumentOutOfRangeException>(key >= Min && key <= Max);

            var res = new vanEmdeBoasTreeDataInfo<TData>() { remove = true };
            if (min == max)// there's only one value in cluster 
            {
                res.HitNode = this;
                res.HitMin = true;
                res.rmdata = MinData;

                min = max = null;
                MinData = MaxData = default(TData);
            }
            else if (u == 1)// leaf node
            {
                res.HitMin = key == 0;
                res.HitNode = this;

                if (key == 0)
                {
                    min = 1;
                    res.rmdata = MinData;
                    MinData = MaxData;
                }
                else
                {
                    max = 0;
                    res.rmdata = MaxData;
                    MaxData = MinData;
                }
            }
            else
            {
                if (key == min)// key is the minimum 
                {
                    res.HitMin = true;
                    res.HitNode = this;
                    res.rmdata = MinData;

                    int first = _summary.min.Value;
                    key = first << lsqrt | _clusters[first].min.Value;// get the first node of first cluster
                    min = key;// update min and key, then delete min from sub-cluster
                    MinData = _clusters[first].MinData;
                    _clusters[key >> lsqrt].Remove(key & lsbits);
                }
                else
                    res = _clusters[key >> lsqrt].Remove(key & lsbits);
                int h = key >> lsqrt;
                if (_clusters[h].min == null)
                {
                    _summary.Remove(h); // delete cluster from summary 
                    if (key == max) // if max is deleted, then find previous cluster to update maximum. 
                    {
                        int? smax = _summary?.max;
                        if (smax == null)
                            max = min;
                        else
                        {
                            max = smax << lsqrt | _clusters[smax.Value].max;
                            MaxData = _clusters[smax.Value].MaxData;
                        }
                    }
                }
                else if (key == max)
                {
                    max = h << lsqrt | _clusters[h].max;
                    MaxData = _clusters[h].MaxData;
                }
            }
            num--;
            return res;
        }
        /// <summary>
        /// Find the smallest key bigger than <paramref name="key"/>
        /// 获取集合中比<paramref name="key"/>大的最小键值
        /// </summary>
        public int? Successor(int key)
        {
            if (u == 1)
                if (key == 0 && max == 1)
                    return 1;
                else
                    return null;
            else if (min != null && key < min)
                return min;
            else
            {
                int h = key >> lsqrt, l = key & lsbits;
                var maxl = _clusters[h]?.max;
                if (maxl != null && l < maxl)
                    return h << lsqrt | _clusters[h].Successor(l);
                else
                {
                    var succ = _summary?.Successor(h);//find next cluster
                    if (succ == null)
                        return null;
                    else
                        return succ << lsqrt | _clusters[succ.Value].min;
                }
            }
        }
        /// <summary>
        /// Find the biggest key smaller than <paramref name="key"/>
        /// 获取集合中比<paramref name="key"/>小的最大键值
        /// </summary>
        public int? Predecessor(int key)
        {
            if (u == 1)
                if (key == 1 && min == 0)
                    return 0;
                else
                    return null;
            else if (max != null && key > max)
                return max;
            else
            {
                int h = key >> lsqrt, l = key & lsbits;
                var minl = _clusters[h]?.min;
                if (minl != null && l > minl)
                    return h << lsqrt | _clusters[h].Predecessor(l);
                else
                {
                    var pred = _summary?.Predecessor(h);//find previous cluster
                    if (pred == null)
                        if (min != null && key > min)
                            return min;
                        else return null;
                    else
                        return pred << lsqrt | _clusters[pred.Value].max;
                }
            }
        }

        internal vanEmdeBoasTreeNode<TData> ExpandUp(Func<int, vanEmdeBoasTreeNode<TData>> newNode)
        {
            Contract.Requires<ArgumentNullException>(newNode != null);

            var newtop = newNode(u << 1);
            newtop.num = num;
            newtop.min = min.Value;
            newtop.MinData = Remove(newtop.min.Value).GetData();
            newtop.max = max.Value;
            newtop.MaxData = MaxData;
            newtop._clusters[0] = this;
            newtop.Summary.Create(0, null, createsummary);
            return newtop;
        }
        internal vanEmdeBoasTreeNode<TData> TrimDown(Func<int, vanEmdeBoasTreeNode<TData>> newNode)
        {
            Contract.Requires<ArgumentNullException>(newNode != null);

            if (u == 1)
                return this;
            if (num == 0) // null node
                return new vanEmdeBoasTreeNode<TData>(1);
            if (_summary?.min == null) // only 1 key
            {
                var newtop = new vanEmdeBoasTreeNode<TData>(Utils.Ceil2(min.Value));
                newtop.Create(min.Value, MinData, newNode);
                return newtop;
            }
            else if (_summary.min == 0 && _summary.max == 0) // only 1 cluster
            {
                var newtop = _clusters[0];
                _clusters[0] = null; // clear reference
                //SummaryNode.Remove(0);
                newtop.Create(min.Value, MinData, newNode);
                return newtop.TrimDown(newNode); // recursively trim
            }
            else if (lsqrt == hsqrt - 1 && u > 2) // right half clusters are null
            {
                int lcount = 1 << lsqrt, hcount = 1 << hsqrt;
                for (int i = lcount; i < hcount; i++)
                    if (_clusters[i] != null && _clusters[i].Count > 0)
                        return this;

                // trim higher half
                var newtop = newNode(u - 1);
                newtop.num = num;
                newtop.min = min;
                newtop.MinData = MinData;
                newtop.max = max;
                newtop.MaxData = MaxData;
                for (int i = 0; i < lcount; i++)
                    if (_clusters[i] != null && _clusters[i].Count > 0)
                    {
                        newtop._clusters[i] = _clusters[i];
                        _clusters[i] = null; // clear reference
                        newtop.Summary.Create(i, null, createsummary);
                    }
                return newtop;
            }
            else return this;
        }

        public override string ToString()
        {
            if (u > 1) return $"Min={MinData}";
            else return $"Min={MinData}, Max={MaxData}";
        }
    }

    /// <summary>
    /// A struct indicating a data position in vEB tree node
    /// 表示在vEB树结点的数据位的结构体
    /// </summary>
    public struct vanEmdeBoasTreeDataInfo<TData>
    {
        internal bool remove;
        internal TData rmdata;
        /// <summary>
        /// Node hit by operations
        /// 操作命中的结点
        /// </summary>
        public vanEmdeBoasTreeNode<TData> HitNode;
        /// <summary>
        /// If the data is the MinData
        /// 操作是否命中的是最小的结点
        /// </summary>
        public bool HitMin;
        /// <summary>
        /// Get the data in the position described by the node.
        /// 获取结构体表示的数据位的数据
        /// </summary>
        /// <returns></returns>
        public TData GetData() => remove ? rmdata : (HitMin ? HitNode.MinData : HitNode.MaxData);

        public static vanEmdeBoasTreeDataInfo<TData> Null => default(vanEmdeBoasTreeDataInfo<TData>);
    }
}
