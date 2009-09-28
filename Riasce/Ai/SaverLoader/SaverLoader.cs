using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used to save and load memory to file
    /// </summary>
    class SaverLoader
    {
        #region Static
        /// <summary>
        /// Default file type filter
        /// </summary>
        private static readonly string defaultFileTypeFilter = "XML Memory files|*.xml|Legacy Memory files|*.AiMemory";
        #endregion

        #region Fields
        /// <summary>
        /// File name
        /// </summary>
        private string fileName;

        /// <summary>
        /// Name mapper
        /// </summary>
        private NameMapper nameMapper;

        /// <summary>
        /// Memory
        /// </summary>
        private Memory memory;

        /// <summary>
        /// Total verb list
        /// </summary>
        private HashSet<Concept> totalVerbList;

        /// <summary>
        /// Rejected theories
        /// </summary>
        private RejectedTheories rejectedTheories;

        /// <summary>
        /// Total theory list
        /// </summary>
        private TheoryList totalTheoryList;

        /// <summary>
        /// Whether file needs to be saved
        /// </summary>
        private bool fileNeedSave = false;
        #endregion

        #region Constructor
        public SaverLoader()
        {
            ResetSavingParameters();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Save
        /// </summary>
        public void Save()
        {
            if (fileName == null || nameMapper == null || memory == null || totalVerbList == null || rejectedTheories == null || totalTheoryList == null)
                throw new SaveLoadException("Missing saving parameters");
            
            //Serialize stuff
            //IFormatter formatter = new BinaryFormatter();

            if (File.Exists(fileName))
                File.Copy(fileName, GetNextNewBackupName(fileName));

            MemoryDump memoryDump = new MemoryDump(nameMapper, memory, rejectedTheories, totalVerbList, totalTheoryList);

            memoryDump.SaveXmlFile(fileName);

            ResetSavingParameters();
        }

        /// <summary>
        /// Load
        /// </summary>
        public void Load()
        {
            if (fileName == null)
                throw new SaveLoadException("Missing fileName");

            MemoryDump memoryDump;

            memoryDump = new MemoryDump(fileName);
    

            nameMapper = memoryDump.GetNameMapper();
            memory = memoryDump.GetMemory();
            totalVerbList = memoryDump.GetTotalVerbList(memory);
            rejectedTheories = memoryDump.GetRejectedTheories(memory);
 
            if (nameMapper == null || memory == null || totalVerbList == null || rejectedTheories == null)
                throw new SaveLoadException("Couldn't load all data from file");
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Reset saving parameters
        /// </summary>
        private void ResetSavingParameters()
        {
            fileName = null;
            nameMapper = null;
            memory = null;
            totalVerbList = null;
            rejectedTheories = null;
        }

        /// <summary>
        /// Get next backup file name from saving file name
        /// </summary>
        /// <param name="fileName">saving file name</param>
        /// <returns>next backup file name from saving file name</returns>
        private string GetNextNewBackupName(string fileName)
        {
            string newBackupName = fileName + ".noMoreBackup";
            string formatedNumber;
            for (int i = 0; i < 10000; i++)
            {
                formatedNumber = i.ToString();

                while (formatedNumber.Length < 4)
                    formatedNumber = '0' + formatedNumber;

                newBackupName = fileName + ".backUp." + formatedNumber;

                if (!File.Exists(newBackupName))
                    break;
            }

            return newBackupName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// FileName to save or load
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// Name mapper
        /// </summary>
        public NameMapper NameMapper
        {
            get { return nameMapper; }
            set { nameMapper = value; }
        }

        /// <summary>
        /// Memory
        /// </summary>
        public Memory Memory
        {
            get { return memory; }
            set { memory = value; }
        }

        /// <summary>
        /// Operator list
        /// </summary>
        public HashSet<Concept> TotalVerbList
        {
            get { return totalVerbList; }
            set { totalVerbList = value; }
        }

        /// <summary>
        /// Rejected theories
        /// </summary>
        public RejectedTheories RejectedTheories
        {
            get { return rejectedTheories; }
            set { rejectedTheories = value; }
        }

        /// <summary>
        /// Postulated theories
        /// </summary>
        public TheoryList TotalTheoryList
        {
            get { return totalTheoryList; }
            set { totalTheoryList = value; }
        }

        /// <summary>
        /// Default file type filter
        /// </summary>
        public string DefaultFileTypeFilter
        {
            get { return defaultFileTypeFilter; }
        }

        /// <summary>
        /// Whether current file need to be saved or not
        /// </summary>
        public bool FileNeedSave
        {
            get { return fileNeedSave; }
            set { fileNeedSave = value; }
        }
        #endregion
    }
}
