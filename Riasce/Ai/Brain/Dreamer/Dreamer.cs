using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used to create dreams
    /// </summary>
    static class Dreamer
    {
        #region Constants
        /// <summary>
        /// Maximum number of dream element
        /// </summary>
        private static readonly int maxDreamElement = 4;

        /// <summary>
        /// Maximum number of randomly choosen complement concepts
        /// </summary>
        private static readonly int samplingSize = 100;
        #endregion

        #region Public Methods
        /// <summary>
        /// Get dream
        /// </summary>
        /// <param name="memory">memory to look into</param>
        /// <param name="nameMapper">name mapper</param>
        /// <returns>dream</returns>
        public static List<string> GetDream(Memory memory, NameMapper nameMapper)
        {
            List<string> dream = new List<string>();

            Concept sourceConcept = memory.GetRandomItem();
            Proposition dreamElement;

            while (dream.Count < maxDreamElement)
            {
                dreamElement = GetNextElement(sourceConcept, memory);

                if (dreamElement == null)
                    throw new DreamException("I can't remember any dream right now");

                if (dreamElement != null)
                {
                    sourceConcept = dreamElement.Complement;
                    dream.Add(ArgumentToString(dreamElement, memory, nameMapper));
                }
            }
            if (dream.Count == 0 || dream == null)
                throw new DreamException("I can't remember any dream right now");

            return dream;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Get next dream element
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="memory"></param>
        /// <returns></returns>
        private static Proposition GetNextElement(Concept subject, Memory memory)
        {

            HashSet<Concept> complementList = memory.GetRandomSample(samplingSize);
            HashSet<Concept> verbList = Memory.TotalVerbList.GetRandomSample(Memory.TotalVerbList.Count);

            foreach (Concept verb in verbList)
                foreach (Concept complement in complementList)
                    if (ConnectionManager.FindObstructionToPlug(subject, verb, complement, true) == null)
                        return new Proposition(subject, verb, complement);

            return null;
        }

        /// <summary>
        /// Convert argument to string 
        /// </summary>
        /// <param name="dreamElement">argument</param>
        /// <param name="memory">memory to look into</param>
        /// <param name="nameMapper">name mapper to look into</param>
        /// <returns>string connection</returns>
        private static string ArgumentToString(Proposition dreamElement, Memory memory, NameMapper nameMapper)
        {
            return GetConceptName(dreamElement.Subject, memory, nameMapper) + " " + GetConceptName(dreamElement.Verb, memory, nameMapper) + " " + GetConceptName(dreamElement.Complement, memory, nameMapper);
        }

        /// <summary>
        /// Get concept name
        /// </summary>
        /// <param name="concept">concept name</param>
        /// <param name="memory">memory</param>
        /// <param name="nameMapper">name mapper</param>
        /// <returns>concept name</returns>
        private static string GetConceptName(Concept concept, Memory memory, NameMapper nameMapper)
        {
            return nameMapper.GetConceptNames(memory.GetIdFromConcept(concept))[0];
        }
        #endregion
    }
}
