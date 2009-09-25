﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class DefinitionSorter : AbstractDefinitionSorter
    {
        #region Static
        public static readonly int SORT_BY_VERB_NAME = 1;

        public static readonly int SORT_BY_LOOSE_COMPLEMENT_CARDINALITY = 2;

        public static readonly int SORT_BY_CUSTOM_ORDER = 3;

        public static readonly int SORT_BY_PROOF_LENGTH = 4;

        public static readonly Dictionary<string, int> customOrder;
        #endregion

        #region Constructor
        static DefinitionSorter()
        {
            List<string> sortedOperatorList = new List<string>();
            sortedOperatorList.Add("isa");
            sortedOperatorList.Add("from");
            sortedOperatorList.Add("madeof");
            sortedOperatorList.Add("make");
            sortedOperatorList.Add("madeby");
            sortedOperatorList.Add("without");
            sortedOperatorList.Add("own");
            sortedOperatorList.Add("ownedby");
            sortedOperatorList.Add("contradict");
            sortedOperatorList.Add("antagonize");
            sortedOperatorList.Add("was");
            sortedOperatorList.Add("originof");
            sortedOperatorList.Add("synergize");
            sortedOperatorList.Add("allow");
            sortedOperatorList.Add("need");
            sortedOperatorList.Add("oppress");
            sortedOperatorList.Add("oppressedby");
            sortedOperatorList.Add("become");
            sortedOperatorList.Add("largerthan");
            sortedOperatorList.Add("smallerthan");
            sortedOperatorList.Add("partof");
            sortedOperatorList.Add("someare");
            sortedOperatorList.Add("notpartof");

            customOrder = new Dictionary<string, int>();
            int key = 0;
            foreach (string operatorName in sortedOperatorList)
            {
                customOrder.Add(operatorName, key);
                key++;
            }
        }
        #endregion

        #region Public methods
        public override Dictionary<string, List<string>> SortByLooseCardinality(Dictionary<string, List<string>> definition)
        {
            List<SortableDefinitionElement> sortableElementList = GetSortableElementList(definition, SORT_BY_LOOSE_COMPLEMENT_CARDINALITY);

            sortableElementList.Sort();

            return GetDefinition(sortableElementList);
        }

        public override Dictionary<string, List<string>> SortByVerbName(Dictionary<string, List<string>> definition)
        {
            List<SortableDefinitionElement> sortableElementList = GetSortableElementList(definition, SORT_BY_VERB_NAME);

            sortableElementList.Sort();

            return GetDefinition(sortableElementList);
        }

        public override Dictionary<string, List<string>> SortByCustomOrder(Dictionary<string, List<string>> definition)
        {
            List<SortableDefinitionElement> sortableElementList = GetSortableElementList(definition, SORT_BY_CUSTOM_ORDER);

            sortableElementList.Sort();

            return GetDefinition(sortableElementList);
        }

        public override Dictionary<string, List<string>> SortComplementsByProofLength(Dictionary<string, List<string>> definition, Concept subject, NameMapper nameMapper, Memory memory)
        {
            List<SortableDefinitionElement> sortableElementList = GetSortableElementList(definition, SORT_BY_PROOF_LENGTH);

            foreach (SortableDefinitionElement sortableDefinitionElement in sortableElementList)
                sortableDefinitionElement.SortByProofLength(subject, nameMapper, memory);

            //sortableElementList.Sort();

            return GetDefinition(sortableElementList);
        }
        #endregion

        #region Private methods
        private List<SortableDefinitionElement> GetSortableElementList(Dictionary<string, List<string>> definition, int sortingMode)
        {
            List<SortableDefinitionElement> sortableElementList = new List<SortableDefinitionElement>();

            foreach (KeyValuePair<string, List<string>> verbAndComplementList in definition)
                sortableElementList.Add(new SortableDefinitionElement(verbAndComplementList.Key, verbAndComplementList.Value, sortingMode));

            return sortableElementList;
        }

        private Dictionary<string, List<string>> GetDefinition(List<SortableDefinitionElement> sortableElementList)
        {
            Dictionary<string, List<string>> definition = new Dictionary<string, List<string>>();

            foreach (SortableDefinitionElement sortableDefinitionElement in sortableElementList)
                definition.Add(sortableDefinitionElement.Verb, sortableDefinitionElement.ComplementList);

            return definition;
        }
        #endregion
    }
}
