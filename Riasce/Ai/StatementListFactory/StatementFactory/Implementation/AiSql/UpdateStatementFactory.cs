using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Create update statement
    /// </summary>
    class UpdateStatementFactory : AbstractStatementFactory
    {
        /// <summary>
        /// Create update statement
        /// </summary>
        /// <param name="humanName">human's name</param>
        /// <param name="humanStatement">human's raw statement</param>
        /// <returns>Update statement</returns>
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
