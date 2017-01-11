using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced.Tree.BinaryTree
{
    public class CartesianTree<TNode, TKey> : BinarySearchTree<TNode, TKey>, IPriorityQueue<TNode> where TNode : BinaryTreeNode, IKeyedNode<TKey>
    {
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

        protected override void DeleteInternal(TNode node)
        {
            base.DeleteInternal(node);
        }

        protected override TNode SearchInternal(TKey key, bool modify)
        {
            return base.SearchInternal(key, modify);
        }

        #region IPriorityQueue Implementation
        public TNode Min() => Root;

        public TNode ExtractMin()
        {
            throw new NotImplementedException();
        }

        void IPriorityQueue<TNode>.Insert(TNode data)
        {
            throw new NotImplementedException();
        }

        public void PriorityUpdate(TNode data)
        {
            throw new NotImplementedException();
        }

        public IPriorityQueue<TNode> Merge(IPriorityQueue<TNode> another)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
