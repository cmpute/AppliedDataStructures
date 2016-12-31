using System;
using System.Collections.Advanced;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    public static class BinaryHeapTest
    {
        public static void Test()
        {
            Console.WriteLine("Value Type Test....");
            BinaryHeap<int> heap = new BinaryHeap<int>();

            Console.WriteLine("Random insert....");
            Random r = new Random();
            for (int i = 0; i < 20; i++)
            {
                int t = r.Next(100);
                Console.Write(t + "\t");
                heap.Insert(t);
            }
            Console.WriteLine("<.");
            foreach (var t in heap)
                Console.Write($"[{t}]\t");
            Console.WriteLine("<.");

            Console.WriteLine("Extract Min....");
            while (heap.Count > 0)
                Console.Write(heap.ExtractMin() + "\t");
            Console.WriteLine("<.");

            Console.WriteLine("Reference Type Test....");
            BinaryHeap<BinaryHeapValueNode> heapr = new BinaryHeap<BinaryHeapValueNode>(new BHVNComparer());

            Console.WriteLine("Random insert....");
            for (int i = 0; i < 20; i++)
            {
                int t = r.Next(100);
                Console.Write(t + "\t");
                heapr.Insert(new BinaryHeapValueNode() { Key = t });
            }
            Console.WriteLine("<.");
            foreach (var t in heapr)
                Console.Write($"[{t.IndexInHeap}]={t.Key}|order={t.Data}\t");
            Console.WriteLine("<.");

            Console.WriteLine("Extract Min....");
            while (heapr.Count > 0)
            {
                var t = heapr.ExtractMin();
                Console.Write($"[{t.IndexInHeap}]={t.Key}|order={t.Data}\t");
            }
            Console.WriteLine("<.");
        }
    }
    class BinaryHeapValueNode : IIndexedHeapNode
    {
        static int order = 1;
        public int IndexInHeap { get; set; }
        public int Data { get; set; } = order++;
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
