using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Class for Binary Tree Node
    /// 二叉树结点类
    /// </summary>
    /// <remarks>
    /// 继承结点可以在结点上维护额外信息减小操作的时间复杂度
    /// 结点没有保留双亲指针，因为部分树不需要向上回溯的过程
    /// </remarks>
    public class BinaryTreeNode
    {
        internal int _version = 0; // 作为持久数据结构版本记录的标记

        public virtual BinaryTreeNode Parent { get; set; }
        public virtual BinaryTreeNode LeftChild { get; set; }
        public virtual BinaryTreeNode RightChild { get; set; }

        #region Update Operations

        internal void SearchDown()
        {

        }
        internal void SearchUp()
        {
            _version++;

            if (LeftChild != null && LeftChild._version > _version)
                _version = LeftChild._version;
            if (RightChild != null && RightChild._version > _version)
                _version = RightChild._version;
            OnSearchUp();
            OnSearchUpRecursive();

            if (Parent != null)
            {
                BinaryTreeNode current = Parent;
                while (current != null)
                {
                    var temp = _version;

                    var nsearchup = !current.OnSearchUpRecursive();
                    if (current.LeftChild != null && current.LeftChild._version > _version)
                        _version = current.LeftChild._version;
                    if (current.RightChild != null && current.RightChild._version > _version)
                        _version = current.RightChild._version;

                    if (temp == _version && nsearchup) break; // No updating any more
                    current = current.Parent;
                }
            }
        }

        /// <summary>
        /// Invoke when travel down from root through the tree
        /// 在搜索下行时调用方法
        /// </summary>
        /// <remarks>
        /// If there are some lazy operations which need being performed on child nodes, the performation should be implemented here.
        /// 如果有延迟操作的标记需要应用到子节点上，应在此方法中实现
        /// </remarks>
        protected virtual void OnSearchDown()
        {

        }
        /// <summary>
        /// Invoke when travel back from the leaves through the tree
        /// 在搜索上行开始时调用的方法
        /// </summary>
        /// <remarks>
        /// If there exist information which need being updated lasily from child nodes, the update should be implemented here.
        /// 如果有信息需要从子节点更新，并且是懒惰更新，应在此方法中实现
        /// </remarks>
        protected virtual void OnSearchUp()
        {

        }
        /// <summary>
        /// Invoke when travel back from the leaves through the tree recursively
        /// 在递归搜索上行时调用的方法
        /// </summary>
        /// <remarks>
        /// If there exist information which need being updated instantly from child nodes, the update should be implemented here.
        /// 如果有信息需要从子节点更新，并且是立即更新，应在此方法中实现
        /// </remarks>
        /// <returns>是否有信息修改，如果为false则会终止递归调用</returns>
        protected virtual bool OnSearchUpRecursive()
        {
            return false;
        }

        #endregion

        /// <summary>
        /// 寻找当前结点在二叉树中序遍历中的后继
        /// </summary>
        public virtual BinaryTreeNode Successor()
        {
            BinaryTreeNode current = this;
            if(current.RightChild !=null)
            {
                current = current.RightChild;
                while (current.LeftChild != null)
                    current = current.LeftChild;
            }
            else
            {
                while (current.Parent?.RightChild == current)
                    current = current.Parent;
                current = current.Parent;
            }
            return current;
        }
        /// <summary>
        /// 寻找当前结点在二叉树中序遍历中的前驱
        /// </summary>
        public virtual BinaryTreeNode Predecessor()
        {
            BinaryTreeNode current = this;
            if (current.LeftChild != null)
            {
                current = current.LeftChild;
                while (current.RightChild != null)
                    current = current.RightChild;
            }
            else
            {
                while (current.Parent?.LeftChild == current)
                    current = current.Parent;
                current = current.Parent;
            }
            return current;
        }

        #region Balance Operations
        // Operations for Balanced Binary Tree (BBT), specifically for Balanced Binary Search Tree (BBST)

        /// <summary>
        /// Zig - Right Rotation at the node
        /// 平衡二叉树的右旋操作
        /// </summary>
        /// <remarks>
        /// 在旋转之后需要考虑更新树根
        /// </remarks>
        public void Zig()
        {
            var lnode = LeftChild;
            if (lnode == null) throw new InvalidOperationException("左孩子为空时无法进行Zig操作");
            this.SearchDown();
            lnode.SearchDown();
            LeftChild = lnode.LeftChild;
            LeftChild.Parent = this;
            lnode.Parent = Parent;
            if (Parent != null)
                if (Parent.LeftChild == this)
                    Parent.LeftChild = lnode;
                else
                    Parent.RightChild = lnode;
            Parent = lnode;
            lnode.RightChild = this;
            this.SearchUp();
            lnode.SearchUp();
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
            var rnode = RightChild;
            if (rnode == null) throw new InvalidOperationException("左孩子为空时无法进行Zig操作");
            this.SearchDown();
            rnode.SearchDown();
            RightChild = rnode.RightChild;
            RightChild.Parent = this;
            rnode.Parent = Parent;
            if (Parent != null)
                if (Parent.RightChild == this)
                    Parent.RightChild = rnode;
                else
                    Parent.LeftChild = rnode;
            Parent = rnode;
            rnode.LeftChild = this;
            this.SearchUp();
            rnode.SearchUp();
        }

        #endregion
    }
}
