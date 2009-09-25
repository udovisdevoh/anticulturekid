using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractReciprocator
    {
        /// <summary>
        /// Repair a concept's reciprocal connections, (verbs metaConnected with inverse_of or permutable_side)
        /// (The only connections affected will be the optimized connections)
        /// </summary>
        /// <param name="concept">Concept to repair</param>
        public abstract void Reciprocate(Concept concept);

        /// <summary>
        /// Repair a collection of concepts's reciprocal connections
        /// </summary>
        /// <param name="conceptCollection">collection of concepts</param>
        public abstract void ReciprocateRange(IEnumerable<Concept> conceptCollection);
    }
}
