using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class ConnectionException : Exception
    {
        public ConnectionException(string info)
            : base(info)
        { }
    }
}
