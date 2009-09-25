using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used to convert AiSql Queries as string into real conditions
    /// </summary>
    class ConditionBuilder
    {
        #region Fields
        private QuerySplitter querySplitter = new QuerySplitter();
        #endregion

        #region Public Methods
        /// <summary>
        /// Create a condition from string
        /// </summary>
        /// <param name="text">provided textual condition</param>
        /// <param name="nameMapper">name mapper to look into</param>
        /// <param name="memory">memory to look into</param>
        /// <returns>Parsed condition</returns>
        public Condition ParseString(string text, NameMapper nameMapper, Memory memory)
        {
            text = text.FixStringForHimmlStatementParsing();
            text = text.TryRemoveUselessParantheses();

            List<string> splittedQuery = querySplitter.TrySplit(text);
            if (splittedQuery != null)
            {
                Condition leftChild = ParseString(splittedQuery[0], nameMapper, memory);
                Condition rightChild = ParseString(splittedQuery[2], nameMapper, memory);
                string logicOperator = splittedQuery[1];

                if (logicOperator != "and" && logicOperator != "or" && logicOperator != "and not")
                    throw new AiSqlException("Couldn't parse expression: " + text);

                return new Condition(leftChild, logicOperator, rightChild);
            }
            else
            {
                List<Concept> verbAndComplement = GetVerbAndComplement(text,nameMapper,memory);
                Concept verb = verbAndComplement[0];
                Concept complement = verbAndComplement[1];

                return new Condition(verb, complement);
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Get verb and complement from atomic query
        /// </summary>
        /// <param name="query">atomic query</param>
        /// <param name="nameMapper">name mapper to look into</param>
        /// <param name="currentMemory">memory to look into</param>
        /// <returns>verb and complement from atomic query</returns>
        private List<Concept> GetVerbAndComplement(string query, NameMapper nameMapper, Memory currentMemory)
        {
            if (query.StartsWith("not "))
                query = query.Substring(4);

            string[] words = query.Split(' ');
            if (words.Length != 2)
                throw new AiSqlException("Couldn't parse: (" + query + ") as a search criteria");

            string verbName = words[0];
            string complementName = words[1];

            int verbId = nameMapper.GetOrCreateConceptId(verbName);
            int complementId = nameMapper.GetOrCreateConceptId(complementName);

            Concept verb = currentMemory.GetOrCreateConcept(verbId);
            Concept complement = currentMemory.GetOrCreateConcept(complementId);

            return new List<Concept>() { verb, complement };
        }
        #endregion
    }
}