using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a collection of theories
    /// </summary>
    class TheoryList : ICollection<Theory>
    {
        #region Fields
        /// <summary>
        /// Internal theory list
        /// </summary>
        private List<Theory> theoryList = new List<Theory>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Add a theory to list
        /// </summary>
        /// <param name="item">theory</param>
        public void Add(Theory item)
        {
            if (item != null)
                theoryList.Add(item);
        }

        /// <summary>
        /// Whether the list contains theory
        /// </summary>
        /// <param name="item">theory</param>
        /// <returns>true if list contains theory</returns>
        public bool Contains(Theory item)
        {
            foreach (Theory currentTheory in theoryList)
                if (currentTheory.Equals(item))
                    return true;

            return false;
        }

        /// <summary>
        /// Count theories in list
        /// </summary>
        public int Count
        {
            get { return theoryList.Count; }
        }

        /// <summary>
        /// Remove theory from list
        /// </summary>
        /// <param name="item">theory</param>
        /// <returns>true if succeeded</returns>
        public bool Remove(Theory item)
        {
            foreach (Theory currentTheory in theoryList)
            {
                if (currentTheory.Equals(item))
                {
                    theoryList.Remove(currentTheory);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove the oldest postulated theory
        /// </summary>
        /// <returns>true if succeed, else false</returns>
        public bool RemoveOldestTheory()
        {
            if (theoryList.Count > 0)
            {
                theoryList.RemoveAt(0);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove the least probable postulated theory
        /// </summary>
        /// <param name="howManyToRemove">how many theories to remove</param>
        /// <returns>true if succeed, else false</returns>
        public bool RemoveLeastProbableTheory(int howManyToRemove)
        {
            if (theoryList.Count > 0)
            {
                List<Theory> sortedTheoryList = new List<Theory>(theoryList);
                sortedTheoryList.Sort();

                while (sortedTheoryList.Count >= howManyToRemove && howManyToRemove > 0)
                {
                    sortedTheoryList.RemoveAt(sortedTheoryList.Count - 1);
                    howManyToRemove--;
                }

                this.theoryList = sortedTheoryList;

                return true;
            }
            return false;
        }

        public IEnumerator<Theory> GetEnumerator()
        {
            return theoryList.GetEnumerator();
        }
        
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public void CopyTo(Theory[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the most probable theory
        /// </summary>
        /// <param name="rejectedTheories">rejected theories</param>
        /// <returns>most probable logic theory</returns>
        public Theory GetBestLogicTheory(RejectedTheories rejectedTheories)
        {
            if (theoryList.Count < 1)
                throw new TheoryException("Couldn't make any theory for now");

            RemoveExistingConnections();
            RemoveRejectedTheories(rejectedTheories);

            if (theoryList.Count < 1)
                throw new TheoryException("Couldn't make any theory for now");

            List<Theory> sortedTheoryList = new List<Theory>(theoryList);
            sortedTheoryList.Sort();

            for (int i = 0; i < sortedTheoryList.Count; i++)
                if (!sortedTheoryList[i].IsSemanticLinguistic && !sortedTheoryList[i].IsPhonetic)
                    return sortedTheoryList[i];

            throw new TheoryException("Couldn't make any theory for now");
        }

        /// <summary>
        /// Get best linguistic theory
        /// </summary>
        /// <param name="rejectedTheories">rejected theories</param>
        /// <returns>most probable linguistic theory</returns>
        public Theory GetBestLinguisticTheory(RejectedTheories rejectedTheories)
        {
            if (theoryList.Count < 1)
                throw new TheoryException("Couldn't make any theory for now");

            RemoveExistingConnections();
            RemoveRejectedTheories(rejectedTheories);

            if (theoryList.Count < 1)
                throw new TheoryException("Couldn't make any theory for now");

            List<Theory> sortedTheoryList = new List<Theory>(theoryList);
            sortedTheoryList.Sort();

            for (int i = 0; i < sortedTheoryList.Count; i++)
                if (sortedTheoryList[i].IsSemanticLinguistic)
                    return sortedTheoryList[i];

            throw new TheoryException("Couldn't make any theory for now");
        }

        /// <summary>
        /// Get best phonetic theory
        /// </summary>
        /// <param name="rejectedTheories">rejected theories</param>
        /// <returns>most probable phonetic theory</returns>
        public Theory GetBestPhoneticTheory(RejectedTheories rejectedTheories)
        {
            if (theoryList.Count < 1)
                throw new TheoryException("Couldn't make any theory for now");

            RemoveExistingConnections();
            RemoveRejectedTheories(rejectedTheories);

            if (theoryList.Count < 1)
                throw new TheoryException("Couldn't make any theory for now");

            List<Theory> sortedTheoryList = new List<Theory>(theoryList);
            sortedTheoryList.Sort();

            for (int i = 0; i < sortedTheoryList.Count; i++)
                if (sortedTheoryList[i].IsPhonetic)
                    return sortedTheoryList[i];

            throw new TheoryException("Couldn't make any theory for now");
        }

        /// <summary>
        /// Get best metaConnection theory
        /// </summary>
        /// <param name="rejectedTheories">rejected theories</param>
        /// <returns>most probable metaConnection theory</returns>
        public Theory GetBestMetaTheory(RejectedTheories rejectedTheories)
        {
            if (theoryList.Count < 1)
                throw new TheoryException("Couldn't make any theory for now");

            RemoveExistingConnections();
            RemoveRejectedTheories(rejectedTheories);

            if (theoryList.Count < 1)
                throw new TheoryException("Couldn't make any theory for now");

            List<Theory> metaConnectionTheoryList = new List<Theory>();
            foreach (Theory theory in theoryList)
                if (theory.MetaOperatorName != null)
                    metaConnectionTheoryList.Add(theory);

            if (metaConnectionTheoryList.Count < 1)
                throw new TheoryException("Couldn't make any theory for now");

            List<Theory> sortedTheoryList = new List<Theory>(metaConnectionTheoryList);
            sortedTheoryList.Sort();
            return sortedTheoryList[0];
        }

        /// <summary>
        /// Remove theories about existing connections
        /// </summary>
        public void RemoveExistingConnections()
        {
            foreach (Theory theory in new List<Theory>(theoryList))
            {
                if (theory.GetConcept(0) != null && theory.GetConcept(1) != null && theory.GetConcept(2) != null)
                {
                    if (theory.GetConcept(0).GetOptimizedConnectionBranch(theory.GetConcept(1)).ComplementConceptList.Contains(theory.GetConcept(2)))
                    {
                        theoryList.Remove(theory);
                    }
                    else if (theory.GetConcept(0).GetFlatConnectionBranch(theory.GetConcept(1)).ComplementConceptList.Contains(theory.GetConcept(2)))
                    {
                        theoryList.Remove(theory);
                    }
                }
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Remove theories that got rejected
        /// </summary>
        /// <param name="rejectedTheories">theories that got rejected</param>
        public void RemoveRejectedTheories(RejectedTheories rejectedTheories)
        {
            foreach (Theory theory in new List<Theory>(theoryList))
            {
                if (theory.GetConcept(0) != null && theory.GetConcept(1) != null && theory.GetConcept(2) != null)
                {
                    if (rejectedTheories.Contains(theory))
                    {
                        theoryList.Remove(theory);
                    }
                }
            }
        }

        /// <summary>
        /// Sort the list by theory probability
        /// </summary>
        public void Sort()
        {
            theoryList.Sort();
        }

        /// <summary>
        /// Clear theory list
        /// </summary>
        public void Clear()
        {
            theoryList.Clear();
        }

        /// <summary>
        /// Remove theories that contain concept not in memory
        /// </summary>
        /// <param name="memory">memory</param>
        public void RemoveTheoriesAboutConceptNotIn(Memory memory)
        {
            HashSet<Theory> newList = new HashSet<Theory>(theoryList);
            foreach (Theory currentTheory in theoryList)
            {
                foreach (Concept featuredConcept in currentTheory.GetFeaturedConceptList())
                {
                    if (!memory.Contains(featuredConcept))
                    {
                        newList.Remove(currentTheory);
                        break;
                    }
                }
            }

            theoryList = new List<Theory>(newList);
        }
        #endregion
    }
}
