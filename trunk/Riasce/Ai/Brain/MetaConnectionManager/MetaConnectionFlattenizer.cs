using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents the metaConnection flattenizer
    /// </summary>
    class MetaConnectionFlattenizer
    {
        #region Public Methods
        /// <summary>
        /// Returns all the verbs that are (recursively or not) metaconnected to sourceVerb through metaOperator name
        /// If data exist in cache, use data from cache, or else, regenerate it
        /// </summary>
        /// <param name="sourceVerb">source verb</param>
        /// <param name="metaOperatorName">metaOperator name</param>
        /// <param name="isMetaConnectionPositive">whether you want positive or negative metaconnections</param>
        /// <returns>List of verbs that are (recursively or not) metaconnected to sourceVerb through metaOperator name</returns>
        public HashSet<Concept> GetFlatVerbListFromMetaConnection(Concept sourceVerb, string metaOperatorName, bool isMetaConnectionPositive)
        {
            HashSet<Concept> affectedVerbList;

            if (isMetaConnectionPositive)
                affectedVerbList = RenderFlatMetaConnectionVerbList(sourceVerb, metaOperatorName, new HashSet<string>(), isMetaConnectionPositive);
            else
                affectedVerbList = GetFlatNegativeMetaConnectionsToVerb(sourceVerb, metaOperatorName);

            return affectedVerbList;
        }

        /// <summary>
        /// Add a verb concept to rememberable total list of verbs
        /// </summary>
        /// <param name="concept">verb to remember</param>
        public void AddVerbToList(Concept concept)
        {
            Memory.TotalVerbList.Add(concept);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get flat negative metaConnection to verb
        /// </summary>
        /// <param name="verb">verb concept</param>
        /// <param name="metaOperatorName">metaOperator name</param>
        /// <returns>flat negative metaConnection to verb</returns>
        private HashSet<Concept> GetFlatNegativeMetaConnectionsToVerb(Concept verb, string metaOperatorName)
        {
            HashSet<Concept> flatNegativeMetaConnectionList = new HashSet<Concept>();

            foreach (Concept farVerb in Memory.TotalVerbList)
            {
                HashSet<Concept> farVerbConnectionList = GetFlatVerbListFromMetaConnection(farVerb, metaOperatorName, true);
                if (farVerbConnectionList.Contains(verb))
                    flatNegativeMetaConnectionList.Add(farVerb);
            }

            return flatNegativeMetaConnectionList;
        }

        /// <summary>
        /// Render flat metaconnection verb list
        /// </summary>
        /// <param name="sourceVerb">source verb</param>
        /// <param name="metaOperatorName">metaOperator name</param>
        /// <param name="ignoreList">ignore list</param>
        /// <param name="isMetaConnectionPositive">whether the metaConnection is positive</param>
        /// <returns>rendered verb list</returns>
        private HashSet<Concept> RenderFlatMetaConnectionVerbList(Concept sourceVerb, string metaOperatorName, HashSet<string> ignoreList, bool isMetaConnectionPositive)
        {
            HashSet<Concept> flatMetaConnectionVerbList = new HashSet<Concept>();

            #region To build the first layer, we determine which branch must be used from bool isMetaConnectionPositive
            if (isMetaConnectionPositive)
                flatMetaConnectionVerbList.UnionWith(sourceVerb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection(metaOperatorName));
            else
                flatMetaConnectionVerbList.UnionWith(sourceVerb.MetaConnectionTreeNegative.GetAffectedOperatorsByMetaConnection(metaOperatorName));
            #endregion

            #region We add current case to ignore list and we exit method if current case is in ignore list
            if (ignoreList.Count == 0)
                ignoreList.Add("first element");
            else if (ignoreList.Contains(isMetaConnectionPositive + metaOperatorName + sourceVerb.GetHashCode()))
                return flatMetaConnectionVerbList;
            else
                ignoreList.Add(isMetaConnectionPositive + metaOperatorName + sourceVerb.GetHashCode());
            #endregion

            if (metaOperatorName == "sublar")
            {
                flatMetaConnectionVerbList = RenderFlatMetaConnectionSublar(flatMetaConnectionVerbList, sourceVerb, ignoreList, isMetaConnectionPositive);
            }
            else if (metaOperatorName == "consics")
            {
                flatMetaConnectionVerbList = RenderFlatMetaConnectionConsics(flatMetaConnectionVerbList, sourceVerb, ignoreList, isMetaConnectionPositive); 
            }
            else if (metaOperatorName == "liffid")
            {
                flatMetaConnectionVerbList = RenderFlatMetaConnectionLiffid(flatMetaConnectionVerbList, sourceVerb, ignoreList, isMetaConnectionPositive); 
            }
            else if (metaOperatorName == "muct")
            {
                flatMetaConnectionVerbList = RenderFlatMetaConnectionMuct(flatMetaConnectionVerbList, sourceVerb, ignoreList, isMetaConnectionPositive);
            }
            else if (metaOperatorName == "unlikely")
            {
                flatMetaConnectionVerbList = RenderFlatMetaConnectionUnlikely(flatMetaConnectionVerbList, sourceVerb, ignoreList, isMetaConnectionPositive);
            }
            else if (metaOperatorName == "cant")
            {
                flatMetaConnectionVerbList = RenderFlatMetaConnectionCant(flatMetaConnectionVerbList, sourceVerb, ignoreList, isMetaConnectionPositive);
            }
            else if (metaOperatorName == "inverse_implication")
            {
                flatMetaConnectionVerbList = RenderFlatMetaConnectionInverseImplication(flatMetaConnectionVerbList, sourceVerb, ignoreList, isMetaConnectionPositive);
            }
            else if (metaOperatorName == "direct_implication")
            {
                flatMetaConnectionVerbList = RenderFlatMetaConnectionDirectImplication(flatMetaConnectionVerbList, sourceVerb, ignoreList, isMetaConnectionPositive);
            }
            return flatMetaConnectionVerbList;
        }

        /// <summary>
        /// Render flat metaConnection direct Implication
        /// </summary>
        /// <param name="directImplicationFlatMetaConnectionVerbList">direct Implication flat metaConnection verb list</param>
        /// <param name="sourceVerb">source verb</param>
        /// <param name="ignoreList">ignore list</param>
        /// <param name="isMetaConnectionPositive">whether the metaConnection is positive</param>
        /// <returns>rendered verb list</returns>
        private HashSet<Concept> RenderFlatMetaConnectionDirectImplication(HashSet<Concept> directImplicationFlatMetaConnectionVerbList, Concept sourceVerb, HashSet<string> ignoreList, bool isMetaConnectionPositive)
        {
            if (isMetaConnectionPositive)
            {
                //For recursivity
                HashSet<Concept> connectionList = new HashSet<Concept>(directImplicationFlatMetaConnectionVerbList);//RenderFlatMetaConnectionVerbList(sourceVerb, "direct_implication", ignoreList, isMetaConnectionPositive);
                foreach (Concept farVerb in connectionList)
                {
                    HashSet<Concept> farConnectionList = RenderFlatMetaConnectionVerbList(farVerb, "direct_implication", ignoreList, isMetaConnectionPositive);
                    directImplicationFlatMetaConnectionVerbList.UnionWith(farConnectionList);
                }

                HashSet<Concept> inverseImplicationConnections = RenderFlatMetaConnectionVerbList(sourceVerb, "inverse_implication", ignoreList, isMetaConnectionPositive);
                foreach (Concept inverseImplicationVerb in inverseImplicationConnections)
                {
                    HashSet<Concept> farInverseVerbList = RenderFlatMetaConnectionVerbList(inverseImplicationVerb, "inverse_of", ignoreList, isMetaConnectionPositive);
                    directImplicationFlatMetaConnectionVerbList.UnionWith(farInverseVerbList);

                    farInverseVerbList = RenderFlatMetaConnectionVerbList(inverseImplicationVerb, "inverse_implication", ignoreList, isMetaConnectionPositive);
                    directImplicationFlatMetaConnectionVerbList.UnionWith(farInverseVerbList);

                    #warning Next block might cause problem (new stuff)
                    if (inverseImplicationVerb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("permutable_side").Contains(inverseImplicationVerb))
                        directImplicationFlatMetaConnectionVerbList.Add(inverseImplicationVerb);
                }

                //For recursivity
                connectionList = new HashSet<Concept>(directImplicationFlatMetaConnectionVerbList);//RenderFlatMetaConnectionVerbList(sourceVerb, "direct_implication", ignoreList, isMetaConnectionPositive);
                foreach (Concept farVerb in connectionList)
                {
                    HashSet<Concept> farConnectionList = RenderFlatMetaConnectionVerbList(farVerb, "direct_implication", ignoreList, isMetaConnectionPositive);
                    directImplicationFlatMetaConnectionVerbList.UnionWith(farConnectionList);
                }

                HashSet<Concept> inverseOfConnections = RenderFlatMetaConnectionVerbList(sourceVerb, "inverse_of", ignoreList, isMetaConnectionPositive);
                foreach (Concept inverseOfVerb in inverseOfConnections)
                {
                    HashSet<Concept> farInverseImplicationVerbList = RenderFlatMetaConnectionVerbList(inverseOfVerb, "inverse_implication", ignoreList, isMetaConnectionPositive);
                    directImplicationFlatMetaConnectionVerbList.UnionWith(farInverseImplicationVerbList);
                }
            }
            else
            {
                //For recursivity
                HashSet<Concept> connectionList = new HashSet<Concept>(directImplicationFlatMetaConnectionVerbList);//RenderFlatMetaConnectionVerbList(sourceVerb, "direct_implication", ignoreList, isMetaConnectionPositive);
                foreach (Concept farVerb in connectionList)
                {
                    HashSet<Concept> farConnectionList = RenderFlatMetaConnectionVerbList(farVerb, "direct_implication", ignoreList, isMetaConnectionPositive);
                    directImplicationFlatMetaConnectionVerbList.UnionWith(farConnectionList);
                }


                HashSet<Concept> inverseOfConnections = RenderFlatMetaConnectionVerbList(sourceVerb, "inverse_of", ignoreList, !isMetaConnectionPositive);
                foreach (Concept inverseOfVerb in inverseOfConnections)
                {
                    HashSet<Concept> farInverseImplicationVerbList = RenderFlatMetaConnectionVerbList(inverseOfVerb, "inverse_implication", ignoreList, isMetaConnectionPositive);
                    directImplicationFlatMetaConnectionVerbList.UnionWith(farInverseImplicationVerbList);

                    HashSet<Concept> farDirectImplicationVerbList = RenderFlatMetaConnectionVerbList(inverseOfVerb, "direct_implication", ignoreList, isMetaConnectionPositive);
                    foreach (Concept directVerb in farDirectImplicationVerbList)
                    {
                        HashSet<Concept> farDirectInverseOfVerbList = RenderFlatMetaConnectionVerbList(directVerb, "inverse_of", ignoreList, !isMetaConnectionPositive);
                        directImplicationFlatMetaConnectionVerbList.UnionWith(farDirectInverseOfVerbList);
                    }
                }

                //HashSet<Concept> inverseImplicationConnections = RenderFlatMetaConnectionVerbList(sourceVerb, "inverse_implication", ignoreList, isMetaConnectionPositive);
            }

            return directImplicationFlatMetaConnectionVerbList;
        }

        /// <summary>
        /// Render flat metaConnection inverse_of
        /// </summary>
        /// <param name="inverseFlatMetaConnectionVerbList">inverse flat metaConnection verb list</param>
        /// <param name="sourceVerb">source verb</param>
        /// <param name="ignoreList">ignore list</param>
        /// <param name="isMetaConnectionPositive">whether the metaConnection is positive</param>
        /// <returns>rendered verb list</returns>
        private HashSet<Concept> RenderFlatMetaConnectionInverseImplication(HashSet<Concept> inverseFlatMetaConnectionVerbList, Concept sourceVerb, HashSet<string> ignoreList, bool isMetaConnectionPositive)
        {
            if (isMetaConnectionPositive)
            {
                HashSet<Concept> inverseOfConnections = RenderFlatMetaConnectionVerbList(sourceVerb, "inverse_of", ignoreList, isMetaConnectionPositive);
                foreach (Concept inverseOfVerb in inverseOfConnections)
                {
                    HashSet<Concept> farInverseImplicationVerbList = RenderFlatMetaConnectionVerbList(inverseOfVerb, "direct_implication", ignoreList, isMetaConnectionPositive);
                    inverseFlatMetaConnectionVerbList.UnionWith(farInverseImplicationVerbList);
                }

                //Pre recursivity
                HashSet<Concept> directImplicationConnections = RenderFlatMetaConnectionVerbList(sourceVerb, "direct_implication", ignoreList, isMetaConnectionPositive);
                foreach (Concept directImplicationVerb in directImplicationConnections)
                {
                    HashSet<Concept> farInverseImplicationVerbList = RenderFlatMetaConnectionVerbList(directImplicationVerb, "inverse_implication", ignoreList, isMetaConnectionPositive);
                    inverseFlatMetaConnectionVerbList.UnionWith(farInverseImplicationVerbList);

                    #warning Next block might cause problem (new stuff)
                    if (directImplicationVerb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("permutable_side").Contains(directImplicationVerb))
                        inverseFlatMetaConnectionVerbList.Add(directImplicationVerb);
                }

                //Post recursivity
                HashSet<Concept> inverseImplicationConnections = new HashSet<Concept>(inverseFlatMetaConnectionVerbList);//RenderFlatMetaConnectionVerbList(sourceVerb, "inverse_implication", ignoreList, isMetaConnectionPositive);
                foreach (Concept inverseImplicationVerb in inverseImplicationConnections)
                {
                    HashSet<Concept> farDirectImplicationVerbList = RenderFlatMetaConnectionVerbList(inverseImplicationVerb, "direct_implication", ignoreList, isMetaConnectionPositive);
                    inverseFlatMetaConnectionVerbList.UnionWith(farDirectImplicationVerbList);
                }
            }
            else
            {
                HashSet<Concept> inverseOfConnections = RenderFlatMetaConnectionVerbList(sourceVerb, "inverse_of", ignoreList, !isMetaConnectionPositive);
                foreach (Concept inverseOfVerb in inverseOfConnections)
                {
                    HashSet<Concept> farInverseImplicationVerbList = RenderFlatMetaConnectionVerbList(inverseOfVerb, "inverse_implication", ignoreList, isMetaConnectionPositive);
                    foreach (Concept farVerb in farInverseImplicationVerbList)
                    {
                        HashSet<Concept> farInverseOfVerbList = RenderFlatMetaConnectionVerbList(farVerb, "inverse_of", ignoreList, !isMetaConnectionPositive);
                        inverseFlatMetaConnectionVerbList.UnionWith(farInverseOfVerbList);
                    }


                    HashSet<Concept> farDirectImplicationVerbList = RenderFlatMetaConnectionVerbList(inverseOfVerb, "direct_implication", ignoreList, isMetaConnectionPositive);
                    inverseFlatMetaConnectionVerbList.UnionWith(farDirectImplicationVerbList);

                }

                //Pre recursivity
                HashSet<Concept> directImplicationConnections = RenderFlatMetaConnectionVerbList(sourceVerb, "direct_implication", ignoreList, isMetaConnectionPositive);
                foreach (Concept directImplicationVerb in directImplicationConnections)
                {
                    HashSet<Concept> farInverseImplicationVerbList = RenderFlatMetaConnectionVerbList(directImplicationVerb, "inverse_implication", ignoreList, isMetaConnectionPositive);
                    inverseFlatMetaConnectionVerbList.UnionWith(farInverseImplicationVerbList);
                }

                //Post recursivity
                HashSet<Concept> inverseImplicationConnections = new HashSet<Concept>(inverseFlatMetaConnectionVerbList);//RenderFlatMetaConnectionVerbList(sourceVerb, "inverse_implication", ignoreList, isMetaConnectionPositive);
                foreach (Concept inverseImplicationVerb in inverseImplicationConnections)
                {
                    HashSet<Concept> farDirectImplicationVerbList = RenderFlatMetaConnectionVerbList(inverseImplicationVerb, "direct_implication", ignoreList, isMetaConnectionPositive);
                    inverseFlatMetaConnectionVerbList.UnionWith(farDirectImplicationVerbList);
                }
            }

            return inverseFlatMetaConnectionVerbList;
        }

        /// <summary>
        /// Render flat metaConnection cant
        /// </summary>
        /// <param name="cantFlatMetaConnectionVerbList">cant flat metaConnection verb list</param>
        /// <param name="sourceVerb">source verb</param>
        /// <param name="ignoreList">ignore list</param>
        /// <param name="isMetaConnectionPositive">whether the metaConnection is positive</param>
        /// <returns>rendered verb list</returns>
        private HashSet<Concept> RenderFlatMetaConnectionCant(HashSet<Concept> cantFlatMetaConnectionVerbList, Concept sourceVerb, HashSet<string> ignoreList, bool isMetaConnectionPositive)
        {
            HashSet<Concept> inverseDirectImplicationList = RenderFlatMetaConnectionVerbList(sourceVerb, "direct_implication", ignoreList, !isMetaConnectionPositive);

            //Pre recursivity (highly experimental)
            HashSet<Concept> directImplicationList = RenderFlatMetaConnectionVerbList(sourceVerb, "direct_implication", ignoreList, isMetaConnectionPositive);
            foreach (Concept farVerb in directImplicationList)
            {
                HashSet<Concept> farDirectImplicationList = RenderFlatMetaConnectionVerbList(farVerb, "cant", ignoreList, isMetaConnectionPositive);

                foreach (Concept farDirectImplicationConcept in farDirectImplicationList)
                {
                    if (farDirectImplicationConcept != sourceVerb)
                    {
                        if (!directImplicationList.Contains(farDirectImplicationConcept) && !inverseDirectImplicationList.Contains(farDirectImplicationConcept))
                        {
                            cantFlatMetaConnectionVerbList.Add(farDirectImplicationConcept);
                        }
                    }
                }
            }


            HashSet<Concept> inverseOfList = RenderFlatMetaConnectionVerbList(sourceVerb, "inverse_of", ignoreList, isMetaConnectionPositive);
            foreach (Concept farVerb in inverseOfList)
            {
                //Implicit cant from inverse_of
                cantFlatMetaConnectionVerbList.UnionWith(inverseOfList);

                HashSet<Concept> farConnectionList;
                farConnectionList = RenderFlatMetaConnectionVerbList(farVerb, "cant", ignoreList, isMetaConnectionPositive);

                foreach (Concept farCantVerb in farConnectionList)
                {
                    if (sourceVerb != farCantVerb)
                    {
                        if (!directImplicationList.Contains(farCantVerb) && !inverseDirectImplicationList.Contains(farCantVerb))
                        {
                            cantFlatMetaConnectionVerbList.Add(farCantVerb);
                        }
                    }
                }
            }


            HashSet<Concept> cantConnectionList = RenderFlatMetaConnectionVerbList(sourceVerb, "cant", ignoreList, isMetaConnectionPositive);
            foreach (Concept cantVerb in cantConnectionList)
            {
                HashSet<Concept> farFarConnectionList = RenderFlatMetaConnectionVerbList(cantVerb, "inverse_of", ignoreList, isMetaConnectionPositive);
                foreach (Concept farFarConnectionVerb in farFarConnectionList)
                {
                    if (sourceVerb != farFarConnectionVerb)
                    {
                        if (!directImplicationList.Contains(farFarConnectionVerb) && !inverseDirectImplicationList.Contains(farFarConnectionVerb))
                        {
                            cantFlatMetaConnectionVerbList.Add(farFarConnectionVerb);
                        }
                    }
                }

                farFarConnectionList = RenderFlatMetaConnectionVerbList(cantVerb, "direct_implication", ignoreList, !isMetaConnectionPositive);
                foreach (Concept farFarConnectionVerb in farFarConnectionList)
                {
                    if (sourceVerb != farFarConnectionVerb)
                    {
                        if (!directImplicationList.Contains(farFarConnectionVerb) && !inverseDirectImplicationList.Contains(farFarConnectionVerb))
                        {
                            cantFlatMetaConnectionVerbList.Add(farFarConnectionVerb);
                        }
                    }
                }

                foreach (Concept directFarVerb in farFarConnectionList)
                {
                    HashSet<Concept> farFarInverseConnectionList = RenderFlatMetaConnectionVerbList(directFarVerb, "inverse_of", ignoreList, isMetaConnectionPositive);

                    foreach (Concept farFarInverseVerb in farFarInverseConnectionList)
                    {
                        if (farFarInverseVerb != sourceVerb)
                        {
                            if (!directImplicationList.Contains(farFarInverseVerb) && !inverseDirectImplicationList.Contains(farFarInverseVerb))
                            {
                                cantFlatMetaConnectionVerbList.Add(farFarInverseVerb);
                            }
                        }
                    }
                }
            }

            return cantFlatMetaConnectionVerbList;
        }

        /// <summary>
        /// Render flat metaConnection unlikely
        /// </summary>
        /// <param name="unlikelyFlatMetaConnectionVerbList">unlikely flat metaConnection verb list</param>
        /// <param name="sourceVerb">source verb</param>
        /// <param name="ignoreList">ignore list</param>
        /// <param name="isMetaConnectionPositive">whether the metaConnection is positive</param>
        /// <returns>rendered verb list</returns>
        private HashSet<Concept> RenderFlatMetaConnectionUnlikely(HashSet<Concept> unlikelyFlatMetaConnectionVerbList, Concept sourceVerb, HashSet<string> ignoreList, bool isMetaConnectionPositive)
        {
            HashSet<Concept> inverseDirectImplicationList = RenderFlatMetaConnectionVerbList(sourceVerb, "direct_implication", ignoreList, !isMetaConnectionPositive);

            //Pre recursivity (highly experimental)
            HashSet<Concept> directImplicationList = RenderFlatMetaConnectionVerbList(sourceVerb, "direct_implication", ignoreList, isMetaConnectionPositive);
            foreach (Concept farVerb in directImplicationList)
            {
                HashSet<Concept> farDirectImplicationList = RenderFlatMetaConnectionVerbList(farVerb, "unlikely", ignoreList, isMetaConnectionPositive);

                foreach (Concept farDirectImplicationVerb in farDirectImplicationList)
                {
                    if (sourceVerb != farDirectImplicationVerb)
                    {
                        if (!directImplicationList.Contains(farDirectImplicationVerb) && !inverseDirectImplicationList.Contains(farDirectImplicationVerb))
                        {
                            unlikelyFlatMetaConnectionVerbList.Add(farDirectImplicationVerb);
                        }
                    }
                }
            }

            HashSet<Concept> InverseOfList = RenderFlatMetaConnectionVerbList(sourceVerb, "inverse_of", ignoreList, isMetaConnectionPositive);
            foreach (Concept farVerb in InverseOfList)
            {
                HashSet<Concept> farConnectionList;
                farConnectionList = RenderFlatMetaConnectionVerbList(farVerb, "unlikely", ignoreList, isMetaConnectionPositive);
                
                foreach (Concept farUnlikelyVerb in farConnectionList)
                {
                    if (sourceVerb != farUnlikelyVerb)
                    {
                        if (!directImplicationList.Contains(farUnlikelyVerb) && !inverseDirectImplicationList.Contains(farUnlikelyVerb))
                        {
                            unlikelyFlatMetaConnectionVerbList.Add(farUnlikelyVerb);
                        }
                    }
                }
            }

            HashSet<Concept> unlikelyConnectionList = RenderFlatMetaConnectionVerbList(sourceVerb, "unlikely", ignoreList, isMetaConnectionPositive);
            foreach (Concept cantVerb in unlikelyConnectionList)
            {
                HashSet<Concept> farFarConnectionList = RenderFlatMetaConnectionVerbList(cantVerb, "inverse_of", ignoreList, isMetaConnectionPositive);

                foreach (Concept farFarVerb in farFarConnectionList)
                {
                    if (sourceVerb != farFarVerb)
                    {
                        if (!directImplicationList.Contains(farFarVerb) && !inverseDirectImplicationList.Contains(farFarVerb))
                        {
                            unlikelyFlatMetaConnectionVerbList.Add(farFarVerb);
                        }
                    }
                }

                farFarConnectionList = RenderFlatMetaConnectionVerbList(cantVerb, "direct_implication", ignoreList, !isMetaConnectionPositive);

                foreach (Concept farFarVerb in farFarConnectionList)
                {
                    if (sourceVerb != farFarVerb)
                    {
                        if (!directImplicationList.Contains(farFarVerb) && !inverseDirectImplicationList.Contains(farFarVerb))
                        {
                            unlikelyFlatMetaConnectionVerbList.Add(farFarVerb);
                        }
                    }
                }

                foreach (Concept directFarVerb in farFarConnectionList)
                {
                    HashSet<Concept> farFarInverseConnectionList = RenderFlatMetaConnectionVerbList(directFarVerb, "inverse_of", ignoreList, isMetaConnectionPositive);

                    foreach (Concept farFarInverseVerb in farFarInverseConnectionList)
                    {
                        if (sourceVerb != farFarInverseVerb)
                        {
                            if (!directImplicationList.Contains(farFarInverseVerb) && !inverseDirectImplicationList.Contains(farFarInverseVerb))
                            {
                                unlikelyFlatMetaConnectionVerbList.Add(farFarInverseVerb);
                            }
                        }
                    }
                }
            }

            HashSet<Concept> cantConnectionList = RenderFlatMetaConnectionVerbList(sourceVerb, "cant", ignoreList, isMetaConnectionPositive);
            foreach (Concept cantVerb in cantConnectionList)
            {
                if (sourceVerb != cantVerb)
                {
                    if (!directImplicationList.Contains(cantVerb) && !inverseDirectImplicationList.Contains(cantVerb))
                    {
                        unlikelyFlatMetaConnectionVerbList.Add(cantVerb);
                    }
                }
            }

            return unlikelyFlatMetaConnectionVerbList;
        }

        /// <summary>
        /// Render flat metaConnection sublar
        /// </summary>
        /// <param name="sublarFlatMetaConnectionVerbList">sublar flat metaConnection verb list</param>
        /// <param name="sourceVerb">source verb</param>
        /// <param name="ignoreList">ignore list</param>
        /// <param name="isMetaConnectionPositive">whether the metaConnection is positive</param>
        /// <returns>rendered verb list</returns>
        private HashSet<Concept> RenderFlatMetaConnectionSublar(HashSet<Concept> sublarFlatMetaConnectionVerbList, Concept sourceVerb, HashSet<string> ignoreList, bool isMetaConnectionPositive)
        {
            HashSet<Concept> muctConnectionList = RenderFlatMetaConnectionVerbList(sourceVerb, "muct", ignoreList, isMetaConnectionPositive);
            HashSet<Concept> farInverseOfConnectionList;

            foreach (Concept farVerb in muctConnectionList)
            {
                farInverseOfConnectionList = RenderFlatMetaConnectionVerbList(farVerb, "inverse_of", ignoreList, isMetaConnectionPositive);
                sublarFlatMetaConnectionVerbList.UnionWith(farInverseOfConnectionList);

                HashSet<Concept> farPermutableSideConnectionList = RenderFlatMetaConnectionVerbList(farVerb, "permutable_side", ignoreList, isMetaConnectionPositive);
                sublarFlatMetaConnectionVerbList.UnionWith(farPermutableSideConnectionList);
            }

            HashSet<Concept> farLiffidConnectionList;
            HashSet<Concept> inverseOfConnectionList = RenderFlatMetaConnectionVerbList(sourceVerb, "inverse_of", ignoreList, isMetaConnectionPositive);
            foreach (Concept farVerb in inverseOfConnectionList)
            {
                farLiffidConnectionList = RenderFlatMetaConnectionVerbList(farVerb, "liffid", ignoreList, isMetaConnectionPositive);
                sublarFlatMetaConnectionVerbList.UnionWith(farLiffidConnectionList);
            }

            HashSet<Concept> permutableSideConnectionList = RenderFlatMetaConnectionVerbList(sourceVerb, "permutable_side", ignoreList, isMetaConnectionPositive);
            foreach (Concept farVerb in permutableSideConnectionList)
            {
                farLiffidConnectionList = RenderFlatMetaConnectionVerbList(farVerb, "liffid", ignoreList, isMetaConnectionPositive);
                sublarFlatMetaConnectionVerbList.UnionWith(farLiffidConnectionList);
            }

            return sublarFlatMetaConnectionVerbList;
        }

        /// <summary>
        /// Render flat metaConnection consics
        /// </summary>
        /// <param name="consicsFlatMetaConnectionVerbList">consics flat metaConnection verb list</param>
        /// <param name="sourceVerb">source verb</param>
        /// <param name="ignoreList">ignore list</param>
        /// <param name="isMetaConnectionPositive">whether the metaConnection is positive</param>
        /// <returns>rendered verb list</returns>
        private HashSet<Concept> RenderFlatMetaConnectionConsics(HashSet<Concept> consicsFlatMetaConnectionVerbList, Concept sourceVerb, HashSet<string> ignoreList, bool isMetaConnectionPositive)
        {
            HashSet<Concept> farMuctConnectionList;

            HashSet<Concept> inverseOfConnectionList = RenderFlatMetaConnectionVerbList(sourceVerb, "inverse_of", ignoreList, isMetaConnectionPositive);
            foreach (Concept farVerb in inverseOfConnectionList)
            {
                farMuctConnectionList = RenderFlatMetaConnectionVerbList(farVerb, "muct", ignoreList, isMetaConnectionPositive);
                consicsFlatMetaConnectionVerbList.UnionWith(farMuctConnectionList);
            }

            HashSet<Concept> permutableSideConnectionList = RenderFlatMetaConnectionVerbList(sourceVerb, "permutable_side", ignoreList, isMetaConnectionPositive);
            foreach (Concept farVerb in permutableSideConnectionList)
            {
                farMuctConnectionList = RenderFlatMetaConnectionVerbList(farVerb, "muct", ignoreList, isMetaConnectionPositive);
                consicsFlatMetaConnectionVerbList.UnionWith(farMuctConnectionList);
            }

            HashSet<Concept> liffidConnectionList = RenderFlatMetaConnectionVerbList(sourceVerb, "liffid", ignoreList, isMetaConnectionPositive);
            HashSet<Concept> farInverseOfConnectionList;

            foreach (Concept farVerb in liffidConnectionList)
            {
                farInverseOfConnectionList = RenderFlatMetaConnectionVerbList(farVerb, "inverse_of", ignoreList, isMetaConnectionPositive);
                consicsFlatMetaConnectionVerbList.UnionWith(farInverseOfConnectionList);

                HashSet<Concept> farPermutableSideConnectionList = RenderFlatMetaConnectionVerbList(farVerb, "permutable_side", ignoreList, isMetaConnectionPositive);
                consicsFlatMetaConnectionVerbList.UnionWith(farPermutableSideConnectionList);
            }

            return consicsFlatMetaConnectionVerbList;
        }

        /// <summary>
        /// Render flat metaConnection liffid
        /// </summary>
        /// <param name="liffidFlatMetaConnectionVerbList">liffid flat metaConnection verb list</param>
        /// <param name="sourceVerb">source verb</param>
        /// <param name="ignoreList">ignore list</param>
        /// <param name="isMetaConnectionPositive">whether the metaConnection is positive</param>
        /// <returns>rendered verb list</returns>
        private HashSet<Concept> RenderFlatMetaConnectionLiffid(HashSet<Concept> liffidFlatMetaConnectionVerbList, Concept sourceVerb, HashSet<string> ignoreList, bool isMetaConnectionPositive)
        {
            HashSet<Concept> consicsConnectionList = RenderFlatMetaConnectionVerbList(sourceVerb, "consics", ignoreList, isMetaConnectionPositive);
            HashSet<Concept> farInverseOfConnectionList;

            foreach (Concept farVerb in consicsConnectionList)
            {
                farInverseOfConnectionList = RenderFlatMetaConnectionVerbList(farVerb, "inverse_of", ignoreList, isMetaConnectionPositive);
                liffidFlatMetaConnectionVerbList.UnionWith(farInverseOfConnectionList);

                HashSet<Concept> farPermutableSideConnectionList = RenderFlatMetaConnectionVerbList(farVerb, "permutable_side", ignoreList, isMetaConnectionPositive);
                liffidFlatMetaConnectionVerbList.UnionWith(farPermutableSideConnectionList);
            }

            
            #warning! Might cause problem
            //If madeof liffid isa, then isa liffid isa
            //UNLESS ISA PERMUTABLE_SIDE ISA!!!!!!!!!!!!!!!
            if (isMetaConnectionPositive)
            {
                if (!sourceVerb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("permutable_side").Contains(sourceVerb))
                {
                    HashSet<Concept> negativeLiffidConnectionList = RenderFlatMetaConnectionVerbList(sourceVerb, "liffid", ignoreList, !isMetaConnectionPositive);
                    if (negativeLiffidConnectionList.Count > 0)
                        liffidFlatMetaConnectionVerbList.Add(sourceVerb);
                }
            }
            
            
            
            #warning! Might cause problem (cause problem because madeof liffid need and we don't want this
            //If self muct, then self liffid
            if (isMetaConnectionPositive)
            {
                HashSet<Concept> positiveMuctConnectionList = RenderFlatMetaConnectionVerbList(sourceVerb, "muct", ignoreList, isMetaConnectionPositive);
                if (positiveMuctConnectionList.Contains(sourceVerb))
                    liffidFlatMetaConnectionVerbList.Add(sourceVerb);
            }

            return liffidFlatMetaConnectionVerbList;
        }

        /// <summary>
        /// Render flat metaConnection muct
        /// </summary>
        /// <param name="muctFlatMetaConnectionVerbList">muct flat metaConnection verb list</param>
        /// <param name="sourceVerb">source verb</param>
        /// <param name="ignoreList">ignore list</param>
        /// <param name="isMetaConnectionPositive">whether the metaConnection is positive</param>
        /// <returns>rendered verb list</returns>
        private HashSet<Concept> RenderFlatMetaConnectionMuct(HashSet<Concept> muctFlatMetaConnectionVerbList, Concept sourceVerb, HashSet<string> ignoreList, bool isMetaConnectionPositive)
        {
            HashSet<Concept> sublarConnectionList = RenderFlatMetaConnectionVerbList(sourceVerb, "sublar", ignoreList, isMetaConnectionPositive);
            HashSet<Concept> farInverseOfConnectionList;

            foreach (Concept farVerb in sublarConnectionList)
            {
                farInverseOfConnectionList = RenderFlatMetaConnectionVerbList(farVerb, "inverse_of", ignoreList, isMetaConnectionPositive);
                muctFlatMetaConnectionVerbList.UnionWith(farInverseOfConnectionList);

                HashSet<Concept> farPermutableSideConnectionList = RenderFlatMetaConnectionVerbList(farVerb, "permutable_side", ignoreList, isMetaConnectionPositive);
                muctFlatMetaConnectionVerbList.UnionWith(farPermutableSideConnectionList);
            }


            #warning! Might cause problem
            //If madeof muct isa, then isa muct isa
            //UNLESS ISA PERMUTABLE_SIDE ISA!!!!!!!!!!!!!!!
            if (isMetaConnectionPositive)
            {
                if (!sourceVerb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("permutable_side").Contains(sourceVerb))
                {
                    HashSet<Concept> negativeMuctConnectionList = RenderFlatMetaConnectionVerbList(sourceVerb, "muct", ignoreList, !isMetaConnectionPositive);
                    if (negativeMuctConnectionList.Count > 0)
                        muctFlatMetaConnectionVerbList.Add(sourceVerb);
                }
            }

            return muctFlatMetaConnectionVerbList;
        }
        #endregion
    }
}
