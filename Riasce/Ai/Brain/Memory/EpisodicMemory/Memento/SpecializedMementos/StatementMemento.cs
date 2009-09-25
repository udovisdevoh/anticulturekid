using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    class StatementMemento : AbstractMemento
    {
        #region Fields
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
