using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TheoryInfo : AbstractTheoryInfo
    {
        #region Fields
        private Concept subject;
        
        private Concept verb;

        private Concept complement;

        private List<List<Concept>> argumentList = new List<List<Concept>>();
        #endregion

        #region Constructor
        public TheoryInfo(Concept subject, Concept verb, Concept complement)
        {
            this.subject = subject;
            this.verb = verb;
            this.complement = complement;
        }
        #endregion

        #region Methods
        public override IEnumerator<List<Concept>> GetEnumerator()
        {
            return argumentList.GetEnumerator();
        }
        #endregion

        #region Properties
        public override Concept Subject
        {
            get { return subject; }
        }

        public override Concept Verb
        {
            get { return verb; }
        }

        public override Concept Complement
        {
            get { return complement; }
        }

        public override int CountArgument
        {
            get { return argumentList.Count; }
        }

        public override void AddArgument(Concept subject, Concept verb, Concept complement)
        {
            argumentList.Add(new List<Concept>() { subject, verb, complement });
        }
        #endregion
    }
}
