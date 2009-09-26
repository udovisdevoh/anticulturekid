using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exception thrown when creating concept based definition fails
    /// </summary>
    class DefinitionException : Exception
    {
        public DefinitionException(string info)
            : base(info)
        { }
    }
}
