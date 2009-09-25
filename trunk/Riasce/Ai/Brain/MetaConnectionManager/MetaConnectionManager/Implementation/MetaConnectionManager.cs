using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public class MetaConnectionManager : AbstractMetaConnectionManager
    {
        #region Fields
        private MetaConnectionFlattenizer metaConnectionFlattenizer = new MetaConnectionFlattenizer();
        #endregion

        #region Methods
        public override void AddMetaConnection(Concept concept1, string metaOperatorName, Concept concept2)
        {
            metaConnectionFlattenizer.AddVerbToList(concept1);
            metaConnectionFlattenizer.AddVerbToList(concept2);

            #region Throw exceptions if rules are broken
            if (metaOperatorName == "permutable_side")
            {
                if (concept1 != concept2)
                {
                    throw new MetaConnectionException("can only set permutable_side to itself");
                }
                else if (this.GetVerbListFromMetaConnection(concept1, "inverse_of", true).Count != 0)
                {
                    throw new MetaConnectionException("operator already inverse_of something");
                }
                else if (GetVerbFlatListFromMetaConnection(concept1, "muct", true).Contains(concept1))
                {
                    throw new MetaConnectionException("this operator can't self permutable_side because it already self muct");
                }
                else if (GetVerbFlatListFromMetaConnection(concept1, "liffid", true).Contains(concept1))
                {
                    throw new MetaConnectionException("this operator can't self permutable_side because it already self liffid");
                }
                else if (GetVerbFlatListFromMetaConnection(concept1, "sublar", true).Contains(concept1))
                {
                    throw new MetaConnectionException("this operator can't self permutable_side because it already self sublar");
                }
                else if (GetVerbFlatListFromMetaConnection(concept1, "consics", true).Contains(concept1))
                {
                    throw new MetaConnectionException("this operator can't self permutable_side because it already self consics");
                }
            }
            else if (metaOperatorName == "inverse_of")
            {
                if (concept1 == concept2)
                {
                    throw new MetaConnectionException("No operator can be inverse_of itself");
                }
                else if (this.GetVerbListFromMetaConnection(concept1, "inverse_of", true).Count != 0)
                {
                    throw new MetaConnectionException("operator already inverse_of something");
                }
                else if (this.GetVerbListFromMetaConnection(concept1, "permutable_side", true).Count != 0)
                {
                    throw new MetaConnectionException("operator already permutable_side itself");
                }
                else if (GetVerbFlatListFromMetaConnection(concept1, "muct", true).Contains(concept2))
                {
                    throw new MetaConnectionException("operator can't be the inverse_of what it mucts");
                }
                else if (GetVerbFlatListFromMetaConnection(concept1, "liffid", true).Contains(concept2))
                {
                    throw new MetaConnectionException("operator can't be the inverse_of what it liffids");
                }
            }
            else if (metaOperatorName == "cant")
            {
                if (concept1 == concept2)
                {
                    throw new MetaConnectionException("No operator can be mutually exclusive to itself");
                }
                else if (this.GetVerbFlatListFromMetaConnection(concept1, "direct_implication", true).Contains(concept2))
                {
                    throw new MetaConnectionException("It is impossible for operators to be both cant and direct_implication");
                }
                else if (this.GetVerbFlatListFromMetaConnection(concept2, "direct_implication", true).Contains(concept1))
                {
                    throw new MetaConnectionException("It is impossible for operators to be both cant and direct_implication");
                }
            }
            else if (metaOperatorName == "unlikely")
            {
                if (concept1 == concept2)
                {
                    throw new MetaConnectionException("No operator can be unlikely to itself");
                }
            }
            else if (metaOperatorName == "direct_implication")
            {
                if (concept1 == concept2)
                {
                    throw new MetaConnectionException("No operator can direct_implication itself");
                }
                else if (GetVerbFlatListFromMetaConnection(concept1, "cant", true).Contains(concept2))
                {
                    throw new MetaConnectionException("It is impossible for operators to be both cant and direct_implication");
                }
                else if (GetVerbFlatListFromMetaConnection(concept2, "cant", true).Contains(concept1))
                {
                    throw new MetaConnectionException("It is impossible for operators to be both cant and direct_implication");
                }
                else if (GetVerbFlatListFromMetaConnection(concept2, "direct_implication", true).Contains(concept1))
                {
                    throw new MetaConnectionException("No operator can direct_implication itself even when it's not explicit");
                }
                else if (GetVerbFlatListFromMetaConnection(concept1, "permutable_side", true).Contains(concept1))
                {
                    if (GetVerbFlatListFromMetaConnection(concept2, "inverse_of", true).Count > 0)
                    {
                        throw new MetaConnectionException("A permutable_side operator cannot imply an operator that has an inverse_of");
                    }
                }
            }
            else if (metaOperatorName == "inverse_implication")
            {
                if (concept1 == concept2)
                {
                    throw new MetaConnectionException("No operator can inverse_implication itself. Use permutable_side instead");
                }
                else if (GetVerbFlatListFromMetaConnection(concept2, "inverse_implication", true).Contains(concept1))
                {
                    throw new MetaConnectionException("No operators can be mutually inverse_implication even when it's not explicit");
                }
                else if (GetVerbFlatListFromMetaConnection(concept1, "permutable_side", true).Contains(concept1))
                {
                    if (GetVerbFlatListFromMetaConnection(concept2, "inverse_of", true).Count > 0)
                    {
                        throw new MetaConnectionException("A permutable_side operator cannot imply an operator that has an inverse_of");
                    }
                }
            }
            else if (metaOperatorName == "muct")
            {
                if (concept1 == concept2 && GetVerbFlatListFromMetaConnection(concept1, "permutable_side", true).Contains(concept1))
                {
                    throw new MetaConnectionException("this operator can't self muct because it already self permutable_side");
                }
                else if (GetVerbFlatListFromMetaConnection(concept1, "inverse_of", true).Contains(concept2))
                {
                    throw new MetaConnectionException("operator can't muct its inverse");
                }
                else if (GetVerbFlatListFromMetaConnection(concept1, "sublar", true).Contains(concept2))
                {
                    throw new MetaConnectionException("can't both muct and sublar");
                }
            }
            else if (metaOperatorName == "liffid")
            {
                if (concept1 == concept2 && GetVerbFlatListFromMetaConnection(concept1, "permutable_side", true).Contains(concept1))
                {
                    throw new MetaConnectionException("this operator can't self liffid because it already self permutable_side");
                }
                else if (GetVerbFlatListFromMetaConnection(concept1, "inverse_of", true).Contains(concept2))
                {
                    throw new MetaConnectionException("operator can't liffid its inverse");
                }
                else if (GetVerbFlatListFromMetaConnection(concept1, "consics", true).Contains(concept2))
                {
                    throw new MetaConnectionException("can't both liffid and consics");
                }
            }
            else if (metaOperatorName == "sublar")
            {
                if (concept1 == concept2)
                {
                    throw new MetaConnectionException("No operator should self sublar. Use permutable_side instead.");
                }
                else if (GetVerbFlatListFromMetaConnection(concept1, "muct", true).Contains(concept2))
                {
                    throw new MetaConnectionException("can't both muct and sublar");
                }
            }
            else if (metaOperatorName == "consics")
            {
                if (concept1 == concept2)
                {
                    throw new MetaConnectionException("No operator should self consics. Use permutable_side instead.");
                }
                else if (GetVerbFlatListFromMetaConnection(concept1, "liffid", true).Contains(concept2))
                {
                    throw new MetaConnectionException("can't both liffid and consics");
                }
            }

            if (metaOperatorName == "sublar" || metaOperatorName == "consics" || metaOperatorName == "inverse_implication" || metaOperatorName == "direct_implication")
            {
                if (GetVerbFlatListFromMetaConnection(concept1, "inverse_of", true).Count < 1)
                    if (GetVerbFlatListFromMetaConnection(concept1, "permutable_side", true).Count < 1)
                        throw new MetaConnectionException("Not having any permutable_side or inverse_of metaConnection can have undesirable effects.");

                if (GetVerbFlatListFromMetaConnection(concept2, "inverse_of", true).Count < 1)
                    if (GetVerbFlatListFromMetaConnection(concept2, "permutable_side", true).Count < 1)
                        throw new MetaConnectionException("Not having any permutable_side or inverse_of metaConnection can have undesirable effects.");
            }
            #endregion

            if (metaOperatorName == "permutable_side" || metaOperatorName == "inverse_of" || metaOperatorName == "cant" || metaOperatorName == "unlikely")
            {
                concept1.AddMetaConnection(metaOperatorName, concept2, true);
                concept2.AddMetaConnection(metaOperatorName, concept1, true);
            }
            else
            {
                concept1.AddMetaConnection(metaOperatorName, concept2, true);
                concept2.AddMetaConnection(metaOperatorName, concept1, false);
            }
        }

        public override void RemoveMetaConnection(Concept concept1, string metaOperatorName, Concept concept2)
        {
            if (metaOperatorName == "permutable_side" || metaOperatorName == "inverse_of" || metaOperatorName == "cant" || metaOperatorName == "unlikely")
            {
                concept1.RemoveMetaConnection(metaOperatorName, concept2, true);
                concept2.RemoveMetaConnection(metaOperatorName, concept1, true);
            }
            else
            {
                concept1.RemoveMetaConnection(metaOperatorName, concept2, true);
                concept2.RemoveMetaConnection(metaOperatorName, concept1, false);
            }

            if (IsFlatMetaConnected(concept1, metaOperatorName, concept2))
                throw new MetaConnectionException("MetaConnection is still present as the result of the combination of other metaConnections.");
        }

        public override bool IsMetaConnected(Concept concept1, string metaOperatorName, Concept concept2)
        {
            if (metaOperatorName == "permutable_side" || metaOperatorName == "inverse_of" || metaOperatorName == "cant" || metaOperatorName == "unlikely")
            {
                if (concept1.IsMetaConnectedTo(metaOperatorName, concept2, true) != concept2.IsMetaConnectedTo(metaOperatorName, concept1, true))
                    throw new MetaConnectionException("Inconsistant meta connection between operators");
                if (concept1.IsMetaConnectedTo(metaOperatorName, concept2, true))
                    return true;
            }
            else
            {
                if (concept1.IsMetaConnectedTo(metaOperatorName, concept2, true) != concept2.IsMetaConnectedTo(metaOperatorName, concept1, false))
                    throw new MetaConnectionException("Inconsistant meta connection between operators");
                if (concept1.IsMetaConnectedTo(metaOperatorName, concept2, true))
                    return true;
            }
            return false;
        }

        public override bool IsFlatMetaConnected(Concept concept1, string metaOperatorName, Concept concept2)
        {
            HashSet<Concept> verb1FlatListFromMetaConnection = GetVerbFlatListFromMetaConnection(concept1, metaOperatorName,true);
            HashSet<Concept> verb2FlatListFromMetaConnection = GetVerbFlatListFromMetaConnection(concept2, metaOperatorName, false);

            if (verb1FlatListFromMetaConnection.Contains(concept2) != verb2FlatListFromMetaConnection.Contains(concept1))
                throw new MetaConnectionException("Inconsistant flat meta connection between operators");

            if (verb1FlatListFromMetaConnection.Contains(concept2))
                return true;
            else
                return false;
        }

        public override HashSet<Concept> GetIncompatibleVerbList(Concept verb, bool strictMode)
        {
            if (strictMode)
                return GetVerbFlatListFromMetaConnection(verb, "unlikely",true);
            else
                return GetVerbFlatListFromMetaConnection(verb, "cant",true);
        }

        public override HashSet<Concept> GetVerbListFromMetaConnection(Concept verb, string metaOperatorName, bool ConnectionPositivity)
        {
            if (ConnectionPositivity == true)
                return verb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection(metaOperatorName);
            else
                return verb.MetaConnectionTreeNegative.GetAffectedOperatorsByMetaConnection(metaOperatorName);
        }

        public override HashSet<Concept> GetVerbListDependantOn(Concept verb)
        {
            HashSet<Concept> verbIgnoreList = new HashSet<Concept>();
            return GetVerbListDependantOn(verb, verbIgnoreList);
        }

        private HashSet<Concept> GetVerbListDependantOn(Concept verb, HashSet<Concept> verbIgnoreList)
        {
            HashSet<Concept> dependantVerbList = new HashSet<Concept>();

            if (verbIgnoreList.Contains(verb))
                return dependantVerbList;

            verbIgnoreList.Add(verb);

            foreach (string metaOperatorName in LanguageDictionary.MetaOperatorList)
            {
                dependantVerbList.UnionWith(GetVerbListFromMetaConnection(verb, metaOperatorName, true));
                dependantVerbList.UnionWith(GetVerbListFromMetaConnection(verb, metaOperatorName, false));
            }

            HashSet<Concept> newDependantVerbList = new HashSet<Concept>();
            foreach (Concept dependantVerb in dependantVerbList)
                if (!verbIgnoreList.Contains(dependantVerb))
                    newDependantVerbList.UnionWith(GetVerbListDependantOn(dependantVerb, verbIgnoreList));

            dependantVerbList.UnionWith(newDependantVerbList);

            return dependantVerbList;
        }

        public override HashSet<Concept> GetVerbFlatListFromMetaConnection(Concept verb, string metaOperatorName, bool isMetaConnectionPositive)
        {
            return metaConnectionFlattenizer.GetFlatVerbListFromMetaConnection(verb, metaOperatorName, isMetaConnectionPositive);
        }
        #endregion
    }
}