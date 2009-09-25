using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class NeologyException : Exception
    {
        public NeologyException(string info)
            : base(info)
        { }
    }
}
