using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Text;

namespace AntiCulture.Kid
{
    class RegexMatcher
    {
        #region Fields
        private static List<Regex> regexList;
        #endregion

        #region Constructor
        static RegexMatcher()
        {
            regexList = new List<Regex>();
            regexList.Add(GetNullaryOperatorSingleWord(LanguageDictionary.NullaryOperatorList));
            regexList.Add(GetUnaryOperatorDoubleWord(LanguageDictionary.UnaryOperatorList));
            //Equivalence expression
            regexList.Add(new Regex(@"\A[a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+ = [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+\Z"));
            //Binary if
            regexList.Add(new Regex(@"\Aif [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+ then [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+\Z"));
            //Not binary if
            regexList.Add(new Regex(@"\Anot if [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+ then [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+\Z"));
            //Ternary if
            regexList.Add(new Regex(@"\Aif [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+ and [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+ then [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+\Z"));
            //Not ternary if
            regexList.Add(new Regex(@"\Anot if [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+ and [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+ then [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+\Z"));
            //asking why
            regexList.Add(new Regex(@"\Awhy [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+\Z"));
            //asking visualize_why
            regexList.Add(new Regex(@"\Avisualize_why [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+\Z"));
            //asking why not (incomplete match)
            regexList.Add(new Regex(@"\Awhy not [a-z0-9_()]+ [a-z0-9_()]+\Z"));
            //asking visualize_why not (incomplete match)
            regexList.Add(new Regex(@"\Avisualize_why not [a-z0-9_()]+ [a-z0-9_()]+\Z"));
            //asking not
            regexList.Add(new Regex(@"\Anot [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+\Z"));
            //asking why not
            regexList.Add(new Regex(@"\Awhy not [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+\Z"));
            //asking visualize_why not
            regexList.Add(new Regex(@"\Avisualize_why not [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+\Z"));
            //Testing connection
            regexList.Add(new Regex(@"\A[a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+\?\Z"));
            //MetaOperation
            regexList.Add(GetMetaOperation(LanguageDictionary.MetaOperatorList));
            //Quaternary statcross
            regexList.Add(new Regex(@"\Astatcross [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+\Z"));
            //Ternary analogize
            regexList.Add(new Regex(@"\Aanalogize [a-z0-9_()]+ [a-z0-9_()]+ [a-z0-9_()]+\Z"));
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Returns true if string matches one and only one regex, else: false
        /// </summary>
        /// <param name="stringToMatch">string to match</param>
        /// <returns>true if string matches one and only one regex, else: false</returns>
        public bool MatchesOneAndOnlyOneRegex(string stringToMatch)
        {
            bool isAskingWhy = false;
            bool isAskingVisualizeWhy = false;
            bool isNegative = false;
            bool isInterrogation = false;

            if (stringToMatch.ContainsWord("why") || stringToMatch.ContainsWord("how"))
                isAskingWhy = true;
            else if (stringToMatch.ContainsWord("visualize_why"))
                isAskingVisualizeWhy = true;

            if (stringToMatch.ContainsWord("not"))
                isNegative = true;

            if (stringToMatch.ContainsWord("does") || stringToMatch.Contains("?"))
                isInterrogation = true;

            stringToMatch = stringToMatch.ToLower();
            stringToMatch = stringToMatch.RemoveWord("not");
            stringToMatch = stringToMatch.RemoveWord("does");
            stringToMatch = stringToMatch.RemoveWord("why");
            stringToMatch = stringToMatch.RemoveWord("visualize_why");
            stringToMatch = stringToMatch.RemoveWord("how");
            stringToMatch = stringToMatch.RemoveWord("cant");
            stringToMatch = stringToMatch.RemoveWord("unlikely");
            stringToMatch = stringToMatch.RemoveDoubleSpaces();

            if (isNegative)
                stringToMatch = "not " + stringToMatch;

            if (isAskingWhy)
                stringToMatch = "why " + stringToMatch;

            if (isAskingVisualizeWhy)
                stringToMatch = "visualize_why " + stringToMatch;

            if (isInterrogation)
                stringToMatch = stringToMatch + "?";

            int matchCount = 0;
            foreach (Regex regex in regexList)
                if (regex.IsMatch(stringToMatch))
                    matchCount++;

            if (matchCount == 1)
                return true;
            else
                return false;
        }
        #endregion

        #region Private methods
        private static Regex GetNullaryOperatorSingleWord(List<string> oldWordList)
        {
            List<string> wordList = new List<string>(oldWordList);

            wordList.Remove("statcross");
            wordList.Remove("analogize");
            wordList.Remove("select");
            wordList.Remove("update");
            wordList.Remove("imply");

            string[] words = wordList.ToArray();

            string orWordList = string.Join("|",words);

            Regex regex = new Regex(@"\A("+orWordList+@")\Z");
            return regex;
        }

        private static Regex GetUnaryOperatorDoubleWord(List<string> wordList)
        {
            string[] words = wordList.ToArray();

            string orWordList = string.Join("|", words);

            Regex regex = new Regex(@"\A(" + orWordList + @") [a-z0-9_()]+\Z");
            return regex;
        }

        private static Regex GetMetaOperation(List<string> wordList)
        {
            string[] words = wordList.ToArray();

            string orWordList = string.Join("|", words);

            Regex regex = new Regex(@"\A[a-z0-9_()]+ (" + orWordList + @") [a-z0-9_()]+\Z");
            return regex;
        }
        #endregion
    }
}
