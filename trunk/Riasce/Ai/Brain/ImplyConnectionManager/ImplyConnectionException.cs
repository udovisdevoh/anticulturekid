using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exceptions thrown when imply connect/disconnect fails
    /// </summary>
    class ImplyConnectionException : Exception
    {
        public ImplyConnectionException(string info)
            : base(info)
        { }
    }
}
