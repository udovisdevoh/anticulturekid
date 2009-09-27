using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exception thrown when creation of statistical info fails
    /// </summary>
    class StatisticException : Exception
    {
        public StatisticException(string info)
            : base(info)
        { }
    }
}
