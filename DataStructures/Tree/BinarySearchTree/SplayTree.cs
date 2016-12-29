﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class SplayTree<TNode, TKey> : BinarySearchTree<TNode, TKey> where TNode : BinaryTreeNode, IComparableNode<TKey>
    {
        protected void Splay(TNode node)
        {

        }
    }
}
