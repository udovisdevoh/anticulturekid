using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public class StringEventArgs : EventArgs
    {
        #region Fields
        private string name;
        #endregion

        #region Constructor
        public StringEventArgs(string name)
        {
            this.name = name;
        }
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
        }
        #endregion
    }
}
