using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Abstraction of Binary Tree
    /// 二叉树的抽象类
    /// </summary>
    /// <remarks>
    /// 对于完全二叉树的紧凑型实现在此处不予考虑，如需要紧凑实现增强性能的话需要使用结构体数组，并且用Marshall使内存紧凑，这并不是C#的长处，应改用C++
    /// 对于广义表的树存储在此也不予考虑
    /// </remarks>
    public abstract class BinaryTree<TNode> : IRootedTree<TNode> where TNode : BinaryTreeNode
    {
        /// <summary>
        /// 根节点的哨兵，在中序遍历中位于所有结点的最前面
        /// </summary>
        protected readonly BinaryTreeNode _rootTrailer = new BinaryTreeNode();
        public TNode Root
        {
            get { return _rootTrailer.RightChild as TNode; }
            protected set { _rootTrailer.RightChild = value; }
        }
        public int Count { get; protected set; }

        
        public IEnumerator<TNode> GetEnumerator(TraverseOrder order) => Root.GetSubtreeEnumerator(order);
        /// <summary>
        /// Return InOrder Enumerator by default
        /// 默认返回中序遍历器
        /// </summary>
        public IEnumerator<TNode> GetEnumerator() => new BinaryTreeEnumerator<TNode>.InOrderEnumerator(Root);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
