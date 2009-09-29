using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Category list extractor's model
    /// </summary>
    class CategoryListExtractorModel
    {
        #region Fields
        /// <summary>
        /// Wikipedia category page
        /// </summary>
        private WikiCategoryListPage wikiCategoryPage;       
        #endregion

        #region Constructor
        public CategoryListExtractorModel(WikiCategoryListPage wikiCategoryPage)
        {
            this.wikiCategoryPage = wikiCategoryPage;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Extract info from wiki categories from next chunk into file name
        /// </summary>
        /// <param name="chunkSize">chunk size</param>
        /// <param name="fileName">file name</param>
        /// <returns>true if must continue to extract, false if must stop</returns>
        public bool AppendNextChunkToFile(int chunkSize, string fileName)
        {
            if (!File.Exists(fileName))
                using (File.Create(fileName)) { }

            string firstCategoryName = GetCategoryNameFromEntry(CategoryListFileExplorer.GetFirstEntry(fileName));
            string offsetName = GetCategoryNameFromEntry(CategoryListFileExplorer.GetLastEntry(fileName));

            HashSet<string> chunk = DownloadChunk(chunkSize, offsetName);

            if (chunk.Count == 0)
                return false;

            AppendChunkToFile(chunk, fileName);

            if (firstCategoryName != null && ContainsCategoryName(chunk, firstCategoryName)) 
                return false;
            else
                return true;
        }

        /// <summary>
        /// Get progress percentage
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>progress percentage</returns>
        public double GetProgressPercentage(string fileName)
        {
            string lastWord = GetCategoryNameFromEntry(CategoryListFileExplorer.GetLastEntry(fileName));

            char firstCharOfLastWord = lastWord.ToLower()[0];

            List<char> charNameList = new List<char>();
            for (char i = (char)48; i < (char)58; i++)
                charNameList.Add(i);
            for (char i = (char)97; i < (char)123; i++)
                charNameList.Add(i);

            int counter = 0;
            foreach (char currentChar in charNameList)
            {
                if (currentChar == firstCharOfLastWord)
                    break;
                counter++;
            }

            return (double)(counter) / (double)(charNameList.Count) * 100;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Get category name from entry
        /// </summary>
        /// <param name="entry">entry</param>
        /// <returns>category name from entry</returns>
        private string GetCategoryNameFromEntry(string entry)
        {
            if (entry == null)
                return null;
            else
                return entry.Substring(0, entry.IndexOf(": ")).Trim();
        }

        /// <summary>
        /// Contains category name
        /// </summary>
        /// <param name="chunk">chunk</param>
        /// <param name="categoryName">category name</param>
        /// <returns>whether chunk contains category name</returns>
        private bool ContainsCategoryName(HashSet<string> chunk, string categoryName)
        {
            foreach (string element in chunk)
                if (GetCategoryNameFromEntry(element) == categoryName)
                    return true;

            return false;
        }

        /// <summary>
        /// Append chunk to file
        /// </summary>
        /// <param name="chunk">chunk</param>
        /// <param name="fileName">file name</param>
        private void AppendChunkToFile(HashSet<string> chunk, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName, true))
                foreach (string element in chunk)
                    writer.Write(element + "\r\n");
        }

        /// <summary>
        /// Download chunk
        /// </summary>
        /// <param name="chunkSize">chunk size</param>
        /// <param name="offsetName">offset name</param>
        /// <returns>category members</returns>
        private HashSet<string> DownloadChunk(int chunkSize, string offsetName)
        {
            return wikiCategoryPage.GetCategoryList(chunkSize, offsetName);
        }
        #endregion
    }
}
