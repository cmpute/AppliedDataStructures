using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced.Tree
{
    /// <summary>
    /// Node of a k-way tree (a.k.a k-ary tree)
    /// k路树(也称k叉树)的一个节点
    /// </summary>
    /// <remarks>
    /// "K-way" means that the out-degree(count of children) of the node would not be bigger than k.
    /// k路的意思是结点的出度（孩子数）不能超过k。
    /// 
    /// Binary Tree is obviously a specific k-ary tree. And there is also some important k-ary tree like tenary tree. ("k-ary"s: unary, binary, tenary, quaternary, pentanary...) 
    /// 二叉树显然是k叉树的一个特例。也有其它的k叉树非常有用，比如三叉树。
    /// 
    /// K-way tree node can be implemented in different ways (e.g parent-children manner / first child-next sibling manner). The way using a list to store children is most efficient for K-way tree.
    /// k路树有多种不同的实现方式（如父亲-孩子形式/孩子-兄弟形式）。用列表存储孩子对于k路树而言是最经济的。
    /// 
    /// PS: K-ary Search Tree is abbriviated as KST.
    /// 注：k路搜索树的简写是KST
    /// </remarks>
    public class KaryTreeNode : IMultiwayTreeNode
    {
        List<KaryTreeNode> _children;

        /// <summary>
        /// The maximum count of children
        /// 最大孩子数
        /// </summary>
        public int MaxDegree { get; private set; }

        public List<KaryTreeNode> Children;
        IReadOnlyCollection<IMultiwayTreeNode> IMultiwayTreeNode.Children => _children;

        public KaryTreeNode(int maxDegree)
        {
            Contract.Requires<ArgumentException>(maxDegree > 0);

            MaxDegree = maxDegree;
            _children = new List<KaryTreeNode>(maxDegree);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(MaxDegree > 0);
            Contract.Invariant(Children.Count <= MaxDegree);
            Contract.Invariant(Children.Capacity == MaxDegree);
        }
    }
}
