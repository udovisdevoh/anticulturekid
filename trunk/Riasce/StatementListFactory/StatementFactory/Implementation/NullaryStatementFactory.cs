using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    class NullaryStatementFactory : AbstractStatementFactory
    {
        public sealed override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            humanStatement = humanStatement.FixStringForHimmlStatementParsing();
            if (LanguageDictionary.NullaryOperatorList.Contains(humanStatement))
                return new Statement(humanName,Statement.MODE_NULLARY_OPERATOR, humanStatement);
            throw new StatementParsingException("Couldn't find nullary operator " + humanStatement);
        }
    }
}
