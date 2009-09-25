using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class Destroyer
    {
        #region Public Methods
        /// <summary>
        /// From conceptCollection, Remove connections going to concept
        /// (Use this when concept is obsolete)
        /// </summary>
        /// <param name="concept">concept to insulate</param>
        /// <param name="memory">memory to look into</param>
        public void Insulate(Concept conceptToInsulate, Memory memory)
        {
            foreach (Concept otherConcept in memory)
                RemoveOptimizedCollectionFromTo(otherConcept, conceptToInsulate);

            DestroyImplyConnectionDependingOn(conceptToInsulate);

            conceptToInsulate.ClearIndexToImplyCondition();

            memory.Remove(conceptToInsulate);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Remove optimized connections going from "conceptToLookInto" to "conceptToInsulate"
        /// </summary>
        /// <param name="conceptToLookInto">concept to look into</param>
        /// <param name="conceptToInsulate">concept to insulate</param>
        private void RemoveOptimizedCollectionFromTo(Concept conceptToLookInto, Concept conceptToInsulate)
        {
            Concept verb;
            ConnectionBranch optimizedBranch;
            foreach (KeyValuePair<Concept,ConnectionBranch> verbAndBranch in conceptToLookInto.OptimizedConnectionBranchList)
            {
                verb = verbAndBranch.Key;
                optimizedBranch = verbAndBranch.Value;

                optimizedBranch.ComplementConceptList.Remove(conceptToInsulate);
            }
        }

        /// <summary>
        /// Destroy all "imply" connections depending on concept to insulate
        /// </summary>
        /// <param name="conceptToInsulate">concept to insulate</param>
        private void DestroyImplyConnectionDependingOn(Concept conceptToInsulate)
        {
            Concept actionVerb, actionComplement;
            bool isPositive;

            HashSet<Condition> conditionList = new HashSet<Condition>(conceptToInsulate.IndexedImplyConditionList);
            HashSet<Concept> concernedConceptList;

            foreach (Condition condition in conditionList)
            {
                concernedConceptList = condition.GetConcernedConceptList();
                foreach (Concept currentConcept in concernedConceptList)
                    currentConcept.RemoveIndexToImplyCondition(condition);


                actionVerb = condition.ActionVerb;
                actionComplement = condition.ActionComplement;
                isPositive = condition.IsPositive;
                actionVerb.RemoveImplyConnection(actionComplement, condition, isPositive);
            }
        }
        #endregion
    }
}
