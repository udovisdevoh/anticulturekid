using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractVisualizerHistory
    {
        /// <summary>
        /// Set previous concept
        /// </summary>
        /// <param name="previousConceptName">previous concept name</param>
        public abstract void Push(string previousConceptName);

        /// <summary>
        /// Can get previous element or not
        /// </summary>
        /// <returns>Can get previous element or not</returns>
        public abstract bool CanPrevious();

        /// <summary>
        /// Get previous concept from history
        /// </summary>
        /// <returns>previous concept from history</returns>
        public abstract string GetPrevious();

        /// <summary>
        /// Can get next element or not
        /// </summary>
        /// <returns>Can get next element or not</returns>
        public abstract bool CanNext();

        /// <summary>
        /// Get next concept from history
        /// </summary>
        /// <returns>next concept from history</returns>
        public abstract string GetNext();
    }
}
