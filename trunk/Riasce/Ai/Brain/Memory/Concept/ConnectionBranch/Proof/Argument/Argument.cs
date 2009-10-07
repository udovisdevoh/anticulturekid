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
            throw new Exception("Don't use argument constructor, use Argument.Build(Concept,Concept,Concept) instead");
        }

        private Argument()
        {
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

        #region Public Static Methods
        public static Argument Build(Concept subject, Concept verb, Concept complement)
        {
            Argument argument = new Argument();
            argument.subject = subject;
            argument.verb = subject;
            argument.complement = subject;

            return argument;

            //return ArgumentCache.GetOrCreateArgument(subject,verb,complement);
        }

        public static Argument GetNewEmptyArgument()
        {
            return new Argument();
        }
        #endregion

        #region Properties
        public Concept Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public Concept Verb
        {
            get { return verb; }
            set { verb = value; }
        }

        public Concept Complement
        {
            get { return complement; }
            set { complement = value; }
        }
        #endregion
    }
}
