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

    class LinkedListWrapper<T> : IList<T>
    {
        LinkedList<T> _list;
        public LinkedListWrapper(LinkedList<T> list)
        {
            _list = list;
        }

        private LinkedListNode<T> Find(int index)
        {
            if (index <= Count - index)
            {
                var node = _list.First;
                while (index-- > 0)
                    node = node.Next;
                return node;
            }
            else
            {
                index = Count - index - 1;
                var node = _list.Last;
                while (index-- > 0)
                    node = node.Previous;
                return node;
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

        public void Clear() => _list.Clear();

        public bool Contains(T item) => IndexOf(item) >= 0;

        public void CopyTo(T[] array, int arrayIndex) => this.CopyTo<T>(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        public int IndexOf(T item) => this.IndexOf<T>(item);

        public void Insert(int index, T item)
        {
            var loc = Find(index);
            _list.AddBefore(loc, item);
        }

        public bool Remove(T item)
        {
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
            _list.Remove(Find(index));
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
