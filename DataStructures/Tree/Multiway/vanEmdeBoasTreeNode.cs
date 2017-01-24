using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
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
        readonly int lsqrt, hsqrt, lsbits;// lowbit/highbit divider
        int num;
        vanEmdeBoasTreeNode<TData>[] _clusters;
        vanEmdeBoasTreeNode<object> _summary; // summary doesn't need data
        static readonly Func<int, vanEmdeBoasTreeNode<object>> createsummary
            = (size) => new vanEmdeBoasTreeNode<object>(size);

        public int? Min { get; protected set; }
        public int? Max { get; protected set; }
        public TData MinData { get; protected set; }
        public TData MaxData { get; protected set; }
        /// <summary>
        /// size of clusters stored in this node, also known as 'universal set size' of the node.
        /// 结点存储的簇的规模，也就是所谓“全集”的大小
        /// </summary>
        public int UniverseSize => 1 << u;
        public int UniverseBits => u;
        public vanEmdeBoasTreeNode<TData>[] Clusters => _clusters;
        /// <summary>
        /// count of non-null elements
        /// 不为空的元素的个数
        /// </summary>
        public int Count => num;
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
        public IReadOnlyList<IMultiwayTreeNode> Children => Clusters;

        /// <summary>
        /// 新建一个vEB树的节点
        /// </summary>
        /// <param name="bits">size必须是2的幂，尽管代码中没有进行检查，但如果不是2的幂则会运行出错</param>
        public vanEmdeBoasTreeNode(int bits)
        {

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
            if (key == Min || key == Max)
                return true;
            else if (u == 1)
                return false;
            else
                return _clusters[key >> lsqrt]?.Contains(key & lsbits) ?? false;
        }
        public vanEmdeBoasTreeDataInfo<TData> Search(int key)
        {
            if (Min == null || key < Min || key > Max)
                return vanEmdeBoasTreeDataInfo<TData>.Null;
            else if (key == Min)
                return new vanEmdeBoasTreeDataInfo<TData>() { HitNode = this, HitMin = true };
            else if (u == 1)
                return new vanEmdeBoasTreeDataInfo<TData>() { HitNode = this, HitMin = false };
            else
                return _clusters[key >> lsqrt].Search(key & lsbits);
        }
        public vanEmdeBoasTreeDataInfo<TData> Create(int key, TData data, Func<int, vanEmdeBoasTreeNode<TData>> newNode)
        {
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
        /// <param name="newNode">创建新结点的方法</param>
        /// <returns>如果关键字不存在则返回<c>true</c>，关键字已存在则返回<c>false</c></returns>
        public bool Create(int key, TData data, out vanEmdeBoasTreeDataInfo<TData> info, Func<int, vanEmdeBoasTreeNode<TData>> newNode)
        {
            bool updated = true;// update log, if node is new, then num will be updated.
            info = new vanEmdeBoasTreeDataInfo<TData>() { HitNode = this, HitMin = true };
            if (Min == null)// the node is null
            {
                Min = Max = key;
                MinData = MaxData = data;
                num = 1;
            }
            else
            {
                bool replaced = false;
                if (key < Min)// update min and insert min into sub-clusters 
                {
                    int temp = key; // swap data
                    key = Min.Value;
                    Min = temp;
                    TData tempdata = data;
                    data = MinData;
                    MinData = tempdata;
                    replaced = true;
                }
                else if (key == Min) // update min
                {
                    MinData = data;
                    if (Min == Max) MaxData = data;
                    return false;
                }
                if (u > 1)
                {
                    int h = key >> lsqrt, l = key & lsbits;
                    if (_clusters[h] == null)
                        _clusters[h] = newNode(lsqrt);
                    if (_clusters[h].Min == null) // sub-cluster is null 
                    {
                        Summary.Create(h, null, createsummary);
                        _clusters[h].Min = _clusters[h].Max = l;
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
                if (key > Max)
                {
                    Max = key;
                    MaxData = data;
                }
                else if (key == Max) // update max
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
            var res = new vanEmdeBoasTreeDataInfo<TData>() { remove = true };
            if (Min == Max)// there's only one value in cluster 
            {
                res.HitNode = this;
                res.HitMin = true;
                res.rmdata = MinData;

                Min = Max = null;
                MinData = MaxData = default(TData);
            }
            else if (u == 1)// leaf node
            {
                res.HitMin = key == 0;
                res.HitNode = this;

                if (key == 0)
                {
                    Min = 1;
                    res.rmdata = MinData;
                    MinData = MaxData;
                }
                else
                {
                    Max = 0;
                    res.rmdata = MaxData;
                    MaxData = MinData;
                }
            }
            else
            {
                if (key == Min)// key is the minimum 
                {
                    res.HitMin = true;
                    res.HitNode = this;
                    res.rmdata = MinData;

                    int first = _summary.Min.Value;
                    key = first << lsqrt | _clusters[first].Min.Value;// get the first node of first cluster
                    Min = key;// update min and key, then delete min from sub-cluster
                    MinData = _clusters[first].MinData;
                    _clusters[key >> lsqrt].Remove(key & lsbits);
                }
                else
                    res = _clusters[key >> lsqrt].Remove(key & lsbits);
                int h = key >> lsqrt;
                if (_clusters[h].Min == null)
                {
                    _summary.Remove(h); // delete cluster from summary 
                    if (key == Max) // if max is deleted, then find previous cluster to update maximum. 
                    {
                        int? smax = _summary?.Max;
                        if (smax == null)
                            Max = Min;
                        else
                        {
                            Max = smax << lsqrt | _clusters[smax.Value].Max;
                            MaxData = _clusters[smax.Value].MaxData;
                        }
                    }
                }
                else if (key == Max)
                {
                    Max = h << lsqrt | _clusters[h].Max;
                    MaxData = _clusters[h].MaxData;
                }
            }
            num--;
            return res;
        }
        public int? Successor(int key)
        {
            if (u == 1)
                if (key == 0 && Max == 1)
                    return 1;
                else
                    return null;
            else if (Min != null && key < Min)
                return Min;
            else
            {
                int h = key >> lsqrt, l = key & lsbits;
                var maxl = _clusters[h]?.Max;
                if (maxl != null && l < maxl)
                    return h << lsqrt | _clusters[h].Successor(l);
                else
                {
                    var succ = _summary?.Successor(h);//find next cluster
                    if (succ == null)
                        return null;
                    else
                        return succ << lsqrt | _clusters[succ.Value].Min;
                }
            }
        }
        public int? Predecessor(int key)
        {
            if (u == 1)
                if (key == 1 && Min == 0)
                    return 0;
                else
                    return null;
            else if (Max != null && key > Max)
                return Max;
            else
            {
                int h = key >> lsqrt, l = key & lsbits;
                var minl = _clusters[h]?.Min;
                if (minl != null && l > minl)
                    return h << lsqrt | _clusters[h].Predecessor(l);
                else
                {
                    var pred = _summary?.Predecessor(h);//find previous cluster
                    if (pred == null)
                        if (Min != null && key > Min)
                            return Min;
                        else return null;
                    else
                        return pred << lsqrt | _clusters[pred.Value].Max;
                }
            }
        }

        internal vanEmdeBoasTreeNode<TData> ExpandUp(Func<int, vanEmdeBoasTreeNode<TData>> newNode)
        {
            var newtop = newNode(u << 1);
            newtop.num = num;
            newtop.Min = Min.Value;
            newtop.MinData = Remove(newtop.Min.Value).GetData();
            newtop.Max = Max.Value;
            newtop.MaxData = MaxData;
            newtop._clusters[0] = this;
            newtop.Summary.Create(0, null, createsummary);
            return newtop;
        }
        internal vanEmdeBoasTreeNode<TData> TrimDown(Func<int, vanEmdeBoasTreeNode<TData>> newNode)
        {
            if (u == 1)
                return this;
            if (num == 0) // null node
                return new vanEmdeBoasTreeNode<TData>(1);
            if (_summary?.Min == null) // only 1 key
            {
                var newtop = new vanEmdeBoasTreeNode<TData>(1);
                newtop.Create(Min.Value, MinData, newNode);
                return newtop;
            }
            else if (_summary.Min == 0 && _summary.Max == 0) // only 1 cluster
            {
                var newtop = _clusters[0];
                _clusters[0] = null; // clear reference
                //SummaryNode.Remove(0);
                newtop.Create(Min.Value, MinData, newNode);
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
                newtop.Min = Min;
                newtop.MinData = MinData;
                newtop.Max = Max;
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
        public vanEmdeBoasTreeNode<TData> HitNode;
        public bool HitMin;
        public TData GetData() => remove ? rmdata : (HitMin ? HitNode.MinData : HitNode.MaxData);

        public static vanEmdeBoasTreeDataInfo<TData> Null => default(vanEmdeBoasTreeDataInfo<TData>);
    }
}
