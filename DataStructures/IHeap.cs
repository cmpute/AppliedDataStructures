using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Capable of returning min value with IComparer
    /// 能够在给定比较器的情况下返回最小值
    /// </summary>
    /// <remarks>
    /// MaxHeap can be turned into MinHeap by changing IComparer
    /// 最大堆可以通过改变比较方式变成最小堆
    /// </remarks>
    public interface IHeap<T>
    {
        T Min();
    }
}
