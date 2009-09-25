using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractDefinitionMaker
    {
        /// <summary>
        /// Returns a short definition from concept's optimized representation
        /// Must also return metaOperators if concept is an operator
        /// </summary>
        /// <param name="concept">concept</param>
        /// <returns>a short definition</returns>
        public abstract Dictionary<Concept, List<Concept>> GetShortDefinition(Concept concept);

        /// <summary>
        /// Returns a long definition from concept's flat representation
        /// Must also return metaOperators if concept is an operator
        /// </summary>
        /// <param name="concept">concept</param>
        /// <returns>a long definition</returns>
        public abstract Dictionary<Concept, List<Concept>> GetLongDefinition(Concept concept);
    }
}
