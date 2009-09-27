using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used to remove useless connections from concepts
    /// </summary>
    class Optimizer
    {
        #region Public Methods
        /// <summary>
        /// Repair a concept's optimized representation
        /// so no useless connection persist
        /// </summary>
        /// <param name="concept">Concept to repair</param>
        public void Repair(Concept concept)
        {
            if (concept.IsFlatDirty)
                throw new OptimizationException("Repair flat representation before repairing optimized representation");

            //We erase optimized representation
            //We copy every flat connection with argumentless proofs into optimized representation

            concept.OptimizedConnectionBranchList = BuildOptimizedFromFlat(concept.FlatConnectionBranchList);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Rebuild optimized branches from flat branches where proof has no argument
        /// </summary>
        /// <param name="optimizedConnectionBranchList">optimized branch list</param>
        /// <returns>Repaired optimized branches</returns>
        private Dictionary<Concept, ConnectionBranch> BuildOptimizedFromFlat(Dictionary<Concept, ConnectionBranch> flatConnectionBranchList)
        {
            Dictionary<Concept, ConnectionBranch> optimizedConnectionBranchList = new Dictionary<Concept, ConnectionBranch>();

            Concept verb;
            ConnectionBranch flatBranch;
            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndFlatBranch in flatConnectionBranchList)
            {
                verb = verbAndFlatBranch.Key;
                flatBranch = verbAndFlatBranch.Value;

                ConnectionBranch optimizedBranch = RepairOptimizedBranch(flatBranch);
                optimizedConnectionBranchList.Add(verb, optimizedBranch);
            }
            return optimizedConnectionBranchList;
        }

        /// <summary>
        /// Copy data from flat branch to optimized branch where proof has no argument
        /// </summary>
        /// <param name="flatBranch">source flat branch</param>
        /// <returns>optimized branch</returns>
        private ConnectionBranch RepairOptimizedBranch(ConnectionBranch flatBranch)
        {
            ConnectionBranch optimizedBranch = new ConnectionBranch();
            foreach (Concept complement in flatBranch.ComplementConceptList)
                if (flatBranch.GetProofTo(complement).Count == 0)
                    optimizedBranch.AddConnection(complement);

            return optimizedBranch;
        }
        #endregion
    }
}
