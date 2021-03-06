﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Advanced.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced.Tests
{
    [TestClass()]
    public class BinarySearchTreeTests
    {
        BinarySearchTree<BSTValueNode, int> tree = new BinarySearchTree<BSTValueNode, int>() { SupportEquatable = true, KeepInsertOrder = true };
        List<BSTValueNode> nodes = new List<BSTValueNode>();

        void NewNode(int value)
        {
            var node = new BSTValueNode(value);
            tree.InsertNode(node);
            nodes.Add(node);
        }
        [TestInitialize]
        public void Generate()
        {
            tree.Clear(); nodes.Clear();
            NewNode(4); NewNode(6); NewNode(8);
            NewNode(4); NewNode(3); NewNode(2);
            NewNode(4); NewNode(9); NewNode(7);
            NewNode(4);
        }

        [TestMethod()]
        public void SearchTest()
        {
            Assert.AreEqual(4, nodes.IndexOf(nodes.Where(node => node.Key == 3).First()));
        }

        [TestMethod()]
        public void DeleteTest()
        {
            Assert.IsTrue(nodes.Remove(tree.DeleteNode(6)));
            Assert.IsTrue(nodes.Remove(tree.DeleteNode(4)));
            Assert.IsTrue(nodes.Remove(tree.DeleteNode(2)));
            Assert.AreEqual(null, tree.DeleteNode(5));
        }

        [TestMethod()]
        public void SuccessorTest()
        {
            Assert.AreEqual(6,tree.Successor(4).Key);
        }

        [TestMethod()]
        public void PredecessorTest()
        {
            Assert.AreEqual(3, tree.Predecessor(4).Key);
        }

        [TestMethod()]
        public void SearchAllTest()
        {
            if (tree.KeepInsertOrder)
                Assert.IsTrue(tree.SearchNodeAll(4).SequenceEqual(nodes.Where(val => val.Key == 4)));
            else
                Assert.AreEqual(0, tree.SearchNodeAll(4).Except(nodes.Where(val => val.Key == 4)).Count());
        }
    }


    class BSTValueNode : BinaryTreeNode, IKeyProvider<int>
    {
        static int order = 1;
        public int Data { get; set; }
        public int Key { get; set; }
        public BSTValueNode(int key) { Key = key; Data = order++; }

        public override string ToString()
        {
            return $"[{Key}]={Data}";
        }
    }
}