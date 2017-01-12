using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Binary Search Tree (BST)
    /// 二叉搜索树
    /// </summary>
    /// <remarks>
    /// Insert and Delete implementations are naive ones
    /// </remarks>
    public class BinarySearchTree<TNode, TKey> : BinaryTree<TNode>, ISearchTreeEquatable<TNode, TKey> where TNode : BinaryTreeNode, IKeyedNode<TKey>
    {
        IComparer<TKey> _comparer;
        Random _rand = new Random();

        public BinarySearchTree() : this(Comparer<TKey>.Default) { }
        public BinarySearchTree(IComparer<TKey> comparer)
        {
            _comparer = comparer ?? Comparer<TKey>.Default;
        }

        /// <summary>
        /// 是否支持<see cref="ISearchTreeEquatable{TNode, TKey}"/>的操作
        /// </summary>
        public virtual bool SupportEquatable { get; set; } = false;

        /// <summary>
        /// 当元素雷同时，是否在中序遍历的意义下保持插入的顺序（先插入者在前），并且搜索时返回第一个插入的元素
        /// 该属性仅当<see cref="SupportEquatable"/>为true时生效
        /// </summary>
        public bool KeepInsertOrder { get; set; } = true;

        public IComparer<TKey> KeyComparer => _comparer;

        /// <summary>
        /// 用<paramref name="replace"/>子树替换<paramref name="target"/>结点
        /// </summary>
        /// <param name="target">被替换结点</param>
        /// <param name="replace">替换的结点</param>
        protected void Transplant(BinaryTreeNode target, BinaryTreeNode replace)
        {
            if (target == replace) return;

            target.Parent.SearchDown();
            target.TransplantParent(replace);
            replace.SearchUp();
        }

        /// <summary>
        /// Internal Search Method
        /// 内部调用的Search函数
        /// </summary>
        /// <param name="modify">是否在下行过程中调用OnSearchDown</param>
        protected virtual TNode SearchInternal(TKey key, bool modify)
        {
            TNode current = Root, ret = null;
            while (current != null)
            {
                int result = _comparer.Compare(current.Key, key);
                if (result == 0)
                    if (!SupportEquatable || !KeepInsertOrder)
                        return current;
                    else
                        ret = current;

                if (modify) current.SearchDown();

                if (result < 0)
                    current = current.RightChild as TNode; //search into right subtree
                else // keep left first when keep insert order
                    current = current.LeftChild as TNode; //search into left subtree
            }
            return ret;
        }

        public TNode Search(TKey key) => SearchInternal(key, false);

        public virtual TNode Insert(TNode node)
        {
            if (Root == null)
            {
                //null tree
                Root = node;
                Count++;
                return node;
            }

            TNode current = Root;
            int result = -1;
            while (current != null)
            {
                result = _comparer.Compare(current.Key, node.Key);
                if (result == 0)
                    if (!SupportEquatable)
                        return current;
                current.SearchDown();

                if (result < 0 || (result == 0 && (KeepInsertOrder  // keep right first when it keeps insert order
                    || (_rand.Next() % 2 == 0)))) // randomly down when it doesn't keep insert order
                                                  // another way to do this is to take a log of direction, and whenever need to select the direction, reverse it.(like following:)
                                                  // (KeepInsertOrder || (current._flip = !current._flip))     //_flip is the field to log the direction
                {
                    if (current.RightChild == null)
                    {
                        // insert into right
                        current.RightChild = node;
                        current.SearchUp();
                        Count++;
                        break;
                    }
                    current = current.RightChild as TNode; //goto right subtree
                }
                else
                {
                    if (current.LeftChild == null)
                    {
                        // insert into left
                        current.LeftChild = node;
                        current.SearchUp();
                        Count++;
                        break;
                    }
                    current = current.LeftChild as TNode; //goto left subtree
                }
            }

            return node;
        }

        /// <summary>
        /// 删除指定结点，由内部调用，确保删除的结点在内部
        /// </summary>
        /// <param name="node">需要删除的结点</param>
        protected virtual void DeleteInternal(TNode node)
        {
            if (node == null) return;
            if (node.LeftChild == null)
                Transplant(node, node.RightChild);
            else if (node.RightChild == null)
                Transplant(node, node.LeftChild);
            else
            {
                //find successor
                node.SearchDown();
                var succ = node.RightChild;
                while (succ.LeftChild != null)
                {
                    succ.SearchDown();
                    succ = succ.LeftChild;
                }

                // replace successor with its child
                if (succ.Parent != node)
                {
                    Transplant(succ, succ.RightChild);
                    succ.RightChild = node.RightChild;
                }

                // replace node with successor
                Transplant(node, succ);
                succ.LeftChild = node.LeftChild;
            }
            Count--;
        }

        /// <summary>
        /// Delete certain node
        /// 删除指定结点
        /// </summary>
        /// <param name="node">需要删除的结点</param>
        /// <return>结点在树中并且删除成功则返回true，否则返回false</return>
        public bool Delete(TNode node)
        {
            if (!SearchAll(node.Key).Contains(node)) return false;
            DeleteInternal(node);
            return true;
        }

        public TNode Delete(TKey key)
        {
            TNode current = SearchInternal(key, true);
            DeleteInternal(current);
            return current;
        }

        /// <summary>
        /// Find the first successor, key of which is not equal to <see cref="key"/> of the node with the <see cref="key"/>
        /// 寻找关键字为<see cref="key"/>结点的第一个关键字不等于<see cref="key"/>的后继
        /// </summary>
        /// <returns>第一个关键字不为key的后继</returns>
        public virtual TNode Successor(TKey key)
        {
            var ret = Search(key)?.Successor() as TNode;
            if (SupportEquatable)
                while (ret != null && _comparer.Compare(ret.Key, key) == 0)
                    ret = ret.Successor() as TNode;
            return ret as TNode;
        }

        /// <summary>
        /// Find the first predecessor, key of which is not equal to <see cref="key"/> of the node with the <see cref="key"/>
        /// 寻找关键字为<see cref="key"/>结点的第一个关键字不等于<see cref="key"/>的前驱
        /// </summary>
        /// <returns>第一个关键字不为key的前驱</returns>
        public virtual TNode Predecessor(TKey key)
        {
            var ret = Search(key)?.Predecessor() as TNode;
            if (SupportEquatable)
                while (ret != null && _comparer.Compare(ret.Key, key) == 0)
                    ret = ret.Predecessor() as TNode;
            if (ret == _rootTrailer) ret = null;//当访问到哨兵时返回空
            return ret as TNode;
        }

        public virtual IEnumerable<TNode> SearchAll(TKey key)
        {
            TNode current = Search(key);

            if (!SupportEquatable)
                yield return current;
                //throw new NotSupportedException("SupportEquatable设为了false，如果需要进行Search操作请使用Search函数");

            if (KeepInsertOrder)
            {
                while (current != null && _comparer.Compare(current.Key, key) == 0)
                {
                    yield return current;
                    current = current.Successor() as TNode;
                }
            }

            else
            {
                yield return current;
                TNode part = current.Successor() as TNode;
                while (part != null && _comparer.Compare(part.Key, key) == 0)
                {
                    yield return part;
                    part = part.Successor() as TNode;
                }
                part = current.Predecessor() as TNode;
                while (part != null && _comparer.Compare(part.Key, key) == 0)
                {
                    yield return part;
                    part = part.Predecessor() as TNode;
                }
            }
        }

        public virtual void Clear()
        {
            Root = null;
            Count = 0;
        }
    }
}
