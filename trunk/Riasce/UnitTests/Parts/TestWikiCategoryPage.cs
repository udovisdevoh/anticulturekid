using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestWikiCategoryPage
    {
        public static void Test()
        {
            TestCommodore64games();
            TestBritishCivilServants();
        }

        private static void TestBritishCivilServants()
        {
            WikiCategoryPage wikiCategoryPage = new WikiCategoryPage("British_civil_servants");

            HashSet<string> elementNameList = wikiCategoryPage.GetElementNameList();

            if (elementNameList.Count < 30)
                throw new Exception("List is too small");

            if (!elementNameList.Contains("george_alberti"))
                throw new Exception("Doesn't contain george_alberti");
        }

        private static void TestCommodore64games()
        {
            WikiCategoryPage wikiCategoryPage = new WikiCategoryPage("Commodore_64_games");

            HashSet<string> elementNameList = wikiCategoryPage.GetElementNameList();

            if (elementNameList.Count < 800)
                throw new Exception("List is too small");

            if (!elementNameList.Contains("3d_tanx"))
                throw new Exception("Doesn't contain 3d_tanx");
        }
    }
}
