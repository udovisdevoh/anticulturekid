using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class FlattenizationException : Exception
    {
        public FlattenizationException(string info)
            : base(info)
        { }
    }
}
