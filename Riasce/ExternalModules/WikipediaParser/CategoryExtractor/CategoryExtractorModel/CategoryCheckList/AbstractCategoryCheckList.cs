using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractCategoryCheckList
    {
        /// <summary>
        /// Whether the checklist contains element name
        /// </summary>
        /// <param name="elementName">element name</param>
        /// <returns>whether the checklist contains element name or not</returns>
        public abstract bool Contains(string elementName);

        /// <summary>
        /// Add element name to the checklist
        /// </summary>
        /// <param name="elementName">element name</param>
        public abstract void Add(string elementName);
    }
}
