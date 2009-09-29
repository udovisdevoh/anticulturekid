using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Category check list
    /// </summary>
    class CategoryCheckList
    {
        #region Fields
        /// <summary>
        /// Name List
        /// </summary>
        private HashSet<string> nameList;

        /// <summary>
        /// File name
        /// </summary>
        private string fileName;
        #endregion

        #region Constructor
        /// <summary>
        /// Create category checklist from file name
        /// </summary>
        /// <param name="fileName">file name</param>
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
        /// <summary>
        /// Whether the checklist contains element name
        /// </summary>
        /// <param name="elementName">element name</param>
        /// <returns>whether the checklist contains element name or not</returns>
        public bool Contains(string elementName)
        {
            return nameList.Contains(elementName);
        }

        /// <summary>
        /// Add element name to the checklist
        /// </summary>
        /// <param name="elementName">element name</param>
        public void Add(string elementName)
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