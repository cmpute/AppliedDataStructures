using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class vanEmdeBoasTreeNode<TData> : IMultiwayTreeNode
    {
        readonly int u;
        internal readonly int lsqrt, hsqrt;// lowbit/highbit divider
        int num;
        vanEmdeBoasTreeNode<TData>[] _clusters;
        vanEmdeBoasTreeNode<TData> _summary;

        public int? Min { get; protected set; }
        public int? Max { get; protected set; }
        public TData MinData { get; protected set; }
        public TData MaxData { get; protected set; }
        /// <summary>
        /// size of clusters stored in this node, also known as 'universal set size' of the node.
        /// 结点存储的簇的规模，也就是所谓“全集”的大小
        /// </summary>
        public int UniverseSize => u;
        public vanEmdeBoasTreeNode<TData>[] Clusters => _clusters;
        /// <summary>
        /// count of non-null elements
        /// 不为空的元素的个数
        /// </summary>
        public int Count => num;
        public vanEmdeBoasTreeNode<TData> SummaryNode
        {
            get
            {
                if (_summary == null) _summary = new vanEmdeBoasTreeNode<TData>(hsqrt);
                return _summary;
            }
        }
        /// <summary>
        /// this property returns <see cref="Clusters"/>
        /// </summary>
        public IReadOnlyList<IMultiwayTreeNode> Children => Clusters;

        public vanEmdeBoasTreeNode(int size)
        {
            u = size;
            num = 0;
            if (size > 2)
            {
                hsqrt = Utils.Ceil2(Math.Sqrt(u));
                lsqrt = u / hsqrt;
                _clusters = new vanEmdeBoasTreeNode<TData>[hsqrt];
            }
        }

        public bool Contains(int key)
        {
            if (key == Min || key == Max)
                return true;
            else if (u == 2)
                return false;
            else
                return _clusters[key / lsqrt]?.Contains(key % lsqrt) ?? false;
        }
        public vanEmdeBoasTreeDataInfo<TData> Search(int key)
        {
            if (Min == null || key < Min || key > Max)
                return vanEmdeBoasTreeDataInfo<TData>.Null;
            else if (key == Min)
                return new vanEmdeBoasTreeDataInfo<TData>() { HitNode = this, HitMin = true };
            else if (u == 2)
                return new vanEmdeBoasTreeDataInfo<TData>() { HitNode = this, HitMin = false };
            else
                return _clusters[key / lsqrt].Search(key % lsqrt);
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
                if (u > 2)
                {
                    int h = key / lsqrt, l = key % lsqrt;
                    if (_clusters[h] == null)
                        _clusters[h] = newNode(hsqrt);
                    if (_clusters[h].Min == null) // sub-cluster is null 
                    {
                        SummaryNode.Create(h, data, newNode);
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
                    if(!replaced) return false;
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
                num = 0;
            }
            else if (u == 2)// leaf node
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

                    int first = SummaryNode.Min.Value;
                    key = first * lsqrt + _clusters[first].Min.Value;// get the first node of first cluster
                    Min = key;// update min and key, then delete min from sub-cluster
                    MinData = _clusters[first].MinData;
                    _clusters[key / lsqrt].Remove(key % lsqrt);
                }
                else
                    res = _clusters[key / lsqrt].Remove(key % lsqrt);
                int h = key / lsqrt;
                if (_clusters[h].Min == null)
                {
                    SummaryNode.Remove(h); // delete cluster from summary 
                    if (key == Max) // if max is deleted, then find previous cluster to update maximum. 
                    {
                        int? smax = SummaryNode.Max;
                        if (smax == null)
                            Max = Min;
                        else
                        {
                            Max = smax * lsqrt + _clusters[smax.Value].Max;
                            MaxData = _clusters[smax.Value].MaxData;
                        }
                    }
                }
                else if (key == Max)
                {
                    Max = h * lsqrt + _clusters[h].Max;
                    MaxData = _clusters[h].MaxData;
                }
            }
            num--;
            return res;
        }
        public int? Successor(int key)
        {
            if (u == 2)
                if (key == 0 && Max == 1)
                    return 1;
                else
                    return null;
            else if (Min != null && key < Min)
                return Min;
            else
            {
                int h = key / lsqrt, l = key % lsqrt;
                var maxl = _clusters[key / lsqrt]?.Max;
                if (maxl != null && l < maxl)
                    return h * lsqrt + _clusters[h].Successor(l);
                else
                {
                    var succ = SummaryNode?.Successor(h);//find next cluster
                    if (succ == null)
                        return null;
                    else
                        return succ * lsqrt + _clusters[succ.Value].Min;
                }
            }
        }
        public int? Predecessor(int key)
        {
            if (u == 2)
                if (key == 1 && Min == 0)
                    return 0;
                else
                    return null;
            else if (Max != null && key > Max)
                return Max;
            else
            {
                int h = key / lsqrt, l = key % lsqrt;
                var minl = _clusters[key / lsqrt]?.Min;
                if (minl != null && l > minl)
                    return h * lsqrt + _clusters[h].Predecessor(l);
                else
                {
                    var pred = SummaryNode?.Predecessor(h);//find previous cluster
                    if (pred == null)
                        if (Min != null && key > Min)
                            return Min;
                        else return null;
                    else
                        return pred * lsqrt + _clusters[pred.Value].Max;
                }
            }
        }

        public override string ToString()
        {
            if (u > 2) return $"Min={MinData}";
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
