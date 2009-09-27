using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a memento about an operation
    /// </summary>
    class OperationMemento : AbstractMemento
    {
        #region Fields
        /// <summary>
        /// Concept list
        /// </summary>
        private List<Concept> conceptList;

        /// <summary>
        /// Whether the connection is negative
        /// </summary>
        private bool isNegative;

        /// <summary>
        /// Whether the connection is interrogative
        /// </summary>
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