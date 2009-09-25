using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riasce
{
    abstract class AbstractCategoryListExtractorModel
    {
        /// <summary>
        /// Extract info from wiki categories from next chunk into file name
        /// </summary>
        /// <param name="chunkSize">chunk size</param>
        /// <param name="fileName">file name</param>
        /// <returns>true if must continue to extract, false if must stop</returns>
        public abstract bool AppendNextChunkToFile(int chunkSize, string fileName);
    }
}
