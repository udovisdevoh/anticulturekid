using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a memento about a string
    /// </summary>
    class StringMemento : AbstractMemento
    {
        #region Fields
        /// <summary>
        /// String based comment
        /// </summary>
        private string comment;
        #endregion

        #region Constructor
        public StringMemento(DateTime time, string authorName, string comment)
        {
            this.time = time;
            this.authorName = authorName;
            this.comment = comment;
        }
        #endregion
    }
}
