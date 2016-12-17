using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public interface ISearchTreeNode<TKey> : INode
    {
        TKey Key { get; }
    }
}
