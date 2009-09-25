using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a concept name and id mapper. Maps concept names with id and vice-versa
    /// </summary>
    public abstract class AbstractNameMapper : IEnumerable<string>
    {
        /// <summary>
        /// Returns a concept's unique id from provided name
        /// </summary>
        /// <param name="conceptName">Represents the concept's name</param>
        /// <returns>The concept's unique id</returns>
        public abstract int GetOrCreateConceptId(string conceptName);

        /// <summary>
        /// Returns the list of names a concept can have (WILL PROCESS YOU AND ME)
        /// </summary>
        /// <param name="conceptId">Concept's ID</param>
        /// <returns>List of possible names (for instance: circle, round)</returns>
        public abstract List<string> GetConceptNames(int conceptId);

        /// <summary>
        /// Returns the list of names a concept can have
        /// </summary>
        /// <param name="conceptId">Concept's ID</param>
        /// <param name="processYouAndMe">whether you wish to process "you" and "me"</param>
        /// <returns>List of possible names (for instance: circle, round)</returns>
        public abstract List<string> GetConceptNames(int conceptId, bool processYouAndMe);

        /// <summary>
        /// Renames a concept
        /// </summary>
        /// <param name="oldConceptName">old name</param>
        /// <param name="newConceptName">new name</param>
        public abstract void Rename(string oldConceptName, string newConceptName);

        /// <summary>
        /// Adds an alias to a concept
        /// </summary>
        /// <param name="newNameForConcept">new name for concept</param>
        /// <param name="existingNameForConcept">concept's existing name</param>
        public abstract void AddAlias(string newNameForConcept, string existingNameForConcept);

        /// <summary>
        /// Remove aliasing between two concepts.
        /// Separates the concept into two distinct concepts
        /// </summary>
        /// <param name="conceptName1">concept name 1</param>
        /// <param name="conceptName2">concept name 2</param>
        /// <returns>2nd concept's unique id</returns>
        public abstract int RemoveAliasAndGetSecondConceptId(string conceptName1, string conceptName2);

        /// <summary>
        /// Returns proper name for concept from the Ai's point of view
        /// </summary>
        /// <param name="youOrMe">you or me</param>
        /// <returns>you or me from opposite pov</returns>
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
