using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Advanced;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = new BinarySearchTree<ValueNode, int, int>();
            tree.Insert(new ValueNode(4));
            tree.Insert(new ValueNode(6));
            tree.Insert(new ValueNode(8));
            tree.Insert(new ValueNode(3));
            tree.Insert(new ValueNode(2));
            tree.Insert(new ValueNode(9));
            tree.Insert(new ValueNode(7));
            tree.Insert(new ValueNode(4));
            foreach (var t in tree)
                Console.Write($"[{t.Key}/{tree.Count}]={t.Data}\t");
            Console.WriteLine();
            tree.Delete(6);
            tree.Delete(4);
            tree.Delete(2);
            foreach (var t in tree)
                Console.Write($"[{t.Key}/{tree.Count}]={t.Data}\t");
            Console.WriteLine();
            tree.Delete(3);
            tree.Delete(7);
            tree.Delete(8);
            tree.Delete(9);
            foreach (var t in tree)
                Console.Write($"[{t.Key}/{tree.Count}]={t.Data}\t");
            Console.WriteLine();
            tree.Insert(new ValueNode(6));
            tree.Insert(new ValueNode(3));
            foreach (var t in tree)
                Console.Write($"[{t.Key}/{tree.Count}]={t.Data}\t");
            Console.WriteLine();
            Console.WriteLine($"[3]={tree.Search(3).Data}");
            Console.ReadKey();
        }
    }

    class ValueNode : BinarySearchTreeNode<int,int>
    {
        public ValueNode(int key) { Key = key; Data = key*key; }
    }
}
