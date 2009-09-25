using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractAutoCompleteModel
    {
        #region Fields
        protected static string conceptCode = " #";

        protected static string nullaryOrUnaryOrPrimitiveOperatorCode = " *";

        protected static string binaryOperatorCode = " @";

        protected static string metaOperatorCode = " &";

        protected static string nameMappingOperatorCode = " $";
        #endregion

        /// <summary>
        /// Refresh autoComplete model's remembered content from sources
        /// Must be done each time the user entered a comment, but nothing more (not every time
        /// we browse in the autoComplete)
        /// </summary>
        /// <param name="currentMemory">current memory</param>
        /// <param name="nameMapper">current name mapper</param>
        public abstract void RefreshFrom(Memory currentMemory, NameMapper nameMapper);

        /// <summary>
        /// Gets a list of tokens starting with startsWith
        /// </summary>
        /// <param name="startsWith">starts with</param>
        /// <returns>List of tokens starting with startsWith</returns>
        public abstract List<string> GetSelection(string startsWith);

        /// <summary>
        /// Return the selection's value
        /// </summary>
        /// <param name="selectionKey">selection's key</param>
        /// <returns>selection's value</returns>
        public abstract string GetSelectionValue(string selectionKey);

        /// <summary>
        /// Returns true if string matches one and only one regex, else: false
        /// </summary>
        /// <param name="stringToMatch">string to match</param>
        /// <returns>true if string matches one and only one regex, else: false</returns>
        public abstract bool MatchesOneAndOnlyOneRegex(string stringToMatch);

        protected string ParseSymbol(string element)
        {
            if (element.Length < 2)
                return element;

            return element.Substring(element.Length - 1, 1) + " " + element.Substring(0, (element.Length - 2));
        }
    }
}
