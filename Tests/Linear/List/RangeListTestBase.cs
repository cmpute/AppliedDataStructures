using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Advanced.Linear;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections
{
    public class RangeListTestBase<TList> : ListTestBase<TList> where TList : IRangeList<int>
    {

        public RangeListTestBase(TList list) : base(list) { }

        public virtual void AddRangeTest()
        {
            List<int> temp = new List<int>();
            for (int i = 0; i < Cycles; i++)
                temp.Add(rand.Next(100));

            List.AddRange(temp);
            Compare.AddRange(temp);

            Assert.IsTrue(Compare.SequenceEqual(List));
        }

        public virtual void InsertRangeTest()
        {
            List<int> temp = new List<int>();
            for (int i = 0; i < Cycles; i++)
                temp.Add(rand.Next(100));

            var index = rand.Next(List.Count);
            Compare.InsertRange(index, temp);
            List.InsertRange(index, temp);

            Assert.IsTrue(Compare.SequenceEqual(List));
        }

        public virtual void RemoveRangeTest()
        {
            while (Compare.Count > 2)
            {
                int start = rand.Next(Compare.Count);
                int count = rand.Next(Compare.Count - start);

                var a = Compare.GetRange(start, count);
                var b = List.GetRange(start, count);
                Assert.IsTrue(a.SequenceEqual(b));

                List.RemoveRange(start, count);
                Compare.RemoveRange(start, count);

                Assert.IsTrue(Compare.SequenceEqual(List));
            }
        }

        public virtual void ReverseTest()
        {
            List.Reverse();
            Compare.Reverse();

            Assert.IsTrue(Compare.SequenceEqual(List));

            int start = rand.Next(List.Count);
            int count = rand.Next(List.Count - start);
            List.Reverse(start, count);
            Compare.Reverse(start, count);

            Assert.IsTrue(Compare.SequenceEqual(List));
        }

        public virtual void GetRangeTest()
        {
            for (int i = 0; i < 3 * Cycles; i++)
            {
                int start = rand.Next(Compare.Count);
                int count = rand.Next(Compare.Count - start);

                var a = Compare.GetRange(start, count);
                var b = List.GetRange(start, count);
                Assert.IsTrue(a.SequenceEqual(b));
            }
        }
    }
}
