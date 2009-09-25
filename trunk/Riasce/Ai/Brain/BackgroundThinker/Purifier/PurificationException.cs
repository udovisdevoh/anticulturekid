using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class PurificationException : Exception
    {
        #region Constructors
        public PurificationException(string info)
            : base(info)
        { }
        #endregion
    }
}
