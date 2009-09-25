using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Text;

namespace AntiCulture.Kid
{
    class WordMatrixExtractor : AbstractWordMatrixExtractor
    {
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
        #endregion

        #region Private Strings
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

            string previousWord = null;
            foreach (string currentWord in wordList)
            {
                if (previousWord != null)
                    matrix.AddStatistics(previousWord, currentWord);

                previousWord = currentWord;
            }
        }
        #endregion
    }
}
