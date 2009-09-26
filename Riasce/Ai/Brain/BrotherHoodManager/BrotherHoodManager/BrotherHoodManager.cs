﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public class BrotherHoodManager
    {
        #region Fields
        private VerbMetaConnectionCache verbMetaConnectionCache;

        private MetaConnectionManager metaConnectionManager;
        #endregion

        #region Constructor
        public BrotherHoodManager(MetaConnectionManager metaConnectionManager)
        {
            this.metaConnectionManager = metaConnectionManager;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a set of brotherhoods for subject concept
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>a set of brotherhoods for subject concept</returns>
        public BrotherHoodSet GetFlatBrotherHoodSet(Concept subject)
        {
            verbMetaConnectionCache = new VerbMetaConnectionCache();

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

        public BrotherHoodSet GetRandomFlatBrotherHood(Concept subject)
        {
            verbMetaConnectionCache = new VerbMetaConnectionCache();

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
        public BrotherHoodSet GetOptimizedBrotherHoodSet(Concept subject)
        {
            verbMetaConnectionCache = new VerbMetaConnectionCache();

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

        public BrotherHoodSet GetRandomOptimizedBrotherHood(Concept subject)
        {
            verbMetaConnectionCache = new VerbMetaConnectionCache();

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
        public override Dictionary<Concept, double> GetBrotherAndStrengthList(Concept subject)
        {
            verbMetaConnectionCache = new VerbMetaConnectionCache();

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
        public override double GetFraternityStrength(Concept subject, Concept brother)
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
        private HashSet<Concept> GetInverseOrPermutableVerb(Concept verb)
        {
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

            HashSet<Concept> inverseOrPermutableVerb = new HashSet<Concept>();
            inverseOrPermutableVerb.UnionWith(inverseOfVerbList);
            inverseOrPermutableVerb.UnionWith(permutableSideVerbList);
            return inverseOrPermutableVerb;
        }

        private HashSet<Concept> GetBrotherList(Concept subject)
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
        public override HashSet<Concept> GetOptimizedParentConceptList(Concept subject)
        {
            HashSet<Concept> parentConceptList = new HashSet<Concept>();

            foreach (ConnectionBranch flatBranch in subject.OptimizedConnectionBranchList.Values)                
                parentConceptList.UnionWith(flatBranch.ComplementConceptList);

            return parentConceptList;
        }
        #endregion
    }
}