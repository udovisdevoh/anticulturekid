using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class VisualizerException : Exception
    {
        public VisualizerException(string info)
            : base(info)
        { }
    }
}
