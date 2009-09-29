using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents autocomplete working with binary search
    /// </summary>
    class AutoCompleteModelBinarySearch : AbstractAutoCompleteModel
    {
        #region Fields
        private SortedDictionary<string, string> tokenList = new SortedDictionary<string, string>();

        private DisambiguationNamer disambiguationNamer;

        private List<string> indexer = new List<string>();

        private RegexMatcher regexMatcher = new RegexMatcher();
        #endregion

        #region Constructor
        public AutoCompleteModelBinarySearch(DisambiguationNamer disambiguationNamer)
        {
            this.disambiguationNamer = disambiguationNamer;
        }
        #endregion

        #region Public methods
        public override void RefreshFrom(Memory currentMemory, NameMapper nameMapper)
        {
            tokenList = new SortedDictionary<string, string>();

            #region We get operators from LanguageDictionary
            //List<string> longMetaOperatorNameList;
            //Concept metaOperator;
            foreach (string metaOperatorName in LanguageDictionary.MetaOperatorList)
            {
                tokenList.Add(metaOperatorName + metaOperatorCode, metaOperatorName);
            }

            foreach (string operatorName in LanguageDictionary.NameMappingOperatorList)
                if (!tokenList.ContainsKey(operatorName + nameMappingOperatorCode))
                    tokenList.Add(operatorName + nameMappingOperatorCode, operatorName);

            foreach (string operatorName in LanguageDictionary.NullaryOperatorList)
                if (!tokenList.ContainsKey(operatorName + nullaryOrUnaryOrPrimitiveOperatorCode))
                    tokenList.Add(operatorName + nullaryOrUnaryOrPrimitiveOperatorCode, operatorName);

            foreach (string operatorName in LanguageDictionary.PrimitiveOperatorList)
                if (!tokenList.ContainsKey(operatorName + nullaryOrUnaryOrPrimitiveOperatorCode))
                    tokenList.Add(operatorName + nullaryOrUnaryOrPrimitiveOperatorCode, operatorName);

            foreach (string operatorName in LanguageDictionary.UnaryOperatorList)
                if (!tokenList.ContainsKey(operatorName + nullaryOrUnaryOrPrimitiveOperatorCode))
                    tokenList.Add(operatorName + nullaryOrUnaryOrPrimitiveOperatorCode, operatorName);
            #endregion

            #region We add operators
            int verbId;
            string verbName;
            List<string> longOperatorNameList;
            foreach (Concept verb in Memory.TotalVerbList)
            {
                verbId = currentMemory.GetIdFromConcept(verb);
                verbName = nameMapper.GetConceptNames(verbId)[0];

                longOperatorNameList = GetLongConceptName(verb, currentMemory, nameMapper);

                if (longOperatorNameList != null)
                {
                    foreach (string longOperatorName in longOperatorNameList)
                    {
                        if (!tokenList.ContainsKey(verbName + binaryOperatorCode))
                        {
                            tokenList.Add(longOperatorName + binaryOperatorCode, verbName);
                        }
                    }
                }
            }
            #endregion

            #region We add concepts
            List<string> shortConceptNameList;
            List<string> longConceptNameList;
            int conceptId;
            foreach (Concept concept in currentMemory)
            {
                try
                {
                    conceptId = currentMemory.GetIdFromConcept(concept);

                    shortConceptNameList = new List<string>(nameMapper.GetConceptNames(conceptId));

                    for (int i = 0; i < shortConceptNameList.Count; i++)
                        shortConceptNameList[i] = nameMapper.InvertYouAndMePov(shortConceptNameList[i]);

                    longConceptNameList = GetLongConceptName(concept, currentMemory, nameMapper);

                    if (longConceptNameList != null)
                    {
                        foreach (string longConceptName in longConceptNameList)
                        {
                            if (!tokenList.ContainsKey(longConceptName + conceptCode) && !tokenList.ContainsKey(longConceptName + binaryOperatorCode) && !tokenList.ContainsKey(longConceptName + metaOperatorCode))
                            {
                                tokenList.Add(longConceptName + conceptCode, shortConceptNameList[0]);
                            }
                        }
                    }
                }
                catch (MemoryException)
                {
                }
            }
            #endregion

            
            //Adding random values to make performance tests
            /*
            string randomName;
            for (int i = 0; i < 300000; i++)
            {
                randomName = Text.StringManipulation.GetRandomString();
                if (!tokenList.ContainsKey(randomName + conceptCode))
                    tokenList.Add(randomName + conceptCode, randomName);
            }
            */

            indexer = new List<string>(tokenList.Keys);
        }

        public override List<string> GetSelection(string startsWith)
        {
            List<string> selection = new List<string>();

            int startIndex = GetStartIndex(startsWith,indexer,0,indexer.Count);
            int endIndex = GetEndIndex(startsWith, indexer, startIndex, indexer.Count);

            if (startIndex + 1 == endIndex && !indexer[startIndex].StartsWith(startsWith))
                return selection;

            for (int i = startIndex; i < endIndex; i++)
                selection.Add(ParseSymbol(indexer[i]));         

            return selection;
        }

        public override string GetSelectionValue(string selectionKey)
        {
            return tokenList[selectionKey];
        }

        public override bool MatchesOneAndOnlyOneRegex(string stringToMatch)
        {
            return regexMatcher.MatchesOneAndOnlyOneRegex(stringToMatch);
        }
        #endregion

        #region Private methods
        private List<string> GetLongConceptName(Concept subject, Memory currentMemory, NameMapper nameMapper)
        {
            Concept bestParent = disambiguationNamer.GetContextConcept(subject);
            List<string> longConceptNameList = new List<string>();
            string bestParentName = null;

            if (bestParent != null)
            {
                int bestParentId = currentMemory.GetIdFromConcept(bestParent);
                bestParentName = nameMapper.InvertYouAndMePov(nameMapper.GetConceptNames(bestParentId)[0]);
            }

            int subjectId = currentMemory.GetIdFromConcept(subject);

            List<string> subjectNameList = nameMapper.GetConceptNames(subjectId);
            foreach (string currentSubjectName in subjectNameList)
            {
                string correctPersonCurrentSubjectName = nameMapper.InvertYouAndMePov(currentSubjectName);
                if (bestParentName == null || correctPersonCurrentSubjectName.Contains("("))
                    longConceptNameList.Add(correctPersonCurrentSubjectName);
                else
                    longConceptNameList.Add(correctPersonCurrentSubjectName + " (" + RemoveDisambiguation(bestParentName) + ")");
            }

            return longConceptNameList;
        }

        private string RemoveDisambiguation(string conceptName)
        {
            if (conceptName.Contains("_("))
            {
                return conceptName.Substring(0, conceptName.LastIndexOf("_"));
            }
            else
            {
                return conceptName;
            }
        }

        private int GetStartIndex(string startsWith, List<string> indexer, int searchZoneStart, int searchZoneEnd)
        {
            if (searchZoneStart == searchZoneEnd)
                return searchZoneStart;
            else if (searchZoneStart + 1 == searchZoneEnd)
            {
                if (indexer[searchZoneStart].StartsWith(startsWith))
                    return searchZoneStart;
                else
                    return searchZoneEnd;
            }

            int pivot = (searchZoneStart + searchZoneEnd) / 2;

            if (indexer[pivot].CompareTo(startsWith) > 0 || indexer[pivot].StartsWith(startsWith))
                return GetStartIndex(startsWith, indexer, searchZoneStart, pivot);
            else
                return GetStartIndex(startsWith, indexer, pivot, searchZoneEnd);
        }

        private int GetEndIndex(string startsWith, List<string> indexer, int searchZoneStart, int searchZoneEnd)
        {
            if (searchZoneStart == searchZoneEnd)
                return searchZoneStart;
            else if (searchZoneStart + 1 == searchZoneEnd)
                return searchZoneEnd;

            int pivot = (searchZoneStart + searchZoneEnd) / 2;


            if (indexer[pivot].CompareTo(startsWith) < 0 || indexer[pivot].StartsWith(startsWith))
                return GetEndIndex(startsWith, indexer, pivot, searchZoneEnd);
            else
                return GetEndIndex(startsWith, indexer, searchZoneStart, pivot);
        }
        #endregion
    }
}
