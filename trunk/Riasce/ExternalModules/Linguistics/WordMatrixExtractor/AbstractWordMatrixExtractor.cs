using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractWordMatrixExtractor
    {
        /// <summary>
        /// Create a word pair occurence matrix from text file
        /// </summary>
        /// <param name="textFileName">text file name</param>
        /// <returns>word pair occurence matrix from text file</returns>
        public abstract Matrix BuildMatrixFromTextFile(string textFileName);
    }
}
