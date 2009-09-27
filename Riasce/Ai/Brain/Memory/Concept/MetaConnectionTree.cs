using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public class MetaConnectionTree
    {
        #region Fields
        private Dictionary<string, HashSet<Concept>> metaConnections = new Dictionary<string, HashSet<Concept>>();
        #endregion

        #region Methods
        /// <summary>
        /// Adds a connection to the other concept
        /// </summary>
        /// <param name="metaOperator">metaOperator name</param>
        /// <param name="concept">complement concept</param>
        public void AddMetaConnection(string metaOperatorName, Concept concept)
        {
            HashSet<Concept> connectionSubSet;
            if (!metaConnections.TryGetValue(metaOperatorName, out connectionSubSet))
            {
                connectionSubSet = new HashSet<Concept>();
                metaConnections.Add(metaOperatorName,connectionSubSet);
            }
            connectionSubSet.Add(concept);
        }

        /// <summary>
        /// Remove a connection to the other concept
        /// </summary>
        /// <param name="metaOperator">metaOperator name</param>
        /// <param name="concept">complement concept</param>
        public void RemoveMetaConnection(string metaOperatorName, Concept concept)
        {
            HashSet<Concept> connectionSubSet;
            if (metaConnections.TryGetValue(metaOperatorName, out connectionSubSet))
                connectionSubSet.Remove(concept);
        }

        /// <summary>
        /// Test whether meta connection exist or not
        /// </summary>
        /// <param name="metaOperator">metaOperator name</param>
        /// <param name="concept">complement concept</param>
        /// <returns>If connection exist: true, else: false</returns>
        public bool IsMetaConnectedTo(string metaOperatorName, Concept concept)
        {
            HashSet<Concept> connectionSubSet;
            if (metaConnections.TryGetValue(metaOperatorName, out connectionSubSet))
                if (connectionSubSet.Contains(concept))
                    return true;
            return false;
        }

        /// <summary>
        /// Returns all complement operators for specified metaOperator
        /// </summary>
        /// <param name="metaOperatorName">meta operator name</param>
        /// <returns>HashSet containing a list of operators</returns>
        public override HashSet<Concept> GetAffectedOperatorsByMetaConnection(string metaOperatorName)
        {
            HashSet<Concept> affectedOperators;

            if (metaConnections.TryGetValue(metaOperatorName, out affectedOperators))
                return affectedOperators;
            else
                return new HashSet<Concept>();
        }
        #endregion

        #region Properties
        public bool IsEmpty
        {
            get
            {
                if (metaConnections.Count > 0)
                    return false;
                else
                    return true;
            }
        }

        public bool IsConnectedToSomething
        {
            get
            {
                foreach (HashSet<Concept> verbList in metaConnections.Values)
                    if (verbList.Count > 0)
                        return true;
                return false;
            }
        }
        #endregion
    }
}