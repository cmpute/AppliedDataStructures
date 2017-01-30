using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced.Tree
{
    /// <summary>
    /// Tree stored densely in an array, usually used to act as a complete tree (especially a complete binary tree).
    /// 稠密存储的树，常用来表示完全树（尤其是完全二叉树）
    /// </summary>
    /// <remarks>
    /// For what is a complete tree, see <see cref="TreeKind"/>
    /// </remarks>
    public abstract class CompactTree<TNode> : IRootedTree<TNode>
    {
        protected virtual IList<TNode> Nodes { get; set; }

        /// <inheritdoc/>
        public int Count => Nodes.Count;

        /// <inheritdoc/>
        public TNode Root => Nodes.Count > 0 ? Nodes[0] : default(TNode);

        /// <inheritdoc/>
        public IEnumerator<TNode> GetEnumerator() => Nodes.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => Nodes.GetEnumerator();

        public void Clear() => Nodes.Clear();
    }
}
