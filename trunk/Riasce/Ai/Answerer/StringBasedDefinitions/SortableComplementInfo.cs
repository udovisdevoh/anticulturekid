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
        private string name;

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
        public int CompareTo(object obj)
        {
            SortableComplementInfo other = (SortableComplementInfo)(obj);
            return this.proofLength - other.proofLength;
        }
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
        }

        public int ProofLength
        {
            get { return proofLength; }
        }
        #endregion
    }
}
