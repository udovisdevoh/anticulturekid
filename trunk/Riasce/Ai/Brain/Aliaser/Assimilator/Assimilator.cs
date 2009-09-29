using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used by aliaser to allow a concept to assimilate another concept's connections
    /// </summary>
    class Assimilator
    {
        #region Public Methods
        /// <summary>
        /// Make assimilator concept assimilate assimilated concept
        /// </summary>
        /// <param name="assimilator">assimilator concept</param>
        /// <param name="assimilated">assimilated concept</param>
        public void Assimilate(Concept assimilator, Concept assimilated)
        {
            AssimilateConnections(assimilator, assimilated);
            AssimilateImplyConnections(assimilator, assimilated);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// "Assimilator concept" will assimilate "assimilated concept"'s connections
        /// </summary>
        /// <param name="assimilator">assimilator concept</param>
        /// <param name="assimilated">assimilated concept</param>
        private void AssimilateConnections(Concept assimilator, Concept assimilated)
        {
            Concept verb;
            ConnectionBranch optimizedBranch;
            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in assimilated.OptimizedConnectionBranchList)
            {
                verb = verbAndBranch.Key;
                optimizedBranch = verbAndBranch.Value;

                foreach (Concept complement in optimizedBranch.ComplementConceptList)
                    if (ConnectionManager.FindObstructionToPlug(assimilator, verb, complement, false) != null)
                        throw new AssimilationException("These concepts have incompatible connections and can't be merged");
            }

            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in assimilated.OptimizedConnectionBranchList)
            {
                verb = verbAndBranch.Key;
                optimizedBranch = verbAndBranch.Value;

                foreach (Concept complement in optimizedBranch.ComplementConceptList)
                    if (ConnectionManager.FindObstructionToPlug(assimilator, verb, complement, false) == null)
                    {
                        if (assimilator != complement)
                        {
                            assimilator.GetOptimizedConnectionBranch(verb).AddConnection(complement);
                        }
                    }
                    else
                    {
                        throw new AssimilationException("These concepts have incompatible connections and can't be merged");
                    }
            }
            assimilator.IsFlatDirty = true;
        }

        /// <summary>
        /// "Assimilator concept" will assimilate "assimilated concept"'s imply connections
        /// </summary>
        /// <param name="assimilator">assimilator concept</param>
        /// <param name="assimilated">assimilated concept</param>
        private void AssimilateImplyConnections(Concept assimilator, Concept assimilated)
        {
            Condition newCondition;
            Concept actionVerb, actionComplement;
            bool isPositive;
            HashSet<Concept> newConcernedConceptList;

            foreach (Condition condition in assimilated.IndexedImplyConditionList)
            {
                actionVerb = condition.ActionVerb;
                actionComplement = condition.ActionComplement;
                isPositive = condition.IsPositive;

                if (actionVerb == null || actionComplement == null)
                    throw new AssimilationException("Invalid condition's action verb or complement");

                if (actionVerb == assimilated)
                    actionVerb = assimilator;

                if (actionComplement == assimilated)
                    actionComplement = assimilator;

                newCondition = condition.ReplaceConcept(assimilated, assimilator);
                newCondition.ActionVerb = actionVerb;
                newCondition.ActionComplement = actionComplement;

                actionVerb.AddImplyConnection(actionComplement, newCondition, isPositive);

                newConcernedConceptList = newCondition.GetConcernedConceptList();
                foreach (Concept currentConcept in newConcernedConceptList)
                {
                    currentConcept.AddIndexToImplyCondition(newCondition);
                }
            }
        }
        #endregion
    }
}
