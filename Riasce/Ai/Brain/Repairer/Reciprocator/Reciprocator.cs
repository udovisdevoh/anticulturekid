using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used to copy a concepts connection to its complement as inverse or permutable side connection
    /// </summary>
    static class Reciprocator
    {
        #region Methods
        /// <summary>
        /// Repair a concept's reciprocal connections, (verbs metaConnected with inverse_of or permutable_side)
        /// (The only connections affected will be the optimized connections)
        /// </summary>
        /// <param name="concept">Concept to repair</param>
        public static void Reciprocate(Concept subject)
        {
            FeelingMonitor.Add(FeelingMonitor.RECIPROCATING);
            Repair(subject);
        }

        /// <summary>
        /// Repair a collection of concepts's reciprocal connections
        /// </summary>
        /// <param name="conceptCollection">collection of concepts</param>
        public static void ReciprocateRange(IEnumerable<Concept> conceptCollection)
        {
            FeelingMonitor.Add(FeelingMonitor.RECIPROCATING);
            foreach (Concept subject in conceptCollection)
                Repair(subject);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Repair a concept
        /// </summary>
        /// <param name="subject">subject concept to repair</param>
        private static void Repair(Concept subject)
        {
            if (subject.IsFlatDirty || subject.IsOptimizedDirty)
                throw new ReciprocatorException("Repair concept first");

            foreach (KeyValuePair<Concept, ConnectionBranch> branchAndVerb in subject.FlatConnectionBranchList)
            {
                Concept verb = branchAndVerb.Key;
                ConnectionBranch branch = branchAndVerb.Value;

                #region We try to get the verb list from verbMetaConnectionCache
                HashSet<Concept> inverseOfVerbList = VerbMetaConnectionCache.GetVerbFlatListFromCache(verb, "inverse_of", true);
                if (inverseOfVerbList == null)
                {
                    inverseOfVerbList = MetaConnectionManager.GetVerbFlatListFromMetaConnection(verb, "inverse_of", true);
                    VerbMetaConnectionCache.Remember(verb, "inverse_of", true, inverseOfVerbList);
                }
                
                HashSet<Concept> permutableSideVerbList = VerbMetaConnectionCache.GetVerbFlatListFromCache(verb, "permutable_side", true);
                if (permutableSideVerbList == null)
                {
                    permutableSideVerbList = MetaConnectionManager.GetVerbFlatListFromMetaConnection(verb, "permutable_side", true);
                    VerbMetaConnectionCache.Remember(verb, "permutable_side", true, permutableSideVerbList);
                }
                #endregion

                foreach (Concept complement in branch.ComplementConceptList)
                {
                    foreach (Concept reciprocVerb in inverseOfVerbList)
                        complement.AddOptimizedConnection(reciprocVerb, subject);
                    foreach (Concept reciprocVerb in permutableSideVerbList)
                        complement.AddOptimizedConnection(reciprocVerb, subject);
                }
            }

            RepairedFlatBranchCache.Clear();

            subject.IsFlatDirty = true;
        }
        #endregion
    }
}
