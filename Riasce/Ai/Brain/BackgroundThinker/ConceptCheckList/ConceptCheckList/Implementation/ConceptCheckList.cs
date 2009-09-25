using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riasce
{
    class ConceptCheckList : AbstractConceptCheckList
    {
        #region Fields
        private HashSet<Concept> indexedConcepts = new HashSet<Concept>();

        private Dictionary<Concept, NeglectedConceptInfo> indexedNeglectedConceptInfo = new Dictionary<Concept, NeglectedConceptInfo>();
        #endregion

        #region Public Methods
        public override Concept GetNextNeglectedConcept(Memory memory)
        {
            foreach (Concept currentConcept in memory)
            {
                if (!ContainsConcept(currentConcept))
                {
                    remember(currentConcept, DateTime.Now);
                    return currentConcept;
                }
            }

            List<NeglectedConceptInfo> neglectedConceptInfoList = new List<NeglectedConceptInfo>(indexedNeglectedConceptInfo.Values);

            if (neglectedConceptInfoList.Count > 0)
            {
                neglectedConceptInfoList.Sort();
                remember(neglectedConceptInfoList[0].Concept, DateTime.Now);
                return neglectedConceptInfoList[0].Concept;
            }

            return null;
        }
        #endregion

        #region Private Methods
        private bool ContainsConcept(Concept currentConcept)
        {
            return indexedConcepts.Contains(currentConcept);
        }

        private void remember(Concept currentConcept, DateTime dateTime)
        {
            indexedConcepts.Add(currentConcept);

            NeglectedConceptInfo neglectedConceptInfo;

            if (indexedNeglectedConceptInfo.TryGetValue(currentConcept, out neglectedConceptInfo))
            {
                neglectedConceptInfo.DateTime = dateTime;
            }
            else
            {
                neglectedConceptInfo = new NeglectedConceptInfo(currentConcept, dateTime);
                indexedNeglectedConceptInfo.Add(currentConcept, neglectedConceptInfo);
            }
        }
        #endregion
    }
}
