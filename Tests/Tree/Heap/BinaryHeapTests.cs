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
    public class BinaryHeapTests
    {
        BinaryHeap<int> heap = new BinaryHeap<int>();
        BinaryHeap<BinaryHeapValueNode> heapr = new BinaryHeap<BinaryHeapValueNode>(new BHVNComparer());

        [TestInitialize()]
        public void Generate()
        {
            heap.Clear(); heapr.Clear();
            Random r = new Random();
            Console.WriteLine("Test Data:");
            for (int i = 0; i < 20; i++)
            {
                int t = r.Next(100);
                Console.Write(t + "\t");
                heapr.Insert(new BinaryHeapValueNode() { Key = t });
                heap.Insert(t);
            }
            Console.WriteLine("<.");
        }

        [TestMethod()]
        public void ExtractMinTest()
        {
            while (heap.Count > 0)
                Assert.AreEqual(heap.Min(), heap.ExtractMin());
            while (heapr.Count > 0)
                Assert.AreEqual(heapr.Min(), heapr.ExtractMin());
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