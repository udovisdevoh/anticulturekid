using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exceptions thrown when purification fails
    /// </summary>
    class PurificationException : Exception
    {
        public PurificationException(string info)
            : base(info)
        { }
    }
}
