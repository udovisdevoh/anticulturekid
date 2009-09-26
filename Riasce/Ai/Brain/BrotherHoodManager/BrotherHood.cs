using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public class BrotherHood : IEnumerable<Concept>
    {
        #region Fields
        private Concept verb;

        private Concept complement;

        private HashSet<Concept> brotherList = new HashSet<Concept>();
        #endregion

        #region Constructor
        public BrotherHood(Concept verb, Concept complement)
        {
            this.verb = verb;
            this.complement = complement;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add a brother to brotherhood
        /// </summary>
        /// <param name="brotherConcept">brother concept</param>
        public void Add(Concept brother)
        {
            brotherList.Add(brother);
        }

        public IEnumerator<Concept> GetEnumerator()
        {
            return brotherList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Properties
        public Concept Verb
        {
            get { return verb; }
        }

        public Concept Complement
        {
            get { return complement; }
        }

        public int Count
        {
            get { return brotherList.Count; }
        }
        #endregion
    }
}
