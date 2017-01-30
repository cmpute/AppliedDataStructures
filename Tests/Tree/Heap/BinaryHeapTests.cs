using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Advanced.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced.Tests
{
    [TestClass()]
    public class BinaryHeapTests
    {
        BinaryHeap<int> heap = new BinaryHeap<int>();
        BinaryHeap<BinaryHeapValueNode> heapr = new BinaryHeap<BinaryHeapValueNode>(new BHVNComparer());
        List<int> compare = new List<int>();

        [TestInitialize()]
        public void Generate()
        {
            heap.Clear(); heapr.Clear(); compare.Clear();
            Random r = new Random();
            Console.WriteLine("Test Data:");
            for (int i = 0; i < 20; i++)
            {
                int t = r.Next(100);
                Console.Write(t + "\t");
                heapr.Insert(new BinaryHeapValueNode() { Key = t });
                heap.Insert(t);
                compare.Add(t);
            }
            Console.WriteLine("<.");
        }

        [TestMethod()]
        public void ExtractMinTest()
        {
            compare.Sort((x, y) => y - x); //descending
            while (compare.Count > 0)
            {
                Assert.AreEqual(heap.Min(), compare[compare.Count - 1]);
                Assert.AreEqual(heap.Min(), heap.ExtractMin());
                Assert.AreEqual(heapr.Min().Key, compare[compare.Count - 1]);
                Assert.AreEqual(heapr.Min(), heapr.ExtractMin());
                compare.RemoveAt(compare.Count - 1);
            }
        }

        [TestMethod()]
        public void MergeTest()
        {
            var heapt = new BinaryHeap<int>();
            var heaprt = new BinaryHeap<BinaryHeapValueNode>(new BHVNComparer());
            Random r = new Random();
            for (int i = 0; i < 20; i++)
            {
                int t = r.Next(100);
                heaprt.Insert(new BinaryHeapValueNode() { Key = t });
                heapt.Insert(t);
                compare.Add(t);
            }

            heap.Merge(heapt);
            heapr.Merge(heaprt);
            ExtractMinTest();
        }
    }

    class BinaryHeapValueNode : IIndexedHeapNode
    {
        public int IndexInHeap { get; set; }
        public int Key { get; set; }
    }

    class BHVNComparer : IComparer<BinaryHeapValueNode>
    {
        public int Compare(BinaryHeapValueNode x, BinaryHeapValueNode y)
        {
            return x.Key - y.Key;
        }
    }
}