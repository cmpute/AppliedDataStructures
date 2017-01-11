using System;
using System.Collections.Advanced;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 封装一个方法，该方法只有一个引用参数并且不返回值
    /// </summary>
    /// <typeparam name="T">此委托封装的方法的类型参数</typeparam>
    /// <param name="target">此委托封装的方法的参数</param>
    public delegate void ActionRef<T>(ref T target);
}
namespace System.Collections.Generic
{
    /// <summary>
    /// A list class that support fast bulk/range operations. e.g. reverse, range edit, cyclic offset
    /// 支持区间操作的列表
    /// </summary>
    public class RangeList<T> : IList<T>
    {
        SplayRangeTree<T> tree;

        public RangeList() : this(null) { }
        public RangeList(IEnumerable<T> initData)
        {
            tree = new SplayRangeTree<T>(initData);
        }

        #region IList Implementation

        public T this[int index]
        {
            get
            {
                return tree.IndexSearch(index + 1)._data;
            }

            set
            {
                tree.IndexSearch(index + 1)._data = value;
            }
        }

        public int Count => tree.Count;

        public bool IsReadOnly => false;

        public void Add(T item) => tree.Insert(new[] { item }, Count);

        public void Clear() => tree.Clear();

        public bool Contains(T item) => IndexOf(item) >= 0;

        public void CopyTo(T[] array, int arrayIndex)
        {
            int current = arrayIndex;
            var iter = GetEnumerator();
            while (iter.MoveNext())
                array[current++] = iter.Current;
        }

        public IEnumerator<T> GetEnumerator()
        {
            List<T> temp = new List<T>(Count);
            for (int i = 0; i < Count; i++)
                temp.Add(this[i]);
            return temp.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(T item)
        {
            int count = 0;
            if ((Object)item == null)
            {
                foreach (var node in tree.Skip(1).Take(tree.Count))
                    if ((Object)(node._data) == null)
                        return count;
                    else
                        count++;
                return -1;
            }
            else
            {
                EqualityComparer<T> c = EqualityComparer<T>.Default;

                foreach (var node in tree.Skip(1).Take(tree.Count))
                    if (c.Equals(node._data, item))
                        return count;
                    else
                        count++;

                return -1;
            }
        }

        public void Insert(int index, T item) => tree.Insert(new[] { item }, index);

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        public void RemoveAt(int index) => tree.Delete(index, index);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        #endregion

        #region Range Operations
        public void AddRange(IEnumerable<T> data) => InsertRange(Count, data);
        public void InsertRange(int index, IEnumerable<T> data) => tree.Insert(data, index);
        public T[] RemoveRange(int index, int count) => tree.Delete(index, index + count - 1);
        public void OperateRange(int index, int count, ActionRef<T> operation) => tree.Operate(operation, index, index + count - 1);
        public void Reverse() => Reverse(0, Count);
        public void Reverse(int index, int count) => tree.Reverse(index, index + count - 1);
        #endregion
    }

