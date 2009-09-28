using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exception thrown when statement parsing fails
    /// </summary>
    class StatementParsingException : Exception
    {
        public StatementParsingException(string info)
            : base(info)
        { }
    }
}
