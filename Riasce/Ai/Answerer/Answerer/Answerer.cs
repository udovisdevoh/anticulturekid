using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents Ai's ability to answer to human statements in the MainWindow
    /// Some kind of layer between Ai class and MainWindow
    /// </summary>
    class Answerer
    {
        #region Fields
        /// <summary>
        /// Persistant reference to ai's name mapper
        /// </summary>
        private NameMapper nameMapper;

        /// <summary>
        /// Persistant reference to ai's brain's
        /// </summary>
        private Brain brain;

        /// <summary>
        /// Persistant reference to ai's main window
        /// </summary>
        private MainWindow mainWindow;

        private Alterator alterator;

        /// <summary>
        /// Persistant reference to ai's aiVisibleCommentBuilder
        /// </summary>
        private AbstractAiVisibleCommentBuilder aiVisibleCommentBuilder = null;

        /// <summary>
        /// Persistant reference to Ai's statementList factory
        /// </summary>
        private AbstractStatementListFactory statementListFactory;

        /// <summary>
        /// Persistant reference to ai's aiName
        /// </summary>
        private Name aiName;

        /// <summary>
        /// Persistant reference to ai's humanName
        /// </summary>
        private Name humanName;

        private StringBasedDefinitionSorter definitionSorter = new StringBasedDefinitionSorter();

        private StringBasedDefinitionBuilder definitionManager = new StringBasedDefinitionBuilder();

        private Random random = new Random();
        #endregion

        #region Constructor
        public Answerer(Brain brain, NameMapper nameMapper, MainWindow mainWindow, Alterator alterator, AbstractAiVisibleCommentBuilder aiVisibleCommentBuilder, AbstractStatementListFactory statementListFactory, Name aiName, Name humanName)
        {
            this.brain = brain;
            this.nameMapper = nameMapper;
            this.mainWindow = mainWindow;
            this.alterator = alterator;
            this.aiVisibleCommentBuilder = aiVisibleCommentBuilder;
            this.statementListFactory = statementListFactory;
            this.aiName = aiName;
            this.humanName = humanName;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Main use case, choose decision from atypical statement provided by human
        /// </summary>
        /// <param name="statement">atypical statement provided by human</param>
        public void ChooseDecisionFromStatement(Statement statement)
        {
            Trauma trauma = null;

            #warning Remove comments
            try
            {
                brain.EpisodicRemember(humanName.Value, statement, nameMapper);

                bool preAlterationStatementCorrectness = TestStatementCorrectness(statement);
                if (!preAlterationStatementCorrectness || statement.NamingAssociationOperatorName != null)
                {
                    trauma = alterator.TryAlterBrainFromStatement(statement);
                }

                if (trauma != null)
                    AnswerWithTrauma(trauma);
                else
                    AnswerToStatement(statement, preAlterationStatementCorrectness);

                mainWindow.RefreshAutoCompleteFrom(brain.GetUnRepairedMemory(mainWindow), nameMapper);
                mainWindow.UpdateTreeViewIfNeeded();
                mainWindow.VerbNameListForHelp = nameMapper.GetTotalVerbNameList(brain.GetUnRepairedMemory(mainWindow));
            }
            catch (Exception e)
            {
                AnswerExceptionMessage(e.Message);
            }
        }

        /// <summary>
        /// Secondary use case, answer with provided trauma
        /// </summary>
        /// <param name="trauma"></param>
        public void AnswerWithTrauma(Trauma trauma)
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

        /// <summary>
        /// The Ai parses a general affectation (from wiki categories), calls the alterator and answers something about it
        /// </summary>
        /// <param name="affectedConceptNameList">affected concept name list</param>
        /// <param name="generalAffectationStatement">common affectation to concept list</param>
        public void ParseGeneralAffectation(HashSet<string> affectedConceptNameList, string generalAffectationStatement)
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
                        alterator.TryAlterBrainFromStatement(statement);
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

        /// <summary>
        /// The Ai tells about his feeling in the MainWindow
        /// </summary>
        public void TellAboutFeelings()
        {
            //mainWindow.ResetFeelings();
            foreach (string currentFeeling in FeelingMonitor.GetCurrentFeelingListAndReset())
            {
                aiVisibleCommentBuilder.FeelingMessage = currentFeeling;
                Paragraph aiVisibleCommentAboutFeeling = aiVisibleCommentBuilder.Build();
                mainWindow.AddToFeelings(aiVisibleCommentAboutFeeling);
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// Answer to an atypical human statement
        /// </summary>
        /// <param name="statement">atypical human statement</param>
        /// <param name="preAlterationStatementCorrectness">whether the statement was correct (true) before being told</param>
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
                AnswerToStatementUpdate(statement);
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
                AnswerToStatementWhy(statement, false);
            else if (statement.IsAskingVisualizeWhy)
            {
                mainWindow.tabControl.SelectedIndex = 1;
                AnswerToStatementWhy(statement, true);
                return;
            }
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

        /// <summary>
        /// The Ai answers with an exception message
        /// </summary>
        /// <param name="exceptionMessage">provided exception message</param>
        private void AnswerExceptionMessage(string exceptionMessage)
        {
            aiVisibleCommentBuilder.AiName = aiName.Value;
            aiVisibleCommentBuilder.ExceptionMessage = exceptionMessage;
            Paragraph visibleComment = aiVisibleCommentBuilder.Build();
            mainWindow.AddToOutputText(visibleComment);
        }

        /// <summary>
        /// The Ai answers to a visualization statement by calling the MainWindow's visualizer upon a concept
        /// </summary>
        /// <param name="statement">human visualize statement</param>
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
                connectionsNoWhy = definitionManager.DefinitionIdsToStrings(connectionsNoWhyById,brain,nameMapper);
            }
            else
            {
                definitionByConceptId = brain.GetShortDefinition(conceptId);
            }

            definition = definitionManager.DefinitionIdsToStrings(definitionByConceptId, brain, nameMapper);

            #region We sort the definition
            if (mainWindow.IsVisualizerShowingLongDefinition)
                definition = definitionSorter.SortComplementsByProofLength(definition, brain.GetConceptToLookIntoIt(conceptId, this), nameMapper, brain.GetUnRepairedMemory(mainWindow));
            definition = definitionSorter.SortByCustomOrder(definition);
            #endregion

            mainWindow.VisualizeConcept(nameMapper.InvertYouAndMePov(conceptName), definition, connectionsNoWhy);
        }

        /// <summary>
        /// The Ai answers with a StatCross statistical expression
        /// </summary>
        /// <param name="statement">human statCross statement</param>
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

        /// <summary>
        /// The Ai answers with a list of selected concept
        /// </summary>
        /// <param name="statement">select statement provided by human</param>
        private void AnswerToStatementSelect(Statement statement)
        {
            HashSet<int> selection = brain.Select(statement.SelectCondition, nameMapper, true);
            HashSet<string> selectionByName = new HashSet<string>();
            string conceptName;
            foreach (int conceptId in selection)
            {
                conceptName = nameMapper.GetConceptNames(conceptId)[0];
                selectionByName.Add(conceptName);
            }
            aiVisibleCommentBuilder.Selection = selectionByName;
        }

        /// <summary>
        /// The Ai answers with the list of concepts affected by human update statement
        /// </summary>
        /// <param name="statement">human update statement</param>
        private void AnswerToStatementUpdate(Statement statement)
        {
            HashSet<int> selection = brain.Select(statement.SelectCondition, nameMapper, false);
            HashSet<string> selectionByName = new HashSet<string>();
            string conceptName;
            foreach (int conceptId in selection)
            {
                conceptName = nameMapper.InvertYouAndMePov(nameMapper.GetConceptNames(conceptId)[0]);
                selectionByName.Add(conceptName);
            }
            ParseGeneralAffectation(selectionByName, statement.UpdateAction);
            aiVisibleCommentBuilder.Selection = selectionByName;
        }

        /// <summary>
        /// The Ai will answer to Imply statement by telling whether "imply" connection was
        /// added, removed or left in current state
        /// </summary>
        /// <param name="statement">human imply statement</param>
        /// <param name="preAlterationStatementCorrectness">whether the statement was correct before being processed</param>
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

        /// <summary>
        /// The Ai will negate question it was asking to human
        /// </summary>
        /// <param name="statement">human no statement</param>
        private void AnswerToStatementNo(Statement statement)
        {
            brain.DiscardCurrentTheory();
            aiVisibleCommentBuilder.IsDiscardingTheory = true;
        }

        /// <summary>
        /// The Ai will consider question it was asking to human to be true
        /// </summary>
        /// <param name="statement">human yes statement</param>
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
            trauma = alterator.TryAlterBrainFromStatement(statement);

            if (trauma != null)
                AnswerWithTrauma(trauma);
            else if (statement.MetaOperatorName == null)
                AnswerToStatementOperation(statement);
            else
                AnswerToStatementMetaOperation(statement);
        }

        /// <summary>
        /// The Ai will answer with an analogy
        /// </summary>
        /// <param name="statement">human analogy request statement</param>
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

        /// <summary>
        /// The Ai will answer by acknowledging to the human statement
        /// </summary>
        /// <param name="statement">human statement to acknowledge to</param>
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

        /// <summary>
        /// The Ai will answer to human operation statement by affirmation, negation or uncertainity
        /// depending on his knowledge
        /// </summary>
        /// <param name="statement">human operation statement</param>
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

        /// <summary>
        /// The Ai will answer to human metaOperation statement by affirmation, negation or uncertainity
        /// depending on his knowledge
        /// </summary>
        /// <param name="statement">human metaOperation statement</param>
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

        /// <summary>
        /// The Ai will answer to human's "why" statement with a proof or a graphical proof
        /// </summary>
        /// <param name="statement">human's "why" statement</param>
        /// <param name="graphicalMode">whether human wants a graphical answer or not</param>
        private void AnswerToStatementWhy(Statement statement, bool graphicalMode)
        {
            List<string> proofDefinitionList = new List<string>();

            if (brain.TestConnection(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                                  nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)),
                                  nameMapper.GetOrCreateConceptId(statement.GetConceptName(2))))
            {
                #region If connection exist
                if (statement.IsNegative)
                {
                    #region If human assumed connection didn't exist
                    List<List<int>> proof = brain.GetProofToConnection(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                                                                       nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)),
                                                                       nameMapper.GetOrCreateConceptId(statement.GetConceptName(2)));


                    foreach (List<int> argument in proof)
                        proofDefinitionList.Add(nameMapper.GetConceptNames(argument[0])[0] + " " + nameMapper.GetConceptNames(argument[1])[0] + " " + nameMapper.GetConceptNames(argument[2])[0]);

                    if (proof.Count == 0)
                        proofDefinitionList.Add("someone told me");

                    proofDefinitionList.Add(nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(1)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(2)));

                    if (graphicalMode)
                        mainWindow.VisualizeProof(proofDefinitionList);
                    else
                        aiVisibleCommentBuilder.ProofDefinitionListPositiveRefutation = proofDefinitionList;
                    #endregion
                }
                else
                {
                    #region If human assumed connection did exist

                    List<List<int>> proof = brain.GetProofToConnection(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                                                                       nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)),
                                                                       nameMapper.GetOrCreateConceptId(statement.GetConceptName(2)));


                    foreach (List<int> argument in proof)
                        proofDefinitionList.Add(nameMapper.GetConceptNames(argument[0])[0] + " " + nameMapper.GetConceptNames(argument[1])[0] + " " + nameMapper.GetConceptNames(argument[2])[0]);

                    if (proof.Count == 0)
                        proofDefinitionList.Add("someone told me");

                    proofDefinitionList.Add(nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(1)) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(2)));

                    if (graphicalMode)
                        mainWindow.VisualizeProof(proofDefinitionList);
                    else
                        aiVisibleCommentBuilder.ProofDefinitionListRegular = proofDefinitionList;
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

                        List<List<int>> refutation = brain.GetProofToConnection(obstruction[0], obstruction[1], obstruction[2]);

                        foreach (List<int> argument in refutation)
                            proofDefinitionList.Add(nameMapper.GetConceptNames(argument[0])[0] + " " + nameMapper.GetConceptNames(argument[1])[0] + " " + nameMapper.GetConceptNames(argument[2])[0]);

                        proofDefinitionList.Add(obstructionByString[0] + " " + obstructionByString[1] + " " + obstructionByString[2]);
                        proofDefinitionList.Add(nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " " + statement.GetConceptName(1) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(2)));

                        if (graphicalMode)
                            mainWindow.VisualizeProof(proofDefinitionList);
                        else
                            aiVisibleCommentBuilder.ProofDefinitionListRefutation = proofDefinitionList;
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
                        
                        List<List<int>> refutation = brain.GetProofToConnection(obstruction[0], obstruction[1], obstruction[2]);

                        foreach (List<int> argument in refutation)
                            proofDefinitionList.Add(nameMapper.GetConceptNames(argument[0])[0] + " " + nameMapper.GetConceptNames(argument[1])[0] + " " + nameMapper.GetConceptNames(argument[2])[0]);

                        proofDefinitionList.Add(obstructionByString[0] + " " + obstructionByString[1] + " " + obstructionByString[2]);
                        proofDefinitionList.Add(nameMapper.InvertYouAndMePov(statement.GetConceptName(0)) + " " + statement.GetConceptName(1) + " " + nameMapper.InvertYouAndMePov(statement.GetConceptName(2)));

                        if (graphicalMode)
                            mainWindow.VisualizeProof(proofDefinitionList);
                        else
                            aiVisibleCommentBuilder.ProofDefinitionListRefutation = proofDefinitionList;
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
        }

        /// <summary>
        /// The Ai will answer with a short "whatis" definition
        /// </summary>
        /// <param name="statement">human's "whatis" question statement</param>
        private void AnswerToStatementWhatis(Statement statement)
        {
            string conceptName = statement.GetConceptName(0);
            int conceptId = nameMapper.GetOrCreateConceptId(conceptName);

            aiVisibleCommentBuilder.DefinitionListWhatis = definitionManager.GetDefinitionWhatis(conceptId, brain,nameMapper);
            aiVisibleCommentBuilder.DefinitionListWhatis = definitionSorter.SortByCustomOrder(aiVisibleCommentBuilder.DefinitionListWhatis);
            aiVisibleCommentBuilder.DefinitionListPositiveImply = GetDefinitionListImply(conceptId, true);
            aiVisibleCommentBuilder.DefinitionListNegativeImply = GetDefinitionListImply(conceptId, false);
            aiVisibleCommentBuilder.ConceptToDefine = nameMapper.InvertYouAndMePov(statement.GetConceptName(0));
        }

        /// <summary>
        /// The Ai will answer with a long "define" definition
        /// </summary>
        /// <param name="statement">human's "define" question statement</param>
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

            aiVisibleCommentBuilder.DefinitionListDefine = definitionSorter.SortComplementsByProofLength(aiVisibleCommentBuilder.DefinitionListDefine, brain.GetConceptToLookIntoIt(conceptId, this), nameMapper, brain.GetUnRepairedMemory(mainWindow));
            aiVisibleCommentBuilder.DefinitionListDefine = definitionSorter.SortByCustomOrder(aiVisibleCommentBuilder.DefinitionListDefine);
            aiVisibleCommentBuilder.DefinitionListPositiveImply = GetDefinitionListImply(conceptId, true);
            aiVisibleCommentBuilder.DefinitionListNegativeImply = GetDefinitionListImply(conceptId, false);
            aiVisibleCommentBuilder.ConceptToDefine = nameMapper.InvertYouAndMePov(statement.GetConceptName(0));
        }

        /// <summary>
        /// The Ai will answer to the human's requestion to associate namess
        /// </summary>
        /// <param name="statement">human's naming association request statement</param>
        private void AnswerToStatementNamingAssociation(Statement statement)
        {
            if (statement.IsNegative)
                aiVisibleCommentBuilder.StatementAcknowledgeAndLearn = statement.GetConceptName(0) + " not " + statement.NamingAssociationOperatorName + " " + statement.GetConceptName(1);
            else
                aiVisibleCommentBuilder.StatementAcknowledgeAndLearn = statement.GetConceptName(0) + " " + statement.NamingAssociationOperatorName + " " + statement.GetConceptName(1);
        }

        /// <summary>
        /// The Ai will teach something about a concept selected by human
        /// </summary>
        /// <param name="statement">human teachabout request statement</param>
        private void AnswerToStatementTeachAbout(Statement statement)
        {
            int conceptId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));

            KeyValuePair<List<int>, List<List<int>>> connectionAndProof = brain.TeachAbout(conceptId);
            List<int> connection = connectionAndProof.Key;

            aiVisibleCommentBuilder.ProofDefinitionListTeachabout = new List<string>();
            foreach (List<int> argument in connectionAndProof.Value)
                aiVisibleCommentBuilder.ProofDefinitionListTeachabout.Add(nameMapper.GetConceptNames(argument[0])[0] + " " + nameMapper.GetConceptNames(argument[1])[0] + " " + nameMapper.GetConceptNames(argument[2])[0]);

            aiVisibleCommentBuilder.ProofDefinitionListTeachabout.Add(nameMapper.GetConceptNames(connection[0])[0] + " " + nameMapper.GetConceptNames(connection[1])[0] + " " + nameMapper.GetConceptNames(connection[2])[0]);
        }

        /// <summary>
        /// The Ai will teach something about a concept selected by himself
        /// </summary>
        /// <param name="statement">human teach request statement</param>
        private void AnswerToStatementTeach(Statement statement)
        {
            KeyValuePair<List<int>, List<List<int>>> connectionAndProof = brain.TeachAboutRandomConcept();
            List<int> connection = connectionAndProof.Key;

            aiVisibleCommentBuilder.ProofDefinitionListTeachabout = new List<string>();
            foreach (List<int> argument in connectionAndProof.Value)
                aiVisibleCommentBuilder.ProofDefinitionListTeachabout.Add(nameMapper.GetConceptNames(argument[0])[0] + " " + nameMapper.GetConceptNames(argument[1])[0] + " " + nameMapper.GetConceptNames(argument[2])[0]);

            aiVisibleCommentBuilder.ProofDefinitionListTeachabout.Add(nameMapper.GetConceptNames(connection[0])[0] + " " + nameMapper.GetConceptNames(connection[1])[0] + " " + nameMapper.GetConceptNames(connection[2])[0]);
        }

        /// <summary>
        /// The Ai will ask something about a concept provided by human
        /// </summary>
        /// <param name="statement">human statement asking the ai to ask something about a concept</param>
        private void AnswerToStatementAskAbout(Statement statement)
        {
            int conceptId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));
            List<int> question = brain.GetBestQuestionAbout(conceptId);
            aiVisibleCommentBuilder.DefinitionListWhatis = definitionManager.GetDefinitionWhatis(question[0], brain, nameMapper);
            aiVisibleCommentBuilder.DefinitionListPositiveImply = GetDefinitionListImply(question[0], true);
            aiVisibleCommentBuilder.DefinitionListNegativeImply = GetDefinitionListImply(question[0], false);
            aiVisibleCommentBuilder.ConceptToDefine = nameMapper.GetConceptNames(question[0])[0];
            aiVisibleCommentBuilder.StatementStubAskingForRandomConnection = nameMapper.GetConceptNames(question[0])[0] + " " + nameMapper.GetConceptNames(question[1])[0];
        }

        /// <summary>
        /// The Ai will ask something about a random concept
        /// </summary>
        /// <param name="statement">human statement asking the ai
        /// to ask something about a random concept</param>
        private void AnswerToStatementAsk(Statement statement)
        {
            List<int> question = brain.GetBestQuestionAboutRandomConcept();
            aiVisibleCommentBuilder.DefinitionListWhatis = definitionManager.GetDefinitionWhatis(question[0],brain, nameMapper);
            aiVisibleCommentBuilder.DefinitionListPositiveImply = GetDefinitionListImply(question[0], true);
            aiVisibleCommentBuilder.DefinitionListNegativeImply = GetDefinitionListImply(question[0], false);
            aiVisibleCommentBuilder.ConceptToDefine = nameMapper.GetConceptNames(question[0])[0];
            aiVisibleCommentBuilder.StatementStubAskingForRandomConnection = nameMapper.GetConceptNames(question[0])[0] + " " + nameMapper.GetConceptNames(question[1])[0];
        }

        /// <summary>
        /// The human will answer by telling a connection theory about concept provided by human
        /// The theory will be based on statistical inference on existing connections
        /// </summary>
        /// <param name="statement">human statement asking Ai
        /// to make a theory about a specific concept</param>
        private void AnswerToStatementThinkAbout(Statement statement)
        {
            int conceptId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));
            aiVisibleCommentBuilder.Theory = brain.GetBestTheoryAboutConcept(conceptId, nameMapper);
        }

        /// <summary>
        /// The human will answer by telling a connection theory about concept provided by human
        /// The theory will be based on contextual semantic proximity
        /// </summary>
        /// <param name="statement">human statement asking Ai
        /// to make a theory about a specific concept</param>
        private void AnswerToStatementLinguisThinkAbout(Statement statement)
        {
            int conceptId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));
            aiVisibleCommentBuilder.Theory = brain.GetBestLinguisticTheoryAboutConcept(conceptId, nameMapper);
        }

        /// <summary>
        /// The human will answer by telling a connection theory about concept provided by human
        /// The theory will be based on concept name phonology
        /// </summary>
        /// <param name="statement">human statement asking Ai
        /// to make a theory about a specific concept</param>
        private void AnswerToStatementPhonoThinkAbout(Statement statement)
        {
            int conceptId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));
            aiVisibleCommentBuilder.Theory = brain.GetBestPhoneticTheoryAboutConcept(conceptId, nameMapper);
        }

        /// <summary>
        /// The human will answer by telling a metaConnection theory about operator provided by human
        /// The theory will be based on statistical inference on existing connections
        /// </summary>
        /// <param name="statement">human statement asking Ai
        /// to make a theory about a specific operator</param>
        private void AnswerToStatementMetaThinkAbout(Statement statement)
        {
            int conceptId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));
            aiVisibleCommentBuilder.Theory = brain.GetBestTheoryAboutOperator(conceptId, nameMapper);
        }

        /// <summary>
        /// The human will answer by telling a connection theory about a random concept
        /// The theory will be based on statistical inference on existing connections
        /// </summary>
        /// <param name="statement">human statement asking Ai
        /// to make a theory about a random concept</param>
        private void AnswerToStatementThink(Statement statement)
        {
            aiVisibleCommentBuilder.Theory = brain.GetBestTheoryAboutRandomConcept(nameMapper);
        }

        /// <summary>
        /// The human will answer by telling a connection theory about a random concept
        /// The theory will be based on contextual semantic proximity
        /// </summary>
        /// <param name="statement">human statement asking Ai
        /// to make a theory about a random concept</param>
        private void AnswerToStatementLinguisThink(Statement statement)
        {
            aiVisibleCommentBuilder.Theory = brain.GetBestLinguisticTheoryAboutRandomConcept(nameMapper);
        }

        /// <summary>
        /// The human will answer by telling a connection theory about a random concept
        /// The theory will be based on concept name phonology
        /// </summary>
        /// <param name="statement">human statement asking Ai
        /// to make a theory about a random concept</param>
        private void AnswerToStatementPhonoThink(Statement statement)
        {
            aiVisibleCommentBuilder.Theory = brain.GetBestPhoneticTheoryAboutRandomConcept(nameMapper);
        }

        /// <summary>
        /// The human will answer by telling a metaConnection theory about a random operator
        /// The theory will be based on statistical inference on existing connections
        /// </summary>
        /// <param name="statement">human statement asking Ai
        /// to make a theory about a random operator</param>
        private void AnswerToStatementMetaThink(Statement statement)
        {
            aiVisibleCommentBuilder.Theory = brain.GetBestTheoryAboutRandomOperator(nameMapper);
        }

        /// <summary>
        /// The Ai will select a random behavior and apply it to concept provided by human
        /// </summary>
        /// <param name="statement">human statement asking ai to talk about a specific concept</param>
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

            for (int i = 0; i < answerList.Count; )
            {
                try
                {
                    answerList[i](statement);
                    break;
                }
                catch (Exception)
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// The Ai will select a random behavior and apply it to randomconcept
        /// </summary>
        /// <param name="statement">human statement asking ai to talk about a random concept</param>
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


            for (int i = 0; i < answerList.Count; )
            {
                try
                {
                    answerList[i](statement);
                    break;
                }
                catch (Exception)
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// The Ai will consider all theories to be true when expected probability is
        /// above a threshold
        /// </summary>
        /// <param name="statement">human asking Ai to go crazy</param>
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

        /// <summary>
        /// Returns an analogy from subject, verb and complement names
        /// </summary>
        /// <param name="subjectName">subject name</param>
        /// <param name="verbName">verb name</param>
        /// <param name="complementName">complement name</param>
        /// <returns>analogy about subject, verb and complement</returns>
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

        private Dictionary<string, List<string>> GetDefinitionListImply(int verbId, bool isPositive)
        {
            return brain.GetDefinitionListImply(verbId, nameMapper, isPositive);
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
        #endregion
    }
}
