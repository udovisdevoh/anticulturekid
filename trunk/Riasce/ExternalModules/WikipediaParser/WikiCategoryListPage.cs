using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Wikipedia category list page
    /// </summary>
    class WikiCategoryListPage
    {
        #region Fields
        /// <summary>
        /// Url
        /// </summary>
        private string url;
        #endregion

        #region Contructor
        public WikiCategoryListPage()
        {
            this.url = "http://en.wikipedia.org/w/index.php?title=Special:Categories";
        }

        public WikiCategoryListPage(string url)
        {
            this.url = url;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Get the list of category from wiki
        /// </summary>
        /// <param name="chunkSize">chunk size</param>
        /// <param name="offset">offset name</param>
        /// <returns>the list of category from wiki</returns>
        public HashSet<string> GetCategoryList(int chunkSize, string offset)
        {
            string formattedChunkSize = chunkSize.ToString();
            if (formattedChunkSize.Length == 4)
                formattedChunkSize = "" + formattedChunkSize[0] + ',' + formattedChunkSize[1] + formattedChunkSize[2] + formattedChunkSize[3];

            HashSet<string> categoryList = new HashSet<string>();

            if (offset == null)
                offset = "";

            string pageContent = WebExplorer.GetStringPageContent(url + "&limit=" + chunkSize + "&offset=" + offset);
            int indexOfPrevious = pageContent.IndexOf("previous " + formattedChunkSize);
            pageContent = pageContent.Substring(indexOfPrevious);
            pageContent = pageContent.Substring(pageContent.IndexOf("<ul>") + 4);
            pageContent = pageContent.Substring(0, pageContent.IndexOf("</ul>"));

            string[] lineList = pageContent.Split('\n');
            string chunkLine;
            foreach (string line in lineList)
            {
                chunkLine = FormatChunkLine(line);
                if (chunkLine != null)
                {
                    categoryList.Add(chunkLine);
                }
            }

            return categoryList;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Format chunk line
        /// </summary>
        /// <param name="line">line</param>
        /// <returns>formated chunk line</returns>
        private string FormatChunkLine(string line)
        {
            if (line.Contains("index.php") || line.Contains("&quot;") || line.Length < 9 || line.Contains("%"))
                return null;

            string cardinality;
            string name;


            name = line.Substring(line.IndexOf("Category:") + 9);
            name = name.Substring(0,name.IndexOf('"'));

            if (name.Contains(':'))
                return null;

            cardinality = line.Substring(line.LastIndexOf('(') + 1);
            cardinality = cardinality.Substring(0, cardinality.IndexOf(' '));
            cardinality = cardinality.Replace(",", "");

            return name + " : " + cardinality;
        }
        #endregion
    }
}
