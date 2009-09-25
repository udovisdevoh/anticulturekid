using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public abstract class AbstractBrotherHoodSet : IEnumerable<BrotherHood>
    {
        /// <summary>
        /// Get or return brotherhood from verb and complement
        /// </summary>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <returns>brotherhood from verb and complement</returns>
        public abstract BrotherHood GetOrCreateBrotherHood(Concept verb, Concept complement);

        public abstract IEnumerator<BrotherHood> GetEnumerator();

        public abstract int CountBrothers
        {
            get;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
