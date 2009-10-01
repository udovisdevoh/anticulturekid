using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used to add, remove and test "Imply" connections
    /// </summary>
    static class ImplyConnectionManager
    {
        #region Public Methods
        /// <summary>
        /// Add imply metaconnection to verb
        /// </summary>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <param name="condition">condition</param>
        public static void AddImplyConnection(Concept verb, Concept complement, Condition condition)
        {
            Memory.TotalVerbList.Add(verb);

            condition.ActionVerb = verb;
            condition.ActionComplement = complement;
            condition.IsPositive = true;

            HashSet<Concept> concernedConceptList;

            HashSet<Concept> inverseVerbList = verb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("inverse_of");
            inverseVerbList.UnionWith(verb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("permutable_side"));

            if (inverseVerbList.Count < 1)
                throw new ImplyConnectionException("Cannot create imply connection to verb that have no metaConnection of type inverse_of or permutable_side");

            if (condition.IsOrContains(verb,complement))
                throw new ImplyConnectionException("Totology detected: the conditions contains the result");

            verb.AddImplyConnection(complement, condition, true);

            #region Adding indexes to positive condition
            verb.AddIndexToImplyCondition(condition);
            complement.AddIndexToImplyCondition(condition);
            concernedConceptList = condition.GetConcernedConceptList();
            foreach (Concept currentConcept in concernedConceptList)
                currentConcept.AddIndexToImplyCondition(condition);
            #endregion

            Condition inverseCondition = condition.Reverse();

            #region Adding indexes to inverse condition
            foreach (Concept inverseVerb in inverseVerbList)
            {
                Memory.TotalVerbList.Add(inverseVerb);
                inverseCondition.ActionVerb = inverseVerb;
                inverseCondition.ActionComplement = complement;
                complement.AddIndexToImplyCondition(inverseCondition);
                break;
            }
            concernedConceptList = inverseCondition.GetConcernedConceptList();
            foreach (Concept currentConcept in concernedConceptList)
            {
                currentConcept.AddIndexToImplyCondition(inverseCondition);
            }
            #endregion

            foreach (Concept inverseVerb in inverseVerbList)
            {
                inverseCondition.ActionVerb = inverseVerb;
                inverseCondition.ActionComplement = complement;
                inverseCondition.IsPositive = false;
                inverseVerb.AddIndexToImplyCondition(inverseCondition);
                inverseVerb.AddImplyConnection(complement, inverseCondition, false);
            }

            RepairedFlatBranchCache.Clear();
        }

        /// <summary>
        /// Remove imply metaconnection to verb
        /// </summary>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <param name="condition">condition</param>
        public static void RemoveImplyConnection(Concept verb, Concept complement, Condition condition)
        {
            verb.RemoveImplyConnection(complement, condition, true);

            HashSet<Concept> inverseVerbList = verb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("inverse_of");
            inverseVerbList.UnionWith(verb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("permutable_side"));

            Condition inverseCondition = condition.Reverse();

            foreach (Concept inverseVerb in inverseVerbList)
                inverseVerb.RemoveImplyConnection(complement, inverseCondition, false);

            RepairedFlatBranchCache.Clear();
        }

        /// <summary>
        /// Returns true if Imply Connection exists
        /// </summary>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <param name="condition">condition</param>
        /// <returns>Returns true if Imply Connection exists, else: false</returns>
        public static bool TestImplyConnection(Concept verb, Concept complement, Condition condition)
        {
            bool exists = verb.TestImplyConnection(complement, condition, true);

            HashSet<Concept> inverseVerbList = verb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("inverse_of");
            inverseVerbList.UnionWith(verb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("permutable_side"));

            Condition inverseCondition = condition.Reverse();

            foreach (Concept inverseVerb in inverseVerbList)
                if (exists != inverseVerb.TestImplyConnection(complement, inverseCondition, false))
                    throw new ImplyConnectionException("Imply connection inconsistency");

            return exists;
        }
        #endregion
    }
}
