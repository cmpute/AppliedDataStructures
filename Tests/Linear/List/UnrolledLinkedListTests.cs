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
    public class UnrolledLinkedListTests : ListTestBase<UnrolledLinkedList<UnrolledLinkedListNode<int>, int>>
    {
        const int cycles = 10;
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
        public override void AccessTest() => base.AccessTest();

        [TestMethod()]
        public void OperateAtTest()
        {
            List.OperateAt(5, (ref int x) => x++);
            List[6]++;
            Compare[5]++;
            Compare[6]++;
            Assert.IsTrue(Compare.SequenceEqual(List));
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

        public void PrintList()
        {
            List.PrintTo(Console.Out);
            Console.WriteLine();
        }
    }
}