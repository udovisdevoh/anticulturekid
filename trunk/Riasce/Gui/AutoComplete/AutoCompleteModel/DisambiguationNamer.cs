using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Add paranthesed disambiguation concept name to autocomplete concept names
    /// </summary>
    public class DisambiguationNamer
    {
        #region Fields
        /// <summary>
        /// Brotherhood manager
        /// </summary>
        private BrotherHoodManager brotherHoodManager;

        /// <summary>
        /// Disambiguation concept mapping cache
        /// </summary>
        private Dictionary<Concept, Concept> cache = new Dictionary<Concept, Concept>();

        /// <summary>
        /// Repairer
        /// </summary>
        private Repairer repairer;
        #endregion

        #region Constructor
        public DisambiguationNamer(BrotherHoodManager brotherHoodManager, Repairer repairer)
        {
            this.brotherHoodManager = brotherHoodManager;
            this.repairer = repairer;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the context concept for subject concept
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>context concept</returns>
        public Concept GetContextConcept(Concept subject)
        {
            Concept bestParent = null;

            if (cache.TryGetValue(subject, out bestParent))
                return bestParent;

            HashSet<Concept> parentConceptList = brotherHoodManager.GetOptimizedParentConceptList(subject);

            double maxBrotherHoodStrength = 0;
            double currentBrotherHoodStrength;
            bestParent = null;

            foreach (Concept currentParentConcept in parentConceptList)
            {
                currentBrotherHoodStrength = brotherHoodManager.GetFraternityStrength(subject, currentParentConcept);
                if (maxBrotherHoodStrength == 0 || currentBrotherHoodStrength > maxBrotherHoodStrength)
                {
                    bestParent = currentParentConcept;
                    maxBrotherHoodStrength = currentBrotherHoodStrength;
                }
            }

            if (bestParent != null)
                cache.Add(subject, bestParent);

            return bestParent;
        }
        #endregion
    }
}
