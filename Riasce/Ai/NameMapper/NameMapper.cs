using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Misc;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a concept name and id mapper. Maps concept names with id and vice-versa
    /// </summary>
    public class NameMapper : IEnumerable<string>
    {
        #region Fields
        /// <summary>
        /// Represents the concept id counter for unique concept id
        /// Similar to an SQL sequence
        /// Similar to AutoIncrement
        /// </summary>
        private int counter = 0;

        /// <summary>
        /// Key: concept name
        /// Value: concept id
        /// </summary>
        private Dictionary<string, int> idMap = new Dictionary<string, int>();

        /// <summary>
        /// Key: concept id
        /// Value: concept name list
        /// </summary>
        private Dictionary<int, List<string>> nameMap = new Dictionary<int, List<string>>();

        /// <summary>
        /// ai's name
        /// </summary>
        private Name aiName;

        /// <summary>
        /// human's name
        /// </summary>
        private Name humanName;

        /// <summary>
        /// Matches concept names between ()
        /// </summary>
        private static Regex regexParanthesedConceptName;
        #endregion

        #region Constructor
        static NameMapper()
        {
            regexParanthesedConceptName = new Regex(@"\A[a-z0-9_]+\([a-z0-9_]+\)\Z");
        }

        /// <summary>
        /// Create name mapper
        /// </summary>
        /// <param name="aiName">ai's name</param>
        /// <param name="humanName">human's name</param>
        public NameMapper(Name aiName, Name humanName)
        {
            this.aiName = aiName;

            this.humanName = humanName;

            this.counter = 0;
        }

        /// <summary>
        /// Create a name mapper
        /// </summary>
        /// <param name="aiName">ai's name</param>
        /// <param name="humanName">human's name</param>
        /// <param name="startCounterValue">start counter value (default: 0), it is similar to SQL sequence numbers</param>
        public NameMapper(Name aiName, Name humanName, int startCounterValue)
        {
            this.aiName = aiName;

            this.humanName = humanName;

            this.counter = startCounterValue;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a concept's unique id from provided name
        /// </summary>
        /// <param name="conceptName">Represents the concept's name</param>
        /// <returns>The concept's unique id</returns>
        public int GetOrCreateConceptId(string conceptName)
        {
            FunctionArgument.Ensure(conceptName, "concept name");
            EnsureWordConformParantheseRules(conceptName);
                
            #region We take care of YOU and ME special concept names
            if (conceptName == "me" || conceptName == "i")
                conceptName = humanName.Value;
            else if (conceptName == "you")
                conceptName = aiName.Value;
            #endregion

            int conceptId;
            if (idMap.TryGetValue(conceptName, out conceptId))
            {
                return conceptId;
            }
            else
            {
                return AddNewConceptGetId(conceptName);
            }
        }

        /// <summary>
        /// Returns the list of names a concept can have (WILL PROCESS YOU AND ME)
        /// </summary>
        /// <param name="conceptId">Concept's ID</param>
        /// <returns>List of possible names (for instance: circle, round)</returns>
        public List<string> GetConceptNames(int conceptId)
        {
            return GetConceptNames(conceptId, true);
        }

        /// <summary>
        /// Returns the list of names a concept can have
        /// </summary>
        /// <param name="conceptId">Concept's ID</param>
        /// <param name="processYouAndMe">whether you wish to process "you" and "me"</param>
        /// <returns>List of possible names (for instance: circle, round)</returns>
        public List<string> GetConceptNames(int conceptId, bool processYouAndMe)
        {
            FunctionArgument.Ensure(conceptId, "concept id");

            if (processYouAndMe)
            {
                #region When retrieving concepts linked to human name or ai name
                if (GetOrCreateConceptId(aiName.Value) == conceptId)
                    return new List<string>() { "me" };
                else if (GetOrCreateConceptId(humanName.Value) == conceptId)
                    return new List<string>() { "you" };
                #endregion
            }

            List<string> conceptNames;
            if (nameMap.TryGetValue(conceptId, out conceptNames))
            {
                return conceptNames;
            }
            else
            {
                throw new NameMappingException("Couldn't find concept id " + conceptId);
            }
        }

        /// <summary>
        /// Renames a concept
        /// </summary>
        /// <param name="oldConceptName">old name</param>
        /// <param name="newConceptName">new name</param>
        public void Rename(string oldConceptName, string newConceptName)
        {
            FunctionArgument.Ensure(oldConceptName, "old concept name");
            FunctionArgument.Ensure(newConceptName, "new concept name");
            EnsureWordConformParantheseRules(oldConceptName);
            EnsureWordConformParantheseRules(newConceptName);

            if (!idMap.ContainsKey(oldConceptName))
                throw new NameMappingException("Cannot rename non-existant concept " + oldConceptName);

            if (idMap.ContainsKey(newConceptName))
                throw new NameMappingException("Cannot rename concept to " + newConceptName + " because it already exists");

            int conceptId = GetOrCreateConceptId(oldConceptName);

            if (!nameMap[conceptId].Contains(oldConceptName))
                throw new NameMappingException("Cannot rename " + oldConceptName + " because it's not properly assigned (serious error)");

            nameMap[conceptId].Remove(oldConceptName);
            nameMap[conceptId].Add(newConceptName);
            idMap[newConceptName] = conceptId;
            idMap.Remove(oldConceptName);
        }

        /// <summary>
        /// Adds an alias to a concept
        /// </summary>
        /// <param name="newNameForConcept">new name for concept</param>
        /// <param name="existingNameForConcept">concept's existing name</param>
        public void AddAlias(string newNameForConcept, string existingNameForConcept)
        {
            if (newNameForConcept == "me" || newNameForConcept == "you" || existingNameForConcept == "me" || existingNameForConcept == "you" || newNameForConcept == "i" || existingNameForConcept == "i")
                throw new NameMappingException("No concept can be mapped to \"you\" and \"me\"");

            FunctionArgument.Ensure(newNameForConcept, "new name for concept");
            FunctionArgument.Ensure(existingNameForConcept, "existing name for concept");

            EnsureWordConformParantheseRules(newNameForConcept);
            EnsureWordConformParantheseRules(existingNameForConcept);

            int conceptId = GetOrCreateConceptId(existingNameForConcept);
            if (idMap.ContainsKey(newNameForConcept))
            {
                int existingIdNewNameForConcept = idMap[newNameForConcept];

                if (existingIdNewNameForConcept == conceptId)
                    throw new NameMappingException(newNameForConcept + " is already an alias to " + existingNameForConcept);

                nameMap[existingIdNewNameForConcept].Remove(newNameForConcept);

                idMap.Remove(newNameForConcept);

            }
            nameMap[conceptId].Add(newNameForConcept);
            idMap.Add(newNameForConcept, conceptId);
        }

        /// <summary>
        /// Remove aliasing between two concepts.
        /// Separates the concept into two distinct concepts
        /// </summary>
        /// <param name="conceptName1">concept name 1</param>
        /// <param name="conceptName2">concept name 2</param>
        /// <returns>2nd concept's unique id</returns>
        public int RemoveAliasAndGetSecondConceptId(string concept1Name, string concept2Name)
        {
            FunctionArgument.Ensure(concept1Name, "concept 1 name");
            FunctionArgument.Ensure(concept2Name, "concept 2 name");

            EnsureWordConformParantheseRules(concept1Name);
            EnsureWordConformParantheseRules(concept2Name);

            int concept1Id = GetOrCreateConceptId(concept1Name);
            int concept2Id = GetOrCreateConceptId(concept2Name);

            if (concept1Id != concept2Id)
                throw new NameMappingException("Cannot unalias " + concept2Name + " to " + concept1Name + " because they are not joined");

            List<string> concept1NameList = GetConceptNames(concept1Id);
            concept1NameList.Remove(concept2Name);
            idMap.Remove(concept2Name);

            return AddNewConceptGetId(concept2Name);
        }

        /// <summary>
        /// Returns proper name for concept from the Ai's point of view
        /// </summary>
        /// <param name="youOrMe">you or me</param>
        /// <returns>you or me from opposite pov</returns>
        public string InvertYouAndMePov(string youOrMe)
        {
            if (youOrMe == "me")
                return "you";
            else if (youOrMe == "you")
                return "me";
            else
                return youOrMe;
            //return GetConceptNames(GetOrCreateConceptId(youOrMe))[0];
        }

        /// <summary>
        /// Whether the name mapper contains concept name
        /// </summary>
        /// <param name="conceptName">concept name</param>
        /// <returns>whether the name mapper contains concept name</returns>
        public bool Contains(string conceptName)
        {
            return idMap.ContainsKey(conceptName);
        }

        /// <summary>
        /// Used only when loading data from file, add new info in name mapper from serialized structure
        /// </summary>
        /// <param name="conceptInfo">concept info</param>
        public void AssimilateConceptInfo(ConceptInfo conceptInfo)
        {
            foreach (string name in conceptInfo.NameList)
                idMap[name] = conceptInfo.Id;
            
            List<string> internalNameList;

            if (!nameMap.TryGetValue(conceptInfo.Id, out internalNameList))
            {
                internalNameList = new List<string>();
                nameMap.Add(conceptInfo.Id, internalNameList);
            }

            foreach (string name in conceptInfo.NameList)
                internalNameList.Add(name);
        }

        /// <summary>
        /// Returns the list of all verb names
        /// </summary>
        /// <param name="currentMemory">memory to look into</param>
        /// <returns>the list of all verb names</returns>
        public List<string> GetTotalVerbNameList(Memory currentMemory)
        {
            List<string> totalVerbNameList = new List<string>();
            int verbId;
            foreach (Concept verb in Memory.TotalVerbList)
            {
                verbId = currentMemory.GetIdFromConcept(verb);
                totalVerbNameList.Add(GetConceptNames(verbId)[0]);
            }
            return totalVerbNameList;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return idMap.Keys.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get next concept id
        /// </summary>
        /// <returns>next concept id</returns>
        private int GetNextId()
        {
            return counter++;
        }

        /// <summary>
        /// Add a new concept and get its id
        /// </summary>
        /// <param name="conceptName">new concept's name</param>
        /// <returns>new concept's id</returns>
        private int AddNewConceptGetId(string conceptName)
        {
            if (conceptName == "me" || conceptName == "you" || conceptName == "i")
                throw new NameMappingException("No concept can be mapped to \"you\" and \"me\"");

            int conceptId = GetNextId();
            FunctionArgument.Ensure(conceptName, "concept name");

            if (idMap.ContainsKey(conceptName))
                throw new NameMappingException("Concept name already assigned to a concept id");

            idMap.Add(conceptName, conceptId);
            nameMap.Add(conceptId, new List<string> { conceptName });

            return conceptId;
        }

        /// <summary>
        /// Throw exception when concept contains badly formated () chars
        /// </summary>
        /// <param name="conceptName">concept name</param>
        private void EnsureWordConformParantheseRules(string conceptName)
        {
            if ((conceptName.Contains('(') || conceptName.Contains(')')) && !regexParanthesedConceptName.IsMatch(conceptName))
                throw new NameMappingException("Invalid concept name. Names with parantheses must be formated as *(*) where * is a set of letters, numbers or underscores");
        }
        #endregion

        #region Properties
        /// <summary>
        /// ai's name
        /// </summary>
        public Name AiName
        {
            get { return aiName; }
            set { aiName = value; }
        }

        /// <summary>
        /// human's name
        /// </summary>
        public Name HumanName
        {
            get { return humanName; }
            set { humanName = value; }
        }

        /// <summary>
        /// Represents the concept id counter for unique concept id
        /// Similar to an SQL sequence
        /// Similar to AutoIncrement
        /// </summary>
        public int Counter
        {
            get { return counter; }
        }

        /// <summary>
        /// Count how many concept names exists in name mapper
        /// </summary>
        public int Count
        {
            get { return idMap.Keys.Count; }
        }
        #endregion
    }
}
