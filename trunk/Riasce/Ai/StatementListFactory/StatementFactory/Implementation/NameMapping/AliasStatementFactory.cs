using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Used to create alias statements
    /// </summary>
    class AliasStatementFactory : AbstractStatementFactory
    {
        #region Public Methods
        /// <summary>
        /// Create alias statement from string
        /// </summary>
        /// <param name="humanName">human name</param>
        /// <param name="humanStatement">human statement</param>
        /// <returns>parsed alias statement</returns>
        public sealed override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            bool isNegative = false;
            humanStatement = humanStatement.RemoveWord("aliasof");

            if (humanStatement.ContainsWord("not"))
            {
                humanStatement = humanStatement.RemoveWord("not");
                isNegative = true;
            }

            if (LanguageDictionary.StringContainsSpecialKeyWordsOrMetaOperators(humanStatement))
                throw new StatementParsingException("cannot use special keywords in naming association");

            string[] words = humanStatement.Split(' ');

            if (words.Length != 2)
                throw new StatementParsingException("Expecting only 2 words (or 3 with not)");

            return new Statement(humanName, Statement.MODE_NAMING_ASSOCIATION, "aliasof", words[0], words[1], isNegative, humanStatement);
        }
        #endregion
    }
}