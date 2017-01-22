using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{ 
    public static class BinaryTreeEnumerator<TNode>
        where TNode : class, IBinaryTreeNode, IPersistent
    {
        /// <summary>
        /// Enumerator for preorder, inorder and postorder traversing
        /// 递归遍历的迭代器
        /// </summary>
        public abstract class RecursiveEnumerator : IEnumerator<TNode>, IEnumerator
        {
            protected TNode _root;
            protected Stack<TNode> _stack;
            protected TNode _current;
            protected int _version;
            bool _null = false;

            internal RecursiveEnumerator(TNode root)
            {
                if (SentinelEx.NotEqualNull(root))
                {
                    _root = root;
                    _version = root.Version;
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
                if (_version != _root.Version)
                    throw new InvalidOperationException("在枚举过程中树被修改过");
                if (SentinelEx.EqualNull(_current))
                {
                    if (SentinelEx.EqualNull(_root)) return false;
                    InitPosition();
                    return true;
                }
                return MoveNextInternal();
            }
            public void Reset()
            {
                if (_version != _root.Version)
                    throw new InvalidOperationException("在枚举过程中树被修改过");
                _current = null;
                _stack.Clear();
            }
            protected abstract void InitPosition();
            protected abstract bool MoveNextInternal();
        }

        /// <remarks>
        /// Inorder enumerator use stack to traverse, it doesn't requires node to support finding successor
        /// 中序遍历器采用栈的方式进行遍历，而不要求结点支持寻找后继的操作
        /// </remarks>
        public class InOrderEnumerator : RecursiveEnumerator
        {
            public InOrderEnumerator(TNode root) : base(root) { }
            protected override void InitPosition()
            {
                _current = _root;
                while (SentinelEx.NotEqualNull(_current.LeftChild))
                {
                    _stack.Push(_current);
                    _current = _current.LeftChild as TNode;
                }
            }
            protected override bool MoveNextInternal()
            {
                if (SentinelEx.NotEqualNull(_current.RightChild))
                {
                    _stack.Push(_current);
                    _current = _current.RightChild as TNode;
                    while (SentinelEx.NotEqualNull(_current.LeftChild))
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
            public PreOrderEnumerator(TNode root) : base(root) { }

            protected override void InitPosition()
            {
                _current = _root;
            }
            protected override bool MoveNextInternal()
            {
                if (SentinelEx.NotEqualNull(_current.RightChild))
                    _stack.Push(_current.RightChild as TNode);
                if (SentinelEx.NotEqualNull(_current.LeftChild))
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
            public PostOrderEnumerator(TNode root) : base(root) { }

            protected override void InitPosition()
            {
                _stack.Push(_root as TNode);
                _current = _root;
            }

            protected override bool MoveNextInternal()
            {
                if (_stack.Count == 0) return false;
                if (_stack.Peek().LeftChild != _current && _stack.Peek().RightChild != _current)
                {
                    TNode temp = _stack.Peek();
                    while (SentinelEx.NotEqualNull(temp))
                    {
                        if (SentinelEx.NotEqualNull(temp.LeftChild))
                        {
                            if (SentinelEx.NotEqualNull(temp.RightChild))
                                _stack.Push(temp.RightChild as TNode);
                            _stack.Push(temp.LeftChild as TNode);

                        }
                        else
                        {
                            if (SentinelEx.NotEqualNull(temp.RightChild))
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
            protected TNode _root;
            protected Queue<TNode> _queue;
            protected TNode _current;
            protected int _version;

            internal LevelOrderEnumerator(TNode root)
            {
                _root = root;
                _version = root.Version;
                _queue = new Queue<TNode>();
                if (root != null) _queue.Enqueue(root);
            }
            public TNode Current { get { return _current; } }
            object IEnumerator.Current { get { return _current; } }
            public void Dispose() { }
            public bool MoveNext()
            {
                if (_version != _root.Version)
                    throw new InvalidOperationException("在枚举过程中树被修改过");
                if (_queue.Count == 0) return false;
                _current = _queue.Dequeue();
                if (SentinelEx.NotEqualNull(_current.LeftChild)) _queue.Enqueue(_current.LeftChild as TNode);
                if (SentinelEx.NotEqualNull(_current.RightChild)) _queue.Enqueue(_current.RightChild as TNode);
                return true;
            }
            public void Reset()
            {
                if (_version != _root.Version)
                    throw new InvalidOperationException("在枚举过程中树被修改过");
                _current = null;
                _queue.Clear();
            }
        }
    }
}
