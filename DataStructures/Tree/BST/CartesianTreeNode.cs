using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class CartesianTreeNode<TKey> : BinaryTreeNode, IKeyProvider<TKey>
    {
        static Random prior_rand = new Random();
        public TKey Key { get; set; }
        internal int Priority { get; set; }

        public CartesianTreeNode(TKey key)
        {
            Key = key;
            Priority = prior_rand.Next();
        }
    }
}
