using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Node of a threaded binary tree
    /// 线索二叉树的结点
    /// </summary>
    public class ThreadedBinaryTreeNode : BinaryTreeNode
    {
        /* There is no more edit in BinaryTree Members, All modifications are in the ThreadedBinaryTreeNode class.
         * As long as a binary tree use ThreadedBinaryTreeNode as its node type, the tree is a threaded binary tree.
         * 
         * 在BinaryTree类型中没有修改，线索二叉树相比二叉树所有的修改均在ThreadedBinaryTreeNode的类型中。
         * 只要一颗二叉树使用ThreadedBinaryTreeNode作为结点类型，这棵树就是线索二叉树。
         */

        bool _leftThreaded = true, _rightThreaded = true;

        /// <summary>
        /// Update thread of this node
        /// 更新当前结点的线索
        /// </summary>
        public void UpdateThread()
        {
            if (_leftThreaded || _lchild == null)
            {
                _leftThreaded = true;
                _lchild = base.Predecessor();
            }

            if (_rightThreaded || _rchild == null)
            {
                _rightThreaded = true;
                _rchild = base.Successor();
            }
        }
        /// <summary>
        /// Get or set left child. If left child is threaded, then return <see cref="BinaryTreeNode.Null"/>.
        /// 获取或设置左孩子。如果左孩子为线索指针，则返回<see cref="BinaryTreeNode.Null"/>
        /// </summary>
        public override BinaryTreeNode LeftChild
        {
            get { return _leftThreaded ? Null : base.LeftChild; }
            set
            {
                base.LeftChild = value;
                _leftThreaded = false;
                UpdateThread(); 
                (value as ThreadedBinaryTreeNode)?.UpdateThread();
            }
        }
        /// <summary>
        /// Get or set right child. If right child is threaded, then return <see cref="BinaryTreeNode.Null"/>.
        /// 获取或设置右孩子。如果右孩子为线索指针，则返回<see cref="BinaryTreeNode.Null"/>
        /// </summary>
        public override BinaryTreeNode RightChild
        {
            get { return _rightThreaded ? Null : base.RightChild; }
            set
            {
                base.RightChild = value;
                _rightThreaded = false;
                UpdateThread();
                (value as ThreadedBinaryTreeNode)?.UpdateThread();
            }
        }

        public bool IsLeftThreaded => _leftThreaded;
        public bool IsRightThreaded => _rightThreaded;

        /// <summary>
        /// Get directly the node stored in the position of left child
        /// 直接获取存储在左子树位子上的结点
        /// </summary>
        public BinaryTreeNode RawLeftChild => _lchild;
        /// <summary>
        /// Get directly the node stored in the position of right child
        /// 直接获取存储在右子树位子上的结点
        /// </summary>
        public BinaryTreeNode RawRightChild => _rchild;

        public override BinaryTreeNode Successor()
        {
            if (RightChild != null)
            {
                BinaryTreeNode current = RightChild;
                while (current.LeftChild != null)
                    current = current.LeftChild;
                return current;
            }
            else return RawRightChild;
        }
        public override BinaryTreeNode Predecessor()
        {
            if (LeftChild != null)
            {
                BinaryTreeNode current = LeftChild;
                while (current.RightChild != null)
                    current = current.RightChild;
                return current;
            }
            else return RawLeftChild;
        }
    }
}
