using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a concept which can also be an operator.
    /// Facade to FlatConnectionSet, OptimizedConnectionSet and MetaConnectionSet
    /// </summary>
    public abstract class AbstractConcept
    {
        #region Operations on optimized connections
        /// <summary>
        /// Add a optimized connection
        /// </summary>
        /// <param name="verbConcept">verb concept</param>
        /// <param name="complementConcept">complement concept</param>
        public abstract void AddOptimizedConnection(Concept verbConcept, Concept complementConcept);

        /// <summary>
        /// Remove an optimized connection
        /// </summary>
        /// <param name="verbConcept">verb concept</param>
        /// <param name="complementConcept">complement concept</param>
        public abstract void RemoveOptimizedConnection(Concept verbConcept, Concept complementConcept);

        /// <summary>
        /// Returns true if optimized connection exists
        /// </summary>
        /// <param name="verbConcept">verb concept</param>
        /// <param name="complementConcept">complement concept</param>
        /// <returns>True if optimized connection exists</returns>
        public abstract bool IsOptimizedConnectedTo(Concept verbConcept, Concept complementConcept);
        #endregion

        #region Operations of flat connections
        /// <summary>
        /// Add a flat connection
        /// </summary>
        /// <param name="verbConcept">verb concept</param>
        /// <param name="complementConcept">complement concept</param>
        public abstract void AddFlatConnection(Concept verbConcept, Concept complementConcept);

        /// <summary>
        /// Remove a flat connection
        /// </summary>
        /// <param name="verbConcept">verb concept</param>
        /// <param name="complementConcept">complement concept</param>
        public abstract void RemoveFlatConnection(Concept verbConcept, Concept complementConcept);

        /// <summary>
        /// Returns true if flat connection exists
        /// </summary>
        /// <param name="verbConcept">verb concept</param>
        /// <param name="complementConcept">complement concept</param>
        /// <returns>True if flat connection exists</returns>
        public abstract bool IsFlatConnectedTo(Concept verbConcept, Concept complementConcept);

        /// <summary>
        /// Dirthen concept's flat representation for each branches matching operator list
        /// </summary>
        /// <param name="operatorList">list of operators to match</param>
        public abstract void FlatDirthenFromOperatorList(HashSet<Concept> operatorList);
        #endregion

        #region Operations on meta connection
        /// <summary>
        /// Adds a connection to the other concept
        /// </summary>
        /// <param name="metaOperator">metaOperator name</param>
        /// <param name="concept">complement concept</param>
        public abstract void AddMetaConnection(string metaOperatorName, Concept complementConcept, bool connectionSens);

        /// <summary>
        /// Remove a connection to the other concept
        /// </summary>
        /// <param name="metaOperator">metaOperator name</param>
        /// <param name="concept">complement concept</param>
        public abstract void RemoveMetaConnection(string metaOperatorName, Concept complementConcept, bool connectionSens);

        /// <summary>
        /// Test whether meta connection exist or not
        /// </summary>
        /// <param name="metaOperator">metaOperator name</param>
        /// <param name="concept">complement concept</param>
        /// <returns>If connection exist: true, else: false</returns>
        public abstract bool IsMetaConnectedTo(string metaOperatorName, Concept complementConcept, bool connectionSens);
        #endregion

        #region Operations of implyConnections
        public abstract void AddImplyConnection(Concept complement, Condition condition, bool isPositive);

        public abstract void RemoveImplyConnection(Concept complement, Condition condition, bool isPositive);

        public abstract bool TestImplyConnection(Concept complement, Condition condition, bool isPositive);

        public abstract void AddIndexToImplyCondition(Condition condition);

        public abstract void RemoveIndexToImplyCondition(Condition condition);

        public abstract void ClearIndexToImplyCondition();
        #endregion

        /// <summary>
        /// Whether the concept's flat representation need repair or not
        /// </summary>
        public abstract bool IsFlatDirty
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the concept's optimized representation need repair or not
        /// </summary>
        public abstract bool IsOptimizedDirty
        {
            get;
            set;
        }

        /// <summary>
        /// Whether concept or operator is connected or metaConnected to something or not
        /// </summary>
        public abstract bool IsConnectedToSomething
        {
            get;
        }
    }
}
