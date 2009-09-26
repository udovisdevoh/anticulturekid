using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used to build concept based concept definition
    /// </summary>
    class DefinitionMaker
    {
        /// <summary>
        /// Returns a short definition from concept's optimized representation
        /// Must also return metaOperators if concept is an operator
        /// </summary>
        /// <param name="concept">concept</param>
        /// <returns>a short definition</returns>
        public Dictionary<Concept, List<Concept>> GetShortDefinition(Concept concept)
        {
            if (concept.IsFlatDirty || concept.IsOptimizedDirty)
                throw new DefinitionException("Repair concept first");

            Dictionary<Concept, List<Concept>> definition = new Dictionary<Concept, List<Concept>>();

            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in concept.OptimizedConnectionBranchList)
                if (verbAndBranch.Value.ComplementConceptList.Count > 0)
                    definition.Add(verbAndBranch.Key, new List<Concept>(verbAndBranch.Value.ComplementConceptList));

            return definition;
        }

        /// <summary>
        /// Returns a long definition from concept's flat representation
        /// Must also return metaOperators if concept is an operator
        /// </summary>
        /// <param name="concept">concept</param>
        /// <returns>a long definition</returns>
        public Dictionary<Concept, List<Concept>> GetLongDefinition(Concept concept)
        {
            if (concept.IsFlatDirty || concept.IsOptimizedDirty)
                throw new DefinitionException("Repair concept first");

            Dictionary<Concept, List<Concept>> definition = new Dictionary<Concept, List<Concept>>();

            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in concept.FlatConnectionBranchList)
                if (verbAndBranch.Value.ComplementConceptList.Count > 0)
                    definition.Add(verbAndBranch.Key, new List<Concept>(verbAndBranch.Value.ComplementConceptList));

            return definition;
        }
    }
}
