using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    [Serializable]
    class ConnectionTheoryInfo
    {
        #region Fields
        private int conceptId1;

        private int conceptId2;

        private int conceptId3;
        #endregion

        #region Constructor
        public ConnectionTheoryInfo()
        {
            this.conceptId1 = -1;
            this.conceptId2 = -1;
            this.conceptId3 = -1;
        }

        public ConnectionTheoryInfo(int conceptId1, int conceptId2, int conceptId3)
        {
            this.conceptId1 = conceptId1;
            this.conceptId2 = conceptId2;
            this.conceptId3 = conceptId3;
        }
        #endregion

        #region Properties
        public int ConceptId1
        {
            get { return conceptId1; }
            set { conceptId1 = value; }
        }

        public int ConceptId2
        {
            get { return conceptId2; }
            set { conceptId2 = value; }
        }

        public int ConceptId3
        {
            get { return conceptId3; }
            set { conceptId3 = value; }
        }
        #endregion
    }
}
