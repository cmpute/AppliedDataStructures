using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections
{
    public class ListTestBase<TList> where TList : IList<int>
    {
        protected TList List { get; set; }
        List<int> _compare = new List<int>();
        protected List<int> Compare => _compare;
        protected Random rand = new Random();

        protected virtual int Cycles => 10;

        public ListTestBase(TList list)
        {
            List = list;
        }

        public virtual void RandomGenerate()
        {
            Compare.Clear();
            List.Clear();

            for (int i = 0; i < 2 * Cycles; i++)
            {
                var c = rand.Next(100);
                Compare.Add(c);
                List.Add(c);
                Console.Write(c + "\t");
            }
            Console.WriteLine("<.");

            Assert.AreEqual(Cycles * 2, List.Count);
            Assert.IsTrue(Compare.SequenceEqual(List));
        }

        public virtual void AddTest()
        {
            for (int i = 0; i < Cycles; i++)
            {
                var c = rand.Next(100);
                List.Add(c); Compare.Add(c);
                Assert.IsTrue(Compare.SequenceEqual(List));
            }
        }

        public virtual void ClearTest()
        {
            List.Clear();
            Assert.AreEqual(0, List.Count);
            Assert.AreEqual(0, List.Count());
        }

        public virtual void ContainsTest()
        {
            int c = List[rand.Next(List.Count)];
            Assert.AreEqual(Compare.Contains(c), List.Contains(c));

            for (int i = 0; i < Cycles; i++)
            {
                c = rand.Next(100);
                Assert.AreEqual(Compare.Contains(c), List.Contains(c));
            }
        }

        public virtual void CopyToTest()
        {
            int[] target = new int[2 * Cycles];
            List.CopyTo(target, 0);
            Assert.IsTrue(Compare.SequenceEqual(target));
        }

        public virtual void GetEnumeratorTest()
        {
            var iter = List.GetEnumerator();
            foreach (var item in Compare)
            {
                Assert.AreEqual(true, iter.MoveNext());
                Assert.AreEqual(item, iter.Current);
            }
            Assert.AreEqual(false, iter.MoveNext());
        }

        public virtual void AccessTest()
        {
            for (int i = 0; i < 2 * Cycles; i++)
                Assert.AreEqual(Compare[i], List[i]);

            for (int i = 0; i < 2 * Cycles; i++)
            {
                var c = rand.Next(100);
                Compare[i] = c; List[i] = c;
            }

            for (int i = 0; i < 2 * Cycles; i++)
                Assert.AreEqual(Compare[i], List[i]);
        }

        public virtual void IndexOfTest()
        {
            int c = List[rand.Next(List.Count)];
            Assert.AreEqual(Compare.IndexOf(c), List.IndexOf(c));

            for (int i = 0; i < Cycles; i++)
            {
                c = rand.Next(100);
                Assert.AreEqual(Compare.IndexOf(c), List.IndexOf(c));
            }
        }

        public virtual void InsertTest()
        {
            for (int i = 0; i < Cycles; i++)
            {
                var c = rand.Next(100);
                var index = rand.Next(List.Count);
                List.Insert(index, c); Compare.Insert(index, c);
                Assert.IsTrue(Compare.SequenceEqual(List));
            }
        }

        public virtual void RemoveTest()
        {
            for (int i = 0; i < Cycles; i++)
            {
                var val = Compare[rand.Next(List.Count)];

                Assert.AreEqual(Compare.Remove(val), List.Remove(val));
                Assert.AreEqual(Compare.Count, List.Count);
                Assert.IsTrue(Compare.SequenceEqual(List));
            }
            Assert.IsTrue(Compare.SequenceEqual(List));
        }

        public virtual void RemoveAtTest()
        {
            for (int i = 0; i < Cycles; i++)
            {
                var index = rand.Next(List.Count);
                Compare.RemoveAt(index); List.RemoveAt(index);

                Assert.AreEqual(Compare.Count, List.Count);
                Assert.IsTrue(Compare.SequenceEqual(List));
            }
        }

        public void CompareLog()
        {
            Console.WriteLine("[");
            foreach (var item in List)
                Console.Write(item + "\t");
            Console.WriteLine("|");
            foreach (var item in Compare)
                Console.Write(item + "\t");
            Console.WriteLine("]");
        }
    }
}
