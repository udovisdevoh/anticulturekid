using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a connection branch that can be either flat or optimized
    /// </summary>
    public class ConnectionBranch
    {
        #region Fields
        /// <summary>
        /// Complement concept list in brach
        /// </summary>
        private HashSet<Concept> complementConceptList = new HashSet<Concept>();

        /// <summary>
        /// Proof list
        /// key: complement concept
        /// value: proof
        /// </summary>
        private Dictionary<Concept, Proof> proofList = new Dictionary<Concept, Proof>();

        /// <summary>
        /// Whether the branch is dirty
        /// </summary>
        private bool isDirty;
        #endregion
        
        #region Methods
        /// <summary>
        /// Add a conntection to complement concept
        /// </summary>
        /// <param name="complementConcept">complement concept</param>
        public void AddConnection(Concept complementConcept)
        {
            complementConceptList.Add(complementConcept);
        }

        /// <summary>
        /// Remove a conntection to complement concept
        /// </summary>
        /// <param name="complementConcept">complement concept</param>
        public void RemoveConnection(Concept complementConcept)
        {
            complementConceptList.Remove(complementConcept);
        }

        /// <summary>
        /// Returns true if flat connection exist to complement concept
        /// </summary>
        /// <param name="complementConcept">complement concept</param>
        /// <returns>if flat connection to complement concept exist: true</returns>
        public bool IsConnectedTo(Concept complementConcept)
        {
            return complementConceptList.Contains(complementConcept);
        }

        /// <summary>
        /// Returns a proof to connection to complement concept
        /// </summary>
        /// <param name="complementConcept">complement concept</param>
        /// <returns>proof to connection to complement concept
        /// it should be empty if the connection is direct</returns>
        public Proof GetProofTo(Concept complementConcept)
        {
            Proof proof;
            if (proofList.TryGetValue(complementConcept, out proof))
                return proof;
            else
                return null;
        }

        /// <summary>
        /// Sets the proof to a connection to the complement concept
        /// </summary>
        /// <param name="complementConcept">complement concept</param>
        /// <param name="prooft">proof object you wish to pass</param>
        public void SetProofTo(Concept complementConcept, Proof proof)
        {
            proofList[complementConcept] = proof;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Whether the connection branch needs to be repaired or not
        /// </summary>
        public bool IsDirty
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

        /// <summary>
        /// Complement concept list in brach
        /// </summary>
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
