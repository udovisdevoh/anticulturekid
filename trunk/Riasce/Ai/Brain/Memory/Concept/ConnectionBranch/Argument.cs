using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents a proof's argument
    /// </summary>
    class Argument
    {
        #region Fields
        private Concept subject, verb, complement;
        #endregion

        #region Constructor
        public Argument(Concept subject, Concept verb, Concept complement)
        {
            this.subject = subject;
            this.verb = verb;
            this.complement = complement;
        }
        #endregion

        #region Properties
        public Concept Subject
        {
            get { return subject; }
        }

        public Concept Verb
        {
            get { return verb; }
        }

        public Concept Complement
        {
            get { return complement; }
        }
        #endregion
    }
}
