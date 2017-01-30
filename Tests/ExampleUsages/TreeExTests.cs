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
    public class TreeExTests
    {
        [TestMethod()]
        public void JudgeKindTest()
        {
            TestTree tree = new TestTree();
            tree.SetRoot(new BinaryTreeNode());
            Assert.IsTrue(tree.JudgeKind() == TreeKind.Perfect);
            tree.Root.LeftChild = new BinaryTreeNode();
            Assert.IsTrue(tree.JudgeKind() == (TreeKind.Complete | TreeKind.Strict));
            tree.Root.RightChild = new BinaryTreeNode();
            Assert.IsTrue(tree.JudgeKind() == TreeKind.Perfect);

            tree.Root.LeftChild.LeftChild = new BinaryTreeNode();
            Assert.IsTrue(tree.JudgeKind() == TreeKind.Complete);
            tree.Root.RightChild.LeftChild = new BinaryTreeNode();
            Assert.IsTrue(tree.JudgeKind() == TreeKind.Ordinary);
            tree.Root.LeftChild.RightChild = new BinaryTreeNode();
            Assert.IsTrue(tree.JudgeKind() == TreeKind.Complete);
            tree.Root.RightChild.RightChild = new BinaryTreeNode();
            Assert.IsTrue(tree.JudgeKind() == TreeKind.Perfect);

            tree.Root.RightChild.LeftChild.LeftChild = new BinaryTreeNode();
            tree.Root.RightChild.LeftChild.RightChild = new BinaryTreeNode();
            Assert.IsTrue(tree.JudgeKind() == TreeKind.Full);

            tree.Clear();
            tree.SetRoot(new BinaryTreeNode());
            tree.Root.LeftChild = new BinaryTreeNode();
            tree.Root.LeftChild.RightChild = new BinaryTreeNode();
            Assert.IsTrue(tree.JudgeKind() == TreeKind.Strict);
        }
    }
    class TestTree : BinaryTree<BinaryTreeNode>
    {
        public void SetRoot(BinaryTreeNode node) => Root = node;
    }
}