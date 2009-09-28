using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Create imply statements
    /// </summary>
    class ImplyStatementFactory : AbstractStatementFactory
    {
        #region Public Methods
        /// <summary>
        /// Create imply statements
        /// </summary>
        /// <param name="humanName">human's name</param>
        /// <param name="humanStatement">human's raw statement</param>
        /// <returns>parsed imply statement</returns>
        public override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            while (humanStatement.Contains("( "))
                humanStatement = humanStatement.Replace("( ", "(");

            while (humanStatement.Contains(" )"))
                humanStatement = humanStatement.Replace(" )", ")");

            if (humanStatement.ContainsWord("if") && humanStatement.ContainsWord("then"))
                humanStatement = TransformIfThenElseToImply(humanStatement);

            bool isNegative = false;

            humanStatement = humanStatement.Replace(" and not ", " and_not ");
            if (humanStatement.ContainsWord("not"))
            {
                isNegative = true;
                humanStatement = humanStatement.RemoveWord("not").Trim();
            }
            humanStatement = humanStatement.Replace(" and_not ", " and not ");

            humanStatement = humanStatement.Trim();
            string conditions = humanStatement.Substring(humanStatement.IndexOf("where ") + 6);
            string actions = humanStatement.Substring(6);
            int wherePosition = actions.IndexOf("where ");

            if (wherePosition < 1)
                throw new StatementParsingException("actions to perform are invalid");

            actions = actions.Substring(0, wherePosition - 1);

            return new Statement(humanName, Statement.MODE_EXPLY, actions, conditions,isNegative);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Transform "if, then, else" syntax to "imply" syntax
        /// </summary>
        /// <param name="humanStatement">human's raw statement</param>
        /// <returns>"imply" syntax</returns>
        private string TransformIfThenElseToImply(string humanStatement)
        {
            bool isNegative = false;
            string newStatement = "";
            if (humanStatement.StartsWith("not"))
            {
                isNegative = true;
                humanStatement = humanStatement.Substring(4);
            }
            humanStatement = humanStatement.Replace(" and_not ", " and not ");

            string subject = humanStatement.Replace("(","").Substring(3);
            subject = subject.Substring(0,subject.IndexOf(' ')).Trim();

            humanStatement = humanStatement.RemoveWord(subject);
            humanStatement = humanStatement.Replace("(" + subject + " ", "(");

            string action = humanStatement.Substring(humanStatement.LastIndexOf(" then ") + 5).Trim();
            string condition = humanStatement.Substring(0,humanStatement.LastIndexOf(" then "));
            condition = condition.Substring(2).Trim();

            if (isNegative)
                newStatement += "not ";

            newStatement += "imply " + action + " where " + condition;

            return newStatement;
        }
        #endregion
    }
}
