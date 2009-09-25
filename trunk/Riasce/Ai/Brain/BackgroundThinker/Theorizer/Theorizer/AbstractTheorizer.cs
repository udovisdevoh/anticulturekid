using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractTheorizer
    {
        /// <summary>
        /// Returns the best connection theory or metaConnection theory about concept or operator
        /// </summary>
        /// <param name="conceptOrOperator">concept or operator</param>
        /// <returns>the best connection theory or metaConnection theory about concept or operator</returns>
        public abstract Theory GetBestTheoryAboutConcept(Concept conceptOrOperator);

        /// <summary>
        /// Returns the best metaConnection or connection theory from concept collection
        /// </summary>
        /// <param name="conceptCollection">concept collection</param>
        /// <returns>the best metaConnection or connection theory from concept collection</returns>
        public abstract Theory GetBestTheoryFromConceptEnumeration(IEnumerable<Concept> conceptCollection);

        public abstract List<Theory> GetRandomConnectionTheoryListAbout(Concept conceptToThinkAbout);

        public abstract List<Theory> GetRandomLinguisticTheoryListAbout(Memory memory, NameMapper nameMapper, Concept conceptToThinkAbout);

        public abstract Theory GetRandomMetaConnectionTheoryAbout(Concept operatorToThinkAbout);
    }
}
