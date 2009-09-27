using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a memento about a metaOperation
    /// </summary>
    class MetaOperationMemento : AbstractMemento
    {
        #region Fields
        /// <summary>
        /// Concept list
        /// </summary>
        private List<Concept> conceptList;

        /// <summary>
        /// MetaOperator
        /// </summary>
        private string metaOperator;

        /// <summary>
        /// Whether the metaConnection is negative
        /// </summary>
        private bool isNegative;

        /// <summary>
        /// Whether the metaConnection is interrogative
        /// </summary>
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
