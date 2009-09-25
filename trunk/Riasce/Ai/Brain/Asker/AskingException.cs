using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exceptions thrown when asking something invalid to Ai
    /// or when processing of "ask" request fails
    /// </summary>
    class AskingException : Exception
    {
        public AskingException(string info)
            : base(info)
        { }
    }
}
