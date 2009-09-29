using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Create statistics on concepts
    /// </summary>
    class StatMaker
    {
        #region Fields
        /// <summary>
        /// Random number generator
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// Maximum sampling size when create random statistics to find the best one
        /// </summary>
        private static readonly int samplingSize = 20;
        #endregion

        #region Public Methods
        /// <summary>
        /// From all concept having connection denominatorVerb denominatorComplement,
        /// returns stat object with ratio (from 0 to 1) of concept having also connection numeratorVerb numeratorComplement
        /// </summary>
        /// <param name="denominatorVerb">denominator verb</param>
        /// <param name="denominatorComplement">denominator complement</param>
        /// <param name="numeratorVerb">numerator verb</param>
        /// <param name="numeratorComplement">numerator complement</param>
        /// <returns>From all concept having connection denominatorVerb denominatorComplement,
        /// returns stat object with ratio (from 0 to 1) of concept having also connection numeratorVerb numeratorComplement</returns>
        public Stat GetStatOn(Concept denominatorVerb, Concept denominatorComplement, Concept numeratorVerb, Concept numeratorComplement)
        {
            double ratio;
            if (denominatorComplement.IsFlatDirty || denominatorComplement.IsFlatDirty || numeratorVerb.IsFlatDirty || numeratorComplement.IsFlatDirty)
                throw new StatisticException("Repair concepts first");

            int numeratorCount = 0;
            HashSet<Concept> denominatorSubjectList = GetFlatConceptListHavingConnection(denominatorVerb, denominatorComplement);
            HashSet<Concept> numeratorSubjectList = GetFlatConceptListHavingConnection(numeratorVerb, numeratorComplement);

            foreach (Concept denominatorSubject in denominatorSubjectList)
                if (numeratorSubjectList.Contains(denominatorSubject))
                    ++numeratorCount;

            if (denominatorSubjectList.Count == 0)
                ratio = -1;
            else
                ratio = (double)(numeratorCount) / (double)(denominatorSubjectList.Count);

            return new Stat(denominatorVerb, denominatorComplement, numeratorVerb, numeratorComplement,ratio);
        }

        /// <summary>
        /// From all concept having connection denominatorVerb denominatorComplement,
        /// returns stat object with ratio (from 0 to 1) of concept having also connection to random numeratorVerb numeratorComplement
        /// </summary>
        /// <param name="denominatorVerb">denominator verb</param>
        /// <param name="denominatorComplement">denominator complement</param>
        /// <returns>From all concept having connection denominatorVerb denominatorComplement,
        /// returns stat object with ratio (from 0 to 1) of concept having also connection to random numeratorVerb numeratorComplement</returns>
        public Stat GetStatOn(Concept denominatorVerb, Concept denominatorComplement)
        {
            if (denominatorComplement.IsFlatDirty || denominatorComplement.IsFlatDirty)
                throw new StatisticException("Repair concepts first");

            HashSet<Concept> denominatorSubjectList = GetFlatConceptListHavingConnection(denominatorVerb, denominatorComplement);
            Concept numeratorVerb = GetRandomVerbFromSubjectList(denominatorSubjectList, denominatorVerb);
            Concept numeratorComplement = GetRandomComplement(denominatorSubjectList, numeratorVerb);

            return GetStatOn(denominatorVerb, denominatorComplement, numeratorVerb, numeratorComplement);
        }

        /// <summary>
        /// Return a statistic on a random concept from concept collection
        /// </summary>
        /// <param name="conceptCollection">concept collection</param>
        /// <returns>a statistic on a random concept from concept collection</returns
        public Stat GetRandomStat(IEnumerable<Concept> conceptCollection)
        {
            HashSet<Concept> sample = conceptCollection.GetRandomSample(samplingSize);
            Stat stat = null;

            foreach (Concept denominatorComplement in sample)
            {
                try
                {
                    stat = GetStatOn(denominatorComplement);
                    break;
                }
                catch (StatisticException)
                {
                    continue;
                }
            }

            if (stat == null)
            {
                throw new StatisticException("Couldn't find anything to build statistics on");
            }
            else
            {
                return stat;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get stat on denominator complement
        /// </summary>
        /// <param name="denominatorComplement">denominator complement concept</param>
        /// <returns>Stat object</returns>
        private Stat GetStatOn(Concept denominatorComplement)
        {
            HashSet<Concept> verbHash = new HashSet<Concept>();

            Concept verb;
            ConnectionBranch flatBranch;
            foreach (KeyValuePair<Concept,ConnectionBranch> verbAndFlatBranch in denominatorComplement.FlatConnectionBranchList)
            {
                verb = verbAndFlatBranch.Key;
                flatBranch = verbAndFlatBranch.Value;
                if (flatBranch.ComplementConceptList.Count > 0)
                {
                    verbHash.Add(verb);
                }
            }

            if (verbHash.Count < 1)
                throw new StatisticException("Couldn't find anything to build statistics on");

            List<Concept> verbList = new List<Concept>(verbHash);
            Concept denominatorVerbInverse = verbList[random.Next(0, verbHash.Count)];

            HashSet<Concept> allowedDenominatorVerbHash = MetaConnectionManager.GetVerbFlatListFromMetaConnection(denominatorVerbInverse, "inverse_of", true);
            allowedDenominatorVerbHash.UnionWith(MetaConnectionManager.GetVerbFlatListFromMetaConnection(denominatorVerbInverse, "permutable_side", true));

            if (allowedDenominatorVerbHash.Count < 1)
                throw new StatisticException("Couldn't find anything to build statistics on");

            List<Concept> allowedDenominatorVerbList = new List<Concept>(allowedDenominatorVerbHash);
            Concept denominatorVerb = allowedDenominatorVerbList[random.Next(0, allowedDenominatorVerbHash.Count)];

            return GetStatOn(denominatorVerb, denominatorComplement);
        }

        /// <summary>
        /// Returns all concepts having provided flat connection
        /// </summary>
        /// <param name="denominatorVerb">denominator verb concept</param>
        /// <param name="denominatorComplement">denominator complement concept</param>
        /// <returns>all concepts having provided flat connection</returns>
        private HashSet<Concept> GetFlatConceptListHavingConnection(Concept denominatorVerb, Concept denominatorComplement)
        {
            HashSet<Concept> conceptListHavingConnection = new HashSet<Concept>();

            HashSet<Concept> permutableSideOrInverseOfVerbList = MetaConnectionManager.GetVerbFlatListFromMetaConnection(denominatorVerb, "inverse_of", true);
            permutableSideOrInverseOfVerbList.UnionWith(MetaConnectionManager.GetVerbFlatListFromMetaConnection(denominatorVerb, "permutable_side", true));

            foreach (Concept inverseVerb in permutableSideOrInverseOfVerbList)
            {
                conceptListHavingConnection.UnionWith(denominatorComplement.GetFlatConnectionBranch(inverseVerb).ComplementConceptList);
            }

            return conceptListHavingConnection;
        }

        /// <summary>
        /// Returns a random verb from subject list
        /// </summary>
        /// <param name="subjectList">subject concept list</param>
        /// <param name="verbNotAllowed">verbs not allowed</param>
        /// <returns>random verb from subject list</returns>
        private Concept GetRandomVerbFromSubjectList(HashSet<Concept> subjectList, Concept verbNotAllowed)
        {
            HashSet<Concept> allowedVerbHash = new HashSet<Concept>();

            Concept verb;
            ConnectionBranch flatBranch;
            foreach (Concept subject in subjectList)
            {
                foreach (KeyValuePair<Concept, ConnectionBranch> verbAndFlatBranch in subject.FlatConnectionBranchList)
                {
                    verb = verbAndFlatBranch.Key;
                    flatBranch = verbAndFlatBranch.Value;

                    if (flatBranch.ComplementConceptList.Count > 0)
                    {
                        allowedVerbHash.Add(verb);
                    }
                }
            }

            allowedVerbHash.Remove(verbNotAllowed);

            List<Concept> allowedVerbList = new List<Concept>(allowedVerbHash);

            if (allowedVerbList.Count < 1)
                throw new StatisticException("Couldn't find anything to build statistics on");

            return allowedVerbList[random.Next(0, allowedVerbList.Count)];
        }

        /// <summary>
        /// Returns a random complement concept to subjects in list using provided verb
        /// </summary>
        /// <param name="subjectList">subject list</param>
        /// <param name="verb">verb concept</param>
        /// <returns>random complement concept to subjects in list using provided verb</returns>
        private Concept GetRandomComplement(HashSet<Concept> subjectList, Concept verb)
        {
            HashSet<Concept> complementHash = new HashSet<Concept>();

            foreach (Concept subject in subjectList)
                complementHash.UnionWith(subject.GetFlatConnectionBranch(verb).ComplementConceptList);

            List<Concept> complementList = new List<Concept>(complementHash);

            if (complementList.Count < 1)
                throw new StatisticException("Couldn't find anything to build statistics on");

            return complementList[random.Next(0, complementList.Count)];
        }
        #endregion
    }
}
