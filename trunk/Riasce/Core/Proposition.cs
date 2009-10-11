using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents a proposition (subject, verb and complement)
    /// </summary>
    public class Proposition : IEquatable<Proposition>
    {
        #region Fields
        /// <summary>
        /// Subject, verb and complement concepts
        /// </summary>
        private Concept subject, verb, complement;

        /// <summary>
        /// Whether the proposition is positive (new core feature)
        /// </summary>
        private bool isPositive = true;
        #endregion

        #region Constructor
        /// <summary>
        /// Create a proposition from subject, verb and complement
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="verb"></param>
        /// <param name="complement"></param>
        public Proposition(Concept subject, Concept verb, Concept complement)
        {
            this.subject = subject;
            this.verb = verb;
            this.complement = complement;
        }
        #endregion

        #region IEquatable<Proposition> Members
        public bool Equals(Proposition other)
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
        /// <summary>
        /// Subject concept
        /// </summary>
        public Concept Subject
        {
            get { return subject; }
        }

        /// <summary>
        /// Verb concept
        /// </summary>
        public Concept Verb
        {
            get { return verb; }
        }

        /// <summary>
        /// Complement concept
        /// </summary>
        public Concept Complement
        {
            get { return complement; }
        }

        /// <summary>
        /// Whether the proposition is positive (new core feature)
        /// </summary>
        public bool IsPositive
        {
            get { return isPositive; }
            set { isPositive = value; }
        }
        #endregion
    }
}