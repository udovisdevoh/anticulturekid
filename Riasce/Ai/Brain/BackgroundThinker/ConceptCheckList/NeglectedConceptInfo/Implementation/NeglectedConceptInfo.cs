using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riasce
{
    class NeglectedConceptInfo : AbstractNeglectedConceptInfo
    {
        #region Private Fields
        private DateTime dateTime;

        private Concept concept;
        #endregion

        #region Constructor
        public NeglectedConceptInfo(Concept concept, DateTime dateTime)
        {
            this.concept = concept;
            this.dateTime = dateTime;
        }
        #endregion

        #region Override
        public override int CompareTo(NeglectedConceptInfo other)
        {
            TimeSpan duration = this.dateTime - other.dateTime;
            return duration.Seconds;
        }
        #endregion

        #region Properties
        public Concept Concept
        {
            get { return concept; }
        }

        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }
        #endregion
    }
}
