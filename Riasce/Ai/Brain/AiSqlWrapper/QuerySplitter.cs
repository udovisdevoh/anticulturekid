using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used to split an AiSql query into smaller chunks
    /// </summary>
    static class QuerySplitter
    {
        #region Public methods
        /// <summary>
        /// Split a query into two queries. Return null if query can't be splitted
        /// </summary>
        /// <param name="query">query to split</param>
        /// <returns>Splitted query. element[0] query1, element[1]: separator, element[2] query2</returns>
        public static List<string> TrySplit(string query)
        {
            query = query.Replace(")or(", ") or (");
            query = query.Replace(" or(", " or (");
            query = query.Replace(")or ", ") or ");
            query = query.Replace(")and(", ") and (");
            query = query.Replace(" and(", " and (");
            query = query.Replace(")and ", ") and ");
            query = query.Replace(")and not(", ") and not (");
            query = query.Replace(" and not(", " and not (");
            query = query.Replace(")and not ", ") and not ");

            query = query.RemoveDoubleSpaces();
            query = query.Trim();

            query = query.TryRemoveUselessParantheses();

            string[] words = query.Split(' ');

            if (words.Length < 2)
                throw new AiSqlException("Couldn't parse query: " + query);

            if (words.Length == 2 || (words.Length == 3 && words[0] == "not"))
            {
                return null; //Couldn't split query, must use it as an atom
            }
            else
            {
                List<string> splittedQuery;
                splittedQuery = TrySplitOr(query);
                if (splittedQuery == null)
                    splittedQuery = TrySplitAndNot(query);
                if (splittedQuery == null)
                    splittedQuery = TrySplitAnd(query);
                if (splittedQuery == null)
                    throw new AiSqlException("Couldn't split query: " + query);

                return splittedQuery;
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Try split query into two chunks when separated by "or"
        /// </summary>
        /// <param name="query">query to split</param>
        /// <returns>Splitted query. element[0] query1, element[1]: separator, element[2] query2</returns>
        private static List<string> TrySplitOr(string query)
        {
            List<string> splittedQuery = new List<string>();
            Dictionary<int, int> depthMap = query.GetParantheseDepthMap();

            List<int> orPosition = GetPositionsListFor(query, " or ");

            foreach (int position in orPosition)
            {
                if (depthMap[position] == 0)
                {
                    splittedQuery.Add(query.Substring(0, position).TryRemoveUselessParantheses());
                    splittedQuery.Add("or");
                    splittedQuery.Add(query.Substring(position + 4).TryRemoveUselessParantheses());

                    return splittedQuery;
                }
            }

            return null;
        }

        /// <summary>
        /// Try split query into two chunks when separated by "and not"
        /// </summary>
        /// <param name="query">query to split</param>
        /// <returns>Splitted query. element[0] query1, element[1]: separator, element[2] query2</returns>
        private static List<string> TrySplitAndNot(string query)
        {
            List<string> splittedQuery = new List<string>();
            Dictionary<int, int> depthMap = query.GetParantheseDepthMap();

            List<int> andNotPosition = GetPositionsListFor(query, " and not ");

            foreach (int position in andNotPosition)
            {
                if (depthMap[position] == 0)
                {
                    splittedQuery.Add(query.Substring(0, position).TryRemoveUselessParantheses());
                    splittedQuery.Add("and not");
                    splittedQuery.Add(query.Substring(position + 9).TryRemoveUselessParantheses());

                    return splittedQuery;
                }
            }

            return null;
        }

        /// <summary>
        /// Try split query into two chunks when separated by "and"
        /// </summary>
        /// <param name="query">query to split</param>
        /// <returns>Splitted query. element[0] query1, element[1]: separator, element[2] query2</returns>
        private static List<string> TrySplitAnd(string query)
        {
            List<string> splittedQuery = new List<string>();
            Dictionary<int, int> depthMap = query.GetParantheseDepthMap();

            List<int> andPosition = GetPositionsListFor(query, " and ");

            foreach (int position in andPosition)
            {
                if (depthMap[position] == 0)
                {
                    splittedQuery.Add(query.Substring(0, position).TryRemoveUselessParantheses());
                    splittedQuery.Add("and");
                    splittedQuery.Add(query.Substring(position + 5).TryRemoveUselessParantheses());

                    return splittedQuery;
                }
            }

            return null;
        }

        /// <summary>
        /// Get position list to where to cut using needle
        /// </summary>
        /// <param name="haystack">haystack</param>
        /// <param name="needle">needle</param>
        /// <returns>position list to where to cut</returns>
        private static List<int> GetPositionsListFor(string haystack, string needle)
        {
            List<int> positionList = new List<int>();

            for (int charPosition = 0; charPosition < haystack.Length - needle.Length; charPosition++)
            {
                if (haystack.Substring(charPosition, needle.Length) == needle)
                {
                    positionList.Add(charPosition);
                }
            }

            return positionList;
        }
        #endregion
    }
}
