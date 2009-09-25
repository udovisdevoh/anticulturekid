using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    class BinaryStatementFactory : AbstractStatementFactory
    {
        #region Methods
        public sealed override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            bool isNegative = false;
            bool isInterrogative = false;
            bool isAskingWhy = false;
            bool isAskingVisualizeWhy = false;

            humanStatement = humanStatement.FixStringForHimmlStatementParsing();

            #region If statement contains "?"
            if (humanStatement.Contains("?"))
            {
                isInterrogative = true;
                humanStatement = humanStatement.Replace("?", "");
            }
            #endregion

            #region If statement contains "does"
            if (humanStatement.ContainsWord("does"))
            {
                isInterrogative = true;
                humanStatement = humanStatement.RemoveWord("does");
            }
            #endregion

            #region If statements contains "how" or "why"
            if (humanStatement.ContainsWord("why") || humanStatement.ContainsWord("how"))
            {
                isAskingWhy = true;
                isInterrogative = true;
                humanStatement = humanStatement.RemoveWord("why");
                humanStatement = humanStatement.RemoveWord("how");
            }
            else if (humanStatement.ContainsWord("visualize_why"))
            {
                humanStatement = humanStatement.RemoveWord("visualize_why");
                isAskingVisualizeWhy = true;
                isInterrogative = true;
            }
            #endregion

            #region If statement contains "not"
            if (humanStatement.ContainsWord("not"))
            {
                isNegative = true;
                humanStatement = humanStatement.RemoveWord("not");
            }
            #endregion

            if (LanguageDictionary.StringContainsSpecialKeyWords(humanStatement))
                throw new StatementParsingException("Couldn't parse operation statement. Primitive operators cannot be used as concepts.");

            List<string> words = new List<string>(humanStatement.Split(' '));

            if (words.Count != 3)
                throw new StatementParsingException("Couldn't parse operation statement. Bad word count.");

            if (LanguageDictionary.MetaOperatorList.Contains(words[1]))
                return new Statement(humanName, Statement.MODE_META_OPERATION, words[1], words[0], words[2], isNegative, humanStatement);
            else
                return new Statement(humanName, Statement.MODE_OPERATION, words[0], words[1], words[2], isInterrogative, isNegative, isAskingWhy, isAskingVisualizeWhy, humanStatement);
        }
        #endregion
    }
}
