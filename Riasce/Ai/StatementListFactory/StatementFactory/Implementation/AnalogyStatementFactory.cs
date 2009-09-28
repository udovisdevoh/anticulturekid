using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    class AnalogyStatementFactory : AbstractStatementFactory
    {
        public sealed override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            humanStatement = humanStatement.FixStringForHimmlStatementParsing();
            humanStatement = humanStatement.RemoveWord("analogize");
            humanStatement = humanStatement.RemoveWord("why");
            humanStatement = humanStatement.RemoveWord("does");
            humanStatement = humanStatement.RemoveWord("how");
            humanStatement = humanStatement.RemoveWord("statcross");
            humanStatement = humanStatement.RemoveWord("not");
            humanStatement = humanStatement.Replace("?","");

            if (LanguageDictionary.StringContainsSpecialKeyWords(humanStatement))
                throw new StatementParsingException("Couldn't parse operation statement. Primitive operators cannot be used as concepts.");

            List<string> words = new List<string>(humanStatement.Split(' '));

            if (words.Count == 3)
                return new Statement(humanName, Statement.MODE_ANALOGY, words[0], words[1], words[2]);
            else if (words.Count == 2)
                return new Statement(humanName, Statement.MODE_ANALOGY, words[0], words[1]);
            else if (words.Count == 1)
            {
                if (words[0] == "")
                    return new Statement(humanName, Statement.MODE_ANALOGY);
                else
                    return new Statement(humanName, Statement.MODE_ANALOGY, words[0]);
            }
            else if (words.Count == 0)
                return new Statement(humanName, Statement.MODE_ANALOGY);
            else
                throw new StatementParsingException("Couldn't parse operation statement. Bad word count.");
        }
    }
}
