using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractFlatPurifier
    {
        /// <summary>
        /// Remove inconsistant FLAT connections from concept
        /// For instance, cat isa animal, cat contradict animal, isa cant contradict
        /// Ooops! Do we keep cat isa animal or do we keep cat contradict animal?
        /// </summary>
        /// <param name="concept">concept to repair</param>
        /// <returns>Trauma object about removed connections</returns>
        public abstract Trauma Purify(Concept concept);

        /// <summary>
        /// Return the most obstructable connection for provided concept, but look deep in flat connections
        /// </summary>
        /// <param name="subject">provided concept</param>
        /// <param name="flatConnectionSource">source flat connection for which obstruction can be found</param>
        /// <returns>If most obstructable connection has obstruction, return obstructable connection, else: null</returns>
        public abstract List<Concept> GetMostObstructableConnection(Concept subject, out List<Concept> flatConnectionSource);
    }
}
