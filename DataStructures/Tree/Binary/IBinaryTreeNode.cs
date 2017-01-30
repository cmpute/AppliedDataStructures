using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced.Tree
{
    /// <summary>
    /// Abstraction of a binary tree node, only used in extension methods.
    /// 二叉树结点的抽象接口，仅用于一些扩展方法
    /// </summary>
    public interface IBinaryTreeNode
    {
        IBinaryTreeNode Parent { get; }
        IBinaryTreeNode LeftChild { get; }
        IBinaryTreeNode RightChild { get; }
    }
}
