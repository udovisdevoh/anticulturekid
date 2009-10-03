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

        /// <summary>
        /// Get best logic theory
        /// </summary>
        /// <param name="rejectedTheories">rejected theories</param>
        /// <returns>best logic theory</returns>
        public Theory GetBestLogicTheory(RejectedTheories rejectedTheories)
        {
            return totalTheoryList.GetBestLogicTheory(rejectedTheories);
        }

        /// <summary>
        /// Get best linguistic theory
        /// </summary>
        /// <param name="rejectedTheories">rejected theories</param>
        /// <returns>best linguistic theory</returns>
        public Theory GetBestLinguisticTheory(RejectedTheories rejectedTheories)
        {
            return totalTheoryList.GetBestLinguisticTheory(rejectedTheories);
        }

        /// <summary>
        /// Get best phonetic theory
        /// </summary>
        /// <param name="rejectedTheories">rejected theories</param>
        /// <returns>best phonetic theory</returns>
        public Theory GetBestPhoneticTheory(RejectedTheories rejectedTheories)
        {
            return totalTheoryList.GetBestPhoneticTheory(rejectedTheories);
        }

        /// <summary>
        /// Get best metaConnection theory
        /// </summary>
        /// <param name="rejectedTheories">rejected theories</param>
        /// <returns>best metaConnection theory</returns>
        public Theory GetBestMetaTheory(RejectedTheories rejectedTheories)
        {
            return totalTheoryList.GetBestMetaTheory(rejectedTheories);
        }

        /// <summary>
        /// Get best metaConnection theory about subject concept
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="rejectedTheories">rejected theories</param>
        /// <returns>best metaConnection theory about subject concept</returns>
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

        /// <summary>
        /// Get best logic theory about subject concept
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="rejectedTheories">rejected theories</param>
        /// <returns>best logic theory about subject concept</returns>
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

        /// <summary>
        /// Get best linguistic theory about subject concept
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="rejectedTheories">rejected theories</param>
        /// <returns>best linguistic theory about subject concept</returns>
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

        /// <summary>
        /// Get best phonetic theory about subject concept
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="rejectedTheories">rejected theories</param>
        /// <returns>best phonetic theory about subject concept</returns>
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

        /// <summary>
        /// Remove theories about existing connections
        /// </summary>
        public void RemoveExistingConnections()
        {
            totalTheoryList.RemoveExistingConnections();
        }

        /// <summary>
        /// Reset all theory lists
        /// </summary>
        public void Reset()
        {
            totalTheoryList.Clear();
            theoryListForConcepts.Clear();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Total theory list
        /// </summary>
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
