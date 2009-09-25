using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Misc;

namespace Text
{
    static class StringManipulation
    {
        #region Fields
        private static Regex prohibitedChars = new System.Text.RegularExpressions.Regex("[^a-z0-9?=,_() ]");

        private static Random random = new Random();
        #endregion

        #region Public Methods
        public static string FixStringForHimmlStatementParsing(this string text)
        {
            Argument.Ensure(text, "text");
            text = text.Replace(",", " and ");
            text = text.Trim();
            text = text.RemoveDoubleSpaces();
            text = text.ToLower();
            text = text.RemoveProhibitedChars();
            return text;
        }

        public static string ConceptNameFormat(this string text)
        {
            return text.ToLower().RemoveProhibitedChars().Replace(" ", "");
        }

        public static string RemoveDoubleSpaces(this string text)
        {
            Argument.Ensure(text, "text");

            while (text.Contains("  "))
                text = text.Replace("  ", " ");
            return text;
        }

        public static string RemoveProhibitedChars(this string text)
        {
            Argument.Ensure(text, "text");

            return prohibitedChars.Replace(text, "");
        }

        public static string RemoveWord(this string text, string word)
        {
            Argument.Ensure(text, "text");
            Argument.Ensure(word, "word");

            text = " " + text + " ";
            text = text.Replace(" " + word + " ", " ");

            text = text.Trim();
            return text;
        }

        public static string ReplaceLastWord(this string text, string newLastWord)
        {
            if (!text.Contains(' ') || text.Length == 0)
                return newLastWord;

            return text.Substring(0, text.LastIndexOf(' ')) + " " + newLastWord;
        }

        public static string TryRemoveUselessParantheses(this string text)
        {
            string newText = text;

            if (newText[0] == '(' && newText[newText.Length - 1] == ')')
                newText = newText.Substring(1, newText.Length - 2).Trim();

            Dictionary<int, int> depthMap = newText.GetParantheseDepthMap();

            foreach (int depth in depthMap.Values)
                if (depth < 0)
                    return text;

            if (newText.Length < text.Length)
                newText = TryRemoveUselessParantheses(newText);

            return newText;
        }

        public static string ReplaceFirstOccurenceOf(this string text, string from, string to)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;
            if (String.IsNullOrEmpty(from))
                return text;
            if (String.IsNullOrEmpty(to))
                to = String.Empty;
            int loc = text.IndexOf(from);
            return text.Remove(loc, from.Length).Insert(loc, to);
        }

        public static string ReplaceFirstOccurenceOfWord(this string text, string from, string to)
        {
            if (!text.Contains(from))
                return text;
            else if (!text.ContainsWord(from))
                return text;

            string[] words = text.Split(' ');

            string newText = "";
            bool foundWord = false;
            foreach (string word in words)
            {
                if (word == from && !foundWord)
                {
                    foundWord = true;
                    newText += " " + to;
                }
                else
                {
                    newText += " " + word;
                }
            }

            newText = newText.Trim();
            return newText;
        }

        public static bool ContainsWord(this string text, string word)
        {
            Argument.Ensure(text, "text");
            Argument.Ensure(word, "word");

            bool containsWord = false;
            text = " " + text + " ";
            if (text.Contains(" " + word + " "))
                containsWord = true;
            text = text.Trim();
            return containsWord;
        }

        public static string GetLastWord(this string text)
        {
            if (text.Length == 0)
                return null;

            if (!text.Contains(' '))
                return text;

            string[] words = text.Split(' ');

            return words[words.Length - 1];
        }

        public static string GetRandomString()
        {
            string randomString = "";
            int length = random.Next(5, 40);

            for (int i = 0; i < length; i++)
            {
                randomString += (char)random.Next(97, 123);
            }

            return randomString;
        }

        public static Dictionary<int, int> GetParantheseDepthMap(this string text)
        {
            Dictionary<int, int> depthMap = new Dictionary<int, int>();
            int currentDepth = 0;
            int counter = 0;
            foreach (char character in text)
            {
                if (character == '(')
                    currentDepth++;
                else if (character == ')')
                    currentDepth--;

                depthMap[counter] = currentDepth;

                counter++;
            }

            if (currentDepth != 0)
                throw new Exception("Parantheses not closed properly");

            return depthMap;
        }

        public static string GetWordBeforeFirstOccurenceOfWord(this string text, string wordToFind)
        {
            if (!text.Contains(' '))
                return null;

            string[] words = text.Split(' ');

            string wordBefore = null;

            foreach (string currentWord in words)
            {
                if (currentWord == wordToFind)
                    return wordBefore;

                wordBefore = currentWord;
            }
            return null;
        }

        public static int CountEndingChar(this string text, string character)
        {
            int count = 0;
            while (text.EndsWith(character) && text.Length > 1)
            {
                text = text.Substring(0, text.Length - 1);
                count++;
            }
            return count;
        }

        public static int CountStartingChar(this string text, string character)
        {
            int count = 0;
            while (text.StartsWith(character) && text.Length > 1)
            {
                text = text.Substring(1);
                count++;
            }
            return count;
        }

        public static int CountWord(this string text)
        {
            text = text.FixStringForHimmlStatementParsing();
            string[] wordList = text.Split(' ');
            return wordList.Length;
        }

        public static string ReplaceWord(this string text, string from, string to)
        {
            from = from.Trim();
            to = to.Trim();

            text = " " + text + " ";

            text = text.Replace(" " + from + " ", " " + to + " ");

            return text.Trim();
        }
        #endregion
    }
}
