using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Exception throws when node is linked to itself
    /// </summary>
    public class ReferenceLoopException : Exception
    {
        public ReferenceLoopException() : base("不能将链接指向自身") { }
        public ReferenceLoopException(string message) : base(message) { }
        public ReferenceLoopException(string message, Exception innerException) : base(message, innerException) { }
    }
}
