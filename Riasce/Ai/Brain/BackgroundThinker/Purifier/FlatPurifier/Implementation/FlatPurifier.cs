using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class FlatPurifier : AbstractFlatPurifier
    {
        #region Fields
        private ConnectionManager connectionManager;

        private Repairer repairer;
        #endregion

        #region Constructor
        public FlatPurifier(ConnectionManager connectionManager, Repairer repairer)
        {
            this.connectionManager = connectionManager;
            this.repairer = repairer;
        }
        #endregion

        #region Public Methods
        public override Trauma Purify(Concept concept)
        {
            //loop:
            //  repair the concept
            //  get the optimized connection for which the amount of flat obstruction is the largest
            //  remove the optimized connection
            //  repeat process until the most objectable optimized connection has no obstruction

            Trauma trauma = new Trauma();

            List<Concept> obstruction;
            List<Concept> mostObstructableConnection;
            List<Concept> flatConnectionSource;

            #region Finding obstructable connection in flat connections
            mostObstructableConnection = GetMostObstructableConnection(concept, out flatConnectionSource);
            if (mostObstructableConnection != null)
            {
                repairer.Repair(mostObstructableConnection[0], mostObstructableConnection[2]);
                connectionManager.UnPlug(mostObstructableConnection[0], mostObstructableConnection[1], mostObstructableConnection[2]);
                repairer.Repair(mostObstructableConnection[0], mostObstructableConnection[2]);

                obstruction = connectionManager.FindObstructionToPlug(flatConnectionSource[0], flatConnectionSource[1], flatConnectionSource[2], false);

                if (obstruction != null)
                    trauma.Add(mostObstructableConnection[0], mostObstructableConnection[1], mostObstructableConnection[2], obstruction[0], obstruction[1], obstruction[2]);
            }
            #endregion

            if (trauma.Count > 0)
            {
                FeelingMonitor.Add(FeelingMonitor.PURIFICATION);
                return trauma;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Private Methods
        public override List<Concept> GetMostObstructableConnection(Concept subject, out List<Concept> flatConnectionSource)
        {
            //flatConnectionSource = GetMostObstructableFlatConnection(subject);
            flatConnectionSource = GetSomeArbitraryObstructableFlatConnection(subject);

            if (flatConnectionSource == null)
                return null;

            Proof proof = flatConnectionSource[0].GetFlatConnectionBranch(flatConnectionSource[1]).GetProofTo(flatConnectionSource[2]);

            if (proof == null || proof.Count == 0)
                throw new PurificationException("This obstructable connection is not flat");

            return GetFirstOptimizedArgument(proof);
        }

        /// <summary>
        /// From proof, returns the first argument that is an optimized connection
        /// </summary>
        /// <param name="proof">proof to look into</param>
        /// <returns>From proof, returns the last argument that is an optimized connection</returns>
        private List<Concept> GetFirstOptimizedArgument(Proof proof)
        {
            Concept subject,verb,complement;
            foreach (List<Concept> argument in proof)
            {
                subject = argument[0];
                verb = argument[1];
                complement = argument[2];

                if (subject.IsOptimizedConnectedTo(verb, complement))
                {
                    return argument;
                }
            }
            return null;
        }

        private List<Concept> GetSomeArbitraryObstructableFlatConnection(Concept subject)
        {
            Concept verb;
            ConnectionBranch flatBranch;
            Proof proof = null;
            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in subject.FlatConnectionBranchList)
            {
                verb = verbAndBranch.Key;
                flatBranch = verbAndBranch.Value;
                foreach (Concept complement in flatBranch.ComplementConceptList)
                {
                    if (connectionManager.FindObstructionToPlugAllowInverseAndPermutableSide(subject,verb,complement,false) != null)
                    {
                        proof = subject.GetFlatConnectionBranch(verb).GetProofTo(complement);

                        if (!subject.GetOptimizedConnectionBranch(verb).ComplementConceptList.Contains(complement))
                        {
                            return new List<Concept>() { subject, verb, complement };
                        }
                    }
                }
            }

            return null;
        }

        private List<Concept> GetMostObstructableFlatConnection(Concept subject)
        {
            List<Concept> mostObstructableConnection = null;
            int currentObstructionCount;
            int maxObstructionCount = 0;

            Concept verb;
            ConnectionBranch flatBranch;
            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in subject.FlatConnectionBranchList)
            {
                verb = verbAndBranch.Key;
                flatBranch = verbAndBranch.Value;
                foreach (Concept complement in flatBranch.ComplementConceptList)
                {
                    currentObstructionCount = connectionManager.CountObstructionToPlug(subject, verb, complement, false);
                    if (currentObstructionCount > maxObstructionCount)
                    {
                        maxObstructionCount = currentObstructionCount;
                        mostObstructableConnection = new List<Concept>() { subject, verb, complement };
                    }
                }
            }
            return mostObstructableConnection;
        }
        #endregion
    }
}