    /// <summary>
    /// a tree supporting index operations
    /// 支持索引操作的树
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    class SplayRangeTree<T> : SplayTree<SplayRangeTreeNode<T>, int>
    {
        readonly SplayRangeTreeNode<T> lnil, rnil;
        public SplayRangeTree(IEnumerable<T> init) : base()
        {
            SupportEquatable = false;
            var p = Root = lnil = new SplayRangeTreeNode<T>(default(T)); // left trailor
            if (init != null)
                foreach (var item in init)
                {
                    p.RightChild = new SplayRangeTreeNode<T>(item);
                    p = p.RightChild;
                }
            p.RightChild = rnil = new SplayRangeTreeNode<T>(default(T)); // right trailor
            Splay(p);
        }

        public SplayRangeTreeNode<T> IndexSearch(int index) // index start from 1.
        {
            SplayRangeTreeNode<T> p = Root;
            p.SearchDown();
            while (index != p.LeftChild.subcount)
            {
                if (index < p.LeftChild.subcount)
                    p = p.LeftChild;
                else
                {
                    index -= p.LeftChild.subcount + 1;
                    p = p.RightChild;
                }
                p.SearchDown();
            }
            return p;
        }
        public void SelectSegment(int startindex, int endindex)
        {
            Splay(IndexSearch(startindex));
            Splay(IndexSearch(endindex), Root);
        }

        public void Insert(IEnumerable<T> data, int startindex)
        {
            if (data == null) return;
            if (startindex > Count) throw new ArgumentOutOfRangeException("插入位置不正确");

            SelectSegment(startindex, startindex + 1);

            var p = Root.RightChild.LeftChild;
            var iter = data.GetEnumerator();
            if (p == SplayRangeTreeNode<T>.nil)
                if (iter.MoveNext())
                {
                    p = Root.RightChild.LeftChild = new SplayRangeTreeNode<T>(iter.Current);
                    Count++;
                }
                else
                    return;
            while (iter.MoveNext())
            {
                p.RightChild = new SplayRangeTreeNode<T>(iter.Current);
                p = p.RightChild;
                Count++;
            }

            Splay(p);
        }
        public T[] Delete(int startindex, int endindex)
        {
            if (endindex < startindex) throw new ArgumentException("操作范围不正确");
            if (endindex >= Count) throw new ArgumentOutOfRangeException("操作范围不正确");

            // Splay segment
            SelectSegment(startindex, endindex + 2);

            // Remove nodes
            var res = Root.RightChild.LeftChild.GetSubtreeEnumerator(TraversalOrder.InOrder);
            Root.RightChild.LeftChild = SplayRangeTreeNode<T>.nil;

            // Update
            Splay(Root.RightChild);
            Count -= endindex - startindex + 1;

            // Report nodes
            List<T> ret = new List<T>();
            while (res.MoveNext())
                ret.Add(res.Current._data);
            return ret.ToArray();

        }
        public void Reverse(int startindex, int endindex)
        {
            if (endindex < startindex) throw new InvalidOperationException("操作范围不正确");
            else if (endindex == startindex) return;
            if (endindex >= Count) throw new ArgumentOutOfRangeException("操作范围不正确");

            // Splay segment
            SelectSegment(startindex, endindex + 2);

            // Set flag
            Root.RightChild.LeftChild._rev ^= true;

            // Update
            Splay(Root.RightChild.LeftChild);
        }
        public void Operate(ActionRef<T> optr, int startindex, int endindex)
        {
            if (endindex < startindex) throw new InvalidOperationException("操作范围不正确");
            if (endindex >= Count) throw new ArgumentOutOfRangeException("操作范围不正确");

            // Splay segment
            SelectSegment(startindex, endindex + 2);

            // Set flag
            Root.RightChild.LeftChild._operations += optr;

            // Update
            Splay(Root.RightChild.LeftChild);
        }
        public override void Clear()
        {
            if (Count != 0) Delete(0, Count - 1);
        }
    }

    class SplayRangeTreeNode<T> : BinaryTreeNode, IKeyedNode<int>
    {
        internal static readonly SplayRangeTreeNode<T> nil = new SplayRangeTreeNode<T>(default(T)) { subcount = 0 };

        internal ActionRef<T> _operations;
        internal bool _rev = false; // reverse flag
        internal int subcount = 1;
        internal T _data;

        /// <remarks>
        /// The tree is indeed a search tree which the index in the list is the key, but the index cannot be retrieved directly from the field "subcount"
        /// 这的确是一棵以元素在列表中位置为关键字的搜索树，但这个关键字不能从subcount变量中直接得到
        /// </remarks>
        public int Key//=> subcount;
        {
            get { throw new NotSupportedException(); }
        }

        public SplayRangeTreeNode(T data)
        {
            _data = data;
            _par = _lchild = _rchild = nil;
        }

        public new SplayRangeTreeNode<T> LeftChild
        {
            get { return base.LeftChild as SplayRangeTreeNode<T>; }
            set { base.LeftChild = value; }
        }
        public new SplayRangeTreeNode<T> RightChild
        {
            get { return base.RightChild as SplayRangeTreeNode<T>; }
            set { base.RightChild = value; }
        }

        protected override void OnSearchDown()
        {
            if (Equals(nil)) return;
            if (_rev)
            {
                var temp = LeftChild;
                LeftChild = RightChild;
                RightChild = temp;
                LeftChild._rev ^= _rev;
                RightChild._rev ^= _rev;
                _rev = false;
            }
            if (_operations != null)
            {
                LeftChild._operations += _operations;
                RightChild._operations += _operations;
                _operations.Invoke(ref _data);
                _operations = null;
            }
        }
        protected override void OnSearchUp()
        {
            base.OnSearchUp();
            if (Equals(nil)) return;
            subcount = LeftChild.subcount + RightChild.subcount + 1;
        }

        protected override bool IsNil()
        {
            return ReferenceEquals(this, nil) || base.IsNil();
        }
    }
}
