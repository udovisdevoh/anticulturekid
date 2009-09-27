using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exception thrown when using what to prove as an argument
    /// </summary>
    class TotologyException : Exception
    {
        public TotologyException(string info)
            : base(info)
        { }
    }
}
