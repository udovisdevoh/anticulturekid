using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class MctFromMetaConnection : AbstractMctFromMetaConnection
    {
        #region Fields
        private static readonly int samplingSize = 20;

        private RejectedTheories rejectedTheories;

        private Random random = new Random();
        #endregion

        #region Constructor
        public MctFromMetaConnection(RejectedTheories rejectedTheories)
        {
            this.rejectedTheories = rejectedTheories;
        }
        #endregion

        #region Methods
        public override Theory GetBestTheoryAboutOperator(Concept verb)
        {
            List<Theory> listTheoryAboutVerb = GetAllTheoriesAboutOperatorFromMetaConnections(verb);

            Theory bestTheory = null;
            foreach (Theory currentTheory in listTheoryAboutVerb)
                if (bestTheory == null || currentTheory > bestTheory)
                    bestTheory = currentTheory;
            return bestTheory;
        }

        public override Theory GetBestTheoryAboutOperatorsInMemory()
        {
            HashSet<Concept> conceptSample = Memory.TotalVerbList.GetRandomSample(samplingSize);

            if (conceptSample.Count < 1)
                return null;

            Theory bestTheory = null;
            Theory currentTheory;
            foreach (Concept currentConcept in conceptSample)
            {
                currentTheory = GetBestTheoryAboutOperator(currentConcept);
                if (bestTheory == null || (currentTheory != null && currentTheory > bestTheory))
                    bestTheory = currentTheory;
            }
            return bestTheory;
        }

        private List<Theory> GetAllTheoriesAboutOperatorFromMetaConnections(Concept verb)
        {
            List<Theory> theoryList = new List<Theory>();

            theoryList.AddRange(GetTheoriesAbout(verb, "muct"));
            theoryList.AddRange(GetTheoriesAbout(verb, "liffid"));
            theoryList.AddRange(GetTheoriesAbout(verb, "sublar"));
            theoryList.AddRange(GetTheoriesAbout(verb, "consics"));
            theoryList.AddRange(GetTheoriesAbout(verb, "cant"));
            theoryList.AddRange(GetTheoriesAbout(verb, "unlikely"));

            return theoryList;
        }

        private List<Theory> GetTheoriesAbout(Concept verb, string metaOperatorName)
        {
            //in order to find muct, sublar, liffid and consics theories
            //for instance
            //madeof muct iso
            //madeof muct isi
            //isa direct_implication iso
            //isa direct_implication isi
            //isa direct_implication ise
            //maybe madeof muct isa

            List<Theory> theoryList = new List<Theory>();
            Theory currentTheory;
            List<MetaConnectionArgument> argumentList;

            foreach (Concept postulateVerb in Memory.TotalVerbList)
            {
                double predictedProbability;
                int totalMatchingPostulate = 0;
                HashSet<Concept> directlyImpliedVerbFromPostulate = MetaConnectionManager.GetVerbFlatListFromMetaConnection(postulateVerb, "direct_implication", true);
                HashSet<Concept> farVerbList = MetaConnectionManager.GetVerbFlatListFromMetaConnection(verb, metaOperatorName, true);
                argumentList = new List<MetaConnectionArgument>();
                foreach (Concept farVerb in farVerbList)
                {
                    if (directlyImpliedVerbFromPostulate.Contains(farVerb))
                    {
                        totalMatchingPostulate++;
                        argumentList.Add(new MetaConnectionArgument(postulateVerb, "direct_implication", farVerb));
                        argumentList.Add(new MetaConnectionArgument(verb, metaOperatorName, farVerb));
                    }
                }

                if (directlyImpliedVerbFromPostulate.Count > 1 && totalMatchingPostulate > 1 && !MetaConnectionManager.GetVerbFlatListFromMetaConnection(verb, metaOperatorName, true).Contains(postulateVerb))
                {
                    predictedProbability = (double)(totalMatchingPostulate) / (double)(directlyImpliedVerbFromPostulate.Count);
                    currentTheory = new Theory(predictedProbability, verb, metaOperatorName, postulateVerb);
                    if (!rejectedTheories.Contains(currentTheory))
                    {
                        currentTheory.AddMetaConnectionArgumentRange(argumentList);
                        theoryList.Add(currentTheory);
                    }
                }
            }

            return theoryList;
        }

        public Theory GetRandomTheoryAbout(Concept verb, string metaOperatorName)
        {
            Theory currentTheory = null;
            List<MetaConnectionArgument> argumentList;
            Concept postulateVerb = Memory.TotalVerbList.GetRandomItem();

            double predictedProbability;
            int totalMatchingPostulate = 0;
            HashSet<Concept> directlyImpliedVerbFromPostulate = MetaConnectionManager.GetVerbFlatListFromMetaConnection(postulateVerb, "direct_implication", true);
            HashSet<Concept> farVerbList = MetaConnectionManager.GetVerbFlatListFromMetaConnection(verb, metaOperatorName, true);
            argumentList = new List<MetaConnectionArgument>();
            foreach (Concept farVerb in farVerbList)
            {
                if (directlyImpliedVerbFromPostulate.Contains(farVerb))
                {
                    totalMatchingPostulate++;
                    argumentList.Add(new MetaConnectionArgument(postulateVerb, "direct_implication", farVerb));
                    argumentList.Add(new MetaConnectionArgument(verb, metaOperatorName, farVerb));
                }
            }

            if (directlyImpliedVerbFromPostulate.Count > 1 && totalMatchingPostulate > 1 && !MetaConnectionManager.GetVerbFlatListFromMetaConnection(verb, metaOperatorName, true).Contains(postulateVerb))
            {
                predictedProbability = (double)(totalMatchingPostulate) / (double)(directlyImpliedVerbFromPostulate.Count);
                currentTheory = new Theory(predictedProbability, verb, metaOperatorName, postulateVerb);
                if (!rejectedTheories.Contains(currentTheory))
                {
                    currentTheory.AddMetaConnectionArgumentRange(argumentList);
                }
            }

            return currentTheory;
        }
        #endregion
    }
}
