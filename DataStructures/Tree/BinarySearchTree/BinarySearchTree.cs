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
    public class BinarySearchTree<TNode, TKey, TData> : BinaryTree<TNode>, ISearchTree<TNode, TKey> where TNode : BinarySearchTreeNode<TKey,TData>
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
                Count++;
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
                        Count++;
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
                        Count++;
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
                    if (current.LeftChild == null) replace = current.RightChild as TNode;
                    else if (current.RightChild == null) replace = current.LeftChild as TNode;
                    else
                    {
                        var succ = current.Successor() as BinarySearchTreeNode<TKey, TData>;
                        succ.SwapWith(current);
                        while (Root.Parent != null) Root = Root.Parent as TNode;
                        //current.onSearchDown();
                        replace = current.RightChild as TNode;
                        if (succ.Parent == current)
                            succ.Parent.RightChild = succ.RightChild;
                        else
                            succ.Parent.LeftChild = succ.RightChild;
                        if (succ.RightChild != null)
                            succ.RightChild.Parent = succ.Parent;
                        succ.Parent?.OnSearchUp();
                    }
                    if (current.Parent != null)
                    {
                        if (current.Parent.LeftChild == current) current.Parent.LeftChild = replace;
                        else current.Parent.RightChild = replace;
                    }
                    if (replace != null) replace.Parent = current.Parent;
                    //remove(current);
                    Count--;
                    if (Count == 0) Root = null;
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
