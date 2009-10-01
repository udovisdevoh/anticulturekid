using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AntiCulture.Kid
{
    class ParallelFlattenizer : AbstractFlattenizer
    {
        #region Public Methods
        /// Repair a concept's flat representation and regenerate its optimized representation
        /// so no useless connection persist.
        /// THIS METHOD MUST ONLY BE USED BY REPAIRER CLASS!!!
        /// </summary>
        /// <param name="subject">concept to repair</param>
        /// <param name="repairedBranches">provided HashSet to rememebr which branches were repaired</param>
        public override void Repair(Concept subject, HashSet<ConnectionBranch> repairedBranches)
        {
            this.repairedBranches = repairedBranches;

            ConnectionBranch flatBranch;
            ConnectionBranch optimizedBranch;

            foreach (Concept verb in Memory.TotalVerbList)
            {
                flatBranch = subject.GetFlatConnectionBranch(verb);
                optimizedBranch = subject.GetOptimizedConnectionBranch(verb);

                if (repairedBranches.Contains(flatBranch))
                    RepairFlatBranch(flatBranch, optimizedBranch, subject, verb, new AutoResetEvent(false));
            }

            subject.IsFlatDirty = false;
            subject.IsOptimizedDirty = true;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Use internally to repair flat branches from optimized branches
        /// </summary>
        /// <param name="optimizedBranch">source optimized branch</param>
        /// <param name="optimizedBranch">flat branch to repair</param>
        /// <returns>repaired flat branch</returns>
        private ConnectionBranch RepairFlatBranch(ConnectionBranch flatBranch, ConnectionBranch optimizedBranch, Concept subject, Concept verb, AutoResetEvent autoResetEvent)
        {
            int complementCount = 0;
            int complementCountBeforeLoop = 0;

            repairedBranches.Add(flatBranch);

            Dictionary<ConnectionBranch, ConnectionBranch> branchesToRepair;
            List<AutoResetEvent> waitingBranchResetEventList;

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
                        branchesToRepair = FlatBranchSelector.GetBranchesToRepair(flatBranch, optimizedBranch, subject, verb, repairedBranches);
                        waitingBranchResetEventList = new List<AutoResetEvent>();
                        foreach (KeyValuePair<ConnectionBranch, ConnectionBranch> currentBranch in branchesToRepair)
                            waitingBranchResetEventList.Add(RepairFlatBranchInOtherThread(currentBranch.Key, currentBranch.Value, subject, verb));
                        WaitForBranchesToBeRepaired(waitingBranchResetEventList);
                        complementCount = flatBranch.ComplementConceptList.Count;
                    }

                    if (i == 0)
                        FlatBranchRepairer.FlattenDirectImplication(flatBranch, subject, verb,repairedBranches);
                    else if (i == 1)
                        FlatBranchRepairer.FlattenLiffid(flatBranch, subject, verb, repairedBranches);
                    else if (i == 2)
                        FlatBranchRepairer.FlattenMuct(flatBranch, subject, verb, repairedBranches);
                    else if (i == 3)
                        FlatBranchRepairer.FlattenPositiveImply(flatBranch, subject, verb, repairedBranches);
                    else
                        FlatBranchRepairer.FlattenNegativeImply(flatBranch, subject, verb, repairedBranches);
                }
            }
            while (flatBranch.ComplementConceptList.Count != complementCount || flatBranch.ComplementConceptList.Count != complementCountBeforeLoop);

            flatBranch.IsDirty = false;

            autoResetEvent.Set();

            return flatBranch;
        }

        /// <summary>
        /// Repair flat branch inside another thread
        /// </summary>
        /// <param name="flatBranch">flat branch</param>
        /// <param name="optimizedBranch">optimized branch</param>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <returns>autoResetEvent reference to know when repair is done</returns>
        private AutoResetEvent RepairFlatBranchInOtherThread(ConnectionBranch flatBranch, ConnectionBranch optimizedBranch, Concept subject, Concept verb)
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);

            object[] arguments = { flatBranch, optimizedBranch, subject, verb, autoResetEvent };

            ThreadPool.QueueUserWorkItem(new WaitCallback(RepairFlatBranchInOtherThreadInside), arguments);

            return autoResetEvent;
        }

        /// <summary>
        /// Used only by threadPool to send queued work
        /// </summary>
        /// <param name="obj">arguments</param>
        private void RepairFlatBranchInOtherThreadInside(object obj)
        {
            object[] arguments = (object[])obj;
 
            ConnectionBranch flatBranch = (ConnectionBranch)arguments[0];
            ConnectionBranch optimizedBranch = (ConnectionBranch)arguments[1];
            Concept subject = (Concept)arguments[2];
            Concept verb = (Concept)arguments[3];
            AutoResetEvent autoResetEvent = (AutoResetEvent)arguments[4];

            RepairFlatBranch(flatBranch, optimizedBranch, subject, verb, autoResetEvent);
        }

        /// <summary>
        /// Wait for branches to be repaired in their own threads
        /// </summary>
        /// <param name="waitingBranchResetEventList">waiting branch event list</param>
        private void WaitForBranchesToBeRepaired(List<AutoResetEvent> waitingBranchResetEventList)
        {
            foreach (AutoResetEvent autoResetEvent in waitingBranchResetEventList)
                autoResetEvent.WaitOne();
        }
        #endregion
    }
}
