﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Advanced.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Node = System.Collections.Advanced.Tree.AVLTreeNode<int>;

namespace System.Collections.Advanced.Tests
{
    [TestClass()]
    public class AVLTreeTests
    {
        AVLTree<Node, int> tree = new AVLTree<Node, int>();
        const int nodenum = 20;

        [TestMethod()]
        public void RandomlyInsertTest()
        {
            var list = new SortedList<int, int>();
            Random r = new Random();
            for (int i = 0; i < nodenum; i++)
            {
                int c = r.Next();
                list.Add(c, c);
                tree.InsertNode(new Node { Key = c });
                Assert.IsTrue(tree.Select((node) => node.Key).SequenceEqual(list.Select((pair) => pair.Value)));
                CheckBalance();
            }
        }

        [TestMethod()]
        public void OrderlyInsertTest()
        {
            tree.Clear();
            int j = 0;
            for (int i = 0; i < nodenum; i++, j = 0)
            {
                tree.InsertNode(new Node() { Key = i });
                CheckBalance();
                foreach (var t in tree)
                {
                    Assert.AreEqual(j++, t.Key);
                    Console.Write($"[{t.Key}|h={t.Height}]\t");
                }
                Console.WriteLine("<.");
            }

            for (int i = nodenum - 1; i >= 0; i--, j = 0)
            {
                tree.DeleteNode(i);
                CheckBalance();
                foreach (var t in tree)
                {
                    Assert.AreEqual(j++, t.Key);
                    Console.Write($"[{t.Key}|h={t.Height}]\t");
                }
                Console.WriteLine("<.");
            }
        }

        public void CheckBalance()
        {
            Assert.AreEqual(0, tree.Select(node => Math.Abs(node.BalanceFactor) <= 1).Count(res => !res));
        }
    }
}