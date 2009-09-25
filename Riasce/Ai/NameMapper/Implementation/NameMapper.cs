using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Misc;

namespace AntiCulture.Kid
{
    public class NameMapper : AbstractNameMapper
    {
        #region Fields
        /// <summary>
        /// Represents the concept id counter for unique concept id
        /// Similar to an SQL sequence
        /// Similar to AutoIncrement
        /// </summary>
        private int counter = 0;

        private Dictionary<string, int> idMap = new Dictionary<string, int>();

        private Dictionary<int, List<string>> nameMap = new Dictionary<int, List<string>>();

        private Name aiName;

        private Name humanName;

        private static Regex regexRaranthesedConceptName;
        #endregion

        #region Constructor
        static NameMapper()
        {
            regexRaranthesedConceptName = new Regex(@"\A[a-z0-9_]+\([a-z0-9_]+\)\Z");
        }

        public NameMapper(Name aiName, Name humanName)
        {
            this.aiName = aiName;

            this.humanName = humanName;

            this.counter = 0;
        }

        public NameMapper(Name aiName, Name humanName, int startCounterValue)
        {
            this.aiName = aiName;

            this.humanName = humanName;

            this.counter = startCounterValue;
        }
        #endregion

        #region Public Methods
        public override int GetOrCreateConceptId(string conceptName)
        {
            Argument.Ensure(conceptName, "concept name");
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

        public override List<string> GetConceptNames(int conceptId)
        {
            return GetConceptNames(conceptId, true);
        }

        public override List<string> GetConceptNames(int conceptId, bool processYouAndMe)
        {
            Argument.Ensure(conceptId, "concept id");

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

        public override void Rename(string oldConceptName, string newConceptName)
        {
            Argument.Ensure(oldConceptName, "old concept name");
            Argument.Ensure(newConceptName, "new concept name");
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

        public override void AddAlias(string newNameForConcept, string existingNameForConcept)
        {
            if (newNameForConcept == "me" || newNameForConcept == "you" || existingNameForConcept == "me" || existingNameForConcept == "you" || newNameForConcept == "i" || existingNameForConcept == "i")
                throw new NameMappingException("No concept can be mapped to \"you\" and \"me\"");

            Argument.Ensure(newNameForConcept, "new name for concept");
            Argument.Ensure(existingNameForConcept, "existing name for concept");

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

        public override int RemoveAliasAndGetSecondConceptId(string concept1Name, string concept2Name)
        {
            Argument.Ensure(concept1Name, "concept 1 name");
            Argument.Ensure(concept2Name, "concept 2 name");

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

        public override string InvertYouAndMePov(string youOrMe)
        {
            if (youOrMe == "me")
                return "you";
            else if (youOrMe == "you")
                return "me";
            else
                return youOrMe;
            //return GetConceptNames(GetOrCreateConceptId(youOrMe))[0];
        }

        public override bool Contains(string conceptName)
        {
            return idMap.ContainsKey(conceptName);
        }

        public override void AssimilateConceptInfo(ConceptInfo conceptInfo)
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

        public override List<string> GetTotalVerbNameList(Memory currentMemory)
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

        public override IEnumerator<string> GetEnumerator()
        {
            return idMap.Keys.GetEnumerator();
        }
        #endregion

        #region Private Methods
        private int GetNextId()
        {
            return counter++;
        }

        private int AddNewConceptGetId(string conceptName)
        {
            if (conceptName == "me" || conceptName == "you" || conceptName == "i")
                throw new NameMappingException("No concept can be mapped to \"you\" and \"me\"");

            int conceptId = GetNextId();
            Argument.Ensure(conceptName, "concept name");

            if (idMap.ContainsKey(conceptName))
                throw new NameMappingException("Concept name already assigned to a concept id");

            idMap.Add(conceptName, conceptId);
            nameMap.Add(conceptId, new List<string> { conceptName });

            return conceptId;
        }

        private void EnsureWordConformParantheseRules(string conceptName)
        {
            if ((conceptName.Contains('(') || conceptName.Contains(')')) && !regexRaranthesedConceptName.IsMatch(conceptName))
                throw new NameMappingException("Invalid concept name. Names with parantheses must be formated as *(*) where * is a set of letters, numbers or underscores");
        }
        #endregion

        #region Properties
        public Name AiName
        {
            get { return aiName; }
            set { aiName = value; }
        }

        public Name HumanName
        {
            get { return humanName; }
            set { humanName = value; }
        }

        public int Counter
        {
            get { return counter; }
        }

        public override int Count
        {
            get { return idMap.Keys.Count; }
        }
        #endregion
    }
}
