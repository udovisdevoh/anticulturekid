using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents a prototype object used to build an argument
    /// </summary>
    public class ArgumentPrototype
    {
        #region Fields
        /// <summary>
        /// Argument's subject
        /// </summary>
        private Concept subject;

        /// <summary>
        /// Argument's verb
        /// </summary>
        private Concept verb;

        /// <summary>
        /// Argument's complement
        /// </summary>
        private Concept complement;
        #endregion

        #region Constructor
        public ArgumentPrototype(Concept subject, Concept verb, Concept complement)
        {
            this.subject = subject;
            this.verb = verb;
            this.complement = complement;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Argument's subject
        /// </summary>
        public Concept Subject
        {
            get { return subject; }
        }

        /// <summary>
        /// Argument's verb
        /// </summary>
        public Concept Verb
        {
            get { return verb ; }
        }

        /// <summary>
        /// Argument's complement
        /// </summary>
        public Concept Complement
        {
            get { return complement; }
        }
        #endregion
    }
}
