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
            int complementCountBeforeLoop = 0;

            RepairedFlatBranchCache.Add(flatBranch);

            HashSet<ScheduledRepair> branchesToRepair;

            flatBranch.ComplementConceptList.Clear();

            foreach (Concept complement in optimizedBranch.ComplementConceptList)
            {
                flatBranch.AddConnection(complement);
                flatBranch.SetProofTo(complement, new Proof(subject, verb, complement));
            }

            int i;
            do
            {
                complementCountBeforeLoop = flatBranch.ComplementConceptList.Count;

                for (i = 0; i < 5; i++)
                {
                    if (i == 0 || complementCount != flatBranch.ComplementConceptList.Count)
                    {
                        branchesToRepair = FlatBranchSelector.GetBranchesToRepair(flatBranch, optimizedBranch, subject, verb);
                        foreach (ScheduledRepair scheduledRepair in branchesToRepair)
                            if (!RepairedFlatBranchCache.Contains(scheduledRepair.FlatBranch))
                                RepairFlatBranch(scheduledRepair.FlatBranch, scheduledRepair.OptimizedBranch, scheduledRepair.Subject, scheduledRepair.Verb);
                        complementCount = flatBranch.ComplementConceptList.Count;
                    }

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
            while (flatBranch.ComplementConceptList.Count != complementCount || flatBranch.ComplementConceptList.Count != complementCountBeforeLoop);

            flatBranch.IsDirty = false;

            return flatBranch;
        }
        #endregion
    }
}
