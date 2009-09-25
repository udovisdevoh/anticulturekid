using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractTheoryInfo : IEnumerable<List<Concept>>
    {
        /// <summary>
        /// Returns the subject
        /// </summary>
        public abstract Concept Subject
        {
            get;
        }

        /// <summary>
        /// Returns the verb
        /// </summary>
        public abstract Concept Verb
        {
            get;
        }

        /// <summary>
        /// Returns the complement
        /// </summary>
        public abstract Concept Complement
        {
            get;
        }

        /// <summary>
        /// How many argument in the theory info
        /// </summary>
        public abstract int CountArgument
        {
            get;
        }

        /// <summary>
        /// Add an argument to the theory info
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        public abstract void AddArgument(Concept subject, Concept verb, Concept complement);

        public abstract IEnumerator<List<Concept>> GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
