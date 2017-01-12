using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class TreapNode<TKey> : BinaryTreeNode, IKeyedNode<TKey>
    {
        static Random prior_rand = new Random();
        public TKey Key { get; set; }
        internal int Priority;

        public TreapNode(TKey key)
        {
            Key = key;
            Priority = prior_rand.Next();
        }
    }
}
