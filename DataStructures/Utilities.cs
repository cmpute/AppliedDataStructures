using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 封装一个方法，该方法只有一个引用参数并且不返回值
    /// </summary>
    /// <typeparam name="T">此委托封装的方法的类型参数</typeparam>
    /// <param name="target">此委托封装的方法的参数</param>
    public delegate void ActionRef<T>(ref T target);
}

namespace System.Collections.Advanced
{
    public static class Utils
    {
        public static IComparer<T> AsComparer<T>(this Comparison<T> comparison) => new ComparerWrapper<T>(comparison);
        public static IEqualityComparer<T> AsEqualityComparer<T>(this Comparison<T> comparison) => new ComparerWrapper<T>(comparison);

        /// <summary>
        /// Fast Implementation of Math.Ceiling(Math.Log(<paramref name="n"/>,2))
        /// </summary>
        public static int Ceil2(double n)
        {
            int a = 1, c = 0;
            while (a < n)
            {
                a <<= 1;
                c++;
            }
            return c;
        }

        public static int IndexOf<T>(this IEnumerable<T> list, T item)
        {
            var itor = list.GetEnumerator();
            int count = 0;
            if (ReferenceEquals(item, null))
            {
                while (itor.MoveNext())
                {
                    if (ReferenceEquals(itor.Current, null))
                        return count;
                    count++;
                }
            }
            else
            {
                var comparer = EqualityComparer<T>.Default;
                while (itor.MoveNext())
                {
                    if (comparer.Equals(itor.Current, item))
                        return count;
                    count++;
                }
            }
            return -1;
        }
        public static void CopyTo<T>(this IEnumerable<T> list, T[] array, int arrayIndex)
        {
            Diagnostics.Contracts.Contract.Requires<ArgumentNullException>(array != null);
            Diagnostics.Contracts.Contract.Requires<ArgumentException>(array.Rank == 1);

            int current = arrayIndex;
            var iter = list.GetEnumerator();
            while (iter.MoveNext())
                array[current++] = iter.Current;
        }
    }
    
    class ComparerWrapper<T> : IComparer<T>, IEqualityComparer<T>
    {
        Comparison<T> _comparison;
        public ComparerWrapper(Comparison<T> comparison)
        {
            if (comparison == null) throw new ArgumentNullException("比较delegate不能为空");
            _comparison = comparison;
        }
        public int Compare(T x, T y) => _comparison(x, y);

        public bool Equals(T x, T y) => _comparison(x, y) == 0;

        public int GetHashCode(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
