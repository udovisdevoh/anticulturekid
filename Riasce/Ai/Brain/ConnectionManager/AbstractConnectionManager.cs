using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Abstraction of how a connection manager works
    /// </summary>
    abstract class AbstractConnectionManager
    {
        /// <summary>
        /// Returns a connection that prevents to plug specified concepts
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <param name="strictMode">if true, consider unlikeliness as an obstruction, if not, don't</param>
        /// <returns>List of 3 concepts (subject, verb, complement) that prevents to connect specified concepts
        /// Return null if couldn't find anything obstructing specified connection</returns>
        public abstract List<Concept> FindObstructionToPlug(Concept subject, Concept verb, Concept complement, bool strictMode);

        /// <summary>
        /// Returns a connection that prevents to plug specified concepts, but ignore obstruction for verbs that are merely inverse_of the other
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <param name="strictMode">if true, consider unlikeliness as an obstruction, if not, don't</param>
        /// <returns>List of 3 concepts (subject, verb, complement) that prevents to connect specified concepts
        /// Return null if couldn't find anything obstructing specified connection</returns>
        public abstract List<Concept> FindObstructionToPlugAllowInverseAndPermutableSide(Concept subject, Concept verb, Concept complement, bool strictMode);

        /// <summary>
        /// Returns the amount of possible flat connections that prevent provided connection to be possible
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <param name="strictMode">if true, consider unlikeliness as an obstruction, if not, don't</param>
        /// <returns>the amount of possible flat connections that prevent provided connection to be possible</returns>
        public abstract int CountObstructionToPlug(Concept subject, Concept verb, Concept complement, bool strictMode);

        /// <summary>
        /// Plug subject concept to complement concept using verb concept
        /// Perform subsequent connection to contraposates
        /// Dirten affected concepts
        /// Doesn't check whether connection can be done or not (please use FindObstructionToPlug() method before)
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        public abstract void Plug(Concept subject, Concept verb, Concept complement);

        /// <summary>
        /// UnPlug subject concept from complement concept using verb concept
        /// Unplug subsequent connection to contraposates
        /// Dirten affected concepts
        /// Doesn't check whether disconnection can be done or not (please use FindObstructionToUnPlug() method before)
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        public abstract void UnPlug(Concept subject, Concept verb, Concept complement);

        /// <summary>
        /// Returns true if connection currently exist (in flat representation)
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <returns>True if connection exist, false if not</returns>
        public abstract bool TestConnection(Concept subject, Concept verb, Concept complement);

        /// <summary>
        /// Returns a proof to provided connection
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <returns>Proof object, null if connection doesn't exist</returns>
        public abstract Proof GetProofToConnection(Concept subject, Concept verb, Concept complement);

        /// <summary>
        /// By default: false. Use this to disable repair after adding/removing connections
        /// </summary>
        public abstract bool DisableFlattenizeAndOptimizeAndPurify
        {
            get;
            set;
        }
    }
}
