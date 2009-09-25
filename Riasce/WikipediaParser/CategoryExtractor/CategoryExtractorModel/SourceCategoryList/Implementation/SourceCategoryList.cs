using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Riasce
{
    class SourceCategoryList : AbstractSourceCategoryList
    {
        #region Fields
        private List<string> elementList;

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
        public override string GetNext()
        {
            if (elementList.Count == 0)
                return null;

            if (currentIndex < elementList.Count -1)
                currentIndex++;         

            return elementList[currentIndex];
        }

        public override string GetPrevious()
        {
            if (elementList.Count == 0)
                return null;

            if (currentIndex > 0)
                currentIndex--;

            return elementList[currentIndex];
        }

        public override bool HasNext()
        {
            return currentIndex < elementList.Count - 1;
        }

        public override bool HasPrevious()
        {
            return currentIndex > 0;
        }
        #endregion
    }
}
