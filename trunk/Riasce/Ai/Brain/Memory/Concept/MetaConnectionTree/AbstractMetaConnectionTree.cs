using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public abstract class AbstractMetaConnectionTree
    {
        /// <summary>
        /// Adds a connection to the other concept
        /// </summary>
        /// <param name="metaOperator">metaOperator name</param>
        /// <param name="concept">complement concept</param>
        public abstract void AddMetaConnection(string metaOperatorName, Concept concept);

        /// <summary>
        /// Remove a connection to the other concept
        /// </summary>
        /// <param name="metaOperator">metaOperator name</param>
        /// <param name="concept">complement concept</param>
        public abstract void RemoveMetaConnection(string metaOperatorName, Concept concept);

        /// <summary>
        /// Test whether meta connection exist or not
        /// </summary>
        /// <param name="metaOperator">metaOperator name</param>
        /// <param name="concept">complement concept</param>
        /// <returns>If connection exist: true, else: false</returns>
        public abstract bool IsMetaConnectedTo(string metaOperatorName, Concept concept);

        /// <summary>
        /// Returns all complement operators for specified metaOperator
        /// </summary>
        /// <param name="metaOperatorName">meta operator name</param>
        /// <returns>HashSet containing a list of operators</returns>
        public abstract HashSet<Concept> GetAffectedOperatorsByMetaConnection(string metaOperatorName);
    }
}
