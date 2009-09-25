using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class StatementParsingException : Exception
    {
        public StatementParsingException(string info)
            : base(info)
        { }
    }
}
