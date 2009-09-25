using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Text;

namespace AntiCulture.Kid
{
    class PhoneticMatrixManager : AbstractPhoneticMatrixManager
    {
        #region Public Constants
        private static readonly int maxToWordCount = 60;
        #endregion

        #region Public Methods
        public override Matrix BuildMatrixFromTextFile(string textFileName)
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

        public override void LearnNewWord(Matrix matrix, string word)
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

        private HashSet<string> GetTotalPhonemeList(string word1, string word2)
        {
            HashSet<string> totalPhonemeList1 = GetTotalPhonemeList(word1);
            HashSet<string> totalPhonemeList2 = GetTotalPhonemeList(word2);

            totalPhonemeList1.UnionWith(totalPhonemeList2);

            return totalPhonemeList1;
        }

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

        private void Reduce(Matrix matrix)
        {
            matrix.NormalData = Reduce(matrix.NormalData);
            matrix.ReversedData = Reduce(matrix.ReversedData);
        }

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
