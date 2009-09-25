using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TeachingException : Exception
    {
        #region Constructors
        public TeachingException(string info)
            : base(info)
        { }
        #endregion
    }
}
