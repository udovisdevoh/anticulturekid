using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class SaveLoadException : Exception
    {
        public SaveLoadException(string info)
            : base(info)
        { }
    }
}
