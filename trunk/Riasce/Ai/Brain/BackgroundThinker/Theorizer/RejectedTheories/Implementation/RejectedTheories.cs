using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class RejectedTheories : AbstractRejectedTheories
    {
        #region Fields
        private static readonly int maxCount = 45;

        private List<Theory> theoryList = new List<Theory>();
        #endregion

        #region Constructor
        public RejectedTheories()
        {
        }
        #endregion

        #region Methods
        public override void Add(Theory theory)
        {
            theoryList.Add(theory);
            if (theoryList.Count > maxCount && theoryList.Count > 0)
                theoryList.Remove(GetMostProbableTheory());
        }

        public override bool Contains(Theory theory)
        {
            foreach (Theory otherTheory in theoryList)
                if (theory.Equals(otherTheory))
                    return true;

            return false;
        }

        public override bool ContainsOneFromRange(IEnumerable<Theory> contraposateList)
        {
            foreach (Theory currentTheory in contraposateList)
                if (Contains(currentTheory))
                    return true;
            return false;
        }

        private Theory GetMostProbableTheory()
        {
            if (theoryList.Count < 1)
                return null;

            Theory mostProbableTheory = null;
            foreach (Theory currentTheory in theoryList)
                if (mostProbableTheory == null || currentTheory > mostProbableTheory)
                    mostProbableTheory = currentTheory;

            return mostProbableTheory;
        }

        public override IEnumerator<Theory> GetEnumerator()
        {
            return theoryList.GetEnumerator();
        }

        public override void Assimilate(RejectedTheories otherRejectedSet)
        {
            foreach (Theory theory in otherRejectedSet)
                if (!this.Contains(theory))
                    this.Add(theory);
        }

        public void Clear()
        {
            theoryList.Clear();
        }
        #endregion
    }
}
