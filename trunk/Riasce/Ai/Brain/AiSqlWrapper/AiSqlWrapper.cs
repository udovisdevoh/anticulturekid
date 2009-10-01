using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents a brain component module used to make AiSql queries to brain's memory
    /// </summary>
    static class AiSqlWrapper
    {
        #region Fields and parts
        /// <summary>
        /// Cache of repaired branch to improve flattenizer's performance
        /// </summary>
        private static HashSet<ConnectionBranch> repairedBranches;

        /// <summary>
        /// Name mapper to look into
        /// </summary>
        private static NameMapper nameMapper;

        /// <summary>
        /// Memory to look into
        /// </summary>
        private static Memory currentMemory;
        #endregion

        #region Public methods
        /// <summary>
        /// Returns a selection of concept from query (in a SQLish like way)
        /// </summary>
        /// <param name="conditions">a set of logic conditions</param>
        /// <param name="nameMapper">name mapper to look into</param>
        /// <param name="currentMemory">memory to look into</param>
        /// <param name="repairer">repairer to use</param>
        /// <param name="isStrictMode">whether it consider stuff like "pine" as "isa pine"</param>
        /// <returns>Returns a selection of concept from query (in a SQLish way)</returns>
        public static HashSet<Concept> Select(string conditions, NameMapper nameMapper, Memory currentMemory, bool isConsiderSelfMuct)
        {
            repairedBranches = new HashSet<ConnectionBranch>();
            AiSqlWrapper.nameMapper = nameMapper;
            AiSqlWrapper.currentMemory = currentMemory;
            conditions = conditions.FixStringForHimmlStatementParsing();
            conditions = conditions.TryRemoveUselessParantheses();
            return Select(conditions, isConsiderSelfMuct);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Get all concept matching provided query
        /// </summary>
        /// <param name="query">AiSql query</param>
        /// <param name="isConsiderSelfMuct">whether we consider stuff like pie isa pie because isa muct isa</param>
        /// <returns>all concept matching provided query</returns>
        private static HashSet<Concept> Select(string query, bool isConsiderSelfMuct)
        {
            HashSet<Concept> selection1;
            HashSet<Concept> selection2;
            List<string> splittedQuery = QuerySplitter.TrySplit(query);
            if (splittedQuery != null)
            {
                selection1 = Select(splittedQuery[0], isConsiderSelfMuct);
                selection2 = Select(splittedQuery[2], isConsiderSelfMuct);

                if (splittedQuery[1] == "and")
                {
                    selection1.IntersectWith(selection2);
                    return selection1;
                }
                else if (splittedQuery[1] == "or")
                {
                    selection1.UnionWith(selection2);
                    return selection1;
                }
                else if (splittedQuery[1] == "and not") //but not
                {
                    selection1.ExceptWith(selection2);
                    return selection1;
                }
                else
                {
                    throw new AiSqlException("Couldn't parse expression: "+query);
                }
            }
            else
            {
                List<Concept> verbAndComplement = GetVerbAndComplement(query);
                Concept verb = verbAndComplement[0];
                Concept complement = verbAndComplement[1];

                Repairer.Repair(complement, repairedBranches);

                selection1 = GetSubjectListHaving(verb, complement, isConsiderSelfMuct);

                if (query.StartsWith("not "))
                {
                    selection2 = new HashSet<Concept>(currentMemory);
                    selection2.ExceptWith(selection1);
                    selection1 = selection2;
                }

                return selection1;
            }
        }

        /// <summary>
        /// Extract verb and complement from provided atomic aiSql query
        /// </summary>
        /// <param name="query">provided atomic aiSql query</param>
        /// <returns>verb and complement</returns>
        private static List<Concept> GetVerbAndComplement(string query)
        {
            Concept verb, complement;
            int verbId, complementId;

            if (query.StartsWith("not "))
                query = query.Substring(4);

            string[] words = query.Split(' ');
            if (words.Length != 2)
                throw new AiSqlException("Couldn't parse: (" + query + ") as a search criteria");

            string verbName = words[0];
            string complementName = words[1];

            verbId = nameMapper.GetOrCreateConceptId(verbName);
            verb = currentMemory.GetOrCreateConcept(verbId);

            complementId = nameMapper.GetOrCreateConceptId(complementName);
            complement = currentMemory.GetOrCreateConcept(complementId);

            return new List<Concept>() { verb, complement };
        }

        /// <summary>
        /// Returns all concept having provided connection (provided verb and complement)
        /// </summary>
        /// <param name="verb">verb</param>
        /// <param name="complement">complement</param>
        /// <param name="isConsiderSelfMuct">whether we consider stuff like pie isa pie because isa muct isa</param>
        /// <returns>all concept having provided connection</returns>
        private static HashSet<Concept> GetSubjectListHaving(Concept verb, Concept complement, bool isConsiderSelfMuct)
        {
            if (verb.IsFlatDirty || (complement != null && complement.IsFlatDirty))
                throw new AiSqlException("Repair concepts first");

            HashSet<Concept> inverseOfVerbList = MetaConnectionManager.GetVerbFlatListFromMetaConnection(verb, "inverse_of", true);
            inverseOfVerbList.UnionWith(MetaConnectionManager.GetVerbFlatListFromMetaConnection(verb, "permutable_side", true));



            HashSet<Concept> subjectList = new HashSet<Concept>();

            if (complement != null)
            {
                foreach (Concept inverseVerb in inverseOfVerbList)
                    subjectList.UnionWith(complement.GetFlatConnectionBranch(inverseVerb).ComplementConceptList);

                if (isConsiderSelfMuct)
                {
                    //If the verb self muct, we add complement to subjects
                    //Will allow stuff like "plant isa plant" or "wood madeof wood"
                    if (MetaConnectionManager.GetVerbFlatListFromMetaConnection(verb, "muct", true).Contains(verb))
                        subjectList.Add(complement);
                }
            }
            else //Null complement can be any complement
            {
                foreach (Concept subject in currentMemory)
                    if (subject.GetFlatConnectionBranch(verb).ComplementConceptList.Count > 0)
                        subjectList.Add(subject);
            }

            return subjectList;
        }
        #endregion
    }
}
