using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riasce
{
    class WikiCategoryPage : AbstractWikiCategoryPage
    {
        #region Fields
        private static readonly string defaultUrl = "http://en.wikipedia.org/w/index.php?title=Category:";
        
        private string categoryName;
        #endregion

        #region Constructor
        public WikiCategoryPage(string categoryName)
        {
            this.categoryName = categoryName;
        }
        #endregion

        #region Public Methods
        public override HashSet<string> GetElementNameList()
        {
            HashSet<string> elementNameList = GetFlatElementList();
            /*
            HashSet<string> subCategoryNameList = GetSubCategoryNameList();

            foreach (string subCategoryName in subCategoryNameList)
                elementNameList.UnionWith(new WikiCategoryPage(subCategoryName).GetElementNameList());
            */

            return elementNameList;
        }
        #endregion

        #region Private Methods
        private HashSet<string> GetSubCategoryNameList()
        {
            HashSet<string> subCategoryNameList = new HashSet<string>();
            string pageContent = WebExplorer.GetStringPageContent(defaultUrl + categoryName);

            if (!pageContent.Contains("<h2>Subcategories</h2>"))
                return subCategoryNameList;

            pageContent = pageContent.Substring(pageContent.IndexOf("<h2>Subcategories</h2>"));
            pageContent = pageContent.Replace("\n", "");
            pageContent = pageContent.Replace("\r", "");
            pageContent = pageContent.Replace("<li>", "\n");

            string[] lineList = pageContent.Split('\n');

            string trimmedLine;
            foreach (string line in lineList)
            {
                trimmedLine = line.Trim();

                if (trimmedLine == "")
                    continue;

                if (!trimmedLine.Contains("href=\"/wiki/Category:"))
                    continue;

                trimmedLine = trimmedLine.Substring(trimmedLine.IndexOf("href=\"/wiki/Category:") + 21);
                trimmedLine = trimmedLine.Substring(0, trimmedLine.IndexOf('"'));

                if (trimmedLine.Contains(':'))
                    continue;

                if (trimmedLine.Contains('%'))
                    continue;

                if (trimmedLine.Contains("&quot;"))
                    continue;

                if (trimmedLine.Contains("List_of"))
                    continue;

                subCategoryNameList.Add(trimmedLine);
            }

            return subCategoryNameList;
        }

        private HashSet<string> GetFlatElementList()
        {
            return GetFlatElementList("");
        }

        private HashSet<string> GetFlatElementList(string from)
        {
            HashSet<string> elementList = new HashSet<string>();
            string pageContent = WebExplorer.GetStringPageContent(defaultUrl + categoryName + "&from=" + from);

            pageContent = pageContent.Substring(pageContent.LastIndexOf("<h2>"));
            pageContent = pageContent.Substring(pageContent.IndexOf("<ul>") + 4);

            pageContent = pageContent.Replace("\n", "");
            pageContent = pageContent.Replace("\r", "");
            pageContent = pageContent.Replace("<li>", "\n");

            string[] lineList = pageContent.Split('\n');

            string formatedLine;
            foreach (string line in lineList)
            {
                if (line.Trim() != "")
                {
                    formatedLine = line.Substring(line.IndexOf("<a href=\"/wiki/")+15);
                    formatedLine = formatedLine.Substring(0, formatedLine.IndexOf('"'));
                    formatedLine = formatedLine.Replace(".", "");
                    formatedLine = formatedLine.Replace("*", "");
                    formatedLine = formatedLine.Replace("/", "");
                    formatedLine = formatedLine.Replace("-", "_");
                    formatedLine = formatedLine.Replace(",", "");
                    formatedLine = formatedLine.Replace("__", "_");
                    formatedLine = formatedLine.ToLower();

                    if (formatedLine.Contains(':'))
                        continue;

                    if (formatedLine.Contains('%'))
                        continue;

                    if (formatedLine.Contains("&quot;"))
                        continue;

                    if (formatedLine.Contains("list_of"))
                        continue;


                    elementList.Add(formatedLine);
                }
            }

            string nextOffsetName = GetNextOffsetName(pageContent);

            if (nextOffsetName != null)
                elementList.UnionWith(GetFlatElementList(nextOffsetName));

            return elementList;
        }

        private string GetNextOffsetName(string textZone)
        {
            if (!textZone.Contains("next 200</a>"))
                return null;

            textZone = textZone.Substring(0,textZone.IndexOf("next 200"));
            textZone = textZone.Substring(textZone.LastIndexOf("from=") + 5);
            textZone = textZone.Substring(0,textZone.IndexOf('"'));

            return textZone;
        }
        #endregion
    }
}
