using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced.Tree
{
    public class AVLTreeNode<TKey> : BinaryTreeNode, IKeyProvider<TKey>
    {
        public TKey Key { get; set; }
        public int Height { get; private set; } = 1;
        public int BalanceFactor => LeftHeight - RightHeight;

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(Height >= 1);
            Contract.Invariant(LeftHeight >= 0);
            Contract.Invariant(RightHeight >= 0);
        }

        private int LeftHeight => (LeftChild as AVLTreeNode<TKey>)?.Height ?? 0;
        private int RightHeight => (RightChild as AVLTreeNode<TKey>)?.Height ?? 0;
        private AVLTreeNode<TKey> TallerChild => BalanceFactor > 0 ? LeftChild as AVLTreeNode<TKey> : RightChild as AVLTreeNode<TKey>;
        protected override bool OnSearchUpRecursive()
        {
            var b = base.OnSearchUpRecursive();
            if (Math.Abs(BalanceFactor) > 1)
                TallerChild.TallerChild.Connect34();
            int tempval = Math.Max(LeftHeight, RightHeight) + 1;
            if (tempval != Height)
            {
                Height = tempval;
                return true;
            }
            else
                return b;
        }
    }
}
