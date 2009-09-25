using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public class BrotherHood : AbstractBrotherHood
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
        public override void Add(Concept brother)
        {
            brotherList.Add(brother);
        }

        public override IEnumerator<Concept> GetEnumerator()
        {
            return brotherList.GetEnumerator();
        }
        #endregion

        #region Properties
        public override Concept Verb
        {
            get { return verb; }
        }

        public override Concept Complement
        {
            get { return complement; }
        }

        public override int Count
        {
            get { return brotherList.Count; }
        }
        #endregion
    }
}
