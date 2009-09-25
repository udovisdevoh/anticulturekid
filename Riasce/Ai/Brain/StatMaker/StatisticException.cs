using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class StatisticException : Exception
    {
        public StatisticException(string info)
            : base(info)
        { }
    }
}
