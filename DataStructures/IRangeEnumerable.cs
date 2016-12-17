using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Indicating being enumerable in part of the collection
    /// 表示集合可以在指定区间内进行枚举
    /// </summary>
    public interface IRangeEnumerable : IList, IEnumerable
    {
        /// <summary>
        /// 返回一个循环访问集合指定部分的枚举器
        /// </summary>
        /// <param name="startindex">枚举的开始位置</param>
        /// <param name="count">枚举区间的长度，如果为负数则是向前枚举</param>
        IEnumerator GetEnumerator(int startindex, int count);
    }
    /// <summary>
    /// Indicating being enumerable in part of the collection
    /// 表示集合可以在指定区间内进行枚举
    /// </summary>
    public interface IRangeEnumerable<T> : IList<T>, IEnumerable<T>
    {
        /// <summary>
        /// 返回一个循环访问集合指定部分的枚举器
        /// </summary>
        /// <param name="startindex">枚举的开始位置</param>
        /// <param name="count">枚举区间的长度，如果为负数则是向前枚举</param>
        IEnumerator<T> GetEnumerator(int startindex, int count);
    }
}
