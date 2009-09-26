using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public class BrotherHoodSet : IEnumerable<BrotherHood>
    {
        #region Fields
        private Dictionary<Concept, Dictionary<Concept, BrotherHood>> brotherHoodList = new Dictionary<Concept, Dictionary<Concept, BrotherHood>>();
        #endregion

        #region Methods
        public override BrotherHood GetOrCreateBrotherHood(Concept verb, Concept complement)
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
