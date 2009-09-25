using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class MemoryException : Exception
    {
        #region Constructors
        public MemoryException(string info)
            : base(info)
        { }
        #endregion
    }
}
