using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TheoryException : Exception
    {
        #region Constructors
        public TheoryException(string info)
            : base(info)
        { }
        #endregion
    }
}
