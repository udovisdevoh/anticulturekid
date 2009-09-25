using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractLetterMatrix
    {
        /// <summary>
        /// Learn from the word or sentence and add statistics to the matrix
        /// </summary>
        /// <param name="source">source word or sentence</param>
        public abstract void Learn(string source);

        /// <summary>
        /// Learn from the word group and add content to the matrix
        /// </summary>
        /// <param name="source">word group</param>
        public abstract void Learn(IEnumerable<string> source);

        /// <summary>
        /// Return a random word from matrix which will not be longer than longest word yet
        /// </summary>
        /// <returns>a random word from matrix</returns>
        public abstract string GenerateWord();
    }
}
