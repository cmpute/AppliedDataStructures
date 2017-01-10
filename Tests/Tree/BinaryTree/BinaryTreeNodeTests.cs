using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced.Tests
{
    [TestClass()]
    public class BinaryTreeNodeTests
    {
        [TestMethod()]
        public void ZigTest()
        {
            BinaryTreeNode v = new BinaryTreeNode(),
                p = new BinaryTreeNode(),
                c = new BinaryTreeNode();
            SetLChild(v, p);
            SetLChild(p, c);

            p.Zig();

            Assert.AreEqual(p.LeftChild, c);
            Assert.AreEqual(p.RightChild, v);
            Assert.AreEqual(c.Parent, p);
            Assert.AreEqual(v.Parent, p);
        }

        [TestMethod()]
        public void ZagTest()
        {
            BinaryTreeNode v = new BinaryTreeNode(),
                p = new BinaryTreeNode(),
                c = new BinaryTreeNode();
            SetRChild(v, p);
            SetRChild(p, c);

            p.Zag();

            Assert.AreEqual(p.LeftChild, v);
            Assert.AreEqual(p.RightChild, c);
            Assert.AreEqual(c.Parent, p);
            Assert.AreEqual(v.Parent, p);
        }

        [TestMethod()]
        public void SuccAndPredTest()
        {
            /*      1
             *    /   \
             *   2     3
             *  / \   /
             * 4  5  6
             *   /
             *  7
             */
            BinaryTreeNode v1 = new BinaryTreeNode(), v2 = new BinaryTreeNode(),
                v3 = new BinaryTreeNode(), v4 = new BinaryTreeNode(), v5 = new BinaryTreeNode(),
                v6 = new BinaryTreeNode(), v7 = new BinaryTreeNode();
            SetLChild(v1, v2); SetRChild(v1, v3);
            SetLChild(v2, v4); SetRChild(v2, v5);
            SetLChild(v5, v7);
            SetLChild(v3, v6);

            Assert.AreEqual(BinaryTreeNode.Null, v3.Successor());
            Assert.AreEqual(v1, v5.Successor());
            Assert.AreEqual(v7, v2.Successor());
            Assert.AreEqual(v6, v1.Successor());

            Assert.AreEqual(BinaryTreeNode.Null, v4.Predecessor());
            Assert.AreEqual(v5, v1.Predecessor());
            Assert.AreEqual(v2, v7.Predecessor());
            Assert.AreEqual(v1, v6.Predecessor());
        }

        private void SetLChild(BinaryTreeNode parent,BinaryTreeNode child)
        {
            parent.LeftChild = child;
            child.Parent = parent;
        }
        private void SetRChild(BinaryTreeNode parent, BinaryTreeNode child)
        {
            parent.RightChild = child;
            child.Parent = parent;
        }
    }
}