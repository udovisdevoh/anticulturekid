using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class Trauma : IEnumerable<List<Concept>>
    {
        #region Fields
        private List<List<Concept>> connectionList = new List<List<Concept>>();
        #endregion

        #region Methods
        /// <summary>
        /// Add a connection and its source to trauma
        /// </summary>
        /// <param name="nonSenseSubject">non-sense subject</param>
        /// <param name="nonSenseVerb">non-sense verb</param>
        /// <param name="nonSenseComplement">non-sense complement</param>
        /// <param name="sourceSubject">source subject</param>
        /// <param name="sourceVerb">source verb</param>
        /// <param name="sourceComplement">source complement</param>
        public void Add(Concept nonSenseSubject, Concept nonSenseVerb, Concept nonSenseComplement, Concept sourceSubject, Concept sourceVerb, Concept sourceComplement)
        {
            connectionList.Add(new List<Concept>() { nonSenseSubject, nonSenseVerb, nonSenseComplement, sourceSubject, sourceVerb, sourceComplement });
        }

        public IEnumerator<List<Concept>> GetEnumerator()
        {
            return connectionList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Return the trauma's connection size
        /// </summary>
        public int Count
        {
            get { return connectionList.Count; }
        }
        #endregion
    }
}
