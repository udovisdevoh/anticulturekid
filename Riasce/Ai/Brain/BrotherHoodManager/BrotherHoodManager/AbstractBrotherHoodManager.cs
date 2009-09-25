using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public abstract class AbstractBrotherHoodManager
    {
        /// <summary>
        /// Returns a set of brotherhoods for subject concept
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>a set of brotherhoods for subject concept</returns>
        public abstract BrotherHoodSet GetFlatBrotherHoodSet(Concept subject);

        /// <summary>
        /// Returns a set of brotherhoods for subject concept
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>a set of brotherhoods for subject concept</returns>
        public abstract BrotherHoodSet GetOptimizedBrotherHoodSet(Concept subject);

        /// <summary>
        /// Returns all brothers of subject and their brotherhood strengths
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>all brothers of subject and their brotherhood strengths</returns>
        public abstract Dictionary<Concept, double> GetBrotherAndStrengthList(Concept subject);

        /// <summary>
        /// Get the fraternity strength
        /// </summary>
        /// <param name="subject">concept 1</param>
        /// <param name="brother">concept 2</param>
        /// <returns>fraternity strength</returns>
        public abstract double GetFraternityStrength(Concept subject, Concept brother);

        /// <summary>
        /// Get the list of concept plugged to subject
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>list of concept plugged to subject</returns>
        public abstract HashSet<Concept> GetOptimizedParentConceptList(Concept subject);
    }
}
