using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractPurifier
    {
        /// <summary>
        /// Remove inconsistant OPTIMIZED connections from concept
        /// For instance, cat isa animal, cat contradict animal, isa cant contradict
        /// Ooops! Do we keep cat isa animal or do we keep cat contradict animal?
        /// </summary>
        /// <param name="concept">concept to repair</param>
        /// <returns>Trauma object about removed connections</returns>
        public abstract Trauma PurifyOptimized(Concept concept);

        /// <summary>
        /// Remove inconsistant FLAT connections from concept
        /// For instance, cat isa animal, cat contradict animal, isa cant contradict
        /// Ooops! Do we keep cat isa animal or do we keep cat contradict animal?
        /// </summary>
        /// <param name="concept">concept to repair</param>
        /// <returns>Trauma object about removed connections</returns>
        public abstract Trauma PurifyFlat(Concept concept);

        /// <summary>
        /// Remove inconsistant OPTIMIZED connections from concepts in conceptCollection
        /// For instance, cat isa animal, cat contradict animal, isa cant contradict
        /// Ooops! Do we keep cat isa animal or do we keep cat contradict animal?
        /// </summary>
        /// <param name="conceptCollection">concept collection</param>
        /// <returns>Trauma object about removed connections</returns>
        public abstract Trauma PurifyRangeOptimized(IEnumerable<Concept> conceptCollection);
    }
}
