using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class OptimizedPurifier
    {
        #region Fields
        private ConnectionManager connectionManager;

        private Repairer repairer;
        #endregion

        #region Constructor
        public OptimizedPurifier(ConnectionManager connectionManager, Repairer repairer)
        {
            this.connectionManager = connectionManager;
            this.repairer = repairer;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Remove inconsistant OPTIMIZED connections from concept
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

            #region Finding obstructable connection in optimized connections
            do
            {
                repairer.Repair(concept);
                mostObstructableConnection = GetMostObstructableConnection(concept);
                if (mostObstructableConnection != null)
                {
                    connectionManager.UnPlug(mostObstructableConnection[0], mostObstructableConnection[1], mostObstructableConnection[2]);
                    obstruction = connectionManager.FindObstructionToPlug(mostObstructableConnection[0], mostObstructableConnection[1], mostObstructableConnection[2], false);
                    trauma.Add(mostObstructableConnection[0], mostObstructableConnection[1], mostObstructableConnection[2], obstruction[0], obstruction[1], obstruction[2]);
                }
            } while (mostObstructableConnection != null);
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
        /// Remove inconsistant OPTIMIZED connections from concepts in conceptCollection
        /// For instance, cat isa animal, cat contradict animal, isa cant contradict
        /// Ooops! Do we keep cat isa animal or do we keep cat contradict animal?
        /// </summary>
        /// <param name="conceptCollection">concept collection</param>
        /// <returns>Trauma object about removed connections</returns>
        public Trauma PurifiyRange(IEnumerable<Concept> conceptCollection)
        {
            #warning Must be eventually optimized because it might be slow when the memory is big

            //loop:
            //  repair the concepts in collection
            //  get the optimized connection for which the amount of flat obstruction is the largest
            //  remove the optimized connection
            //  repeat process until the most objectable optimized connection has no obstruction

            Trauma trauma = new Trauma();

            List<Concept> obstruction;
            List<Concept> mostObstructableConnection;

            #region Finding obstructable connection in optimized connections
            do
            {
                repairer.RepairRange(conceptCollection);
                mostObstructableConnection = GetMostObstructableConnection(conceptCollection);
                if (mostObstructableConnection != null)
                {
                    connectionManager.UnPlug(mostObstructableConnection[0], mostObstructableConnection[1], mostObstructableConnection[2]);
                    obstruction = connectionManager.FindObstructionToPlug(mostObstructableConnection[0], mostObstructableConnection[1], mostObstructableConnection[2], false);
                    trauma.Add(mostObstructableConnection[0], mostObstructableConnection[1], mostObstructableConnection[2], obstruction[0], obstruction[1], obstruction[2]);
                }
            } while (mostObstructableConnection != null);
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
        /// <summary>
        /// Return the most obstructable connection for provided concept
        /// </summary>
        /// <param name="subject">provided concept</param>
        /// <returns>If most obstructable connection has obstruction, return obstructable connection, else: null</returns>
        private List<Concept> GetMostObstructableConnection(Concept subject)
        {
            List<Concept> mostObstructableConnection = null;
            int currentObstructionCount;
            int maxObstructionCount = 0;

            Concept verb;
            ConnectionBranch optimizedBranch;
            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in subject.OptimizedConnectionBranchList)
            {
                verb = verbAndBranch.Key;
                optimizedBranch = verbAndBranch.Value;
                foreach (Concept complement in optimizedBranch.ComplementConceptList)
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

        /// <summary>
        /// Return the most obstructable connection in concept collection
        /// </summary>
        /// <param name="conceptCollection">concept collection</param>
        /// <returns>If most obstructable connection has obstruction, return obstructable connection, else: null</returns>
        private List<Concept> GetMostObstructableConnection(IEnumerable<Concept> conceptCollection)
        {
            List<Concept> mostObstructableConnection = null;
            int currentObstructionCount;
            int maxObstructionCount = 0;

            Concept verb;
            ConnectionBranch optimizedBranch;
            foreach (Concept subject in conceptCollection)
            {
                foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in subject.OptimizedConnectionBranchList)
                {
                    verb = verbAndBranch.Key;
                    optimizedBranch = verbAndBranch.Value;
                    foreach (Concept complement in optimizedBranch.ComplementConceptList)
                    {
                        currentObstructionCount = connectionManager.CountObstructionToPlug(subject, verb, complement, false);
                        if (currentObstructionCount > maxObstructionCount)
                        {
                            maxObstructionCount = currentObstructionCount;
                            mostObstructableConnection = new List<Concept>() { subject, verb, complement };
                        }
                    }
                }
            }
            return mostObstructableConnection;
        }
        #endregion
    }
}
