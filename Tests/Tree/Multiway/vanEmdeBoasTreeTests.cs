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
        const int _size = 1 << 6;

        [TestInitialize]
        [TestMethod]
        public void Generate()
        {
            compare = new Dictionary<int, int>(_size);
            tree = new vanEmdeBoasTree<vanEmdeBoasTreeNode<int>, int>(_size, (size) => new vanEmdeBoasTreeNode<int>(size));
            for (int i = 0; i < _size; i++)
            {
                if (r.Next() % 2 == 0)
                    continue;
                var value = r.Next();
                compare.Add(i, value);
                tree.InsertNode(i, value);
            }
            
            EqualTest(_size);
        }

        public void EqualTest(int size)
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
            for (int i = 0; i < _size; i++)
            {
                if (r.Next() % 2 == 0)
                    continue;
                if (!compare.ContainsKey(i))
                    continue;

                var rc = compare[i]; int rr;
                Assert.AreEqual(compare.Remove(i), tree.DeleteNode(i, out rr));
                Assert.AreEqual(rc, rr);
            }

            EqualTest(_size);
        }

        [TestMethod()]
        public void SearchNodeTest()
        {
            for (int i = 0; i < _size; i++)
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

            for (int i = 0; i < _size; i++)
            {
                if (r.Next() % 2 == 0)
                    continue;
                var value = r.Next();
                compare.Add(i, value);
                tree.InsertNode(i, value);
            }

            EqualTest(_size);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            for (int i = 0; i < _size; i++)
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

            EqualTest(_size);
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

        [TestMethod()]
        public void TrimExcessTest()
        {
            //ensures size = 8
            var value = r.Next();
            if (!compare.ContainsKey(7))
            {
                compare.Add(7, value);
                tree.InsertNode(7, value);
            }

            for (int i = 8; i < _size; i++)
            {
                var tempdic = tree.ToDictionary();
                if (!compare.ContainsKey(i))
                    continue;
                compare.Remove(i);
                tree.DeleteNode(i);
            }

            tree.TrimExcess();

            Assert.AreEqual(8, tree.Capacity);
            EqualTest(8);

            if (!compare.ContainsKey(3))
            {
                value = r.Next();
                compare.Add(3, value);
                tree.InsertNode(3, value);
            }

            for (int i = 4; i < 8; i++)
            {
                if (!compare.ContainsKey(i))
                    continue;
                compare.Remove(i);
                tree.DeleteNode(i);
            }
            
            tree.TrimExcess();

            Assert.AreEqual(4, tree.Capacity);
            EqualTest(4);
        }

        [TestMethod()]
        public void ExpandTest()
        {
            var value = r.Next();
            compare.Add(200, value);
            tree.InsertNode(200, value);

            EqualTest(_size * _size);

            compare.Remove(200);
            Assert.AreEqual(value, tree.DeleteNode(200));

            tree.TrimExcess();
            EqualTest(_size);
        }
    }
}