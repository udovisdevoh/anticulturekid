using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public abstract class AbstractBrotherHood : IEnumerable<Concept>
    {
        /// <summary>
        /// Add a brother to brotherhood
        /// </summary>
        /// <param name="brotherConcept">brother concept</param>
        public abstract void Add(Concept brotherConcept);

        public abstract IEnumerator<Concept> GetEnumerator();

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract Concept Verb
        {
            get;
        }

        public abstract Concept Complement
        {
            get;
        }

        public abstract int Count
        {
            get;
        }
    }
}
