using System;
using System.Collections.Advanced;

namespace Test
{
    public static class BinarySearchTreeTest
    {
        public static void Test()
        {
            var tree = new BinarySearchTree<BSTValueNode, int>();
            tree.SupportEquatable = true;
            //tree.KeepInsertOrder = false;

            Console.WriteLine("Insert values....");
            tree.Insert(new BSTValueNode(4));
            tree.Insert(new BSTValueNode(6));
            tree.Insert(new BSTValueNode(8));
            tree.Insert(new BSTValueNode(4));
            tree.Insert(new BSTValueNode(3));
            tree.Insert(new BSTValueNode(2));
            tree.Insert(new BSTValueNode(4));
            tree.Insert(new BSTValueNode(9));
            tree.Insert(new BSTValueNode(7));
            tree.Insert(new BSTValueNode(4));
            foreach (var t in tree)
                Console.Write($"[{t.Key}/{tree.Count}]={t.Data}\t");
            Console.WriteLine("<.");

            Console.WriteLine("Search All nodes with key=4....");
            foreach (var t in tree.SearchAll(4))
                Console.Write($"[{t.Key}/{tree.Count}]={t.Data}\t");
            Console.WriteLine("<.");

            Console.WriteLine("Successor and Predecessor of nodes with key=4....");
            BSTValueNode ts = tree.Successor(4), tp = tree.Predecessor(4);
            Console.WriteLine($"successor [{ts.Key}]={ts.Data}\tpredecessor [{tp.Key}]={tp.Data} <.");

            Console.WriteLine("Delete values....");
            tree.Delete(6);
            tree.Delete(4);
            tree.Delete(2);
            foreach (var t in tree)
                Console.Write($"[{t.Key}/{tree.Count}]={t.Data}\t");
            Console.WriteLine("<.");
            tree.Delete(3);
            tree.Delete(7);
            tree.Delete(8);
            tree.Delete(9);
            foreach (var t in tree)
                Console.Write($"[{t.Key}/{tree.Count}]={t.Data}\t");
            Console.WriteLine("<.");

            Console.WriteLine("Insert values again....");
            tree.Insert(new BSTValueNode(6));
            tree.Insert(new BSTValueNode(3));
            foreach (var t in tree)
                Console.Write($"[{t.Key}/{tree.Count}]={t.Data}\t");
            Console.WriteLine("<.");

            Console.WriteLine("Search node again....");
            Console.WriteLine($"[3]={tree.Search(3)?.Data}");
        }
    }
    class BSTValueNode : BinaryTreeNode, IKeyedNode<int>
    {
        static int order = 1;
        public int Data { get; set; }
        public int Key { get; set; }
        public BSTValueNode(int key) { Key = key; Data = order++; }
    }
}
