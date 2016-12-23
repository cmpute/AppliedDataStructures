using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Node with a comparable key.
    /// 包含可以比较的关键字的结点
    /// </summary>
    /// <typeparam name="TKey">关键字类型</typeparam>
    public interface IComparableNode<TKey> : INode
    {
        /// <summary>
        /// Key for comparing
        /// 用于比较的关键字
        /// </summary>
        /// <remarks>
        /// 关键字最好实现IComparable接口
        /// </remarks>
        TKey Key { get; }
    }
}
