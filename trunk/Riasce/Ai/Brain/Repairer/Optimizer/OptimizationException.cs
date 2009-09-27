using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exception thrown when concept optimization fails
    /// </summary>
    class OptimizationException : Exception
    {
        public OptimizationException(string info)
            : base(info)
        { }
    }
}
