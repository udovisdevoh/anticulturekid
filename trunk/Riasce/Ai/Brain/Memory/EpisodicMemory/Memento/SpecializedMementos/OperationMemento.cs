using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class OperationMemento : AbstractMemento
    {
        #region Fields
        private List<Concept> conceptList;

        private bool isNegative;

        private bool isInterrogative;
        #endregion

        #region Constructor
        public OperationMemento(DateTime time, string authorName, List<Concept> conceptList, bool isNegative, bool isInterrogative)
        {
            this.time = time;
            this.authorName = authorName;
            this.conceptList = conceptList;
            this.isNegative = isNegative;
            this.isInterrogative = isInterrogative;
        }
        #endregion
    }
}