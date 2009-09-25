using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractMctFromConnection
    {
        /// <summary>
        /// Returns the best metaConnection theory about operator
        /// </summary>
        /// <param name="verb">operator</param>
        /// <returns>the best metaConnection theory about operator, null if nothing found</returns>
        public abstract Theory GetBestTheoryAboutOperator(Concept verb);

        /// <summary>
        /// Returns the best metaConnection theory from operators in memory
        /// </summary>
        /// <returns>the best metaConnection theory from operators in memory, null if nothing found</returns>
        public abstract Theory GetBestTheoryAboutOperatorsInMemory();
    }
}
