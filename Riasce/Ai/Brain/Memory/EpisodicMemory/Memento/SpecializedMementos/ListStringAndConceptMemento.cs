using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a memento about a list of string and concept
    /// </summary>
    class ListStringAndConceptMemento : AbstractMemento
    {
        #region Fields
        /// <summary>
        /// List of string to remember
        /// </summary>
        private IEnumerable<string> stringList;

        /// <summary>
        /// List of concept to remember
        /// </summary>
        private IEnumerable<Concept> conceptList;

        /// <summary>
        /// Whether the memento is positive or negative
        /// </summary>
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
