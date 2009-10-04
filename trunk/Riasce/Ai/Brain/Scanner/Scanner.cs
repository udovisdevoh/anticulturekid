using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    static class Scanner
    {
        #region Public Methods
        /// <summary>
        /// Scan memory and return output
        /// </summary>
        /// <param name="memory">memory</param>
        /// <param name="nameMapper">name mapper</param>
        /// <param name="isFlatMode">whether we scan flat memory</param>
        /// <returns>memory scanning output</returns>
        public static string ScanMemoryGetOutput(Memory memory, NameMapper nameMapper, bool isFlatMode)
        {
            string output = "\n";
            int count = 0;
            foreach (Concept subject in memory)
            {
                foreach (Concept verb in Memory.TotalVerbList)
                {
                    HashSet<Concept> inverseOfVerbList = GetInverseOrPermutableVerbList(verb);

                    HashSet<Concept> subjectComplementList;
                    if (isFlatMode)
                        subjectComplementList = subject.GetFlatConnectionBranch(verb).ComplementConceptList;
                    else
                        subjectComplementList = subject.GetOptimizedConnectionBranch(verb).ComplementConceptList;

                    foreach (Concept complement in subjectComplementList)
                    {
                        foreach (Concept currentInverseVerb in inverseOfVerbList)
                        {

                            HashSet<Concept> complementSubjectList;
                            if (isFlatMode)
                                complementSubjectList = complement.GetFlatConnectionBranch(currentInverseVerb).ComplementConceptList;
                            else
                                complementSubjectList = complement.GetOptimizedConnectionBranch(currentInverseVerb).ComplementConceptList;

                            if (!complementSubjectList.Contains(subject))
                            {
                                output += GetFormatedInconsistency(subject, verb, complement, currentInverseVerb, nameMapper, memory);
                                count++;
                            }
                        }
                    }
                }
            }

            output += "\nMemory scanning finished. Total inconsistencies found: " + count;

            return output;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get list of inverse or permutable side verbs
        /// </summary>
        /// <param name="verb">source verb</param>
        /// <returns>list of inverse or permutable side verbs</returns>
        private static HashSet<Concept> GetInverseOrPermutableVerbList(Concept verb)
        {
            HashSet<Concept> inverseOrPermutableVerbList = new HashSet<Concept>();

            inverseOrPermutableVerbList.UnionWith(verb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("inverse_of"));
            inverseOrPermutableVerbList.UnionWith(verb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("permutable_side"));

            return inverseOrPermutableVerbList;
        }

        /// <summary>
        /// Get string formated inconsistency info
        /// </summary>
        /// <param name="subject">subject</param>
        /// <param name="verb">verb</param>
        /// <param name="complement">complement</param>
        /// <param name="inverseVerb">inverse verb</param>
        /// <param name="nameMapper">name mapper</param>
        /// <param name="memory">memory</param>
        /// <returns>string formated inconsistency info</returns>
        private static string GetFormatedInconsistency(Concept subject, Concept verb, Concept complement, Concept inverseVerb, NameMapper nameMapper, Memory memory)
        {
            string formatedInconsistency = "\n";

            formatedInconsistency += GetConceptName(subject, nameMapper, memory);
            formatedInconsistency += " ";
            formatedInconsistency += GetConceptName(verb, nameMapper, memory);
            formatedInconsistency += " ";
            formatedInconsistency += GetConceptName(complement, nameMapper, memory);
            formatedInconsistency += " but ";
            formatedInconsistency += GetConceptName(complement, nameMapper, memory);
            formatedInconsistency += " not ";
            formatedInconsistency += GetConceptName(inverseVerb, nameMapper, memory);
            formatedInconsistency += " ";
            formatedInconsistency += GetConceptName(subject, nameMapper, memory);

            return formatedInconsistency;
        }

        /// <summary>
        /// Get concept name
        /// </summary>
        /// <param name="concept">concept</param>
        /// <param name="nameMapper">name mapper</param>
        /// <param name="memory">memory</param>
        /// <returns>concept name</returns>
        private static string GetConceptName(Concept concept, NameMapper nameMapper, Memory memory)
        {
            int conceptId = memory.GetIdFromConcept(concept);
            return nameMapper.GetConceptNames(conceptId)[0];
        }
        #endregion
    }
}
