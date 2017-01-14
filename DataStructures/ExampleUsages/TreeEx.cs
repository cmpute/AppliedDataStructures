using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Extension methods for Trees
    /// 提供树的扩展方法
    /// </summary>
    public static class TreeEx
    {
        /// <summary>
        /// 将二叉搜索树转换为<see cref="ICollection{T}"/>对象，以提供集合操作
        /// </summary>
        public static ICollection<TNode> ToCollection<TNode, TKey>(this BinarySearchTree<TNode, TKey> tree) where TNode : BinaryTreeNode, IKeyedNode<TKey> => new BinarySearchTreeDictionary<TNode, TKey>(tree);
        /// <summary>
        /// 将二叉搜索树转换为<see cref="IDictionary{TKey, TValue}"/>对象，以提供字典操作
        /// </summary>
        public static IDictionary<TKey, TNode> ToDictionary<TNode, TKey>(this BinarySearchTree<TNode, TKey> tree) where TNode : BinaryTreeNode, IKeyedNode<TKey> => new BinarySearchTreeDictionary<TNode, TKey>(tree);

        #region Traversal

        /// <summary>
        /// Get the inorder enumerator of subtree, root of which is <paramref name="partialroot"/>
        /// 获取以<paramref name="partialroot"/>为根的子树的中序遍历器
        /// </summary>
        /// <param name="partialroot">需要遍历的子树的根</param>
        public static IEnumerator<TNode> GetSubtreeEnumerator<TNode>(this TNode partialroot) where TNode : BinaryTreeNode => new BinaryTreeEnumerator<TNode>.InOrderEnumerator(partialroot);
        /// <summary>
        /// Get the enumerator of subtree, root of which is <paramref name="partialroot"/>
        /// 获取以<paramref name="partialroot"/>为根的子树的遍历器
        /// </summary>
        /// <param name="order">二叉树遍历方式</param>
        /// <param name="partialroot">需要遍历的子树的根</param>
        public static IEnumerator<TNode> GetSubtreeEnumerator<TNode>(this TNode partialroot, TraverseOrder order) where TNode : BinaryTreeNode
        {
            switch (order)
            {
                case TraverseOrder.InOrder:
                    return new BinaryTreeEnumerator<TNode>.InOrderEnumerator(partialroot);
                case TraverseOrder.PreOrder:
                    return new BinaryTreeEnumerator<TNode>.PreOrderEnumerator(partialroot);
                case TraverseOrder.PostOrder:
                    return new BinaryTreeEnumerator<TNode>.PostOrderEnumerator(partialroot);
                case TraverseOrder.LevelOrder:
                    return new BinaryTreeEnumerator<TNode>.LevelOrderEnumerator(partialroot);
                default:
                    return null;
            }
        }

        public static IEnumerable<IBinaryTreeNode> TraverseSubtree(this IBinaryTreeNode partialroot) => TraverseSubtree(partialroot, TraverseOrder.InOrder);
        /// <summary>
        /// enumerate the subtree with the root <paramref name="partialroot"/>
        /// 遍历以<paramref name="partialroot"/>为根的子树
        /// </summary>
        /// <param name="partialroot">需要遍历的子树的根</param>
        /// <param name="order">二叉树遍历方式</param>
        public static IEnumerable<IBinaryTreeNode> TraverseSubtree(this IBinaryTreeNode partialroot, TraverseOrder order) where TNode : IBinaryTreeNode
        {
            var current = new IBinaryTreeNode[] { partialroot };
            switch (order)
            {
                case TraverseOrder.InOrder:
                    return partialroot.LeftChild.TraverseSubtree(order)
                        .Concat(current)
                        .Concat(partialroot.RightChild.TraverseSubtree(order));
                case TraverseOrder.PreOrder:
                    return current
                        .Concat(partialroot.LeftChild.TraverseSubtree(order))
                        .Concat(partialroot.RightChild.TraverseSubtree(order));
                case TraverseOrder.PostOrder:
                    return partialroot.LeftChild.TraverseSubtree(order)
                        .Concat(partialroot.RightChild.TraverseSubtree(order))
                        .Concat(current);
                case TraverseOrder.LevelOrder:
                    return LevelOrderTraverse(partialroot);
                default:
                    return null;
            }
        }
        private static IEnumerable<IBinaryTreeNode> LevelOrderTraverse(IBinaryTreeNode partialroot)
        {
            var queue = new Queue<IBinaryTreeNode>();
            queue.Enqueue(partialroot);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current.LeftChild != null) queue.Enqueue(current.LeftChild);
                if (current.RightChild != null) queue.Enqueue(current.RightChild);
                yield return current;
            }
        }

        public static IEnumerable<IMultiwayTreeNode> TraverseSubtree(this IMultiwayTreeNode partialroot) => TraverseSubtree(partialroot, TraverseOrder.InOrder);
        public static IEnumerable<IMultiwayTreeNode> TraverseSubtree(this IMultiwayTreeNode partialroot, TraverseOrder order)
        {
            var current = new IMultiwayTreeNode[] { partialroot };
            switch (order)
            {
                case TraverseOrder.InOrder:
                    throw new NotSupportedException("多叉树不支持中序遍历");
                case TraverseOrder.PreOrder:
                    return current.Concat(partialroot.Children);
                case TraverseOrder.PostOrder:
                    return partialroot.Children.Reverse().Concat(current);
                case TraverseOrder.LevelOrder:
                    return LevelOrderTraverse(partialroot);
                default:
                    return null;
            }
        }
        private static IEnumerable<IMultiwayTreeNode> LevelOrderTraverse(IMultiwayTreeNode partialroot)
        {
            var queue = new Queue<IMultiwayTreeNode>();
            queue.Enqueue(partialroot);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current.Children != null)
                    foreach (var node in current.Children)
                        queue.Enqueue(node);
                yield return current;
            }
        }

        #endregion
    }

    class BinarySearchTreeDictionary<TNode, TKey> : ICollection<TNode>, IDictionary<TKey,TNode> where TNode : BinaryTreeNode, IKeyedNode<TKey>
    {
        BinarySearchTree<TNode, TKey> _tree;
        public BinarySearchTreeDictionary(BinarySearchTree<TNode, TKey> tree) { _tree = tree; }

        #region ICollection Members
        public int Count => _tree.Count;

        public bool IsReadOnly => false;

        public void Add(TNode item) => _tree.Insert(item);

        public void Clear() => _tree.Clear();

        public bool Contains(TNode item) => _tree.Search(item.Key) != null;

        public void CopyTo(TNode[] array, int arrayIndex)
        {
            int current = arrayIndex;
            var iter = GetEnumerator();
            while (iter.MoveNext())
                array[current++] = iter.Current;
        }

        public IEnumerator<TNode> GetEnumerator() => _tree.GetEnumerator();

        public bool Remove(TNode item) => _tree.Delete(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region IDictionary Members

        TNode KeyValueCheck(TKey key, TNode value)
        {
            if (_tree.KeyComparer.Compare(key, value.Key) != 0) throw new ArgumentException("结点键值与输入的参数键值不匹配");
            return value;
        }

        public TNode this[TKey key]
        {
            get
            {
                return _tree.Search(key);
            }

            set
            {
                var target = _tree.Search(key);
                if (target != value)
                {
                    _tree.Delete(key);
                    _tree.Insert(value);
                }
            }
        }

        public ICollection<TKey> Keys => _tree.Select(node => node.Key).ToList();

        public ICollection<TNode> Values => this;

        public void Add(KeyValuePair<TKey, TNode> item) => Add(item.Key, item.Value);

        public void Add(TKey key, TNode value) => _tree.Insert(KeyValueCheck(key, value));

        public bool Contains(KeyValuePair<TKey, TNode> item) => Contains(KeyValueCheck(item.Key, item.Value));

        public bool ContainsKey(TKey key) => _tree.Search(key) != null;

        public void CopyTo(KeyValuePair<TKey, TNode>[] array, int arrayIndex)
        {
            int current = arrayIndex;
            var iter = GetEnumerator();
            while (iter.MoveNext())
                array[current++] = new KeyValuePair<TKey, TNode>(iter.Current.Key, iter.Current);
        }

        public bool Remove(TKey key) => _tree.Delete(key) != null;

        public bool Remove(KeyValuePair<TKey, TNode> item) => _tree.Delete(KeyValueCheck(item.Key, item.Value));

        public bool TryGetValue(TKey key, out TNode value)
        {
            value = _tree.Search(key);
            return value != null;
        }

        IEnumerator<KeyValuePair<TKey, TNode>> IEnumerable<KeyValuePair<TKey, TNode>>.GetEnumerator() => _tree.Select(node => new KeyValuePair<TKey, TNode>(node.Key, node)).GetEnumerator();

        #endregion
    }
}
