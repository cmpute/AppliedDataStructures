using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class vanEmdeBoasTreeNode<TData> : IMultiwayTreeNode
    {
        readonly int u, lsqrt, hsqrt;// lowbit/highbit divider
        int num;
        List<vanEmdeBoasTreeNode<TData>> _clusters;
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
        public List<vanEmdeBoasTreeNode<TData>> Clusters => _clusters;
        /// <summary>
        /// count of non-null clusters
        /// 不为空的子簇的个数
        /// </summary>
        public int ClusterCount => num;
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

        internal vanEmdeBoasTreeNode(int size)
        {
            u = size;
            num = 0;
            if(size>2)
            {
                lsqrt = Ceil2(Math.Sqrt(u));
                hsqrt = u / hsqrt;
                _clusters = new List<vanEmdeBoasTreeNode<TData>>(hsqrt);
            }
        }

        //2^ceil(log2(n))
        private static int Ceil2(double n)
        {
            int a = 1, c = 0;
            while (a < n)
            {
                a <<= 1;
                c++;
            }
            return 1 << c;
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
                return default(vanEmdeBoasTreeDataInfo<TData>);
            else if (key == Min)
                return new vanEmdeBoasTreeDataInfo<TData>() { HitNode = this, HitMin = true };
            else if (u == 2)
                return new vanEmdeBoasTreeDataInfo<TData>() { HitNode = this, HitMin = true };
            else
                return _clusters[key / lsqrt].Search(key % lsqrt);
        }
        public vanEmdeBoasTreeDataInfo<TData> Create(int key, TData data, Func<int, vanEmdeBoasTreeNode<TData>> newNode)
        {
            vanEmdeBoasTreeDataInfo<TData> res;
            Create(key, data, out res, newNode);
            return res;
        }
        public bool Create(int key, TData data, out vanEmdeBoasTreeDataInfo<TData> info, Func<int, vanEmdeBoasTreeNode<TData>> newNode)
        {
            bool updated = true;// update log, if node is updated, then num will be updated.
            info = new vanEmdeBoasTreeDataInfo<TData>() { HitNode = this, HitMin = true };
            if (Min == null)// the node is null
            {
                Min = Max = key;
                MinData = MaxData = data;
                num = 1;
                updated = true;
            }
            else
            {
                if (key < Min)// update min and insert min into sub-clusters 
                {
                    int temp = key; // swap data
                    key = Min.Value;
                    Min = temp;
                    TData tempdata = data;
                    data = MinData;
                    MinData = tempdata;
                    updated = true;
                }
                if (key == Min)
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
                        info.HitNode = _clusters[h];
                        updated = true;
                    }
                    else
                        updated = _clusters[h].Create(l, data, out info, newNode);
                }
                if (key > Max)
                {
                    Max = key;
                    MaxData = data;
                    updated = true;
                }
                if (updated) num++;
            }
            return updated;
        }

        //移除val，移除前需判断是否包含 
        vanEmdeBoasTreeDataInfo<TData> Remove(int key)
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
                int h = key / lsqrt;
                if (key == Min)// key is the minimum 
                {
                    int first = SummaryNode.Min.Value;
                    key = first * lsqrt + _clusters[first].Min.Value;// get the first node of first cluster
                    res.HitMin = true;
                    res.HitNode = this;
                    res.rmdata = MinData;
                    Min = key;// update min and key, then delete min from sub-cluster
                    MinData = _clusters[first].MinData;
                    _clusters[h].Remove(key % lsqrt);
                }
                else
                    res = _clusters[h].Remove(key % lsqrt);
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
    }
}
