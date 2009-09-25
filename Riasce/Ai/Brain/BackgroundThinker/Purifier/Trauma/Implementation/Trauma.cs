using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class Trauma : AbstractTrauma
    {
        #region Fields
        private List<List<Concept>> connectionList = new List<List<Concept>>();
        #endregion

        #region Methods
        public override IEnumerator<List<Concept>> GetEnumerator()
        {
            return connectionList.GetEnumerator();
        }

        public override void Add(Concept nonSenseSubject, Concept nonSenseVerb, Concept nonSenseComplement, Concept sourceSubject, Concept sourceVerb, Concept sourceComplement)
        {
            connectionList.Add(new List<Concept>() { nonSenseSubject, nonSenseVerb, nonSenseComplement, sourceSubject, sourceVerb, sourceComplement });
        }
        #endregion

        #region Properties
        public override int Count
        {
            get { return connectionList.Count; }
        }
        #endregion
    }
}
