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
    public class TreapTests
    {
        Treap<BSTValueNode, int> tree = null;
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
                temp.Add(new BSTValueNode(r.Next(100)) { Data = r.Next(100) });

            temp.Sort((node1, node2) => Comparer<int>.Default.Compare(node1.Key, node2.Key));
            tree = new Treap<BSTValueNode, int>(temp, (node1, node2) => node1.Data - node2.Data) { SupportEquatable = true };
            compare.AddRange(temp);

            CompareLog();
            Assert.IsTrue(compare.OrderBy(node => node.Key).SequenceEqual(tree));
        }

        [TestMethod]
        public void InsertTest()
        {
            for (int i = 0; i < cycnum; i++)
            {
                var node = new BSTValueNode(r.Next(100)) { Data = r.Next(100) };
                compare.Add(node);
                tree.InsertNode(node);
            }

            CompareLog();
            Assert.IsTrue(compare.OrderBy(node => node.Key).SequenceEqual(tree));
        }

        [TestMethod]
        public void DeleteTest()
        {
            List<BSTValueNode> del = new List<BSTValueNode>();
            foreach (var node in tree)
                if (r.Next() % 2 == 0)
                    del.Add(node);
            
            foreach(var node in del)
            {
                Console.WriteLine($"delete [{node.Key}]{node.Data}");

                compare.Remove(node);
                tree.DeleteNode(node);

                CompareLog();
            }

            Assert.IsTrue(compare.OrderBy(node => node.Key).SequenceEqual(tree));
        }

        [TestMethod()]
        public void MinTest()
        {
            Assert.AreEqual(compare.Min(node => node.Data), tree.Min().Data);
        }

        [TestMethod()]
        public void ExtractMinTest()
        {
            while (tree.Count > 0)
                Assert.AreEqual(tree.Min(), tree.ExtractMin());
        }

        [TestMethod()]
        public void PriorityUpdateTest()
        {
            var change = new List<BSTValueNode>();
            foreach (var node in tree)
                if (r.Next() % 2 == 0)
                    change.Add(node);

            foreach(var node in change)
            {
                node.Data += r.Next(100) - r.Next(100);
                tree.PriorityUpdate(node);
                 
                CompareLog();
                Assert.IsTrue(compare.OrderBy(t => t.Key).SequenceEqual(tree));
            }
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
                Console.Write($"[{item.Key}]{item.Data}\t");
            Console.WriteLine("|");
            foreach (var item in compare)
                Console.Write($"[{item.Key}]{item.Data}\t");
            Console.WriteLine("]");
        }
    }
}