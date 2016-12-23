using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// 不允许允许重复TKey的二叉搜索树
    /// </summary>
    public class BinarySearchTree<TNode, TKey> : BinaryTree<TNode>, ISearchTree<TNode, TKey> where TNode : BinaryTreeNode, IComparableNode<TKey>
    {
        protected IComparer<TKey> _comparer;

        public BinarySearchTree()
        {
            _comparer = Comparer<TKey>.Default;
        }
        public BinarySearchTree(IComparer<TKey> comparer)
        {
            _comparer = comparer;
        }

        public virtual TNode Search(TKey key)
        {
            TNode current = Root;
            while (current != null)
            {
                int result = _comparer.Compare(current.Key, key);
                if (result == 0)
                    return current;
                if (result < 0)
                    current = current.RightChild as TNode;
                else
                    current = current.LeftChild as TNode;
            }
            return current;
        }

        public virtual TNode Insert(TNode node)
        {
            if (Root == null)
            {
                Root = node;
                return node;
            }
            TNode current = Root;
            int result = -1;
            while (current != null)
            {
                result = _comparer.Compare(current.Key, node.Key);
                if (result == 0)
                    return current;
                if (result < 0)
                {
                    if (current.RightChild == null)
                    {
                        current.RightChild = node;
                        node.Parent = current;
                        current.OnSearchUp();
                        break;
                    }
                    current = current.RightChild as TNode;
                }
                else
                {
                    if (current.LeftChild == null)
                    {
                        current.LeftChild = node;
                        node.Parent = current;
                        current.OnSearchUp();
                        break;
                    }
                    current = current.LeftChild as TNode;
                }
            }
            return node;
        }

        public virtual TNode Delete(TKey key)
        {
            TNode current = Root;
            while (current != null)
            {
                int result = _comparer.Compare(current.Key, key);
                if (result == 0)
                {
                    TNode replace = null;
                    if (current.LeftChild == null) replace = current.LeftChild as TNode;
                    else if (current.RightChild == null) replace = current.RightChild as TNode;
                    else
                    {
                        var succ = current.Successor();
                        succ.SwapWith(current);
                        //current.onSearchDown();
                        if (current.Parent == succ)
                            current.Parent.RightChild = current.RightChild;
                        else
                            current.Parent.LeftChild = current.RightChild;
                        current.RightChild.Parent = current.Parent;
                        current.Parent.OnSearchUp();
                    }
                    if (current.Parent.LeftChild == current) current.Parent.LeftChild = replace;
                    else current.Parent.RightChild = replace;
                    //current.Parent = null;
                    return current;
                }
                if (result < 0)
                    current = current.RightChild as TNode;
                else
                    current = current.LeftChild as TNode;
            }
            return current;
        }
    }
}
