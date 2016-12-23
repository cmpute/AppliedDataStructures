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
            var tree = new BinarySearchTree<ValueNode, int>();
            tree.Insert(new ValueNode(4));
            tree.Insert(new ValueNode(6));
            tree.Insert(new ValueNode(8));
            tree.Insert(new ValueNode(3));
            tree.Insert(new ValueNode(2));
            tree.Insert(new ValueNode(9));
            tree.Insert(new ValueNode(7));
            tree.Insert(new ValueNode(4));
            foreach (var t in tree)
                Console.WriteLine(t.Key);
            tree.Delete(7);
            tree.Delete(4);
            tree.Delete(2);
            foreach (var t in tree)
                Console.WriteLine(t.Key);
            Console.ReadKey();
        }
    }

    class ValueNode : BinaryTreeNode, IComparableNode<int>
    {
        public ValueNode(int key) { Key = key; }
        public int Key { get; set; }
    }
}
