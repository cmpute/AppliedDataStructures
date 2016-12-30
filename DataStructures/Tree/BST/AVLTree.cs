using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class AVLTree<TNode, TKey> : BinarySearchTree<TNode, TKey> where TNode : BinaryTreeNode, IKeyedNode<TKey>
    {
    }
}
