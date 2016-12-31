using System;
using System.Collections.Advanced;
using Node = System.Collections.Advanced.AVLTreeNode<int>;

namespace Test
{
    public static class AVLTreeTest
    {
        public static void Test()
        {
            AVLTree<Node, int> tree = new AVLTree<Node, int>();

            Console.WriteLine("Random insert...");
            tree.Insert(new Node() { Key = 6 });
            tree.Insert(new Node() { Key = 3 });
            tree.Insert(new Node() { Key = 7 });
            tree.Insert(new Node() { Key = 2 });
            tree.Insert(new Node() { Key = 5 });
            tree.Insert(new Node() { Key = 9 });
            tree.Insert(new Node() { Key = 1 });
            tree.Insert(new Node() { Key = 8 });
            foreach (var t in tree)
                Console.Write($"[{t.Key}|h={t.Height}]\t");
            Console.WriteLine("<.");

            Console.WriteLine("Orderly insert...");
            tree.Clear();
            for (int i = 0; i < 12; i++)
            {
                tree.Insert(new Node() { Key = i });
                foreach (var t in tree)
                    Console.Write($"[{t.Key}|h={t.Height}]\t");
                Console.WriteLine("<.");
            }

            Console.WriteLine("reversely delete...");
            for (int i = 12; i > 0; i--)
            {
                tree.Delete(i);
                foreach (var t in tree)
                    Console.Write($"[{t.Key}|h={t.Height}]\t");
                Console.WriteLine("<.");
            }
        }
    }
}
