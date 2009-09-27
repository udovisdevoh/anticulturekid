using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exceptions thrown when flattenization fails
    /// </summary>
    class FlattenizationException : Exception
    {
        public FlattenizationException(string info)
            : base(info)
        { }
    }
}
