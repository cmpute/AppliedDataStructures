using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Interface that indicates that the structure can be merged into another structure
    /// 表示该结构可以并入另一个结构的接口
    /// </summary>
    /// <typeparam name="TIn">可以合并的结构的类型</typeparam>
    public interface IMergable<in TIn> //<in TIn, out TOut>
    {
        /// <summary>
        /// Merge with another structure
        /// 与另一个结构合并
        /// </summary>
        /// <param name="target">合并的对象</param
        /// <remarks>
        /// The merging may be not copying original structures, it may destroy origin structure.
        /// </remarks>
        void Merge(TIn target);
        //TOut Merge(TIn target);
    }
}
