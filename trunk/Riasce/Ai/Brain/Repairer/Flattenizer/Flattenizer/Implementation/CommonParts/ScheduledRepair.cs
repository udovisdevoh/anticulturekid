using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a scheduled repair of flat branch
    /// </summary>
    class ScheduledRepair
    {
        #region Private Fields
        /// <summary>
        /// Flat branch
        /// </summary>
        private ConnectionBranch flatBranch;
        
        /// <summary>
        /// Optimized branch
        /// </summary>
        private ConnectionBranch optimizedBranch;

        /// <summary>
        /// Subject concept
        /// </summary>
        private Concept subject;

        /// <summary>
        /// Verb concept
        /// </summary>
        private Concept verb;
        #endregion

        /// <summary>
        /// Create scheduled repair
        /// </summary>
        /// <param name="flatBranch">flat branch</param>
        /// <param name="optimizedBranch">optimized branch</param>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        public ScheduledRepair(ConnectionBranch flatBranch, ConnectionBranch optimizedBranch, Concept subject, Concept verb)
        {
            this.flatBranch = flatBranch;
            this.optimizedBranch = optimizedBranch;
            this.subject = subject;
            this.verb = verb;
        }

        #region Properties
        /// <summary>
        /// Flat Branch
        /// </summary>
        public ConnectionBranch FlatBranch
        {
            get { return flatBranch; }
        }

        /// <summary>
        /// Optimized Branch
        /// </summary>
        public ConnectionBranch OptimizedBranch
        {
            get { return optimizedBranch; }
        }

        /// <summary>
        /// Subject concept
        /// </summary>
        public Concept Subject
        {
            get { return subject; }
        }

        /// <summary>
        /// Verb concept
        /// </summary>
        public Concept Verb
        {
            get { return verb; }
        }
        #endregion
    }
}
