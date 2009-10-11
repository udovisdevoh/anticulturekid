using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exception thrown when specification is invalid
    /// </summary>
    class SpecificationException : Exception
    {
        public SpecificationException(string message) : base(message)
        {
        }
    }
}
