using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class UpdateStatementFactory : AbstractStatementFactory
    {
        public override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            humanStatement = humanStatement.Trim();
            string conditions = humanStatement.Substring(humanStatement.IndexOf("where ") + 6);
            string actions = humanStatement.Substring(7);
            int wherePosition = actions.IndexOf("where ");

            if (wherePosition < 1)
                throw new StatementParsingException("actions to perform are invalid");

            actions = actions.Substring(0, wherePosition - 1);

            return new Statement(humanName, Statement.MODE_UPDATE, actions, conditions);
        }
    }
}
