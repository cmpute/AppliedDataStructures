using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Static Search Tree
    /// 静态搜索树的抽象接口
    /// </summary>
    public interface IStaticSearchTree<out TNode, in TKey> : IRootedTree<TNode>
        //where TNode : IKeyProvider<TKey>
    {
        /// <summary>
        /// 在搜索树中根据关键字搜索结点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>搜索到的结点，结点不在树中则返回<c>null</c></returns>
        TNode SearchNode(TKey key);
    }
    /// <summary>
    /// Abstaction of Search Tree
    /// 搜索树的抽象接口
    /// </summary>
    /// <typeparam name="TNode">结点类型</typeparam>
    /// <typeparam name="TKey">关键字类型</typeparam>
    /// <remarks>
    /// A search tree is a tree that every node of it has a key, and the key of leftchild is larger/smaller than rightchild.
    /// </remarks>
    public interface ISearchTree<TNode, in TKey> : IStaticSearchTree<TNode, TKey>
        //where TNode : IKeyProvider<TKey>
    {
        /// <summary>
        /// 在搜索树中插入结点，如果已存在同样Key值的结点则返回该结点
        /// </summary>
        /// <param name="node">需要插入的结点，结点已存在于树中则返回该结点</param>
        TNode InsertNode(TNode node);
        /// <summary>
        /// 在搜索树中删除指定的结点
        /// </summary>
        /// <param name="node">需要删除的结点</param>
        /// <returns>如果结点存在且删除成功则返回true</returns>
        bool DeleteNode(TNode node);
        /// <summary>
        /// 在搜索树中根据关键字删除结点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>删除掉的结点，结点不在树中则返回<c>null</c></returns>
        TNode DeleteNode(TKey key);
        IComparer<TKey> KeyComparer { get; }
    }
}
