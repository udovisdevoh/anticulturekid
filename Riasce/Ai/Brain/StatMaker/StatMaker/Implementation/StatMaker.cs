using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class StatMaker : AbstractStatMaker
    {
        #region Fields
        private MetaConnectionManager metaConnectionManager;

        private Random random = new Random();

        private static readonly int samplingSize = 20;
        #endregion

        #region Constructor
        public StatMaker(MetaConnectionManager metaConnectionManager)
        {
            this.metaConnectionManager = metaConnectionManager;
        }
        #endregion

        #region Methods
        public override Stat GetStatOn(Concept denominatorVerb, Concept denominatorComplement, Concept numeratorVerb, Concept numeratorComplement)
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

        public override Stat GetStatOn(Concept denominatorVerb, Concept denominatorComplement)
        {
            if (denominatorComplement.IsFlatDirty || denominatorComplement.IsFlatDirty)
                throw new StatisticException("Repair concepts first");

            HashSet<Concept> denominatorSubjectList = GetFlatConceptListHavingConnection(denominatorVerb, denominatorComplement);
            Concept numeratorVerb = GetRandomVerbFromSubjectList(denominatorSubjectList, denominatorVerb);
            Concept numeratorComplement = GetRandomComplement(denominatorSubjectList, numeratorVerb);

            return GetStatOn(denominatorVerb, denominatorComplement, numeratorVerb, numeratorComplement);
        }

        public override Stat GetRandomStat(IEnumerable<Concept> conceptCollection)
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

            HashSet<Concept> allowedDenominatorVerbHash = metaConnectionManager.GetVerbFlatListFromMetaConnection(denominatorVerbInverse, "inverse_of", true);
            allowedDenominatorVerbHash.UnionWith(metaConnectionManager.GetVerbFlatListFromMetaConnection(denominatorVerbInverse, "permutable_side", true));

            if (allowedDenominatorVerbHash.Count < 1)
                throw new StatisticException("Couldn't find anything to build statistics on");

            List<Concept> allowedDenominatorVerbList = new List<Concept>(allowedDenominatorVerbHash);
            Concept denominatorVerb = allowedDenominatorVerbList[random.Next(0, allowedDenominatorVerbHash.Count)];

            return GetStatOn(denominatorVerb, denominatorComplement);
        }

        private HashSet<Concept> GetFlatConceptListHavingConnection(Concept denominatorVerb, Concept denominatorComplement)
        {
            HashSet<Concept> conceptListHavingConnection = new HashSet<Concept>();

            HashSet<Concept> permutableSideOrInverseOfVerbList = metaConnectionManager.GetVerbFlatListFromMetaConnection(denominatorVerb, "inverse_of", true);
            permutableSideOrInverseOfVerbList.UnionWith(metaConnectionManager.GetVerbFlatListFromMetaConnection(denominatorVerb, "permutable_side", true));

            foreach (Concept inverseVerb in permutableSideOrInverseOfVerbList)
            {
                conceptListHavingConnection.UnionWith(denominatorComplement.GetFlatConnectionBranch(inverseVerb).ComplementConceptList);
            }

            return conceptListHavingConnection;
        }

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
