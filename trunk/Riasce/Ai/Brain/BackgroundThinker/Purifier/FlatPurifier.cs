using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used to remove implicity connections that make no sens
    /// by removing explicit connections on which they are based
    /// </summary>
    class FlatPurifier
    {
        #region Public Methods
        /// <summary>
        /// Remove inconsistant FLAT connections from concept
        /// For instance, cat isa animal, cat contradict animal, isa cant contradict
        /// Ooops! Do we keep cat isa animal or do we keep cat contradict animal?
        /// </summary>
        /// <param name="concept">concept to repair</param>
        /// <returns>Trauma object about removed connections</returns>
        public Trauma Purify(Concept concept)
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
                Repairer.Repair(mostObstructableConnection[0], mostObstructableConnection[2]);
                ConnectionManager.UnPlug(mostObstructableConnection[0], mostObstructableConnection[1], mostObstructableConnection[2]);
                Repairer.Repair(mostObstructableConnection[0], mostObstructableConnection[2]);

                obstruction = ConnectionManager.FindObstructionToPlug(flatConnectionSource[0], flatConnectionSource[1], flatConnectionSource[2], false);

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

        /// <summary>
        /// Return the most obstructable connection for provided concept, but look deep in flat connections
        /// </summary>
        /// <param name="subject">provided concept</param>
        /// <param name="flatConnectionSource">source flat connection for which obstruction can be found</param>
        /// <returns>If most obstructable connection has obstruction, return obstructable connection, else: null</returns>
        public List<Concept> GetMostObstructableConnection(Concept subject, out List<Concept> flatConnectionSource)
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
        #endregion

        #region Private Methods
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

        /// <summary>
        /// We get some randomly chosen implicit connection that are being
        /// obstructed by another connection
        /// </summary>
        /// <param name="subject">subject concept to perform selection on</param>
        /// <returns>randomly chosen implicit connection that are being
        /// obstructed by another connection (these implicit connection are
        /// about subject concept)</returns>
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
                    if (ConnectionManager.FindObstructionToPlugAllowInverseAndPermutableSide(subject,verb,complement,false) != null)
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

        /// <summary>
        /// We try to find the most obstructable implicit connections
        /// </summary>
        /// <param name="subject">subject concept to perform selection on</param>
        /// <returns>most obstructable implicit connections about subject</returns>
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
                    currentObstructionCount = ConnectionManager.CountObstructionToPlug(subject, verb, complement, false);
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
