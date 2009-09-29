﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used to purify the Ai's memory by removing connections
    /// that make no sense. It is a faceade to FlatPurifier and OptimizedPurifier
    /// </summary>
    class Purifier
    {
        #region Fields
        private OptimizedPurifier optimizedPurifier;

        private FlatPurifier flatPurifier;
        #endregion

        #region Constructor
        public Purifier()
        {
            flatPurifier = new FlatPurifier();
            optimizedPurifier = new OptimizedPurifier();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Remove inconsistant OPTIMIZED connections from concept
        /// For instance, cat isa animal, cat contradict animal, isa cant contradict
        /// Ooops! Do we keep cat isa animal or do we keep cat contradict animal?
        /// </summary>
        /// <param name="concept">concept to repair</param>
        /// <returns>Trauma object about removed connections</returns>
        public Trauma PurifyOptimized(Concept concept)
        {
            return optimizedPurifier.Purify(concept);
        }

        /// <summary>
        /// Remove inconsistant FLAT connections from concept
        /// For instance, cat isa animal, cat contradict animal, isa cant contradict
        /// Ooops! Do we keep cat isa animal or do we keep cat contradict animal?
        /// </summary>
        /// <param name="concept">concept to repair</param>
        /// <returns>Trauma object about removed connections</returns>
        public Trauma PurifyFlat(Concept concept)
        {
            return flatPurifier.Purify(concept);
        }

        /// <summary>
        /// Remove inconsistant OPTIMIZED connections from concepts in conceptCollection
        /// For instance, cat isa animal, cat contradict animal, isa cant contradict
        /// Ooops! Do we keep cat isa animal or do we keep cat contradict animal?
        /// </summary>
        /// <param name="conceptCollection">concept collection</param>
        /// <returns>Trauma object about removed connections</returns>
        public Trauma PurifyRangeOptimized(IEnumerable<Concept> conceptCollection)
        {
            return optimizedPurifier.PurifiyRange(conceptCollection);
        }
        #endregion
    }
}
