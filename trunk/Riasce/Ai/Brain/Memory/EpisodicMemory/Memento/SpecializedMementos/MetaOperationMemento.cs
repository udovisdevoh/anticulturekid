using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class MetaOperationMemento : AbstractMemento
    {
        #region Fields
        private List<Concept> conceptList;

        private string metaOperator;

        private bool isNegative;

        private bool isInterrogative;
        #endregion

        #region Constructor
        public MetaOperationMemento(DateTime time, string authorName, List<Concept> conceptList, string metaOperator, bool isNegative, bool isInterrogative)
        {
            this.time = time;
            this.authorName = authorName;
            this.conceptList = conceptList;
            this.metaOperator = metaOperator;
            this.isNegative = isNegative;
            this.isInterrogative = isInterrogative;
        }
        #endregion
    }
}
