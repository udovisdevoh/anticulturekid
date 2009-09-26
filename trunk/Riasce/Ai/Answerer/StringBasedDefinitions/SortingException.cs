using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exceptions thrown when sorting fails
    /// </summary>
    class SortingException : Exception
    {
        public SortingException(string info)
        : base(info)
        { }
    }
}
