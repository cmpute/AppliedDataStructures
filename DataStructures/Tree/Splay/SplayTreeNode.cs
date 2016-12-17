using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class SplayTreeNode<TKey> : BalancedBinaryTreeNode, ISearchTreeNode<TKey>
    {
        TKey _key;

        public SplayTreeNode(TKey key)
        {
            _key = key;
        }

        public TKey Key { get { return _key; } }
    }
}
