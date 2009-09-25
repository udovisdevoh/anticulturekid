using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractDefinitionSorter
    {
        /// <summary>
        /// Sort definition set by branch cardinality
        /// </summary>
        /// <param name="definition">definition set</param>
        /// <returns>Sorted definition set</returns>
        public abstract Dictionary<string, List<string>> SortByLooseCardinality(Dictionary<string, List<string>> definition);

        /// <summary>
        /// Sort definition set by verb name
        /// </summary>
        /// <param name="definition">definition set</param>
        /// <returns>Sorted definition set</returns>
        public abstract Dictionary<string, List<string>> SortByVerbName(Dictionary<string, List<string>> definition);

        /// <summary>
        /// Sort definition by predefined hardcoded order based on operator names
        /// </summary>
        /// <param name="definition">definition set</param>
        /// <returns>Sorted definition set</returns>
        public abstract Dictionary<string, List<string>> SortByCustomOrder(Dictionary<string, List<string>> definition);

        /// <summary>
        /// For each connection branch, sort complements by proof length (from shorter to longer)
        /// </summary>
        /// <param name="definition">definition set</param>
        /// <param name="conceptToLookInto">we need the concept so we can know how long the proofs are</param>
        /// <param name="nameMapper">name mapper to look into</param>
        /// <param name="memory">memory to look into</param>
        /// <returns>Sorted definition set</returns>
        public abstract Dictionary<string, List<string>> SortComplementsByProofLength(Dictionary<string, List<string>> definition, Concept concept, NameMapper nameMapper, Memory memory);
    }
}
