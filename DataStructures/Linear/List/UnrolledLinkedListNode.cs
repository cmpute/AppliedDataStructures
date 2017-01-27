using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class UnrolledLinkedListNode<TData> : IPersistent, IMergable<UnrolledLinkedListNode<TData>>, IPrintable
    {
        bool _rmref; //whether to remove reference, not necessary for value type

        public TData[] Items { get; protected set; }

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
            Items = new TData[capacity];
#if DEBUG
            _rmref = true;
#else
            _rmref = !typeof(TData).IsValueType;
#endif
        }

        public int Capacity => Items.Length;
        public virtual float MergeRatio => 0.3f;
        public int Count { get; protected set; } = 0;
        public int Version { get; protected set; }

        public void Split(Func<int, UnrolledLinkedListNode<TData>> newNode) => Split(Count >> 1, newNode);
        public void Split(int splitIndex, Func<int, UnrolledLinkedListNode<TData>> newNode)
        {
            var newnext = newNode(Capacity);
            newnext.Next = Next;
            newnext.Previous = this;

            newnext.Count = Count - splitIndex;
            Count = splitIndex;
            Array.Copy(Items, Count, newnext.Items, 0, newnext.Count);
            if(_rmref) Array.Clear(Items, Count, newnext.Count);

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
            if (newNext.Count > 0)
            {
                var copycount = newNext.Count - startIndex;
                Array.Copy(newNext.Items, startIndex, Items, Count, copycount);
                if (_rmref) Array.Clear(newNext.Items, 0, startIndex);
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
                Array.Copy(Items, index, Items, index + 1, Count - index);
            Items[index] = item;
            Count++;

            Update();
        }

        public TData Delete(int index)
        {
            var res = Items[index];
            if (index < --Count)
            {
                Array.Copy(Items, index + 1, Items, index, Count - index);
                if (_rmref) Items[Count] = default(TData);
            }

            if (!FixDown())
                Update();

            return res;
        }

        private bool FixDown()
        {
            bool fix = false;
            if (Count < Capacity * MergeRatio)
            {
                if (Next != this && Count + Next.Count <= Capacity)
                {
                    Merge(Next);
                    fix = true;
                }
                if (Previous != this && Count + Previous.Count <= Previous.Capacity)
                {
                    Previous.Merge(this);
                    fix = true;
                }
            }

            return fix;
        }

        protected virtual void Update() { Version++; }

        public void PrintTo(TextWriter textOut)
        {
            if (Count == 0) textOut.Write("[null node]");

            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            sb.Append(Items[0]);

            for (int i = 1; i < Count; i++)
            {
                sb.Append(' ');
                sb.Append(Items[i]);
            }
            sb.Append(']');

            textOut.Write(sb.ToString());
        }
    }
}
