using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used
    /// </summary>
    class Asker
    {
        #region Fields
        private Random random = new Random();

        /// <summary>
        /// Maximum amount of questions created when picking the best one
        /// </summary>
        private static readonly int samplingSize = 7;

        private MetaConnectionManager metaConnectionManager;
        #endregion

        #region Constructor
        public Asker(MetaConnectionManager metaConnectionManager)
        {
            this.metaConnectionManager = metaConnectionManager;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the best question to ask about subject concept
        /// </summary>
        /// <param name="concept">subject concept</param>
        /// <returns>best question to ask about subject concept</returns>
        public List<Concept> GetBestQuestionAbout(Concept subject)
        {
            Concept bestVerb;

            //bestVerb = GetLeastDocumentedVerb(subject);
            bestVerb = GetRandomVerb(subject);

            if (bestVerb == null || random.Next(0,6) == 1)
                bestVerb = Memory.TotalVerbList.GetRandomItem();

            if (bestVerb == null)
                throw new AskingException("Cannot ask anything about that");

            return new List<Concept>() { subject, bestVerb };
        }

        /// <summary>
        /// Gets the best question to ask about concepts in concept collection
        /// </summary>
        /// <param name="conceptEnumeration">concept collection</param>
        /// <returns>>best question to ask about concepts in concept collection</returns>
        public List<Concept> GetBestQuestionAboutRandomConcept(IEnumerable<Concept> conceptCollection)
        {
            HashSet<Concept> conceptSample = conceptCollection.GetRandomSample(samplingSize);

            if (conceptSample.Count < 1)
            {
                FeelingMonitor.Add(FeelingMonitor.EMPTINESS);
                throw new TeachingException("Couldn't find any concept to ask about");
            }

            //Concept concept = GetConceptWithLeastOptimizedConnection(conceptSample);
            Concept concept = GetConceptWithMostAsymmetricOptimizedConnection(conceptSample);

            if (concept == null)
            {
                FeelingMonitor.Add(FeelingMonitor.EMPTINESS);
                throw new TeachingException("Couldn't find any concept to ask about");
            }

            return GetBestQuestionAbout(concept);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Pick the verb that is the least connected to complements
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>least documented verb</returns>
        private Concept GetLeastDocumentedVerb(Concept subject)
        {
            int minimumComplementCount = -1;
            Concept verb;
            Concept bestVerb = null;
            ConnectionBranch flatBranch;
            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in subject.FlatConnectionBranchList)
            {
                verb = verbAndBranch.Key;
                flatBranch = verbAndBranch.Value;
                if (flatBranch.ComplementConceptList.Count < minimumComplementCount || minimumComplementCount == -1)
                {
                    //if (flatBranch.ComplementConceptList.Count > 0)
                    //{
                        minimumComplementCount = flatBranch.ComplementConceptList.Count;
                        bestVerb = verb;
                    //}
                }
            }
            return bestVerb;
        }

        /// <summary>
        /// Pick a random verb connected to subject
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>random verb</returns>
        private Concept GetRandomVerb(Concept subject)
        {
            List<Concept> verbList = new List<Concept>(Memory.TotalVerbList);
            return verbList[random.Next(verbList.Count)];
        }

        /// <summary>
        /// Whether it is possible to ask questions about concept or not
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>Whether it is possible to ask questions about concept or not </returns>
        private bool IsConceptAskable(Concept subject)
        {
            ConnectionBranch flatBranch;
            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in subject.FlatConnectionBranchList)
            {
                flatBranch = verbAndBranch.Value;
                if (flatBranch.ComplementConceptList.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// From concept collection, return the concept with the least amount of optimized connections
        /// </summary>
        /// <param name="conceptCollection">concept collection</param>
        /// <returns>the concept with the least amount of optimized connections</returns>
        private Concept GetConceptWithLeastOptimizedConnection(IEnumerable<Concept> conceptCollection)
        {
            int minimumOptimizedConnectionCount = -1;
            int currentOptimizedConnectionCount;
            Concept conceptWithLeastOptimizedConnection = null;

            foreach (Concept currentConcept in conceptCollection)
            {
                currentOptimizedConnectionCount = CountOptimizedConnection(currentConcept);
                if (minimumOptimizedConnectionCount == -1 || currentOptimizedConnectionCount < minimumOptimizedConnectionCount)
                {
                    minimumOptimizedConnectionCount = currentOptimizedConnectionCount;
                    conceptWithLeastOptimizedConnection = currentConcept;
                }
            }

            return conceptWithLeastOptimizedConnection;
        }

        /// <summary>
        /// From concept collection, return the concept with the largest level of asymetry
        /// </summary>
        /// <param name="conceptCollection">concept collection</param>
        /// <returns>the concept with the largest level of asymetry</returns>
        private Concept GetConceptWithMostAsymmetricOptimizedConnection(HashSet<Concept> conceptCollection)
        {
            double maximumAsymetry = -1;
            double currentAsymerty;
            Concept mostAsymetricConcept = null;

            foreach (Concept currentConcept in conceptCollection)
            {
                currentAsymerty = GetConceptAsymetry(currentConcept);
                if (maximumAsymetry == -1 || currentAsymerty > maximumAsymetry)
                {
                    maximumAsymetry = currentAsymerty;
                    mostAsymetricConcept = currentConcept;
                }
            }

            return mostAsymetricConcept;
        }

        /// <summary>
        /// Return how many optimized collections are in concept
        /// </summary>
        /// <param name="concept">concept</param>
        /// <returns>how many optimized collections are in concept</returns>
        private int CountOptimizedConnection(Concept concept)
        {
            int optimizedConnectionCount = 0;
            foreach (ConnectionBranch branch in concept.OptimizedConnectionBranchList.Values)
            {
                optimizedConnectionCount += branch.ComplementConceptList.Count;
            }

            return optimizedConnectionCount;
        }

        /// <summary>
        /// Return the level of asymetri of provided concept (from 0 to 1)
        /// </summary>
        /// <param name="concept">concept</param>
        /// <returns>concept's level of asymetry (from 0 to 1)</returns>
        private double GetConceptAsymetry(Concept concept)
        {
            double asymetry = 0;

            Concept verb;
            ConnectionBranch optimizedBranch;
            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in concept.OptimizedConnectionBranchList)
            {
                verb = verbAndBranch.Key;
                optimizedBranch = verbAndBranch.Value;
                asymetry += GetBranchAsymetry(concept, verb);
            }

            asymetry /= concept.OptimizedConnectionBranchList.Count;

            return asymetry;
        }

        /// <summary>
        /// Return a connection branch's asymetry (from 0 to 1)
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <returns>a connection branch's asymetry (from 0 to 1)</returns>
        private double GetBranchAsymetry(Concept subject, Concept verb)
        {
            double asymetry = 0;

            int connectionCount = subject.GetOptimizedConnectionBranch(verb).ComplementConceptList.Count;
            int permutableSideOrInverseOfConnectionCount = 0;

            foreach (Concept inverseOfVerb in metaConnectionManager.GetVerbFlatListFromMetaConnection(verb, "inverse_of", true))
                permutableSideOrInverseOfConnectionCount += subject.GetOptimizedConnectionBranch(inverseOfVerb).ComplementConceptList.Count;
            
            foreach (Concept permutableSideVerb in metaConnectionManager.GetVerbFlatListFromMetaConnection(verb, "permutable_side", true))
                permutableSideOrInverseOfConnectionCount += subject.GetOptimizedConnectionBranch(permutableSideVerb).ComplementConceptList.Count;

            if (connectionCount > permutableSideOrInverseOfConnectionCount)
            {
                if (connectionCount == 0)
                    asymetry = 1;
                else
                    asymetry = 1 - ((double)(permutableSideOrInverseOfConnectionCount) / (double)(connectionCount));
            }
            else if (connectionCount < permutableSideOrInverseOfConnectionCount)
            {
                if (permutableSideOrInverseOfConnectionCount == 0)
                    asymetry = 1;
                else
                    asymetry = 1 - ((double)(connectionCount) / (double)(permutableSideOrInverseOfConnectionCount));
            }
            else
            {
                asymetry = 0;
            }

            return asymetry;
        }
        #endregion
    }
}
