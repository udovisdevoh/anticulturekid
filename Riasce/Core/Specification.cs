using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents a composite specification
    /// It is used to evaluate conditions
    /// </summary>
    class Specification
    {
        #region Constants
        /// <summary>
        /// When the specification is not compisite (leaf of the tree)
        /// </summary>
        public const byte NULL_GATE = 0;

        /// <summary>
        /// When the operator between the two child specification is "or"
        /// </summary>
        public const byte OR_GATE = 1;

        /// <summary>
        /// When the operator between the two child specification is "and"
        /// </summary>
        public const byte AND_GATE = 2;

        /// <summary>
        /// When the operator between the two child specification is "xor"
        /// </summary>
        public const byte XOR_GATE = 3;
        #endregion

        #region Fields
        /// <summary>
        /// Whether the specification is positive (true by default)
        /// </summary>
        private bool isPositive;

        /// <summary>
        /// Specification's gate (only used if atomicCondition is null)
        /// For composite specifications only
        /// </summary>
        private byte gate;

        /// <summary>
        /// Only used when gate is NULL_GATE, when specification
        /// contains only one atomic condition
        /// </summary>
        private Proposition atomicCondition;

        /// <summary>
        /// Left child specification (only for composite specifications)
        /// </summary>
        private Specification leftChildSpecification;

        /// <summary>
        /// Right child specification (only for composite specifications)
        /// </summary>
        private Specification rightChildSpecification;
        #endregion

        #region Constructors
        /// <summary>
        /// Create an atomic specification
        /// </summary>
        /// <param name="isPositive">whether the atomic specification is positive</param>
        /// <param name="atomicCondition">atomic condition to match</param>
        public Specification(bool isPositive, Proposition atomicCondition)
        {
            this.isPositive = isPositive;
            this.atomicCondition = atomicCondition;
            this.gate = NULL_GATE;
        }

        /// <summary>
        /// Create a composite specification
        /// </summary>
        /// <param name="isPositive">whether the composite specification is positive</param>
        /// <param name="leftChildSpecification">left child specification</param>
        /// <param name="gate">gate (use constants: AND_GATE, OR_GATE, XOR_GATE)</param>
        /// <param name="rightChildSpecification">right child specification</param>
        public Specification(bool isPositive, Specification leftChildSpecification, byte gate, Specification rightChildSpecification)
        {
            this.isPositive = isPositive;
            this.leftChildSpecification = leftChildSpecification;
            this.gate = gate;
            this.rightChildSpecification = rightChildSpecification;
        }
        #endregion
    }
}