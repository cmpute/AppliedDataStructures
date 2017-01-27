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

        Func<int, TNode> _new;
        UnrolledLinkedListNode<TData> head;
        int _version;
        int blocksize;

        public int Capacity => blocksize * blocksize;

        public UnrolledLinkedList(Func<int, TNode> newNode) : this(newNode, _defaultCapacity) { }
        public UnrolledLinkedList(Func<int, TNode> newNode, int capacity)
        {
            _new = newNode;
            blocksize = (int)Math.Ceiling(Math.Sqrt(capacity));
        }

        private Tuple<UnrolledLinkedListNode<TData>, int> Find(int index)
        {
            if (index <= Count - index) // Find orderly faster
            {
                var current = head;
                while (index >= current.Count)
                {
                    index -= current.Count;
                    current = current.Next;
                }
                return new Tuple<UnrolledLinkedListNode<TData>, int>(current, index);
            }
            else // Find reversely faster
            {
                index = Count - index;
                var current = head.Previous;
                while (index > current.Count)
                {
                    index -= current.Count;
                    current = current.Previous;
                }
                return new Tuple<UnrolledLinkedListNode<TData>, int>(current, current.Count - index);
            }
        }

        public void OperateAt(int index, ActionRef<TData> operation)
        {
            var res = Find(index);
            var data = res.Item1.Items[res.Item2];
            operation(ref data);
            res.Item1.Items[res.Item2] = data;
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
                var res = Find(index);
                return res.Item1.Items[res.Item2];
            }
            set
            {
                var res = Find(index); // can be improved by add log of last operation
                res.Item1.Items[res.Item2] = value;
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

            Count++;
        }

        public void Clear()
        {
            head = null;
            Count = 0;
            _version++;
        }

        public bool Contains(TData item) => IndexOf(item) >= 0;

        public void CopyTo(TData[] array, int arrayIndex) => this.CopyTo<TData>(array, arrayIndex);

        public IEnumerator<TData> GetEnumerator(bool reverse)
        {
            if (reverse)
                return new BackEnumerator(this);
            else
                return new Enumerator(this);
        }
        public IEnumerator<TData> GetEnumerator() => new Enumerator(this);

        public virtual int IndexOf(TData item) => EnumerateIndexOf(item, false);

        public virtual int LastIndexOf(TData item) => EnumerateIndexOf(item, true);

        public virtual int EnumerateIndexOf(TData item, bool reverse)
        {
            var itor = GetEnumerator(reverse);
            int count = 0;
            if (ReferenceEquals(item, null))
            {
                while (itor.MoveNext())
                {
                    if (ReferenceEquals(itor.Current, null))
                        return reverse ? Count - count - 1 : count;
                    count++;
                }
            }
            else
            {
                var comparer = EqualityComparer<TData>.Default;
                while (itor.MoveNext())
                {
                    if (comparer.Equals(itor.Current, item))
                        return reverse ? Count - count - 1 : count;
                    count++;
                }
            }
            return -1;
        }

        public void Insert(int index, TData item)
        {
            var loc = Find(index);
            loc.Item1.Insert(loc.Item2, item, _new);
            Count++;
        }

        public bool Remove(TData item)
        {
            var index = IndexOf(item);
            if (index < 0) return false;

            RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            var loc = Find(index);
            loc.Item1.Delete(loc.Item2);
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
