using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Types of tree
    /// 树的种类
    /// </summary>
    /// <remarks>
    /// tree determiners (take binary tree for instance):
    /// complete binary tree: A binary tree in which every level, except possibly the deepest, is entirely filled. At depth n, the height of the tree, all nodes are as far left as possible.
    /// perfect binary tree: A binary tree with all leaf nodes at same depth. All internal nodes have degree k.
    /// full binary tree: A binary tree in which each node has exactly zero or two children. 
    /// strict binary tree: A binary tree that all inner nodes(outdegree is non-zero) have same outdegree. (a.k.a regular binary tree)
    /// 
    /// 树的限定词（以二叉树为例）：
    /// 完全二叉树：一棵除了最底层外，每一层结点都是满的，且最底层的结点全部集中在左边的二叉树。
    /// 真二叉树：所有叶子结点均在同一深度的二叉树。
    /// 满二叉树：所有结点的均只有两个或零个孩子的二叉树。
    /// 严格二叉树：所有内部结点的度数相同的二叉树（亦称正则二叉树）
    /// </remarks>
    [Flags]
    public enum TreeKind
    {
        /// <summary>
        /// 普通树
        /// </summary>
        Ordinary = 0,
        /// <summary>
        /// 真树
        /// </summary>
        Perfect = 1 | 2 | 4 | 8, // A perfect tree is definitly a complete tree and full tree
        /// <summary>
        /// 完全树
        /// </summary>
        Complete = 1,
        /// <summary>
        /// 满树
        /// </summary>
        Full = 2 | 4, // A full tree is definitly a strict tree
        /// <summary>
        /// 正则树
        /// </summary>
        Strict = 2,
    }
}
