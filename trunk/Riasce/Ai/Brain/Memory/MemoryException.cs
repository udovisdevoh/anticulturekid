using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exception thrown when IO to/from memory fails
    /// </summary>
    class MemoryException : Exception
    {
        public MemoryException(string info)
            : base(info)
        { }
    }
}
