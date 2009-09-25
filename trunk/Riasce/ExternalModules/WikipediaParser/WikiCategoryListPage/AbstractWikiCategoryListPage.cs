using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractWikiCategoryListPage
    {
        /// <summary>
        /// Get the list of category from wiki
        /// </summary>
        /// <param name="chunkSize">chunk size</param>
        /// <param name="offset">offset name</param>
        /// <returns>the list of category from wiki</returns>
        public abstract HashSet<string> GetCategoryList(int chunkSize, string offset);
    }
}
