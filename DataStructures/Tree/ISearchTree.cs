using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Abstaction of Search Tree
    /// 搜索树的抽象接口
    /// </summary>
    /// <typeparam name="TNode">结点类型</typeparam>
    /// <typeparam name="TKey">关键字类型</typeparam>
    /// <remarks>
    /// ISearchTree interface does not derive from IDictionary because the TNode doesn't fit for the concept TValue, IDictionary can be easily implemented by a wrapper of a Search Tree
    /// ISearchTree接口没有继承IDictionary接口，因为TNode作为TValue并不是很合适，IDictionay可以通过对搜索树进行封装而非常容易地实现
    /// </remarks>
    public interface ISearchTree<TNode, TKey> where TNode : IKeyedNode<TKey>
    {
        /// <summary>
        /// 在搜索树中根据关键字搜索结点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>搜索到的结点，结点不在树中则返回<c>null</c></returns>
        TNode Search(TKey key);
        /// <summary>
        /// 在搜索树中根据关键字删除结点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>删除掉的结点，结点不在树中则返回<c>null</c></returns>
        TNode Delete(TKey key);
        /// <summary>
        /// 在搜索树中插入结点，如果已存在同样Key值的结点则返回该结点
        /// </summary>
        /// <param name="node">需要插入的结点，结点已存在于树中则返回该结点</param>
        TNode Insert(TNode node);
        IComparer<TKey> Comparer { get; }
    }
}
