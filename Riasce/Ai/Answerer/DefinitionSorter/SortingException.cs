using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class SortingException : Exception
    {
        public SortingException(string info)
        : base(info)
        { }
    }
}
