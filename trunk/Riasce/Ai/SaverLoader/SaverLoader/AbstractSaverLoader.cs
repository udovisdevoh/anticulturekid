using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractSaverLoader
    {
        /// <summary>
        /// Save
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// Load
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// FileName to save or load
        /// </summary>
        public abstract string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// Name mapper
        /// </summary>
        public abstract NameMapper NameMapper
        {
            get;
            set;
        }

        /// <summary>
        /// Memory
        /// </summary>
        public abstract Memory Memory
        {
            get;
            set;
        }

        /// <summary>
        /// Operator list
        /// </summary>
        public abstract HashSet<Concept> TotalVerbList
        {
            get;
            set;
        }

        /// <summary>
        /// Rejected theories
        /// </summary>
        public abstract RejectedTheories RejectedTheories
        {
            get;
            set;
        }

        /// <summary>
        /// Postulated theories
        /// </summary>
        public abstract TheoryList TotalTheoryList
        {
            get;
            set;
        }

        public abstract string DefaultFileTypeFilter
        {
            get;
        }

        public abstract bool FileNeedSave
        {
            get;
            set;
        }
    }
}
