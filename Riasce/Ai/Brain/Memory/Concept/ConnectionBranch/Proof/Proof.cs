using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represent a proof to a connection
    /// </summary>
    public class Proof : IEnumerable<Argument>, IEquatable<Proof>
    {
        #region Fields
        /// <summary>
        /// Argument list
        /// </summary>
        private List<Argument> argumentList;

        /// <summary>
        /// Which statement to prove
        /// </summary>
        private Argument statementToProove = null;
        #endregion

        #region Constructors
        public Proof()
        {
            argumentList = new List<Argument>();
        }

        public Proof(Concept subject, Concept verb, Concept complement)
        {
            statementToProove = new Argument(subject, verb, complement);
            argumentList = new List<Argument>();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds an argument at the end of the proof
        /// </summary>
        /// <param name="argument">argument</param>
        public void AddArgument(Argument argument)
        {
            argumentList.Add(argument);
        }

        /// <summary>
        /// Append an other proof at the end of this proof
        /// </summary>
        /// <param name="otherProof">other proof</param>
        public void AddProof(Proof otherProof)
        {
            #warning Totology detection disabled to improve performances
            /*if (this == otherProof)
            {
                argumentList.Clear();
                throw new TotologyException("Totology detected");
            }
            else if (statementToProove != null && otherProof.ContainsArgument(statementToProove.Subject, statementToProove.Verb, statementToProove.Complement))
            {
                argumentList.Clear();
                throw new TotologyException("Totology detected");
            }*/

            argumentList.AddRange(otherProof.argumentList);
        }

        /// <summary>
        /// Whether this proof is identical to other proof
        /// </summary>
        /// <param name="otherProof">other proof</param>
        /// <returns>true if the proofs are identical, else: false</returns>
        public bool Equals(Proof otherProof)
        {
            if (this.argumentList.Count != otherProof.argumentList.Count)
                return false;

            int counter = 0;
            foreach (Argument currentArgument in this.argumentList)
            {
                if (!currentArgument.Equals(otherProof.argumentList[counter]))
                    return false;

                counter++;
            }

            return true;
        }

        /// <summary>
        /// Whether this proof is identical to other proof
        /// </summary>
        /// <param name="otherProof">other proof</param>
        /// <returns>true if the proofs are identical, else: false</returns>
        public override bool Equals(Object obj)
        {
            Proof proof = (Proof)(obj);
            return this.Equals(proof);
        }

        /// <summary>
        /// Whether proof contains argument
        /// </summary>
        /// <param name="subject">subject</param>
        /// <param name="verb">verb</param>
        /// <param name="complement">complement</param>
        /// <returns>true if proof contains argument, else: false</returns>
        public bool ContainsArgument(Concept subject, Concept verb, Concept complement)
        {
            foreach (Argument argument in argumentList)
                if (argument.Subject == subject && argument.Verb == verb && argument.Complement == complement)
                    return true;
            return false;
        }

        /// <summary>
        /// Returns the very last concept in the proof's last argument
        /// </summary>
        /// <returns>the very last concept in the proof's last argument</returns>
        public Concept GetLastWord()
        {
            return argumentList[argumentList.Count - 1].Complement;
        }

        /// <summary>
        /// Returns the very first concept in the proof's first argument
        /// </summary>
        /// <returns>the very first concept in the proof's first argument</returns>
        public Concept GetFirstWord()
        {
            return argumentList[0].Subject;
        }

        /// <summary>
        /// Whether the proof contains duplicate argument or not
        /// </summary>
        /// <returns>Whether the proof contains duplicate argument or not</returns>
        public bool ContainsDuplicateArgument()
        {
            HashSet<int> ignoreList = new HashSet<int>();
            int currentHashCode;
            foreach (Argument argument in argumentList)
            {
                currentHashCode = argument.GetHashCode();
                if (ignoreList.Contains(currentHashCode))
                    return true;

                ignoreList.Add(currentHashCode);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public IEnumerator<Argument> GetEnumerator()
        {
            return argumentList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Operator Overload
        public static bool operator ==(Proof proof1, Proof proof2)
        {
            if ((object)(proof1) == null && (object)(proof2) == null)
                return true;
            else if ((object)(proof1) == null || (object)(proof2) == null)
                return false;

            return proof1.Equals(proof2);
        }

        public static bool operator !=(Proof proof1, Proof proof2)
        {
            return !(proof1 == proof2);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Returns the amount of arguments in the proof
        /// </summary>
        public int Count
        {
            get { return argumentList.Count; }
        }
        #endregion
    }
}