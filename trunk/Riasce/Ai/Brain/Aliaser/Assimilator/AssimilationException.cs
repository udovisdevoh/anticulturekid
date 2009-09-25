using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exceptions thrown by assimilator when assimilation fails
    /// </summary>
    class AssimilationException : Exception
    {
        public AssimilationException(string info)
            : base(info)
        { }
    }
}
