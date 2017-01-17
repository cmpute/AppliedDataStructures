using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// van Emde Boas Tree (a.k.a vEB-tree)
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class vanEmdeBoasTree<TNode, TKey> : ISearchTree<TNode, TKey> where TNode : vanEmdeBoasTreeNode
    {
        IComparer<TKey> _comparer;
        public IComparer<TKey> KeyComparer => _comparer;

        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public TNode Root { get; protected set; }

        TNode Successor(TKey key) { throw new NotImplementedException(); }
        TNode Predecessor(TKey key) { throw new NotImplementedException(); }
        

        public TNode Search(TKey key)
        {
            throw new NotImplementedException();
        }

        public TNode Delete(TKey key)
        {
            throw new NotImplementedException();
        }

        public TNode Insert(TNode node)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<TNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Delete(TNode node)
        {
            throw new NotImplementedException();
        }
    }
}
