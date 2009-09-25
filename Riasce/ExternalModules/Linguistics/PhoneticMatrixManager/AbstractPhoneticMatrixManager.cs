using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractPhoneticMatrixManager
    {
        /// <summary>
        /// Create a word phonetic proximity matrix from text file
        /// </summary>
        /// <param name="textFileName">text file name</param>
        /// <returns>word phonetic proximity matrix from text file</returns>
        public abstract Matrix BuildMatrixFromTextFile(string textFileName);

        /// <summary>
        /// Add a new word to phonetic matrix
        /// </summary>
        /// <param name="matrix">matrix to add new word to</param>
        /// <param name="word">word</param>
        public abstract void LearnNewWord(Matrix matrix, string word);
    }
}
