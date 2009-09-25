using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class DefinitionException : Exception
    {
        public DefinitionException(string info)
            : base(info)
        { }
    }
}
