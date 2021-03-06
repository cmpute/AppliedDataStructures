﻿using System;
using System.Collections.Generic;
using System.Collections.Advanced.Tree;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract = System.Diagnostics.Contracts.Contract;

namespace System.Collections.Advanced.Linear
{
    /// <summary>
    /// A list class that support fast bulk/range operations. e.g. reverse, range edit, cyclic offset
    /// 支持区间操作的列表
    /// </summary>
    public class SplayList<T> : IRangeList<T>
    {
        SplayRangeTree<T> tree;

        public SplayList() : this(null) { }
        public SplayList(IEnumerable<T> initData)
        {
            tree = new SplayRangeTree<T>(initData);
        }

        #region IList Implementation

        public T this[int index]
        {
            get
            {
                Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index < Count);
                return tree.IndexSearch(index + 1)._data;
            }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index < Count);
                tree.IndexSearch(index + 1)._data = value;
            }
        }

        public int Count => tree.Count;

        public bool IsReadOnly => false;

        public void Add(T item) => tree.Insert(new[] { item }, Count);

        public void Clear() => tree.Clear();

        public bool Contains(T item) => IndexOf(item) >= 0;

        public void CopyTo(T[] array, int arrayIndex) => this.CopyTo<T>(array, arrayIndex);

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
            Contract.Ensures(Contract.Result<int>() >= -1);
            Contract.Ensures(Contract.Result<int>() < Count);

            return this.IndexOf<T>(item);
        }

        public void Insert(int index, T item)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index <= Count);
            tree.Insert(new[] { item }, index);
        }

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

        public void RemoveAt(int index)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index <= Count);
            tree.Delete(index, index);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        #endregion

        #region Range Operations
        public void AddRange(IEnumerable<T> data) => InsertRange(Count, data);
        public void InsertRange(int index, IEnumerable<T> data)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index <= Count);
            tree.Insert(data, index);
        }
        public IList<T> GetRange(int index, int count)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(count >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(index + count <= Count);
            Contract.Ensures(Contract.Result<IList<T>>() != null);
            Contract.EndContractBlock();

            if (count > 0) return tree.Report(index, index + count - 1);
            else return new List<T>();
        }
        public void RemoveRange(int index, int count)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(count >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(index + count <= Count);
            Contract.EndContractBlock();

            if (count > 0)
                tree.Delete(index, index + count - 1);
        }
        public void OperateRange(int index, int count, ActionRef<T> operation)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(count >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(index + count <= Count);
            Contract.EndContractBlock();

            if (count > 0)
                tree.Operate(operation, index, index + count - 1);
        }
        public void Reverse() => Reverse(0, Count);
        public void Reverse(int index, int count)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(count >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(index + count <= Count);
            Contract.EndContractBlock();

            if (count > 0)
                tree.Reverse(index, index + count - 1);
        }
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
            var p = Root = lnil = new SplayRangeTreeNode<T>(default(T)); // left trailer
            if (init != null)
                foreach (var item in init)
                {
                    p.RightChild = new SplayRangeTreeNode<T>(item);
                    p = p.RightChild;
                }
            p.RightChild = rnil = new SplayRangeTreeNode<T>(default(T)); // right trailer
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
        public void Delete(int startindex, int endindex)
        {

            // Splay segment
            SelectSegment(startindex, endindex + 2);

            // Remove subtree
            Root.RightChild.LeftChild = SplayRangeTreeNode<T>.nil;

            // Update
            Splay(Root.RightChild);
            Count -= endindex - startindex + 1;
        }
        public void Reverse(int startindex, int endindex)
        {

            if (endindex == startindex) return;

            // Splay segment
            SelectSegment(startindex, endindex + 2);

            // Set flag
            Root.RightChild.LeftChild._rev ^= true;

            // Update
            Splay(Root.RightChild.LeftChild);
        }
        public List<T> Report(int startindex, int endindex)
        {
            // Splay segment
            SelectSegment(startindex, endindex + 2);

            // Remove nodes
            var res = Root.RightChild.LeftChild.GetSubtreeEnumerator(TraverseOrder.InOrder);

            List<T> ret = new List<T>();
            while (res.MoveNext())
                ret.Add(res.Current._data);
            return ret;
        }
        public void Operate(ActionRef<T> optr, int startindex, int endindex)
        {

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

    class SplayRangeTreeNode<T> : BinaryTreeNode, IKeyProvider<int>
    {
        internal static readonly SplayRangeTreeNode<T> nil = new SplayRangeTreeNode<T>(default(T)) { subcount = 0 };

        internal ActionRef<T> _operations;
        internal bool _rev = false; // reverse flag
        internal int subcount = 1;
        internal T _data;

        /// <remarks>
        /// The tree is indeed a search tree which the index in the list is the key, but the index cannot be retrieved directly from the field "subcount"
        /// 这的确是一棵以元素在列表中位置为关键字的搜索树，但这个关键字不能从subcount变量中直接得到
        /// The tree is a search tree which the index in the list is the key
        /// 这是一棵以元素在列表中位置为关键字的搜索树
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

        public override bool IsSentinel()
        {
            return ReferenceEquals(this, nil) || base.IsSentinel();
        }
    }

}
