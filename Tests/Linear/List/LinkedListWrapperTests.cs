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
    public class LinkedListWrapperTests : ListTestBase<IList<int>>
    {

        public LinkedListWrapperTests() : base(new LinkedList<int>().AsList()) { }

        [TestInitialize]
        [TestMethod]
        public override void RandomGenerate() => base.RandomGenerate();

        [TestMethod]
        public override void AccessTest() => base.AccessTest();

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
        public override void IndexOfTest() => base.IndexOfTest();

        [TestMethod()]
        public override void InsertTest() => base.InsertTest();

        [TestMethod()]
        public override void RemoveTest() => base.RemoveTest();

        [TestMethod()]
        public override void RemoveAtTest() => base.RemoveAtTest();
    }
}