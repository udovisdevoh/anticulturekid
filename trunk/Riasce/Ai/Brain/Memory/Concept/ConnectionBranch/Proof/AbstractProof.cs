using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public abstract class AbstractProof : IEnumerable<List<Concept>>, IEquatable<AbstractProof>
    {
        /// <summary>
        /// Adds an argument at the end of the proof
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        public abstract void AddArgument(Concept subject, Concept verb, Concept complement);

        /// <summary>
        /// Append an other proof at the end of this proof
        /// </summary>
        /// <param name="otherProof">other proof</param>
        public abstract void AddProof(Proof otherProof);

        /// <summary>
        /// Returns the amount of arguments in the proof
        /// </summary>
        public abstract int Count
        {
            get;
        }

        /// <summary>
        /// Whether this proof is identical to other proof
        /// </summary>
        /// <param name="otherProof">other proof</param>
        /// <returns>true if the proofs are identical, else: false</returns>
        public abstract bool Equals(AbstractProof otherProof);

        /// <summary>
        /// Whether proof contains argument
        /// </summary>
        /// <param name="subject">subject</param>
        /// <param name="verb">verb</param>
        /// <param name="complement">complement</param>
        /// <returns>true if proof contains argument, else: false</returns>
        public abstract bool ContainsArgument(Concept subject, Concept verb, Concept complement);

        /// <summary>
        /// Returns the very first concept in the proof's first argument
        /// </summary>
        /// <returns>the very first concept in the proof's first argument</returns>
        public abstract Concept GetFirstWord();

        /// <summary>
        /// Returns the very last concept in the proof's last argument
        /// </summary>
        /// <returns>the very last concept in the proof's last argument</returns>
        public abstract Concept GetLastWord();

        /// <summary>
        /// Whether the proof contains duplicate argument or not
        /// </summary>
        /// <returns>Whether the proof contains duplicate argument or not</returns>
        public abstract bool ContainsDuplicateArgument();

        public override bool Equals(Object obj)
        {
            AbstractProof proof = (AbstractProof)(obj);
            return this.Equals(proof);
        }

        public abstract IEnumerator<List<Concept>> GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static bool operator ==(AbstractProof proof1, AbstractProof proof2)
        {
            if ((object)(proof1) == null && (object)(proof2) == null)
                return true;
            else if ((object)(proof1) == null || (object)(proof2) == null)
                return false;

            return proof1.Equals(proof2);
        }

        public static bool operator !=(AbstractProof proof1, AbstractProof proof2)
        {
            return !(proof1 == proof2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
