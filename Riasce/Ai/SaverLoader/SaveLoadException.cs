using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exception thrown when saving or loading fails
    /// </summary>
    class SaveLoadException : Exception
    {
        public SaveLoadException(string info)
            : base(info)
        { }
    }
}
