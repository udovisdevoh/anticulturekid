using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Riasce
{
    class CategoryCheckList : AbstractCategoryCheckList
    {
        #region Fields
        private HashSet<string> nameList;

        private string fileName;
        #endregion

        #region Constructor
        public CategoryCheckList(string fileName)
        {
            this.fileName = fileName;
            nameList = new HashSet<string>();

            if (!File.Exists(fileName))
                return;
            
            string line;
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                do
                {
                    line = streamReader.ReadLine();
                    if (line != null)
                        nameList.Add(line);
                } while (line != null);
            }
        }
        #endregion

        #region Public Methods
        public override bool Contains(string elementName)
        {
            return nameList.Contains(elementName);
        }

        public override void Add(string elementName)
        {
            if (elementName == null || elementName.Trim() == "")
                return;

            if (!nameList.Contains(elementName))
            {
                nameList.Add(elementName);

                using (StreamWriter streamWriter = new StreamWriter(fileName,true))
                {
                    streamWriter.Write(elementName + "\r\n");
                }
            }
        }
        #endregion
    }
}