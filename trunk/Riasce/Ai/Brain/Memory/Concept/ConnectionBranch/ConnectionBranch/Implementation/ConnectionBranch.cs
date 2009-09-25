using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public class ConnectionBranch : AbstractConnectionBranch
    {
        #region Fields
        private HashSet<Concept> complementConceptList = new HashSet<Concept>();

        private Dictionary<Concept, Proof> proofList = new Dictionary<Concept, Proof>();

        private bool isDirty;
        #endregion
        
        #region Methods
        public override void AddConnection(Concept complementConcept)
        {
            complementConceptList.Add(complementConcept);
        }

        public override void RemoveConnection(Concept complementConcept)
        {
            complementConceptList.Remove(complementConcept);
        }

        public override bool IsConnectedTo(Concept complementConcept)
        {
            return complementConceptList.Contains(complementConcept);
        }

        public override Proof GetProofTo(Concept complementConcept)
        {
            Proof proof;
            if (proofList.TryGetValue(complementConcept, out proof))
                return proof;
            else
                return null;
        }

        public override void SetProofTo(Concept complementConcept, Proof proof)
        {
            proofList[complementConcept] = proof;
        }
        #endregion

        #region Properties
        public override bool IsDirty
        {
            get
            {
                return isDirty;
            }
            set
            {
                isDirty = value;
            }
        }

        public HashSet<Concept> ComplementConceptList
        {
            get
            {
                return complementConceptList;
            }
        }
        #endregion
    }
}
