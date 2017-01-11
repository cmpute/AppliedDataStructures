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

        [TestInitialize]
        [TestMethod]
        public void RandomGenerate()
        {
            List<int> compare = new List<int>();
            list.Clear();
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                var c = r.Next(100);
                compare.Add(c);
                Console.Write(c + "\t");
            }
            list.AddRange(compare);

            for (int i = 0; i < 10; i++)
            {
                var c = r.Next(100);
                list.Add(c); compare.Add(c);
                Console.Write(c + "\t");
            }
            Console.WriteLine("<.");

            Assert.AreEqual(20, list.Count);
            Assert.IsTrue(compare.SequenceEqual(list));
        }

        [TestMethod()]
        public void AddTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ClearTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ContainsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CopyToTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetEnumeratorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IndexOfTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InsertTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveAtTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddRangeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InsertRangeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveRangeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RangeOperationTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReverseTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReverseTest1()
        {
            Assert.Fail();
        }
    }
}