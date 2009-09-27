using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exceptions thrown when teaching fails
    /// </summary>
    class TeachingException : Exception
    {
        public TeachingException(string info)
            : base(info)
        { }
    }
}
