using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents the verb metaConnection cache used to improve metaOperation flattenization
    /// </summary>
    public static class VerbMetaConnectionCache
    {
        #region Fields
        /// <summary>
        /// Positive verb metaConnection cache
        /// </summary>
        private static Dictionary<Concept,Dictionary<string,HashSet<Concept>>> positiveCache = new Dictionary<Concept,Dictionary<string,HashSet<Concept>>>();

        /// <summary>
        /// Negative verb metaConnection cache
        /// </summary>
        private static Dictionary<Concept, Dictionary<string, HashSet<Concept>>> negativeCache = new Dictionary<Concept, Dictionary<string, HashSet<Concept>>>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the cached verb list
        /// </summary>
        /// <param name="verb">verb</param>
        /// <param name="metaOperatorName">metaOperator name</param>
        /// <param name="isPositive">whether the connection is positive or not</param>
        /// <returns>a list of flat metaConnected verbs to provided verb</returns>
        public static HashSet<Concept> GetVerbFlatListFromCache(Concept verb, string metaOperatorName, bool isPositive)
        {
            Dictionary<Concept, Dictionary<string, HashSet<Concept>>> currentCache;
            if (isPositive)
                currentCache = positiveCache;
            else
                currentCache = negativeCache;

            Dictionary<string, HashSet<Concept>> verbMetaConnectionSet;
            if (currentCache.TryGetValue(verb, out verbMetaConnectionSet))
            {
                HashSet<Concept> verbMetaConnectionSetForSpecifiedMetaOperator;
                if (verbMetaConnectionSet.TryGetValue(metaOperatorName, out verbMetaConnectionSetForSpecifiedMetaOperator))
                {
                    return verbMetaConnectionSetForSpecifiedMetaOperator;
                }
            }
            return null;
        }

        /// <summary>
        /// Add a list of flat metaConnected verbs to cache
        /// </summary>
        /// <param name="verb">verb</param>
        /// <param name="metaOperatorName">metaOperator name</param>
        /// <param name="isPositive">whether the connection is positive or not</param>
        /// <param name="flatVerbList">a list of flat metaConnected verbs to provided verb</param>
        public static void Remember(Concept verb, string metaOperatorName, bool isPositive, HashSet<Concept> flatVerbList)
        {
            Dictionary<Concept, Dictionary<string, HashSet<Concept>>> currentCache;
            if (isPositive)
                currentCache = positiveCache;
            else
                currentCache = negativeCache;

            Dictionary<string, HashSet<Concept>> verbMetaConnectionSet;
            if (!currentCache.TryGetValue(verb, out verbMetaConnectionSet))
            {
                verbMetaConnectionSet = new Dictionary<string, HashSet<Concept>>();
                currentCache.Add(verb, verbMetaConnectionSet);
            }

            verbMetaConnectionSet.Add(metaOperatorName, flatVerbList);
        }

        /// <summary>
        /// Reset verb metaConnection cache
        /// </summary>
        public static void Clear()
        {
            positiveCache.Clear();
            negativeCache.Clear();
        }
        #endregion
    }
}
