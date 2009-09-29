using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exception thrown when creation of neology fails
    /// </summary>
    class NeologyException : Exception
    {
        public NeologyException(string info)
            : base(info)
        { }
    }
}