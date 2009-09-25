using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public abstract class AbstractDisambiguationNamer
    {
        /// <summary>
        /// Returns the context concept for subject concept
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>context concept</returns>
        public abstract Concept GetContextConcept(Concept subject);
    }
}
