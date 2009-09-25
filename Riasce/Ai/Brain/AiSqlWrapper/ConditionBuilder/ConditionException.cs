using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exceptions thrown when fail to build condition
    /// </summary>
    class ConditionException : Exception
    {
        public ConditionException(string info)
            : base(info)
        { }
    }
}
