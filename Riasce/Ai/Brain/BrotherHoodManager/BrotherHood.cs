using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents concepts that have a common connection
    /// </summary>
    public class BrotherHood : IEnumerable<Concept>
    {
        #region Fields
        /// <summary>
        /// Common verb
        /// </summary>
        private Concept verb;

        /// <summary>
        /// Common complement
        /// </summary>
        private Concept complement;

        /// <summary>
        /// Brother list
        /// </summary>
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
        /// <summary>
        /// Common verb
        /// </summary>
        public Concept Verb
        {
            get { return verb; }
        }

        /// <summary>
        /// Common complement
        /// </summary>
        public Concept Complement
        {
            get { return complement; }
        }

        /// <summary>
        /// How many concept in the brotherhood
        /// </summary>
        public int Count
        {
            get { return brotherList.Count; }
        }
        #endregion
    }
}
