using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exceptions thrown when destroyer fails to remove a concept's connections
    /// </summary>
    class DestructionException : Exception
    {
        public DestructionException(string info)
            : base(info)
        { }
    }
}
