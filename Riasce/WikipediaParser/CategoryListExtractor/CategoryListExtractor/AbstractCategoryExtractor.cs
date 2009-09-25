using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riasce
{
    abstract class AbstractCategoryListExtractor
    {
        /// <summary>
        /// Current file name
        /// </summary>
        public abstract string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// Chunk size
        /// </summary>
        public abstract int ChunkSize
        {
            get;
            set;
        }

        /// <summary>
        /// Show the window
        /// </summary>
        public abstract void Show();
    }
}
