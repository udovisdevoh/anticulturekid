using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractRegexMatcher
    {
        /// <summary>
        /// Returns true if string matches one and only one regex, else: false
        /// </summary>
        /// <param name="stringToMatch">string to match</param>
        /// <returns>true if string matches one and only one regex, else: false</returns>
        public abstract bool MatchesOneAndOnlyOneRegex(string stringToMatch);
    }
}
