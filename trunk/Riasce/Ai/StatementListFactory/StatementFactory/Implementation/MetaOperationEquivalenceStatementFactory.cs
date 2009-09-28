using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Create equivalence statements
    /// </summary>
    class MetaOperationEquivalenceStatementFactory : AbstractStatementFactory
    {
        #region Fields
        /// <summary>
        /// Matches permutable_side expression
        /// </summary>
        private static readonly Regex permutableSideExpression = new Regex(@"\A([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) = \3 \2 \1\Z");

        /// <summary>
        /// Matches inverse_of expression
        /// </summary>
        private static readonly Regex inverseOfExpression = new Regex(@"\A([a-z0-9_()]+) [a-z0-9_()]+ ([a-z0-9_()]+) = \2 [a-z0-9_()]+ \1\Z");
        #endregion

        #region Public Methods
        /// <summary>
        /// Get interpreted human statement
        /// </summary>
        /// <param name="humanName">human's name</param>
        /// <param name="humanStatement">raw human statement</param>
        /// <returns>interpreted human statement</returns>
        public sealed override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            bool isNegative = humanStatement.ContainsWord("not");

            humanStatement = humanStatement.FixStringForHimmlStatementParsing();
            humanStatement = humanStatement.Replace("?", "");
            humanStatement = humanStatement.Trim();
            humanStatement = humanStatement.RemoveWord("not");

            List<string> words = new List<string>(humanStatement.Split(' '));

            if (permutableSideExpression.IsMatch(humanStatement) && words[1] == words[5])
                return new Statement(humanName, Statement.MODE_META_OPERATION, "permutable_side", words[1], words[5], isNegative, humanStatement);
            else if (inverseOfExpression.IsMatch(humanStatement) && words[1] != words[5])
                return new Statement(humanName, Statement.MODE_META_OPERATION, "inverse_of", words[1], words[5], isNegative, humanStatement);
            else
                throw new StatementParsingException("Couldn't match equivalence expression");
        }
        #endregion
    }
}
