using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Static Search Tree / Offline Search Tree
    /// 静态搜索树（离线数据结构）的抽象接口
    /// </summary>
    public interface IStaticSearchTree<TNode, TKey> where TNode : IKeyedNode<TKey>
    {
        /// <summary>
        /// 在搜索树中根据关键字搜索结点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>搜索到的结点</returns>
        TNode Search(TKey key);
    }
}
