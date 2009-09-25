using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractCategoryListSorterTrimmer
    {
        /// <summary>
        /// Sort and trim a category list file
        /// </summary>
        /// <param name="inputFileName">input file name</param>
        /// <param name="outputFileName">output file name</param>
        public abstract void SortAndTrimCategoryListFile(string inputFileName, string outputFileName);
    }
}
