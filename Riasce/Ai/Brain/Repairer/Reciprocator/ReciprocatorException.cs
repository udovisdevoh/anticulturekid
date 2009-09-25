using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class ReciprocatorException : Exception
    {
        public ReciprocatorException(string info)
            : base(info)
        { }
    }
}
