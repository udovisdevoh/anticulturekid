using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exceptions thrown when analogy creation fails
    /// </summary>
    class AnalogyException : Exception
    {
        public AnalogyException(string info)
            : base(info)
        { }
    }
}
