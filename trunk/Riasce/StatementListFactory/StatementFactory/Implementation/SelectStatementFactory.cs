using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    class SelectStatementFactory : AbstractStatementFactory
    {
        public sealed override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            humanStatement = humanStatement.Remove(0, 13);
            humanStatement = humanStatement.Trim();

            return new Statement(humanName, Statement.MODE_SELECT, humanStatement);
        }
    }
}
