using System;
using System.IO;

namespace System.Collections.Advanced
{
    public interface IPrintable
    {
        void PrintTo(TextWriter textOut);
    }
}
