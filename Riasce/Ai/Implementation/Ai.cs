using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using System.Windows.Documents;
using System.ComponentModel;
using System.Threading;

namespace Riasce
{
    class Ai : AbstractAi
    {
        #region Fields and parts
        private Name aiName = new Name("riasce");

        private Name humanName = new Name("someone");
        
        private Brain brain = new Brain();
        
        private AbstractHumanVisibleCommentBuilder humanVisibleCommentBuilder;

        private AbstractAiVisibleCommentBuilder aiVisibleCommentBuilder;

        private AbstractStatementListFactory statementListFactory;

        private NameMapper nameMapper;

        private MainWindow mainWindow;

        private SaverLoader saverLoader = new SaverLoader();

        private Conversation conversation = null;

        private CategoryListExtractor categoryListExtractor;

        private CategoryListSorterTrimmer categoryListSorterTrimmer = new CategoryListSorterTrimmer();

        private CategoryExtractor categoryExtractor;

        private DefinitionSorter definitionSorter = new DefinitionSorter();

        private WordMatrixExtractor wordMatrixExtractor = new WordMatrixExtractor();

        private PhoneticMatrixManager phoneticMatrixManager = new PhoneticMatrixManager();

        private XmlMatrixSaverLoader xmlMatrixSaverLoader = new XmlMatrixSaverLoader();

        private SemanticLikenessMatrixBuilder semanticLikenessMatrixBuilder = new SemanticLikenessMatrixBuilder();

        private Random random = new Random();

        private string fileName = null;

        private bool fileNeedSave = false;

        private static readonly string defaultFileTypeFilter = "XML Memory files|*.xml|Legacy Memory files|*.AiMemory";
        #endregion

        #region Constructors
        public Ai()
        {
            mainWindow = new MainWindow(brain.DisambiguationNamer);

            humanVisibleCommentBuilder = new XamlHumanVisibleCommentBuilder();
            humanVisibleCommentBuilder.HumanName = humanName.Value;

            aiVisibleCommentBuilder = new XamlAiVisibleCommentBuilder();
            aiVisibleCommentBuilder.AiName = aiName.Value;

            statementListFactory = new StatementListFactory();

            nameMapper = new NameMapper(aiName, humanName);

            mainWindow.RefreshAutoCompleteFrom(brain.GetMemoryToUpdateAutoComplete(), nameMapper);
            mainWindow.SetMemoryTreeViewBinding(brain.GetMemoryToUpdateMemoryTreeView(), nameMapper);

            RefreshWindowTitle();

            //Events
            mainWindow.UserSendComment += UserSendCommentHandler;
            mainWindow.UserWantsToExit += UserWantsToExitHandler;
            mainWindow.UserSignOut += UserSignOutHandler;
            mainWindow.UserRunScript += UserRunScriptHandler;
            mainWindow.UserSaveFile += UserSaveFileHandler;
            mainWindow.UserSaveAsFile += UserSaveAsFileHandler;
            mainWindow.UserOpenFile += UserOpenFileHandler;
            mainWindow.UserViewHelp += UserViewHelpHandler;
            mainWindow.UserFileNew += UserFileNewHandler;
            mainWindow.UserConversationStart += UserConversationStartHandler;
            mainWindow.UserConversationStop += UserConversationStopHandler;
            mainWindow.UserStandardInstinct += UserStandardInstinctHandler;
            mainWindow.UserEgo += UserEgoHandler;
            mainWindow.UserExtractCategoryListFromWiki += UserExtractCategoryListFromWikiHandler;
            mainWindow.UserSortCategoryList += UserSortCategoryListHandler;
            mainWindow.UserOpenCategoryExtractor += UserOpenCategoryExtractorHandler;
            mainWindow.OnVisualizerClickConcept += VisualizerClickConceptHandler;
            mainWindow.OnVisualizerClickWhyLink += VisualizerClickWhyLinkHandler;
            mainWindow.OnVisualizerClickDefineLink += VisualizerClickDefineLinkHandler;
            mainWindow.UserViewEgo += UserViewEgoHandler;
            mainWindow.UserViewStandardInstinct += UserViewStandardInstinctHandler;
            mainWindow.UserOccurenceToSemantic += UserOccurenceToSemanticHandler;
            mainWindow.UserExtractOccurenceMatrix += UserExtractOccurenceMatrixHandler;
            mainWindow.UserExtractPhoneticMatrix += UserExtractPhoneticMatrixHandler;
            humanVisibleCommentBuilder.UserClickConcept += UserClickConceptHandler;
            aiVisibleCommentBuilder.UserClickConcept += UserClickConceptHandler;
        }
        #endregion

        #region Methods
        public override void Start()
        {
            Application application = new Application();
            application.Run(mainWindow);
            brain.StartBackgroundThinker(nameMapper);
        }

        protected override void ChooseDecisionFromStatement(Statement statement)
        {
            Trauma trauma = null;

            #warning Remove comments
            try
            {
                brain.EpisodicRemember(humanName.Value, statement, nameMapper);

                bool preAlterationStatementCorrectness = TestStatementCorrectness(statement);
                if (!preAlterationStatementCorrectness || statement.NamingAssociationOperatorName != null)
                {
                    trauma = TryAlterBrainFromStatement(statement);
                }

                if (trauma != null)
                    AnswerWithTrauma(trauma);
                else
                    AnswerToStatement(statement, preAlterationStatementCorrectness);

                mainWindow.RefreshAutoCompleteFrom(brain.GetMemoryToUpdateAutoComplete(), nameMapper);
                mainWindow.UpdateTreeViewIfNeeded();
                mainWindow.VerbNameListForHelp = nameMapper.GetTotalVerbNameList(brain.GetMemoryToUpdateVerbNameListInHelp());
            }
            catch (Exception e)
            {
                AnswerExceptionMessage(e.Message);
            }
        }

        private bool TestStatementCorrectness(Statement statement)
        {
            if (statement.IsImply)
            {
                if (statement.IsNegative)
                {
                    return !brain.TestImplyConnection(statement.UpdateAction, statement.SelectCondition, nameMapper);
                }
                else
                {
                    return brain.TestImplyConnection(statement.UpdateAction, statement.SelectCondition, nameMapper);
                }
            }
            else if (statement.MetaOperatorName != null)
            {
                bool isMetaConnectionPositive = brain.IsMetaConnected(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                                                                      statement.MetaOperatorName,
                                                                      nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)));

