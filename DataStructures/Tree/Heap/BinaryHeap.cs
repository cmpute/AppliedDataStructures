using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Binary Heap
    /// 二叉堆
    /// </summary>
    /// <typeparam name="TNode">结点上存储的数据类型</typeparam>
    /// <typeparam name="TPrior">优先级关键字的类型</typeparam>
    public class BinaryHeap<TNode, TPrior> : DenseBinaryTree<TNode>, IPriorityQueue<TNode, TPrior> where TNode : IKeyedNode<TPrior>
    {

    }
}
