using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used to build string based definitions
    /// </summary>
    class StringBasedDefinitionBuilder
    {
        #region Public Methods
        /// <summary>
        /// Produce a "whatis" definition from concept id
        /// </summary>
        /// <param name="conceptId">concept Id</param>
        /// <param name="brain">brain to look into</param>
        /// <param name="nameMapper">name mapper to look into</param>
        /// <returns>"whatis" definition from concept id</returns>
        public Dictionary<string, List<string>> GetDefinitionWhatis(int conceptId, Brain brain, NameMapper nameMapper)
        {
            Dictionary<string, List<string>> definitionListWhatis = new Dictionary<string, List<string>>();
            Dictionary<int, List<int>> definitionByConceptId = brain.GetShortDefinition(conceptId);

            //For connections
            foreach (KeyValuePair<int, List<int>> verbAndBranch in definitionByConceptId)
            {
                int verb = verbAndBranch.Key;
                List<int> branch = verbAndBranch.Value;
                string verbName = nameMapper.GetConceptNames(verb)[0];

                List<string> branchInString = new List<string>();
                foreach (int complementId in branch)
                    branchInString.Add(nameMapper.GetConceptNames(complementId)[0]);

                definitionListWhatis.Add(verbName, branchInString);
            }

            //For metaConnections
            foreach (string metaOperator in LanguageDictionary.MetaOperatorList)
            {
                List<int> metaConnectionVerbIdList = brain.GetOptimizedMetaOperationVerbIdList(conceptId, metaOperator);
                List<string> metaConnectionVerbList = new List<string>();
                foreach (int farVerbId in metaConnectionVerbIdList)
                    metaConnectionVerbList.Add(nameMapper.GetConceptNames(farVerbId)[0]);

                if (metaConnectionVerbList.Count > 0)
                    definitionListWhatis.Add(metaOperator.ToUpper(), metaConnectionVerbList);
            }
            return definitionListWhatis;
        }

        /// <summary>
        /// Convert a definition using concept ids to a definition using concept names
        /// </summary>
        /// <param name="definitionById">definition by id</param>
        /// <param name="brain">brain to look into</param>
        /// <param name="nameMapper">name mapper to look into</param>
        /// <returns>definition by string</returns>
        public Dictionary<string, List<string>> DefinitionIdsToStrings(Dictionary<int, List<int>> definitionById, Brain brain, NameMapper nameMapper)
        {
            Dictionary<string, List<string>> definition = new Dictionary<string, List<string>>();
            foreach (KeyValuePair<int, List<int>> verbAndBranch in definitionById)
            {
                int verb = verbAndBranch.Key;
                List<int> branch = verbAndBranch.Value;
                string verbName = nameMapper.GetConceptNames(verb)[0];

                List<string> branchInString = new List<string>();
                foreach (int complementId in branch)
                    branchInString.Add(nameMapper.GetConceptNames(complementId)[0]);

                definition.Add(verbName, branchInString);
            }
            return definition;
        }
        #endregion
    }
}
