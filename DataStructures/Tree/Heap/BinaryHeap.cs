﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced.Tree
{
    /// <summary>
    /// Binary Heap, a most succinct implementation of heap
    /// 二叉堆，堆的最精炼的实现
    /// </summary>
    /// <typeparam name="TValue">结点上存储的数据类型
    /// <para>如果<typeparamref name="TValue"/>实现了<see cref="IIndexedHeapNode"/>接口，那么<see cref="PriorityUpdate(TValue)"/>操作会进行得更快</para>
    /// </typeparam>
    /// <typeparam name="TPrior">优先级关键字的类型</typeparam>
    public class BinaryHeap<TValue> : CompactTree<TValue>, IHeap<TValue, TValue>, IMergable<IPriorityQueue<TValue, TValue>>
    {
        IComparer<TValue> _comparer;
        public IComparer<TValue> PriorityComparer => _comparer;

        public BinaryHeap() : this(Comparer<TValue>.Default) { }
        public BinaryHeap(IComparer<TValue> priorityComparer)
        {
            Nodes = new List<TValue>();
            _comparer = priorityComparer ?? Comparer<TValue>.Default;
        }
        /// <summary>
        /// Initialize a heap from a copy of <paramref name="data"/>.
        /// 用<paramref name="data"/>的一个副本作为初始数据构造一个堆。
        /// </summary>
        /// <param name="data">初始数据</param>
        /// <remarks>如果想要在原数据上进行操作，请使用<see cref="Heapify"/>方法</remarks>
        public BinaryHeap(IEnumerable<TValue> data) : this(data, Comparer<TValue>.Default) { }
        public BinaryHeap(IEnumerable<TValue> data, IComparer<TValue> priorityComparer)
        {
            Nodes = data.ToList();
            _comparer = priorityComparer ?? Comparer<TValue>.Default;
            Heapify();
        }

        /// <summary>
        /// Do swaps in the list in order to make the list fit the rules of heap.Heapify will make a heap on the <paramref name="target"/> instead of making a copy of it.
        /// 对数组进行移动，使之满足最小堆的性质。该方法直接对<paramref cref="target"/>进行操作，而不是拷贝副本。
        /// </summary>
        /// <param name="target">需要堆化的数组</param>
        public static BinaryHeap<TValue> Heapify(IList<TValue> target)
        {
            Contract.Requires<ArgumentNullException>(target != null);
            return Heapify(target, Comparer<TValue>.Default);
        }
        public static BinaryHeap<TValue> Heapify(IList<TValue> target, IComparer<TValue> comparer)
        {
            Contract.Requires<ArgumentNullException>(target != null);

            BinaryHeap <TValue> heap = new BinaryHeap<TValue>();
            heap.Nodes = target;
            heap._comparer = comparer ?? Comparer<TValue>.Default;
            heap.Heapify();
            return heap;
        }
        /// <summary>
        /// Reconstruct heap, and makes it fit the rules
        /// 重新调整堆，使得堆恢复堆的性质
        /// </summary>
        public void Heapify()
        {
            for (int i = Count >> 1; i >= 0; i--)
                PercolateDown(i);
        }

        public TValue ExtractMin()
        {
            Contract.Requires<InvalidOperationException>(Count > 0);

            var min = Root;
            Nodes[0] = Nodes[Count - 1];
            Nodes.RemoveAt(Count - 1);

            if (Count == 0) return min;
            UpdateIndex(0);
            PercolateDown(0);
            return min;
        }

        public void Insert(TValue data)
        {
            Contract.Requires<ArgumentNullException>(data != null);

            Nodes.Add(data);
            UpdateIndex(Count - 1);
            PercolateUp(Count - 1);
        }

        public void Merge(IPriorityQueue<TValue, TValue> another)
        {
            if (another == null) return;

            foreach (var node in another)
                Nodes.Add(node);
            Heapify();
        }

        public TValue Min() => Root;
         
        public void PriorityUpdate(TValue data)
        {
            Contract.Requires<ArgumentNullException>(data != null);
            Contract.Requires<ArgumentNullException>(this.Contains(data));

            int loc = (data as IIndexedHeapNode)?.IndexInHeap ?? Nodes.IndexOf(data);
            PercolateUp(loc);
            PercolateDown(loc);
        }

        private void PercolateUp(int loc)
        {
            int current = loc, parent = current >> 1;
            while (current > 0)
            {
                if (_comparer.Compare(Nodes[parent], Nodes[current]) > 0)
                    SwapNode(current, parent);
                else break;
                current = parent;
                parent >>= 1;
            }
        }

        private void PercolateDown(int loc)
        {
            int current = loc, min = current << 1;
            if (min >= Count) return; // No children, else the minimum is left child.
            if ((min | 1) < Count) //if has right child
                if (_comparer.Compare(Nodes[min], Nodes[min | 1]) > 0) // and right child is smaller
                    min |= 1; // then right child is the minimum
            if (_comparer.Compare(Nodes[current], Nodes[min]) > 0) // children is smaller
            {
                SwapNode(current, min);
                PercolateDown(min);
            }
        }

        private void SwapNode(int i, int j)
        {
            var temp = Nodes[i];
            Nodes[i] = Nodes[j];
            Nodes[j] = temp;
            UpdateIndex(i);
            UpdateIndex(j);
        }

        private void UpdateIndex(int loc)
        {
            if (Nodes[loc] is IIndexedHeapNode)
                (Nodes[loc] as IIndexedHeapNode).IndexInHeap = loc;
        }
    }
}
