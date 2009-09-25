using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AntiCulture.Kid
{
    class SaverLoader : AbstractSaverLoader
    {
        #region Static
        private static readonly string defaultFileTypeFilter = "XML Memory files|*.xml|Legacy Memory files|*.AiMemory";
        #endregion

        #region Fields
        private string fileName;

        private NameMapper nameMapper;

        private Memory memory;

        private HashSet<Concept> totalVerbList;

        private RejectedTheories rejectedTheories;

        private Stream stream;

        private TheoryList totalTheoryList;

        private BinaryFormatter formatter = new BinaryFormatter();

        private bool fileNeedSave = false;
        #endregion

        #region Constructor
        public SaverLoader()
        {
            ResetSavingParameters();
        }
        #endregion

        #region Methods
        public override void Save()
        {
            if (fileName == null || nameMapper == null || memory == null || totalVerbList == null || rejectedTheories == null || totalTheoryList == null)
                throw new SaveLoadException("Missing saving parameters");
            
            //Serialize stuff
            //IFormatter formatter = new BinaryFormatter();

            if (File.Exists(fileName))
                File.Copy(fileName, GetNextNewBackupName(fileName));

            MemoryDump memoryDump = new MemoryDump(nameMapper, memory, rejectedTheories, totalVerbList, totalTheoryList);

            if (fileName.Length > 3 && fileName.ToLower().Substring(fileName.Length - 4) == ".xml")
            {
                memoryDump.SaveXmlFile(fileName);
            }
            else
            {
                stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, memoryDump);
                stream.Close();
            }
            ResetSavingParameters();
        }

        public override void Load()
        {
            if (fileName == null)
                throw new SaveLoadException("Missing fileName");

            MemoryDump memoryDump;

            if (fileName.Length > 3 && fileName.ToLower().Substring(fileName.Length - 4) == ".xml")
            {
                memoryDump = new MemoryDump(fileName);
            }
            else
            {
                //Unserialize stuff
                using (stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    memoryDump = (MemoryDump)formatter.Deserialize(stream);
                }
            }

            nameMapper = memoryDump.GetNameMapper();
            memory = memoryDump.GetMemory();
            totalVerbList = memoryDump.GetTotalVerbList(memory);
            rejectedTheories = memoryDump.GetRejectedTheories(memory);
 
            if (nameMapper == null || memory == null || totalVerbList == null || rejectedTheories == null)
                throw new SaveLoadException("Couldn't load all data from file");
        }

        private void ResetSavingParameters()
        {
            fileName = null;
            nameMapper = null;
            memory = null;
            totalVerbList = null;
            rejectedTheories = null;
        }

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
        public override string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public override NameMapper NameMapper
        {
            get { return nameMapper; }
            set { nameMapper = value; }
        }

        public override Memory Memory
        {
            get { return memory; }
            set { memory = value; }
        }

        public override HashSet<Concept> TotalVerbList
        {
            get { return totalVerbList; }
            set { totalVerbList = value; }
        }

        public override RejectedTheories RejectedTheories
        {
            get { return rejectedTheories; }
            set { rejectedTheories = value; }
        }

        public override TheoryList TotalTheoryList
        {
            get { return totalTheoryList; }
            set { totalTheoryList = value; }
        }

        public override string DefaultFileTypeFilter
        {
            get { return defaultFileTypeFilter; }
        }

        public override bool FileNeedSave
        {
            get { return fileNeedSave; }
            set { fileNeedSave = value; }
        }
        #endregion
    }
}
