using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract = System.Diagnostics.Contracts.Contract;

namespace System.Collections.Advanced
{
    //TODO: Accomplish auto size-increment in unrolled linked list ( using Array.Resize)
    public class UnrolledLinkedList<TNode, TData> : IRangeList<TData>, IPrintable, IPersistent
        where TNode : UnrolledLinkedListNode<TData>
    {
        private const int _minCapacity = 4;
        private const float _splitFactor = 1.414f; // √2
        private const float _mergeFactor = 0.354f; // 1/2√2

        Func<int, TNode> _new;
        UnrolledLinkedListNode<TData> head;
        int blocksize;

        int _hotindex; //overall index of first element in hot node
        UnrolledLinkedListNode<TData> _hotnode;
#if DEBUG
        public bool VerifyHotNode()
        {
            if (_hotindex > Count) return false;
            var index = _hotindex;
            var node = head;
            while (index >= node.Count)
            {
                index -= node.Count;
                node = node.Next;
            }
            return index == 0 && node == (_hotnode ?? head);
        }
#endif

        public int Capacity => blocksize * blocksize;
        public int Version { get; protected set; }
        public virtual float SplitFactor
        {
            get
            {
                Contract.Ensures(Contract.Result<float>() > 1f);
                return _splitFactor;
            }
        }
        public virtual float MergeFactor
        {
            get
            {
                Contract.Ensures(Contract.Result<float>() <= 0.5f);
                return _mergeFactor;
            }
        }

        public UnrolledLinkedList(Func<int, TNode> newNode) : this(newNode, _minCapacity) { }
        public UnrolledLinkedList(Func<int, TNode> newNode, int capacity)
        {
            if (capacity < _minCapacity)
                capacity = _minCapacity;

            _new = newNode;
            blocksize = (int)Math.Ceiling(_splitFactor * Math.Sqrt(capacity));
            head = _new(blocksize);
            head.Next = head;
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

        private void FixNode(UnrolledLinkedListNode<TData> node)
        {
            if (node.Count < node.Capacity * MergeFactor)
            {
                if (node.Next != head && node.Count + node.Next.Count <= node.Capacity)
                {
                    if (node.Next == _hotnode) // update hot
                    {
                        _hotindex -= node.Count;
                        _hotnode = node;
                    }
                    node.Merge(node.Next);
                }
                if (node != head && node.Count + node.Previous.Count <= node.Previous.Capacity)
                {
                    if (node == _hotnode) // update hot
                    {
                        _hotnode = node.Previous;
                        _hotindex -= _hotnode.Count;
                    }
                    node.Previous.Merge(node);
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
                version = list.Version;
            }

            public TData Current => current;

            object IEnumerator.Current => Current;

            public void Dispose() { }

            public bool MoveNext()
            {
                if (version != list.Version)
                    throw new InvalidOperationException("在枚举过程中集合被修改过");

                if (cnode == null)
                {
                    cnode = list.head;
                    if (list.Count == 0) return false;
                    current = cnode._items[index++];
                    return true;
                }
                else
                {
                    if (index < cnode.Count)
                    {
                        current = cnode._items[index++];
                        return true;
                    }
                    else
                    {
                        if (cnode.Next == list.head) return false;
                        cnode = cnode.Next;
                        index = 0;
                        current = cnode._items[index++];
                        return true;
                    }
                }
            }

            public void Reset()
            {
                if (version != list.Version)
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
                version = list.Version;
            }

            public TData Current => current;

            object IEnumerator.Current => current;

            public void Dispose() { }

            public bool MoveNext()
            {
                if (version != list.Version)
                    throw new InvalidOperationException("在枚举过程中集合被修改过");

                if (cnode == null)
                {
                    cnode = list.head.Previous;
                    index = cnode.Count - 1;
                    if (list.Count == 0) return false;
                    current = cnode._items[index--];
                    return true;
                }
                else
                {
                    if (index >= 0)
                    {
                        current = cnode._items[index--];
                        return true;
                    }
                    else
                    {
                        if (cnode == list.head) return false;
                        cnode = cnode.Previous;
                        index = cnode.Count - 1;
                        current = cnode._items[index--];
                        return true;
                    }
                }
            }

            public void Reset()
            {
                if (version != list.Version)
                    throw new InvalidOperationException("在枚举过程中集合被修改过");
                current = default(TData);
                cnode = null;
            }
        }

        #region IList & IPrintable implementation

        /// <summary>
        /// Get or set the value at <paramref name="index"/>.
        /// 获取或设置列表在<paramref name="index"/>的值
        /// </summary>
        /// <param name="index">需要获取或设置的对象的索引</param>
        public TData this[int index]
        {
            get
            {
                return Find(ref index)._items[index];
            }
            set
            {
                Find(ref index)._items[index] = value;
            }
        }
        
        public int Count { get; protected set; }

        public bool IsReadOnly => false;

        public void Add(TData item)
        {
            head.Previous.Insert(head.Previous.Count, item, _new);

            Version++;
            Count++;
        }

        public void Clear()
        {
            head = _new(blocksize);
            head.Next = head;
            Count = 0;
            _hotindex = 0;
            _hotnode = null;
            Version++;
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

            Version++;
            Count++;
        }

        public bool Remove(TData item)
        {
            var index = IndexOf(item);
            if (index < 0) return false;

            RemoveAt(index);
            Version++;
            return true;
        }

        public void RemoveAt(int index)
        {
            var loc = Find(ref index);

            loc.Delete(index); // do not merge head with head.previous

            FixNode(loc);

            Version++;
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

#if DEBUG
            textOut.Write($", hot at [{_hotindex}]:");
            _hotnode?.PrintTo(textOut);
#endif
        }

        #endregion

        #region Range Operations

        public void AddRange(IEnumerable<TData> collection)
        {
            var itor = collection.GetEnumerator();
            var node = head.Previous;

            while (itor.MoveNext())
            {
                if (node.Count >= node.Capacity)
                {
                    node.Next = new UnrolledLinkedListNode<TData>(blocksize);
                    node = node.Next;
                    head.Previous = node;
                }

                node.Insert(node.Count, itor.Current, _new); // Append to last node
                Count++;
            }

            Version++;
        }

        public void InsertRange(int index, IEnumerable<TData> collection)
        {
            var itor = collection.GetEnumerator();
            var node = Find(ref index);

            node.Split(index, _new);
            var next = node.Next;

            while (itor.MoveNext())
            {
                if (node.Count >= node.Capacity)
                {
                    node.Next = new UnrolledLinkedListNode<TData>(blocksize);
                    node = node.Next;
                    next.Previous = node;
                }

                node.Insert(node.Count, itor.Current, _new); // Append to last node
                Count++;
            }

            Version++;
        }

        public void RemoveRange(int index, int count)
        {
            // find end first to make hot node is located at start.
            var end = index + count;
            var endnode = Find(ref end);
            var start = index;
            var startnode = Find(ref start);

            if (ReferenceEquals(startnode, endnode))
            {
                startnode.Delete(start, count);
            Count -= count;
                return;
            }

            endnode.Split(end, _new); // split last node

            startnode.Delete(start, startnode.Count - start); // remove from tail of first node
            var next = startnode.Next;
            startnode.Next = endnode.Next; //remove chain

            if (!typeof(TData).IsValueType)
            {
                while (!ReferenceEquals(next, endnode)) //clear chain
                {
                    next.Clear();
                    next = next.Next;
                }
                endnode.Clear();
            }

            FixNode(startnode);
            Count -= count;
            Version++;
        }

        public void Reverse(int index, int count)
        {
            throw new NotImplementedException();
        }

        public void Reverse()
        {
            throw new NotImplementedException();
        }

        public IList<TData> GetRange(int index, int count)
        {
            var start = index;
            var startnode = Find(ref start);
            var end = index + count;
            var endnode = Find(ref end);

            if (ReferenceEquals(startnode, endnode))
            {
                var res = new TData[count];
                Array.Copy(startnode._items, start, res, 0, count);
                return res.ToList();
            }

            List<TData> result = new List<TData>(count);
            for (int i = start; i < startnode.Count; i++)
                result.Add(startnode._items[i]);

            startnode = startnode.Next;
            while (!ReferenceEquals(startnode, endnode))
            {
                result.AddRange(startnode._items.Take(startnode.Count));
                startnode = startnode.Next;
            }
            result.AddRange(endnode._items.Take(end));
            
            return result;
        }

        #endregion
    }
}
