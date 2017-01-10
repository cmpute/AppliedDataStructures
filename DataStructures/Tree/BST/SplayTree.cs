using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class SplayTree<TNode, TKey> : BinarySearchTree<TNode, TKey> where TNode : BinaryTreeNode, IKeyedNode<TKey>, new()
    {
        protected void Splay(BinaryTreeNode node, BinaryTreeNode target)
        {
            node.SearchDown();
            var p = node.Parent;
            while(p!=target)
            {
                if(p.Parent == target)
                {
                    if (p.LeftChild == node)
                        node.Zig();
                    else
                        node.Zag();
                    return;
                }

                if(p.Parent.LeftChild == p)
                {
                    if (p.LeftChild == node)
                        p.Zig();
                    else
                        node.Zag();
                    node.Zag();
                }
                else
                {
                    if (p.RightChild == node)
                        p.Zag();
                    else
                        node.Zig();
                    node.Zig();
                }
                p = node.Parent;
                //TODO: 如果在树根旋转需要更新树根
            }
        }
    }
}
