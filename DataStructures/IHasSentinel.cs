using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public interface IHasSentinel
    {
        bool IsSentinel();
    }
    static class SentinelEx
    {
        public static bool EqualNull(object a)
        {
            return a == null || ((a as IHasSentinel)?.IsSentinel() ?? false);
        }
        public static bool NotEqualNull(object a)
        {
            return a != null && !((a as IHasSentinel)?.IsSentinel() ?? false);
        }
        public static new bool Equals(object a, object b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (EqualNull(a) && EqualNull(b))
                return true;
            return false;
        }
    }
}
