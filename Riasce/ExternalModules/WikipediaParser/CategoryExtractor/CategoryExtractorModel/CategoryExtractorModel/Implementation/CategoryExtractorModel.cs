using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class CategoryExtractorModel : AbstractCategoryExtractorModel
    {
        #region Fields
        private string sourceFileName;

        private string checkListFileName;

        private string currentCategoryName;

        private CategoryCheckList __categoryCheckList;//Lazy initialization, do not use directly

        private SourceCategoryList __sourceCategoryList;//Lazy initialization, do not use directly
        #endregion

        #region Public Methods
        public override string GetNextCategoryName()
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

        public override string GetPreviousCategoryName()
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

        public override HashSet<string> GetCategoryElementNameList(string categoryName)
        {
            WikiCategoryPage wikiCategoryPage = new WikiCategoryPage(categoryName);
            return wikiCategoryPage.GetElementNameList();
        }

        public override void MarkAsDone(string categoryName)
        {
            categoryCheckList.Add(categoryName);
        }
        #endregion

        #region Properties
        public string SourceFileName
        {
            get { return sourceFileName; }
            set { sourceFileName = value; }
        }

        public string CheckListFileName
        {
            get { return checkListFileName; }
            set { checkListFileName = value; }
        }

        public string CurrentCategoryName
        {
            get { return currentCategoryName; }
            set { currentCategoryName = value; }
        }

        private CategoryCheckList categoryCheckList
        {
            get
            {
                if (__categoryCheckList == null)
                    __categoryCheckList = new CategoryCheckList(checkListFileName);
                return __categoryCheckList;
            }
        }

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
