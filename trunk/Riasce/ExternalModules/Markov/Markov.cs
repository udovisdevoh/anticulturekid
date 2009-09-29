using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Used for markov chain and random word generators
    /// </summary>
    static class Markov
    {
        /// <summary>
        /// Generate random word
        /// </summary>
        /// <param name="wordList">word list</param>
        /// <param name="desiredLength">desired length</param>
        /// <returns>randomly generated word word</returns>
        public static string GenerateRandomWord(IEnumerable<string> wordList, int desiredLength)
        {
            LetterMatrix letterMatrix = new LetterMatrix(wordList);
            return letterMatrix.GenerateWord(desiredLength);
        }
    }
}
