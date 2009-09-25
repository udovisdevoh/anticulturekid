using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    [Serializable]
    abstract class AbstractMemoryDump
    {
        /// <summary>
        /// Write memory dump to xml file
        /// </summary>
        /// <param name="xmlFileName">xml file name</param>
        public abstract void SaveXmlFile(string xmlFileName);
    }
}