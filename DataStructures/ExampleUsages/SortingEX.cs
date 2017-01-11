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
    class ComparerWrapper<T> : IComparer<T>
    {
        Comparison<T> _comparsion;
        public ComparerWrapper(Comparison<T> comparison)
        {
            if (comparison == null) throw new ArgumentNullException("比较delegate不能为空");
            _comparsion = comparison;
        }
        public int Compare(T x, T y) => _comparsion(x, y);
    }
}
