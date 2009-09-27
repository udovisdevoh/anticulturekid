using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used to copy a concepts connection to its complement as inverse or permutable side connection
    /// </summary>
    class Reciprocator
    {
        #region Fields
        /// <summary>
        /// MetaConnection manager
        /// </summary>
        private MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
        #endregion

        #region Methods
        /// <summary>
        /// Repair a concept's reciprocal connections, (verbs metaConnected with inverse_of or permutable_side)
        /// (The only connections affected will be the optimized connections)
        /// </summary>
        /// <param name="concept">Concept to repair</param>
        public void Reciprocate(Concept subject)
        {
            FeelingMonitor.Add(FeelingMonitor.RECIPROCATING);
            Repair(subject, new VerbMetaConnectionCache());
        }

        /// <summary>
        /// Repair a collection of concepts's reciprocal connections
        /// </summary>
        /// <param name="conceptCollection">collection of concepts</param>
        public void ReciprocateRange(IEnumerable<Concept> conceptCollection)
        {
            FeelingMonitor.Add(FeelingMonitor.RECIPROCATING);
            VerbMetaConnectionCache verbMetaConnectionCache = new VerbMetaConnectionCache();
            foreach (Concept subject in conceptCollection)
                Repair(subject, verbMetaConnectionCache);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Repair a concept
        /// </summary>
        /// <param name="subject">subject concept to repair</param>
        /// <param name="verbMetaConnectionCache">verb metaConnection cache</param>
        private void Repair(Concept subject, VerbMetaConnectionCache verbMetaConnectionCache)
        {
            if (subject.IsFlatDirty || subject.IsOptimizedDirty)
                throw new ReciprocatorException("Repair concept first");

            foreach (KeyValuePair<Concept, ConnectionBranch> branchAndVerb in subject.FlatConnectionBranchList)
            {
                Concept verb = branchAndVerb.Key;
                ConnectionBranch branch = branchAndVerb.Value;

                #region We try to get the verb list from verbMetaConnectionCache
                HashSet<Concept> inverseOfVerbList = verbMetaConnectionCache.GetVerbFlatListFromCache(verb, "inverse_of", true);
                if (inverseOfVerbList == null)
                {
                    inverseOfVerbList = metaConnectionManager.GetVerbFlatListFromMetaConnection(verb, "inverse_of", true);
                    verbMetaConnectionCache.Remember(verb, "inverse_of", true, inverseOfVerbList);
                }
                
                HashSet<Concept> permutableSideVerbList = verbMetaConnectionCache.GetVerbFlatListFromCache(verb, "permutable_side", true);
                if (permutableSideVerbList == null)
                {
                    permutableSideVerbList = metaConnectionManager.GetVerbFlatListFromMetaConnection(verb, "permutable_side", true);
                    verbMetaConnectionCache.Remember(verb, "permutable_side", true, permutableSideVerbList);
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

            subject.IsFlatDirty = true;
        }
        #endregion
    }
}
