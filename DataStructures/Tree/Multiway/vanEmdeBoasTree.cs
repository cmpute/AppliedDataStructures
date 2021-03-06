﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced.Tree
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
        private const int _defaultCapacity = 8;

        TNode _root = null;
        Func<int, TNode> _new;
        public int Count => Root?.Count ?? 0;
        public int Capacity => Root?.UniverseSize ?? _defaultCapacity;

        public TNode Root => _root;

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_new != null);
            Contract.Invariant(Count >= 0);
            Contract.Invariant(Capacity >= 2);
        }
        
        /// <summary>
        /// 创建一棵自定义结点新的vEB树
        /// </summary>
        /// <param name="newNode">创建新结点的方法</param>
        public vanEmdeBoasTree(Func<int, TNode> newNode) : this(_defaultCapacity, newNode) { }
        /// <summary>
        /// 创建一棵自定义结点且有初始大小的vEB树
        /// </summary>
        /// <param name="initsize">树全域的初始大小</param>
        /// <param name="newNode">创建新结点的方法</param>
        public vanEmdeBoasTree(int initsize, Func<int, TNode> newNode)
        {
            Contract.Requires<ArgumentException>(initsize > 0);
            Contract.Requires<ArgumentNullException>(newNode != null);
            
            if (initsize < 2) initsize = _defaultCapacity;
            _new = newNode;
            _root = _new(Utils.Ceil2(initsize));
        }
        
        /// <summary>
        /// Find the smallest key bigger than <paramref name="key"/>
        /// 获取集合中比<paramref name="key"/>大的最小键值
        /// </summary>
        public int? SuccessorKey(int key) => Root.Successor(key);
        /// <summary>
        /// Find the biggest key smaller than <paramref name="key"/>
        /// 获取集合中比<paramref name="key"/>小的最大键值
        /// </summary>
        public int? PredecessorKey(int key) => Root.Predecessor(key);

        private void EnsureCapacity(int key)
        {
            while (key > Capacity)
                _root = _root.ExpandUp(_new) as TNode;
            if (_root == null) throw new Exception("在扩容过程中出现异常造成根节点为空");
        }

        public bool Contains(int key) => _root.Contains(key);

        /// <summary>
        /// 若关键字不存在则创建新结点，返回true，否则根据关键字寻找该结点并更新数据，返回false
        /// </summary>
        /// <param name="key">新建结点的关键字</param>
        /// <returns>创建或搜索到的结点</returns>
        public bool InsertNode(int key, TData data)
        {
            if (_root == null) _root = _new(Utils.Ceil2(key));
            else EnsureCapacity(key);
            vanEmdeBoasTreeDataInfo<TData> t;
            return Root.Create(key, data, out t, _new);
        }

        public TData DeleteNode(int key)
        {
            TData res;
            DeleteNode(key, out res);
            return res;
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

        /// <summary>
        /// 清空存有的键值
        /// </summary>
        public void Clear()
        {
            _root = _new(Utils.Ceil2(Capacity));
        }

        public IEnumerator<TNode> GetEnumerator()
        {
            return Root.TraverseSubtree(TraverseOrder.PreOrder).Select(res => res as TNode).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Reorganize the root node making the capacity as small as possible.
        /// 重新组织根节点使得结点数目尽可能小
        /// </summary>
        public void TrimExcess()
        {
            _root = _root.TrimDown(_new) as TNode;
            if (_root == null) throw new Exception("在收缩过程中出现异常造成根节点为空");
        }

        public IDictionary<int, TData> ToDictionary()
        {
            Contract.Requires<InvalidOperationException>(Root != null);

            return Root.TraverseSubtree(0, (level, node, index) => level + index * (1 << ((node as vanEmdeBoasTreeNode<TData>).UniverseBits >> 1)), TraverseOrder.PreOrder)
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
}
