using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
