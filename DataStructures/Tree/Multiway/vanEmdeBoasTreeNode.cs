using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class vanEmdeBoasTreeNode : IMultiwayTreeNode
    {
        int u, num, 
            lsqrt, hsqrt;// lowbit/highbit divider
        List<vanEmdeBoasTreeNode> _clusters;
        vanEmdeBoasTreeNode _summary;

        public int? Min { get; protected set; }
        public int? Max { get; protected set; }
        /// <summary>
        /// size of clusters stored in this node, also known as 'universal set size' of the node.
        /// 结点存储的簇的规模，也就是所谓“全集”的大小
        /// </summary>
        public int Size => u;
        public List<vanEmdeBoasTreeNode> Clusters => _clusters;
        /// <summary>
        /// count of non-null clusters
        /// 不为空的子簇的个数
        /// </summary>
        public int ClusterCount => num;
        public vanEmdeBoasTreeNode SummaryNode
        {
            get
            {
                if (_summary == null) _summary = new vanEmdeBoasTreeNode(hsqrt);
                return _summary;
            }
        }

        /// <summary>
        /// this property returns <see cref="Clusters"/>
        /// </summary>
        public IReadOnlyList<IMultiwayTreeNode> Children => _clusters;

        public vanEmdeBoasTreeNode(int size)
        {
            u = size;
            num = 0;
            if(size>2)
            {
                lsqrt = Ceil2(Math.Sqrt(u));
                hsqrt = u / hsqrt;
                _clusters = new List<vanEmdeBoasTreeNode>(hsqrt);
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

        bool Contains(int key)
        {
            if (key == Min || key == Max)
                return true;
            else if (u == 2)
                return false;
            else
                return _clusters[key / lsqrt]?.Contains(key % lsqrt) ?? false;
        }

        vanEmdeBoasTreeNode Search(int key)
        {
            if (Min == null || key < Min || key > Max)
                return null;
            else if (key == Min)
                return this;
            else if (u == 2)
                return false;
            else
                return _clusters[key / lsqrt]?.Contains(key % lsqrt) ?? false;
        }

        bool Insert(int key, out vanEmdeBoasTreeNode hitnode)
        {
            bool updated = true;//记录是否产生了更新，用来判断是否更新num 
            hitnode = null;
            if (Min == null)//当前簇为空 
            {
                Min = key;
                Max = key;
                num = 1;
                updated = true;
                hitnode = this;
            }
            else
            {
                if (key < Min)//更新min的同时要把min当作新值插入子簇 
                {
                    int temp = key;
                    key = Min.Value;
                    Min = temp;
                    updated = true;
                    hitnode = this;
                }
                if (key == Min)
                {
                    hitnode = this;
                    return false;
                }
                if (u > 2)
                {
                    int h = key / lsqrt, l = key % lsqrt;
                    if (_clusters[h] == null)
                        _clusters[h] = new vanEmdeBoasTreeNode(hsqrt);
                    if (_clusters[h].Min == null)//对应子簇为空 
                    {
                        vanEmdeBoasTreeNode temp;
                        SummaryNode.Insert(h,out temp);
                        _clusters[h].Min = l;
                        _clusters[h].Max = l;
                        _clusters[h].num = l;
                        hitnode = _clusters[h];
                        updated = true;
                    }
                    else
                        updated = _clusters[h].Insert(l, out hitnode);
                }
                if (key > Max)
                {
                    Max = key;
                    updated = true;
                }
                if (updated) num++;
            }
            return updated;
        }

        //移除val，移除前需判断是否包含 
        vanEmdeBoasTreeNode Remove(int key)
        {
            vanEmdeBoasTreeNode res;
            if (Min == Max)//簇只有一个值 
            {
                Min = null;
                Max = null;
                res = this;
            }
            else if (u == 2)//叶子节点 
            {
                if (key == 0)
                    Min = 1;
                else
                    Min = 0;
                Max = Min;
                res = this;
            }
            else
            {
                if (key == Min)//val为最小值 
                {
                    int first = SummaryNode.Min.Value;
                    key = first * lsqrt + _clusters[first].Min.Value;//获取第一子簇的第一个节点 
                    Min = key;//更新min的同时更新val，之后把min从子簇中删除 
                }
                int h = key / lsqrt;
                res = _clusters[h].Remove(key % lsqrt);
                if (_clusters[h].Min == null)
                {
                    SummaryNode.Remove(h); //在summary中删除该簇 
                    if (key == Max) //如果删除的是最大值，则需要从summary中寻找上一簇更新最大值 
                    {
                        int? smax = SummaryNode.Max;
                        if (smax == null)
                            Max = Min;
                        else
                            Max = smax * lsqrt + _clusters[smax.Value].Max;
                    }
                }
                else if (key == Max)
                    Max = h * lsqrt + _clusters[h].Max;
            }
            num--;
            return res;
        }
    }
}
