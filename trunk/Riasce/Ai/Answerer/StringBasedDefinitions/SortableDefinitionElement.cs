using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Used by string based definition sorter to compare branches
    /// </summary>
    class SortableDefinitionElement : IComparable
    {
        #region Fields
        private string verbName;

        private List<string> complementNameList;

        private int sortingMode;

        private int shortestProofLength = 0;
        #endregion

        #region Constructor
        public SortableDefinitionElement(string verb, List<string> complementList, int sortingMode)
        {
            this.verbName = verb;
            this.complementNameList = complementList;
            this.sortingMode = sortingMode;
        }
        #endregion

        #region Public Methods
        public int CompareTo(object obj)
        {
            SortableDefinitionElement otherElement = (SortableDefinitionElement)(obj);

            if (sortingMode == StringBasedDefinitionSorter.SORT_BY_LOOSE_COMPLEMENT_CARDINALITY)
            {
                if (this.Count != otherElement.Count)
                {
                    return this.Count - otherElement.Count;
                }
                else
                {
                    int verb1Key = StringBasedDefinitionSorter.customOrder.Count;
                    int verb2Key = StringBasedDefinitionSorter.customOrder.Count;

                    if (StringBasedDefinitionSorter.customOrder.TryGetValue(this.verbName, out verb1Key)) { }
                    if (StringBasedDefinitionSorter.customOrder.TryGetValue(otherElement.verbName, out verb2Key)) { }

                    if (verb1Key == StringBasedDefinitionSorter.customOrder.Count && verb2Key == StringBasedDefinitionSorter.customOrder.Count)
                    {
                        return this.verbName.CompareTo(otherElement.verbName);
                    }
                    else
                    {
                        return verb1Key - verb2Key;
                    }
                }
            }
            else if (sortingMode == StringBasedDefinitionSorter.SORT_BY_VERB_NAME)
            {
                if (this.verbName != otherElement.verbName)
                {
                    return this.verbName.CompareTo(otherElement.verbName);
                }
                else
                {
                    return this.Count - otherElement.Count;
                }
            }
            else if (sortingMode == StringBasedDefinitionSorter.SORT_BY_CUSTOM_ORDER)
            {
                int verb1Key = StringBasedDefinitionSorter.customOrder.Count;
                int verb2Key = StringBasedDefinitionSorter.customOrder.Count;

                if (StringBasedDefinitionSorter.customOrder.TryGetValue(this.verbName, out verb1Key)) { }
                if (StringBasedDefinitionSorter.customOrder.TryGetValue(otherElement.verbName, out verb2Key)) { }

                if (verb1Key == StringBasedDefinitionSorter.customOrder.Count && verb2Key == StringBasedDefinitionSorter.customOrder.Count)
                {
                    if (this.Count != otherElement.Count)
                    {
                        return this.Count - otherElement.Count;
                    }
                    else
                    {
                        return this.verbName.CompareTo(otherElement.verbName);
                    }
                }
                else
                {
                    return verb1Key - verb2Key;
                }
            }
            else if (sortingMode == StringBasedDefinitionSorter.SORT_BY_PROOF_LENGTH)
            {
                int proof1Length, proof2Length;

                proof1Length = this.shortestProofLength;
                proof2Length = otherElement.shortestProofLength;

                if (proof1Length == proof2Length)
                {
                    int verb1Key = StringBasedDefinitionSorter.customOrder.Count;
                    int verb2Key = StringBasedDefinitionSorter.customOrder.Count;
                    if (StringBasedDefinitionSorter.customOrder.TryGetValue(this.verbName, out verb1Key)) { }
                    if (StringBasedDefinitionSorter.customOrder.TryGetValue(otherElement.verbName, out verb2Key)) { }

                    if (verb1Key == StringBasedDefinitionSorter.customOrder.Count && verb2Key == StringBasedDefinitionSorter.customOrder.Count)
                    {
                        if (this.Count != otherElement.Count)
                        {
                            return this.Count - otherElement.Count;
                        }
                        else
                        {
                            return this.verbName.CompareTo(otherElement.verbName);
                        }
                    }
                    else
                    {
                        return verb1Key - verb2Key;
                    }
                }
                else
                {
                    return proof1Length - proof2Length;
                }
            }
            else
            {
                throw new SortingException("Invalid sorting mode");
            }
        }

        public void SortByProofLength(Concept subject, NameMapper nameMapper, Memory memory)
        {
            if (verbName.ToUpper() == verbName)//Ignore metaConnectionBranches
                return;

            if (subject.IsFlatDirty)
                throw new SortingException("Repair concept first");

            int verbId, complementId;
            Concept verb, complement;

            List<SortableComplementInfo> sortableComplementList = new List<SortableComplementInfo>();

            foreach (string complementName in complementNameList)
            {
                verbId = nameMapper.GetOrCreateConceptId(verbName);
                verb = memory.GetOrCreateConcept(verbId);

                complementId = nameMapper.GetOrCreateConceptId(nameMapper.InvertYouAndMePov(complementName));
                complement = memory.GetOrCreateConcept(complementId);

                sortableComplementList.Add(new SortableComplementInfo(complementName, subject, verb, complement));
            }

            sortableComplementList.Sort();

            shortestProofLength = sortableComplementList[0].ProofLength;

            complementNameList = new List<string>();
            foreach (SortableComplementInfo sortableComplementInfo in sortableComplementList)
                complementNameList.Add(sortableComplementInfo.Name);
        }
        #endregion

        #region Properties
        public int Count
        {
            get { return complementNameList.Count; }
        }

        public string Verb
        {
            get { return verbName; }
        }

        public List<string> ComplementList
        {
            get { return complementNameList; }
        }
        #endregion
    }
}
