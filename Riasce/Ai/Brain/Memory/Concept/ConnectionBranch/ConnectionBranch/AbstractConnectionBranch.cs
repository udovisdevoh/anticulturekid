using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public abstract class AbstractConnectionBranch
    {
        /// <summary>
        /// Add a conntection to complement concept
        /// </summary>
        /// <param name="complementConcept">complement concept</param>
        public abstract void AddConnection(Concept complementConcept);

        /// <summary>
        /// Remove a conntection to complement concept
        /// </summary>
        /// <param name="complementConcept">complement concept</param>
        public abstract void RemoveConnection(Concept complementConcept);

        /// <summary>
        /// Returns true if flat connection exist to complement concept
        /// </summary>
        /// <param name="complementConcept">complement concept</param>
        /// <returns>if flat connection to complement concept exist: true</returns>
        public abstract bool IsConnectedTo(Concept complementConcept);

        /// <summary>
        /// Returns a proof to connection to complement concept
        /// </summary>
        /// <param name="complementConcept">complement concept</param>
        /// <returns>proof to connection to complement concept
        /// it should be empty if the connection is direct</returns>
        public abstract Proof GetProofTo(Concept complementConcept);

        /// <summary>
        /// Sets the proof to a connection to the complement concept
        /// </summary>
        /// <param name="complementConcept">complement concept</param>
        /// <param name="prooft">proof object you wish to pass</param>
        public abstract void SetProofTo(Concept complementConcept, Proof proof);

        /// <summary>
        /// Whether the connection branch needs to be repaired or not
        /// </summary>
        public abstract bool IsDirty
        {
            get;
            set;
        }
    }
}
