using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class SerialFlattenizer : AbstractFlattenizer
    {
        #region Public Methods
        /// Repair a concept's flat representation and regenerate its optimized representation
        /// so no useless connection persist.
        /// THIS METHOD MUST ONLY BE USED BY REPAIRER CLASS!!!
        /// </summary>
        /// <param name="subject">concept to repair</param>
        /// <param name="repairedBranches">provided HashSet to rememebr which branches were repaired</param>
        /// <param name="verbConnectionCache">provided cache to remember flattenized metaConnections</param>
        public override void Repair(Concept subject, HashSet<ConnectionBranch> repairedBranches, VerbMetaConnectionCache verbConnectionCache)
        {
            this.repairedBranches = repairedBranches;
            this.verbMetaConnectionCache = verbConnectionCache;

            ConnectionBranch flatBranch;
            ConnectionBranch optimizedBranch;

            foreach (Concept verb in Memory.TotalVerbList)
            {
                flatBranch = subject.GetFlatConnectionBranch(verb);
                optimizedBranch = subject.GetOptimizedConnectionBranch(verb);
                flatBranch = RepairFlatBranch(flatBranch, optimizedBranch, subject, verb);
            }

            subject.IsFlatDirty = false;
            subject.IsOptimizedDirty = true;
        }
        #endregion

        #region Private Methods
        private ConnectionBranch RepairFlatBranch(ConnectionBranch flatBranch, ConnectionBranch optimizedBranch, Concept subject, Concept verb)
        {
            int complementCount = 0;

            repairedBranches.Add(flatBranch);

            Dictionary<ConnectionBranch, ConnectionBranch> branchesToRepair;

            flatBranch.ComplementConceptList.Clear();

            foreach (Concept complement in optimizedBranch.ComplementConceptList)
            {
                flatBranch.AddConnection(complement);
                flatBranch.SetProofTo(complement, new Proof(subject, verb, complement));
            }

            FlatBranchRepairer.VerbMetaConnectionCache = verbMetaConnectionCache;

            int i;
            do
            {
                for (i = 0; i < 5; i++)
                {
                    branchesToRepair = FlatBranchSelector.GetBranchesToRepair(flatBranch, optimizedBranch, subject, verb, repairedBranches, verbMetaConnectionCache);
                    foreach (KeyValuePair<ConnectionBranch, ConnectionBranch> currentBranch in branchesToRepair)
                        RepairFlatBranch(currentBranch.Key, currentBranch.Value, subject, verb);
                    complementCount = flatBranch.ComplementConceptList.Count;

                    if (i == 0)
                        FlatBranchRepairer.FlattenDirectImplication(flatBranch, subject, verb);
                    else if (i == 1)
                        FlatBranchRepairer.FlattenLiffid(flatBranch, subject, verb);
                    else if (i == 2)
                        FlatBranchRepairer.FlattenMuct(flatBranch, subject, verb);
                    else if (i == 3)
                        FlatBranchRepairer.FlattenPositiveImply(flatBranch, subject, verb);
                    else
                        FlatBranchRepairer.FlattenNegativeImply(flatBranch, subject, verb);
                }
            }
            while (flatBranch.ComplementConceptList.Count != complementCount);

            flatBranch.IsDirty = false;

            return flatBranch;
        }
        #endregion
    }
}
