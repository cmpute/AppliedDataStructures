using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// AVL Tree, a kind of Balanced Binary Search Tree
    /// AVL树，一种平衡二叉搜索树
    /// </summary>
    /// <typeparam name="TNode">结点类型</typeparam>
    /// <typeparam name="TKey">关键字类型</typeparam>
    public class AVLTree<TNode, TKey> : BinarySearchTree<TNode, TKey> where TNode : AVLTreeNode<TKey>
    {
        // There is no more edit in BinarySearchTree Members, All modifications are in the AVLTreeNode class
        // 在BinarySearchTree类型中没有修改，AVL相比二叉搜索树所有的修改均在AVLTreeNode的类型中

        /// <summary>
        /// The height of AVL tree.
        /// AVL树的树高
        /// </summary>
        public int Height => Root.Height;
    }
}