                return statement.IsNegative != isMetaConnectionPositive;
            }
            else if (statement.ConceptCount == 3)
            {
                bool isConnectionPositive = brain.TestConnection(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                                                                 nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)),
                                                                 nameMapper.GetOrCreateConceptId(statement.GetConceptName(2)));

                return statement.IsNegative != isConnectionPositive;
            }
            return true;
        }

        private Trauma TryAlterBrainFromStatement(Statement statement)
        {
            Trauma trauma = null;

            if (statement.IsInterrogative || statement.IsAnalogy || statement.IsStatCross || statement.IsSelect || statement.IsUpdate)
                return null;

            fileNeedSave = true;
            RefreshWindowTitle();

            if (statement.IsImply) //If statement is imply
            {
                if (statement.IsNegative)
                    brain.RemoveImplyConnection(statement.UpdateAction, statement.SelectCondition, nameMapper);
                else
                    brain.AddImplyConnection(statement.UpdateAction, statement.SelectCondition, nameMapper);
            }
            else if (statement.GetConceptName(2) != null) //If statement is operation
            {
                #region If statement is operation
                if (statement.IsNegative)
                {
                    brain.TryRemoveConnection(
                        nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                        nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)),
                        nameMapper.GetOrCreateConceptId(statement.GetConceptName(2))
                        );
                }
                else
                {
                    trauma = brain.TryAddConnection(
                        nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                        nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)),
                        nameMapper.GetOrCreateConceptId(statement.GetConceptName(2))
                        );
                }
                #endregion
            }
            else if (statement.MetaOperatorName != null) //If statement is metaoperation
            {
                #region If statement is metaOperation
                if (statement.IsNegative)
                {
                    brain.TryRemoveMetaConnection(
                        nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                        statement.MetaOperatorName,
                        nameMapper.GetOrCreateConceptId(statement.GetConceptName(1))
                        );
                }
                else
                {
                    trauma = brain.TryAddMetaConnection(
                        nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                        statement.MetaOperatorName,
                        nameMapper.GetOrCreateConceptId(statement.GetConceptName(1))
                        );
                }
                #endregion
            }
            else if (statement.NamingAssociationOperatorName == "aliasof")
            {
                if (statement.GetConceptName(0) == statement.GetConceptName(1))
                    throw new StatementParsingException("Cannot use alias with exact same name. (Press ESC to close autocomplete)");

                if (statement.IsNegative)
                {
                    int conceptId0 = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));
                    int conceptId1 = nameMapper.RemoveAliasAndGetSecondConceptId(statement.GetConceptName(0), statement.GetConceptName(1));
                    brain.RemoveAlias(conceptId0, conceptId1);
                }
                else
                {
                    brain.AddAlias(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)), nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)));
                    nameMapper.AddAlias(statement.GetConceptName(0), statement.GetConceptName(1));
                }
            }
            else if (statement.NamingAssociationOperatorName == "rename")
            {
                if (!statement.IsNegative)
                {
                    nameMapper.Rename(statement.GetConceptName(0), statement.GetConceptName(1));
                }
            }
            return trauma;
        }

        #region Answer to statements
        private void AnswerToStatement(Statement statement, bool preAlterationStatementCorrectness)
        {
            aiVisibleCommentBuilder.AiName = aiName.Value;

            if (statement.NullaryOrUnaryOperatorName == "define")
                AnswerToStatementDefine(statement);
            else if (statement.IsStatCross)
                AnswerToStatementStatCross(statement);
            else if (statement.IsSelect)
                AnswerToStatementSelect(statement);
            else if (statement.IsUpdate)
            {
                AnswerToStatementUpdate(statement);
                return;
            }
            else if (statement.IsImply)
                AnswerToStatementImply(statement, preAlterationStatementCorrectness);
            else if (statement.NullaryOrUnaryOperatorName == "whatis")
                AnswerToStatementWhatis(statement);
            else if (statement.NullaryOrUnaryOperatorName == "teach")
                AnswerToStatementTeach(statement);
            else if (statement.NullaryOrUnaryOperatorName == "teachabout")
                AnswerToStatementTeachAbout(statement);
            else if (statement.NullaryOrUnaryOperatorName == "ask")
                AnswerToStatementAsk(statement);
            else if (statement.NullaryOrUnaryOperatorName == "askabout")
                AnswerToStatementAskAbout(statement);
            else if (statement.NullaryOrUnaryOperatorName == "think")
                AnswerToStatementThink(statement);
            else if (statement.NullaryOrUnaryOperatorName == "linguisthink")
                AnswerToStatementLinguisThink(statement);
            else if (statement.NullaryOrUnaryOperatorName == "phonothink")
                AnswerToStatementPhonoThink(statement);
            else if (statement.NullaryOrUnaryOperatorName == "start_psychosis")
            {
                AnswerToStatementStartPsychosis(statement);
                return;
            }
            else if (statement.NullaryOrUnaryOperatorName == "metathink")
                AnswerToStatementMetaThink(statement);
            else if (statement.NullaryOrUnaryOperatorName == "visualize")
            {
                mainWindow.tabControl.SelectedIndex = 1;
                AnswerToStatementVisualize(statement);
                return;
            }
            else if (statement.NullaryOrUnaryOperatorName == "thinkabout")
                AnswerToStatementThinkAbout(statement);
            else if (statement.NullaryOrUnaryOperatorName == "linguisthinkabout")
                AnswerToStatementLinguisThinkAbout(statement);
            else if (statement.NullaryOrUnaryOperatorName == "phonothinkabout")
                AnswerToStatementPhonoThinkAbout(statement);
            else if (statement.NullaryOrUnaryOperatorName == "metathinkabout")
                AnswerToStatementMetaThinkAbout(statement);
            else if (statement.NullaryOrUnaryOperatorName == "no")
                AnswerToStatementNo(statement);
            else if (statement.NullaryOrUnaryOperatorName == "yes")
                AnswerToStatementYes(statement);
            else if (statement.NullaryOrUnaryOperatorName == "talk")
                AnswerToStatementTalk(statement);
            else if (statement.NullaryOrUnaryOperatorName == "talkabout")
                AnswerToStatementTalkAbout(statement);
            else if (statement.IsAnalogy)
                AnswerToStatementAnalogize(statement);
            else if (statement.IsAskingWhy)
                AnswerToStatementWhy(statement);
            else if (preAlterationStatementCorrectness && (statement.MetaOperatorName != null || statement.GetConceptName(2) != null))
                AnswerToStatementAcknowledge(statement);
            else if (statement.MetaOperatorName != null)
                AnswerToStatementMetaOperation(statement);
            else if (statement.GetConceptName(2) != null)
                AnswerToStatementOperation(statement);
            else if (statement.NamingAssociationOperatorName != null)
                AnswerToStatementNamingAssociation(statement);

            if (statement.NullaryOrUnaryOperatorName != "think" && statement.NullaryOrUnaryOperatorName != "thinkabout" && statement.NullaryOrUnaryOperatorName != "talk" && statement.NullaryOrUnaryOperatorName != "talkabout" && statement.NullaryOrUnaryOperatorName != "metathinkabout" && statement.NullaryOrUnaryOperatorName != "metathink" && statement.NullaryOrUnaryOperatorName != "linguisthink" && statement.NullaryOrUnaryOperatorName != "linguisthinkabout" && statement.NullaryOrUnaryOperatorName != "phonothinkabout" && statement.NullaryOrUnaryOperatorName != "phonothink")
                brain.ForgetCurrentTheory();

            mainWindow.AddToOutputText(aiVisibleCommentBuilder.Build());
        }

        private void AnswerToStatementVisualize(Statement statement)
        {
            Dictionary<int, List<int>> definitionByConceptId, connectionsNoWhyById;
            Dictionary<string, List<string>> definition, connectionsNoWhy = null;
            string conceptName = statement.GetConceptName(0);
            int conceptId = nameMapper.GetOrCreateConceptId(conceptName);

            if (mainWindow.IsVisualizerShowingLongDefinition)
            {
                definitionByConceptId = brain.GetLongDefinition(conceptId);
                connectionsNoWhyById = brain.GetShortDefinition(conceptId);
                connectionsNoWhy = DefinitionIdsToStrings(connectionsNoWhyById);
            }
            else
            {
                definitionByConceptId = brain.GetShortDefinition(conceptId);
            }

            definition = DefinitionIdsToStrings(definitionByConceptId);

            #region We sort the definition
            if (mainWindow.IsVisualizerShowingLongDefinition)
                definition = definitionSorter.SortComplementsByProofLength(definition, brain.GetConceptToLookIntoIt(conceptId), nameMapper, brain.GetMemoryToCountConceptComplementProofLength());
            definition = definitionSorter.SortByCustomOrder(definition);
            #endregion

            mainWindow.VisualizeConcept(nameMapper.InvertYouAndMePov(conceptName), definition, connectionsNoWhy);
        }

        private Dictionary<string, List<string>> DefinitionIdsToStrings(Dictionary<int, List<int>> definitionById)
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

        private void AnswerToStatementStatCross(Statement statement)
        {
            StatInfo statInfo;
            int numeratorVerbId;
            int numeratorComplementId;
            int denominatorVerbId;
            int denominatorComplementId;

            if (statement.ConceptCount >= 2)
            {
                denominatorVerbId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));
                denominatorComplementId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(1));

                if (statement.ConceptCount >= 4)
                {
                    numeratorVerbId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(2));
                    numeratorComplementId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(3));
                    statInfo = brain.GetStatOn(denominatorVerbId, denominatorComplementId, numeratorVerbId, numeratorComplementId);
                }
                else
                {
                    statInfo = brain.GetStatOn(denominatorVerbId, denominatorComplementId);
                }
            }
            else
            {
                statInfo = brain.GetRandomStat();
            }

            denominatorVerbId = statInfo.DenominatorVerbId;
            denominatorComplementId = statInfo.DenominatorComplementId;
            numeratorVerbId = statInfo.NumeratorVerbId;
            numeratorComplementId = statInfo.NumeratorComplementId;

            string denominatorVerbName = nameMapper.GetConceptNames(denominatorVerbId)[0];
            string denominatorComplementName = nameMapper.GetConceptNames(denominatorComplementId)[0];
            string numeratorVerbName = nameMapper.GetConceptNames(numeratorVerbId)[0];
            string numeratorComplementName = nameMapper.GetConceptNames(numeratorComplementId)[0];

            aiVisibleCommentBuilder.StatCrossValue = statInfo.Ratio;
            aiVisibleCommentBuilder.StatCrossExpression = new List<string>() { denominatorVerbName, denominatorComplementName, numeratorVerbName, numeratorComplementName };
        }

        private void AnswerToStatementSelect(Statement statement)
        {
            HashSet<int> selection = brain.Select(statement.SelectCondition, nameMapper,true);
            HashSet<string> selectionByName = new HashSet<string>();
            string conceptName;
            foreach (int conceptId in selection)
            {
                conceptName = nameMapper.GetConceptNames(conceptId)[0];
                selectionByName.Add(conceptName);
            }
            aiVisibleCommentBuilder.Selection = selectionByName;
        }

        private void AnswerToStatementUpdate(Statement statement)
        {
            HashSet<int> selection = brain.Select(statement.SelectCondition, nameMapper,false);
            HashSet<string> selectionByName = new HashSet<string>();
            string conceptName;
            foreach (int conceptId in selection)
            {
                conceptName = nameMapper.InvertYouAndMePov(nameMapper.GetConceptNames(conceptId)[0]);
                selectionByName.Add(conceptName);
            }
            ParseGeneralAffectation(selectionByName, statement.UpdateAction);
        }

        private void AnswerToStatementImply(Statement statement, bool preAlterationStatementCorrectness)
        {
            if (preAlterationStatementCorrectness)
            {
                aiVisibleCommentBuilder.ImplyConnectionMessage = "No changes made";
            }
            else if (brain.TestImplyConnection(statement.UpdateAction, statement.SelectCondition, nameMapper))
            {
                aiVisibleCommentBuilder.ImplyConnectionMessage = "Imply connection created";
            }
            else
            {
                aiVisibleCommentBuilder.ImplyConnectionMessage = "Imply connection removed";
            }
        }

        private void AnswerToStatementNo(Statement statement)
        {
            brain.DiscardCurrentTheory();
            aiVisibleCommentBuilder.IsDiscardingTheory = true;
        }

        private void AnswerToStatementYes(Statement statement)
        {
            Trauma trauma = null;
            Theory currentTheory = brain.CurrentTheory;

            if (currentTheory == null)
            {
                FeelingMonitor.Add(FeelingMonitor.EMPTINESS);
                throw new VisibleCommentConstructionException("Couldn't find theory to validate");
            }

            statement = brain.GetStatementFromTheory(currentTheory, nameMapper);
            trauma = TryAlterBrainFromStatement(statement);

            if (trauma != null)
                AnswerWithTrauma(trauma);
            else if (statement.MetaOperatorName == null)
                AnswerToStatementOperation(statement);
            else
                AnswerToStatementMetaOperation(statement);
        }

        private void AnswerToStatementAnalogize(Statement statement)
        {           
            List<int> analogyByInt = null;
            
            if (statement.ConceptCount == 0)
                analogyByInt = brain.GetAnalogy();
            else if (statement.ConceptCount == 1)
                analogyByInt = brain.GetAnalogy(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)));
            else if (statement.ConceptCount == 2)
                analogyByInt = brain.GetAnalogy(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)), nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)));
            else if (statement.ConceptCount == 3)
                analogyByInt = brain.GetAnalogy(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)), nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)), nameMapper.GetOrCreateConceptId(statement.GetConceptName(2)));

            if (analogyByInt != null && analogyByInt.Count == 6)
            {
                aiVisibleCommentBuilder.Analogy = new List<string>();
                aiVisibleCommentBuilder.Analogy.Add(nameMapper.GetConceptNames(analogyByInt[0])[0] + " " + nameMapper.GetConceptNames(analogyByInt[1])[0] + " " + nameMapper.GetConceptNames(analogyByInt[2])[0]);
                aiVisibleCommentBuilder.Analogy.Add(nameMapper.GetConceptNames(analogyByInt[3])[0] + " " + nameMapper.GetConceptNames(analogyByInt[4])[0] + " " + nameMapper.GetConceptNames(analogyByInt[5])[0]);
            }
            else
            {
                FeelingMonitor.Add(FeelingMonitor.EMPTINESS);
                throw new StatementParsingException("Couldn't create analogy.");
            }
        }

        private void AnswerToStatementAcknowledge(Statement statement)
        {
            aiVisibleCommentBuilder.StatementAcknowledge += nameMapper.InvertYouAndMePov(statement.GetConceptName(0));
            if (statement.IsNegative)
                aiVisibleCommentBuilder.StatementAcknowledge += " not";
            if (statement.MetaOperatorName != null)
                aiVisibleCommentBuilder.StatementAcknowledge += " " + statement.MetaOperatorName;
            aiVisibleCommentBuilder.StatementAcknowledge += " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(1));

            if (statement.ConceptCount == 3)
                aiVisibleCommentBuilder.StatementAcknowledge += " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(2));
        }

        private void AnswerToStatementOperation(Statement statement)
        {
            if (brain.TestConnection(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                                  nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)),
                                  nameMapper.GetOrCreateConceptId(statement.GetConceptName(2))))
            {
                #region If connection exists
                if (statement.IsNegative)
                {
                    if (statement.IsInterrogative || !statement.IsInterrogative) //just to maintain explicit cohesion
                    {
                        #region If connection exist even though human is trying to teach otherwise
                        aiVisibleCommentBuilder.ProofDefinitionListPositiveRefutation = new List<string>();

                        List<List<int>> proof = brain.GetProofToConnection(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                                                                           nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)),
                                                                           nameMapper.GetOrCreateConceptId(statement.GetConceptName(2)));


                        foreach (List<int> argument in proof)
                            aiVisibleCommentBuilder.ProofDefinitionListPositiveRefutation.Add(nameMapper.GetConceptNames(argument[0])[0] + " " + nameMapper.GetConceptNames(argument[1])[0] + " " + nameMapper.GetConceptNames(argument[2])[0]);

                        if (proof.Count == 0)
                        {
                            string analogy = GetAnalogyOn(statement.GetConceptName(0), statement.GetConceptName(1), statement.GetConceptName(2));

                            if (analogy != null)
                                aiVisibleCommentBuilder.ProofDefinitionListPositiveRefutation.Add(analogy);

                            aiVisibleCommentBuilder.ProofDefinitionListPositiveRefutation.Add("someone told me");
                        }

                        aiVisibleCommentBuilder.ProofDefinitionListPositiveRefutation.Add(nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(1)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(2)));
                        #endregion
                    }
                }
                else
                {
                    if (statement.IsInterrogative)
                    {
                        #region If connection exist and human is asking whether it exists
                        aiVisibleCommentBuilder.ProofDefinitionListRegular = new List<string>();

                        List<List<int>> proof = brain.GetProofToConnection(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                                                                           nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)),
                                                                           nameMapper.GetOrCreateConceptId(statement.GetConceptName(2)));


                        foreach (List<int> argument in proof)
                            aiVisibleCommentBuilder.ProofDefinitionListRegular.Add(nameMapper.GetConceptNames(argument[0])[0] + " " + nameMapper.GetConceptNames(argument[1])[0] + " " + nameMapper.GetConceptNames(argument[2])[0]);

                        if (proof.Count == 0)
                        {
                            string analogy = GetAnalogyOn(statement.GetConceptName(0), statement.GetConceptName(1), statement.GetConceptName(2));
                            
                            if (analogy != null)
                                aiVisibleCommentBuilder.ProofDefinitionListRegular.Add(analogy);

                            aiVisibleCommentBuilder.ProofDefinitionListRegular.Add("someone told me");
                        }

                        aiVisibleCommentBuilder.ProofDefinitionListRegular.Add(nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(1)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(2)));
                        #endregion
                    }
                    else
                    {
                        #region If connection exists because human is teaching Ai
                        aiVisibleCommentBuilder.StatementAcknowledgeAndLearn = nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(1)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(2));
                        #endregion
                    }
                }
                #endregion
            }
            else
            {
                #region If connection doesn't exist
                if (statement.IsNegative)
                {
                    if (statement.IsInterrogative)
                    {
                        #region Connection doesn't exist and Ai is asking whether it doesn't exit or does it
                        aiVisibleCommentBuilder.StatementNotAcknowledging = nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(1)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(2));
                        #endregion
                    }
                    else
                    {
                        aiVisibleCommentBuilder.StatementAcknowledgeAndLearn = nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " not " + nameMapper.InvertYouAndMePov(statement.GetConceptName(1)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(2));
                    }
                }
                else
                {
                    
                    List<int> obstruction = brain.FindObstructionToConnection(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                                                                              nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)),
                                                                              nameMapper.GetOrCreateConceptId(statement.GetConceptName(2)));
                    if (obstruction == null)
                    {
                        #region If ai sees no reason to believe connection or not
                        aiVisibleCommentBuilder.StatementNotAcknowledging = nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(1)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(2));
                        #endregion
                    }
                    else
                    {
                        #region If we tried to teach something to Ai, but Ai found obstruction
                        List<string> obstructionByString = new List<string>();

                        foreach (int conceptId in obstruction)
                            obstructionByString.Add(nameMapper.GetConceptNames(conceptId)[0]);
                        aiVisibleCommentBuilder.ProofDefinitionListRefutation = new List<string>();

                        List<List<int>> refutation = brain.GetProofToConnection(obstruction[0], obstruction[1], obstruction[2]);

                        foreach (List<int> argument in refutation)
                            aiVisibleCommentBuilder.ProofDefinitionListRefutation.Add(nameMapper.GetConceptNames(argument[0])[0] + " " + nameMapper.GetConceptNames(argument[1])[0] + " " + nameMapper.GetConceptNames(argument[2])[0]);

                        aiVisibleCommentBuilder.ProofDefinitionListRefutation.Add(obstructionByString[0] + " " + obstructionByString[1] + " " + obstructionByString[2]);
                        aiVisibleCommentBuilder.ProofDefinitionListRefutation.Add(nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(1)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(2)));
                        #endregion
                    }
                }
                #endregion
            }
        }

        private void AnswerToStatementMetaOperation(Statement statement)
        {
            if (statement.IsNegative)
            {
                aiVisibleCommentBuilder.StatementAcknowledgeAndLearn = nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " not " + statement.MetaOperatorName + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(1));
            }
            else
            {
                aiVisibleCommentBuilder.StatementAcknowledgeAndLearn = nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " " + statement.MetaOperatorName + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(1));
            }
        }

        private void AnswerToStatementWhy(Statement statement)
        {
            if (brain.TestConnection(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                                  nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)),
                                  nameMapper.GetOrCreateConceptId(statement.GetConceptName(2))))
            {
                #region If connection exist
                if (statement.IsNegative)
                {
                    #region If human assumed connection didn't exist
                    aiVisibleCommentBuilder.ProofDefinitionListPositiveRefutation = new List<string>();

                    List<List<int>> proof = brain.GetProofToConnection(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                                                                       nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)),
                                                                       nameMapper.GetOrCreateConceptId(statement.GetConceptName(2)));


                    foreach (List<int> argument in proof)
                        aiVisibleCommentBuilder.ProofDefinitionListPositiveRefutation.Add(nameMapper.GetConceptNames(argument[0])[0] + " " + nameMapper.GetConceptNames(argument[1])[0] + " " + nameMapper.GetConceptNames(argument[2])[0]);

                    if (proof.Count == 0)
                    {
                        /*aiVisibleCommentBuilder.ProofDefinitionListPositiveRefutation.Add("me ignore answer");

                        string analogy = GetAnalogyOn(statement.GetConceptName(0), statement.GetConceptName(1), statement.GetConceptName(2));
                        
                        if (analogy != null)
                            aiVisibleCommentBuilder.ProofDefinitionListPositiveRefutation.Add(analogy);*/

                        aiVisibleCommentBuilder.ProofDefinitionListPositiveRefutation.Add("someone told me");
                    }

                    aiVisibleCommentBuilder.ProofDefinitionListPositiveRefutation.Add(nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(1)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(2)));
                    #endregion
                }
                else
                {
                    #region If human assumed connection did exist
                    aiVisibleCommentBuilder.ProofDefinitionListRegular = new List<string>();

                    List<List<int>> proof = brain.GetProofToConnection(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                                                                       nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)),
                                                                       nameMapper.GetOrCreateConceptId(statement.GetConceptName(2)));


                    foreach (List<int> argument in proof)
                        aiVisibleCommentBuilder.ProofDefinitionListRegular.Add(nameMapper.GetConceptNames(argument[0])[0] + " " + nameMapper.GetConceptNames(argument[1])[0] + " " + nameMapper.GetConceptNames(argument[2])[0]);

                    if (proof.Count == 0)
                    {
                        /*aiVisibleCommentBuilder.ProofDefinitionListRegular.Add("me ignore answer");

                        string analogy = GetAnalogyOn(statement.GetConceptName(0), statement.GetConceptName(1), statement.GetConceptName(2));
                        
                        if (analogy != null)
                            aiVisibleCommentBuilder.ProofDefinitionListRegular.Add(analogy);*/

                        aiVisibleCommentBuilder.ProofDefinitionListRegular.Add("someone told me");
                    }


                    aiVisibleCommentBuilder.ProofDefinitionListRegular.Add(nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(1)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(2)));
                    #endregion
                }
                #endregion
            }
            else
            {
                #region If connection doesn't exist
                if (statement.IsNegative)
                {
                    #region If human assumed connection didn't exist

                    List<int> obstruction = brain.FindObstructionToConnection(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                                                                              nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)),
                                                                              nameMapper.GetOrCreateConceptId(statement.GetConceptName(2)));
                    if (obstruction == null)
                    {
                        #region If ai sees no reason to believe connection or not
                        aiVisibleCommentBuilder.StatementNotAcknowledging = statement.GetConceptName(0) + " " + statement.GetConceptName(1) + " " + statement.GetConceptName(2);
                        #endregion
                    }
                    else
                    {
                        #region If we tried to teach something to Ai, but Ai found obstruction
                        List<string> obstructionByString = new List<string>();

                        foreach (int conceptId in obstruction)
                            obstructionByString.Add(nameMapper.GetConceptNames(conceptId)[0]);
                        aiVisibleCommentBuilder.ProofDefinitionListRefutation = new List<string>();

                        List<List<int>> refutation = brain.GetProofToConnection(obstruction[0], obstruction[1], obstruction[2]);

                        foreach (List<int> argument in refutation)
                            aiVisibleCommentBuilder.ProofDefinitionListRefutation.Add(nameMapper.GetConceptNames(argument[0])[0] + " " + nameMapper.GetConceptNames(argument[1])[0] + " " + nameMapper.GetConceptNames(argument[2])[0]);

                        aiVisibleCommentBuilder.ProofDefinitionListRefutation.Add(obstructionByString[0] + " " + obstructionByString[1] + " " + obstructionByString[2]);
                        aiVisibleCommentBuilder.ProofDefinitionListRefutation.Add(nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " " + statement.GetConceptName(1) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(2)));
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region If human assumed connection did exist

                    List<int> obstruction = brain.FindObstructionToConnection(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                                                                              nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)),
                                                                              nameMapper.GetOrCreateConceptId(statement.GetConceptName(2)));
                    if (obstruction == null)
                    {
                        #region If ai sees no reason to believe connection or not
                        aiVisibleCommentBuilder.StatementNotAcknowledging = nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " " + statement.GetConceptName(1) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(2));
                        #endregion
                    }
                    else
                    {
                        #region If we tried to teach something to Ai, but Ai found obstruction
                        List<string> obstructionByString = new List<string>();

                        foreach (int conceptId in obstruction)
                            obstructionByString.Add(nameMapper.GetConceptNames(conceptId)[0]);
                        aiVisibleCommentBuilder.ProofDefinitionListRefutation = new List<string>();

                        List<List<int>> refutation = brain.GetProofToConnection(obstruction[0], obstruction[1], obstruction[2]);

                        foreach (List<int> argument in refutation)
                            aiVisibleCommentBuilder.ProofDefinitionListRefutation.Add(nameMapper.GetConceptNames(argument[0])[0] + " " + nameMapper.GetConceptNames(argument[1])[0] + " " + nameMapper.GetConceptNames(argument[2])[0]);

                        aiVisibleCommentBuilder.ProofDefinitionListRefutation.Add(obstructionByString[0] + " " + obstructionByString[1] + " " + obstructionByString[2]);
                        aiVisibleCommentBuilder.ProofDefinitionListRefutation.Add(nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " " + statement.GetConceptName(1) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(2)));
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
        }

        private void AnswerToStatementWhatis(Statement statement)
        {
            string conceptName = statement.GetConceptName(0);
            int conceptId = nameMapper.GetOrCreateConceptId(conceptName);

            aiVisibleCommentBuilder.DefinitionListWhatis = GetDefinitionWhatis(conceptId);
            aiVisibleCommentBuilder.DefinitionListWhatis = definitionSorter.SortByCustomOrder(aiVisibleCommentBuilder.DefinitionListWhatis);
            aiVisibleCommentBuilder.DefinitionListPositiveImply = GetDefinitionListImply(conceptId,true);
            aiVisibleCommentBuilder.DefinitionListNegativeImply = GetDefinitionListImply(conceptId,false);
            aiVisibleCommentBuilder.ConceptToDefine = nameMapper.InvertYouAndMePov(statement.GetConceptName(0));
        }

        private Dictionary<string, List<string>> GetDefinitionWhatis(int conceptId)
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

        private void AnswerToStatementDefine(Statement statement)
        {
            string conceptName = statement.GetConceptName(0);
            int conceptId = nameMapper.GetOrCreateConceptId(conceptName);

            Dictionary<int, List<int>> definitionByConceptId = brain.GetLongDefinition(conceptId);
            aiVisibleCommentBuilder.DefinitionListDefine = new Dictionary<string, List<string>>();

            //For connections
            foreach (KeyValuePair<int, List<int>> verbAndBranch in definitionByConceptId)
            {
                int verb = verbAndBranch.Key;
                List<int> branch = verbAndBranch.Value;
                string verbName = nameMapper.GetConceptNames(verb)[0];

                List<string> branchInString = new List<string>();
                foreach (int complementId in branch)
                    branchInString.Add(nameMapper.GetConceptNames(complementId)[0]);

                
                aiVisibleCommentBuilder.DefinitionListDefine.Add(verbName, branchInString);
            }

            //For metaConnections
            foreach (string metaOperator in LanguageDictionary.MetaOperatorList)
            {
                List<int> metaConnectionVerbIdList = brain.GetFlatMetaOperationVerbIdList(conceptId, metaOperator);
                List<string> metaConnectionVerbList = new List<string>();
                foreach (int farVerbId in metaConnectionVerbIdList)
                    metaConnectionVerbList.Add(nameMapper.GetConceptNames(farVerbId)[0]);

                if (metaConnectionVerbList.Count > 0)
                    aiVisibleCommentBuilder.DefinitionListDefine.Add(metaOperator.ToUpper(), metaConnectionVerbList);
            }

            aiVisibleCommentBuilder.DefinitionListDefine = definitionSorter.SortComplementsByProofLength(aiVisibleCommentBuilder.DefinitionListDefine, brain.GetConceptToLookIntoIt(conceptId), nameMapper, brain.GetMemoryToCountConceptComplementProofLength());
            aiVisibleCommentBuilder.DefinitionListDefine = definitionSorter.SortByCustomOrder(aiVisibleCommentBuilder.DefinitionListDefine);
            aiVisibleCommentBuilder.DefinitionListPositiveImply = GetDefinitionListImply(conceptId,true);
            aiVisibleCommentBuilder.DefinitionListNegativeImply = GetDefinitionListImply(conceptId,false);
            aiVisibleCommentBuilder.ConceptToDefine = nameMapper.InvertYouAndMePov(statement.GetConceptName(0));
        }

        private void AnswerExceptionMessage(string exceptionMessage)
        {
            aiVisibleCommentBuilder.AiName = aiName.Value;
            aiVisibleCommentBuilder.ExceptionMessage = exceptionMessage;
            Paragraph visibleComment = aiVisibleCommentBuilder.Build();
            mainWindow.AddToOutputText(visibleComment);
        }

        private void AnswerToStatementNamingAssociation(Statement statement)
        {
            if (statement.IsNegative)
                aiVisibleCommentBuilder.StatementAcknowledgeAndLearn = statement.GetConceptName(0) + " not " + statement.NamingAssociationOperatorName + " " + statement.GetConceptName(1);
            else
                aiVisibleCommentBuilder.StatementAcknowledgeAndLearn = statement.GetConceptName(0) + " " + statement.NamingAssociationOperatorName + " " + statement.GetConceptName(1);
        }

        private void AnswerToStatementTeachAbout(Statement statement)
        {
            int conceptId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));

            KeyValuePair<List<int>,List<List<int>>> connectionAndProof = brain.TeachAbout(conceptId);
            List<int> connection = connectionAndProof.Key;

            aiVisibleCommentBuilder.ProofDefinitionListTeachabout = new List<string>();
            foreach (List<int> argument in connectionAndProof.Value)
                aiVisibleCommentBuilder.ProofDefinitionListTeachabout.Add(nameMapper.GetConceptNames(argument[0])[0] + " " + nameMapper.GetConceptNames(argument[1])[0] + " " + nameMapper.GetConceptNames(argument[2])[0]);

            aiVisibleCommentBuilder.ProofDefinitionListTeachabout.Add(nameMapper.GetConceptNames(connection[0])[0] + " " + nameMapper.GetConceptNames(connection[1])[0] + " " + nameMapper.GetConceptNames(connection[2])[0]);
        }

        private void AnswerToStatementTeach(Statement statement)
        {
            KeyValuePair<List<int>, List<List<int>>> connectionAndProof = brain.TeachAboutRandomConcept();
            List<int> connection = connectionAndProof.Key;

            aiVisibleCommentBuilder.ProofDefinitionListTeachabout = new List<string>();
            foreach (List<int> argument in connectionAndProof.Value)
                aiVisibleCommentBuilder.ProofDefinitionListTeachabout.Add(nameMapper.GetConceptNames(argument[0])[0] + " " + nameMapper.GetConceptNames(argument[1])[0] + " " + nameMapper.GetConceptNames(argument[2])[0]);

            aiVisibleCommentBuilder.ProofDefinitionListTeachabout.Add(nameMapper.GetConceptNames(connection[0])[0] + " " + nameMapper.GetConceptNames(connection[1])[0] + " " + nameMapper.GetConceptNames(connection[2])[0]);
        }

        private void AnswerToStatementAskAbout(Statement statement)
        {
            int conceptId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));
            List<int> question = brain.GetBestQuestionAbout(conceptId);
            aiVisibleCommentBuilder.DefinitionListWhatis = GetDefinitionWhatis(question[0]);
            aiVisibleCommentBuilder.DefinitionListPositiveImply = GetDefinitionListImply(question[0],true);
            aiVisibleCommentBuilder.DefinitionListNegativeImply = GetDefinitionListImply(question[0],false);
            aiVisibleCommentBuilder.ConceptToDefine = nameMapper.GetConceptNames(question[0])[0];
            aiVisibleCommentBuilder.StatementStubAskingForRandomConnection = nameMapper.GetConceptNames(question[0])[0] + " " + nameMapper.GetConceptNames(question[1])[0];
        }

        private void AnswerToStatementAsk(Statement statement)
        {
            List<int> question = brain.GetBestQuestionAboutRandomConcept();
            aiVisibleCommentBuilder.DefinitionListWhatis = GetDefinitionWhatis(question[0]);
            aiVisibleCommentBuilder.DefinitionListPositiveImply = GetDefinitionListImply(question[0],true);
            aiVisibleCommentBuilder.DefinitionListNegativeImply = GetDefinitionListImply(question[0],false);
            aiVisibleCommentBuilder.ConceptToDefine = nameMapper.GetConceptNames(question[0])[0];
            aiVisibleCommentBuilder.StatementStubAskingForRandomConnection = nameMapper.GetConceptNames(question[0])[0] + " " + nameMapper.GetConceptNames(question[1])[0];
        }

        private void AnswerToStatementThinkAbout(Statement statement)
        {
            int conceptId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));
            aiVisibleCommentBuilder.Theory = brain.GetBestTheoryAboutConcept(conceptId, nameMapper);
        }

        private void AnswerToStatementLinguisThinkAbout(Statement statement)
        {
            int conceptId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));
            aiVisibleCommentBuilder.Theory = brain.GetBestLinguisticTheoryAboutConcept(conceptId, nameMapper);
        }

        private void AnswerToStatementPhonoThinkAbout(Statement statement)
        {
            int conceptId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));
            aiVisibleCommentBuilder.Theory = brain.GetBestPhoneticTheoryAboutConcept(conceptId, nameMapper);
        }

        private void AnswerToStatementMetaThinkAbout(Statement statement)
        {
            int conceptId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));
            aiVisibleCommentBuilder.Theory = brain.GetBestTheoryAboutOperator(conceptId, nameMapper);
        }

        private void AnswerToStatementThink(Statement statement)
        {
            aiVisibleCommentBuilder.Theory = brain.GetBestTheoryAboutRandomConcept(nameMapper);
        }

        private void AnswerToStatementLinguisThink(Statement statement)
        {
            aiVisibleCommentBuilder.Theory = brain.GetBestLinguisticTheoryAboutRandomConcept(nameMapper);
        }

        private void AnswerToStatementPhonoThink(Statement statement)
        {
            aiVisibleCommentBuilder.Theory = brain.GetBestPhoneticTheoryAboutRandomConcept(nameMapper);
        }

        private void AnswerToStatementMetaThink(Statement statement)
        {
            aiVisibleCommentBuilder.Theory = brain.GetBestTheoryAboutRandomOperator(nameMapper);
        }

        private void AnswerToStatementTalkAbout(Statement statement)
        {
            Action<Statement> answerThinkAbout = new Action<Statement>(AnswerToStatementThinkAbout);
            Action<Statement> answerAskAbout = new Action<Statement>(AnswerToStatementAskAbout);
            Action<Statement> answerTeachAbout = new Action<Statement>(AnswerToStatementTeachAbout);
            Action<Statement> answerAnalogize = new Action<Statement>(AnswerToStatementAnalogize);
            Action<Statement> answerMetaThinkAbout = new Action<Statement>(AnswerToStatementMetaThinkAbout);
            Action<Statement> answerLinguisThinkAbout = new Action<Statement>(AnswerToStatementLinguisThinkAbout);
            Action<Statement> answerPhonoThinkAbout = new Action<Statement>(AnswerToStatementPhonoThinkAbout);
            
            HashSet<Action<Statement>> answerListSample = new HashSet<Action<Statement>>();
            answerListSample.Add(answerThinkAbout);
            answerListSample.Add(answerAskAbout);
            answerListSample.Add(answerTeachAbout);
            answerListSample.Add(answerAnalogize);
            answerListSample.Add(answerMetaThinkAbout);
            answerListSample.Add(answerLinguisThinkAbout);
            answerListSample.Add(answerPhonoThinkAbout);
            List<Action<Statement>> answerList = new List<Action<Statement>>();

            Action<Statement> answerStatement = null;
            while (answerListSample.Count > 0)
            {
                int randomId = random.Next(answerListSample.Count);
                int counter = 0;
                foreach (Action<Statement> currentAnswer in answerListSample)
                {
                    if (counter == randomId)
                        answerStatement = currentAnswer;
                    counter++;
                }

                answerList.Add(answerStatement);
                answerListSample.Remove(answerStatement);
            }

            try
            {
                answerList[0](statement);
            }
            catch (Exception)
            {
                try
                {
                    answerList[1](statement);
                }
                catch (Exception)
                {
                    try
                    {
                        answerList[2](statement);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            answerList[3](statement);
                        }
                        catch (Exception)
                        {
                            try
                            {
                                answerList[4](statement);
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    answerList[5](statement);
                                }
                                catch (Exception)
                                {
                                    answerList[6](statement);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AnswerToStatementTalk(Statement statement)
        {
            Action<Statement> answerThink = new Action<Statement>(AnswerToStatementThink);
            Action<Statement> answerAsk = new Action<Statement>(AnswerToStatementAsk);
            Action<Statement> answerTeach = new Action<Statement>(AnswerToStatementTeach);
            Action<Statement> answerAnalogize = new Action<Statement>(AnswerToStatementAnalogize);
            Action<Statement> answerStatcross = new Action<Statement>(AnswerToStatementStatCross);
            Action<Statement> answerMetaThink = new Action<Statement>(AnswerToStatementMetaThink);
            Action<Statement> answerLinguisThink = new Action<Statement>(AnswerToStatementLinguisThink);
            Action<Statement> answerPhonoThink = new Action<Statement>(AnswerToStatementPhonoThink);

            HashSet<Action<Statement>> answerListSample = new HashSet<Action<Statement>>();
            answerListSample.Add(answerThink);
            answerListSample.Add(answerAsk);
            answerListSample.Add(answerTeach);
            answerListSample.Add(answerAnalogize);
            answerListSample.Add(answerStatcross);
            answerListSample.Add(answerMetaThink);
            answerListSample.Add(answerLinguisThink);
            answerListSample.Add(answerPhonoThink);
            List<Action<Statement>> answerList = new List<Action<Statement>>();

            Action<Statement> answerStatement = null;
            while (answerListSample.Count > 0)
            {
                int randomId = random.Next(answerListSample.Count);
                int counter = 0;
                foreach (Action<Statement> currentAnswer in answerListSample)
                {
                    if (counter == randomId)
                        answerStatement = currentAnswer;
                    counter++;
                }

                answerList.Add(answerStatement);
                answerListSample.Remove(answerStatement);
            }

            try
            {
                answerList[0](statement);
            }
            catch (Exception)
            {
                try
                {
                    answerList[1](statement);
                }
                catch (Exception)
                {
                    try
                    {
                        answerList[2](statement);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            answerList[3](statement);
                        }
                        catch (Exception)
                        {
                            try
                            {
                                answerList[4](statement);
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    answerList[5](statement);
                                }
                                catch (Exception)
                                {
                                    try
                                    {
                                        answerList[6](statement);
                                    }
                                    catch (Exception)
                                    {
                                        answerList[7](statement);
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        private void AnswerWithTrauma(Trauma trauma)
        {
            List<int> traumaByInt = brain.GetTraumaByInt(trauma);

            List<string> traumaByString = new List<string>();

            foreach (int conceptId in traumaByInt)
                traumaByString.Add(nameMapper.GetConceptNames(conceptId)[0]);

            aiVisibleCommentBuilder.AiName = aiName.Value;
            aiVisibleCommentBuilder.Trauma = traumaByString;

            Paragraph visibleComment = aiVisibleCommentBuilder.Build();
            mainWindow.AddToOutputText(visibleComment);
        }

        private void AnswerToStatementStartPsychosis(Statement statement)
        {
            brain.DisableRepairAfterMetaConnection = true;

            List<Theory> theoryList = brain.GetTheoryListWithMinimumProbability((double)(statement.Probability / 100.0), 10);

            int lineCounter = 0;
            foreach (Theory currentTheory in theoryList)
            {
                statement = brain.GetStatementFromTheory(currentTheory, nameMapper);
                ChooseDecisionFromStatement(statement);

                if (lineCounter == theoryList.Count - 1)
                    brain.DisableRepairAfterMetaConnection = false;

                lineCounter++;
            }

            brain.CleanBackgroundThinker();
        }

        private string GetAnalogyOn(string subjectName, string verbName, string complementName)
        {
            int subjectId = nameMapper.GetOrCreateConceptId(subjectName);
            int verbId = nameMapper.GetOrCreateConceptId(verbName);
            int complementId = nameMapper.GetOrCreateConceptId(complementName);

            List<int> analogy = brain.GetAnalogy(subjectId, verbId, complementId);

            if (analogy == null)
                return null;

            return nameMapper.GetConceptNames(analogy[3])[0] + " " + nameMapper.GetConceptNames(analogy[4])[0] + " " + nameMapper.GetConceptNames(analogy[5])[0];
        }
        #endregion

        private void ResetHumanName()
        {
            string newName = mainWindow.GetNewHumanNameFromInputBox(humanName.Value);
            if (newName != "" && newName != null)
                humanName.Value = newName;
        }

        private void AllowUserToLoadScriptFile()
        {
            string fileName = mainWindow.GetInputFile();

            if (fileName != null)
                RunScriptFile(fileName);
        }

        private void OpenFile()
        {
            fileName = mainWindow.GetInputFile(defaultFileTypeFilter);

            if (fileName != null)
            {
                brain.ResetBackgroundThinkerTheoryMemory();
                saverLoader.FileName = fileName;
                saverLoader.Load();
                Memory.TotalVerbList = saverLoader.TotalVerbList;
                brain.SetMemoryToLoad(saverLoader.Memory);
                nameMapper = saverLoader.NameMapper;
                brain.SetRejectedTheoriesToLoad(saverLoader.RejectedTheories);
                humanName.Value = nameMapper.HumanName.Value;
                aiName.Value = nameMapper.AiName.Value;
                nameMapper.HumanName = humanName;
                nameMapper.AiName = aiName;
                mainWindow.RefreshAutoCompleteFrom(this.brain.GetMemoryToUpdateAutoComplete(), this.nameMapper);
                mainWindow.SetMemoryTreeViewBinding(brain.GetMemoryToUpdateMemoryTreeView(), nameMapper);
                mainWindow.VerbNameListForHelp = nameMapper.GetTotalVerbNameList(brain.GetMemoryToUpdateVerbNameListInHelp());
                fileNeedSave = false;
                RefreshWindowTitle();
            }
        }

        private void ResetMemory()
        {
            fileName = null;
            fileNeedSave = false;
            RefreshWindowTitle();
            Memory.TotalVerbList = new HashSet<Concept>();
            brain.SetMemoryToLoad(new Memory());
            nameMapper = new NameMapper(aiName, humanName);
            brain.ResetBackgroundThinkerTheoryMemory();
            brain.ResetRejectedTheories();
            mainWindow.RefreshAutoCompleteFrom(this.brain.GetMemoryToUpdateAutoComplete(), this.nameMapper);
            mainWindow.SetMemoryTreeViewBinding(brain.GetMemoryToUpdateMemoryTreeView(), nameMapper);
            mainWindow.VerbNameListForHelp = nameMapper.GetTotalVerbNameList(brain.GetMemoryToUpdateVerbNameListInHelp());
        }

        private void SaveFile(string fileName)
        {
            if (fileName != null)
            {
                saverLoader.FileName = fileName;
                saverLoader.Memory = brain.GetMemoryToSave();
                saverLoader.NameMapper = nameMapper;
                saverLoader.TotalVerbList = Memory.TotalVerbList;
                saverLoader.RejectedTheories = brain.GetRejectedTheoriesToSave();
                saverLoader.TotalTheoryList = brain.GetTheoryListToSave();
                saverLoader.Save();
                fileNeedSave = false;
                RefreshWindowTitle();
            }
        }

        private void RunScriptFile(string fileName)
        {
            TextReader handle = new StreamReader(fileName);
            List<Statement> statementList;
            string line;

            do
            {
                line = handle.ReadLine();
                if (line != null)
                {
                    statementList = statementListFactory.GetInterpretedHumanStatementList(humanName.Value, line);
                    foreach (Statement statement in statementList)
                        ChooseDecisionFromStatement(statement);
                }
            } while (line != null);

            handle.Close();

            mainWindow.RefreshAutoCompleteFrom(brain.GetMemoryToUpdateAutoComplete(), nameMapper);
        }

        private void DeployInstinct(AbstractInstinct instinct)
        {
            brain.DisableRepairAfterMetaConnection = true;

            List<Statement> statementList;
            int lineCounter = 0;
            foreach (string line in instinct)
            {
                statementList = statementListFactory.GetInterpretedHumanStatementList("instinct", line);

                int statementCounter = 0;
                foreach (Statement statement in statementList)
                {
                    if (statementCounter == statementList.Count - 1 && lineCounter == instinct.Count - 1)
                        brain.DisableRepairAfterMetaConnection = false;

                    ChooseDecisionFromStatement(statement);
                    statementCounter++;
                }
                lineCounter++;
            }
        }

        private void ParseGeneralAffectation(HashSet<string> affectedConceptNameList, string generalAffectationStatement)
        {
            brain.DisableFlattenizeAndOptimize = true;
            List<Statement> statementList;
            foreach (string conceptName in affectedConceptNameList)
            {
                statementList = statementListFactory.GetInterpretedHumanStatementList("wikipedia", conceptName.Trim() + " " + generalAffectationStatement.Trim());
                foreach (Statement statement in statementList)
                {
                    try
                    {
                        TryAlterBrainFromStatement(statement);
                    }
                    catch (ConnectionException)
                    {
                        FeelingMonitor.Add(FeelingMonitor.CONNECTION_EXCEPTION);
                    }
                }
            }
            brain.DisableFlattenizeAndOptimize = false;
            brain.ForceRepairMemory();
        }

        private Dictionary<string, List<string>> GetDefinitionListImply(int verbId, bool isPositive)
        {
            return brain.GetDefinitionListImply(verbId, nameMapper, isPositive);
        }

        private void RefreshWindowTitle()
        {
            string title = "";
            if (fileName == null)
                title += "untitled.xml";
            else
                title += fileName;

            if (title.Contains('\\'))
                title = title.Substring(title.LastIndexOf('\\') + 1);

            if (fileNeedSave)
                title += "*";

            title += " - Riasce";

            mainWindow.Title = title;
        }

        private void StopBackgroundThinker()
        {
            Stack<Trauma> traumaStack = brain.StopBackgroundThinker();

            while (traumaStack.Count > 0)
                AnswerWithTrauma(traumaStack.Pop());
        }
        #endregion
        
        #region Handlers
        public void UserSendCommentHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            List<Statement> statementList;
            Paragraph visibleComment;

            try
            {
                statementList = statementListFactory.GetInterpretedHumanStatementList(humanName.Value, mainWindow.InputText);

                foreach (Statement statement in statementList)
                {
                    if (statement.NullaryOrUnaryOperatorName == "exit")
                        mainWindow.Close();

                    humanVisibleCommentBuilder.SetConstructionSettingsAccordingToStatement(statement);
                    visibleComment = humanVisibleCommentBuilder.Build();

                    mainWindow.AddToOutputText(visibleComment);
                }

                foreach (Statement statement in statementList)
                    ChooseDecisionFromStatement(statement);
            }
            catch (StatementParsingException)
            {
                humanVisibleCommentBuilder.HumanName = humanName.Value;
                humanVisibleCommentBuilder.UnparsableComment = mainWindow.InputText;
                visibleComment = humanVisibleCommentBuilder.Build();
                mainWindow.AddToOutputText(visibleComment);
            }
            finally
            {
                mainWindow.InputText = "";

                if (conversation != null)
                    ChooseDecisionFromStatement(conversation.GetCurrentStatementForAiToAnswerTo());

                TellAboutFeelings();
            }

            brain.StartBackgroundThinker(nameMapper);
        }

        private void TellAboutFeelings()
        {
            //mainWindow.ResetFeelings();
            foreach (string currentFeeling in FeelingMonitor.GetCurrentFeelingListAndReset())
            {
                aiVisibleCommentBuilder.FeelingMessage = currentFeeling;
                Paragraph aiVisibleCommentAboutFeeling = aiVisibleCommentBuilder.Build();
                mainWindow.AddToFeelings(aiVisibleCommentAboutFeeling);
            }
        }

        private void UserWantsToExitHandler(object sender, CancelEventArgs e)
        {
            StopBackgroundThinker();

            if (fileNeedSave)
            {
                MessageBoxResult result = mainWindow.AskToSave();
                if (result == MessageBoxResult.Yes)
                {
                    UserSaveFileHandler(this, new EventArgs());
                    if (fileNeedSave)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
            Application.Current.Shutdown();
        }

        private void UserSignOutHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();
            ResetHumanName();
            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserRunScriptHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();
            AllowUserToLoadScriptFile();
            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserOpenFileHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            if (fileNeedSave)
            {
                MessageBoxResult result = mainWindow.AskToSave();
                if (result == MessageBoxResult.Yes)
                {
                    UserSaveFileHandler(this, new EventArgs());
                    if (fileNeedSave)
                        return;
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            OpenFile();

            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserFileNewHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            if (fileNeedSave)
            {
                MessageBoxResult result = mainWindow.AskToSave();
                if (result == MessageBoxResult.Yes)
                {
                    UserSaveFileHandler(this, new EventArgs());
                    if (fileNeedSave)
                        return;
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            ResetMemory();
            mainWindow.ResetTextFields();

            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserSaveFileHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            if (fileName == null)
                fileName = mainWindow.GetOutputFile(defaultFileTypeFilter);
            SaveFile(fileName);

            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserSaveAsFileHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            string fileName = mainWindow.GetOutputFile(defaultFileTypeFilter);

            if (fileName != null)
                this.fileName = fileName;

            SaveFile(fileName);

            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserViewHelpHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            mainWindow.tabControl.SelectedIndex = 2;

            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserConversationStartHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            conversation = new Conversation();
            ChooseDecisionFromStatement(conversation.GetCurrentStatementForAiToAnswerTo());

            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserConversationStopHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            conversation = null;

            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserStandardInstinctHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            StandardInstinct standardInstinct = new StandardInstinct();
            DeployInstinct(standardInstinct);

            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserEgoHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            Ego ego = new Ego();
            DeployInstinct(ego);

            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserClickConceptHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            Span source = (Span)(sender);
            string textStatement = (string)(source.Tag);

            string[] words = textStatement.Split(' ');
            if (words[0] == "InvertYouAndMePov" && words.Length == 3)
                textStatement = words[1] + " " + nameMapper.InvertYouAndMePov(words[2]);

            //Paragraph visibleComment;

            List<Statement> statementList = statementListFactory.GetInterpretedHumanStatementList(humanName.Value, textStatement);

            /*
            foreach (Statement statement in statementList)
            {
                humanVisibleCommentBuilder.SetConstructionSettingsAccordingToStatement(statement);
                visibleComment = humanVisibleCommentBuilder.Build();
                mainWindow.AddToOutputText(visibleComment);            
            }
            */

            foreach (Statement statement in statementList)
            {
                ChooseDecisionFromStatement(statement);
            }

            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserExtractCategoryListFromWikiHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            categoryListExtractor = new CategoryListExtractor();

            categoryListExtractor.FileName = mainWindow.GetOutputFile("Raw Category List|*.RawCategoryList",false);
            if (categoryListExtractor.FileName == null)
                return;

            categoryListExtractor.Show();

            Thread thread = new Thread(categoryListExtractor.Start);
            thread.Start();

            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserSortCategoryListHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            string inputFileName = mainWindow.GetInputFile("Raw Category List|*.RawCategoryList");

            if (inputFileName == null)
                return;

            string outputFileName = mainWindow.GetOutputFile("Sorted Category List|*.SortedCategoryList");

            if (outputFileName != null)
                categoryListSorterTrimmer.SortAndTrimCategoryListFile(inputFileName, outputFileName);

            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserOpenCategoryExtractorHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            categoryExtractor = new CategoryExtractor();
            categoryExtractor.OnGeneralAffectation += CategoryExtractorGeneralAffectationHandler;
            categoryExtractor.Start();

            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserExtractOccurenceMatrixHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();
            string sourceTextFileName, targetXmlFile;

            sourceTextFileName = mainWindow.GetInputFile("TEXT FILE|*.txt");

            if (sourceTextFileName != null)
            {
                targetXmlFile = mainWindow.GetOutputFile("XML File|*.xml");

                if (targetXmlFile != null && targetXmlFile != sourceTextFileName)
                {
                    Matrix matrix = wordMatrixExtractor.BuildMatrixFromTextFile(sourceTextFileName);
                    xmlMatrixSaverLoader.Save(matrix, targetXmlFile);
                }
            }

            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserExtractPhoneticMatrixHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();
            string sourceTextFileName, targetXmlFile;

            sourceTextFileName = mainWindow.GetInputFile("TEXT FILE|*.txt");

            if (sourceTextFileName != null)
            {
                targetXmlFile = mainWindow.GetOutputFile("XML File|*.xml");

                if (targetXmlFile != null && targetXmlFile != sourceTextFileName)
                {
                    Matrix matrix = phoneticMatrixManager.BuildMatrixFromTextFile(sourceTextFileName);
                    xmlMatrixSaverLoader.Save(matrix, targetXmlFile);
                }
            }

            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserOccurenceToSemanticHandler(object sender, EventArgs e)
        {
            string sourceXmlFileName, targetXmlFile;
            StopBackgroundThinker();

            int sourceWordCount = mainWindow.GetIntegerInput("Parse up to how many source words?", 20, 6000, 20000);
            int targetWordCount = mainWindow.GetIntegerInput("Generate how many target word per source word?", 20, 60, 20000);

            sourceXmlFileName = mainWindow.GetInputFile("XML File|*.xml");
            if (sourceXmlFileName != null)
            {
                targetXmlFile = mainWindow.GetOutputFile("XML File|*.xml");

                if (targetXmlFile != null && targetXmlFile != sourceXmlFileName)
                {
                    Matrix matrix = xmlMatrixSaverLoader.Load(sourceXmlFileName);

                    matrix = semanticLikenessMatrixBuilder.Build(matrix,sourceWordCount, targetWordCount);

                    xmlMatrixSaverLoader.Save(matrix, targetXmlFile);
                }
            }

            brain.StartBackgroundThinker(nameMapper);
        }

        private void CategoryExtractorGeneralAffectationHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            HashSet<string> affectedConceptNameList = categoryExtractor.CategoryElementNameList;            
            string generalAffectationStatement = categoryExtractor.GeneralAffectation;
            ParseGeneralAffectation(affectedConceptNameList, generalAffectationStatement);

            brain.StartBackgroundThinker(nameMapper);
        }

        private void VisualizerClickConceptHandler(object sender, StringEventArgs e)
        {
            StopBackgroundThinker();

            List<Statement> statementList = statementListFactory.GetInterpretedHumanStatementList(humanName.Value, "visualize " + nameMapper.InvertYouAndMePov(e.Name));

            foreach (Statement statement in statementList)
                ChooseDecisionFromStatement(statement);

            brain.StartBackgroundThinker(nameMapper);
        }

        private void VisualizerClickWhyLinkHandler(object sender, StringEventArgs e)
        {
            StopBackgroundThinker();

            string[] words = e.Name.Split(' ');

            List<Statement> statementList = statementListFactory.GetInterpretedHumanStatementList(humanName.Value, "why " + nameMapper.InvertYouAndMePov(words[0]) + " " + words[1] + " " + nameMapper.InvertYouAndMePov(words[2]));

            foreach (Statement statement in statementList)
                ChooseDecisionFromStatement(statement);

            brain.StartBackgroundThinker(nameMapper);
        }

        private void VisualizerClickDefineLinkHandler(object sender, StringEventArgs e)
        {
            StopBackgroundThinker();

            List<Statement> statementList = statementListFactory.GetInterpretedHumanStatementList(humanName.Value, "define " + nameMapper.InvertYouAndMePov(e.Name));

            foreach (Statement statement in statementList)
                ChooseDecisionFromStatement(statement);

            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserViewEgoHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            Ego ego = new Ego();
            Paragraph paragraph;

            foreach (string line in ego)
            {
                paragraph = new Paragraph();
                paragraph.Inlines.Add("//" + line);
                mainWindow.AddToOutputText(paragraph);
            }

            brain.StartBackgroundThinker(nameMapper);
        }

        private void UserViewStandardInstinctHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            StandardInstinct instinct = new StandardInstinct();
            Paragraph paragraph;

            foreach (string line in instinct)
            {
                paragraph = new Paragraph();
                paragraph.Inlines.Add("//" + line);
                mainWindow.AddToOutputText(paragraph);
            }

            brain.StartBackgroundThinker(nameMapper);
        }
        #endregion
    }
}