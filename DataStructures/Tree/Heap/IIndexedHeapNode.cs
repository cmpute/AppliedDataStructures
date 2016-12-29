using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Heap node with index, so that searching will be faster in the (dense) heap
    /// 有索引属性的堆结点，在有索引的情况下堆中的定位操作较快
    /// </summary>
    public interface IIndexedHeapNode
    {
        /// <summary>
        /// 获取和设置在稠密存储的堆中的索引（该属性不应手动设置，而应由Heap修改）
        /// </summary>
        int IndexInHeap { get; set; }
    }
}
