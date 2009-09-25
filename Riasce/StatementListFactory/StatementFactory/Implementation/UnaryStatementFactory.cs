using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    class UnaryStatementFactory : AbstractStatementFactory
    {
        public sealed override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            humanStatement = humanStatement.FixStringForHimmlStatementParsing();
            humanStatement = humanStatement.Replace("?","");
            string[] words = humanStatement.Split(' ');
            if (LanguageDictionary.UnaryOperatorList.Contains(words[0]))
                return new Statement(humanName, Statement.MODE_UNARY_OPERATOR, words[0], words[1]);
            throw new StatementParsingException("Couldn't find unary operator " + humanStatement);
        }
    }
}
