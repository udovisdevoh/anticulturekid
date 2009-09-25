using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public class Concept : AbstractConcept
    {
        #region Fields
        private MetaConnectionTree metaConnectionTreePositive = new MetaConnectionTree();

        private MetaConnectionTree metaConnectionTreeNegative = new MetaConnectionTree();
        
        private Dictionary<Concept, ConnectionBranch> optimizedConnectionBranchList = new Dictionary<Concept, ConnectionBranch>();

        private Dictionary<Concept, ConnectionBranch> flatConnectionBranchList = new Dictionary<Concept, ConnectionBranch>();

        private ImplyConnectionTree implyConnectionTreePositive = new ImplyConnectionTree();

        private ImplyConnectionTree implyConnectionTreeNegative = new ImplyConnectionTree();

        private HashSet<Condition> indexedImplyConditionList = new HashSet<Condition>();
        
        private bool isFlatDirty = false;

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

        #region Methods
        #region Operations on Optimized Connections
        public override void AddOptimizedConnection(Concept verbConcept, Concept complementConcept)
        {
            ConnectionBranch connectionBranch;
            if (!optimizedConnectionBranchList.TryGetValue(verbConcept, out connectionBranch))
            {
                connectionBranch = new ConnectionBranch();
                optimizedConnectionBranchList.Add(verbConcept, connectionBranch);
            }
            connectionBranch.AddConnection(complementConcept);
        }

        public override void RemoveOptimizedConnection(Concept verbConcept, Concept complementConcept)
        {
            ConnectionBranch connectionBranch;
            if (optimizedConnectionBranchList.TryGetValue(verbConcept, out connectionBranch))
                connectionBranch.RemoveConnection(complementConcept);
        }

        public override bool IsOptimizedConnectedTo(Concept verbConcept, Concept complementConcept)
        {
            ConnectionBranch connectionBranch;
            if (optimizedConnectionBranchList.TryGetValue(verbConcept, out connectionBranch))
                if (connectionBranch.IsConnectedTo(complementConcept))
                    return true;
            return false;
        }
        #endregion

        #region Operations on Flat Connections
        public override void AddFlatConnection(Concept verbConcept, Concept complementConcept)
        {
            ConnectionBranch connectionBranch;
            if (!flatConnectionBranchList.TryGetValue(verbConcept, out connectionBranch))
            {
                connectionBranch = new ConnectionBranch();
                flatConnectionBranchList.Add(verbConcept, connectionBranch);
            }
            connectionBranch.AddConnection(complementConcept);
        }

        public override void RemoveFlatConnection(Concept verbConcept, Concept complementConcept)
        {
            ConnectionBranch connectionBranch;
            if (flatConnectionBranchList.TryGetValue(verbConcept, out connectionBranch))
                connectionBranch.RemoveConnection(complementConcept);
        }

        public override bool IsFlatConnectedTo(Concept verbConcept, Concept complementConcept)
        {
            ConnectionBranch connectionBranch;
            if (flatConnectionBranchList.TryGetValue(verbConcept, out connectionBranch))
                if (connectionBranch.IsConnectedTo(complementConcept))
                    return true;
            return false;
        }

        public override void FlatDirthenFromOperatorList(HashSet<Concept> operatorList)
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
        public override void AddMetaConnection(string metaOperatorName, Concept complementConcept, bool connectionSens)
        {
            if (connectionSens == true)
                metaConnectionTreePositive.AddMetaConnection(metaOperatorName, complementConcept);
            else
                metaConnectionTreeNegative.AddMetaConnection(metaOperatorName, complementConcept);
        }

        public override void RemoveMetaConnection(string metaOperatorName, Concept complementConcept, bool connectionSens)
        {
            if (connectionSens == true)
                metaConnectionTreePositive.RemoveMetaConnection(metaOperatorName, complementConcept);
            else
                metaConnectionTreeNegative.RemoveMetaConnection(metaOperatorName, complementConcept);
        }

        public override bool IsMetaConnectedTo(string metaOperatorName, Concept complementConcept, bool connectionSens)
        {
            if (connectionSens == true)
                return metaConnectionTreePositive.IsMetaConnectedTo(metaOperatorName, complementConcept);
            else
                return metaConnectionTreeNegative.IsMetaConnectedTo(metaOperatorName, complementConcept);
        }
        #endregion

        #region Operations on implyConnections
        public override void AddImplyConnection(Concept complement, Condition condition, bool isPositive)
        {
            if (isPositive)
                implyConnectionTreePositive.AddConnection(complement, condition);
            else
                implyConnectionTreeNegative.AddConnection(complement, condition);
        }

        public override void RemoveImplyConnection(Concept complement, Condition condition, bool isPositive)
        {
            if (isPositive)
                implyConnectionTreePositive.RemoveConnection(this, complement, condition);
            else
                implyConnectionTreeNegative.RemoveConnection(this, complement, condition);
        }

        public override bool TestImplyConnection(Concept complement, Condition condition, bool isPositive)
        {
            if (isPositive)
                return implyConnectionTreePositive.TestConnection(complement, condition);
            else
                return implyConnectionTreeNegative.TestConnection(complement, condition);
        }

        public override void AddIndexToImplyCondition(Condition condition)
        {
            indexedImplyConditionList.Add(condition);
        }

        public override void RemoveIndexToImplyCondition(Condition condition)
        {
            indexedImplyConditionList.Remove(condition);
        }

        public override void ClearIndexToImplyCondition()
        {
            indexedImplyConditionList.Clear();
        }
        #endregion

        #region Getters
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
        #endregion

        #region Properties
        public override bool IsConnectedToSomething
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

        public override bool IsFlatDirty
        {
            get {return isFlatDirty;}
            set {isFlatDirty = value;}
        }

        public override bool IsOptimizedDirty
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

        public MetaConnectionTree MetaConnectionTreePositive
        {
            get { return metaConnectionTreePositive; }
        }

        public MetaConnectionTree MetaConnectionTreeNegative
        {
            get { return metaConnectionTreeNegative; }
        }

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

        public string DebuggerName
        {
            get {return debuggerName;}
            set {debuggerName = value;}
        }

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

        public ImplyConnectionTree ImplyConnectionTreePositive
        {
            get { return implyConnectionTreePositive; }
        }

        public ImplyConnectionTree ImplyConnectionTreeNegative
        {
            get { return implyConnectionTreeNegative; }
        }

        public HashSet<Condition> IndexedImplyConditionList
        {
            get { return indexedImplyConditionList; }
        }
        #endregion
    }
}
