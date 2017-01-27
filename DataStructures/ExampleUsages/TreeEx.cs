using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract = System.Diagnostics.Contracts.Contract;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Extension methods for Trees
    /// 提供树的扩展方法
    /// </summary>
    public static class TreeEx
    {
        #region Adapters

        /// <summary>
        /// 将二叉搜索树转换为<see cref="ICollection{T}"/>对象，以提供集合操作
        /// </summary>
        public static ICollection<TNode> AsCollection<TSource, TNode, TKey>(this TSource tree)
            where TSource : ISearchTree<TNode, TKey>
            where TNode : IKeyProvider<TKey> => new SearchTreeDictionaryAdapter<TSource, TNode, TKey>(tree);
        /// <summary>
        /// 将二叉搜索树转换为<see cref="IDictionary{TKey, TValue}"/>对象，以提供字典操作
        /// </summary>
        public static IDictionary<TKey, TNode> AsDictionary<TSource, TNode, TKey>(this TSource tree)
            where TSource : ISearchTree<TNode, TKey>
            where TNode : IKeyProvider<TKey> => new SearchTreeDictionaryAdapter<TSource, TNode, TKey>(tree);
        
        #endregion

        #region Traversal

        /// <summary>
        /// Get the enumerator of subtree, root of which is <paramref name="partialroot"/>
        /// 获取以<paramref name="partialroot"/>为根的子树的遍历器
        /// </summary>
        /// <param name="order">二叉树遍历方式</param>
        /// <param name="partialroot">需要遍历的子树的根</param>
        public static IEnumerator<TNode> GetSubtreeEnumerator<TNode>(this TNode partialroot, TraverseOrder order = TraverseOrder.InOrder)
            where TNode : class, IBinaryTreeNode, IPersistent
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

        public static IEnumerable<IBinaryTreeNode> TraverseSubtree(this IBinaryTreeNode partialroot, TraverseOrder order = TraverseOrder.InOrder)
            => partialroot.TraverseSubtree<object>(null, null, null, order).Select(res => res.Item1);
        /// <summary>
        /// enumerate the subtree with the root <paramref name="partialroot"/>, provided with certain information (of type <typeparamref name="TLevel"/>) of the node
        /// 遍历以<paramref name="partialroot"/>为根的子树，并且在过程中提供结点有关信息(<typeparamref name="TLevel"/>类型的)
        /// </summary>
        /// <typeparam name="TLevel">提供的每层结点额外信息的类型</typeparam>
        /// <param name="partialroot">需要遍历的子树的根</param>
        /// <param name="seed">附加参数的初始值</param>
        /// <param name="leftlevelfunc">遍历到某一层结点时将附加参数传给下一层左孩子时需要进行的操作</param>
        /// <param name="rightlevelfunc">遍历到某一层结点时将附加参数传给下一层右孩子时需要进行的操作</param>
        /// <param name="order">二叉树遍历方式</param>
        /// <return>遍历得到的结点与附加参数的集合</return>
        public static IEnumerable<Tuple<IBinaryTreeNode, TLevel>> TraverseSubtree<TLevel>(
            this IBinaryTreeNode partialroot,
            TLevel seed,
            Func<TLevel, IBinaryTreeNode, TLevel> leftlevelfunc,
            Func<TLevel, IBinaryTreeNode, TLevel> rightlevelfunc,
            TraverseOrder order = TraverseOrder.InOrder)
        {
            var current = new Tuple<IBinaryTreeNode, TLevel>[] { new Tuple<IBinaryTreeNode, TLevel>(partialroot, seed) };
            var leftlevel = leftlevelfunc == null ? seed : leftlevelfunc(seed, partialroot);
            var rightlevel = rightlevelfunc == null ? seed : rightlevelfunc(seed, partialroot);
            switch (order)
            {
                case TraverseOrder.InOrder:
                    return partialroot.LeftChild.TraverseSubtree(leftlevel, leftlevelfunc, rightlevelfunc, order)
                        .Concat(current)
                        .Concat(partialroot.RightChild.TraverseSubtree(rightlevel, leftlevelfunc, rightlevelfunc, order));
                case TraverseOrder.PreOrder:
                    return current
                        .Concat(partialroot.LeftChild.TraverseSubtree(leftlevel, leftlevelfunc, rightlevelfunc, order))
                        .Concat(partialroot.RightChild.TraverseSubtree(rightlevel, leftlevelfunc, rightlevelfunc, order));
                case TraverseOrder.PostOrder:
                    return partialroot.LeftChild.TraverseSubtree(leftlevel, leftlevelfunc, rightlevelfunc, order)
                        .Concat(partialroot.RightChild.TraverseSubtree(rightlevel, leftlevelfunc, rightlevelfunc, order))
                        .Concat(current);
                case TraverseOrder.LevelOrder:
                    return LevelOrderTraverse(partialroot, seed, leftlevelfunc, rightlevelfunc);
                default:
                    return null;
            }
        }
        private static IEnumerable<Tuple<IBinaryTreeNode, TLevel>> LevelOrderTraverse<TLevel>(
            IBinaryTreeNode partialroot,
            TLevel seed,
            Func<TLevel, IBinaryTreeNode, TLevel> leftlevelfunc,
            Func<TLevel, IBinaryTreeNode, TLevel> rightlevelfunc)
        {
            var queue = new Queue<Tuple<IBinaryTreeNode, TLevel>>();
            queue.Enqueue(new Tuple<IBinaryTreeNode, TLevel>(partialroot, seed));
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (!SentinelEx.EqualNull(current.Item1.LeftChild))
                    queue.Enqueue(new Tuple<IBinaryTreeNode, TLevel>(current.Item1.LeftChild,
                        leftlevelfunc == null ? current.Item2 : leftlevelfunc(current.Item2, partialroot)));
                if (!SentinelEx.EqualNull(current.Item1.RightChild))
                    queue.Enqueue(new Tuple<IBinaryTreeNode, TLevel>(current.Item1.RightChild,
                        rightlevelfunc == null ? current.Item2 : rightlevelfunc(current.Item2, partialroot)));
                yield return current;
            }
        }

        public static IEnumerable<IMultiwayTreeNode> TraverseSubtree(this IMultiwayTreeNode partialroot, TraverseOrder order = TraverseOrder.InOrder)
            => partialroot.TraverseSubtree<object>(null, null, order).Select(res => res.Item1);
        /// <summary>
        /// enumerate the subtree with the root <paramref name="partialroot"/>, provided with certain information (of type <typeparamref name="TLevel"/>) of the node
        /// 遍历以<paramref name="partialroot"/>为根的子树，并且在过程中提供结点有关信息(<typeparamref name="TLevel"/>类型的)
        /// </summary>
        /// <typeparam name="TLevel">提供的每层结点额外信息的类型</typeparam>
        /// <param name="partialroot">需要遍历的子树的根</param>
        /// <param name="seed">附加参数的初始值</param>
        /// <param name="levelfunc">遍历到某一层结点时将附加参数传给下一层孩子时需要进行的操作。第一个参数是该层的附加参数，
        /// 第二个参数是遍历到的结点，第三个是遍历到的结点在上层结点孩子结点中的编号</param>
        /// <param name="order">多叉树遍历方式</param>
        /// <returns>遍历得到的结点与附加参数的集合</returns>
        public static IEnumerable<Tuple<IMultiwayTreeNode, TLevel>> TraverseSubtree<TLevel>(
            this IMultiwayTreeNode partialroot,
            TLevel seed,
            Func<TLevel, IMultiwayTreeNode, int, TLevel> levelfunc,
            TraverseOrder order = TraverseOrder.LevelOrder)
        {
            var current = new Tuple<IMultiwayTreeNode, TLevel>[] { new Tuple<IMultiwayTreeNode, TLevel>(partialroot, seed) };
            switch (order)
            {
                case TraverseOrder.InOrder:
                    throw new NotSupportedException("多叉树不支持中序遍历");
                case TraverseOrder.PreOrder:
                    return current.Concat(partialroot.Children?
                        .SelectMany((node, index) => node?.TraverseSubtree(
                            levelfunc == null ? seed : levelfunc(seed, partialroot, index), levelfunc, order)
                            ?? Enumerable.Empty<Tuple<IMultiwayTreeNode, TLevel>>())
                        ?? Enumerable.Empty<Tuple<IMultiwayTreeNode, TLevel>>());
                case TraverseOrder.PostOrder:
                    return (partialroot.Children?
                        .SelectMany((node, index) => node?.TraverseSubtree(
                            levelfunc == null ? seed : levelfunc(seed, partialroot, index), levelfunc, order)
                            ?? Enumerable.Empty<Tuple<IMultiwayTreeNode, TLevel>>())
                        ?? Enumerable.Empty<Tuple<IMultiwayTreeNode, TLevel>>())
                        .Reverse().Concat(current);
                case TraverseOrder.LevelOrder:
                    return LevelOrderTraverse(partialroot, seed, levelfunc);
                default:
                    return null;
            }
        }
        private static IEnumerable<Tuple<IMultiwayTreeNode, TLevel>> LevelOrderTraverse<TLevel>(
            IMultiwayTreeNode partialroot,
            TLevel seed,
            Func<TLevel, IMultiwayTreeNode, int, TLevel> levelfunc)
        {
            var queue = new Queue<Tuple<IMultiwayTreeNode, TLevel>>();
            queue.Enqueue(new Tuple<IMultiwayTreeNode, TLevel>(partialroot, seed));
            int index;
            while (queue.Count > 0)
            {
                index = 0;
                var current = queue.Dequeue();
                if (SentinelEx.NotEqualNull(current.Item1.Children))
                    foreach (var node in current.Item1.Children)
                        queue.Enqueue(new Tuple<IMultiwayTreeNode, TLevel>(node,
                            levelfunc == null ? current.Item2 : levelfunc(current.Item2, partialroot, index++)));
                yield return current;
            }
        }

        #endregion

        #region Tree Type Judging

        public static TreeKind JudgeKind<TNode>(this IRootedTree<TNode> tree)
            where TNode : class, IBinaryTreeNode
        {
            var last = tree.Root;
            if (SentinelEx.EqualNull(last)) throw new InvalidOperationException("树不能为空");
            TreeKind result = TreeKind.Ordinary;

            int degreelog = tree.Root.GetDegree();
            int nstrict = 0;
            int depthlog = -1;
            bool newline = false, completeflag = true;
            Func<int, IBinaryTreeNode, int> updatefunc = (int level, IBinaryTreeNode node) => level + 1;
            foreach (var current in LevelOrderTraverse(tree.Root, 0, updatefunc, updatefunc))
            {
                var node = current.Item1;
                var depth = current.Item2;
                if (node.GetDegree() != 0)
                {
                    if (node.GetDegree() != degreelog)
                    {
                        nstrict++;
                        if (nstrict > 1) break; //more than one inner node differs with root, then it must be a ordinary tree.
                    }
                }
                else
                {
                    if (depthlog == -1)
                        depthlog = depth;
                    else if (depth > depthlog)
                        if (newline)
                            completeflag = false;
                        else
                            newline = true;
                }
                last = node as TNode;
            }
            if (last != tree.Root && SentinelEx.EqualNull(last.Parent.LeftChild))
                completeflag = false;
            if (nstrict == 0)
                if (degreelog == 2)
                    result |= TreeKind.Full;
                else
                    result |= TreeKind.Strict;
            if (nstrict <= 1 && completeflag)
                if (!newline && (last == tree.Root || !SentinelEx.EqualNull(last.Parent.RightChild)))
                    result |= TreeKind.Perfect;
                else
                    result |= TreeKind.Complete;
            return result;
        }
        public static int GetDegree(this IBinaryTreeNode node)
        {
            if (SentinelEx.NotEqualNull(node.LeftChild))
                if (SentinelEx.NotEqualNull(node.RightChild))
                    return 2;
                else
                    return 1;
            else if (SentinelEx.NotEqualNull(node.RightChild))
                return 1;
            else return 0;
        }
        public static int GetDegree(this IMultiwayTreeNode node) => node.Children?.Count(cnode => SentinelEx.NotEqualNull(cnode)) ?? 0;
        private static bool AreChildrenLeftPadding(this IMultiwayTreeNode node)
        {
            if (node.Children == null) return true;
            bool flag = true;
            foreach (var t in node.Children)
                if (t != null && !flag) return false;
                else flag = false;
            return true;
        }

        #endregion
    }

    class SearchTreeDictionaryAdapter<TStructure, TNode, TKey> : ICollection<TNode>, IDictionary<TKey, TNode>
        where TStructure : ISearchTree<TNode, TKey>
        where TNode : IKeyProvider<TKey>
    {
        TStructure _tree;
        public SearchTreeDictionaryAdapter(TStructure tree) { _tree = tree; }

        #region ICollection Members
        public int Count => _tree.Count;

        public bool IsReadOnly => false;

        public void Add(TNode item) => _tree.InsertNode(item);

        public void Clear() => _tree.Clear();

        public bool Contains(TNode item) => _tree.SearchNode(item.Key) != null;

        public void CopyTo(TNode[] array, int arrayIndex) => this.CopyTo<TNode>(array, arrayIndex);

        public IEnumerator<TNode> GetEnumerator() => _tree.GetEnumerator();

        public bool Remove(TNode item) => _tree.DeleteNode(item);

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
                return _tree.SearchNode(key);
            }

            set
            {
                var target = _tree.SearchNode(key);
                if (!target.Equals(value))
                {
                    _tree.DeleteNode(key);
                    _tree.InsertNode(value);
                }
            }
        }

        public ICollection<TKey> Keys => _tree.Select(node => node.Key).ToList();

        public ICollection<TNode> Values => this;

        public void Add(KeyValuePair<TKey, TNode> item) => Add(item.Key, item.Value);

        public void Add(TKey key, TNode value) => _tree.InsertNode(KeyValueCheck(key, value));

        public bool Contains(KeyValuePair<TKey, TNode> item) => Contains(KeyValueCheck(item.Key, item.Value));

        public bool ContainsKey(TKey key) => _tree.SearchNode(key) != null;

        public void CopyTo(KeyValuePair<TKey, TNode>[] array, int arrayIndex) => this.CopyTo<KeyValuePair<TKey, TNode>>(array, arrayIndex);

        public bool Remove(TKey key) => _tree.DeleteNode(key) != null;

        public bool Remove(KeyValuePair<TKey, TNode> item) => _tree.DeleteNode(KeyValueCheck(item.Key, item.Value));

        public bool TryGetValue(TKey key, out TNode value)
        {
            value = _tree.SearchNode(key);
            return value != null;
        }

        IEnumerator<KeyValuePair<TKey, TNode>> IEnumerable<KeyValuePair<TKey, TNode>>.GetEnumerator() => _tree.Select(node => new KeyValuePair<TKey, TNode>(node.Key, node)).GetEnumerator();

        #endregion
    }
}
