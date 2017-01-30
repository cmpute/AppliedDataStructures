using System;
using System.Collections.Advanced.Tree;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract = System.Diagnostics.Contracts.Contract;

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
            Contract.Requires<ArgumentNullException>(data != null);
            Contract.Requires<ArgumentNullException>(comparer != null);
            BinaryHeap<T> heap = new BinaryHeap<T>(data, comparer);
            while (heap.Count > 0) yield return heap.ExtractMin();
        }
    }
}
