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
    public class SortingExTests
    {
        [TestMethod()]
        public void HeapSortTest()
        {
            Random r = new Random();
            List<int> target = new List<int>();
            for (int i = 0; i < 100; i++)
                target.Add(r.Next());

            var res = target.HeapSort();
            target.Sort();

            Assert.IsTrue(res.SequenceEqual(target));
        }
    }
}