using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class AVLTreeNode<TKey> : BinaryTreeNode, IKeyProvider<TKey>
    {
        public TKey Key { get; set; }
        public int Height { get; private set; } = 1;
        public int BalanceFactor => LeftHeight - RightHeight;

        private int LeftHeight => (LeftChild as AVLTreeNode<TKey>)?.Height ?? 0;
        private int RightHeight => (RightChild as AVLTreeNode<TKey>)?.Height ?? 0;
        private AVLTreeNode<TKey> TallerChild => BalanceFactor > 0 ? LeftChild as AVLTreeNode<TKey> : RightChild as AVLTreeNode<TKey>;
        private bool OnSearchUpRecursive(bool rotate)
        {
            if (rotate && Math.Abs(BalanceFactor) > 1)
                TallerChild.TallerChild.RotateAt();
            int tempval = Math.Max(LeftHeight, RightHeight) + 1;
            if (tempval != Height)
            {
                Height = tempval;
                return true;
            }
            else
                return false;
        }
        protected override bool OnSearchUpRecursive()
        {
            var b = base.OnSearchUpRecursive();
            if (OnSearchUpRecursive(true))
                return true;
            else
                return b;
        }

        /// <summary>
        /// Reconstruct connected node a,b,c and their childrens.
        /// 将相连的三个顶点和其子孙连进行重构
        /// </summary>
        /// <returns>新的祖先，即<paramref name="b"/></returns>
        /// <remarks>
        /// after reconstruction:
        /// 重构以后的形状
        ///      b
        ///    /  \
        ///   a    c
        ///  / \  / \
        /// t0 t1 t2 t3
        /// </remarks>
        protected static AVLTreeNode<TKey> Connect34(AVLTreeNode<TKey> a, AVLTreeNode<TKey> b, AVLTreeNode<TKey> c, BinaryTreeNode t0, BinaryTreeNode t1, BinaryTreeNode t2, BinaryTreeNode t3, BinaryTreeNode top)
        {
            top.TransferParent(b);

            a.LeftChild = t0;
            a.RightChild = t1;
            a.OnSearchUpRecursive(false);

            c.LeftChild = t2;
            c.RightChild = t3;
            c.OnSearchUpRecursive(false);

            b.LeftChild = a;
            b.RightChild = c;
            b.OnSearchUpRecursive(false);

            return b;
        }
        /// <summary>
        /// make v and its parent and grandparent balanced
        /// 将v点与其两代祖先进行平衡
        /// </summary>
        /// <returns>新的祖先</returns>
        protected AVLTreeNode<TKey> RotateAt()
        {
            var v = this;
            AVLTreeNode<TKey> p = v.Parent as AVLTreeNode<TKey>, g = p.Parent as AVLTreeNode<TKey>;
            if (p == g.LeftChild)
                if (v == p.LeftChild)
                {
                    //       g
                    //      / \
                    //     p   t3
                    //    / \
                    //   v   t2
                    //  / \
                    // t0 t1
                    return Connect34(v, p, g, v.LeftChild, v.RightChild, p.RightChild, g.RightChild,g); // zig
                }
                else
                {
                    //       g
                    //      / \
                    //     p   t3
                    //    / \
                    //   t0  v
                    //      / \
                    //     t1 t2
                    return Connect34(p, v, g, p.LeftChild, v.LeftChild, v.RightChild, g.RightChild,g); // zag-zig
                }
            else
            {
                if (v == p.LeftChild)
                {
                    //      g
                    //     / \
                    //    t0  p
                    //       / \
                    //      v   t3
                    //     / \
                    //    t1 t2
                    return Connect34(g, v, p, g.LeftChild, v.LeftChild, v.RightChild, p.RightChild,g); // zig-zag
                }
                else
                {
                    //   g
                    //  / \
                    // t0  p
                    //    / \
                    //   t1  v
                    //      / \
                    //     t2 t3
                    return Connect34(g, p, v, g.LeftChild, p.LeftChild, v.LeftChild, v.RightChild,g); // zig
                }
            }
        }
    }
}
