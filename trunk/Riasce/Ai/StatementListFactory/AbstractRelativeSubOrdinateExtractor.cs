using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractRelativeSubOrdinateExtractor
    {
        /// <summary>
        /// Remove and return relative subordinate list from original string
        /// </summary>
        /// <param name="originalStatement">original string</param>
        /// <param name="relativeSubOrdinateList">list of relative subordinate</param>
        /// <returns>original string stripped from relative subordinate</returns>
        public abstract string ExtractList(string originalStatement, out List<string> relativeSubOrdinateList);

        /// <summary>
        /// Remove and return relative subordinate from string
        /// </summary>
        /// <param name="fromString">from string</param>
        /// <param name="subOrdinate">sub ordinate</param>
        /// <returns>original string stripped from relative subordinate</returns>
        public abstract string Extract(string fromString, out string subOrdinate);
    }
}
