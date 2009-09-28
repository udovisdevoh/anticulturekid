using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a metaConnection theory info
    /// </summary>
    [Serializable]
    class MetaConnectionTheoryInfo
    {
        #region Fields
        /// <summary>
        /// Concept 1's id
        /// </summary>
        private int conceptId1;

        /// <summary>
        /// MetaOperator's name
        /// </summary>
        private string metaOperatorName;

        /// <summary>
        /// Concept 1's id
        /// </summary>
        private int conceptId2;
        #endregion

        #region Constructor
        public MetaConnectionTheoryInfo()
        {
            this.conceptId1 = -1;
            this.metaOperatorName = null;
            this.conceptId2 = -1;
        }

        public MetaConnectionTheoryInfo(int conceptId1, string metaOperatorName, int conceptId2)
        {
            this.conceptId1 = conceptId1;
            this.metaOperatorName = metaOperatorName;
            this.conceptId2 = conceptId2;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Concept 1's id
        /// </summary>
        public int ConceptId1
        {
            get { return conceptId1; }
            set { conceptId1 = value; }
        }

        /// <summary>
        /// MetaOperator's name
        /// </summary>
        public string MetaOperatorName
        {
            get { return metaOperatorName; }
            set { metaOperatorName = value; }
        }

        /// <summary>
        /// Concept 2's id
        /// </summary>
        public int ConceptId2
        {
            get { return conceptId2; }
            set { conceptId2 = value; }
        }
        #endregion
    }
}