using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Used by string definitions sorter to sort complements
    /// </summary>
    class SortableComplementInfo : IComparable
    {
        #region Fields
        /// <summary>
        /// Complement's name
        /// </summary>
        private string name;

        /// <summary>
        /// Proof length
        /// </summary>
        private int proofLength;
        #endregion

        #region Constructor
        public SortableComplementInfo(string complementName, Concept subject, Concept verb, Concept complement)
        {
            this.name = complementName;

            Proof proof = subject.GetFlatConnectionBranch(verb).GetProofTo(complement);

            if (proof == null)
                proofLength = 0;
            else
                proofLength = proof.Count;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Compare a sortable sortable complement info to another one
        /// in order to sort a definition
        /// </summary>
        /// <param name="obj">other sortable complement info</param>
        /// <returns>difference</returns>
        public int CompareTo(object obj)
        {
            SortableComplementInfo other = (SortableComplementInfo)(obj);
            return this.proofLength - other.proofLength;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Complement name
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Proof length
        /// </summary>
        public int ProofLength
        {
            get { return proofLength; }
        }
        #endregion
    }
}
