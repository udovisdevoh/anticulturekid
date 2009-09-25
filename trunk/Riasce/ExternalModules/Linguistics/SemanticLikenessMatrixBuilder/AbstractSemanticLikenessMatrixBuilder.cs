using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractSemanticLikenessMatrixBuilder
    {
        /// <summary>
        /// Build semantic likeness matrix from occurence matrix
        /// </summary>
        /// <param name="occurenceMatrix">word occurence matrix</param>
        /// <returns>semantic likeness matrix</returns>
        public abstract Matrix BuildAndBreakSourceMatrix(Matrix occurenceMatrix);

        /// <summary>
        /// Build semantic likeness matrix from occurence matrix
        /// </summary>
        /// <param name="occurenceMatrix">word occurence matrix</param>
        /// <param name="sourceWordCount">max source word count</param>
        /// <param name="targetWordCount">max target word count</param>
        /// <returns>semantic likeness matrix</returns>
        public abstract Matrix Build(Matrix occurenceMatrix, int sourceWordCount, int targetWordCount);
    }
}
