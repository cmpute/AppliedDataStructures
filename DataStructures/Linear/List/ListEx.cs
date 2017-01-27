using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public static class ListEx
    {
        public static IList<T> AsList<T>(this LinkedList<T> list) => new LinkedListWrapper<T>(list);
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
                index = Count - index;
                var node = _list.First;
                while (index-- > 0)
                    node = node.Previous;
                return node;
            }
        }

        public T this[int index]
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

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public void Add(T item) => _list.AddLast(item);

        public void Clear() => _list.Clear();

        public bool Contains(T item) => IndexOf(item) >= 0;

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
