using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exception thrown when visible comment construction fails
    /// </summary>
    class VisibleCommentConstructionException : Exception
    {
        public VisibleCommentConstructionException(string info)
            : base(info)
        { }
    }
}
