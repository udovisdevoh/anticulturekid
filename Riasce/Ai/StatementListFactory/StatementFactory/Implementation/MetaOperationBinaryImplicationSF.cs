using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Text;

namespace AntiCulture.Kid
{
    class MetaOperationBinaryImplicationStatementFactory : AbstractStatementFactory
    {
        #region Fields and parts
        private static readonly Regex directImplication = new Regex(@"\Aif ([a-z0-9_()]+) [a-z0-9_()]+ ([a-z0-9_()]+) then \1 [a-z0-9_()]+ \2\Z");

        private static readonly Regex inverseImplication = new Regex(@"\Aif ([a-z0-9_()]+) [a-z0-9_()]+ ([a-z0-9_()]+) then \2 [a-z0-9_()]+ \1\Z");

        private ImplyStatementFactory implyStatementFactory = new ImplyStatementFactory();
        #endregion

        #region Methods
        #region Getters
        public sealed override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            string metaOpratorName;
            string originalStatement = humanStatement;

            bool isNegative = humanStatement.StartsWith("not");
            if (isNegative)
                humanStatement = humanStatement.Substring(4);

            bool isCant = humanStatement.ContainsWord("cant");
            humanStatement = humanStatement.RemoveWord("cant");

            bool isUnlikely = humanStatement.ContainsWord("unlikely");
            humanStatement = humanStatement.RemoveWord("unlikely");

            humanStatement = humanStatement.Replace("?", "");
            humanStatement = humanStatement.Trim();

            List<string> words = new List<string>(humanStatement.Split(' '));

            if (directImplication.IsMatch(humanStatement))
            {
                if (isCant)
                    metaOpratorName = "cant";
                else if (isUnlikely)
                    metaOpratorName = "unlikely";
                else
                    metaOpratorName = "direct_implication";

                return new Statement(humanName, Statement.MODE_META_OPERATION, metaOpratorName, words[2], words[6], isNegative, originalStatement);
            }
            else if (inverseImplication.IsMatch(humanStatement))
            {
                if (isCant)
                    metaOpratorName = "cant";
                else if (isUnlikely)
                    metaOpratorName = "unlikely";
                else
                    metaOpratorName = "inverse_implication";

                return new Statement(humanName, Statement.MODE_META_OPERATION, metaOpratorName, words[2], words[6], isNegative, originalStatement);
            }
            else
            {
                if (isNegative)
                {
                    return implyStatementFactory.GetInterpretedHumanStatement(humanName, "not " + humanStatement);
                }
                else
                {
                    return implyStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
                }
            }
        }
        #endregion
        #endregion
    }
}