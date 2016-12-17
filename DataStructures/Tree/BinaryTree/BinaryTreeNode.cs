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
        protected virtual void OnSearchDown()
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
        protected virtual void OnSearchUp()
        {
            if (LeftChild != null && LeftChild._version > _version)
                _version = LeftChild._version;
            if (RightChild != null && RightChild._version > _version)
                _version = RightChild._version;
        }
    }
}
