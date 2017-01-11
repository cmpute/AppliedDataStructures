using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class SplayTree<TNode, TKey> : BinarySearchTree<TNode, TKey> where TNode : BinaryTreeNode, IKeyedNode<TKey>
    {
        public SplayTree() : base() { }
        public SplayTree(IComparer<TKey> comparer) : base(comparer) { }

        /// <summary>
        /// Do rotations(splay) to make <see cref="node"/> the root of the tree
        /// 做一系列旋转使得<see cref="node"/>成为树根
        /// </summary>
        /// <param name="node">被旋转的结点</param>
        protected void Splay(BinaryTreeNode node) => Splay(node, _rootTrailer);
        /// <summary>
        /// Do rotations(splay) to make <see cref="node"/> children of <see cref="target"/>
        /// 做一系列旋转使得<see cref="node"/>成为<see cref="target"/>的孩子节点
        /// </summary>
        /// <param name="node">被旋转的结点</param>
        /// <param name="target">需要成为node父亲的结点</param>
        protected void Splay(BinaryTreeNode node, BinaryTreeNode target)
        {
            //TODO: how to deal with it that the node is parent of target?
            if (node == null) return;
            node.SearchDown();
            var p = node.Parent;
            while (p != target)
            {
                if (p.Parent == target)
                {
                    if (p.LeftChild == node)
                        node.Zig();
                    else
                        node.Zag();
                    return;
                }

                if (p.Parent.LeftChild == p)
                {
                    if (p.LeftChild == node)
                        p.Zig();
                    else
                        node.Zag();
                    node.Zig();
                }
                else
                {
                    if (p.RightChild == node)
                        p.Zag();
                    else
                        node.Zig();
                    node.Zag();
                }
                p = node.Parent;
            }
        }

        protected override TNode SearchInternal(TKey key, bool modify)
        {
            var node = base.SearchInternal(key, modify);
            Splay(node);
            return node;
        }

        public override TNode Insert(TNode node)
        {
            var res = base.Insert(node);
            Splay(res);
            return res;
        }
    }
}
