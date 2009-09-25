using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class MetaConnectionException : Exception
    {
        public MetaConnectionException(string info)
            : base(info)
        { }
    }
}
