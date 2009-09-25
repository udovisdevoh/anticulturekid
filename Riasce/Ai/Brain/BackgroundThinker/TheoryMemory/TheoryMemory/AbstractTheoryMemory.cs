using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractTheoryMemory
    {
        /// <summary>
        /// Remember new theory
        /// </summary>
        /// <param name="theory">theory to remember</param>
        public abstract void RememberNewTheory(Theory theory);
    }
}
