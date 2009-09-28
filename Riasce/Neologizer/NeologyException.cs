using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exceptions thown when neologization fails
    /// </summary>
    class NeologyException : Exception
    {
        public NeologyException(string info)
            : base(info)
        { }
    }
}
