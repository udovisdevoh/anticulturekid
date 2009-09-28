using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Word matrix extractor
    /// </summary>
    class WordMatrixExtractor
    {
        #region Public Methods
        /// <summary>
        /// Create a word pair occurence matrix from text file
        /// </summary>
        /// <param name="textFileName">text file name</param>
        /// <returns>word pair occurence matrix from text file</returns>
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
        #endregion

        #region Private Methods
        /// <summary>
        /// Learn from line
        /// </summary>
        /// <param name="matrix">matrix to add information to</param>
        /// <param name="line">line to learn from</param>
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
