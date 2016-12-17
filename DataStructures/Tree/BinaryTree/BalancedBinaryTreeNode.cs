using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Class for balanced binary tree (BBT) node
    /// 平衡二叉树的结点类，包含Zig和Zag操作
    /// </summary>
    public class BalancedBinaryTreeNode : BinaryTreeNode
    {
        public virtual BalancedBinaryTreeNode Parent { get; set; }

        /// <summary>
        /// Zig - Right Rotation at the node
        /// 平衡二叉树的右旋操作
        /// </summary>
        /// <remarks>
        /// 在旋转之后需要考虑更新树根
        /// </remarks>
        public void Zig()
        {
            var lnode = LeftChild as BalancedBinaryTreeNode;
            if (lnode == null) throw new InvalidOperationException("左孩子为空时无法进行Zig操作");
            this.OnSearchDown();
            lnode.OnSearchDown();
            LeftChild = lnode.LeftChild;
            ((BalancedBinaryTreeNode)LeftChild).Parent = this;
            lnode.Parent = Parent;
            if (Parent != null)
                if (Parent.LeftChild == this)
                    Parent.LeftChild = lnode;
                else
                    Parent.RightChild = lnode;
            Parent = lnode;
            lnode.RightChild = this;
            this.OnSearchUp();
            lnode.OnSearchUp();
            _version++;
            lnode._version++;
        }
        /// <summary>
        /// Zag - Left Rotation at the node
        /// 平衡二叉树的左旋操作
        /// </summary>
        /// <remarks>
        /// 在旋转之后需要考虑更新树根
        /// </remarks>
        public void Zag()
        {
            var rnode = RightChild as BalancedBinaryTreeNode;
            if (rnode == null) throw new InvalidOperationException("左孩子为空时无法进行Zig操作");
            this.OnSearchDown();
            rnode.OnSearchDown();
            RightChild = rnode.RightChild;
            ((BalancedBinaryTreeNode)RightChild).Parent = this;
            rnode.Parent = Parent;
            if (Parent != null)
                if (Parent.RightChild == this)
                    Parent.RightChild = rnode;
                else
                    Parent.LeftChild = rnode;
            Parent = rnode;
            rnode.LeftChild = this;
            this.OnSearchUp();
            rnode.OnSearchUp();
            _version++;
            rnode._version++;
        }
    }
}