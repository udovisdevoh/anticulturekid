using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represent a proof to a connection
    /// </summary>
    public class Proof : IEnumerable<List<Concept>>, IEquatable<Proof>
    {
        #region Fields
        /// <summary>
        /// Argument list
        /// </summary>
        private List<List<Concept>> argumentList;

        /// <summary>
        /// Which statement to prove
        /// </summary>
        private List<Concept> statementToProove = null;
        #endregion

        #region Constructors
        public Proof()
        {
            argumentList = new List<List<Concept>>();
        }

        public Proof(Concept subject, Concept verb, Concept complement)
        {
            statementToProove = new List<Concept>() { subject, verb, complement };
            argumentList = new List<List<Concept>>();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds an argument at the end of the proof
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        public void AddArgument(Concept subject, Concept verb, Concept complement)
        {
            argumentList.Add(new List<Concept>() { subject, verb, complement });
        }

        /// <summary>
        /// Append an other proof at the end of this proof
        /// </summary>
        /// <param name="otherProof">other proof</param>
        public void AddProof(Proof otherProof)
        {
            if (this == otherProof)
            {
                argumentList.Clear();
                throw new TotologyException("Totology detected");
            }
            else if (statementToProove != null && otherProof.ContainsArgument(statementToProove[0], statementToProove[1], statementToProove[2]))
            {
                argumentList.Clear();
                throw new TotologyException("Totology detected");
            }

            foreach (List<Concept> argument in otherProof.argumentList)
                this.AddArgument(argument[0], argument[1], argument[2]);
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
            foreach (List<Concept> currentArgument in this.argumentList)
            {
                if (otherProof.argumentList[counter][0] != currentArgument[0])
                    return false;
                else if (otherProof.argumentList[counter][1] != currentArgument[1])
                    return false;
                else if (otherProof.argumentList[counter][2] != currentArgument[2])
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
            foreach (List<Concept> argument in argumentList)
                if (argument[0] == subject && argument[1] == verb && argument[2] == complement)
                    return true;
            return false;
        }

        /// <summary>
        /// Returns the very last concept in the proof's last argument
        /// </summary>
        /// <returns>the very last concept in the proof's last argument</returns>
        public Concept GetLastWord()
        {
            return argumentList[argumentList.Count - 1][2];
        }

        /// <summary>
        /// Returns the very first concept in the proof's first argument
        /// </summary>
        /// <returns>the very first concept in the proof's first argument</returns>
        public Concept GetFirstWord()
        {
            return argumentList[0][0];
        }

        /// <summary>
        /// Whether the proof contains duplicate argument or not
        /// </summary>
        /// <returns>Whether the proof contains duplicate argument or not</returns>
        public bool ContainsDuplicateArgument()
        {
            HashSet<string> ignoreList = new HashSet<string>();
            string currentHashCode;
            foreach (List<Concept> argument in argumentList)
            {
                currentHashCode = argument[0].GetHashCode() + "-" + argument[1].GetHashCode() + "-" + argument[2].GetHashCode() + "|" + argument[0].DebuggerName + "|" + argument[1].DebuggerName + "|" + argument[2].DebuggerName;
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

        public IEnumerator<List<Concept>> GetEnumerator()
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
