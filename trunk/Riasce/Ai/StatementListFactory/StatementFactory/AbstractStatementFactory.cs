using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents the abstraction (bridge pattern) of a statement factory
    /// Its purpose is to convert human generated strings into Statement objects that can be interpreted by an AI
    /// </summary>
    abstract class AbstractStatementFactory
    {
        /// <summary>
        /// Takes a string and return an interpreted human statement so it can be parsed by an artificial intelligence program
        /// </summary>
        /// <param name="humanStatement">represents the original human statement</param>
        /// <param name="humanName">represents name (concept identifier) of the human who initiaties the original textual statement</param>
        /// <returns></returns>
        public abstract Statement GetInterpretedHumanStatement(string humanName, string humanStatement);
    }
}
