using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class CartesianTree<TNode, TKey> : BinarySearchTree<TNode, TKey>, IPriorityQueue<TNode> where TNode : BinaryTreeNode, IKeyedNode<TKey>
    {
        #region fields and ctor.
        IComparer<TNode> _priorityComparer;
        public IComparer<TNode> PriorityComparer => _priorityComparer;

        public CartesianTree(Comparison<TNode> priorityComparison) : this(new ComparerWrapper<TNode>(priorityComparison)) { }
        public CartesianTree(IComparer<TNode> priorityComparer) : this(null, priorityComparer) { }
        public CartesianTree(IEnumerable<TNode> initNodesSorted, Comparison<TNode> priorityComparison) : this(initNodesSorted, new ComparerWrapper<TNode>(priorityComparison)) { }
        public CartesianTree(IEnumerable<TNode> initNodesSorted, IComparer<TNode> priorityComparer) : this(initNodesSorted, priorityComparer, Comparer<TKey>.Default) { }
        public CartesianTree(IComparer<TNode> priorityComparer, IComparer<TKey> keyComparer) : this(null, priorityComparer, keyComparer) { }
        /// <summary>
        /// Build Cartesian Tree by initial nodes sorted by key, priority comparer and key comparer
        /// 根据初始数据和关键字、权值比较器构造笛卡尔树
        /// </summary>
        /// <param name="initNodesKeySorted">initial nodes sorted by key 按照关键字已经排好序的结点</param>
        /// <param name="priorityComparer">权重比较器</param>
        /// <param name="keyComparer">关键字比较器</param>
        public CartesianTree(IEnumerable<TNode> initNodesKeySorted, IComparer<TNode> priorityComparer, IComparer<TKey> keyComparer) : base(keyComparer)
        {
            if (priorityComparer == null) throw new ArgumentNullException("优先级比较器不能为空");
            _priorityComparer = priorityComparer;

            if (initNodesKeySorted != null)
            {
                var iter = initNodesKeySorted.GetEnumerator();
                if (!iter.MoveNext()) return;

                TNode stacktail = iter.Current;
                Root = stacktail;
                var lastkey = iter.Current.Key;
                while (iter.MoveNext())
                {
                    // check if data is key-ordered.
                    if (keyComparer.Compare(lastkey, iter.Current.Key) > 0) throw new ArgumentException("输入的原始集合没有按照关键字进行排序");

                    // find insert position
                    while (stacktail != null)
                        if (_priorityComparer.Compare(iter.Current, stacktail) < 0)
                            stacktail = stacktail.Parent as TNode;
                        else break;
                    
                    // insert
                    var c = iter.Current;
                    if (stacktail != null)
                    {
                        c.LeftChild = stacktail.RightChild;
                        stacktail.RightChild = c;
                    }
                    else
                    {
                        c.LeftChild = Root;
                        Root = c;
                    }

                    // update stack
                    stacktail = c;
                }
            }
        }
        #endregion

        /// <summary>
        /// merge two subtree, all keys of <paramref name="left"/> should be smaller than <paramref name="right"/>
        /// 合并两个子树，子树必须满足<paramref name="left"/>的键值均比<paramref name="right"/>小
        /// </summary>
        /// <param name="left">键值较小的子树</param>
        /// <param name="right">键值较大的子树</param>
        /// <returns>合并后的子树</returns>
        private BinaryTreeNode MergeSub(BinaryTreeNode left, BinaryTreeNode right)
        {
            if (left as TNode == null)
                return right;
            if (right as TNode == null)
                return left;
            if (_priorityComparer.Compare(left as TNode, right as TNode) > 0)
            {
                right.LeftChild.SearchDown();
                right.LeftChild = MergeSub(left, right.LeftChild);
                right.LeftChild.SearchUp();
                return right;
            }
            else
            {
                left.RightChild.SearchDown();
                left.RightChild = MergeSub(left.RightChild, right);
                left.RightChild.SearchUp();
                return left;
            }
        }

        protected override void DeleteInternal(TNode node)
        {
            Transplant(node, MergeSub(node.LeftChild, node.RightChild));
        }

        public override TNode Insert(TNode node)
        {
            var res = base.Insert(node);
            if (res == node) // not already exist
                PercolateUp(res);
            return res;
        }

        protected void PercolateUp(TNode target)
        {
            TNode c = target;
            TNode p = c.Parent as TNode;
            while (p != null && _priorityComparer.Compare(p, c) > 0)
            {
                if (c == p.LeftChild) c.Zig();
                else c.Zag();
                p = (c = p).Parent as TNode;
            }
        }
        /// <remarks>
        /// A naive and complicated implementation of percolating down.
        /// 下滤的复杂暴力写法
        /// </remarks>
        protected void PercolateDown(TNode target)
        {
            TNode c = target;
            while (true)
            {
                TNode l = c.LeftChild as TNode, r = c.RightChild as TNode;
                // Primitive cases
                if (l == null)
                {
                    if (r != null && _priorityComparer.Compare(c, r) > 0)
                        r.Zag(); // exchange c with r.
                    break;
                }
                else if (r == null)
                {
                    if (_priorityComparer.Compare(c, l) > 0)
                        l.Zig(); // exchange c with l.
                    break;
                }
                // General cases
                else if (_priorityComparer.Compare(c, l) > 0)
                    if (_priorityComparer.Compare(c, r) > 0)
                    {
                        c.Parent.SearchDown();
                        c.SearchDown();
                        l.SearchDown();
                        r.SearchDown();
                        // c is in the middle
                        c.RightChild = r.LeftChild;
                        c.LeftChild = l.RightChild;

                        if (_priorityComparer.Compare(l, r) > 0)
                        {
                            /* c>l>r, Transform into
                             *   r
                             *  /
                             * l
                             *  \
                             *   c
                             */
                            c.TransplantParent(r);
                            l.RightChild = c;
                            r.LeftChild = l;

                            c.SearchUp();
                            l.SearchUp();
                            r.SearchUp();
                        }
                        else
                        {
                            /* c>r>l, Transform into
                             * l
                             *  \
                             *   r
                             *  /
                             * c
                             */
                            c.TransplantParent(l);
                            l.RightChild = r;
                            r.LeftChild = c;

                            c.SearchUp();
                            r.SearchUp();
                            l.SearchUp();
                        }
                    }
                    else
                    {
                        /* r>c>l Transform into
                        * l
                        *  \
                        *   c
                        *    \
                        *     r
                        */
                        c.Parent.SearchDown();
                        c.SearchDown();
                        l.SearchDown();

                        c.LeftChild = l.RightChild;

                        c.TransplantParent(l);
                        l.RightChild = c;

                        c.SearchUp();
                        l.SearchUp();
                    }
                else if (_priorityComparer.Compare(c, r) > 0)
                {
                    /* l>c>r Transform into
                    *     r
                    *    /
                    *   c
                    *  /
                    * l
                    */
                    c.Parent.SearchDown();
                    c.SearchDown();
                    r.SearchDown();

                    c.RightChild = r.LeftChild;

                    c.TransplantParent(r);
                    r.LeftChild = c;

                    c.SearchUp();
                    r.SearchUp();
                }
                else break; // already in order
            }
        }

        #region IPriorityQueue & IMergable Implementation
        public TNode Min() => Root;

        public TNode ExtractMin()
        {
            var res = Root;
            Delete(Root);
            return res;
        }

        void IPriorityQueue<TNode>.Insert(TNode data) => Insert(data);

        /// <remarks>
        /// This implementation use the complicate <see cref="PercolateDown"/>, a easier way to achieve this is to delete <paramref name="data"/> and insert it back again
        /// 这个方法的实现使用了复杂的<see cref="PercolateDown"/>方法，一个更简单的做法是直接删除data再重新插入
        /// </remarks>
        public void PriorityUpdate(TNode data)
        {
            PercolateUp(data);
            PercolateDown(data);
        }
        #endregion
    }
}
