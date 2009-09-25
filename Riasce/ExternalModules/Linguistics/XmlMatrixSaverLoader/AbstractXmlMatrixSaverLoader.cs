using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractXmlMatrixSaverLoader
    {
        /// <summary>
        /// Save matrix to XML file
        /// </summary>
        /// <param name="matrix">matrix to save</param>
        /// <param name="fileName">file name</param>
        public abstract void Save(Matrix matrix, string fileName);

        /// <summary>
        /// Load matrix from XML file
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>Loaded matrix</returns>
        public abstract Matrix Load(string fileName);
    }
}
