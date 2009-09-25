using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class VisibleCommentConstructionException : Exception
    {
        #region Constructors
        public VisibleCommentConstructionException(string info)
            : base(info)
        { }
        #endregion
    }
}
