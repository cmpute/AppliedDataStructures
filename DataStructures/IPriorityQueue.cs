using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// a.k.a Heap, capable of returning min value with IComparer
    /// 亦称Heap，能够在给定比较器的情况下返回最小值
    /// </summary>
    /// <typeparam name="TValue">
    /// Type of values stored in PQ
    /// 堆上存储的数据类型</typeparam>
    /// <remarks>
    /// MaxHeap can be turned into MinHeap by changing IComparer
    /// 最大堆可以通过改变比较方式变成最小堆
    /// </remarks>
    public interface IPriorityQueue<TValue>
    {
        /// <summary>
        /// The same as<see cref="Queue{T}.Peek"/>. Return the minimum in the queue
        /// 类似于<see cref="Queue{T}.Peek"/>，返回优先队列的最小值
        /// </summary>
        /// <returns>队列中的最小值</returns>
        TValue Min();
        /// <summary>
        /// The same as <see cref="Queue{T}.Dequeue"/>. Extract the minimun from the queue
        /// 类似于<see cref="Queue{T}.Dequeue"/>. 从队列中取出最小值
        /// </summary>
        /// <returns></returns>
        TValue ExtractMin();
        /// <summary>
        /// The same as<see cref="Queue{T}.Enqueue(T)"/>. Enqueue a new value.
        /// 类似于<see cref="Queue{T}.Enqueue(T)"/>，入列一个新值
        /// </summary>
        /// <param name="data">要插入的值</param>
        void Insert(TValue data);
        /// <summary>
        /// Update the priority of node <paramref name="data"/>, and inform heap of updating
        /// 更新队列中<paramref name="data"/>结点的优先级后通知队列进行更新
        /// </summary>
        /// <param name="data">需要更新的结点</param>
        void PriorityUpdate(TValue data);
        /// <summary>
        /// Merge two Priority Queues
        /// 合并两个堆
        /// </summary>
        /// <param name="another">另一个优先级队列</param>
        /// <returns>合并后的队</returns>
        IPriorityQueue<TValue> Merge(IPriorityQueue<TValue> another);
    }
}
