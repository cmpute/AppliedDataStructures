using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class CartesianTreeNode<TKey> : BinaryTreeNode, IKeyProvider<TKey>, IPriorityProvider<int>
    {
        static Random prior_rand = new Random();
        int _prior;

        public TKey Key { get; set; }
        int IPriorityProvider<int>.Priority => _prior;

        public CartesianTreeNode(TKey key)
        {
            Key = key;
            _prior = prior_rand.Next();
        }
    }
}
