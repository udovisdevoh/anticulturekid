using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TheoryList : AbstractTheoryList
    {
        #region Fields
        private List<Theory> theoryList = new List<Theory>();
        #endregion

        #region Public Methods
        public override void Add(Theory item)
        {
            if (item != null)
                theoryList.Add(item);
        }

        public override bool Contains(Theory item)
        {
            foreach (Theory currentTheory in theoryList)
                if (currentTheory.Equals(item))
                    return true;

            return false;
        }

        public override int Count
        {
            get { return theoryList.Count; }
        }

        public override bool Remove(Theory item)
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

        public override bool RemoveOldestTheory()
        {
            if (theoryList.Count > 0)
            {
                theoryList.RemoveAt(0);
                return true;
            }
            return false;
        }

        public override bool RemoveLeastProbableTheory()
        {
            if (theoryList.Count > 0)
            {
                List<Theory> sortedTheoryList = new List<Theory>(theoryList);
                sortedTheoryList.Sort();
                sortedTheoryList.RemoveAt(sortedTheoryList.Count - 1);
                return true;
            }
            return false;
        }

        public override IEnumerator<Theory> GetEnumerator()
        {
            return theoryList.GetEnumerator();
        }

        public override Theory GetBestLogicTheory(RejectedTheories rejectedTheories)
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

        public override Theory GetBestLinguisticTheory(RejectedTheories rejectedTheories)
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

        public override Theory GetBestPhoneticTheory(RejectedTheories rejectedTheories)
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

        public override Theory GetBestMetaTheory(RejectedTheories rejectedTheories)
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

        public override void Sort()
        {
            theoryList.Sort();
        }

        public override void Clear()
        {
            theoryList.Clear();
        }
        #endregion
    }
}
