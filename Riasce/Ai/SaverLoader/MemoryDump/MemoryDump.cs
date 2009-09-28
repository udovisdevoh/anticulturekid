using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a saveable representation of ai's memory
    /// </summary>
    [Serializable]
    class MemoryDump
    {
        #region Fields
        /// <summary>
        /// Concept info list
        /// </summary>
        private Dictionary<int, ConceptInfo> conceptInfoList = new Dictionary<int, ConceptInfo>();

        /// <summary>
        /// Rejected metaConnection theory info list
        /// </summary>
        private List<MetaConnectionTheoryInfo> rejectedMetaConnectionTheoryInfoList = new List<MetaConnectionTheoryInfo>();

        /// <summary>
        /// Rejected connection theory info list
        /// </summary>
        private List<ConnectionTheoryInfo> rejectedConnectionTheoryInfoList = new List<ConnectionTheoryInfo>();

        /// <summary>
        /// Ai's name value
        /// </summary>
        private string aiNameValue;

        /// <summary>
        /// Human's name value
        /// </summary>
        private string humanNameValue;

        /// <summary>
        /// Name mapper counter
        /// </summary>
        private int nameMapperCounter;

        /// <summary>
        /// Total theory list
        /// </summary>
        [NonSerialized]
        private TheoryList totalTheoryList;

        /// <summary>
        /// Name mapper to look into
        /// </summary>
        [NonSerialized]
        private NameMapper nameMapper;

        /// <summary>
        /// Memory to look into
        /// </summary>
        [NonSerialized]
        private Memory memory;

        /// <summary>
        /// MetaConnection manager
        /// </summary>
        [NonSerialized]
        private MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
        #endregion

        #region Constants
        /// <summary>
        /// Max theory count to save
        /// </summary>
        private static readonly int maxTheoryCountToSave = 1000;

        /// <summary>
        /// Max argument count by theory to save
        /// </summary>
        private static readonly int maxArgumentCountToSave = 7;
        #endregion

        #region Constructor
        /// <summary>
        /// We use this to create a loadable memoryDump object from xml file
        /// </summary>
        /// <param name="xmlFileName">source xml file name</param>
        public MemoryDump(string xmlFileName)
        {
            XmlTextReader textReader = new XmlTextReader(xmlFileName);

            xmlReadAiHumanNameValueAndNameMapperCounter(textReader, out aiNameValue, out humanNameValue, out nameMapperCounter);
            conceptInfoList = xmlReadConceptInfoList(textReader);
            rejectedConnectionTheoryInfoList = xmlReadRejectedConnectionTheoryList(textReader);
            rejectedMetaConnectionTheoryInfoList = xmlReadRejectedMetaConnectionTheoryList(textReader);

            textReader.Close();
        }

        public MemoryDump(NameMapper nameMapper, Memory memory, RejectedTheories rejectedTheories, HashSet<Concept> totalVerbList, TheoryList totalTheoryList)
        {
            this.nameMapperCounter = nameMapper.Counter;
            this.nameMapper = nameMapper;
            this.memory = memory;
            this.aiNameValue = nameMapper.AiName.Value;
            this.humanNameValue = nameMapper.HumanName.Value;
            this.totalTheoryList = totalTheoryList;

            foreach (Concept concept in memory)
            {
                if (concept.IsConnectedToSomething || Memory.TotalVerbList.Contains(concept) || HasNegativeConnection(concept))
                {
                    int id = memory.GetIdFromConcept(concept);
                    UpdateConceptInfo(id, concept, nameMapper.GetConceptNames(id, false));
                }
            }

            foreach (Theory rejectedTheory in rejectedTheories)
            {
                if (rejectedTheory.GetConcept(0).IsConnectedToSomething && rejectedTheory.GetConcept(1).IsConnectedToSomething)
                {
                    try
                    {
                        int conceptId1, conceptId2, conceptId3;
                        conceptId1 = memory.GetIdFromConcept(rejectedTheory.GetConcept(0));
                        conceptId2 = memory.GetIdFromConcept(rejectedTheory.GetConcept(1));
                        if (rejectedTheory.MetaOperatorName == null)
                        {
                            conceptId3 = memory.GetIdFromConcept(rejectedTheory.GetConcept(2));
                            InsertRejectedTheory(conceptId1, conceptId2, conceptId3);
                        }
                        else
                        {
                            InsertRejectedTheory(conceptId1, rejectedTheory.MetaOperatorName, conceptId2);
                        }
                    }
                    catch (MemoryException)
                    {
                        //Just skip theory if concept doesn't exist anymore
                    }
                }
            }
        }
        #endregion

        #region XML saving methods
        /// <summary>
        /// Write memory dump to xml file
        /// </summary>
        /// <param name="xmlFileName">xml file name</param>
        public void SaveXmlFile(string xmlFileName)
        {
            XmlTextWriter textWriter = new XmlTextWriter(xmlFileName, Encoding.UTF8);

            textWriter.Formatting = Formatting.Indented;
            textWriter.Indentation = 4;
            textWriter.WriteStartDocument();

            #region We write the ai element
            textWriter.WriteStartElement("ai");
            textWriter.WriteAttributeString("aiNameValue", aiNameValue);
            textWriter.WriteAttributeString("humanNameValue", humanNameValue);
            textWriter.WriteAttributeString("nameMapperCounter", nameMapperCounter.ToString());

            if (conceptInfoList != null)
                xmlWriteConceptInfoList(textWriter, conceptInfoList);

            if (rejectedConnectionTheoryInfoList != null || rejectedMetaConnectionTheoryInfoList != null)
                xmlWriteRejectedTheoryList(textWriter, rejectedConnectionTheoryInfoList, rejectedMetaConnectionTheoryInfoList);

            //xmlWriteTheoryList(textWriter, totalTheoryList);

            textWriter.WriteEndElement();
            #endregion

            textWriter.WriteEndDocument();
            textWriter.Flush();
            textWriter.Close();
        }

        private void xmlWriteRejectedTheoryList(XmlTextWriter textWriter, List<ConnectionTheoryInfo> rejectedConnectionTheoryList, List<MetaConnectionTheoryInfo> rejectedMetaConnectionTheoryList)
        {
            textWriter.WriteStartElement("rejectedTheoryList");

            if (rejectedConnectionTheoryList != null)
                xmlWriteRejectedConnectionTheory(textWriter, rejectedConnectionTheoryList);

            if (rejectedMetaConnectionTheoryList != null)
                xmlWriteRejectedMetaConnectionTheory(textWriter, rejectedMetaConnectionTheoryList);

            textWriter.WriteEndElement();
        }

        private void xmlWriteRejectedConnectionTheory(XmlTextWriter textWriter, List<ConnectionTheoryInfo> rejectedConnectionTheoryList)
        {
            textWriter.WriteStartElement("rejectedConnectionTheoryList");

            foreach (ConnectionTheoryInfo theory in rejectedConnectionTheoryList)
            {
                textWriter.WriteStartElement("rejectedConnectionTheory");
                xmlWriteConceptIdWithCommentedName(textWriter, theory.ConceptId1, "subject");
                xmlWriteConceptIdWithCommentedName(textWriter, theory.ConceptId2, "verb");
                xmlWriteConceptIdWithCommentedName(textWriter, theory.ConceptId3, "complement");
                textWriter.WriteEndElement();
            }

            textWriter.WriteEndElement();
        }

        private void xmlWriteRejectedMetaConnectionTheory(XmlTextWriter textWriter, List<MetaConnectionTheoryInfo> rejectedMetaConnectionTheoryList)
        {
            textWriter.WriteStartElement("rejectedMetaConnectionTheoryList");

            foreach (MetaConnectionTheoryInfo theory in rejectedMetaConnectionTheoryList)
            {
                textWriter.WriteStartElement("rejectedMetaConnectionTheory");
                xmlWriteConceptIdWithCommentedName(textWriter, theory.ConceptId1, "operatorId1");

                textWriter.WriteStartElement("metaOperator");
                textWriter.WriteValue(theory.MetaOperatorName);
                textWriter.WriteEndElement();

                xmlWriteConceptIdWithCommentedName(textWriter, theory.ConceptId2, "operatorId2");
                textWriter.WriteEndElement();
            }

            textWriter.WriteEndElement();
        }

        private void xmlWriteConceptInfoList(XmlTextWriter textWriter, Dictionary<int, ConceptInfo> conceptInfoList)
        {
            textWriter.WriteStartElement("conceptList", null);
            int conceptId;
            ConceptInfo conceptInfo;
            foreach (KeyValuePair<int, ConceptInfo> conceptInfoAndId in conceptInfoList)
            {
                conceptId = conceptInfoAndId.Key;
                conceptInfo = conceptInfoAndId.Value;
                xmlWriteConceptInfo(textWriter, conceptId, conceptInfo);
            }
            textWriter.WriteEndElement();
        }

        private void xmlWriteConceptInfo(XmlTextWriter textWriter, int conceptId, ConceptInfo conceptInfo)
        {
            textWriter.WriteStartElement("concept");

            textWriter.WriteAttributeString("id", conceptId.ToString());

            xmlWriteConceptNameAsInlineComment(textWriter, conceptId);

            if (conceptInfo.NameList != null)
                xmlWriteConceptNameList(textWriter, conceptInfo.NameList);

            if (conceptInfo.ConnectionInfo != null)
                xmlWriteConceptConnectionList(textWriter, conceptInfo.ConnectionInfo);

            if (conceptInfo.PositiveMetaConnectionList != null || conceptInfo.NegativeMetaConnectionList != null)
                xmlWriteConceptMetaConnectionList(textWriter, conceptInfo.PositiveMetaConnectionList, conceptInfo.NegativeMetaConnectionList);

            if (conceptInfo.ImplyConditionInfoListPositive != null || conceptInfo.ImplyConditionInfoListNegative != null)
                xmlWriteConceptImplyConditionList(textWriter, conceptInfo.ImplyConditionInfoListPositive, conceptInfo.ImplyConditionInfoListNegative);

            textWriter.WriteEndElement();
        }

        private void xmlWriteConceptImplyConditionList(XmlTextWriter textWriter, Dictionary<int, HashSet<ConditionInfo>> implyConditionInfoListPositive, Dictionary<int, HashSet<ConditionInfo>> implyConditionInfoListNegative)
        {
            textWriter.WriteStartElement("implyConditionList");

            if (implyConditionInfoListPositive != null)
                xmlWriteCondeptImplyConditionPositiveOrNegative(textWriter, implyConditionInfoListPositive, "positiveImplyConditionList");

            if (implyConditionInfoListNegative != null)
                xmlWriteCondeptImplyConditionPositiveOrNegative(textWriter, implyConditionInfoListNegative, "negativeImplyConditionList");

            textWriter.WriteEndElement();
        }

        private void xmlWriteCondeptImplyConditionPositiveOrNegative(XmlTextWriter textWriter, Dictionary<int, HashSet<ConditionInfo>> implyConditionInfoList, string xmlElementName)
        {
            textWriter.WriteStartElement(xmlElementName);

            int complementId;
            HashSet<ConditionInfo> conditionList;
            foreach (KeyValuePair<int, HashSet<ConditionInfo>> complementIdAndConditionList in implyConditionInfoList)
            {
                complementId = complementIdAndConditionList.Key;
                conditionList = complementIdAndConditionList.Value;

                xmlWriteImplyConditionBranch(textWriter, complementId, conditionList);
            }

            textWriter.WriteEndElement();
        }

        private void xmlWriteImplyConditionBranch(XmlTextWriter textWriter, int complementId, HashSet<ConditionInfo> conditionList)
        {
            textWriter.WriteStartElement("branch");
            textWriter.WriteAttributeString("complementId", complementId.ToString());
            xmlWriteConceptNameAsInlineComment(textWriter, complementId);

            foreach (ConditionInfo conditionInfo in conditionList)
                xmlWriteConditionInfo(textWriter, conditionInfo);

            textWriter.WriteEndElement();
        }

        private void xmlWriteConditionInfo(XmlTextWriter textWriter, ConditionInfo conditionInfo)
        {
            textWriter.WriteStartElement("condition");

            #region Write logicOperator
            if (conditionInfo.LogicOperator != null)
            {
                textWriter.WriteStartElement("logicOperator");
                textWriter.WriteAttributeString("name", conditionInfo.LogicOperator);
                textWriter.WriteEndElement();
            }
            #endregion

            xmlWriteBoolValue(textWriter, conditionInfo.IsPositive, "isPositive");
            xmlWriteBoolValue(textWriter, conditionInfo.IsActionVerbExist, "isActionVerbExist");
            xmlWriteBoolValue(textWriter, conditionInfo.IsActionComplementExist, "isActionComplementExist");

            xmlWriteConceptIdWithCommentedName(textWriter, conditionInfo.VerbId, "verbId");
            xmlWriteConceptIdWithCommentedName(textWriter, conditionInfo.ComplementId, "complementId");

            if (conditionInfo.IsActionVerbExist)
                xmlWriteConceptIdWithCommentedName(textWriter, conditionInfo.ActionVerbId, "actionVerbId");

            if (conditionInfo.IsActionComplementExist)
                xmlWriteConceptIdWithCommentedName(textWriter, conditionInfo.ActionComplementId, "actionComplementId");

            if (conditionInfo.LeftChild != null && conditionInfo.RightChild != null)
            {
                textWriter.WriteStartElement("leftChild");
                xmlWriteConditionInfo(textWriter, conditionInfo.LeftChild);
                textWriter.WriteEndElement();

                textWriter.WriteStartElement("rightChild");
                xmlWriteConditionInfo(textWriter, conditionInfo.RightChild);
                textWriter.WriteEndElement();
            }

            textWriter.WriteEndElement();
        }

        private void xmlWriteConceptIdWithCommentedName(XmlTextWriter textWriter, int conceptId, string elementName)
        {
            textWriter.WriteStartElement(elementName);
            textWriter.WriteValue(conceptId);
            textWriter.WriteEndElement();
            xmlWriteConceptNameAsInlineComment(textWriter, conceptId);
        }

        private void xmlWriteBoolValue(XmlTextWriter textWriter, bool boolValue, string elementName)
        {
            textWriter.WriteStartElement(elementName);
            textWriter.WriteValue(boolValue);
            textWriter.WriteEndElement();
        }

        private void xmlWriteConceptMetaConnectionList(XmlTextWriter textWriter, Dictionary<string, HashSet<int>> positiveMetaConnectionList, Dictionary<string, HashSet<int>> negativeMetaConnectionList)
        {
            textWriter.WriteStartElement("metaConnectionList");

            xmlWriteConceptMetaConnectionPositiveOrNegative(textWriter, positiveMetaConnectionList, "positiveMetaConnectionList");
            xmlWriteConceptMetaConnectionPositiveOrNegative(textWriter, negativeMetaConnectionList, "negativeMetaConnectionList");

            textWriter.WriteEndElement();
        }

        private void xmlWriteConceptMetaConnectionPositiveOrNegative(XmlTextWriter textWriter, Dictionary<string, HashSet<int>> connectionList, string xmlElementName)
        {
            textWriter.WriteStartElement(xmlElementName);

            if (connectionList != null)
            {
                string metaOperatorName;
                HashSet<int> otherOperatorIdList;
                foreach (KeyValuePair<string, HashSet<int>> metaOperatorNameAndOtherOperatorIdList in connectionList)
                {
                    metaOperatorName = metaOperatorNameAndOtherOperatorIdList.Key;
                    otherOperatorIdList = metaOperatorNameAndOtherOperatorIdList.Value;

                    textWriter.WriteStartElement("metaConnection");
                    textWriter.WriteAttributeString("metaOperator", metaOperatorName);

                    foreach (int otherOperatorId in otherOperatorIdList)
                    {
                        textWriter.WriteStartElement("operator");
                        textWriter.WriteAttributeString("id", otherOperatorId.ToString());
                        textWriter.WriteEndElement();
                        xmlWriteConceptNameAsInlineComment(textWriter, otherOperatorId);
                    }

                    textWriter.WriteEndElement();
                }
            }

            textWriter.WriteEndElement();
        }

        private void xmlWriteConceptNameAsInlineComment(XmlTextWriter textWriter, int conceptId)
        {
            string name = nameMapper.GetConceptNames(conceptId)[0];
            textWriter.Formatting = Formatting.None;
            textWriter.WriteComment(name);
            textWriter.Formatting = Formatting.Indented;
        }

        private void xmlWriteConceptConnectionList(XmlTextWriter textWriter, Dictionary<int, HashSet<int>> connectionInfoList)
        {
            textWriter.WriteStartElement("connectionList");

            int verbId;
            HashSet<int> complementIdList;
            foreach (KeyValuePair<int, HashSet<int>> verbIdAndComplementIdList in connectionInfoList)
            {
                verbId = verbIdAndComplementIdList.Key;
                complementIdList = verbIdAndComplementIdList.Value;

                if (complementIdList != null)
                    xmlWriteConnectionBranch(textWriter, verbId, complementIdList);
            }

            textWriter.WriteEndElement();
        }

        private void xmlWriteConnectionBranch(XmlTextWriter textWriter, int verbId, HashSet<int> complementIdList)
        {
            textWriter.WriteStartElement("verb");
            textWriter.WriteAttributeString("id", verbId.ToString());
            xmlWriteConceptNameAsInlineComment(textWriter, verbId);

            foreach (int complementId in complementIdList)
            {
                textWriter.WriteStartElement("complement");
                textWriter.WriteAttributeString("id", complementId.ToString());
                textWriter.WriteEndElement();
                xmlWriteConceptNameAsInlineComment(textWriter, complementId);
            }

            textWriter.WriteEndElement();
        }

        private void xmlWriteConceptNameList(XmlTextWriter textWriter, HashSet<string> nameList)
        {
            textWriter.WriteStartElement("nameList");

            foreach (string name in nameList)
            {
                textWriter.WriteStartElement("name");
                textWriter.WriteValue(name);
                textWriter.WriteEndElement();
            }

            textWriter.WriteEndElement();
        }

        private void xmlWriteTheoryList(XmlTextWriter textWriter, IEnumerable<Theory> theoryList)
        {
            textWriter.WriteStartElement("theoryList");

            int counter = 0;
            foreach (Theory theory in theoryList)
            {
                xmlWriteTheory(textWriter, theory);
                if (counter > maxTheoryCountToSave)
                    break;
                counter++;
            }

            textWriter.WriteEndElement();
        }

        private void xmlWriteTheory(XmlTextWriter textWriter, Theory theory)
        {
            textWriter.WriteStartElement("theory");

            textWriter.WriteAttributeString("predictedProbability", theory.PredictedProbability.ToString());

            if (theory.MetaOperatorName == null)
            {
                xmlWriteConceptIdWithCommentedName(textWriter, memory.GetIdFromConcept(theory.GetConcept(0)), "subject");
                xmlWriteConceptIdWithCommentedName(textWriter, memory.GetIdFromConcept(theory.GetConcept(1)), "verb");
                xmlWriteConceptIdWithCommentedName(textWriter, memory.GetIdFromConcept(theory.GetConcept(2)), "complement");
            }
            else
            {
                xmlWriteConceptIdWithCommentedName(textWriter, memory.GetIdFromConcept(theory.GetConcept(0)), "operator1");

                textWriter.WriteStartElement("metaOperator");
                textWriter.WriteValue(theory.MetaOperatorName);
                textWriter.WriteEndElement();

                xmlWriteConceptIdWithCommentedName(textWriter, memory.GetIdFromConcept(theory.GetConcept(1)), "operator2");
            }

            xmlWriteTheoryConnectionArgumentList(textWriter, theory);
            xmlWriteTheoryMetaConnectionArgumentList(textWriter, theory);

            textWriter.WriteEndElement();
        }

        private void xmlWriteTheoryConnectionArgumentList(XmlTextWriter textWriter, Theory theory)
        {
            textWriter.WriteStartElement("connectionArgumentList");
            List<Concept> argument;
            int counter = 0;
            for (int i = 0; i < theory.CountConnectionArgument && counter < maxArgumentCountToSave; i++)
            {
                argument = theory.GetConnectionArgumentAt(i);

                if (argument[0].GetOptimizedConnectionBranch(argument[1]).ComplementConceptList.Contains(argument[2]))
                {
                    xmlWriteTheoryConnectionArgument(textWriter, argument);
                    counter++;
                }
            }

            textWriter.WriteEndElement();
        }

        private void xmlWriteTheoryMetaConnectionArgumentList(XmlTextWriter textWriter, Theory theory)
        {
            textWriter.WriteStartElement("metaConnectionArgumentList");

            for (int i = 0; i < theory.CountMetaConnectionArgument && i < maxArgumentCountToSave; i++)
                xmlWriteTheoryMetaConnectionArgument(textWriter, theory.GetMetaConnectionArgumentAt(i));

            textWriter.WriteEndElement();
        }

        private void xmlWriteTheoryConnectionArgument(XmlTextWriter textWriter, List<Concept> argument)
        {
            textWriter.WriteStartElement("connectionArgument");

            xmlWriteConceptIdWithCommentedName(textWriter, memory.GetIdFromConcept(argument[0]), "subject");
            xmlWriteConceptIdWithCommentedName(textWriter, memory.GetIdFromConcept(argument[1]), "verb");
            xmlWriteConceptIdWithCommentedName(textWriter, memory.GetIdFromConcept(argument[2]), "complement");

            textWriter.WriteEndElement();
        }

        private void xmlWriteTheoryMetaConnectionArgument(XmlTextWriter textWriter, MetaConnectionArgument metaConnectionArgument)
        {
            textWriter.WriteStartElement("metaConnectionArgument");

            xmlWriteConceptIdWithCommentedName(textWriter, memory.GetIdFromConcept(metaConnectionArgument.Operator1), "operator1");

            textWriter.WriteStartElement("metaOperator");
            textWriter.WriteValue(metaConnectionArgument.MetaOperator);
            textWriter.WriteEndElement();

            xmlWriteConceptIdWithCommentedName(textWriter, memory.GetIdFromConcept(metaConnectionArgument.Operator2), "operator1");

            textWriter.WriteEndElement();
        }
        #endregion

        #region XML loading methods
        private void xmlReadAiHumanNameValueAndNameMapperCounter(XmlTextReader textReader, out string aiNameValue, out string humanNameValue, out int nameMapperCounter)
        {
            nameMapperCounter = -1;
            aiNameValue = null;
            humanNameValue = null;

            while (textReader.Read())
            {
                if (textReader.NodeType == XmlNodeType.Element && textReader.Name=="ai")
                {
                    aiNameValue = textReader.GetAttribute("aiNameValue");
                    humanNameValue = textReader.GetAttribute("humanNameValue");
                    int.TryParse(textReader.GetAttribute("nameMapperCounter"), out nameMapperCounter);

                    break;
                }
            }
        }

        private Dictionary<int, ConceptInfo> xmlReadConceptInfoList(XmlTextReader textReader)
        {
            Dictionary<int, ConceptInfo> conceptInfoList = new Dictionary<int, ConceptInfo>();
            int currentConceptId;
            int currentVerbId = -1;
            int currentComplementId = -1;
            string latestElementName = null;
            string currentMetaOperator = null;
            ConceptInfo currentConceptInfo = null;
            bool readingPositiveMetaConnectionList = true;
            bool readingPositiveImplyConditionList = true;
            int currentFarVerbId = -1;
            int implyActionComplementId = -1;
            ConditionInfo currentConditionInfo = null;
            int childMode = 0; //0: not a child, 1: leftChild, 2: rightChild
            Stack<int> childModeStack = new Stack<int>();
            Stack<ConditionInfo> parentConditionStack = new Stack<ConditionInfo>();

            while (textReader.Read() && (textReader.NodeType != XmlNodeType.EndElement || textReader.Name != "conceptList"))
            {
                if (textReader.NodeType == XmlNodeType.Element && textReader.Name == "concept")
                {
                    int.TryParse(textReader.GetAttribute("id"), out currentConceptId);

                    currentConceptInfo = new ConceptInfo();
                    currentConceptInfo.Id = currentConceptId;

                    while (textReader.Read() && (textReader.NodeType != XmlNodeType.EndElement || textReader.Name != "concept"))
                    {
                        if (textReader.NodeType == XmlNodeType.Element)
                        {
                            latestElementName = textReader.Name;

                            if (textReader.Name == "verb")
                            {
                                int.TryParse(textReader.GetAttribute("id"), out currentVerbId);
                            }
                            else if (textReader.Name == "complement")
                            {
                                int.TryParse(textReader.GetAttribute("id"), out currentComplementId);
                                currentConceptInfo.AddConnectionInfo(currentVerbId, currentComplementId);
                            }
                            else if (textReader.Name == "positiveMetaConnectionList")
                            {
                                readingPositiveMetaConnectionList = true;
                            }
                            else if (textReader.Name == "negativeMetaConnectionList")
                            {
                                readingPositiveMetaConnectionList = false;
                            }
                            else if (textReader.Name == "metaConnection")
                            {
                                currentMetaOperator = textReader.GetAttribute("metaOperator");
                            }
                            else if (textReader.Name == "operator" && currentMetaOperator != null)
                            {
                                int.TryParse(textReader.GetAttribute("id"), out currentFarVerbId);
                                currentConceptInfo.AddMetaConnectionInfo(currentMetaOperator, currentFarVerbId, readingPositiveMetaConnectionList);
                            }
                            else if (textReader.Name == "condition")
                            {
                                ConditionInfo parentConditionInfo = null;

                                if (childMode == 1 || childMode == 2)
                                {
                                    parentConditionInfo = currentConditionInfo;
                                    parentConditionStack.Push(parentConditionInfo);
                                }

                                currentConditionInfo = new ConditionInfo();
                                currentConditionInfo.IsPositive = readingPositiveImplyConditionList;
                                currentConditionInfo.ActionComplementId = implyActionComplementId;

                                if (childMode == 1)
                                {
                                    parentConditionInfo.LeftChild = currentConditionInfo;
                                }
                                else if (childMode == 2)
                                {
                                    parentConditionInfo.RightChild = currentConditionInfo;
                                }
                            }
                            else if (textReader.Name == "logicOperator")
                            {
                                currentConditionInfo.LogicOperator = textReader.GetAttribute("name");
                            }
                            else if (textReader.Name == "positiveImplyConditionList")
                            {
                                readingPositiveImplyConditionList = true;
                            }
                            else if (textReader.Name == "negativeImplyConditionList")
                            {
                                readingPositiveImplyConditionList = false;
                            }
                            else if (textReader.Name == "branch")
                            {
                                int.TryParse(textReader.GetAttribute("complementId"), out implyActionComplementId);
                                childMode = 0;
                            }
                            else if (textReader.Name == "leftChild")
                            {
                                childModeStack.Push(childMode);
                                childMode = 1;
                            }
                            else if (textReader.Name == "rightChild")
                            {
                                childModeStack.Push(childMode);
                                childMode = 2;
                            }
                        }
                        else if (textReader.NodeType == XmlNodeType.EndElement)
                        {
                            if (textReader.Name == "leftChild" || textReader.Name == "rightChild")
                            {
                                if (childModeStack.Count == 0)
                                {
                                    childMode = 0;
                                }
                                else
                                {
                                    childMode = childModeStack.Pop();
                                }
                            }
                            else if (textReader.Name == "condition")
                            {
                                if (childMode == 0)
                                {
                                    HashSet<ConditionInfo> branch;

                                    if (currentConditionInfo.IsPositive)
                                    {
                                        if (!currentConceptInfo.ImplyConditionInfoListPositive.TryGetValue(currentConditionInfo.ActionComplementId, out branch))
                                        {
                                            branch = new HashSet<ConditionInfo>();
                                            currentConceptInfo.ImplyConditionInfoListPositive.Add(currentConditionInfo.ActionComplementId, branch);
                                        }
                                    }
                                    else
                                    {
                                        if (!currentConceptInfo.ImplyConditionInfoListNegative.TryGetValue(currentConditionInfo.ActionComplementId, out branch))
                                        {
                                            branch = new HashSet<ConditionInfo>();
                                            currentConceptInfo.ImplyConditionInfoListNegative.Add(currentConditionInfo.ActionComplementId, branch);
                                        }
                                    }
                                    branch.Add(currentConditionInfo);
                                }
                                else if (childMode == 1 || childMode == 2)
                                {
                                    currentConditionInfo = parentConditionStack.Pop();
                                }
                            }
                        }
                        else if (textReader.NodeType == XmlNodeType.Text)
                        {
                            if (latestElementName == "name")
                            {
                                currentConceptInfo.NameList.Add(textReader.Value);
                            }
                            else if (latestElementName == "isActionVerbExist")
                            {
                                bool implyIsActionVerbExist;
                                bool.TryParse(textReader.Value, out implyIsActionVerbExist);
                                currentConditionInfo.IsActionVerbExist = implyIsActionVerbExist;
                            }
                            else if (latestElementName == "isActionComplementExist")
                            {
                                bool implyIsActionComplementExist;
                                bool.TryParse(textReader.Value, out implyIsActionComplementExist);
                                currentConditionInfo.IsActionComplementExist = implyIsActionComplementExist;
                            }
                            else if (latestElementName == "verbId")
                            {
                                int verbId;
                                int.TryParse(textReader.Value, out verbId);
                                currentConditionInfo.VerbId = verbId;
                            }
                            else if (latestElementName == "complementId")
                            {
                                int complementId;
                                int.TryParse(textReader.Value, out complementId);
                                currentConditionInfo.ComplementId = complementId;
                            }
                            else if (latestElementName == "actionVerbId")
                            {
                                int actionVerbId;
                                int.TryParse(textReader.Value, out actionVerbId);
                                currentConditionInfo.ActionVerbId = actionVerbId;
                            }
                        }
                    }

                    conceptInfoList.Add(currentConceptId, currentConceptInfo);
                }
            }

            return conceptInfoList;
        }

        private List<ConnectionTheoryInfo> xmlReadRejectedConnectionTheoryList(XmlTextReader textReader)
        {
            List<ConnectionTheoryInfo> rejectedTheoryList = new List<ConnectionTheoryInfo>();
            ConnectionTheoryInfo rejectedTheory = null;
            int currentConceptId;
            string lastElementName = null;
            while (textReader.Read() && (textReader.NodeType != XmlNodeType.EndElement || textReader.Name != "rejectedConnectionTheoryList"))
            {
                if (textReader.NodeType == XmlNodeType.Element && textReader.Name == "rejectedConnectionTheory")
                {
                    if (rejectedTheory != null)
                        rejectedTheoryList.Add(rejectedTheory);

                    rejectedTheory = new ConnectionTheoryInfo();
                }
                else if (rejectedTheory != null && textReader.NodeType == XmlNodeType.Element)
                {
                    lastElementName = textReader.Name;
                }
                else if (rejectedTheory != null && textReader.NodeType == XmlNodeType.Text && lastElementName == "subject")
                {
                    int.TryParse(textReader.Value, out currentConceptId);
                    rejectedTheory.ConceptId1 = currentConceptId;
                }
                else if (rejectedTheory != null && textReader.NodeType == XmlNodeType.Text && lastElementName == "verb")
                {
                    int.TryParse(textReader.Value, out currentConceptId);
                    rejectedTheory.ConceptId2 = currentConceptId;
                }
                else if (rejectedTheory != null && textReader.NodeType == XmlNodeType.Text && lastElementName == "complement")
                {
                    int.TryParse(textReader.Value, out currentConceptId);
                    rejectedTheory.ConceptId3 = currentConceptId;
                }
            }

            if (rejectedTheory != null)
                rejectedTheoryList.Add(rejectedTheory);

            return rejectedTheoryList;
        }

        private List<MetaConnectionTheoryInfo> xmlReadRejectedMetaConnectionTheoryList(XmlTextReader textReader)
        {
            List<MetaConnectionTheoryInfo> rejectedTheoryList = new List<MetaConnectionTheoryInfo>();
            MetaConnectionTheoryInfo rejectedTheory = null;
            int currentConceptId;
            string lastElementName = null;

            while (textReader.Read() && (textReader.NodeType != XmlNodeType.EndElement || textReader.Name != "rejectedMetaConnectionTheoryList"))
            {
                if (textReader.NodeType == XmlNodeType.Element && textReader.Name == "rejectedMetaConnectionTheory")
                {
                    if (rejectedTheory != null)
                        rejectedTheoryList.Add(rejectedTheory);

                    rejectedTheory = new MetaConnectionTheoryInfo();
                }
                else if (rejectedTheory != null && textReader.NodeType == XmlNodeType.Element)
                {
                    lastElementName = textReader.Name;
                }
                else if (rejectedTheory != null && textReader.NodeType == XmlNodeType.Text && lastElementName == "operatorId1")
                {
                    int.TryParse(textReader.Value, out currentConceptId);
                    rejectedTheory.ConceptId1 = currentConceptId;
                }

                else if (rejectedTheory != null && textReader.NodeType == XmlNodeType.Text && lastElementName == "metaOperator")
                {
                    rejectedTheory.MetaOperatorName = textReader.Value;
                }

                else if (rejectedTheory != null && textReader.NodeType == XmlNodeType.Text && lastElementName == "operatorId2")
                {
                    int.TryParse(textReader.Value, out currentConceptId);
                    rejectedTheory.ConceptId2 = currentConceptId;
                }
            }

            if (rejectedTheory != null)
                rejectedTheoryList.Add(rejectedTheory);

            return rejectedTheoryList;
        }
        #endregion

        #region Private Saving methods
        private void UpdateConceptInfo(int conceptId, Concept concept, List<string> nameList)
        {
            ConceptInfo conceptInfo;
            if (!conceptInfoList.TryGetValue(conceptId, out conceptInfo))
            {
                conceptInfo = new ConceptInfo();
                conceptInfoList.Add(conceptId, conceptInfo);
            }
            conceptInfo.Id = conceptId;
            AssimilateConnections(conceptInfo, concept);
            AssimilateMetaConnections(conceptInfo, concept);
            AssimilateImplyConnections(conceptInfo, concept);
            conceptInfo.AddNameRange(nameList);
        }

        private void InsertRejectedTheory(int conceptId1, string metaOperatorName, int conceptId2)
        {
            rejectedMetaConnectionTheoryInfoList.Add(new MetaConnectionTheoryInfo(conceptId1, metaOperatorName, conceptId2));
        }

        private void InsertRejectedTheory(int conceptId1, int conceptId2, int conceptId3)
        {
            rejectedConnectionTheoryInfoList.Add(new ConnectionTheoryInfo(conceptId1, conceptId2, conceptId3));
        }

        private void AssimilateConnections(ConceptInfo conceptInfo, Concept concept)
        {
            Concept verb;
            ConnectionBranch optimizedBranch;
            int verbId;
            int complementId;
            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in concept.OptimizedConnectionBranchList)
            {
                verb = verbAndBranch.Key;
                optimizedBranch = verbAndBranch.Value;
                verbId = memory.GetIdFromConcept(verb);

                foreach (Concept complement in optimizedBranch.ComplementConceptList)
                {
                    complementId = memory.GetIdFromConcept(complement);
                    conceptInfo.AddConnectionInfo(verbId, complementId);
                }
            }
        }

        private void AssimilateMetaConnections(ConceptInfo conceptInfo, Concept concept)
        {
            foreach (string metaOperatorName in LanguageDictionary.MetaOperatorList)
            {
                HashSet<Concept> farVerbListPositiveConnection = metaConnectionManager.GetVerbListFromMetaConnection(concept, metaOperatorName, true);
                HashSet<Concept> farVerbListNegativeConnection = metaConnectionManager.GetVerbListFromMetaConnection(concept, metaOperatorName, false);
                int farVerbId;

                foreach (Concept farVerb in farVerbListPositiveConnection)
                {
                    farVerbId = memory.GetIdFromConcept(farVerb);
                    conceptInfo.AddMetaConnectionInfo(metaOperatorName, farVerbId, true);
                }

                foreach (Concept farVerb in farVerbListNegativeConnection)
                {
                    farVerbId = memory.GetIdFromConcept(farVerb);
                    conceptInfo.AddMetaConnectionInfo(metaOperatorName, farVerbId, false);
                }
            }
        }

        private void AssimilateImplyConnections(ConceptInfo conceptInfo, Concept concept)
        {
            Concept complement;
            HashSet<Condition> conditionList;
            ConditionInfo conditionInfo;

            foreach (KeyValuePair<Concept, HashSet<Condition>> complementAndConditionList in concept.ImplyConnectionTreePositive)
            {
                complement = complementAndConditionList.Key;
                conditionList = complementAndConditionList.Value;

                foreach (Condition condition in conditionList)
                {
                    conditionInfo = condition.GetSerializableInfo(memory);
                    conditionInfo.ActionComplementId = memory.GetIdFromConcept(complement);
                    conditionInfo.ActionVerbId = memory.GetIdFromConcept(concept);
                    conceptInfo.AddImplyConditionInfo(memory.GetIdFromConcept(complement), conditionInfo, true);
                }
            }

            foreach (KeyValuePair<Concept, HashSet<Condition>> complementAndConditionList in concept.ImplyConnectionTreeNegative)
            {
                complement = complementAndConditionList.Key;
                conditionList = complementAndConditionList.Value;

                foreach (Condition condition in conditionList)
                {
                    conditionInfo = condition.GetSerializableInfo(memory);
                    conditionInfo.ActionComplementId = memory.GetIdFromConcept(complement);
                    conditionInfo.ActionVerbId = memory.GetIdFromConcept(concept);
                    conceptInfo.AddImplyConditionInfo(memory.GetIdFromConcept(complement), conditionInfo, false);
                }
            }
        }

        private bool HasNegativeConnection(Concept concept)
        {
            FeelingMonitor.Add(FeelingMonitor.SINGLE_SIDE_CONNECTION);

            foreach (Concept farConcept in memory)
                foreach (ConnectionBranch optimizedBranch in farConcept.OptimizedConnectionBranchList.Values)
                    if (optimizedBranch.ComplementConceptList.Contains(concept))
                        return true;

            return false;
        }
        #endregion

        #region Public Loading methods
        /// <summary>
        /// Get name mapper
        /// </summary>
        /// <returns>name mapper</returns>
        public NameMapper GetNameMapper()
        {
            NameMapper nameMapper = new NameMapper(new Name(this.aiNameValue), new Name(this.humanNameValue), nameMapperCounter);

            foreach (ConceptInfo conceptInfo in conceptInfoList.Values)
                nameMapper.AssimilateConceptInfo(conceptInfo);

            return nameMapper;
        }

        /// <summary>
        /// Get Memory
        /// </summary>
        /// <returns>Memory</returns>
        public Memory GetMemory()
        {
            Memory memory = new Memory();

            Concept subject;
            Concept verb;
            Concept complement;

            Concept operator1;
            Concept operator2;
            HashSet<int> farVerbIdList;
            string metaOperatorName;

            foreach (ConceptInfo conceptInfo in conceptInfoList.Values)
            {
                #region We regenerate connections
                subject = memory.GetOrCreateConcept(conceptInfo.Id);
                int verbId;
                HashSet<int> complementIdList;
                foreach (KeyValuePair<int, HashSet<int>> verbIdAndComplementIdList in conceptInfo.ConnectionInfo)
                {
                    verbId = verbIdAndComplementIdList.Key;
                    complementIdList = verbIdAndComplementIdList.Value;
                    verb = memory.GetOrCreateConcept(verbId);
                    foreach (int complementId in complementIdList)
                    {
                        complement = memory.GetOrCreateConcept(complementId);
                        subject.AddOptimizedConnection(verb, complement);
                    }
                }
                subject.IsFlatDirty = true;
                #endregion

                #region We regenerate positive metaConnections
                foreach (KeyValuePair<string, HashSet<int>> metaOperatorAndFarVerbIdList in conceptInfo.PositiveMetaConnectionList)
                {
                    metaOperatorName = metaOperatorAndFarVerbIdList.Key;
                    farVerbIdList = metaOperatorAndFarVerbIdList.Value;

                    operator1 = memory.GetOrCreateConcept(conceptInfo.Id);

                    foreach (int operator2Id in farVerbIdList)
                    {
                        operator2 = memory.GetOrCreateConcept(operator2Id);
                        operator1.MetaConnectionTreePositive.AddMetaConnection(metaOperatorName, operator2);                       
                    }
                }
                #endregion

                #region We regenerate negative metaConnections
                foreach (KeyValuePair<string, HashSet<int>> metaOperatorAndFarVerbIdList in conceptInfo.NegativeMetaConnectionList)
                {
                    metaOperatorName = metaOperatorAndFarVerbIdList.Key;
                    farVerbIdList = metaOperatorAndFarVerbIdList.Value;

                    operator1 = memory.GetOrCreateConcept(conceptInfo.Id);

                    foreach (int operator2Id in farVerbIdList)
                    {
                        operator2 = memory.GetOrCreateConcept(operator2Id);
                        operator1.MetaConnectionTreeNegative.AddMetaConnection(metaOperatorName, operator2);
                    }
                }
                #endregion

                #region We regenerate positive imply connections
                foreach (KeyValuePair<int, HashSet<ConditionInfo>> complementIdAndConditionInfoList in conceptInfo.ImplyConditionInfoListPositive)
                {
                    int complementId = complementIdAndConditionInfoList.Key;
                    complement = memory.GetOrCreateConcept(complementId);
                    HashSet<ConditionInfo> conditionInfoList = complementIdAndConditionInfoList.Value;

                    foreach (ConditionInfo conditionInfo in conditionInfoList)
                    {
                        Condition condition = conditionInfo.RegenerateRealCondition(memory);

                        HashSet<Concept> concernedConceptList = condition.GetConcernedConceptList();
                        foreach (Concept concernedConcept in concernedConceptList)
                            concernedConcept.AddIndexToImplyCondition(condition);

                        subject.AddImplyConnection(complement, condition, true);
                    }
                }
                #endregion

                #region We regenerate negative imply connections
                foreach (KeyValuePair<int, HashSet<ConditionInfo>> complementIdAndConditionInfoList in conceptInfo.ImplyConditionInfoListNegative)
                {
                    int complementId = complementIdAndConditionInfoList.Key;
                    complement = memory.GetOrCreateConcept(complementId);
                    HashSet<ConditionInfo> conditionInfoList = complementIdAndConditionInfoList.Value;

                    foreach (ConditionInfo conditionInfo in conditionInfoList)
                    {
                        Condition condition = conditionInfo.RegenerateRealCondition(memory);

                        HashSet<Concept> concernedConceptList = condition.GetConcernedConceptList();
                        foreach (Concept concernedConcept in concernedConceptList)
                            concernedConcept.AddIndexToImplyCondition(condition);

                        subject.AddImplyConnection(complement, condition, false);
                    }
                }
                #endregion
            }
            
            return memory;
        }

        /// <summary>
        /// Get total verb list from memory
        /// </summary>
        /// <param name="memory">memory</param>
        /// <returns>total verb list from memory</returns>
        public HashSet<Concept> GetTotalVerbList(Memory memory)
        {
            HashSet<Concept> totalVerbList = new HashSet<Concept>();
            
            foreach (Concept subject in memory)
                foreach (Concept verb in subject.OptimizedConnectionBranchList.Keys)
                    totalVerbList.Add(verb);

            foreach (Concept potentialVerb in memory)
                if (!potentialVerb.MetaConnectionTreePositive.IsEmpty || !potentialVerb.MetaConnectionTreeNegative.IsEmpty)
                    totalVerbList.Add(potentialVerb);

            return totalVerbList;
        }

        /// <summary>
        /// Get rejected theories from memory
        /// </summary>
        /// <param name="memory">memory</param>
        /// <returns>rejected theories from memory</returns>
        public RejectedTheories GetRejectedTheories(Memory memory)
        {
            RejectedTheories rejectedTheories = new RejectedTheories();
            Concept subject, verb, complement, operator1, operator2;
            string metaOperatorName;

            foreach (ConnectionTheoryInfo connectionTheoryInfo in rejectedConnectionTheoryInfoList)
            {
                subject = memory.GetOrCreateConcept(connectionTheoryInfo.ConceptId1);
                verb = memory.GetOrCreateConcept(connectionTheoryInfo.ConceptId2);
                complement = memory.GetOrCreateConcept(connectionTheoryInfo.ConceptId3);
                rejectedTheories.Add(new Theory(0.5, subject, verb, complement));
            }

            foreach (MetaConnectionTheoryInfo metaConnectionTheoryInfo in rejectedMetaConnectionTheoryInfoList)
            {
                operator1 = memory.GetOrCreateConcept(metaConnectionTheoryInfo.ConceptId1);
                operator2 = memory.GetOrCreateConcept(metaConnectionTheoryInfo.ConceptId2);
                metaOperatorName = metaConnectionTheoryInfo.MetaOperatorName;
                rejectedTheories.Add(new Theory(0.5, operator1, metaOperatorName, operator2));
            }

            return rejectedTheories;
        }
        #endregion
    }
}