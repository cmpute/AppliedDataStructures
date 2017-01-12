using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Tree stored densely in an array, usually used to act as a complete tree (especially a complete binary tree).
    /// 稠密存储的树，常用来表示完全树（尤其是完全二叉树）
    /// </summary>
    /// <remarks>
    /// tree determiners (take binary tree for instance):
    /// complete binary tree: A binary tree in which every level, except possibly the deepest, is entirely filled. At depth n, the height of the tree, all nodes are as far left as possible.
    /// perfect binary tree: A binary tree with all leaf nodes at same depth. All internal nodes have degree k.
    /// full binary tree: A binary tree in which each node has exactly zero or two children. (a.k.a strict tree)
    /// 
    /// 树的限定词（以二叉树为例）：
    /// 完全二叉树：一棵除了最底层外，每一层结点都是满的，且最底层的结点全部集中在左边的二叉树。
    /// 真二叉树：所有叶子结点均在同一深度的二叉树。
    /// 满二叉树：所有结点的均只有两个或零个孩子的二叉树。(亦称正则二叉树，严格二叉树）
    /// </remarks>
    public abstract class CompactTree<TNode> : IRootedTree<TNode>
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
        public virtual void TrimExcess()
        {
            (_nodes as List<TNode>)?.TrimExcess();
        }

        public void Clear() => _nodes.Clear();
    }
}
