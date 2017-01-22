using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// van Emde Boas Tree (a.k.a vEB-tree)
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <remarks>
    /// A Search Tree works based on comparison, but vEB tree works on bits, so vEB is technically not a search tree.
    /// </remarks>
    public class vanEmdeBoasTree<TNode, TData> : IRootedTree<TNode> where TNode : vanEmdeBoasTreeNode<TData>
    {
        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public TNode Root { get; protected set; }

        TNode Successor(int key) { throw new NotImplementedException(); }
        TNode Predecessor(int key) { throw new NotImplementedException(); }

        /// <summary>
        /// 若关键字不存在则创建新结点，否则根据关键字寻找该结点，并且返回
        /// </summary>
        /// <param name="key">新建结点的关键字</param>
        /// <returns>创建或搜索到的结点</returns>
        public TNode CreateNode(int key) { throw new NotImplementedException(); }

        public TNode DeleteNode(int key)
        {
            throw new NotImplementedException();
        }

        public TNode SearchNode(int key)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<TNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void TrimExcess()
        {

        }
    }
}
