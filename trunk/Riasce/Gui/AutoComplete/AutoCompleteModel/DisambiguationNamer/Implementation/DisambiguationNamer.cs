using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public class DisambiguationNamer : AbstractDisambiguationNamer
    {
        #region Fields
        private BrotherHoodManager brotherHoodManager;

        private Dictionary<Concept, Concept> cache = new Dictionary<Concept, Concept>();

        private Repairer repairer;
        #endregion

        #region Constructor
        public DisambiguationNamer(BrotherHoodManager brotherHoodManager, Repairer repairer)
        {
            this.brotherHoodManager = brotherHoodManager;
            this.repairer = repairer;
        }
        #endregion

        #region Methods
        public override Concept GetContextConcept(Concept subject)
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
