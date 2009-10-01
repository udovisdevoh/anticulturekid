using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is an analogy factory
    /// </summary>
    class Analogizer
    {
        #region Fields
        /// <summary>
        /// Maximum number of randomly choosen concepts when trying to find the best
        /// analogy among them
        /// </summary>
        private static readonly int samplingSize = 20;
        #endregion

        #region Public Methods
        /// <summary>
        /// Get an analogy to provided expression
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <returns>Analogy</returns>
        public Analogy GetAnalogyOnSubjectVerbComplement(Concept subject, Concept verb, Concept complement)
        {
            if (subject.IsFlatDirty || subject.IsOptimizedDirty)
                throw new AnalogyException("Repair concept first");

            if (complement.IsFlatDirty || complement.IsOptimizedDirty)
                throw new AnalogyException("Repair concept first");

            VerbMetaConnectionCache.Clear();

            List<Analogy> analogyList = GetAnalogyList(subject,verb,complement);

            Analogy bestAnalogy = null;
            foreach (Analogy currentAnalogy in analogyList)
            {
                if (bestAnalogy == null || (currentAnalogy != null && currentAnalogy.Relevance > bestAnalogy.Relevance))
                    bestAnalogy = currentAnalogy;
            }

            return bestAnalogy;
        }

        /// <summary>
        /// Get an analogy to provided expression stub
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <returns>Analogy</returns>
        public Analogy GetAnalogyOnSubjectVerb(Concept subject, Concept verb)
        {
            if (subject.IsFlatDirty || subject.IsOptimizedDirty)
                throw new AnalogyException("Repair concept first");

            VerbMetaConnectionCache.Clear();

            List<Analogy> analogyList = new List<Analogy>();

            HashSet<Concept> complementList = subject.GetFlatConnectionBranch(verb).ComplementConceptList;

            foreach (Concept complement in complementList)
                analogyList.Add(GetAnalogyOnSubjectVerbComplement(subject, verb, complement));

            Analogy bestAnalogy = null;
            foreach (Analogy currentAnalogy in analogyList)
            {
                if (bestAnalogy == null || (currentAnalogy != null && currentAnalogy.Relevance > bestAnalogy.Relevance))
                    bestAnalogy = currentAnalogy;
            }

            return bestAnalogy;
        }

        /// <summary>
        /// Get an analogy to subject
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>Analogy</returns>
        public Analogy GetAnalogyOnSubject(Concept subject)
        {
            if (subject.IsFlatDirty || subject.IsOptimizedDirty)
                throw new AnalogyException("Repair concept first");

            VerbMetaConnectionCache.Clear();

            List<Analogy> analogyList = new List<Analogy>();

            foreach (Concept verb in Memory.TotalVerbList)
                analogyList.Add(GetAnalogyOnSubjectVerb(subject,verb));

            Analogy bestAnalogy = null;
            foreach (Analogy currentAnalogy in analogyList)
            {
                if (bestAnalogy == null || (currentAnalogy != null && currentAnalogy.Relevance > bestAnalogy.Relevance))
                    bestAnalogy = currentAnalogy;
            }

            return bestAnalogy;
        }

        /// <summary>
        /// Get a random analogy from random concept in concept collection
        /// </summary>
        /// <param name="conceptCollection">concept collection</param>
        /// <returns>Analogy</returns>
        public Analogy GetBestRandomAnalogy(IEnumerable<Concept> conceptCollection)
        {
            VerbMetaConnectionCache.Clear();

            HashSet<Concept> conceptSample = conceptCollection.GetRandomSample(samplingSize);

            List<Analogy> analogyList = new List<Analogy>();
            foreach (Concept currentConcept in conceptSample)
                analogyList.Add(GetAnalogyOnSubject(currentConcept));

            Analogy bestAnalogy = null;
            foreach (Analogy currentAnalogy in analogyList)
            {
                if (bestAnalogy == null || (currentAnalogy != null && currentAnalogy.Relevance > bestAnalogy.Relevance))
                    bestAnalogy = currentAnalogy;
            }

            return bestAnalogy;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Get a list of possible analogy made on subject, verb and complement
        /// </summary>
        /// <param name="subject">subject</param>
        /// <param name="verb">verb</param>
        /// <param name="complement">complement</param>
        /// <returns>list of possible analogy made on subject, verb and complement</returns>
        private List<Analogy> GetAnalogyList(Concept subject, Concept verb, Concept complement)
        {
            List<Analogy> analogyList = new List<Analogy>();

            Dictionary<Concept, double> subjectBrotherAndStrenghtList = BrotherHoodManager.GetBrotherAndStrengthList(subject);
            Dictionary<Concept, double> complementBrotherAndStrenghtList = BrotherHoodManager.GetBrotherAndStrengthList(complement);

            Concept subjectBrother;
            double subjectBrotherStrength;
            Concept complementBrother;
            double complementBrotherStrength;
            foreach (KeyValuePair<Concept, double> subjectBrotherAndStrength in subjectBrotherAndStrenghtList)
            {
                subjectBrother = subjectBrotherAndStrength.Key;
                subjectBrotherStrength = subjectBrotherAndStrength.Value;

                foreach (KeyValuePair<Concept, double> complementBrotherAndStrength in complementBrotherAndStrenghtList)
                {
                    complementBrother = complementBrotherAndStrength.Key;
                    complementBrotherStrength = complementBrotherAndStrength.Value;

                    if (subjectBrother.GetFlatConnectionBranch(verb).ComplementConceptList.Contains(complementBrother))
                        analogyList.Add(new Analogy(subjectBrotherStrength * complementBrotherStrength, subjectBrother, verb, complementBrother, subject,verb,complement));
                }
            }

            return analogyList;
        }
        #endregion
    }
}
