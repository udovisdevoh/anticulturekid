using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riasce
{
    abstract class AbstractWikiCategoryPage
    {
        /// <summary>
        /// Extract (recursively) article name list from wikipedia category
        /// </summary>
        /// <returns>article name list from wikipedia category</returns>
        public abstract HashSet<string> GetElementNameList();
    }
}
