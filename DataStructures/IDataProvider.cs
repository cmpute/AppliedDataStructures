using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Structure with a comparable key.
    /// 包含可以比较的关键字的结构
    /// </summary>
    /// <typeparam name="TKey">关键字类型</typeparam>
    /// <remarks>
    /// It's recommended that <typeparamref name="TKey"/> implements <see cref="IComparable{T}"/> interface, if not, the <see cref="Comparer{T}.Default"/> will be used.
    /// <typeparamref name="TKey"/>应实现<see cref="IComparable{T}"/>接口，否则会使用<see cref="Comparer{T}.Default"/>比较器。
    /// </remarks>
    public interface IKeyProvider<out TKey>
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
    /// <summary>
    /// Structure with a comparable priority.
    /// 包含可以比较的优先级的结构
    /// </summary>
    /// <typeparam name="TPrior">优先级类型</typeparam>
    /// <remarks>
    /// It's recommended that <typeparamref name="TPrior"/> implements <see cref="IComparable{T}"/> interface, if not, the <see cref="Comparer{T}.Default"/> will be used.
    /// <typeparamref name="TPrior"/>应实现<see cref="IComparable{T}"/>接口，否则会使用<see cref="Comparer{T}.Default"/>比较器。
    /// </remarks>
    public interface IPriorityProvider<out TPrior>
    {
        TPrior Priority { get; }
    }
}
