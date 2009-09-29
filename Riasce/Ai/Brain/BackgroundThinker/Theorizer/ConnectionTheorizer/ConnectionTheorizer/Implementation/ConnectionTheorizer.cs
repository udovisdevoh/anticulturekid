using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class ConnectionTheorizer : AbstractConnectionTheorizer
    {
        #region Fields
        private static readonly int samplingSize = 20;

        private RejectedTheories rejectedTheories;

        private TheoryStatistics theoryStatistics = new TheoryStatistics();

        private static Random random = new Random();
        #endregion

        #region Constructor
        public ConnectionTheorizer(RejectedTheories rejectedTheories)
        {
            this.rejectedTheories = rejectedTheories;
        }
        #endregion

        #region Methods
        public override Theory GetBestTheoryFromConceptCollection(IEnumerable<Concept> conceptCollection, bool useFlatBrotherHood)
        {
            HashSet<Concept> conceptSample = conceptCollection.GetRandomSample(samplingSize);

            if (conceptSample.Count < 1)
                return null;

            Theory bestTheory = null;
            Theory currentTheory;
            foreach (Concept currentConcept in conceptSample)
            {
                currentTheory = GetBestTheoryAboutConcept(currentConcept,useFlatBrotherHood);
                if (bestTheory == null || (currentTheory != null && currentTheory > bestTheory))
                    bestTheory = currentTheory;
            }
            return bestTheory;
        }

        public override Theory GetBestTheoryAboutConcept(Concept subject, bool useFlatBrotherHood)
        {
            BrotherHoodSet brotherHoodSet;

            if (subject.IsFlatDirty)
                throw new TheoryException("Repair concept first");

            if (useFlatBrotherHood)
                brotherHoodSet = BrotherHoodManager.GetFlatBrotherHoodSet(subject);
            else
                brotherHoodSet = BrotherHoodManager.GetOptimizedBrotherHoodSet(subject);

            TheoryStatistics theoryStatistics = GetTheoryStatistics(subject,brotherHoodSet);
            List<Theory> theoryList = GetTheoryList(theoryStatistics, brotherHoodSet.CountBrothers);

            Theory bestTheory = null;

            foreach (Theory currentTheory in theoryList)
            {
                if ((currentTheory != null) && (bestTheory == null || currentTheory > bestTheory))
                    bestTheory = currentTheory;
            }

            return bestTheory;
        }

        private List<Theory> GetTheoryList(TheoryStatistics theoryStatistics, int brotherCount)
        {
            List<Theory> theoryList = new List<Theory>();
            if (brotherCount > 0)
            {
                Theory theory;
                double predictedProbability;
                foreach (TheoryInfo theoryInfo in theoryStatistics)
                {
                    if (theoryInfo.CountArgument >= 4)
                    {
                        predictedProbability = (double)(theoryInfo.CountArgument) / 3.0 / (double)(brotherCount + 1);
                        theory = new Theory(predictedProbability, theoryInfo);
                        if (!rejectedTheories.Contains(theory))
                        {
                            if (theory.GetConcept(0) != theory.GetConcept(2))
                            {
                                if (ConnectionManager.FindObstructionToPlug(theory.GetConcept(0), theory.GetConcept(1), theory.GetConcept(2), true) == null)
                                {
                                    if (IsTheoryConservativeFriendly(theory))
                                    {
                                        theoryList.Add(theory);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return theoryList;
        }

        private bool IsTheoryConservativeFriendly(Theory theory)
        {
            if (theory.MetaOperatorName != null)
                return true;

            Concept subject, verb, complement;

            subject = theory.GetConcept(0);
            verb = theory.GetConcept(1);
            complement = theory.GetConcept(2);

            if (verb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("conservative_thinking").Contains(verb))
                if (subject.GetFlatConnectionBranch(verb).ComplementConceptList.Count < 1)
                    return false;

            return true;
        }

        private TheoryStatistics GetTheoryStatistics(Concept subject, BrotherHoodSet brotherHoodSet)
        {
            TheoryStatistics theoryStatistics = new TheoryStatistics();
            TheoryInfo theoryInfo;
            Concept verb;
            ConnectionBranch flatBranch;
            foreach (BrotherHood brotherHood in brotherHoodSet)
            {
                foreach (Concept brother in brotherHood)
                {
                    foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in brother.FlatConnectionBranchList)
                    {
                        verb = verbAndBranch.Key;
                        flatBranch = verbAndBranch.Value;

                        //if (subject.GetFlatConnectionBranch(verb).ComplementConceptList.Count < 1) //If subject has no connection of this type
                        //    continue;

                        foreach (Concept complement in flatBranch.ComplementConceptList)
                        {
                            if (!subject.GetFlatConnectionBranch(verb).ComplementConceptList.Contains(complement))
                            {
                                theoryInfo = theoryStatistics.GetOrCreateTheoryInfo(subject, verb, complement);

                                theoryInfo.AddArgument(subject, brotherHood.Verb, brotherHood.Complement);
                                theoryInfo.AddArgument(brother, brotherHood.Verb, brotherHood.Complement);
                                theoryInfo.AddArgument(brother, verb, complement);
                            }
                        }
                    }
                }
            }
            return theoryStatistics;
        }

        /// <summary>
        /// Returns a collection of theories that are contraposate to provided theory
        /// </summary>
        /// <param name="theory">theory</param>
        /// <returns>a collection of theories that are contraposate to provided theory</returns>
        private HashSet<Theory> GetContraposateListToTheory(Theory theory)
        {
            HashSet<Theory> contraposateList = new HashSet<Theory>();

            HashSet<Concept> listVerbInverseOf = MetaConnectionManager.GetVerbFlatListFromMetaConnection(theory.GetConcept(1), "inverse_of", true);

            foreach (Concept inverseVerb in listVerbInverseOf)
            {
                Theory contraposate = new Theory(theory.PredictedProbability, theory.GetConcept(2), inverseVerb, theory.GetConcept(0));
                contraposateList.Add(contraposate);
            }

            return contraposateList;
        }

        public override List<Theory> GetRandomTheoryListAbout(Concept subject)
        {
            if (subject.IsFlatDirty)
                throw new TheoryException("Repair concept first");


            BrotherHoodSet brotherHoodSet;
            if (random.Next(2) == 0)
                brotherHoodSet = BrotherHoodManager.GetRandomFlatBrotherHood(subject);
            else
                brotherHoodSet = BrotherHoodManager.GetRandomOptimizedBrotherHood(subject);

            if (brotherHoodSet == null)
                return null;

            TheoryStatistics theoryStatistics = GetTheoryStatistics(subject, brotherHoodSet);

            //Theory bestTheory = null;
            List<Theory> theoryList = GetTheoryList(theoryStatistics, brotherHoodSet.CountBrothers);

            /*
            foreach (Theory currentTheory in theoryList)
            {
                if ((currentTheory != null) && (bestTheory == null || currentTheory > bestTheory))
                    bestTheory = currentTheory;
            }
            */

            return theoryList;
        }
        #endregion
    }
}
