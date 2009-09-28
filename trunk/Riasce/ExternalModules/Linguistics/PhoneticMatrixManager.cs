using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Build and manage phonetic matrixes
    /// </summary>
    class PhoneticMatrixManager
    {
        #region Public Constants
        /// <summary>
        /// Max word in phonetic matrix by starting value
        /// </summary>
        private static readonly int maxToWordCount = 60;
        #endregion

        #region Public Methods
        /// <summary>
        /// Create a word phonetic proximity matrix from text file
        /// </summary>
        /// <param name="textFileName">text file name</param>
        /// <returns>word phonetic proximity matrix from text file</returns>
        public Matrix BuildMatrixFromTextFile(string textFileName)
        {
            Matrix matrix = new Matrix();
            string line;
            using (StreamReader file = new StreamReader(textFileName))
            {
                while ((line = file.ReadLine()) != null)
                    LearnFromLine(matrix, line);
            }

            return matrix;
        }

        /// <summary>
        /// Add a new word to phonetic matrix
        /// </summary>
        /// <param name="matrix">matrix to add new word to</param>
        /// <param name="word">word</param>
        public void LearnNewWord(Matrix matrix, string word)
        {
            float comparison;
            if (matrix.ContainsKey(word))
                return;

            matrix.SetStatistics(word, word, 0);

            foreach (string otherWord in matrix.NormalData.Keys)
            {
                comparison = ComparePhonemeOccurence(word, otherWord);
                matrix.SetStatistics(word, otherWord, comparison);
                matrix.SetStatistics(otherWord, word, comparison);
            }

            Reduce(matrix);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Learn information from text line
        /// </summary>
        /// <param name="matrix">matrix to add information to</param>
        /// <param name="line">text line</param>
        private void LearnFromLine(Matrix matrix, string line)
        {
            line = line.Replace("-", "_");
            line = line.Replace("?", "");
            line = line.Replace("(", "");
            line = line.Replace(")", "");
            line = line.Replace("’", "'");
            line = line.Replace("'", " ");

            while (line.Contains("__"))
                line = line.Replace("__", "_");

            line = line.RemoveWord("_");

            line = line.FixStringForHimmlStatementParsing();

            string[] wordList = line.Split(' ');

            foreach (string currentWord in wordList)
                LearnNewWord(matrix, currentWord);
        }

        /// <summary>
        /// Compare phoneme occurence
        /// </summary>
        /// <param name="word1">word 1</param>
        /// <param name="word2">word 2</param>
        /// <returns>phoneme occurence proximity (from 0 to 1)</returns>
        private float ComparePhonemeOccurence(string word1, string word2)
        {
            float occurence = 0;

            HashSet<string> totalPhonemeList = GetTotalPhonemeList(word1,word2);
            foreach (string phoneme in totalPhonemeList)
                if (word1.Contains(phoneme) && word2.Contains(phoneme))
                    occurence++;

            if (totalPhonemeList.Count > 0)
                occurence /= totalPhonemeList.Count;
            else
                occurence = 0;

            return occurence;
        }

        /// <summary>
        /// Get total phoneme list from two words
        /// </summary>
        /// <param name="word1">word 1</param>
        /// <param name="word2">word 2</param>
        /// <returns>total phoneme list from for words</returns>
        private HashSet<string> GetTotalPhonemeList(string word1, string word2)
        {
            HashSet<string> totalPhonemeList1 = GetTotalPhonemeList(word1);
            HashSet<string> totalPhonemeList2 = GetTotalPhonemeList(word2);

            totalPhonemeList1.UnionWith(totalPhonemeList2);

            return totalPhonemeList1;
        }

        /// <summary>
        /// Get total phoneme list from provided word
        /// </summary>
        /// <param name="word">word</param>
        /// <returns>total phoneme list int provided word</returns>
        private HashSet<string> GetTotalPhonemeList(string word)
        {
            HashSet<string> totalPhonemeList = new HashSet<string>();

            string currentPhoneme = "";
            foreach (char letter in word)
            {
                currentPhoneme += letter;

                if (currentPhoneme.Length == 4)
                    currentPhoneme = currentPhoneme.Substring(1);

                if (currentPhoneme.Length == 3)
                    totalPhonemeList.Add(currentPhoneme);
            }

            return totalPhonemeList;
        }

        /// <summary>
        /// Remove unsignificant information from matrix
        /// </summary>
        /// <param name="matrix">matrix to reduce</param>
        private void Reduce(Matrix matrix)
        {
            matrix.NormalData = Reduce(matrix.NormalData);
            matrix.ReversedData = Reduce(matrix.ReversedData);
        }

        /// <summary>
        /// RRemove unsignificant information from matrix
        /// </summary>
        /// <param name="data">matrix's data</param>
        /// <returns>reduced data</returns>
        private Dictionary<string, Dictionary<string, float>> Reduce(Dictionary<string, Dictionary<string, float>> data)
        {
            string fromWord;
            Dictionary<string, float> row;
            Dictionary<string, Dictionary<string, float>> newData = new Dictionary<string, Dictionary<string, float>>();
            IEnumerable<KeyValuePair<string, float>> sortedRow;

            foreach (KeyValuePair<string, Dictionary<string, float>> fromWordAndRow in data)
            {
                fromWord = fromWordAndRow.Key;
                row = fromWordAndRow.Value;
                sortedRow = row.OrderByDescending(pair => pair.Value);
                sortedRow = sortedRow.Take(maxToWordCount);
                sortedRow = sortedRow.Where(pair => pair.Value > 0);
                row = sortedRow.ToDictionary(pair => pair.Key, pair => pair.Value);
                newData.Add(fromWord, row);
            }

            return newData;
        }
        #endregion
    }
}
