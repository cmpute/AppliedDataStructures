using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Class for Binary Tree Node
    /// 二叉树结点类
    /// </summary>
    /// <remarks>
    /// 继承结点可以在结点上维护额外信息减小操作的时间复杂度
    /// 结点没有保留双亲指针，因为部分树不需要向上回溯的过程
    /// </remarks>
    public class BinaryTreeNode : IBinaryTreeNode, IPersistent
    {
        #region Leaf Trailor
        static readonly BinaryTreeNode _nil = new BinaryTreeNode() { _par = null, _lchild = null, _rchild = null };
        /// <summary>
        /// Trailor of leaf nodes
        /// 叶子节点为空时的哨兵
        /// </summary>
        public static BinaryTreeNode Null => _nil;

        public static bool operator ==(BinaryTreeNode a, BinaryTreeNode b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (((object)a == null || a.IsNil()) &&
                ((object)b == null || b.IsNil()))
                return true;
            return false;
        }
        public static bool operator !=(BinaryTreeNode a, BinaryTreeNode b) => !(a == b);
        /// <summary>
        /// Method for indicating whether it's leaf trailor in the tree.It should be override whenever nil is re-defined.
        /// 用来判断当前结点是否为叶结点哨兵的方法。如果重新定义了nil对象，则应重载此方法。
        /// </summary>
        /// <returns>如果当前结点是哨兵则返回<c>true</c></returns>
        protected virtual bool IsNil()
        {
            return ReferenceEquals(this, _nil);
        }

        public override string ToString()
        {
            if (Equals(_nil)) return "Nil Leaf";
            return base.ToString();
        }
        #endregion

        public int Version { get; private set; } = 0; // 作为持久数据结构版本记录的标记

        #region Relatives
        protected BinaryTreeNode _par = _nil, _lchild = _nil, _rchild = _nil;
        /// <remarks>
        /// Do not edit Parent Manually, setting LeftChild/RightChild will automatically set the Parent property.
        /// If you want to edit Parent only, access _par instead.
        /// 不要手动修改Parent属性，在设置LeftChild/RightChild时会自动设置该属性。
        /// 如果需要仅修改parent值，则请访问_par字段。
        /// </remarks>
        public virtual BinaryTreeNode Parent { get { return _par; } }
        public virtual BinaryTreeNode LeftChild
        {
            get { return _lchild; }
            set
            {
                if (value == this) throw new InvalidOperationException("不能将自己设为自己的孩子");
                _lchild = value;
                if (!ReferenceEquals(value, null)) value._par = this;
            }
        }
        public virtual BinaryTreeNode RightChild
        {
            get { return _rchild; }
            set
            {
                if (value == this) throw new InvalidOperationException("不能将自己设为自己的孩子");
                _rchild = value;
                if (!ReferenceEquals(value, null)) value._par = this;
            }
        }

        IBinaryTreeNode IBinaryTreeNode.Parent => Parent;
        IBinaryTreeNode IBinaryTreeNode.LeftChild => LeftChild;
        IBinaryTreeNode IBinaryTreeNode.RightChild => RightChild;

        /// <summary>
        /// 寻找当前结点在二叉树中序遍历中的后继
        /// </summary>
        public virtual BinaryTreeNode Successor()
        {
            BinaryTreeNode current = this;
            if (current.RightChild != null)
            {
                current = current.RightChild;
                while (current.LeftChild != null)
                    current = current.LeftChild;
            }
            else
            {
                while (current.Parent?.RightChild == current)
                    current = current.Parent;
                current = current.Parent;
            }
            return current;
        }
        /// <summary>
        /// 寻找当前结点在二叉树中序遍历中的前驱
        /// </summary>
        public virtual BinaryTreeNode Predecessor()
        {
            BinaryTreeNode current = this;
            if (current.LeftChild != null)
            {
                current = current.LeftChild;
                while (current.RightChild != null)
                    current = current.RightChild;
            }
            else
            {
                while (current.Parent?.LeftChild == current)
                    current = current.Parent;
                current = current.Parent;
            }
            return current;
        }

        /// <summary>
        /// make parent of this the parent of <paramref name="newChild"/>
        /// 让当前结点的父亲成为<paramref name="newChild"/>的父亲
        /// </summary>
        internal void TransferParent(BinaryTreeNode newChild)
        {
            if (Parent.LeftChild == this)
                Parent.LeftChild = newChild;
            else
                Parent.RightChild = newChild;
        }
        #endregion

        #region Update Operations

        internal void SearchDown()
        {
            OnSearchDown();
        }
        internal void SearchUp()
        {
            Version++;

            if (LeftChild != null && LeftChild.Version > Version)
                Version = LeftChild.Version;
            if (RightChild != null && RightChild.Version > Version)
                Version = RightChild.Version;
            OnSearchUp();
            OnSearchUpRecursive();

            if (Parent != null)
            {
                BinaryTreeNode current = Parent;
                while (current != null)
                {
                    var temp = Version;

                    var nsearchup = !current.OnSearchUpRecursive();
                    if (current.LeftChild != null && current.LeftChild.Version > Version)
                        Version = current.LeftChild.Version;
                    if (current.RightChild != null && current.RightChild.Version > Version)
                        Version = current.RightChild.Version;

                    if (temp == Version && nsearchup) break; // No updating any more
                    current = current.Parent;
                }
            }
        }

        /// <summary>
        /// Invoke when travel down from root through the tree
        /// 在搜索下行时调用方法
        /// </summary>
        /// <remarks>
        /// If there are some lazy operations which need being performed on child nodes, the performation should be implemented here.
        /// 如果有延迟操作的标记需要应用到子节点上，应在此方法中实现
        /// </remarks>
        protected virtual void OnSearchDown()
        {

        }
        /// <summary>
        /// Invoke when travel back from the leaves through the tree
        /// 在搜索上行开始时调用的方法
        /// </summary>
        /// <remarks>
        /// If there exist information which need being updated lasily from child nodes, the update should be implemented here.
        /// 如果有信息需要从子节点更新，并且是懒惰更新，应在此方法中实现
        /// </remarks>
        protected virtual void OnSearchUp()
        {

        }
        /// <summary>
        /// Invoke when travel back from the leaves through the tree recursively
        /// 在递归搜索上行时调用的方法
        /// </summary>
        /// <remarks>
        /// If there exist information which need being updated instantly from child nodes, the update should be implemented here.
        /// 如果有信息需要从子节点更新，并且是立即更新，应在此方法中实现
        /// </remarks>
        /// <returns>是否有信息修改，如果为false则会终止递归调用</returns>
        protected virtual bool OnSearchUpRecursive()
        {
            return false;
        }

        #endregion

        #region Balance Operations
        // Operations for Balanced Binary Tree (BBT), specifically for Balanced Binary Search Tree (BBST)

        /// <summary>
        /// Zig - Right Rotation at the node
        /// 平衡二叉树的右旋操作
        /// </summary>
        /// <remarks>
        /// c.Zig() =>
        ///     p            c  
        ///    / \          / \ 
        ///   c   z        x   p
        ///  / \      ->      / \
        /// x   y            y   z
        /// 
        /// 在旋转之后需要考虑更新树根
        /// </remarks>
        public void Zig()
        {
            var p = Parent;
            p.SearchDown();
            this.SearchDown();
            
            p.LeftChild = RightChild;
            p.TransferParent(this);
            RightChild = p;

            p.OnSearchUp();
            this.OnSearchUp();
        }
        /// <summary>
        /// Zag - Left Rotation at the node
        /// 平衡二叉树的左旋操作
        /// </summary>
        /// <remarks>
        /// c.Zag() =>
        ///    p                  c  
        ///   / \                / \ 
        ///  x   c              p   z
        ///     / \     ->     / \ 
        ///    y   z          x   y
        /// 
        /// 在旋转之后需要考虑更新树根
        /// </remarks>
        public void Zag()
        {
            var p = Parent;
            p.OnSearchDown();
            this.OnSearchDown();

            p.RightChild = LeftChild;
            p.TransferParent(this);
            LeftChild = p;

            p.OnSearchUp();
            this.OnSearchUp();
        }

        #endregion
    }
}
