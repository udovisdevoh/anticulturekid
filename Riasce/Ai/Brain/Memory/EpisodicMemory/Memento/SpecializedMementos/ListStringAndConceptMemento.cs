using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class ListStringAndConceptMemento : AbstractMemento
    {
        #region Fields
        private IEnumerable<string> stringList;

        private IEnumerable<Concept> conceptList;

        private bool isNegative;
        #endregion

        #region Constructor
        public ListStringAndConceptMemento(DateTime time, string authorName, IEnumerable<string> stringList, IEnumerable<Concept> conceptList, bool isNegative)
        {
            this.time = time;
            this.authorName = authorName;
            this.stringList = stringList;
            this.conceptList = conceptList;
            this.isNegative = isNegative;
        }
        #endregion
    }
}
