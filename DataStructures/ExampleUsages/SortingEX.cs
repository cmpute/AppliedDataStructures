using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Extended methods for sorting
    /// </summary>
    public static class SortingEx
    {
        public static IEnumerable<T> HeapSort<T>(this IEnumerable<T> data) => HeapSort(data, Comparer<T>.Default);
        public static IEnumerable<T> HeapSort<T>(this IEnumerable<T> data, Comparison<T> comparison) => HeapSort(data, new ComparerWrapper<T>(comparison));
        public static IEnumerable<T> HeapSort<T>(this IEnumerable<T> data, IComparer<T> comparer)
        {
            BinaryHeap<T> heap = new BinaryHeap<T>(data, comparer);
            while (heap.Count > 0) yield return heap.ExtractMin();
        }
    }
}
