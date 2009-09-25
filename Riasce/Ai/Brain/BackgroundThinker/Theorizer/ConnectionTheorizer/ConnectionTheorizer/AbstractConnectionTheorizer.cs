using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractConnectionTheorizer
    {
        /// <summary>
        /// Gets the most probable theory about concept
        /// </summary>
        /// <param name="concept">concept</param>
        /// <param name="useFlatBrotherHood">if true, use flat brotherHood else, use optimizedBrotherhood</param>
        /// <returns>most probable theory about concept, null if nothing found</returns>
        public abstract Theory GetBestTheoryAboutConcept(Concept concept, bool useFlatBrotherHood);

        /// <summary>
        /// Gets the most probable theory from concept collection
        /// </summary>
        /// <param name="conceptCollection">concept collection</param>
        /// /// <param name="useFlatBrotherHood">if true, use flat brotherHood else, use optimizedBrotherhood</param>
        /// <returns>most probable theory from concept collection, null if nothing found</returns>
        public abstract Theory GetBestTheoryFromConceptCollection(IEnumerable<Concept> conceptCollection, bool useFlatBrotherHood);

        public abstract List<Theory> GetRandomTheoryListAbout(Concept conceptToThinkAbout);
    }
}
