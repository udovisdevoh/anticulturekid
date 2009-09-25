using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractTrauma : IEnumerable<List<Concept>>
    {
        public abstract IEnumerator<List<Concept>> GetEnumerator();

        /// <summary>
        /// Return the trauma's connection size
        /// </summary>
        public abstract int Count
        {
            get;
        }

        /// <summary>
        /// Add a connection and its source to trauma
        /// </summary>
        /// <param name="nonSenseSubject">non-sense subject</param>
        /// <param name="nonSenseVerb">non-sense verb</param>
        /// <param name="nonSenseComplement">non-sense complement</param>
        /// <param name="sourceSubject">source subject</param>
        /// <param name="sourceVerb">source verb</param>
        /// <param name="sourceComplement">source complement</param>
        public abstract void Add(Concept nonSenseSubject, Concept nonSenseVerb, Concept nonSenseComplement, Concept sourceSubject, Concept sourceVerb, Concept sourceComplement);

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
