using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    static class Markov
    {
        public static string GenerateRandomWord(IEnumerable<string> wordList, int desiredLength)
        {
            LetterMatrix letterMatrix = new LetterMatrix(wordList);
            return letterMatrix.GenerateWord(desiredLength);
        }
    }
}
