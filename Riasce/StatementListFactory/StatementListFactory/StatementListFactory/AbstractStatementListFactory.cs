using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractStatementListFactory
    {
        /// <summary>
        /// Represents the abstraction (bridge pattern) of a statementList factory
        /// Its purpose is to convert human generated strings into collections of statement objects that can be interpreted one by one by an AI
        /// </summary>
        #region Methods
        #region Getters
        /// <summary>
        /// Takes a string and return a list of interpreted human statement so they can be parsed one by one by an artificial intelligence program
        /// </summary>
        /// <param name="humanStatement">represents the original human list of statements</param>
        /// <param name="humanName">represents name (concept identifier) of the human who initiaties the original textual statement</param>
        /// <returns></returns>
        public abstract List<Statement> GetInterpretedHumanStatementList(string humanName, string humanStatementList);
        #endregion
        #endregion
    }
}
