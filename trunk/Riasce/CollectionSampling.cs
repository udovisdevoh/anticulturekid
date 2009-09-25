using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    static class CollectionSampling
    {
        #region Fields
        private static Random random = new Random();
        #endregion

        #region Methods
        public static HashSet<Concept> GetRandomSample(this IEnumerable<Concept> conceptCollection, int samplingSize)
        {
            int randomIndex;
            List<Concept> conceptList = new List<Concept>(conceptCollection);
            HashSet<Concept> conceptSample = new HashSet<Concept>();
            Concept concept;
            while (conceptList.Count > 0 && conceptSample.Count < samplingSize)
            {
                randomIndex = random.Next(0, conceptList.Count);
                concept = conceptList[randomIndex];
                conceptList.RemoveAt(randomIndex);
                if (!conceptSample.Contains(concept))
                    conceptSample.Add(concept);
            }
            return conceptSample;
        }

        public static Concept GetRandomItem(this IEnumerable<Concept> conceptCollection)
        {
            List<Concept> conceptList = new List<Concept>(conceptCollection);
            if (conceptList.Count < 1)
                return null;
            else
                return conceptList[random.Next(0, conceptList.Count)];
        }
        #endregion
    }
}
