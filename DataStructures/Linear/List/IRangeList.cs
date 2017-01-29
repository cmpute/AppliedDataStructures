using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public interface IRangeList<T> : IList<T>
    {
        void AddRange(IEnumerable<T> collection);
        void InsertRange(int index, IEnumerable<T> collection);
        IList<T> GetRange(int index, int count);
        void RemoveRange(int index, int count);
        void Reverse(int index, int count);
        void Reverse();
    }
}
