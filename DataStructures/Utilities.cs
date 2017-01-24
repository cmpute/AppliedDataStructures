using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    static class Utils
    {

        //ceil(log2(n))
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
}
