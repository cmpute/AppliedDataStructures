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
    public class UnrolledLinkedListTests
    {
        UnrolledLinkedList<UnrolledLinkedListNode<int>, int> list = new UnrolledLinkedList<UnrolledLinkedListNode<int>, int>(size => new UnrolledLinkedListNode<int>(size), 2 * cycnum);
        List<int> compare = new List<int>();
        Random r = new Random();
        const int cycnum = 10;

        [TestInitialize]
        [TestMethod]
        public void RandomGenerate()
        {
            compare.Clear();
            list.Clear();
            for (int i = 0; i < cycnum; i++)
            {
                var c = r.Next(100);
                compare.Add(c);
                list.Add(c);

                Assert.IsTrue(compare.SequenceEqual(list));
                Console.Write(c + "\t");
            }

            for (int i = 0; i < cycnum; i++)
            {
                var c = r.Next(100);
                compare.Add(c);
                list.Add(c);

                Assert.IsTrue(compare.SequenceEqual(list));
                Console.Write(c + "\t");
            }

            Console.WriteLine("<.");
            PrintList();

            Assert.AreEqual(2 * cycnum, list.Count);
            Assert.IsTrue(compare.SequenceEqual(list));
        }

        [TestMethod]
        public void EnumerateTest()
        {
            var itor = list.GetEnumerator();
            var compareitor = compare.GetEnumerator();
            bool flag1 = compareitor.MoveNext(), flag2 = itor.MoveNext();
            Assert.AreEqual(flag1, flag2);
            while (flag1 && flag2)
            {
                Assert.AreEqual(compareitor.Current, itor.Current);
                flag1 = compareitor.MoveNext();
                flag2 = itor.MoveNext();
                Assert.AreEqual(flag1, flag2);
            }

            compare.Reverse();
            var revitor = list.GetEnumerator(true);
            var revcompareitor = compare.GetEnumerator();
            flag1 = revcompareitor.MoveNext(); flag2 = revitor.MoveNext();
            Assert.AreEqual(flag1, flag2);
            while (flag1 && flag2)
            {
                Assert.AreEqual(revcompareitor.Current, revitor.Current);
                flag1 = revcompareitor.MoveNext();
                flag2 = revitor.MoveNext();
                Assert.AreEqual(flag1, flag2);
            }
        }

        [TestMethod]
        public void AccessTest()
        {
            for (int i = 0; i < 2 * cycnum; i++)
                Assert.AreEqual(compare[i], list[i]);
        }

        [TestMethod()]
        public void OperateAtTest()
        {
            list.OperateAt(5, (ref int x) => x++);
            list[6]++;
            compare[5]++;
            compare[6]++;
            Assert.IsTrue(compare.SequenceEqual(list));
        }

        [TestMethod()]
        public void AddTest()
        {
            for (int i = 0; i < cycnum; i++)
            {
                var c = r.Next(100);
                list.Add(c); compare.Add(c);
                Assert.IsTrue(compare.SequenceEqual(list));
            }
        }

        [TestMethod()]
        public void ClearTest()
        {
            list.Clear();
            Assert.AreEqual(0, list.Count);
            Assert.AreEqual(0, list.Count());
        }

        [TestMethod()]
        public void ContainsTest()
        {
            int c = list[r.Next(list.Count)];
            Assert.AreEqual(compare.Contains(c), list.Contains(c));

            for (int i = 0; i < cycnum; i++)
            {
                c = r.Next(100);
                Assert.AreEqual(compare.Contains(c), list.Contains(c));
            }
        }

        [TestMethod()]
        public void CopyToTest()
        {
            int[] target = new int[2 * cycnum];
            list.CopyTo(target, 0);
            Assert.IsTrue(compare.SequenceEqual(target));
        }

        [TestMethod()]
        public void IndexOfTest()
        {
            int c = list[r.Next(list.Count)];
            Assert.AreEqual(compare.IndexOf(c), list.IndexOf(c));
            Assert.AreEqual(compare.LastIndexOf(c), list.LastIndexOf(c));

            for (int i = 0; i < cycnum; i++)
            {
                c = r.Next(100);
                Assert.AreEqual(compare.LastIndexOf(c), list.LastIndexOf(c));
            }
        }

        [TestMethod()]
        public void InsertTest()
        {
            for (int i = 0; i < cycnum; i++)
            {
                var c = r.Next(100);
                var index = r.Next(list.Count);
                list.Insert(index, c); compare.Insert(index, c);

                Console.Write("Insert at " + index + ": ");
                PrintList();
            }
            Assert.IsTrue(compare.SequenceEqual(list));
        }

        [TestMethod()]
        public void RemoveTest()
        {
            for (int i = 0; i < cycnum; i++)
            {
                var val = compare[r.Next(list.Count)];
                Assert.AreEqual(compare.Remove(val), list.Remove(val));

                Console.Write("Delete " + val + ": ");
                PrintList();

                Assert.IsTrue(compare.SequenceEqual(list));
            }
            Assert.IsTrue(compare.SequenceEqual(list));
        }

        [TestMethod()]
        public void RemoveAtTest()
        {
            for (int i = 0; i < cycnum; i++)
            {
                var index = r.Next(list.Count);
                compare.RemoveAt(index); list.RemoveAt(index);

                Console.Write("Delete at " + index + ": ");
                PrintList();

                Assert.IsTrue(compare.SequenceEqual(list));
            }

            Assert.AreEqual(compare.Count, list.Count);
            Assert.IsTrue(compare.SequenceEqual(list));
        }

        public void PrintList()
        {
            list.PrintTo(Console.Out);
            Console.WriteLine();
        }
    }
}