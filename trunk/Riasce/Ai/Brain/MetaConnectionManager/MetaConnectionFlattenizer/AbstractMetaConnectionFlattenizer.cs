using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractMetaConnectionFlattenizer
    {
        /// <summary>
        /// Returns all the verbs that are (recursively or not) metaconnected to sourceVerb through metaOperator name
        /// If data exist in cache, use data from cache, or else, regenerate it
        /// </summary>
        /// <param name="sourceVerb">source verb</param>
        /// <param name="metaOperatorName">metaOperator name</param>
        /// <param name="isMetaConnectionPositive">whether you want positive or negative metaconnections</param>
        /// <returns>List of verbs that are (recursively or not) metaconnected to sourceVerb through metaOperator name</returns>
        public abstract HashSet<Concept> GetFlatVerbListFromMetaConnection(Concept sourceVerb, string metaOperatorName, bool isMetaConnectionPositive);

        /// <summary>
        /// Add a verb concept to rememberable total list of verbs
        /// </summary>
        /// <param name="concept">verb to remember</param>
        public abstract void AddVerbToList(Concept concept);
    }
}
