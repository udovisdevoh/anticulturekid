using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents the theorizer's memory
    /// </summary>
    class TheoryMemory
    {
        #region Constants
        /// <summary>
        /// Max theory count for each concept
        /// </summary>
        private static readonly int maxTheoryCountForConcept = 1000;
        #endregion

        #region Fields
        /// <summary>
        /// Total theory list
        /// </summary>
        private TheoryList totalTheoryList = new TheoryList();

        /// <summary>
        /// Theory list for each concept
        /// </summary>
        private Dictionary<Concept, TheoryList> theoryListForConcepts = new Dictionary<Concept, TheoryList>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Remember new theory
        /// </summary>
        /// <param name="theory">theory to remember</param>
        public void RememberNewTheory(Theory theory)
        {
            if (theory == null)
                return;

            #region We manage total theory list
            if (!totalTheoryList.Contains(theory))
                totalTheoryList.Add(theory);

            if (totalTheoryList.Count > maxTheoryCountForConcept)
                totalTheoryList.RemoveLeastProbableTheory((maxTheoryCountForConcept / 2) + 1);
            #endregion

            #region We manage theories added to list of theories for each concept
            TheoryList theoryListForConcept;

            if (!theoryListForConcepts.TryGetValue(theory.GetConcept(0), out theoryListForConcept))
            {
                theoryListForConcept = new TheoryList();
                theoryListForConcepts.Add(theory.GetConcept(0), theoryListForConcept);
            }

            if (!theoryListForConcept.Contains(theory))
                theoryListForConcept.Add(theory);

            if (theoryListForConcept.Count > maxTheoryCountForConcept)
                theoryListForConcept.RemoveLeastProbableTheory((maxTheoryCountForConcept / 2) + 1);
            #endregion

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

                if (theoryListForConcept.Count > maxTheoryCountForConcept)
                    theoryListForConcept.RemoveLeastProbableTheory((maxTheoryCountForConcept / 2) + 1);
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
