﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// van Emde Boas Tree (a.k.a vEB-tree)
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <remarks>
    /// A Search Tree works based on comparison, but vEB tree works on bits, so vEB is technically not a search tree.
    /// </remarks>
    public class vanEmdeBoasTree<TNode, TData> : IRootedTree<TNode> where TNode : vanEmdeBoasTreeNode<TData>
    {
        private const int _defaultCapacity = 4;

        TNode _root = null;
        Func<int, TNode> _new;
        public int Count => Root?.Count ?? 0;
        public int Capacity => Root?.UniverseSize ?? _defaultCapacity;

        public TNode Root { get { return _root; } }
        public vanEmdeBoasTree(Func<int, TNode> newNode) : this(_defaultCapacity, newNode) { }
        public vanEmdeBoasTree(int initsize, Func<int, TNode> newNode)
        {
            _new = newNode;
            _root = _new(Utils.Ceil2(initsize));
        }

        public int? Successor(int key) => Root.Successor(key);
        public int? Predecessor(int key) => Root.Predecessor(key);

        private void AddLevel()
        {
            var newroot = _new(_root.UniverseSize << 1);
            var newminkey = _root.Min.Value;
            var newmin = _root.Remove(newminkey);
            newroot.Create(newminkey, newmin.GetData(), _new);
        }
        private void EnsureCapacity(int key)
        {
            while (key > Capacity)
                AddLevel();
        }

        public bool Contains(int key) => _root.Contains(key);

        /// <summary>
        /// 若关键字不存在则创建新结点，返回true，否则根据关键字寻找该结点并更新数据，返回false
        /// </summary>
        /// <param name="key">新建结点的关键字</param>
        /// <returns>创建或搜索到的结点</returns>
        public bool InsertNode(int key, TData data)
        {
            if (_root == null) _root = _new(_defaultCapacity);
            vanEmdeBoasTreeDataInfo<TData> t;
            return Root.Create(key, data, out t, _new);
        }

        public bool DeleteNode(int key, out TData removedData)
        {
            if (Contains(key))
            {
                removedData = _root.Remove(key).GetData();
                return true;
            }
            else
            {
                removedData = default(TData);
                return false;
            }
        }

        public TData SearchNode(int key) => _root.Search(key).GetData();

        public void Clear()
        {
            _root = _new(Capacity);
        }

        public IEnumerator<TNode> GetEnumerator()
        {
            return Root.TraverseSubtree(TraverseOrder.PreOrder).Select(res => res as TNode).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void TrimExcess()
        {
            throw new NotImplementedException();
        }

        public IDictionary<int, TData> ToDictionary()
            => Root.TraverseSubtree(0, (level, node, index) => level + index * (node as vanEmdeBoasTreeNode<TData>).lsqrt, TraverseOrder.PreOrder)
                .SelectMany(res =>
                {
                    var node = (res.Item1 as vanEmdeBoasTreeNode<TData>);
                    if (node.Count == 0) return Enumerable.Empty<Tuple<int, TData>>();
                    if (node.UniverseSize == 2 && node.Count == 2)
                        return new[] { new Tuple<int, TData>(res.Item2, node.MinData), new Tuple<int, TData>(res.Item2 + 1, node.MaxData) };
                    else return new[] { new Tuple<int, TData>(res.Item2 + node.Min.Value, node.MinData) };
                })
                .ToDictionary(res => res.Item1, res => res.Item2);
    }
}
