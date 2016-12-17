using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class SplayTree<TNode, TKey> : BinaryTree<TNode>, ISearchTree<TNode, TKey> where TNode : SplayTreeNode<TKey>
    {
        IComparer<TKey> _comparer;

        public SplayTree()
        {
            _comparer = Comparer<TKey>.Default;
        }
        public SplayTree(IComparer<TKey> comparer)
        {
            _comparer = comparer;
        }

        #region Interface boilerplates

        public TNode this[TKey key]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// Get the collection of Keys
        /// 获取关键字的集合
        /// </summary>
        /// <remarks>
        /// Actually this getter consumes considerable time, avoid using this or optimize this through override getter in derived classes
        /// 实际上获取树的该属性非常耗时，应避免使用或考虑在派生类中进行优化
        /// </remarks>
        public virtual ICollection<TKey> Keys
        {
            get
            {
                var keys = new List<TKey>();
                using (var iter = GetEnumerator())
                    while (iter.MoveNext())
                        keys.Add(iter.Current.Key);
                return keys;
            }
        }

        /// <summary>
        /// Get the collection of nodes (thus get all the values)
        /// 获取结点的集合（即获得了全部的值信息)
        /// </summary>
        /// <remarks>
        /// Actually this getter consumes considerable time, avoid using this or optimize this through override getter in derived classes
        /// 实际上获取树的该属性非常耗时，应避免使用或考虑在派生类中进行优化
        /// </remarks>
        public virtual ICollection<TNode> Values
        {
            get
            {
                var values = new List<TNode>();
                using (var iter = GetEnumerator())
                    while (iter.MoveNext())
                        values.Add(iter.Current);
                return values;
            }
        }

        public void Add(KeyValuePair<TKey, TNode> item)
        {
            throw new NotImplementedException();
        }

        public void Add(TKey key, TNode value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TNode> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TNode>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TNode> item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TNode value)
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValuePair<TKey, TNode>> IEnumerable<KeyValuePair<TKey, TNode>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
