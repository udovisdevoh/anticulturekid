using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riasce
{
    abstract class AbstractSourceCategoryList
    {
        /// <summary>
        /// Get next element
        /// </summary>
        /// <returns>Next element or null if no more element available</returns>
        public abstract string GetNext();

        /// <summary>
        /// Get previous element
        /// </summary>
        /// <returns>Previous element or null if no more element available</returns>
        public abstract string GetPrevious();

        /// <summary>
        /// Whether it's possible to call GetNext()
        /// </summary>
        /// <returns>Whether it's possible to call GetNext()</returns>
        public abstract bool HasNext();

        /// <summary>
        /// Whether it's possible to call GetPrevious()
        /// </summary>
        /// <returns>Whether it's possible to call GetPrevious()</returns>
        public abstract bool HasPrevious();
    }
}
