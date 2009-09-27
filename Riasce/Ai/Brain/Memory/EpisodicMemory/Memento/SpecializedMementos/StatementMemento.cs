using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a memento about a statement
    /// </summary>
    class StatementMemento : AbstractMemento
    {
        #region Fields
        /// <summary>
        /// The statement to remember
        /// </summary>
        private Statement statement;
        #endregion

        #region Constructors
        public StatementMemento(DateTime time, string authorName, Statement statement)
        {
            this.time = time;
            this.authorName = authorName;
            this.statement = statement;
        }
        #endregion
    }
}
