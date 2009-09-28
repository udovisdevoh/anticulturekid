using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Build rename statement
    /// </summary>
    class RenameStatementFactory : AbstractStatementFactory
    {
        /// <summary>
        /// Rename statement
        /// </summary>
        /// <param name="humanName">human's name</param>
        /// <param name="humanStatement">human's raw statement</param>
        /// <returns>Rename statement</returns>
        public sealed override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            bool isNegative = false;
            humanStatement = humanStatement.RemoveWord("rename");

            string[] words = humanStatement.Split(' ');

            if (words.Length != 2)
                throw new StatementParsingException("Expecting exactly 2 concepts");

            return new Statement(humanName, Statement.MODE_NAMING_ASSOCIATION, "rename", words[0], words[1], isNegative, humanStatement);
        }
    }
}
