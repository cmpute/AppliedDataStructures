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
        Treap<TreapValueNode, int, int> tree = null;
        List<TreapValueNode> compare = new List<TreapValueNode>();
        const int cycnum = 10;
        Random r = new Random();

        [TestInitialize]
        [TestMethod()]
        public void Generate()
        {
            compare.Clear();

            List<TreapValueNode> temp = new List<TreapValueNode>();
            for (int i = 0; i < cycnum; i++)
                temp.Add(new TreapValueNode(r.Next(100)) { Priority = r.Next(100) });

            temp.Sort((node1, node2) => Comparer<int>.Default.Compare(node1.Key, node2.Key));
            tree = new Treap<TreapValueNode, int, int>(temp) { SupportEquatable = true };
            compare.AddRange(temp);

            CompareLog();
            Assert.IsTrue(compare.OrderBy(node => node.Key).SequenceEqual(tree));
        }

        [TestMethod]
        public void InsertTest()
        {
            for (int i = 0; i < cycnum; i++)
            {
                var node = new TreapValueNode(r.Next(100)) { Priority = r.Next(100) };
                compare.Add(node);
                tree.InsertNode(node);
            }

            CompareLog();
            Assert.IsTrue(compare.OrderBy(node => node.Key).SequenceEqual(tree));
        }

        [TestMethod]
        public void DeleteTest()
        {
            List<TreapValueNode> del = new List<TreapValueNode>();
            foreach (var node in tree)
                if (r.Next() % 2 == 0)
                    del.Add(node);
            
            foreach(var node in del)
            {
                Console.WriteLine($"delete [{node.Key}]{node.Priority}");

                compare.Remove(node);
                tree.DeleteNode(node);

                CompareLog();
            }

            Assert.IsTrue(compare.OrderBy(node => node.Key).SequenceEqual(tree));
        }

        [TestMethod()]
        public void MinTest()
        {
            Assert.AreEqual(compare.Min(node => node.Priority), tree.Min().Priority);
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
            var change = new List<TreapValueNode>();
            foreach (var node in tree)
                if (r.Next() % 2 == 0)
                    change.Add(node);

            foreach(var node in change)
            {
                node.Priority += r.Next(100) - r.Next(100);
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
                Assert.IsTrue(n.Priority <= ((n.LeftChild as TreapValueNode)?.Priority ?? 100000));
                Assert.IsTrue(n.Priority <= ((n.RightChild as TreapValueNode)?.Priority ?? 100000));
            }
        }

        public void CompareLog()
        {
            Console.Write("[");
            foreach (var item in tree)
                Console.Write($"[{item.Key}]{item.Priority}\t");
            Console.WriteLine("|");
            foreach (var item in compare)
                Console.Write($"[{item.Key}]{item.Priority}\t");
            Console.WriteLine("]");
        }
    }
    class TreapValueNode : BinaryTreeNode, IKeyProvider<int>, IPriorityProvider<int>
    {
        static int order = 1;
        public int Priority { get; set; }
        public int Key { get; set; }
        public TreapValueNode(int key) { Key = key; Priority = order++; }

        public override string ToString()
        {
            return $"[{Key}]={Priority}";
        }
    }
}