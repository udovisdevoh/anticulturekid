using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exception thrown when dream fails
    /// </summary>
    class DreamException : Exception
    {
        public DreamException(string info)
            : base(info)
        { }
    }
}
