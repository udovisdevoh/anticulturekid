using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exception thrown when adding/removing/testing/flattening of metaConnection fails
    /// </summary>
    class MetaConnectionException : Exception
    {
        public MetaConnectionException(string info)
            : base(info)
        { }
    }
}
