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
    public class ThreadedBinaryTreeNodeTests
    {
        BinarySearchTree<ThreadedBSTNode, int> tree = new BinarySearchTree<ThreadedBSTNode, int>();

        void Check()
        {
            var order = SuccessorTest();
            var reverse = PredecessorTest();
            Assert.IsTrue(order.SequenceEqual(tree));
            Assert.IsTrue(reverse.Reverse().SequenceEqual(tree));
        }
        void NewNode(int value)
        {
            var node = new ThreadedBSTNode() { Key = value };
            tree.Insert(node);

            Check();
        }
        [TestMethod]
        public void Generate()
        {
            tree.Clear();
            NewNode(4); NewNode(6); NewNode(8);
            NewNode(4); NewNode(3); NewNode(2);
            NewNode(4); NewNode(9); NewNode(7);
            NewNode(4);

            tree.Delete(4); Check();
            tree.Delete(2); Check();
            tree.Delete(7); Check();
        }

        private IEnumerable<ThreadedBSTNode> SuccessorTest()
        {
            var c = tree.Root;
            while (c.LeftChild!= null)
                c = c.LeftChild as ThreadedBSTNode;

            while (c != null)
            {
                yield return c;
                c = c.Successor() as ThreadedBSTNode;
            }
        }

        private IEnumerable<ThreadedBSTNode> PredecessorTest()
        {
            var c = tree.Root;
            while (c.RightChild != null)
                c = c.RightChild as ThreadedBSTNode;

            while (c != null)
            {
                yield return c;
                c = c.Predecessor() as ThreadedBSTNode;
            }
        }
    }

    class ThreadedBSTNode : ThreadedBinaryTreeNode, IKeyProvider<int>
    {
        public int Key { get; set; }
        public override string ToString() => "Key=" + Key;
    }
}