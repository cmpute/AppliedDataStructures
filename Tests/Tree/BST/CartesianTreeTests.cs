using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Advanced.Tree.BinaryTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced.Tests
{
    [TestClass()]
    public class CartesianTreeTests
    {
        CartesianTree<BSTValueNode, int> tree = null;
        List<BSTValueNode> compare = new List<BSTValueNode>();
        const int cycnum = 10;
        Random r = new Random();

        [TestInitialize]
        [TestMethod()]
        public void Generate()
        {
            compare.Clear();

            List<BSTValueNode> temp = new List<BSTValueNode>();
            for (int i = 0; i < cycnum; i++)
                temp.Add(new BSTValueNode(i) { Data = r.Next(100) });

            tree = new CartesianTree<BSTValueNode, int>(temp, (node1, node2) => node1.Data - node2.Data);
            compare.AddRange(temp);
            CompareLog();
            Assert.IsTrue(temp.SequenceEqual(tree));
        }

        [TestMethod()]
        public void MinTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExtractMinTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PriorityUpdateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void MergeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void HeapTest()
        {
            foreach (var n in tree)
            {
                Assert.IsTrue(n.Data <= ((n.LeftChild as BSTValueNode)?.Data ?? 100000));
                Assert.IsTrue(n.Data <= ((n.RightChild as BSTValueNode)?.Data ?? 100000));
            }
        }

        public void CompareLog()
        {
            Console.Write("[");
            foreach (var item in tree)
                Console.Write($"[{item.Data}]{item.Key}\t");
            Console.WriteLine("|");
            foreach (var item in compare)
                Console.Write($"[{item.Data}]{item.Key}\t");
            Console.WriteLine("]");
        }
    }
}