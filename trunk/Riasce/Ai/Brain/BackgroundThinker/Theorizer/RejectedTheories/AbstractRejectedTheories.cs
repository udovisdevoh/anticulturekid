using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractRejectedTheories : IEnumerable<Theory>
    {
        /// <summary>
        /// Add a theory to rejected theories
        /// </summary>
        /// <param name="theory">theory to add</param>
        public abstract void Add(Theory theory);

        /// <summary>
        /// Returns true if theory is part of rejected theories
        /// </summary>
        /// <param name="theory">theory</param>
        /// <returns>true if theory is part of rejected theories, else: false</returns>
        public abstract bool Contains(Theory theory);

        /// <summary>
        /// Return true if at least one of the theory is part of rejected theories
        /// </summary>
        /// <param name="theoryListToMatch"></param>
        /// <returns>true if at least one of the theory is part of rejected theories</returns>
        public abstract bool ContainsOneFromRange(IEnumerable<Theory> theoryListToMatch);

        public abstract IEnumerator<Theory> GetEnumerator();

        /// <summary>
        /// Assimilate another set of theories
        /// </summary>
        /// <param name="otherRejectedSet">other set of theories</param>
        public abstract void Assimilate(RejectedTheories otherRejectedSet);

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
