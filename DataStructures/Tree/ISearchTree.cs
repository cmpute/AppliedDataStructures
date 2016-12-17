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
    public interface ISearchTree<TNode, TKey> :IDictionary<TKey, TNode> where TNode : ISearchTreeNode<TKey>
    {
    }
}
