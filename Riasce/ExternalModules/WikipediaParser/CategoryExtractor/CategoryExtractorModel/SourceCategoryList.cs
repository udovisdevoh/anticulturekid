using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Source category list
    /// </summary>
    class SourceCategoryList
    {
        #region Fields
        /// <summary>
        /// Element list
        /// </summary>
        private List<string> elementList;

        /// <summary>
        /// Current index
        /// </summary>
        private int currentIndex = -1;
        #endregion

        #region Constructor
        public SourceCategoryList(string fileName)
        {
            elementList = new List<string>();

            string line;
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                do
                {
                    line = streamReader.ReadLine();
                    if (line != null)
                        elementList.Add(line.Substring(0, line.IndexOf(": ")).Trim());
                } while (line != null);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get next element
        /// </summary>
        /// <returns>Next element or null if no more element available</returns>
        public string GetNext()
        {
            if (elementList.Count == 0)
                return null;

            if (currentIndex < elementList.Count -1)
                currentIndex++;         

            return elementList[currentIndex];
        }

        /// <summary>
        /// Get previous element
        /// </summary>
        /// <returns>Previous element or null if no more element available</returns>
        public string GetPrevious()
        {
            if (elementList.Count == 0)
                return null;

            if (currentIndex > 0)
                currentIndex--;

            return elementList[currentIndex];
        }

        /// <summary>
        /// Whether it's possible to call GetNext()
        /// </summary>
        /// <returns>Whether it's possible to call GetNext()</returns>
        public bool HasNext()
        {
            return currentIndex < elementList.Count - 1;
        }

        /// <summary>
        /// Whether it's possible to call GetPrevious()
        /// </summary>
        /// <returns>Whether it's possible to call GetPrevious()</returns>
        public bool HasPrevious()
        {
            return currentIndex > 0;
        }
        #endregion
    }
}
