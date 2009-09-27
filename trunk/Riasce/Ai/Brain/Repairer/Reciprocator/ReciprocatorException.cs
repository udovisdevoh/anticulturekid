using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exception thrown when reciprocation fails
    /// </summary>
    class ReciprocatorException : Exception
    {
        public ReciprocatorException(string info)
            : base(info)
        { }
    }
}
