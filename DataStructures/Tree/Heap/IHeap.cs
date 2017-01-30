using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced.Tree
{
    /// <summary>
    /// Heap, tree-based priority queue
    /// 堆，即优先级队列树
    /// </summary>
    /// <typeparam name="TPriority">优先级的类型</typeparam>
    /// <typeparam name="TNode">结点类型</typeparam>
    public interface IHeap<TPriority, TNode> : IRootedTree<TNode>, IPriorityQueue<TPriority, TNode>
    {
    }
}
