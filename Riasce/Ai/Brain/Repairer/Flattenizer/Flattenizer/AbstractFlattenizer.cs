using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents the abstraction of connection flattenizers
    /// </summary>
    public abstract class AbstractFlattenizer
    {
        #region Public Methods
        /// <summary>
        /// Repair a concept's flat representation and regenerate its optimized representation
        /// so no useless connection persist.
        /// THIS METHOD MUST ONLY BE USED BY REPAIRER CLASS!!!
        /// </summary>
        /// <param name="subject">concept to repair</param>
        public abstract void Repair(Concept subject);
        #endregion

        #region Protected Methods
        /// <summary>
        /// Get proof object from proof prototype
        /// </summary>
        /// <param name="proofPrototype">provided proof prototype</param>
        /// <returns>proof object</returns>
        protected Proof GetProofFromPrototype(HashSet<ArgumentPrototype> proofPrototype)
        {
            Proof proof = new Proof();

            foreach (ArgumentPrototype argumentPrototype in proofPrototype)
            {
                Proof argumentProof = argumentPrototype.Subject.GetFlatConnectionBranch(argumentPrototype.Verb).GetProofTo(argumentPrototype.Complement);
                if (argumentProof.Count > 0)
                    proof.AddProof(argumentProof);

                proof.AddArgument(new Proposition(argumentPrototype.Subject, argumentPrototype.Verb, argumentPrototype.Complement));
            }
            return proof;
        }
        #endregion
    }
}
