using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public abstract class AbstractVerbMetaConnectionCache
    {
        /// <summary>
        /// Gets the cached verb list
        /// </summary>
        /// <param name="verb">verb</param>
        /// <param name="metaOperatorName">metaOperator name</param>
        /// <param name="isPositive">whether the connection is positive or not</param>
        /// <returns>a list of flat metaConnected verbs to provided verb</returns>
        public abstract HashSet<Concept> GetVerbFlatListFromCache(Concept verb, string metaOperatorName, bool isPositive);

        /// <summary>
        /// Add a list of flat metaConnected verbs to cache
        /// </summary>
        /// <param name="verb">verb</param>
        /// <param name="metaOperatorName">metaOperator name</param>
        /// <param name="isPositive">whether the connection is positive or not</param>
        /// <param name="flatVerbList">a list of flat metaConnected verbs to provided verb</param>
        public abstract void Remember(Concept verb, string metaOperatorName, bool isPositive, HashSet<Concept> flatVerbList);
    }
}
