using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TotologyException : Exception
    {
        public TotologyException(string info)
            : base(info)
        { }
    }
}
