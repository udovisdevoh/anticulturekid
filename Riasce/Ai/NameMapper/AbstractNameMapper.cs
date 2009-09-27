using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public abstract class AbstractNameMapper
    {

        public abstract List<string> GetConceptNames(int conceptId);


        public abstract List<string> GetConceptNames(int conceptId, bool processYouAndMe);


        public abstract void Rename(string oldConceptName, string newConceptName);


        public abstract void AddAlias(string newNameForConcept, string existingNameForConcept);


        public abstract int RemoveAliasAndGetSecondConceptId(string conceptName1, string conceptName2);


        public abstract string InvertYouAndMePov(string youOrMe);

        /// <summary>
        /// Used only when loading data from file, add new info in name mapper from serialized structure
        /// </summary>
        /// <param name="conceptInfo">concept info</param>
        public abstract void AssimilateConceptInfo(ConceptInfo conceptInfo);

        /// <summary>
        /// Returns the list of all verb names
        /// </summary>
        /// <param name="currentMemory">memory to look into</param>
        /// <returns>the list of all verb names</returns>
        public abstract List<string> GetTotalVerbNameList(Memory currentMemory);

        public abstract bool Contains(string conceptName);

        public abstract IEnumerator<string> GetEnumerator();

        public abstract int Count
        {
            get;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
