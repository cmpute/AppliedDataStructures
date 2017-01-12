using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Treap, a kind of randomized(balanced) Binary Search Tree
    /// 树堆， 一种随机(平衡二叉树)
    /// </summary>
    /// <typeparam name="TNode">结点的类型</typeparam>
    /// <typeparam name="TKey">比较的关键字</typeparam>
    /// <remarks>
    /// Treap is based on <see cref="CartesianTree{TNode, TKey}"/>, it make the priority of every node a random number, thus makes the tree balanced.
    /// 树堆基于笛卡尔树，它使每一个结点的权重为一个随机数来使得二叉树平衡。
    /// </remarks>
    public class Treap<TNode, TKey> : CartesianTree<TNode, TKey> where TNode : TreapNode<TKey>
    {
        public Treap() : this(Comparer<TKey>.Default) { }
        public Treap(IEnumerable<TNode> initNodesSorted) : this(initNodesSorted, Comparer<TKey>.Default) { }
        public Treap(IComparer<TKey> comparer) : this(null, comparer) { }
        /// <summary>
        /// Build Cartesian Tree by the key comparer and initial nodes sorted by key comparer
        /// 根据初始数据和关键字比较器构造树堆
        /// </summary>
        /// <param name="initNodesSorted">initial nodes sorted by key 按照关键字已经排好序的结点</param>
        /// <param name="comparer">关键字比较器</param>
        public Treap(IEnumerable<TNode> initNodesSorted, IComparer<TKey> comparer) : base(initNodesSorted, (node1, node2) => node1.Priority - node2.Priority, comparer) { }
    }
}
