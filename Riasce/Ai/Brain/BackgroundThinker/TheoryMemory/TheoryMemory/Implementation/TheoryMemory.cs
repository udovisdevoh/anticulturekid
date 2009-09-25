using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TheoryMemory : AbstractTheoryMemory
    {
        #region Constants
        private static readonly int maxTheoryCountForConcept = 1000;
        #endregion

        #region Fields
        private TheoryList totalTheoryList = new TheoryList();

        private Dictionary<Concept, TheoryList> theoryListForConcepts = new Dictionary<Concept, TheoryList>();
        #endregion

        #region Public Methods
        public override void RememberNewTheory(Theory theory)
        {
            if (theory == null)
                return;

            if (!totalTheoryList.Contains(theory))
                totalTheoryList.Add(theory);

            TheoryList theoryListForConcept;

            if (!theoryListForConcepts.TryGetValue(theory.GetConcept(0), out theoryListForConcept))
            {
                theoryListForConcept = new TheoryList();
                theoryListForConcepts.Add(theory.GetConcept(0), theoryListForConcept);
            }

            if (!theoryListForConcept.Contains(theory))
                theoryListForConcept.Add(theory);

            if (theoryListForConcept.Count > maxTheoryCountForConcept + 2)
            {
                theoryListForConcept.RemoveOldestTheory();
                theoryListForConcept.RemoveLeastProbableTheory();
            }

            #region We add theory to complement concept if not null
            if (theory.GetConcept(2) != null)
            {
                if (!theoryListForConcepts.TryGetValue(theory.GetConcept(2), out theoryListForConcept))
                {
                    theoryListForConcept = new TheoryList();
                    theoryListForConcepts.Add(theory.GetConcept(2), theoryListForConcept);
                }

                if (!theoryListForConcept.Contains(theory))
                    theoryListForConcept.Add(theory);

                if (theoryListForConcept.Count > maxTheoryCountForConcept + 2)
                {
                    theoryListForConcept.RemoveOldestTheory();
                    theoryListForConcept.RemoveLeastProbableTheory();
                }
            }
            #endregion
        }

        public Theory GetBestLogicTheory(RejectedTheories rejectedTheories)
        {
            return totalTheoryList.GetBestLogicTheory(rejectedTheories);
        }

        public Theory GetBestLinguisticTheory(RejectedTheories rejectedTheories)
        {
            return totalTheoryList.GetBestLinguisticTheory(rejectedTheories);
        }

        public Theory GetBestPhoneticTheory(RejectedTheories rejectedTheories)
        {
            return totalTheoryList.GetBestPhoneticTheory(rejectedTheories);
        }

        public Theory GetBestMetaTheory(RejectedTheories rejectedTheories)
        {
            return totalTheoryList.GetBestMetaTheory(rejectedTheories);
        }

        public Theory GetBestMetaTheoryAbout(Concept subject, RejectedTheories rejectedTheories)
        {
            TheoryList theoryListForConcept;
            if (!theoryListForConcepts.TryGetValue(subject, out theoryListForConcept))
            {
                theoryListForConcept = new TheoryList();
                theoryListForConcepts.Add(subject, theoryListForConcept);
            }

            return theoryListForConcept.GetBestMetaTheory(rejectedTheories);
        }

        public Theory GetBestLogicTheoryAbout(Concept subject, RejectedTheories rejectedTheories)
        {
            TheoryList theoryListForConcept;
            if (!theoryListForConcepts.TryGetValue(subject, out theoryListForConcept))
            {
                theoryListForConcept = new TheoryList();
                theoryListForConcepts.Add(subject, theoryListForConcept);
            }

            return theoryListForConcept.GetBestLogicTheory(rejectedTheories);
        }

        public Theory GetBestLinguisticTheoryAbout(Concept subject, RejectedTheories rejectedTheories)
        {
            TheoryList theoryListForConcept;
            if (!theoryListForConcepts.TryGetValue(subject, out theoryListForConcept))
            {
                theoryListForConcept = new TheoryList();
                theoryListForConcepts.Add(subject, theoryListForConcept);
            }

            return theoryListForConcept.GetBestLinguisticTheory(rejectedTheories);
        }

        public Theory GetBestPhoneticTheoryAbout(Concept subject, RejectedTheories rejectedTheories)
        {
            TheoryList theoryListForConcept;
            if (!theoryListForConcepts.TryGetValue(subject, out theoryListForConcept))
            {
                theoryListForConcept = new TheoryList();
                theoryListForConcepts.Add(subject, theoryListForConcept);
            }

            return theoryListForConcept.GetBestPhoneticTheory(rejectedTheories);
        }

        public void RemoveExistingConnections()
        {
            totalTheoryList.RemoveExistingConnections();
        }

        public void Reset()
        {
            totalTheoryList.Clear();
            theoryListForConcepts.Clear();
        }
        #endregion

        #region Properties
        public TheoryList TotalTheoryList
        {
            get
            {
                totalTheoryList.Sort();
                return totalTheoryList;
            }
        }
        #endregion
    }
}
