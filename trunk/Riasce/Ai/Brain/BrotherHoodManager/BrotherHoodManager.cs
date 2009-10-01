using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used to find brother concepts to concepts
    /// </summary>
    public static class BrotherHoodManager
    {
        #region Public Methods
        /// <summary>
        /// Returns a set of brotherhoods for subject concept
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>a set of brotherhoods for subject concept</returns>
        public static BrotherHoodSet GetFlatBrotherHoodSet(Concept subject)
        {
            BrotherHoodSet brotherHoodSet = new BrotherHoodSet();

            Concept verb;
            ConnectionBranch flatBranch;
            BrotherHood brotherHood;
            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in subject.FlatConnectionBranchList)
            {
                verb = verbAndBranch.Key;
                flatBranch = verbAndBranch.Value;
                foreach (Concept complement in flatBranch.ComplementConceptList)
                {
                    brotherHood = brotherHoodSet.GetOrCreateBrotherHood(verb, complement);

                    HashSet<Concept> inverseOfOrPermutableSideVerbList = GetInverseOrPermutableVerb(verb);

                    foreach (Concept inverseVerb in inverseOfOrPermutableSideVerbList)
                    {
                        HashSet<Concept> metaComplementList = complement.GetFlatConnectionBranch(inverseVerb).ComplementConceptList;
                        foreach (Concept metaComplement in metaComplementList)
                        {
                            if (metaComplement != subject)
                            {
                                brotherHood.Add(metaComplement);
                            }
                        }
                    }
                }
            }

            return brotherHoodSet;
        }

        /// <summary>
        /// Get a random flat brotherhood to subject concept
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>random flat brotherhood to subject concept</returns>
        public static BrotherHoodSet GetRandomFlatBrotherHood(Concept subject)
        {
            if (subject.FlatConnectionBranchList.Count < 1)
                return null;

            BrotherHoodSet brotherHoodSet = new BrotherHoodSet();

            Concept verb = subject.FlatConnectionBranchList.Keys.GetRandomItem();
            ConnectionBranch flatBranch= subject.GetFlatConnectionBranch(verb);
            Concept complement = flatBranch.ComplementConceptList.GetRandomItem();

            if (complement == null)
                return null;

            BrotherHood brotherHood = brotherHoodSet.GetOrCreateBrotherHood(verb, complement);

            HashSet<Concept> inverseOfOrPermutableSideVerbList = GetInverseOrPermutableVerb(verb);

            foreach (Concept inverseVerb in inverseOfOrPermutableSideVerbList)
            {
                HashSet<Concept> metaComplementList = complement.GetFlatConnectionBranch(inverseVerb).ComplementConceptList;
                foreach (Concept metaComplement in metaComplementList)
                {
                    if (metaComplement != subject)
                    {
                        brotherHood.Add(metaComplement);
                    }
                }
            }

            return brotherHoodSet;
        }

        /// <summary>
        /// Returns a set of brotherhoods for subject concept
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>a set of brotherhoods for subject concept</returns>
        public static BrotherHoodSet GetOptimizedBrotherHoodSet(Concept subject)
        {
            BrotherHoodSet brotherHoodSet = new BrotherHoodSet();

            Concept verb;
            ConnectionBranch optimizedBranch;
            BrotherHood brotherHood;
            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in subject.OptimizedConnectionBranchList)
            {
                verb = verbAndBranch.Key;
                optimizedBranch = verbAndBranch.Value;
                foreach (Concept complement in optimizedBranch.ComplementConceptList)
                {
                    brotherHood = brotherHoodSet.GetOrCreateBrotherHood(verb, complement);

                    HashSet<Concept> inverseOfOrPermutableSideVerbList = GetInverseOrPermutableVerb(verb);

                    foreach (Concept inverseVerb in inverseOfOrPermutableSideVerbList)
                    {
                        HashSet<Concept> metaComplementList = complement.GetOptimizedConnectionBranch(inverseVerb).ComplementConceptList;
                        foreach (Concept metaComplement in metaComplementList)
                        {
                            if (metaComplement != subject)
                            {
                                brotherHood.Add(metaComplement);
                            }
                        }
                    }
                }
            }

            return brotherHoodSet;
        }

        /// <summary>
        /// Get a random optimized brotherhood to subject concept
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>random optimized brotherhood to subject concept</returns>
        public static BrotherHoodSet GetRandomOptimizedBrotherHood(Concept subject)
        {
            if (subject.OptimizedConnectionBranchList.Count < 1)
                return null;

            BrotherHoodSet brotherHoodSet = new BrotherHoodSet();

            Concept verb = subject.OptimizedConnectionBranchList.Keys.GetRandomItem();
            ConnectionBranch optimizedBranch = subject.GetOptimizedConnectionBranch(verb);
            Concept complement = optimizedBranch.ComplementConceptList.GetRandomItem();

            if (complement == null)
                return null;

            BrotherHood brotherHood = brotherHoodSet.GetOrCreateBrotherHood(verb, complement);

            HashSet<Concept> inverseOfOrPermutableSideVerbList = GetInverseOrPermutableVerb(verb);

            foreach (Concept inverseVerb in inverseOfOrPermutableSideVerbList)
            {
                HashSet<Concept> metaComplementList = complement.GetOptimizedConnectionBranch(inverseVerb).ComplementConceptList;
                foreach (Concept metaComplement in metaComplementList)
                {
                    if (metaComplement != subject)
                    {
                        brotherHood.Add(metaComplement);
                    }
                }
            }

            return brotherHoodSet;
        }

        /// <summary>
        /// Returns all brothers of subject and their brotherhood strengths
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>all brothers of subject and their brotherhood strengths</returns>
        public static Dictionary<Concept, double> GetBrotherAndStrengthList(Concept subject)
        {
            HashSet<Concept> brotherList = GetBrotherList(subject);

            Dictionary<Concept, double> brotherAndStrengthList = new Dictionary<Concept, double>();

            foreach (Concept brother in brotherList)
                brotherAndStrengthList.Add(brother, GetFraternityStrength(subject, brother));

            return brotherAndStrengthList;
        }

        /// <summary>
        /// Get the fraternity strength
        /// </summary>
        /// <param name="subject">concept 1</param>
        /// <param name="brother">concept 2</param>
        /// <returns>fraternity strength</returns>
        public static double GetFraternityStrength(Concept subject, Concept brother)
        {
            double fraternityStrength = 0;

            int totalSubjectConnection = 0;
            int totalSubjectConnectionInBrother = 0;
            int totalBrotherConnection = 0;
            int totalBrotherConnectionInSubject = 0;

            Concept verb;
            ConnectionBranch flatBranch;
            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in subject.FlatConnectionBranchList)
            {
                verb = verbAndBranch.Key;
                flatBranch = verbAndBranch.Value;
                HashSet<Concept> complementList = flatBranch.ComplementConceptList;
                totalSubjectConnection += complementList.Count;
                foreach (Concept complement in complementList)
                    if (brother.GetFlatConnectionBranch(verb).ComplementConceptList.Contains(complement))
                        totalSubjectConnectionInBrother++;
            }

            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in brother.FlatConnectionBranchList)
            {
                verb = verbAndBranch.Key;
                flatBranch = verbAndBranch.Value;
                HashSet<Concept> complementList = flatBranch.ComplementConceptList;
                totalBrotherConnection += complementList.Count;
                foreach (Concept complement in complementList)
                    if (subject.GetFlatConnectionBranch(verb).ComplementConceptList.Contains(complement))
                        totalBrotherConnectionInSubject++;
            }

            if (totalSubjectConnection > 0)
                fraternityStrength += (double)(totalSubjectConnectionInBrother) / (double)(totalSubjectConnection);
            if (totalBrotherConnection > 0)
                fraternityStrength += (double)(totalBrotherConnectionInSubject) / (double)(totalBrotherConnection);

            fraternityStrength /= 2;

            return fraternityStrength;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Return list of verbs that are inverse_of or permutable_side to provided verb concept
        /// </summary>
        /// <param name="verb">provided verb concept</param>
        /// <returns>list of verbs that are inverse_of or permutable_side to provided verb concept</returns>
        private static HashSet<Concept> GetInverseOrPermutableVerb(Concept verb)
        {
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

            HashSet<Concept> inverseOrPermutableVerb = new HashSet<Concept>();
            inverseOrPermutableVerb.UnionWith(inverseOfVerbList);
            inverseOrPermutableVerb.UnionWith(permutableSideVerbList);
            return inverseOrPermutableVerb;
        }

        /// <summary>
        /// Get the list of brother concepts to subject concept
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>brother concepts to subject concept</returns>
        private static HashSet<Concept> GetBrotherList(Concept subject)
        {
            HashSet<Concept> brotherList = new HashSet<Concept>();

            Concept verb;
            ConnectionBranch flatBranch;
            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in subject.FlatConnectionBranchList)
            {
                verb = verbAndBranch.Key;
                flatBranch = verbAndBranch.Value;
                foreach (Concept complement in flatBranch.ComplementConceptList)
                {
                    HashSet<Concept> inverseOfOrPermutableSideVerbList = GetInverseOrPermutableVerb(verb);
                    foreach (Concept inverseVerb in inverseOfOrPermutableSideVerbList)
                    {
                        HashSet<Concept> metaComplementList = complement.GetFlatConnectionBranch(inverseVerb).ComplementConceptList;
                        foreach (Concept metaComplement in metaComplementList)
                        {
                            if (metaComplement != subject)
                            {
                                brotherList.Add(metaComplement);
                            }
                        }
                    }
                }
            }
            return brotherList;
        }

        /// <summary>
        /// Get the list of concept plugged to subject
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>list of concept plugged to subject</returns>
        public static HashSet<Concept> GetOptimizedParentConceptList(Concept subject)
        {
            HashSet<Concept> parentConceptList = new HashSet<Concept>();

            foreach (ConnectionBranch flatBranch in subject.OptimizedConnectionBranchList.Values)                
                parentConceptList.UnionWith(flatBranch.ComplementConceptList);

            return parentConceptList;
        }
        #endregion
    }
}
