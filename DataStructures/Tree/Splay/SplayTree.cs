using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class SplayTree<TNode, TKey> : BinaryTree<TNode>, ISearchTree<TNode, TKey> where TNode : BinaryTreeNode, IComparableNode<TKey>
    {
        public TNode Delete(TKey key)
        {
            throw new NotImplementedException();
        }
        public TNode Insert(TNode node)
        {
            throw new NotImplementedException();
        }
        public TNode Search(TKey key)
        {
            throw new NotImplementedException();
        }
    }
}
