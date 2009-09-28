using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Text;

namespace AntiCulture.Kid
{
    class MetaOperationStatementFactory : AbstractStatementFactory
    {
        #region Fields
        private static readonly Regex equivalenceExpression = new Regex(@"\A[a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+ = [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+\Z");
        #endregion

        #region Parts
        private MetaOperationEquivalenceStatementFactory metaOperationEquivalenceStatementFactory = new MetaOperationEquivalenceStatementFactory();

        private MetaOperationImplicationStatementFactory metaOperationImplicationStatementFactory = new MetaOperationImplicationStatementFactory();
        #endregion

        #region Methods
        #region Getters
        public sealed override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            humanStatement = humanStatement.FixStringForHimmlStatementParsing();
            humanStatement = humanStatement.Replace("?", "");
            humanStatement = humanStatement.Trim();

            string[] words = humanStatement.Split(' ');

            #region Take charge "not" word
            string humanStatementWithoutNot = " " + humanStatement+ " ";
            humanStatementWithoutNot = humanStatementWithoutNot.Replace(" not ", " ");
            humanStatementWithoutNot = humanStatementWithoutNot.Trim();
            #endregion

            if (equivalenceExpression.IsMatch(humanStatementWithoutNot))
                return metaOperationEquivalenceStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
            else if ((words[0] == "if" || humanStatement.Contains(" if ")) && humanStatement.Contains(" then "))
                return metaOperationImplicationStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
            else
                throw new StatementParsingException("Couldn't match meta operation");
        }
        #endregion
        #endregion
    }
}
