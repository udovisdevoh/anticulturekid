using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// BrotherHood Set (a set of set of concepts with common connections)
    /// </summary>
    public class BrotherHoodSet : IEnumerable<BrotherHood>
    {
        #region Fields
        /// <summary>
        /// This class represents a set of brotherhood
        /// </summary>
        private Dictionary<Concept, Dictionary<Concept, BrotherHood>> brotherHoodList = new Dictionary<Concept, Dictionary<Concept, BrotherHood>>();
        #endregion

        #region Methods
        /// <summary>
        /// Get or create brotherhood
        /// </summary>
        /// <param name="verb">common verb</param>
        /// <param name="complement">common complement</param>
        /// <returns>new brotherhood</returns>
        public BrotherHood GetOrCreateBrotherHood(Concept verb, Concept complement)
        {
            Dictionary<Concept, BrotherHood> brotherHoodSubSet;
            if (!brotherHoodList.TryGetValue(verb, out brotherHoodSubSet))
            {
                brotherHoodSubSet = new Dictionary<Concept, BrotherHood>();
                brotherHoodList.Add(verb,brotherHoodSubSet);
            }

            BrotherHood brotherHood;
            if (!brotherHoodSubSet.TryGetValue(complement, out brotherHood))
            {
                brotherHood = new BrotherHood(verb, complement);
                brotherHoodSubSet.Add(complement, brotherHood);
            }

            return brotherHood;
        }

        public override IEnumerator<BrotherHood> GetEnumerator()
        {
            HashSet<BrotherHood> enumeration = new HashSet<BrotherHood>();

            foreach (Dictionary<Concept, BrotherHood> subSet in brotherHoodList.Values)
                enumeration.UnionWith(subSet.Values);

            return enumeration.GetEnumerator();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Count how many brothers (in total) in all brotherhoods
        /// </summary>
        public override int CountBrothers
        {
            get
            {
                int countBrothers = 0;
                foreach (BrotherHood brotherHood in this)
                    countBrothers += brotherHood.Count;
                return countBrothers;
            }
        }
        #endregion
    }
}
