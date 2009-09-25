using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class DefinitionMaker : AbstractDefinitionMaker
    {
        public override Dictionary<Concept, List<Concept>> GetShortDefinition(Concept concept)
        {
            if (concept.IsFlatDirty || concept.IsOptimizedDirty)
                throw new DefinitionException("Repair concept first");

            Dictionary<Concept, List<Concept>> definition = new Dictionary<Concept, List<Concept>>();

            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in concept.OptimizedConnectionBranchList)
                if (verbAndBranch.Value.ComplementConceptList.Count > 0)
                    definition.Add(verbAndBranch.Key, new List<Concept>(verbAndBranch.Value.ComplementConceptList));

            return definition;
        }

        public override Dictionary<Concept, List<Concept>> GetLongDefinition(Concept concept)
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
