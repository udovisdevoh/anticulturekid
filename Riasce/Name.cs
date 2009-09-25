using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a renamble persistant renamable string.
    /// </summary>
    public class Name
    {
        #region Fields
        private string stringValue = "";
        #endregion

        #region Constructors
        public Name(string stringValue)
        {
            this.stringValue = stringValue;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Change a string's internal value for all references to this
        /// </summary>
        public string Value
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
        #endregion
    }
}