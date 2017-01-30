using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Advanced.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Node = System.Collections.Advanced.Tree.CartesianTreeNode<int>;

namespace System.Collections.Advanced.Tests
{
    [TestClass()]
    public class CartesianTreeTests
    {
        CartesianTree<Node, int> tree = null;
        List<Node> compare = new List<Node>();
        const int cycnum = 10;
        Random r = new Random();

        [TestInitialize]
        [TestMethod()]
        public void Generate()
        {
            compare.Clear();

            List<Node> temp = new List<Node>();
            for (int i = 0; i < cycnum; i++)
                temp.Add(new Node(r.Next(100)));

            temp.Sort((node1, node2) => Comparer<int>.Default.Compare(node1.Key, node2.Key));
            tree = new CartesianTree<Node, int>(temp) { SupportEquatable = true };
            compare.AddRange(temp);

            Assert.IsTrue(compare.OrderBy(node => node.Key).SequenceEqual(tree));
        }

        [TestMethod]
        public void ModificationTest()
        {
            for (int i = 0; i < cycnum; i++)
            {
                var node = new Node(r.Next(100));
                compare.Add(node);
                tree.InsertNode(node);
            }

            Assert.IsTrue(compare.OrderBy(node => node.Key).SequenceEqual(tree));

            List<Node> del = new List<Node>();
            foreach (var node in tree)
                if (r.Next() % 2 == 0)
                    del.Add(node);

            foreach (var node in del)
            {
                compare.Remove(node);
                tree.DeleteNode(node);
            }

            Assert.IsTrue(compare.OrderBy(node => node.Key).SequenceEqual(tree));
        }
    }
}