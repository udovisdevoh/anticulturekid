using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class ImplyConnectionException : Exception
    {
        public ImplyConnectionException(string info)
            : base(info)
        { }
    }
}
