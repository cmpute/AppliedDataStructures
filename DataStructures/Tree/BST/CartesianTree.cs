using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced.Tree
{
    /// <summary>
    /// Cartesian Tree, randomized(balanced) Binary Search Tree
    /// 笛卡尔树， 一种随机(平衡)二叉树
    /// </summary>
    /// <typeparam name="TNode">结点的类型</typeparam>
    /// <typeparam name="TKey">比较的关键字</typeparam>
    /// <remarks>
    /// Cartesian Tree is based on <see cref="Treap{TNode, TKey}"/>, it make the priority of every node a random number, thus makes the tree balanced.
    /// 笛卡尔树基于树堆，它使每一个结点的权重为一个随机数来使得二叉树平衡。
    /// </remarks>
    public class CartesianTree<TNode, TKey> : Treap<TNode, TKey, int> where TNode : CartesianTreeNode<TKey>
    {
        public CartesianTree() : this(Comparer<TKey>.Default) { }
        public CartesianTree(IEnumerable<TNode> initNodesSorted) : this(initNodesSorted, Comparer<TKey>.Default) { }
        public CartesianTree(IComparer<TKey> comparer) : this(null, comparer) { }
        /// <summary>
        /// Build Cartesian Tree by the key comparer and initial nodes sorted by key comparer
        /// 根据初始数据和关键字比较器构造笛卡尔树
        /// </summary>
        /// <param name="initNodesSorted">initial nodes sorted by key 按照关键字已经排好序的结点</param>
        /// <param name="comparer">关键字比较器</param>
        public CartesianTree(IEnumerable<TNode> initNodesSorted, IComparer<TKey> comparer) : base(initNodesSorted, Comparer<int>.Default, comparer) { }
    }
}
