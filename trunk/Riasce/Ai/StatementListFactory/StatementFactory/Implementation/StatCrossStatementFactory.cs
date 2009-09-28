using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Build statCross statements
    /// </summary>
    class StatCrossStatementFactory : AbstractStatementFactory
    {
        /// <summary>
        /// Build statcross statement
        /// </summary>
        /// <param name="humanName">human's name</param>
        /// <param name="humanStatement">human's raw statement</param>
        /// <returns>statcross statement</returns>
        public sealed override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            humanStatement = humanStatement.FixStringForHimmlStatementParsing();
            humanStatement = humanStatement.RemoveWord("analogize");
            humanStatement = humanStatement.RemoveWord("why");
            humanStatement = humanStatement.RemoveWord("does");
            humanStatement = humanStatement.RemoveWord("statcross");
            humanStatement = humanStatement.RemoveWord("how");
            humanStatement = humanStatement.RemoveWord("not");
            humanStatement = humanStatement.Replace("?", "");

            if (LanguageDictionary.StringContainsSpecialKeyWords(humanStatement))
                throw new StatementParsingException("Couldn't parse operation statement. Primitive operators cannot be used as concepts.");

            List<string> words = new List<string>(humanStatement.Split(' '));

            if (words.Count == 4)
                return new Statement(humanName, Statement.MODE_STATCROSS, words[0], words[1], words[2], words[3]);
            else if (words.Count == 2)
                return new Statement(humanName, Statement.MODE_STATCROSS, words[0], words[1]);
            else if (words.Count == 0 || words.Count == 1 && words[0] == "")
                return new Statement(humanName, Statement.MODE_STATCROSS);
            else
                throw new StatementParsingException("Couldn't parse operation statement. Bad word count.");
        }
    }
}
