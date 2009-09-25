using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractOptimizer
    {
        /// <summary>
        /// Repair a concept's optimized representation
        /// so no useless connection persist
        /// </summary>
        /// <param name="concept">Concept to repair</param>
        public abstract void Repair(Concept concept);
    }
}
