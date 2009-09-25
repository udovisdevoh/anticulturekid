using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riasce
{
    abstract class AbstractConceptCheckList
    {
        /// <summary>
        /// Return the concept which was least recently returned from here
        /// If never returned and in memory, return it
        /// </summary>
        /// <param name="memory">memory</param>
        /// <returns>the concept which was least recently returned from here
        /// If never returned and in memory, return it</returns>
        public abstract Concept GetNextNeglectedConcept(Memory memory);
    }
}
