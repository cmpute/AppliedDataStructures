using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Advanced.Linear;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced.Tests
{
    [TestClass()]
    public class UnrolledLinkedListTests : RangeListTestBase<UnrolledLinkedList<UnrolledLinkedListNode<int>, int>>
    {
        const int cycles = 20;
        protected override int Cycles => cycles;

        public UnrolledLinkedListTests() : base(new UnrolledLinkedList<UnrolledLinkedListNode<int>, int>(size => new UnrolledLinkedListNode<int>(size), 2 * cycles)) { }

        [TestInitialize]
        [TestMethod]
        public override void RandomGenerate() => base.RandomGenerate();

        [TestMethod]
        public override void GetEnumeratorTest()
        {
            base.GetEnumeratorTest();

            Compare.Reverse();
            var listiter = List.GetEnumerator(true);
            foreach (var item in Compare)
            {
                Assert.AreEqual(true, listiter.MoveNext());
                Assert.AreEqual(item, listiter.Current);
            }
            Assert.AreEqual(false, listiter.MoveNext());
        }

        [TestMethod]
        public override void AccessTest()
        {
            base.AccessTest();
#if DEBUG
            Assert.IsTrue(List.VerifyHotNode());
#endif
        }

        [TestMethod()]
        public override void AddTest() => base.AddTest();

        [TestMethod()]
        public override void ClearTest() => base.ClearTest();

        [TestMethod()]
        public override void ContainsTest() => base.ContainsTest();

        [TestMethod()]
        public override void CopyToTest() => base.CopyToTest();

        [TestMethod()]
        public override void IndexOfTest()
        {
            base.IndexOfTest();

            int c = List[rand.Next(List.Count)];
            Assert.AreEqual(Compare.LastIndexOf(c), List.LastIndexOf(c));

            for (int i = 0; i < Cycles; i++)
            {
                c = rand.Next(100);
                Assert.AreEqual(Compare.LastIndexOf(c), List.LastIndexOf(c));
            }
        }

        [TestMethod()]
        public override void InsertTest() => base.InsertTest();

        [TestMethod()]
        public override void RemoveTest() => base.RemoveTest();

        [TestMethod()]
        public override void RemoveAtTest() => base.RemoveAtTest();

        [TestMethod()]
        public override void AddRangeTest() => base.AddRangeTest();

        [TestMethod()]
        public override void InsertRangeTest() => base.InsertRangeTest();

        [TestMethod()]
        public override void RemoveRangeTest()
        {
            while (Compare.Count > 2)
            {
                int start = rand.Next(Compare.Count);
                int count = rand.Next(Compare.Count - start);
#if DEBUG
                Assert.IsTrue(List.VerifyHotNode());
#endif
                Console.Write('[');
                Compare.PrintTo(Console.Out);
                Console.WriteLine(']');
                List.PrintTo(Console.Out);
                Console.WriteLine();
                var a = Compare.GetRange(start, count);
                var b = List.GetRange(start, count);

                Console.Write('[');
                a.PrintTo(Console.Out);
                Console.WriteLine('|');
                b.PrintTo(Console.Out);
                Console.WriteLine(']');
                Assert.IsTrue(a.SequenceEqual(b));
                List.PrintTo(Console.Out);
                Console.WriteLine(']');

#if DEBUG
                Assert.IsTrue(List.VerifyHotNode());
#endif
                Console.WriteLine($"Remove {count} starting from [{start}]");
                List.RemoveRange(start, count);
                Compare.RemoveRange(start, count);

                Assert.IsTrue(Compare.SequenceEqual(List));
            }
        }

        [TestMethod()]
        public override void GetRangeTest() => base.GetRangeTest();

        public void PrintList()
        {
            Console.Write('[');
            Compare.PrintTo(Console.Out);
            Console.WriteLine(']');
            List.PrintTo(Console.Out);
            Console.WriteLine();
        }
    }
}