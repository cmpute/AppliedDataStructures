﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Binary Tree stored densely in an array, usually used to act as Complete Binary Tree
    /// 稠密存储的二叉树（常用来表示完全二叉树）
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public abstract class DenseBinaryTree<TNode> : IRootedTree<TNode>
    {
        protected IList<TNode> _nodes;

        /// <inheritdoc/>
        public int Count => _nodes.Count;

        /// <inheritdoc/>
        public TNode Root => _nodes.Count > 0 ? _nodes[0] : default(TNode);

        /// <inheritdoc/>
        public IEnumerator<TNode> GetEnumerator() => _nodes.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => _nodes.GetEnumerator();

        /// <summary>
        /// 如果是用<see cref="List{T}"/>存储的结点数据，那么该方法将会调用<see cref="List{T}.TrimExcess"/>方法。
        /// </summary>
        public void TrimExcess()
        {
            (_nodes as List<TNode>)?.TrimExcess();
        }

        public void Clear() => _nodes.Clear();
    }
}
