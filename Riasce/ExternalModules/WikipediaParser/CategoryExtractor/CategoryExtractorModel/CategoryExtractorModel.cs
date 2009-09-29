using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Category extractor model
    /// </summary>
    class CategoryExtractorModel
    {
        #region Fields
        /// <summary>
        /// Source file name
        /// </summary>
        private string sourceFileName;

        /// <summary>
        /// Checklist file name
        /// </summary>
        private string checkListFileName;

        /// <summary>
        /// Current category name
        /// </summary>
        private string currentCategoryName;

        /// <summary>
        /// Lazy initialization, do not use directly, use without underscore instead
        /// </summary>
        private CategoryCheckList __categoryCheckList;//Lazy initialization, do not use directly

        /// <summary>
        /// Lazy initialization, do not use directly, use without underscore instead
        /// </summary>
        private SourceCategoryList __sourceCategoryList;//Lazy initialization, do not use directly
        #endregion

        #region Public Methods
        /// <summary>
        /// Get the next category name to extract
        /// </summary>
        /// <returns>next category name to extract</returns>
        public string GetNextCategoryName()
        {
            string categoryName;
            do
            {
                categoryName = sourceCategoryList.GetNext();
            } while (categoryCheckList.Contains(categoryName) && categoryName != null && sourceCategoryList.HasNext());

            if (categoryCheckList.Contains(categoryName))
                return null;
            else
                return categoryName;
        }

        /// <summary>
        /// Get the previous category name to extract
        /// </summary>
        /// <returns>previous category name to extract</returns>
        public string GetPreviousCategoryName()
        {
            string categoryName;
            do
            {
                categoryName = sourceCategoryList.GetPrevious();
            } while (categoryCheckList.Contains(categoryName) && categoryName != null && sourceCategoryList.HasPrevious());

            if (categoryCheckList.Contains(categoryName))
                return null;
            else
                return categoryName;
        }

        /// <summary>
        /// Get the elements from provided category
        /// </summary>
        /// <param name="categoryName">category name</param>
        /// <returns>the elements in provided category</returns>
        public HashSet<string> GetCategoryElementNameList(string categoryName)
        {
            WikiCategoryPage wikiCategoryPage = new WikiCategoryPage(categoryName);
            return wikiCategoryPage.GetElementNameList();
        }

        /// <summary>
        /// Add category name to check list (once it's applied or ignored)
        /// </summary>
        /// <param name="categoryName">category name</param>
        public void MarkAsDone(string categoryName)
        {
            categoryCheckList.Add(categoryName);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Source file name
        /// </summary>
        public string SourceFileName
        {
            get { return sourceFileName; }
            set { sourceFileName = value; }
        }

        /// <summary>
        /// Checklist file name
        /// </summary>
        public string CheckListFileName
        {
            get { return checkListFileName; }
            set { checkListFileName = value; }
        }

        /// <summary>
        /// Current category name
        /// </summary>
        public string CurrentCategoryName
        {
            get { return currentCategoryName; }
            set { currentCategoryName = value; }
        }

        /// <summary>
        /// Category checklist
        /// </summary>
        private CategoryCheckList categoryCheckList
        {
            get
            {
                if (__categoryCheckList == null)
                    __categoryCheckList = new CategoryCheckList(checkListFileName);
                return __categoryCheckList;
            }
        }

        /// <summary>
        /// Source category list
        /// </summary>
        private SourceCategoryList sourceCategoryList
        {
            get
            {
                if (__sourceCategoryList == null)
                    __sourceCategoryList = new SourceCategoryList(sourceFileName);
                return __sourceCategoryList;
            }
        }
        #endregion
    }
}
