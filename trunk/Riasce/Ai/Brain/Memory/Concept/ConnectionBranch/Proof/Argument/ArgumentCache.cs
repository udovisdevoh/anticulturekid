using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    static class ArgumentCache
    {
        #region Fields
        private static Dictionary<Concept, Dictionary<Concept, Dictionary<Concept, Argument>>> cacheData = new Dictionary<Concept, Dictionary<Concept, Dictionary<Concept, Argument>>>();
        #endregion

        #region Public Methods
        public static Argument GetOrCreateArgument(Concept subject, Concept verb, Concept complement)
        {
            Dictionary<Concept, Dictionary<Concept, Argument>> subjectCache;

            if (!cacheData.TryGetValue(subject, out subjectCache))
            {
                subjectCache = new Dictionary<Concept, Dictionary<Concept, Argument>>();
                cacheData.Add(subject, subjectCache);
            }

            Dictionary<Concept, Argument> verbCache;
            
            if (!subjectCache.TryGetValue(verb, out verbCache))
            {
                verbCache = new Dictionary<Concept, Argument>();
                subjectCache.Add(verb, verbCache);
            }

            Argument argument;

            if (!verbCache.TryGetValue(complement, out argument))
            {
                argument = Argument.GetNewEmptyArgument();
                argument.Subject = subject;
                argument.Verb = verb;
                argument.Complement = complement;
                verbCache.Add(complement, argument);
            }

            return argument;
            
        }
        #endregion
    }
}
