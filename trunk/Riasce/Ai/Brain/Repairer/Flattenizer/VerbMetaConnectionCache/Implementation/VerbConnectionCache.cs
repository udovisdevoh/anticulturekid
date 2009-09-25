using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public class VerbMetaConnectionCache : AbstractVerbMetaConnectionCache
    {
        #region Fields
        private Dictionary<Concept,Dictionary<string,HashSet<Concept>>> positiveCache = new Dictionary<Concept,Dictionary<string,HashSet<Concept>>>();

        private Dictionary<Concept, Dictionary<string, HashSet<Concept>>> negativeCache = new Dictionary<Concept, Dictionary<string, HashSet<Concept>>>();
        #endregion

        #region Methods
        public override HashSet<Concept> GetVerbFlatListFromCache(Concept verb, string metaOperatorName, bool isPositive)
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

        public override void Remember(Concept verb, string metaOperatorName, bool isPositive, HashSet<Concept> flatVerbList)
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
        #endregion
    }
}
