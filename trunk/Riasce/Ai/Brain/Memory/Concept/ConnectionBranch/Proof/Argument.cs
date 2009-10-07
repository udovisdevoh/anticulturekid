using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents a proof's argument
    /// </summary>
    public class Argument : IEquatable<Argument>
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

        #region IEquatable<Argument> Members
        public bool Equals(Argument other)
        {
            if (subject != other.subject)
                return false;
            if (verb != other.verb)
                return false;
            if (complement != other.complement)
                return false;
            return true;
        }
        #endregion

        #region Public Methods
        public override int GetHashCode()
        {
            return subject.GetHashCode() ^ verb.GetHashCode() ^ complement.GetHashCode() ^ subject.DebuggerName.GetHashCode() ^ verb.DebuggerName.GetHashCode() ^ complement.DebuggerName.GetHashCode();
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
