using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Node of a doubly-chained tree.
    /// 双链树结点
    /// </summary>
    /// <remarks>
    /// Doubly-chained tree (a.k.a first-child next-sibling binary tree, left-child right-sibling binary tree, filial-heir chain)
    /// 双链树（亦称孩子-兄弟二叉树）是一种通过二叉树来表示多叉树的结构。
    /// </remarks>
    public class DoublyChainedTreeNode : BinaryTreeNode, IMultiwayTreeNode
    {
        public IReadOnlyCollection<IMultiwayTreeNode> Children => EnumerateChildren().ToList();

        public IEnumerable<DoublyChainedTreeNode> EnumerateChildren()
        {
            var p = LeftChild as DoublyChainedTreeNode;
            while (p != null)
            {
                yield return p;
                p = p.RightChild as DoublyChainedTreeNode;
            }
        }
    }
}
