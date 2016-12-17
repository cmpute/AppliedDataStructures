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
            //var tree = new BinaryTree<ValueNode>();
            //tree.Root = new ValueNode() { Value = 1 };
            //tree.Root.LeftChild = new ValueNode() { Value = 2 };
            //tree.Root.LeftChild.LeftChild = new ValueNode { Value = 4 };
            //tree.Root.LeftChild.LeftChild.LeftChild = new ValueNode { Value = 7 };
            //tree.Root.LeftChild.RightChild = new ValueNode { Value = 5 };
            //tree.Root.LeftChild.RightChild.LeftChild = new ValueNode { Value = 8 };
            //tree.Root.LeftChild.RightChild.RightChild = new ValueNode { Value = 9 };
            //tree.Root.RightChild = new ValueNode { Value = 3 };
            //tree.Root.RightChild.RightChild = new ValueNode { Value = 6 };
            //tree.Root.RightChild.RightChild.LeftChild = new ValueNode { Value = 10 };
            //tree.Root.RightChild.RightChild.LeftChild.LeftChild = new ValueNode { Value = 12 };
            //tree.Root.RightChild.RightChild.RightChild = new ValueNode { Value = 11 };
            //var iter = tree.GetEnumerator(TraversalOrder.LevelOrder);
            //while(iter.MoveNext())
            //{
            //    Console.WriteLine(iter.Current.Value);
            //}
            Console.ReadKey();
        }
    }

    class ValueNode : BinaryTreeNode
    {
        public int Value { get; set; }
    }
}
