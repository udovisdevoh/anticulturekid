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
        public override void Repair(Concept subject)
        {
            ConnectionBranch flatBranch;
            ConnectionBranch optimizedBranch;

            foreach (Concept verb in Memory.TotalVerbList)
            {
                flatBranch = subject.GetFlatConnectionBranch(verb);
                optimizedBranch = subject.GetOptimizedConnectionBranch(verb);

                if (!RepairedFlatBranchCache.Contains(flatBranch))
                    RepairFlatBranch(flatBranch, optimizedBranch, subject, verb);
            }

            subject.IsFlatDirty = false;
            subject.IsOptimizedDirty = true;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Repair flat branch
        /// </summary>
        /// <param name="flatBranch">flat branch to repair</param>
        /// <param name="optimizedBranch">optimized branch</param>
        /// <param name="subject">subject</param>
        /// <param name="verb">verb</param>
        /// <returns>repaired flat branch</returns>
        private ConnectionBranch RepairFlatBranch(ConnectionBranch flatBranch, ConnectionBranch optimizedBranch, Concept subject, Concept verb)
        {
            int complementCount = 0;          

            flatBranch.ComplementConceptList.Clear();

            foreach (Concept complement in optimizedBranch.ComplementConceptList)
            {
                flatBranch.AddConnection(complement);
                flatBranch.SetProofTo(complement, new Proof(subject, verb, complement));
            }

            HashSet<ScheduledRepair> branchesToRepair;

            int i;
            do
            {
                complementCount = flatBranch.ComplementConceptList.Count;

                branchesToRepair = FlatBranchSelector.GetBranchesToRepair(flatBranch, optimizedBranch, subject, verb);
                foreach (ScheduledRepair scheduledRepair in branchesToRepair)
                    if (!RepairedFlatBranchCache.Contains(scheduledRepair.FlatBranch) && scheduledRepair.FlatBranch != flatBranch)
                        RepairFlatBranch(scheduledRepair.FlatBranch, scheduledRepair.OptimizedBranch, scheduledRepair.Subject, scheduledRepair.Verb);

                FlatBranchRepairer.FlattenDirectImplication(flatBranch, subject, verb);

                branchesToRepair = FlatBranchSelector.GetBranchesToRepair(flatBranch, optimizedBranch, subject, verb);
                foreach (ScheduledRepair scheduledRepair in branchesToRepair)
                    if (!RepairedFlatBranchCache.Contains(scheduledRepair.FlatBranch) && scheduledRepair.FlatBranch != flatBranch)
                        RepairFlatBranch(scheduledRepair.FlatBranch, scheduledRepair.OptimizedBranch, scheduledRepair.Subject, scheduledRepair.Verb);

                FlatBranchRepairer.FlattenLiffid(flatBranch, subject, verb);

                branchesToRepair = FlatBranchSelector.GetBranchesToRepair(flatBranch, optimizedBranch, subject, verb);
                foreach (ScheduledRepair scheduledRepair in branchesToRepair)
                    if (!RepairedFlatBranchCache.Contains(scheduledRepair.FlatBranch) && scheduledRepair.FlatBranch != flatBranch)
                        RepairFlatBranch(scheduledRepair.FlatBranch, scheduledRepair.OptimizedBranch, scheduledRepair.Subject, scheduledRepair.Verb);

                FlatBranchRepairer.FlattenMuct(flatBranch, subject, verb);

                branchesToRepair = FlatBranchSelector.GetBranchesToRepair(flatBranch, optimizedBranch, subject, verb);
                foreach (ScheduledRepair scheduledRepair in branchesToRepair)
                    if (!RepairedFlatBranchCache.Contains(scheduledRepair.FlatBranch) && scheduledRepair.FlatBranch != flatBranch)
                        RepairFlatBranch(scheduledRepair.FlatBranch, scheduledRepair.OptimizedBranch, scheduledRepair.Subject, scheduledRepair.Verb);

                FlatBranchRepairer.FlattenPositiveImply(flatBranch, subject, verb);

                branchesToRepair = FlatBranchSelector.GetBranchesToRepair(flatBranch, optimizedBranch, subject, verb);
                foreach (ScheduledRepair scheduledRepair in branchesToRepair)
                    if (!RepairedFlatBranchCache.Contains(scheduledRepair.FlatBranch) && scheduledRepair.FlatBranch != flatBranch)
                        RepairFlatBranch(scheduledRepair.FlatBranch, scheduledRepair.OptimizedBranch, scheduledRepair.Subject, scheduledRepair.Verb);

                FlatBranchRepairer.FlattenNegativeImply(flatBranch, subject, verb);
            }
            while (flatBranch.ComplementConceptList.Count != complementCount);

            flatBranch.IsDirty = false;

            RepairedFlatBranchCache.Add(flatBranch);

            return flatBranch;
        }
        #endregion
    }
}
