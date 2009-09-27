using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exceptions thrown when name mapping fails
    /// </summary>
    class NameMappingException : Exception
    {
        public NameMappingException(string info)
            : base(info)
        { }
    }
}
