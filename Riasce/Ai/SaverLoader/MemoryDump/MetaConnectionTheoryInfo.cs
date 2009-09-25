using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    [Serializable]
    class MetaConnectionTheoryInfo
    {
        #region Fields
        private int conceptId1;

        private string metaOperatorName;

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
        public int ConceptId1
        {
            get { return conceptId1; }
            set { conceptId1 = value; }
        }

        public string MetaOperatorName
        {
            get { return metaOperatorName; }
            set { metaOperatorName = value; }
        }

        public int ConceptId2
        {
            get { return conceptId2; }
            set { conceptId2 = value; }
        }
        #endregion
    }
}
