using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Search Tree which suppot nodes with same key
    /// 支持相等元素的搜索树
    /// </summary>
    /// <remarks>
    /// 此时<see cref="ISearchTree{TNode, TKey}.Search(TKey)"/>的返回值为第一个匹配的结点，
    /// <see cref="ISearchTree{TNode, TKey}.Insert(TNode)"/>返回值为插入的结点
    /// </remarks>
    public interface ISearchTreeEquatable<TNode, TKey> : ISearchTree<TNode, TKey> where TNode : IComparableNode<TKey>
    {
        /// <summary>
        /// 在搜索树中根据关键字搜索所有匹配结点
        /// </summary>
        /// <param name="key">查找的关键字</param>
        /// <returns>与关键字<paramref name="key"/>相等的所有元素</returns>
        IEnumerable<TNode> SearchAll(TKey key);

        //new void Insert(TNode node);
    }
}
