using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AntiCulture.Kid
{
    class TestCategoryListExtractorModel
    {
        public static void Test()
        {
            WikiCategoryListPage wikiCategoryPage = new WikiCategoryListPage();
            CategoryListExtractorModel categoryListExtractorModel = new CategoryListExtractorModel(wikiCategoryPage);

            long previousLength;
            FileInfo fileInfo = new FileInfo("TestCategoryList.txt");
            if (fileInfo.Exists)
                previousLength = fileInfo.Length;
            else
                previousLength = 0;

            categoryListExtractorModel.AppendNextChunkToFile(100, "TestCategoryList.txt");

            fileInfo = new FileInfo("TestCategoryList.txt");
            if (fileInfo.Length <= previousLength)
                throw new Exception("File should be bigger now");
        }
    }
}
