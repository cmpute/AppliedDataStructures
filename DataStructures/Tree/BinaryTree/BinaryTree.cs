using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Abstraction of Binary Tree
    /// 二叉树的抽象类
    /// </summary>
    /// <remarks>
    /// 对于完全二叉树的紧凑型实现在此处不予考虑，如需要紧凑实现增强性能的话需要使用结构体数组，并且用Marshall使内存紧凑，这并不是C#的长处，应改用C++
    /// 对于广义表的树存储在此也不予考虑
    /// </remarks>
    public abstract class BinaryTree<TNode> : IRootedTree<TNode> where TNode : BinaryTreeNode
    {
        public TNode Root { get; set; }
        public int Count { get; protected set; }

        public IEnumerator<TNode> GetEnumerator(TraversalOrder order)
        {
            switch (order)
            {
                case TraversalOrder.InOrder:
                    return new InOrderEnumerator(this);
                case TraversalOrder.PreOrder:
                    return new PreOrderEnumerator(this);
                case TraversalOrder.PostOrder:
                    return new PostOrderEnumerator(this);
                case TraversalOrder.LevelOrder:
                    return new LevelOrderEnumerator(this);
                default:
                    return null;
            }
        }
        /// <summary>
        /// Return InOrder Enumerator by default
        /// 默认返回中序遍历器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TNode> GetEnumerator()
        {
            return new InOrderEnumerator(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new InOrderEnumerator(this);
        }

        #region Traverse Enumerators

        /// <summary>
        /// Enumerator for preorder, inorder and postorder traversing
        /// 递归遍历的迭代器
        /// </summary>
        public abstract class RecursiveEnumerator : IEnumerator<TNode>, IEnumerator
        {
            protected BinaryTree<TNode> _tree;
            protected Stack<TNode> _stack;
            protected TNode _current;
            protected int _version;
            bool _null = false;

            internal RecursiveEnumerator(BinaryTree<TNode> tree)
            {
                if (tree.Root != null)
                {
                    _tree = tree;
                    _version = tree.Root._version;
                    _stack = new Stack<TNode>();
                }
                else _null = true;
            }

            public void Dispose() { }
            public TNode Current { get { return _current; } }
            object IEnumerator.Current { get { return _current; } }
            public bool MoveNext()
            {
                if (_null) return false;
                if (_version != _tree.Root._version)
                    throw new InvalidOperationException("在枚举过程中树被修改过");
                if (_current == null)
                {
                    if (_tree.Root == null) return false;
                    InitPosition();
                    return true;
                }
                return MoveNextInternal();
            }
            public void Reset()
            {
                if (_version != _tree.Root._version)
                    throw new InvalidOperationException("在枚举过程中树被修改过");
                _current = null;
                _stack.Clear();
            }
            protected abstract void InitPosition();
            protected abstract bool MoveNextInternal();
        }

        /// <remarks>
        /// 中序遍历器采用栈的方式进行遍历，而不要求结点支持Successor操作
        /// </remarks>
        public class InOrderEnumerator : RecursiveEnumerator
        {
            public InOrderEnumerator(BinaryTree<TNode> tree) : base(tree) { }
            protected override void InitPosition()
            {
                _current = _tree.Root;
                while (_current.LeftChild != null)
                {
                    _stack.Push(_current);
                    _current = _current.LeftChild as TNode;
                }
            }
            protected override bool MoveNextInternal()
            {
                if (_current.RightChild != null)
                {
                    _stack.Push(_current);
                    _current = _current.RightChild as TNode;
                    while (_current.LeftChild != null)
                    {
                        _stack.Push(_current);
                        _current = _current.LeftChild as TNode;
                    }
                    return true;
                }
                else
                {
                    while (_stack.Count > 0 && _stack.Peek().RightChild == _current)
                        _current = _stack.Pop(); //向上找到右行点
                    if (_stack.Count == 0)
                        return false;
                    _current = _stack.Pop();
                    return true;
                }
            }
        }
        public class PreOrderEnumerator : RecursiveEnumerator
        {
            public PreOrderEnumerator(BinaryTree<TNode> tree) : base(tree) { }

            protected override void InitPosition()
            {
                _current = _tree.Root;
            }
            protected override bool MoveNextInternal()
            {
                if (_current.RightChild != null)
                    _stack.Push(_current.RightChild as TNode);
                if (_current.LeftChild != null)
                {
                    _current = _current.LeftChild as TNode;
                    return true;
                }
                if (_stack.Count == 0)
                    return false;
                _current = _stack.Pop();
                return true;
            }
        }
        public class PostOrderEnumerator : RecursiveEnumerator
        {
            public PostOrderEnumerator(BinaryTree<TNode> tree) : base(tree) { }

            protected override void InitPosition()
            {
                _stack.Push(_tree.Root as TNode);
                _current = _tree.Root;
            }

            protected override bool MoveNextInternal()
            {
                if (_stack.Count == 0) return false;
                if (_stack.Peek().LeftChild != _current && _stack.Peek().RightChild != _current)
                {
                    TNode temp = _stack.Peek();
                    while (temp != null)
                    {
                        if (temp.LeftChild != null)
                        {
                            if (temp.RightChild != null)
                                _stack.Push(temp.RightChild as TNode);
                            _stack.Push(temp.LeftChild as TNode);

                        }
                        else
                        {
                            if (temp.RightChild != null)
                                _stack.Push(temp.RightChild as TNode);
                            else break;
                        }
                        temp = _stack.Peek();
                    }
                }
                _current = _stack.Pop();
                return true;
            }
        }

        public class LevelOrderEnumerator : IEnumerator<TNode>, IEnumerator
        {
            protected BinaryTree<TNode> _tree;
            protected Queue<TNode> _queue;
            protected TNode _current;
            protected int _version;

            internal LevelOrderEnumerator(BinaryTree<TNode> tree)
            {
                _tree = tree;
                _version = tree.Root._version;
                _queue = new Queue<TNode>();
                if (_tree.Root != null) _queue.Enqueue(_tree.Root);
            }
            public TNode Current { get { return _current; } }
            object IEnumerator.Current { get { return _current; } }
            public void Dispose(){}
            public bool MoveNext()
            {
                if (_version != _tree.Root._version)
                    throw new InvalidOperationException("在枚举过程中树被修改过");
                if (_queue.Count == 0) return false;
                _current = _queue.Dequeue();
                if (_current.LeftChild != null) _queue.Enqueue(_current.LeftChild as TNode);
                if (_current.RightChild != null) _queue.Enqueue(_current.RightChild as TNode);
                return true;
            }
            public void Reset()
            {
                if (_version != _tree.Root._version)
                    throw new InvalidOperationException("在枚举过程中树被修改过");
                _current = null;
                _queue.Clear();
            }
        }

        #endregion
    }
}
