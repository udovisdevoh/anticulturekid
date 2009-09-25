using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractCategoryExtractorModel
    {
        /// <summary>
        /// Get the next category name to extract
        /// </summary>
        /// <returns>next category name to extract</returns>
        public abstract string GetNextCategoryName();

        /// <summary>
        /// Get the previous category name to extract
        /// </summary>
        /// <returns>previous category name to extract</returns>
        public abstract string GetPreviousCategoryName();

        /// <summary>
        /// Get the elements from provided category
        /// </summary>
        /// <param name="categoryName">category name</param>
        /// <returns>the elements in provided category</returns>
        public abstract HashSet<string> GetCategoryElementNameList(string categoryName);

        /// <summary>
        /// Add category name to check list (once it's applied or ignored)
        /// </summary>
        /// <param name="categoryName">category name</param>
        public abstract void MarkAsDone(string categoryName);
    }
}
