using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract = System.Diagnostics.Contracts.Contract;

namespace System.Collections.Advanced
{
    public class UnrolledLinkedList<TNode, TData> : IList<TData>, IPrintable
        where TNode : UnrolledLinkedListNode<TData>
    {
        private const int _defaultCapacity = 4;
        private const float _splitFactor = 1.4f;

        Func<int, TNode> _new;
        UnrolledLinkedListNode<TData> head;
        int _version = 0;
        int blocksize;

        int _hotindex; //overall index of first element in hot node
        UnrolledLinkedListNode<TData> _hotnode;

        public int Capacity => blocksize * blocksize;

        public UnrolledLinkedList(Func<int, TNode> newNode) : this(newNode, _defaultCapacity) { }
        public UnrolledLinkedList(Func<int, TNode> newNode, int capacity)
        {
            _new = newNode;
            blocksize = (int)Math.Ceiling(_splitFactor * Math.Sqrt(capacity));
        }

        private UnrolledLinkedListNode<TData> Find(ref int index)
        {
            if (index < _hotindex)
            {
                if (index < _hotindex - index) // front -> hotindex
                {
                    _hotindex = index;
                    _hotnode = head;

                    while (index >= _hotnode.Count)
                    {
                        index -= _hotnode.Count;
                        _hotnode = _hotnode.Next;
                    }

                    _hotindex -= index;
                    return _hotnode;
                }
                else // hotindex -> front
                {
                    var temp = index;
                    _hotnode = _hotnode.Previous;

                    index = _hotindex - index;
                    while (index > _hotnode.Count)
                    {
                        index -= _hotnode.Count;
                        _hotnode = _hotnode.Previous;
                    }

                    index = _hotnode.Count - index;
                    _hotindex = temp - index;
                    return _hotnode;
                }
            }
            else
            {
                if (index - _hotindex < Count - index) // hotindex -> end
                {
                    var temp = index;

                    if (_hotnode == null) _hotnode = head;
                    index -= _hotindex;
                    while (index >= _hotnode.Count)
                    {
                        index -= _hotnode.Count;
                        _hotnode = _hotnode.Next;
                    }

                    _hotindex = temp - index;
                    return _hotnode;
                }
                else // end -> hotindex
                {
                    _hotindex = index;
                    _hotnode = head.Previous;

                    index = Count - index;
                    while (index > _hotnode.Count)
                    {
                        index -= _hotnode.Count;
                        _hotnode = _hotnode.Previous;
                    }

                    index = _hotnode.Count - index;
                    _hotindex -= index;
                    return _hotnode;
                }
            }
        }

        struct Enumerator : IEnumerator<TData>
        {
            UnrolledLinkedList<TNode, TData> list;
            UnrolledLinkedListNode<TData> cnode;
            TData current;
            int index;
            int version;

            public Enumerator(UnrolledLinkedList<TNode, TData> list)
            {
                this.list = list;
                cnode = null;
                current = default(TData);
                index = 0;
                version = list._version;
            }

            public TData Current => current;

            object IEnumerator.Current => Current;

            public void Dispose() { }

            public bool MoveNext()
            {
                if (version != list._version)
                    throw new InvalidOperationException("在枚举过程中集合被修改过");

                if (cnode == null)
                {
                    cnode = list.head;
                    if (list.Count == 0) return false;
                    current = cnode.Items[index++];
                    return true;
                }
                else
                {
                    if (index < cnode.Count)
                    {
                        current = cnode.Items[index++];
                        return true;
                    }
                    else
                    {
                        if (cnode.Next == list.head) return false;
                        cnode = cnode.Next;
                        index = 0;
                        current = cnode.Items[index++];
                        return true;
                    }
                }
            }

            public void Reset()
            {
                if (version != list._version)
                    throw new InvalidOperationException("在枚举过程中集合被修改过");
                current = default(TData);
                cnode = null;
            }
        }

        struct BackEnumerator : IEnumerator<TData>
        {
            UnrolledLinkedList<TNode, TData> list;
            UnrolledLinkedListNode<TData> cnode;
            TData current;
            int index;
            int version;

            public BackEnumerator(UnrolledLinkedList<TNode, TData> list)
            {
                this.list = list;
                cnode = null;
                current = default(TData);
                index = 0;
                version = list._version;
            }

            public TData Current => current;

            object IEnumerator.Current => current;

            public void Dispose() { }

            public bool MoveNext()
            {
                if (version != list._version)
                    throw new InvalidOperationException("在枚举过程中集合被修改过");

                if (cnode == null)
                {
                    cnode = list.head.Previous;
                    index = cnode.Count - 1;
                    if (list.Count == 0) return false;
                    current = cnode.Items[index--];
                    return true;
                }
                else
                {
                    if (index >= 0)
                    {
                        current = cnode.Items[index--];
                        return true;
                    }
                    else
                    {
                        if (cnode == list.head) return false;
                        cnode = cnode.Previous;
                        index = cnode.Count - 1;
                        current = cnode.Items[index--];
                        return true;
                    }
                }
            }

            public void Reset()
            {
                if (version != list._version)
                    throw new InvalidOperationException("在枚举过程中集合被修改过");
                current = default(TData);
                cnode = null;
            }
        }

        #region IList & IPrintable implementation

        /// <summary>
        /// Get or set the value at <paramref name="index"/>. If get and set at the same time, use <see cref="OperateAt(int, ActionRef{TData})"/> method to improve efficiency.
        /// 获取或设置列表在<paramref name="index"/>的值，若同时获取和设置，请使用<see cref="OperateAt(int, ActionRef{TData})"/>方法以提高效率。
        /// </summary>
        /// <param name="index">需要获取或设置的对象的索引</param>
        public TData this[int index]
        {
            get
            {
                return Find(ref index).Items[index];
            }
            set
            {
                Find(ref index).Items[index] = value;
            }
        }
        
        public int Count { get; protected set; }

        public bool IsReadOnly => false;

        public void Add(TData item)
        {
            if (Count == 0)
            {
                head = _new(blocksize << 1);
                head.Next = head;
            }
            head.Previous.Insert(head.Previous.Count, item, _new);

            _version++;
            Count++;
        }

        public void Clear()
        {
            head = null;
            Count = 0;
            _hotindex = 0;
            _hotnode = null;
            _version++;
        }

        public bool Contains(TData item) => IndexOf(item) >= 0;

        public void CopyTo(TData[] array, int arrayIndex) => this.CopyTo<TData>(array, arrayIndex);

        public IEnumerator<TData> GetEnumerator(bool backward)
        {
            if (backward)
                return new BackEnumerator(this);
            else
                return new Enumerator(this);
        }
        public IEnumerator<TData> GetEnumerator() => new Enumerator(this);

        public virtual int IndexOf(TData item) => EnumerateIndexOf(item, false);

        public virtual int LastIndexOf(TData item) => EnumerateIndexOf(item, true);

        public virtual int EnumerateIndexOf(TData item, bool backward)
        {
            var itor = GetEnumerator(backward);
            int count = 0;
            if (ReferenceEquals(item, null))
            {
                while (itor.MoveNext())
                {
                    if (ReferenceEquals(itor.Current, null))
                        return backward ? Count - count - 1 : count;
                    count++;
                }
            }
            else
            {
                var comparer = EqualityComparer<TData>.Default;
                while (itor.MoveNext())
                {
                    if (comparer.Equals(itor.Current, item))
                        return backward ? Count - count - 1 : count;
                    count++;
                }
            }
            return -1;
        }

        public void Insert(int index, TData item)
        {
            var loc = Find(ref index);
            loc.Insert(index, item, _new);

            _version++;
            Count++;
        }

        public bool Remove(TData item)
        {
            var index = IndexOf(item);
            if (index < 0) return false;

            RemoveAt(index);
            _version++;
            return true;
        }

        public void RemoveAt(int index)
        {
            var loc = Find(ref index);
            var prev = loc.Previous;
            var prevoffset = loc.Count;

            loc.Delete(index);

            if (!ReferenceEquals(prev.Next, loc)) // Merged with previous node
            {
                _hotindex -= prevoffset;
                _hotnode = prev;
            }

            _version++;
            Count--;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void PrintTo(TextWriter textOut)
        {
            if (Count == 0) textOut.WriteLine("Null Unrolled Linked List");

            var node = head;
            node.PrintTo(textOut);

            while (node.Next != head)
            {
                node = node.Next;
                textOut.Write("->");
                node.PrintTo(textOut);
            }
        }

        #endregion

        #region Range Operations
        #endregion
    }
}
