using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Misc
{
    static class FunctionArgument
    {
        #region Methods
        public static void Ensure(object argument, string argumentName)
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName + " argument is null");
        }
        #endregion
    }
}