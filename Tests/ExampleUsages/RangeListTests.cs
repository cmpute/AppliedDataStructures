using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic.Tests
{
    [TestClass()]
    public class RangeListTests
    {
        RangeList<int> list = new RangeList<int>();
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
                Console.Write(c + "\t");
            }
            list.AddRange(compare);

            for (int i = 0; i < cycnum; i++)
            {
                var c = r.Next(100);
                list.Add(c); compare.Add(c);
                Console.Write(c + "\t");
            }
            Console.WriteLine("<.");

            Assert.AreEqual(cycnum * 2, list.Count);
            Assert.IsTrue(compare.SequenceEqual(list));
        }

        [TestMethod()]
        public void AddTest()
        {
            for (int i = 0; i < cycnum; i++)
            {
                var c = r.Next(100);
                list.Add(c); compare.Add(c);
            }
            Assert.IsTrue(compare.SequenceEqual(list));
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
        public void GetEnumeratorTest()
        {
            var iter = compare.GetEnumerator();
            foreach(var item in list)
            {
                iter.MoveNext();
                Assert.AreEqual(iter.Current, item);
            }
        }

        [TestMethod()]
        public void GetSetTest()
        {
            for (int i = 0; i < 2 * cycnum; i++)
                Assert.AreEqual(compare[i], list[i]);

            for (int i = 0; i < 2 * cycnum; i++)
            {
                var c = r.Next(100);
                compare[i] = c; list[i] = c;
            }

            for (int i = 0; i < 2 * cycnum; i++)
                Assert.AreEqual(compare[i], list[i]);
        }

        [TestMethod()]
        public void IndexOfTest()
        {
            int c = list[r.Next(list.Count)];
            Assert.AreEqual(compare.IndexOf(c), list.IndexOf(c));

            for (int i = 0; i < cycnum; i++)
            {
                c = r.Next(100);
                Assert.AreEqual(compare.IndexOf(c), list.IndexOf(c));
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
                Assert.IsTrue(compare.SequenceEqual(list));
            }
            Assert.IsTrue(compare.SequenceEqual(list));
        }

        [TestMethod()]
        public void AddRangeTest()
        {
            List<int> temp = new List<int>();
            for (int i = 0; i < cycnum; i++)
                temp.Add(r.Next(100));

            list.AddRange(temp);
            compare.AddRange(temp);

            Assert.IsTrue(compare.SequenceEqual(list));
        }

        [TestMethod()]
        public void InsertRangeTest()
        {
            List<int> temp = new List<int>();
            for (int i = 0; i < cycnum; i++)
                temp.Add(r.Next(100));

            var index = r.Next(list.Count);
            compare.InsertRange(index, temp);
            list.InsertRange(index, temp);

            Assert.IsTrue(compare.SequenceEqual(list));
        }

        [TestMethod()]
        public void RemoveRangeTest()
        {
            int start = r.Next(list.Count);
            int count = r.Next(list.Count - start);
            list.RemoveRange(start, count);
            compare.RemoveRange(start, count);

            Assert.IsTrue(compare.SequenceEqual(list));
        }

        [TestMethod()]
        public void OperateRangeTest()
        {
            int start = r.Next(list.Count);
            int count = r.Next(list.Count - start);
            int add = r.Next(100);

            list.OperateRange(start, count, (ref int target) => target += add);
            for (int i = 0; i < count; i++)
                compare[start + i] += add;

            Assert.IsTrue(compare.SequenceEqual(list));
        }

        [TestMethod()]
        public void ReverseTest()
        {
            list.Reverse();
            compare.Reverse();
            
            Assert.IsTrue(compare.SequenceEqual(list));

            int start = r.Next(list.Count);
            int count = r.Next(list.Count - start);
            list.Reverse(start, count);
            compare.Reverse(start, count);

            Assert.IsTrue(compare.SequenceEqual(list));
        }

        public void CompareLog()
        {
            Console.WriteLine("[");
            foreach (var item in list)
                Console.Write(item + "\t");
            Console.WriteLine("|");
            foreach (var item in compare)
                Console.Write(item + "\t");
            Console.WriteLine("]");
        }
    }
}