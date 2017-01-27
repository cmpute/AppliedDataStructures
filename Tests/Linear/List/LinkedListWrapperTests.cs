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
    public class LinkedListWrapperTests
    {
        IList<int> list = new LinkedList<int>().AsList();
        List<int> compare = new List<int>();
        Random r = new Random();
        const int cycnum = 10;

        [TestInitialize]
        [TestMethod]
        public void RandomGenerate()
        {
            compare.Clear();
            list.Clear();
            for (int i = 0; i < 2 * cycnum; i++)
            {
                var c = r.Next(100);
                compare.Add(c);
                list.Add(c);
                Console.Write(c + "\t");
            }

            Assert.AreEqual(cycnum * 2, list.Count);
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
    }
}