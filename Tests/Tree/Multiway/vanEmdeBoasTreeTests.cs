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
    public class vanEmdeBoasTreeTests
    {
        Dictionary<int, int> compare;
        vanEmdeBoasTree<vanEmdeBoasTreeNode<int>, int> tree;
        Random r = new Random();
        const int size = 64;

        [TestInitialize]
        [TestMethod]
        public void Generate()
        {
            compare = new Dictionary<int, int>(size);
            tree = new vanEmdeBoasTree<vanEmdeBoasTreeNode<int>, int>(size, (size) => new vanEmdeBoasTreeNode<int>(size));
            for (int i = 0; i < size; i++)
            {
                if (r.Next() % 2 == 0)
                    continue;
                var value = r.Next();
                compare.Add(i, value);
                tree.InsertNode(i, value);
            }
            
            EqualTest();
        }

        public void EqualTest()
        {
            Assert.AreEqual(size, tree.Capacity);
            Assert.AreEqual(compare.Count, tree.Count);
            for (int i = 0; i < size; i++)
            {
                if (compare.ContainsKey(i))
                {
                    Assert.IsTrue(tree.Contains(i));
                    Assert.AreEqual(compare[i], tree.SearchNode(i));
                }
            }
        }

        [TestMethod()]
        public void DeleteNodeTest()
        {
            for (int i = 0; i < size; i++)
            {
                if (r.Next() % 2 == 0)
                    continue;
                if (!compare.ContainsKey(i))
                    continue;

                var rc = compare[i]; int rr;
                Assert.AreEqual(compare.Remove(i), tree.DeleteNode(i, out rr));
                Assert.AreEqual(rc, rr);
            }

            EqualTest();
        }

        [TestMethod()]
        public void SearchNodeTest()
        {
            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(compare.ContainsKey(i), tree.Contains(i));
                if (compare.ContainsKey(i))
                    Assert.AreEqual(compare[i], tree.SearchNode(i));
            }
        }

        [TestMethod()]
        public void ClearTest()
        {
            tree.Clear();
            compare.Clear();

            Assert.AreEqual(0, tree.Count);

            for (int i = 0; i < size; i++)
            {
                if (r.Next() % 2 == 0)
                    continue;
                var value = r.Next();
                compare.Add(i, value);
                tree.InsertNode(i, value);
            }

            EqualTest();
        }

        [TestMethod()]
        public void UpdateTest()
        {
            for (int i = 0; i < size; i++)
            {
                if (r.Next() % 2 == 0)
                    continue;

                var value = r.Next();
                Assert.AreEqual(compare.ContainsKey(i), !tree.InsertNode(i, value));
                if (compare.ContainsKey(i))
                    compare[i] = value;
                else
                    compare.Add(i, value);
            }

            EqualTest();
        }

        [TestMethod]
        public void ToDictionaryTest()
        {
            Assert.IsTrue(tree.ToDictionary().SequenceEqual(compare));
        }

        [TestMethod]
        public void SuccAndPredTest()
        {
            List<int> keycol = new List<int>();
            int? key = 0;

            if (tree.Contains(0)) keycol.Add(0);
            while ((key = tree.Successor(key.Value)) != null)
                keycol.Add(key.Value);

            Assert.IsTrue(keycol.SequenceEqual(compare.Keys));

            keycol.Clear();
            key = tree.Capacity;
            while ((key = tree.Predecessor(key.Value)) != null)
                keycol.Add(key.Value);

            Assert.IsTrue(keycol.SequenceEqual(compare.Keys.Reverse()));
        }

        //[TestMethod()]
        //public void TrimExcessTest()
        //{
        //    Assert.Fail();
        //}
    }
}