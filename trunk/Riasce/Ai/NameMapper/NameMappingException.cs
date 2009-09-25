using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class NameMappingException : Exception
    {
        #region Constructors
        public NameMappingException(string info)
            : base(info)
        { }
        #endregion
    }
}
