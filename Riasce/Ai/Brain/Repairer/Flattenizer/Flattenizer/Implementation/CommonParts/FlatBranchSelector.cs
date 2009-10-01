using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    static class FlatBranchSelector
    {
        #region Public Methods
        /// <summary>
        /// Get which branches to repair
        /// keys: flat branches
        /// values: optimized branches
        /// </summary>
        /// <param name="flatBranch">parent flatBranch</param>
        /// <param name="optimizedBranch">parent optmizedBranch</param>
        /// <param name="subject">subject</param>
        /// <param name="verb">verb</param>
        /// <param name="repairedBranches"></param>
        /// <returns>which branches to repair (keys: flat branches, values: optimized branches)</returns>
        public static Dictionary<ConnectionBranch, ConnectionBranch> GetBranchesToRepair(ConnectionBranch flatBranch, ConnectionBranch optimizedBranch, Concept subject, Concept verb, HashSet<ConnectionBranch> repairedBranches)
        {
            Dictionary<ConnectionBranch, ConnectionBranch> branchesToRepair = new Dictionary<ConnectionBranch, ConnectionBranch>();

            AddBranchesToRepairFromDirectImplication(branchesToRepair, flatBranch, optimizedBranch, subject, verb, repairedBranches);
            AddBranchesToRepairFromMuct(branchesToRepair, flatBranch, optimizedBranch, subject, verb, repairedBranches);
            AddBranchesToRepairFromLiffid(branchesToRepair, flatBranch, optimizedBranch, subject, verb, repairedBranches);
            AddBranchesToRepairFromNegativeImply(branchesToRepair, flatBranch, optimizedBranch, subject, verb, repairedBranches);
            AddBranchesToRepairFromPositiveImply(branchesToRepair, flatBranch, optimizedBranch, subject, verb, repairedBranches);

            return branchesToRepair;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Add flat branches to repair from direct implication
        /// </summary>
        /// <param name="branchesToRepair">branches to repair</param>
        /// <param name="flatBranch">parent flat branch</param>
        /// <param name="optimizedBranch">optimized branch</param>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="repairedBranches">repaired branches</param>
        private static void AddBranchesToRepairFromDirectImplication(Dictionary<ConnectionBranch, ConnectionBranch> branchesToRepair, ConnectionBranch flatBranch, ConnectionBranch optimizedBranch, Concept subject, Concept verb, HashSet<ConnectionBranch> repairedBranches)
        {
            HashSet<Concept> directImplicationVerbList = VerbMetaConnectionCache.GetVerbFlatListFromCache(verb, "direct_implication", false);
            if (directImplicationVerbList == null)
            {
                directImplicationVerbList = MetaConnectionManager.GetVerbFlatListFromMetaConnection(verb, "direct_implication", false);
                VerbMetaConnectionCache.Remember(verb, "direct_implication", false, directImplicationVerbList);
            }

            foreach (Concept directlyImpliedVerb in directImplicationVerbList)
            {
                ConnectionBranch farFlatBranch = subject.GetFlatConnectionBranch(directlyImpliedVerb);
                ConnectionBranch farOptimizedBranch = subject.GetOptimizedConnectionBranch(directlyImpliedVerb);
                
                if (!repairedBranches.Contains(farFlatBranch))
                    if (!branchesToRepair.ContainsKey(farFlatBranch))
                        branchesToRepair.Add(farFlatBranch, farOptimizedBranch);
            }
        }

        /// <summary>
        /// Add branches to repair from muct
        /// </summary>
        /// <param name="branchesToRepair">branches to repair</param>
        /// <param name="flatBranch">parent flat branch</param>
        /// <param name="optimizedBranch">optimized branch</param>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="repairedBranches">repaired branches</param>
        private static void AddBranchesToRepairFromMuct(Dictionary<ConnectionBranch, ConnectionBranch> branchesToRepair, ConnectionBranch flatBranch, ConnectionBranch optimizedBranch, Concept subject, Concept verb, HashSet<ConnectionBranch> repairedBranches)
        {
            HashSet<Concept> muctVerbList = VerbMetaConnectionCache.GetVerbFlatListFromCache(verb, "muct", true);
            if (muctVerbList == null)
            {
                muctVerbList = MetaConnectionManager.GetVerbFlatListFromMetaConnection(verb, "muct", true);
                VerbMetaConnectionCache.Remember(verb, "muct", true, muctVerbList);
            }

            foreach (Concept muctVerb in muctVerbList)
            {
                HashSet<Concept> complementList = new HashSet<Concept>(subject.GetFlatConnectionBranch(muctVerb).ComplementConceptList);

                foreach (Concept complement in complementList)
                {
                    ConnectionBranch farFlatBranch = complement.GetFlatConnectionBranch(verb);
                    ConnectionBranch farOptimizedBranch = complement.GetOptimizedConnectionBranch(verb);
                    
                    if (!repairedBranches.Contains(farFlatBranch))
                        if (!branchesToRepair.ContainsKey(farFlatBranch))
                            branchesToRepair.Add(farFlatBranch, farOptimizedBranch);
                }
            }
        }

        /// <summary>
        /// Add branches to repair from liffid
        /// </summary>
        /// <param name="branchesToRepair">branches to repair</param>
        /// <param name="flatBranch">parent flat branch</param>
        /// <param name="optimizedBranch">optimized branch</param>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="repairedBranches">repaired branches</param>
        private static void AddBranchesToRepairFromLiffid(Dictionary<ConnectionBranch, ConnectionBranch> branchesToRepair, ConnectionBranch flatBranch, ConnectionBranch optimizedBranch, Concept subject, Concept verb, HashSet<ConnectionBranch> repairedBranches)
        {
            HashSet<Concept> liffidVerbList = VerbMetaConnectionCache.GetVerbFlatListFromCache(verb, "liffid", true);
            if (liffidVerbList == null)
            {
                liffidVerbList = MetaConnectionManager.GetVerbFlatListFromMetaConnection(verb, "liffid", true);
                VerbMetaConnectionCache.Remember(verb, "liffid", true, liffidVerbList);
            }

            foreach (Concept liffidVerb in liffidVerbList)
            {
                HashSet<Concept> complementList = new HashSet<Concept>(subject.GetFlatConnectionBranch(verb).ComplementConceptList);

                foreach (Concept complement in complementList)
                {
                    ConnectionBranch farFlatBranch = complement.GetFlatConnectionBranch(liffidVerb);
                    ConnectionBranch farOptimizedBranch = complement.GetOptimizedConnectionBranch(liffidVerb);

                    if (!repairedBranches.Contains(farFlatBranch))
                        if (!branchesToRepair.ContainsKey(farFlatBranch))
                            branchesToRepair.Add(farFlatBranch, farOptimizedBranch);
                }
            }
        }

        /// <summary>
        /// Add branches to repair from positive imply
        /// </summary>
        /// <param name="branchesToRepair">branches to repair</param>
        /// <param name="flatBranch">parent flat branch</param>
        /// <param name="optimizedBranch">optimized branch</param>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="repairedBranches">repaired branches</param>
        private static void AddBranchesToRepairFromPositiveImply(Dictionary<ConnectionBranch, ConnectionBranch> branchesToRepair, ConnectionBranch flatBranch, ConnectionBranch optimizedBranch, Concept subject, Concept verb, HashSet<ConnectionBranch> repairedBranches)
        {
            Concept complement;
            HashSet<Condition> conditionList;
            foreach (KeyValuePair<Concept, HashSet<Condition>> complementAndConditionList in verb.ImplyConnectionTreePositive)
            {
                complement = complementAndConditionList.Key;
                conditionList = complementAndConditionList.Value;

                foreach (Condition condition in conditionList)
                {
                    AddBranchesToRepairFromConditionPositiveImply(branchesToRepair, flatBranch, optimizedBranch, subject, verb, condition, repairedBranches);
                }
            }
        }

        /// <summary>
        /// Add branches to repair from negative imply
        /// </summary>
        /// <param name="branchesToRepair">branches to repair</param>
        /// <param name="flatBranch">parent flat branch</param>
        /// <param name="optimizedBranch">optimized branch</param>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="repairedBranches">repaired branches</param>
        private static void AddBranchesToRepairFromNegativeImply(Dictionary<ConnectionBranch, ConnectionBranch> branchesToRepair, ConnectionBranch flatBranch, ConnectionBranch optimizedBranch, Concept subject, Concept verb, HashSet<ConnectionBranch> repairedBranches)
        {
            Concept complement;
            HashSet<Condition> conditionList;
            foreach (KeyValuePair<Concept, HashSet<Condition>> complementAndConditionList in verb.ImplyConnectionTreeNegative)
            {
                complement = complementAndConditionList.Key;
                conditionList = complementAndConditionList.Value;

                foreach (Condition condition in conditionList)
                {
                    AddBranchesToRepairFromConditionNegativeImply(branchesToRepair, flatBranch, optimizedBranch, subject, verb, condition, repairedBranches);
                }
            }
        }

        /// <summary>
        /// Repair flat branch from positive imply condition
        /// </summary>
        /// <param name="branchesToRepair">branches to repair</param>
        /// <param name="flatBranch">parent flat branch</param>
        /// <param name="optimizedBranch">optimized branch</param>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="condition">positive imply condition</param>
        /// <param name="repairedBranches">repaired branches</param>
        private static void AddBranchesToRepairFromConditionPositiveImply(Dictionary<ConnectionBranch, ConnectionBranch> branchesToRepair, ConnectionBranch flatBranch, ConnectionBranch optimizedBranch, Concept subject, Concept verb, Condition condition, HashSet<ConnectionBranch> repairedBranches)
        {
            if (subject == condition.ActionComplement) //Cannot plug a concept to itself
                return;

            //Get list of branch on which the condition is dependent on
            HashSet<Concept> verbListToFlattenize = condition.GetVerbListToFlattenize();

            foreach (Concept dependantVerb in verbListToFlattenize)
            {
                ConnectionBranch farFlatBranch = subject.GetFlatConnectionBranch(dependantVerb);
                ConnectionBranch farOptimizedBranch = subject.GetOptimizedConnectionBranch(dependantVerb);

                if (!repairedBranches.Contains(farFlatBranch))
                    if (!branchesToRepair.ContainsKey(farFlatBranch))
                        branchesToRepair.Add(farFlatBranch, farOptimizedBranch);
            }
        }

        /// <summary>
        /// Repair flat branch from negative imply condition
        /// </summary>
        /// <param name="branchesToRepair">branches to repair</param>
        /// <param name="flatBranch">parent flat branch</param>
        /// <param name="optimizedBranch">optimized branch</param>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="condition">negative imply condition</param>
        /// <param name="repairedBranches">repaired branches</param>
        private static void AddBranchesToRepairFromConditionNegativeImply(Dictionary<ConnectionBranch, ConnectionBranch> branchesToRepair, ConnectionBranch flatBranch, ConnectionBranch optimizedBranch, Concept subject, Concept verb, Condition condition, HashSet<ConnectionBranch> repairedBranches)
        {
            if (subject != condition.ActionComplement) //Ignore flattenization if subject is not concerned
                return;

            //Get list of branch on which the condition is dependent on
            List<List<Concept>> branchPrototypeToFlattenizeList = condition.GetBranchSignatureListToFlattenize();

            foreach (List<Concept> branchSignature in branchPrototypeToFlattenizeList)
            {
                Concept farVerb = branchSignature[0];
                Concept farComplement = branchSignature[1];

                ConnectionBranch farFlatBranch = farComplement.GetFlatConnectionBranch(farVerb);
                ConnectionBranch farOptimizedBranch = farComplement.GetOptimizedConnectionBranch(farVerb);

                if (!repairedBranches.Contains(farFlatBranch))
                    if (!branchesToRepair.ContainsKey(farFlatBranch))
                        branchesToRepair.Add(farFlatBranch, farOptimizedBranch);
            }
        }
        #endregion
    }
}