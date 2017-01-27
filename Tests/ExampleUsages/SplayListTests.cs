using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic.Tests
{
    [TestClass()]
    public class SplayListTests : RangeListTestBase<SplayList<int>>
    {
        public SplayListTests() : base(new SplayList<int>()) { }

        [TestInitialize]
        [TestMethod]
        public override void RandomGenerate() => base.RandomGenerate();

        [TestMethod()]
        public override void AddTest() => base.AddTest();

        [TestMethod()]
        public override void ClearTest() => base.ClearTest();

        [TestMethod()]
        public override void ContainsTest() => base.ContainsTest();

        [TestMethod()]
        public override void CopyToTest() => base.CopyToTest();

        [TestMethod()]
        public override void GetEnumeratorTest() => base.GetEnumeratorTest();

        [TestMethod()]
        public override void AccessTest() => base.AccessTest();

        [TestMethod()]
        public override void IndexOfTest() => base.IndexOfTest();

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
        public override void RemoveRangeTest() => base.RemoveRangeTest();

        [TestMethod()]
        public void OperateRangeTest()
        {
            int start = rand.Next(List.Count);
            int count = rand.Next(List.Count - start);
            int add = rand.Next(100);

            (List as SplayList<int>).OperateRange(start, count, (ref int target) => target += add);
            for (int i = 0; i < count; i++)
                Compare[start + i] += add;

            Assert.IsTrue(Compare.SequenceEqual(List));
        }

        [TestMethod()]
        public override void ReverseTest() => base.ReverseTest();
    }
}