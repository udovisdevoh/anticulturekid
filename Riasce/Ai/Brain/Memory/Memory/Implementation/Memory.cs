using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public class Memory : AbstractMemory
    {
        #region Fields
        private Dictionary<int, Concept> conceptList = new Dictionary<int, Concept>();

        private Dictionary<Concept, int> idList = new Dictionary<Concept, int>();

        private EpisodicMemory episodicMemory = new EpisodicMemory();

        private static HashSet<Concept> totalVerbList = new HashSet<Concept>();
        #endregion

        #region Methods
        public override Concept GetOrCreateConcept(int conceptId)
        {
            Concept concept;
            if (!conceptList.TryGetValue(conceptId, out concept))
            {
                concept = new Concept(conceptId.ToString());
                conceptList.Add(conceptId, concept);
                idList.Add(concept, conceptId);
            }
            return concept;
        }

        public override int GetIdFromConcept(Concept concept)
        {
            int conceptId;
            if (idList.TryGetValue(concept, out conceptId))
                return conceptId;
            else
                throw new MemoryException("Couldn't find concept in memory");            
        }

        public override void SetConcept(int targetConcepId, Concept concept)
        {
            if (conceptList.ContainsKey(targetConcepId) || idList.ContainsKey(concept))
                throw new MemoryException("Concept already assigned in memory for id " + targetConcepId);

            conceptList.Add(targetConcepId, concept);
            idList.Add(concept, targetConcepId);
        }

        public override IEnumerator<Concept> GetEnumerator()
        {
            return conceptList.Values.GetEnumerator();
        }

        public override bool Remove(Concept concept)
        {
            int conceptId;
            if (idList.TryGetValue(concept, out conceptId))
            {
                conceptList.Remove(conceptId);
                idList.Remove(concept);
                return true;
            }
            return false;
        }

        public override int Count
        {
            get { return conceptList.Count; }
        }

        public override void CopyTo(Concept[] array, int arrayIndex)
        {
            conceptList.Values.CopyTo(array, arrayIndex);
        }

        public override void EpisodicRemember(string authorName, string textComment)
        {
            episodicMemory.RememberString(authorName, textComment);
        }

        public override void EpisodicRemember(string authorName, List<Concept> conceptList, bool isNegative, bool isInterrogative)
        {
            episodicMemory.RememberOperation(authorName, conceptList, isNegative, isInterrogative);
        }

        public override void EpisodicRemember(string authorName, List<string> stringList, List<Concept> conceptList, bool isNegative)
        {
            episodicMemory.RememberListStringAndConcept(authorName, stringList, conceptList, isNegative);
        }

        public override void EpisodicRemember(string authorName, List<Concept> conceptList, string metaOperator, bool isNegative, bool isInterrogative)
        {
            episodicMemory.RememberMetaOperation(authorName, conceptList, metaOperator, isNegative, isInterrogative);
        }

        public override void EpisodicRemember(string authorName, Statement statement)
        {
            episodicMemory.RememberStatement(authorName, statement);
        }
        #endregion

        #region Properties
        public static HashSet<Concept> TotalVerbList
        {
            get { return totalVerbList; }
            set { totalVerbList = value; }
        }

        public List<Concept> ReversedConceptList
        {
            get
            {
                List<Concept> conceptList = new List<Concept>(this);
                conceptList.Reverse();
                return conceptList;
            }
        }
        #endregion
    }
}
