using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace System.Collections.Advanced
{
    public class UnrolledLinkedListNode<TData> : IPersistent, IMergable<UnrolledLinkedListNode<TData>>, IPrintable
    {
        bool _rmref; //whether to remove reference, not necessary for value type
        
        internal TData[] _items;
        protected TData[] RawItems { get { return _items; } set { _items = value; } }

        UnrolledLinkedListNode<TData> _prev, _next;
        public UnrolledLinkedListNode<TData> Previous
        {
            get { return _prev; }
            set
            {
                _prev = value;
                if (value != null) value._next = this;
            }
        }
        public UnrolledLinkedListNode<TData> Next
        {
            get { return _next; }
            set
            {
                _next = value;
                if (value != null) value._prev = this;
            }
        }

        public UnrolledLinkedListNode(int capacity)
        {
            _items = new TData[capacity];
#if DEBUG
            _rmref = true;
#else
            _rmref = !typeof(TData).IsValueType;
#endif
        }

        public int Capacity => _items.Length;
        public int Count { get; protected set; }
        public int Version { get; protected set; }

        public void Split(Func<int, UnrolledLinkedListNode<TData>> newNode) => Split(Count >> 1, newNode);
        public void Split(int splitIndex, Func<int, UnrolledLinkedListNode<TData>> newNode)
        {
            if (splitIndex == Count)
                return;

            var newnext = newNode(Capacity);
            newnext.Next = Next;
            newnext.Previous = this;

            newnext.Count = Count - splitIndex;
            Count = splitIndex;
            Array.Copy(_items, Count, newnext._items, 0, newnext.Count);
            if(_rmref) Array.Clear(_items, Count, newnext.Count);

            newnext.Update();
        }

        public void Merge(UnrolledLinkedListNode<TData> newNext) => Merge(newNext, 0);
        /// <summary>
        /// Merge current node with <paramref name="newNext"/>, the nodes between this and <paramref name="newNext"/> are abandoned.
        /// 合并当前结点与<paramref name="newNext"/>， 之间的结点将会被抛弃
        /// </summary>
        /// <param name="newNext">需要被合并的结点，请保证它是当前结点的后继</param>
        /// <param name="startIndex">合并的结点开始合并的数据位置</param>
        public void Merge(UnrolledLinkedListNode<TData> newNext, int startIndex)
        {
            if (newNext == this) return;

            if (newNext.Count > 0)
            {
                var copycount = newNext.Count - startIndex;
                Array.Copy(newNext._items, startIndex, _items, Count, copycount);
                if (_rmref) Array.Clear(newNext._items, 0, startIndex);
                Count += copycount;
            }

            Next = newNext.Next;

            Update();
        }

        public void Insert(int index, TData item, Func<int, UnrolledLinkedListNode<TData>> newNext)
        {
            if (Count == Capacity)
            {
                Split(newNext);
                if (index > Count)
                {
                    Next.Insert(index - Count, item, newNext);
                    return;
                }
            }
            
            if (index < Count)
                Array.Copy(_items, index, _items, index + 1, Count - index);
            _items[index] = item;
            Count++;

            Update();
        }

        ///<param name="head">链表的头节点，删除过程中将保证head不被Merge</param>
        public TData Delete(int index)
        {
            var res = _items[index];
            if (index < --Count)
            {
                Array.Copy(_items, index + 1, _items, index, Count - index);
                if (_rmref) _items[Count] = default(TData);
            }
            
            Update();

            return res;
        }

        public void Delete(int index, int count)
        {
            Count -= count;
            if (index < Count)
                Array.Copy(_items, index + count, _items, index, Count - index);
            if (_rmref) Array.Clear(_items, Count, count);

            Update();
        }

        public void Clear()
        {
            if (Count > 0 && _rmref)
            {
                Array.Clear(_items, 0, Count);
                Count = 0;
            }
            Version++;
        }

        protected virtual void Update() { Version++; }

        public void PrintTo(TextWriter textOut)
        {
            textOut.Write('[');
            _items.Take(Count).PrintTo(textOut);
            textOut.Write(']');
        }
    }
}
