using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// node of Binary Search Tree, the data should be stored in a class of TData in order to make swap operation faster.
    /// 二叉搜索树结点，为便于结点交换操作，因此节点数据需要专门的类型来存储
    /// </summary>
    /// <typeparam name="TKey">用于比较的关键字类型</typeparam>
    /// <typeparam name="TData">节点存储的数据类型</typeparam>
    public class BinarySearchTreeNode<TKey> : BinaryTreeNode, IComparableNode<TKey>
    {
        internal bool _flip = false; // 在搜索下行时的方向记录

        public virtual TKey Key { get; set; }

        public override string ToString()
        {
            return "BST node with Key = " + Key.ToString();
        }
    }
}
