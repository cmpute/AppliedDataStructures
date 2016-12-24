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
    public class BinaryTreeNode : INode
    {
        internal int _version = 0; // 作为持久数据结构版本记录的标记

        public virtual BinaryTreeNode Parent { get; set; }
        public virtual BinaryTreeNode LeftChild { get; set; }
        public virtual BinaryTreeNode RightChild { get; set; }
        /// <summary>
        /// Invoke when travel down from root through the tree
        /// 在搜索下行时调用方法
        /// </summary>
        /// <remarks>
        /// If there are some lazy operations which need being performed on child nodes, the performation should be implemented here.
        /// 如果有延迟操作的标记需要应用到子节点上，应在此方法中实现
        /// </remarks>
        public virtual void OnSearchDown()
        {

        }
        /// <summary>
        /// Invoke when travel back from the leaves through the tree
        /// 在搜索上行时调用方法
        /// </summary>
        /// <remarks>
        /// If there exist information which need being updated from child nodes, the update should be implemented here.
        /// 如果有信息需要从子节点更新，应在次方法中实现
        /// </remarks>
        public virtual void OnSearchUp()
        {
            _version++;
            if (LeftChild != null && LeftChild._version > _version)
                _version = LeftChild._version;
            if (RightChild != null && RightChild._version > _version)
                _version = RightChild._version;
            BinaryTreeNode current = this;
            while (current.Parent != null)
            {
                if (current.LeftChild != null && current.LeftChild._version > _version)
                    _version = LeftChild._version;
                if (current.RightChild != null && current.RightChild._version > _version)
                    _version = RightChild._version;
                current = current.Parent;
            }
        }
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
    }
}
