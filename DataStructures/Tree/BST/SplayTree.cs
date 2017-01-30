using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced.Tree
{
    public class SplayTree<TNode, TKey> : BinarySearchTree<TNode, TKey> where TNode : BinaryTreeNode, IKeyProvider<TKey>
    {
        public SplayTree() : base() { }
        public SplayTree(IComparer<TKey> comparer) : base(comparer) { }

        /// <summary>
        /// Do rotations(splay) to make <see cref="node"/> the root of the tree
        /// 做一系列旋转使得<see cref="node"/>成为树根
        /// </summary>
        /// <param name="node">被旋转的结点</param>
        protected void Splay(BinaryTreeNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Splay(node, _rootTrailer);
        }
        /// <summary>
        /// Do rotations(splay) to make <see cref="node"/> children of <see cref="target"/>
        /// 做一系列旋转使得<see cref="node"/>成为<see cref="target"/>的孩子节点
        /// </summary>
        /// <param name="node">被旋转的结点</param>
        /// <param name="target">需要成为node父亲的结点</param>
        /// <remarks>make sure that the node is a descandent of target.</remarks>
        protected void Splay(BinaryTreeNode node, BinaryTreeNode target)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(target == null || target.IsAncestorOf(node));
            Contract.EndContractBlock();

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
            if (node != null) Splay(node);
            return node;
        }

        protected override TNode InsertInternal(TNode node)
        {
            var res = base.InsertInternal(node);
            if (res != null) Splay(res);
            return res;
        }
    }
}
