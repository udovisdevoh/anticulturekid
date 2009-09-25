using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    class RelativeSubOrdinateExtractor : AbstractRelativeSubOrdinateExtractor
    {
        #region Public Methods
        public override string ExtractList(string originalStatement, out List<string> relativeSubOrdinateList)
        {
            relativeSubOrdinateList = new List<string>();

            string subOrdinate = null;

            do
            {
                originalStatement = Extract(originalStatement, out subOrdinate);

                if (subOrdinate != null)
                    relativeSubOrdinateList.Add(subOrdinate);

            } while (subOrdinate != null);


            return originalStatement;
        }

        public override string Extract(string originalString, out string subOrdinate)
        {
            if (!originalString.ContainsWord("(which"))
            {
                subOrdinate = null;
                return originalString;
            }

            subOrdinate = GetSubOrdinate(originalString);

            string alteredOriginalString = originalString;
            alteredOriginalString = alteredOriginalString.ReplaceFirstOccurenceOf(subOrdinate, "");

            alteredOriginalString = alteredOriginalString.FixStringForHimmlStatementParsing();

            if (subOrdinate != null)
            {
                subOrdinate = GetAbsoluteSubOrdinate(originalString, subOrdinate);
                subOrdinate = subOrdinate.Substring(1, subOrdinate.Length - 2);
            }

            return alteredOriginalString;
        }
        #endregion

        #region Private Methods
        private string GetSubOrdinate(string originalString)
        {
            string subOrdinate = originalString.Substring(originalString.IndexOf("(which"));

            int depth = 0;
            int charCount = 0;
            foreach (char letter in subOrdinate)
            {
                if (letter == '(')
                    depth++;
                else if (letter == ')')
                    depth--;

                if (letter == ')' && depth == 0)
                {
                    subOrdinate = subOrdinate.Substring(0, charCount + 1);
                    subOrdinate = subOrdinate.Trim();
                    return subOrdinate;
                }

                charCount++;
            }

            throw new StatementParsingException("Couldn't find closing parantheses to \"which\" close");

        }

        private string GetAbsoluteSubOrdinate(string originalString, string subOrdinate)
        {
            string whatWhichRepresents = originalString.GetWordBeforeFirstOccurenceOfWord("(which");

            if (whatWhichRepresents != null)
                subOrdinate = subOrdinate.ReplaceFirstOccurenceOfWord("(which", "(" + whatWhichRepresents);
            else
                throw new StatementParsingException("cannot match keyword \"which\" to anything");

            return subOrdinate;
        }
        #endregion
    }
}
