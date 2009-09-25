using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents a connection analogy to another connection, for instance,
    /// apple partof apple_tree is analog to orange partof orange_fruit
    /// </summary>
    class Analogy
    {
        #region Fields
        /// <summary>
        /// From 0 to 1: represents the analogy's relevance
        /// </summary>
        private double relevance;

        /// <summary>
        /// Output subject
        /// </summary>
        private Concept outputSubject;

        /// <summary>
        /// Output verb
        /// </summary>
        private Concept outputVerb;

        /// <summary>
        /// Output complement
        /// </summary>
        private Concept outputComplement;

        /// <summary>
        /// Source subject
        /// </summary>
        private Concept inputSubject;

        /// <summary>
        /// Source verb
        /// </summary>
        private Concept inputVerb;

        /// <summary>
        /// Source complement
        /// </summary>
        private Concept inputComplement;
        #endregion

        #region Constructor
        /// <summary>
        /// Create an analogy from relevence, source connection and target connection
        /// </summary>
        /// <param name="relevance">(from 0 to 1)</param>
        /// <param name="outputSubject">output subject</param>
        /// <param name="outputVerb">output verb</param>
        /// <param name="outputComplement">output complement</param>
        /// <param name="inputSubject">input subject</param>
        /// <param name="inputVerb">input verb</param>
        /// <param name="inputComplement">input complement</param>
        public Analogy(double relevance, Concept outputSubject, Concept outputVerb, Concept outputComplement, Concept inputSubject, Concept inputVerb, Concept inputComplement)
        {
            this.relevance = relevance;
            this.outputSubject = outputSubject;
            this.outputVerb = outputVerb;
            this.outputComplement = outputComplement;
            this.inputSubject = inputSubject;
            this.inputVerb = inputVerb;
            this.inputComplement = inputComplement;
        }
        #endregion

        #region Properties
        /// <summary>
        /// From 0 to 1: represents the analogy's relevance
        /// </summary>
        public double Relevance
        {
            get { return relevance; }
        }

        /// <summary>
        /// Output subject
        /// </summary>
        public Concept OutputSubject
        {
            get { return outputSubject; }
        }

        /// <summary>
        /// Output verb
        /// </summary>
        public Concept OutputVerb
        {
            get { return outputVerb; }
        }

        /// <summary>
        /// Output complement
        /// </summary>
        public Concept OutputComplement
        {
            get { return outputComplement; }
        }

        /// <summary>
        /// Source subject
        /// </summary>
        public Concept InputSubject
        {
            get { return inputSubject; }
        }

        /// <summary>
        /// Source verb
        /// </summary>
        public Concept InputVerb
        {
            get { return inputVerb; }
        }

        /// <summary>
        /// Source complement
        /// </summary>
        public Concept InputComplement
        {
            get { return inputComplement; }
        }
        #endregion
    }
}
