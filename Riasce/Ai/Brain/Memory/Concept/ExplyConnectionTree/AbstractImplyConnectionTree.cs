using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public abstract class AbstractImplyConnectionTree : IEnumerable<KeyValuePair<Concept, HashSet<Condition>>>
    {
        /// <summary>
        /// Add imply connection to complement
        /// </summary>
        /// <param name="complement">complement concept</param>
        /// <param name="condition">condition</param>
        public abstract void AddConnection(Concept complement, Condition condition);

        /// <summary>
        /// Remove imply connection to complement
        /// </summary>
        /// <param name="complement">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <param name="condition">condition</param>
        public abstract void RemoveConnection(Concept verb, Concept complement, Condition condition);

        /// <summary>
        /// Returns true if imply connection exists, else: false
        /// </summary>
        /// <param name="complement">complement concept</param>
        /// <param name="condition">condition</param>
        /// <returns>true if imply connection exists, else: false</returns>
        public abstract bool TestConnection(Concept complement, Condition condition);

        public abstract IEnumerator<KeyValuePair<Concept, HashSet<Condition>>> GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
