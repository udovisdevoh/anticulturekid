using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractImplyConnectionManager
    {
        /// <summary>
        /// Add imply metaconnection to verb
        /// </summary>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <param name="condition">condition</param>
        public abstract void AddImplyConnection(Concept verb, Concept complement, Condition condition);

        /// <summary>
        /// Remove imply metaconnection to verb
        /// </summary>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <param name="condition">condition</param>
        public abstract void RemoveImplyConnection(Concept verb, Concept complement, Condition condition);

        /// <summary>
        /// Returns true if Imply Connection exists
        /// </summary>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <param name="condition">condition</param>
        /// <returns>Returns true if Imply Connection exists, else: false</returns>
        public abstract bool TestImplyConnection(Concept verb, Concept complement, Condition condition);
    }
}
