using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Create Select statements
    /// </summary>
    class SelectStatementFactory : AbstractStatementFactory
    {
        /// <summary>
        /// Create Select statement
        /// </summary>
        /// <param name="humanName">human's name</param>
        /// <param name="humanStatement">human's raw statement</param>
        /// <returns>Select statement</returns>
        public sealed override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            humanStatement = humanStatement.Remove(0, 13);
            humanStatement = humanStatement.Trim();

            return new Statement(humanName, Statement.MODE_SELECT, humanStatement);
        }
    }
}
