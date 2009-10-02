using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a concept which can also be an operator.
    /// Facade to FlatConnectionSet, OptimizedConnectionSet, ImplyConnectionTree and MetaConnectionSet
    /// </summary>
    public class Concept
    {
        #region Fields
        /// <summary>
        /// Positive MetaConnection tree
        /// </summary>
        private MetaConnectionTree metaConnectionTreePositive = new MetaConnectionTree();

        /// <summary>
        /// Negative metaConnection tree
        /// </summary>
        private MetaConnectionTree metaConnectionTreeNegative = new MetaConnectionTree();
        
        /// <summary>
        /// Optimized connection branch list
        /// </summary>
        private Dictionary<Concept, ConnectionBranch> optimizedConnectionBranchList = new Dictionary<Concept, ConnectionBranch>();

        /// <summary>
        /// Flat connection branch list
        /// </summary>
        private Dictionary<Concept, ConnectionBranch> flatConnectionBranchList = new Dictionary<Concept, ConnectionBranch>();

        /// <summary>
        /// Positive imply connection tree
        /// </summary>
        private ImplyConnectionTree implyConnectionTreePositive = new ImplyConnectionTree();

        /// <summary>
        /// Negative imply connection tree
        /// </summary>
        private ImplyConnectionTree implyConnectionTreeNegative = new ImplyConnectionTree();

        /// <summary>
        /// Indexed imply condition list
        /// </summary>
        private HashSet<Condition> indexedImplyConditionList = new HashSet<Condition>();
        
        /// <summary>
        /// Whether the concept is flat dirty or not
        /// </summary>
        private bool isFlatDirty = false;

        /// <summary>
        /// Concept name in debugger (debugger only)
        /// </summary>
        private string debuggerName;
        #endregion

        #region Constructors
        /// <summary>
        /// Create a concept for general use
        /// </summary>
        public Concept()
        {
            this.debuggerName = null;
        }

        /// <summary>
        /// Create a concept object for debugging and unit testing
        /// </summary>
        /// <param name="debuggerName">name displayed in debugger</param>
        public Concept(string debuggerName)
        {
            this.debuggerName = debuggerName;
        }
        #endregion

        #region Operations on Optimized Connections
        /// <summary>
        /// Add a optimized connection
        /// </summary>
        /// <param name="verbConcept">verb concept</param>
        /// <param name="complementConcept">complement concept</param>
        /// <returns>true if optimized connection was created because it didn't exist</returns>
        public bool AddOptimizedConnection(Concept verbConcept, Concept complementConcept)
        {
            ConnectionBranch connectionBranch;
            if (!optimizedConnectionBranchList.TryGetValue(verbConcept, out connectionBranch))
            {
                connectionBranch = new ConnectionBranch();
                optimizedConnectionBranchList.Add(verbConcept, connectionBranch);
            }

            if (!connectionBranch.ComplementConceptList.Contains(complementConcept))
            {
                connectionBranch.AddConnection(complementConcept);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove an optimized connection
        /// </summary>
        /// <param name="verbConcept">verb concept</param>
        /// <param name="complementConcept">complement concept</param>
        /// <returns>true if optimized connection was removed because it did exist</returns>
        public bool RemoveOptimizedConnection(Concept verbConcept, Concept complementConcept)
        {
            ConnectionBranch connectionBranch;
            if (optimizedConnectionBranchList.TryGetValue(verbConcept, out connectionBranch))
            {
                connectionBranch.RemoveConnection(complementConcept);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if optimized connection exists
        /// </summary>
        /// <param name="verbConcept">verb concept</param>
        /// <param name="complementConcept">complement concept</param>
        /// <returns>True if optimized connection exists</returns>
        public bool IsOptimizedConnectedTo(Concept verbConcept, Concept complementConcept)
        {
            ConnectionBranch connectionBranch;
            if (optimizedConnectionBranchList.TryGetValue(verbConcept, out connectionBranch))
                if (connectionBranch.IsConnectedTo(complementConcept))
                    return true;
            return false;
        }
        #endregion

        #region Operations on Flat Connections
        /// <summary>
        /// Add a flat connection
        /// </summary>
        /// <param name="verbConcept">verb concept</param>
        /// <param name="complementConcept">complement concept</param>
        public void AddFlatConnection(Concept verbConcept, Concept complementConcept)
        {
            ConnectionBranch connectionBranch;
            if (!flatConnectionBranchList.TryGetValue(verbConcept, out connectionBranch))
            {
                connectionBranch = new ConnectionBranch();
                flatConnectionBranchList.Add(verbConcept, connectionBranch);
            }
            connectionBranch.AddConnection(complementConcept);
        }

        /// <summary>
        /// Remove a flat connection
        /// </summary>
        /// <param name="verbConcept">verb concept</param>
        /// <param name="complementConcept">complement concept</param>
        public void RemoveFlatConnection(Concept verbConcept, Concept complementConcept)
        {
            ConnectionBranch connectionBranch;
            if (flatConnectionBranchList.TryGetValue(verbConcept, out connectionBranch))
                connectionBranch.RemoveConnection(complementConcept);
        }

        /// <summary>
        /// Returns true if flat connection exists
        /// </summary>
        /// <param name="verbConcept">verb concept</param>
        /// <param name="complementConcept">complement concept</param>
        /// <returns>True if flat connection exists</returns>
        public bool IsFlatConnectedTo(Concept verbConcept, Concept complementConcept)
        {
            ConnectionBranch connectionBranch;
            if (flatConnectionBranchList.TryGetValue(verbConcept, out connectionBranch))
                if (connectionBranch.IsConnectedTo(complementConcept))
                    return true;
            return false;
        }

        /// <summary>
        /// Dirthen concept's flat representation for each branches matching operator list
        /// </summary>
        /// <param name="operatorList">list of operators to match</param>
        public void FlatDirthenFromOperatorList(HashSet<Concept> operatorList)
        {
            ConnectionBranch flatBranch;
            foreach (Concept verb in operatorList)
            {
                flatBranch = GetFlatConnectionBranch(verb);
                flatBranch.IsDirty = true;
            }
        }
        #endregion

        #region Operations on metaConnections
        /// <summary>
        /// Adds a connection to the other concept
        /// </summary>
        /// <param name="metaOperator">metaOperator name</param>
        /// <param name="concept">complement concept</param>
        public void AddMetaConnection(string metaOperatorName, Concept complementConcept, bool connectionSens)
        {
            if (connectionSens == true)
                metaConnectionTreePositive.AddMetaConnection(metaOperatorName, complementConcept);
            else
                metaConnectionTreeNegative.AddMetaConnection(metaOperatorName, complementConcept);
        }

        /// <summary>
        /// Remove a connection to the other concept
        /// </summary>
        /// <param name="metaOperator">metaOperator name</param>
        /// <param name="concept">complement concept</param>
        public void RemoveMetaConnection(string metaOperatorName, Concept complementConcept, bool connectionSens)
        {
            if (connectionSens == true)
                metaConnectionTreePositive.RemoveMetaConnection(metaOperatorName, complementConcept);
            else
                metaConnectionTreeNegative.RemoveMetaConnection(metaOperatorName, complementConcept);
        }

        /// <summary>
        /// Test whether meta connection exist or not
        /// </summary>
        /// <param name="metaOperator">metaOperator name</param>
        /// <param name="concept">complement concept</param>
        /// <returns>If connection exist: true, else: false</returns>
        public bool IsMetaConnectedTo(string metaOperatorName, Concept complementConcept, bool connectionSens)
        {
            if (connectionSens == true)
                return metaConnectionTreePositive.IsMetaConnectedTo(metaOperatorName, complementConcept);
            else
                return metaConnectionTreeNegative.IsMetaConnectedTo(metaOperatorName, complementConcept);
        }
        #endregion

        #region Operations on implyConnections
        /// <summary>
        /// Add imply connection
        /// </summary>
        /// <param name="complement">complement</param>
        /// <param name="condition">condition</param>
        /// <param name="isPositive">connection sense</param>
        public void AddImplyConnection(Concept complement, Condition condition, bool isPositive)
        {
            if (isPositive)
                implyConnectionTreePositive.AddConnection(complement, condition);
            else
                implyConnectionTreeNegative.AddConnection(complement, condition);
        }

        /// <summary>
        /// Remove imply connection
        /// </summary>
        /// <param name="complement">complement</param>
        /// <param name="condition">condition</param>
        /// <param name="isPositive">connection sense</param>
        public void RemoveImplyConnection(Concept complement, Condition condition, bool isPositive)
        {
            if (isPositive)
                implyConnectionTreePositive.RemoveConnection(this, complement, condition);
            else
                implyConnectionTreeNegative.RemoveConnection(this, complement, condition);
        }

        /// <summary>
        /// Remove imply connection
        /// </summary>
        /// <param name="complement">complement</param>
        /// <param name="condition">condition</param>
        /// <param name="isPositive">connection sense</param>
        /// <returns>whether a matching connection exists</returns>
        public bool TestImplyConnection(Concept complement, Condition condition, bool isPositive)
        {
            if (isPositive)
                return implyConnectionTreePositive.TestConnection(complement, condition);
            else
                return implyConnectionTreeNegative.TestConnection(complement, condition);
        }

        /// <summary>
        /// Add an index to imply condition to fasten search
        /// </summary>
        /// <param name="condition">imply condition</param>
        public void AddIndexToImplyCondition(Condition condition)
        {
            indexedImplyConditionList.Add(condition);
        }

        /// <summary>
        /// Remove index to imply condition
        /// </summary>
        /// <param name="condition">imply condition</param>
        public void RemoveIndexToImplyCondition(Condition condition)
        {
            indexedImplyConditionList.Remove(condition);
        }

        /// <summary>
        /// Clear index to imply condition
        /// </summary>
        public void ClearIndexToImplyCondition()
        {
            indexedImplyConditionList.Clear();
        }
        #endregion

        #region Getters
        /// <summary>
        /// Get concepts's flat connection branch from prodived verb
        /// </summary>
        /// <param name="verb">provided verb</param>
        /// <returns>flat connection branch on prodived verb</returns>
        public ConnectionBranch GetFlatConnectionBranch(Concept verb)
        {
            ConnectionBranch connectionBranch;
            if (!flatConnectionBranchList.TryGetValue(verb, out connectionBranch))
            {
                connectionBranch = new ConnectionBranch();
                flatConnectionBranchList.Add(verb, connectionBranch);
            }
            return connectionBranch;
        }

        /// <summary>
        /// Get concepts's optimized connection branch from prodived verb
        /// </summary>
        /// <param name="verb">provided verb</param>
        /// <returns>optimized connection branch on prodived verb</returns>
        public ConnectionBranch GetOptimizedConnectionBranch(Concept verb)
        {
            ConnectionBranch connectionBranch;
            if (!optimizedConnectionBranchList.TryGetValue(verb, out connectionBranch))
            {
                connectionBranch = new ConnectionBranch();
                optimizedConnectionBranchList.Add(verb, connectionBranch);
            }
            return connectionBranch;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Whether concept or operator is connected or metaConnected to something or not
        /// </summary>
        public bool IsConnectedToSomething
        {
            get
            {
                if (metaConnectionTreePositive.IsConnectedToSomething)
                    return true;
                else if (metaConnectionTreeNegative.IsConnectedToSomething)
                    return true;

                foreach (ConnectionBranch optimizedBranch in optimizedConnectionBranchList.Values)
                    if (optimizedBranch.ComplementConceptList.Count > 0)
                        return true;

                if (indexedImplyConditionList.Count > 0)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Whether the concept's flat representation need repair or not
        /// </summary>
        public bool IsFlatDirty
        {
            get {return isFlatDirty;}
            set {isFlatDirty = value;}
        }

        /// <summary>
        /// Whether the concept's optimized representation need repair or not
        /// </summary>
        public bool IsOptimizedDirty
        {
            get
            {
                foreach (ConnectionBranch branch in optimizedConnectionBranchList.Values)
                    if (branch.IsDirty)
                        return true;

                return false;
            }

            set
            {
                foreach (ConnectionBranch branch in optimizedConnectionBranchList.Values)
                    branch.IsDirty = value;

                foreach (Concept verb in flatConnectionBranchList.Keys)
                    GetOptimizedConnectionBranch(verb).IsDirty = value;
            }
        }

        /// <summary>
        /// Positive MetaConnection tree
        /// </summary>
        public MetaConnectionTree MetaConnectionTreePositive
        {
            get { return metaConnectionTreePositive; }
        }

        /// <summary>
        /// Negative metaConnection tree
        /// </summary>
        public MetaConnectionTree MetaConnectionTreeNegative
        {
            get { return metaConnectionTreeNegative; }
        }

        /// <summary>
        /// Flat connection branch list
        /// </summary>
        public Dictionary<Concept, ConnectionBranch> FlatConnectionBranchList
        {
            get
            {
                return flatConnectionBranchList;
            }
            set
            {
                flatConnectionBranchList = value;
            }
        }

        /// <summary>
        /// Optimized connection branch list
        /// </summary>
        public Dictionary<Concept, ConnectionBranch> OptimizedConnectionBranchList
        {
            get
            {
                return optimizedConnectionBranchList;
            }

            set
            {
                optimizedConnectionBranchList = value;
            }
        }

        /// <summary>
        /// Concept name in debugger (debugger only)
        /// </summary>
        public string DebuggerName
        {
            get {return debuggerName;}
            set {debuggerName = value;}
        }

        /// <summary>
        /// All concepts flat plugged to this concept
        /// </summary>
        public HashSet<Concept> ConceptFlatPluggedTo
        {
            get
            {
                HashSet<Concept> conceptFlatPluggedTo = new HashSet<Concept>();

                foreach (ConnectionBranch flatBranch in FlatConnectionBranchList.Values)
                    conceptFlatPluggedTo.UnionWith(flatBranch.ComplementConceptList);

                return conceptFlatPluggedTo;
            }
        }

        /// <summary>
        /// Positive imply connection tree
        /// </summary>
        public ImplyConnectionTree ImplyConnectionTreePositive
        {
            get { return implyConnectionTreePositive; }
        }

        /// <summary>
        /// Negative imply connection tree
        /// </summary>
        public ImplyConnectionTree ImplyConnectionTreeNegative
        {
            get { return implyConnectionTreeNegative; }
        }

        /// <summary>
        /// Indexed imply condition list
        /// </summary>
        public HashSet<Condition> IndexedImplyConditionList
        {
            get { return indexedImplyConditionList; }
        }
        #endregion
    }
}
