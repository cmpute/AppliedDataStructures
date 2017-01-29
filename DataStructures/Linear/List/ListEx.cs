using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract = System.Diagnostics.Contracts.Contract;

namespace System.Collections.Advanced
{
    public static class ListEx
    {
        public static IList<T> AsList<T>(this LinkedList<T> list) => new LinkedListWrapper<T>(list);
        internal static int IndexOf<T>(this IEnumerable<T> list, T item)
        {
            var itor = list.GetEnumerator();
            int count = 0;
            if (ReferenceEquals(item, null))
            {
                while (itor.MoveNext())
                {
                    if (ReferenceEquals(itor.Current, null))
                        return count;
                    count++;
                }
            }
            else
            {
                var comparer = EqualityComparer<T>.Default;
                while (itor.MoveNext())
                {
                    if (comparer.Equals(itor.Current, item))
                        return count;
                    count++;
                }
            }
            return -1;
        }
        internal static void CopyTo<T>(this IEnumerable<T> list, T[] array, int arrayIndex)
        {
            Diagnostics.Contracts.Contract.Requires<ArgumentNullException>(array != null);
            Diagnostics.Contracts.Contract.Requires<ArgumentException>(array.Rank == 1);

            int current = arrayIndex;
            var iter = list.GetEnumerator();
            while (iter.MoveNext())
                array[current++] = iter.Current;
        }
        internal static void PrintTo<T>(this IEnumerable<T> list, System.IO.TextWriter textOut, string separator = " ")
        {
            var iter = list.GetEnumerator();
            if (!iter.MoveNext()) textOut.Write("null");

            StringBuilder sb = new StringBuilder();
            sb.Append(iter.Current);

            while(iter.MoveNext())
            {
                sb.Append(separator);
                sb.Append(iter.Current);
            }

            textOut.Write(sb.ToString());
        }
    }

    /// <remarks>
    /// The linked list is cyclic, and the wrapper uses "Move" To Front (MTF) strategy.
    /// 链表是循环链表，这个封装采用了MTF策略
    /// </remarks>
    class LinkedListWrapper<T> : IList<T>
    {
        LinkedList<T> _list;
        int _hotindex;
        LinkedListNode<T> _hotnode;

        public LinkedListWrapper(LinkedList<T> list)
        {
            _list = list;
        }

        private LinkedListNode<T> Find(int index)
        {
            if (index < _hotindex)
            {
                if (index < _hotindex - index) // front -> hotindex
                {
                    _hotindex = index;
                    _hotnode = _list.First;

                    while (index-- > 0)
                        _hotnode = _hotnode.Next;
                    return _hotnode;
                }
                else // hotindex -> front
                {
                    var temp = index;

                    while (index++ < _hotindex)
                        _hotnode = _hotnode.Previous;

                    _hotindex = temp;
                    return _hotnode;
                }
            }
            else
            {
                if (index - _hotindex < Count - index) // hotindex -> end
                {
                    var temp = index;

                    if (_hotnode == null) _hotnode = _list.First;
                    while (index-- > _hotindex)
                        _hotnode = _hotnode.Next;

                    _hotindex = temp;
                    return _hotnode;
                }
                else // end -> hotindex
                {
                    _hotindex = index++;
                    _hotnode = _list.Last;

                    while (index++ < Count)
                        _hotnode = _hotnode.Previous;
                    return _hotnode;
                }
            }
        }

        public T this[int index]
        {
            get
            {
                Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index < Count);
                return Find(index).Value;
            }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index < Count);
                Find(index).Value = value;
            }
        }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public void Add(T item) => _list.AddLast(item);

        public void Clear()
        {
            _list.Clear();
            _hotindex = 0;
            _hotnode = null;
        }

        public bool Contains(T item) => IndexOf(item) >= 0;

        public void CopyTo(T[] array, int arrayIndex) => this.CopyTo<T>(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        public int IndexOf(T item) => this.IndexOf<T>(item);

        public void Insert(int index, T item)
        {
            _hotnode = _list.AddBefore(Find(index), item);
        }

        public bool Remove(T item)
        {
            // Reset hot node, because LinkedList.Find(LinkedListNode) method is sealed,
            // and it's inefficient to search again
            _hotindex = 0;
            _hotnode = null;

            var res = _list.Find(item);
            if (res != null)
            {
                _list.Remove(_list.Find(item));
                return true;
            }
            else return false;
        }

        public void RemoveAt(int index)
        {
            var loc = Find(index);
            _hotnode = (_hotnode ?? _list.First).Next;
            if (_hotnode == null) _hotindex = 0;
            _list.Remove(loc);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
