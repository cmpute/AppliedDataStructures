using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// 二叉搜索树
    /// </summary>
    public class BinarySearchTree<TNode, TKey> : BinaryTree<TNode>, ISearchTreeEquatable<TNode, TKey> where TNode : BinarySearchTreeNode<TKey>
    {
        protected IComparer<TKey> _comparer;

        public BinarySearchTree() : this(Comparer<TKey>.Default) { }
        public BinarySearchTree(IComparer<TKey> comparer)
        {
            _comparer = comparer;
        }

        /// <summary>
        /// 是否支持<see cref="ISearchTreeEquatable{TNode, TKey}"/>的操作
        /// </summary>
        public bool SupportEquatable { get; set; } = false;

        /// <summary>
        /// 当元素雷同时，是否在中序遍历的意义下保持插入的顺序（先插入者在前），并且搜索时返回第一个插入的元素
        /// 该属性仅当<see cref="SupportEquatable"/>为true时生效
        /// </summary>
        public bool KeepInsertOrder { get; set; } = true;

        /// <summary>
        /// 用replace子树替换u结点
        /// </summary>
        /// <param name="target">被替换结点</param>
        /// <param name="replace">替换的结点</param>
        private void Transplant(BinaryTreeNode target, BinaryTreeNode replace)
        {
            if (target.Parent == null)
                Root = replace as TNode;
            else
            {
                target.Parent.SearchDown();
                if (target == target.Parent.LeftChild)
                    target.Parent.LeftChild = replace;
                else
                    target.Parent.RightChild = replace;
            }
            if (replace != null)
            {
                replace.Parent = target.Parent;
                replace.SearchUp();
            }
        }

        /// <summary>
        /// Internal Search Method
        /// 内部调用的Search函数
        /// </summary>
        /// <param name="modify">是否在下行过程中调用OnSearchDown</param>
        private TNode Search(TKey key, bool modify)
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

        public virtual TNode Search(TKey key)
        {
            return Search(key, false);
        }

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
                    || (current._flip = !current._flip)))) // alternatively down when it doesn't keep insert order
                {
                    if (current.RightChild == null)
                    {
                        // insert into right
                        current.RightChild = node;
                        node.Parent = current;
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
                        node.Parent = current;
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
        /// Delete certain node
        /// 删除指定结点
        /// </summary>
        /// <param name="node">需要删除的结点</param>
        public virtual void Delete(TNode node)
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
                    succ.RightChild.Parent = succ;
                }

                // replace node with successor
                Transplant(node, succ);
                succ.LeftChild = node.LeftChild;
                succ.LeftChild.Parent = succ;
            }
            Count--;
        }

        public virtual TNode Delete(TKey key)
        {
            TNode current = Search(key, true);
            Delete(current);
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
            return ret as TNode;
        }

        public virtual IEnumerable<TNode> SearchAll(TKey key)
        {
            if (!SupportEquatable)
                throw new NotSupportedException("SupportEquatable设为了false，如果需要进行Search操作请使用Search函数");

            TNode current = Search(key);

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
    }
}
