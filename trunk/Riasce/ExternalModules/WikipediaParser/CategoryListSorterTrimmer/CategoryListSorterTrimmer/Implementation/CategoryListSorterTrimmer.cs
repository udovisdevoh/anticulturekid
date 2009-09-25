using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AntiCulture.Kid
{
    class CategoryListSorterTrimmer : AbstractCategoryListSorterTrimmer
    {
        #region Public Methods
        public override void SortAndTrimCategoryListFile(string inputFileName, string outputFileName)
        {
            List<SortableCategory> categoryList = GetCategoryListFromFile(inputFileName);
            categoryList.Sort();
            categoryList = TrimCategoryList(categoryList);
            WriteOutputFile(categoryList, outputFileName);
        }
        #endregion

        #region Private Methods
        private List<SortableCategory> GetCategoryListFromFile(string fileName)
        {
            List<SortableCategory> categoryList = new List<SortableCategory>();

            using (StreamReader streamReader = new StreamReader(fileName))
            {
                string line = null;
                int count;
                string name;
                do
                {
                    line = streamReader.ReadLine();
                    if (line == null || !line.Contains(" : "))
                        break;
                    name = line.Substring(0, line.IndexOf(':')).Trim();
                    count = int.Parse(line.Substring(line.IndexOf(':') + 1).Trim());
                    categoryList.Add(new SortableCategory(count,name));
                } while (line != null);
            }


            return categoryList;
        }

        private void WriteOutputFile(List<SortableCategory> categoryList, string fileName)
        {
            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                foreach (SortableCategory sortableCategory in categoryList)
                {
                    streamWriter.Write(sortableCategory.Name + " : " + sortableCategory.Count + "\r\n");
                }
            }
        }

        private List<SortableCategory> TrimCategoryList(List<SortableCategory> categoryList)
        {
            List<SortableCategory> newCategoryList = new List<SortableCategory>();
            List<string> forbiddenStringList = new List<string>() { "article", "stub", "redirect" };
            
            foreach (SortableCategory currentCategory in categoryList)
                if (!InList(currentCategory.Name, forbiddenStringList) && currentCategory.Count < 1000)
                    newCategoryList.Add(currentCategory);

            return newCategoryList;
        }

        private bool InList(string name, List<string> stringList)
        {
            name = name.ToLower();
            foreach (string forbiddenString in stringList)
                if (name.Contains(forbiddenString))
                    return true;

            return false;
        }
        #endregion
    }
}
