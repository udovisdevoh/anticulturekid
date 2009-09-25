using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents exceptions thrown by AiSql wrapper when something bad occurs
    /// </summary>
    class AiSqlException : Exception
    {
        public AiSqlException(string info)
            : base(info)
        { }
    }
}
