using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced.Tree
{
    public class CartesianTreeNode<TKey> : BinaryTreeNode, IKeyProvider<TKey>, IPriorityProvider<int>
    {
        static Random prior_rand = new Random();
        int _prior;

        public TKey Key { get; set; }
        int IPriorityProvider<int>.Priority => _prior;

        public CartesianTreeNode(TKey key)
        {
            Contract.Requires<ArgumentNullException>(key != null);

            Key = key;
            _prior = GetRandomPriority();
        }

        [Pure]
        protected virtual int GetRandomPriority()
        {
            return prior_rand.Next();
        }
    }
}
