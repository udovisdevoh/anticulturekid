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
        #region Protected Fields
        /// <summary>
        /// Remember which branches are already repaired to improve performance a lot
        /// </summary>
        protected HashSet<ConnectionBranch> repairedBranches;

        /// <summary>
        /// Remember metaConnections to improve performances
        /// </summary>
        protected VerbMetaConnectionCache verbMetaConnectionCache;
        #endregion

        #region Public Methods
        /// <summary>
        /// Repair a concept's flat representation and regenerate its optimized representation
        /// so no useless connection persist
        /// </summary>
        /// <param name="concept">Concept to repair</param>
        public void Repair(Concept subject)
        {
            Repair(subject, new HashSet<ConnectionBranch>(), new VerbMetaConnectionCache());
        }

        /// <summary>
        /// Repair a concept's flat representation and regenerate its optimized representation
        /// so no useless connection persist.
        /// THIS METHOD MUST ONLY BE USED BY REPAIRER CLASS!!!
        /// </summary>
        /// <param name="subject">concept to repair</param>
        /// <param name="repairedBranches">provided HashSet to rememebr which branches were repaired</param>
        /// <param name="verbConnectionCache">provided cache to remember flattenized metaConnections</param>
        public abstract void Repair(Concept subject, HashSet<ConnectionBranch> repairedBranches, VerbMetaConnectionCache verbConnectionCache);
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

                proof.AddArgument(argumentPrototype.Subject, argumentPrototype.Verb, argumentPrototype.Complement);
            }
            return proof;
        }
        #endregion
    }
}
