using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents the abstraction of connection flattenizers
    /// </summary>
    abstract class AbstractFlattenizer
    {
        /// <summary>
        /// Repair a concept's flat representation and regenerate its optimized representation
        /// so no useless connection persist
        /// </summary>
        /// <param name="concept">Concept to repair</param>
        public abstract void Repair(Concept concept);

        /// <summary>
        /// Repair a concept's flat representation and regenerate its optimized representation
        /// so no useless connection persist.
        /// THIS METHOD MUST ONLY BE USED BY REPAIRER CLASS!!!
        /// </summary>
        /// <param name="concept">concept to repair</param>
        /// <param name="repairedBranches">provided HashSet to rememebr which branches were repaired</param>
        /// <param name="verbConnectionCache">provided cache to remember flattenized metaConnections</param>
        public abstract void Repair(Concept concept, HashSet<ConnectionBranch> repairedBranches, VerbMetaConnectionCache verbConnectionCache);
    }
}
