using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exceptions thrown when connection / disconnection of concept fails
    /// </summary>
    class ConnectionException : Exception
    {
        public ConnectionException(string info)
            : base(info)
        { }
    }
}
