﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents all the "imply" connections for a concept
    /// </summary>
    public class ImplyConnectionTree : IEnumerable<KeyValuePair<Concept, HashSet<Condition>>>
    {
        #region Fields
        /// <summary>
        /// Connection list
        /// key: complement concept
        /// value: condition
        /// </summary>
        private Dictionary<Concept, HashSet<Condition>> connectionList = new Dictionary<Concept, HashSet<Condition>>();
        #endregion

        #region Public methods
        /// <summary>
        /// Add imply connection to complement
        /// </summary>
        /// <param name="complement">complement concept</param>
        /// <param name="condition">condition</param>
        public void AddConnection(Concept complement, Condition condition)
        {
            if (TestConnection(complement, condition))
                return;

            HashSet<Condition> conditionList;

            if (!connectionList.TryGetValue(complement, out conditionList))
            {
                conditionList = new HashSet<Condition>();
                connectionList.Add(complement, conditionList);
            }

            conditionList.Add(condition);
        }

        /// <summary>
        /// Remove imply connection to complement
        /// </summary>
        /// <param name="complement">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <param name="condition">condition</param>
        public void RemoveConnection(Concept verb, Concept complement, Condition condition)
        {
            Condition conditionToRemove = null;
            HashSet<Condition> conditionList;
            if (connectionList.TryGetValue(complement, out conditionList))
            {
                foreach (Condition currentCondition in conditionList)
                {
                    if (currentCondition.Equals(condition))
                    {
                        conditionToRemove = currentCondition;
                        break;
                    }
                }
                if (conditionToRemove != null)
                {
                    HashSet<Concept> concernedConceptList = conditionToRemove.GetConcernedConceptList();
                    foreach (Concept currentConcernedConcept in concernedConceptList)
                        currentConcernedConcept.RemoveIndexToImplyCondition(conditionToRemove);
                    complement.RemoveIndexToImplyCondition(conditionToRemove);
                    verb.RemoveIndexToImplyCondition(conditionToRemove);

                    conditionList.Remove(conditionToRemove);
                    if (conditionList.Count < 1)
                        connectionList.Remove(complement);
                }
            }
        }

        /// <summary>
        /// Returns true if imply connection exists, else: false
        /// </summary>
        /// <param name="complement">complement concept</param>
        /// <param name="condition">condition</param>
        /// <returns>true if imply connection exists, else: false</returns>
        public bool TestConnection(Concept complement, Condition condition)
        {
            HashSet<Condition> conditionList;
            if (connectionList.TryGetValue(complement, out conditionList))
                foreach (Condition currentCondition in conditionList)
                    if (currentCondition.Equals(condition))
                        return true;
            return false;
        }

        public IEnumerator<KeyValuePair<Concept, HashSet<Condition>>> GetEnumerator()
        {
            return connectionList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}