using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Abstract Interface for rooted-tree
    /// 有根树的抽象接口
    /// </summary>
    public interface IRootedTree<out TNode> : IEnumerable<TNode> where TNode : INode
    {
        /// <summary>
        /// Get the root of the tree
        /// 获取有根树的根
        /// </summary>
        TNode Root { get; }
    }
}
