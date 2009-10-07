using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using System.Windows.Documents;
using System.ComponentModel;
using System.Threading;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents the program's main controller. It is the Ai program instance
    /// as a whole. It also represents all the use cases.
    /// </summary>
    class Ai
    {
        #region Static
        /// <summary>
        /// Default Ai name
        /// </summary>
        private static readonly string defaultAiName = "riasce";

        /// <summary>
        /// Default human name
        /// </summary>
        private static readonly string defaultHumanName = "someone";
        #endregion

        #region Fields and parts
        /// <summary>
        /// Represents the current changeable Ai name
        /// </summary>
        private Name aiName;

        /// <summary>
        /// Represents the current changeable human name
        /// </summary>
        private Name humanName;

        private Brain brain;

        private Answerer answerer;
        
        private AbstractHumanVisibleCommentBuilder humanVisibleCommentBuilder;

        private AbstractAiVisibleCommentBuilder aiVisibleCommentBuilder;

        private StatementListFactory statementListFactory;

        private NameMapper nameMapper;

        private MainWindow mainWindow;

        private SaverLoader saverLoader;

        private Conversation conversation;

        private CategoryListExtractor categoryListExtractor;

        private CategoryListSorterTrimmer categoryListSorterTrimmer;

        private CategoryExtractor categoryExtractor;

        private WordMatrixExtractor wordMatrixExtractor = new WordMatrixExtractor();

        private PhoneticMatrixManager phoneticMatrixManager = new PhoneticMatrixManager();

        private XmlMatrixSaverLoader xmlMatrixSaverLoader = new XmlMatrixSaverLoader();

        private SemanticLikenessMatrixBuilder semanticLikenessMatrixBuilder = new SemanticLikenessMatrixBuilder();
        #endregion

        #region Constructors
        public Ai()
        {
            aiName = new Name(defaultAiName);
            humanName = new Name(defaultHumanName);
            brain = new Brain();
            mainWindow = new MainWindow(brain.DisambiguationNamer);
            saverLoader = new SaverLoader();
            categoryListSorterTrimmer = new CategoryListSorterTrimmer();
            wordMatrixExtractor = new WordMatrixExtractor();
            phoneticMatrixManager = new PhoneticMatrixManager();
            xmlMatrixSaverLoader = new XmlMatrixSaverLoader();
            semanticLikenessMatrixBuilder = new SemanticLikenessMatrixBuilder();
            humanVisibleCommentBuilder = new XamlHumanVisibleCommentBuilder();
            humanVisibleCommentBuilder.HumanName = humanName.Value;
            aiVisibleCommentBuilder = new XamlAiVisibleCommentBuilder();
            aiVisibleCommentBuilder.AiName = aiName.Value;
            statementListFactory = new StatementListFactory();
            nameMapper = new NameMapper(aiName, humanName);

            mainWindow.RefreshAutoCompleteFrom(brain.GetUnRepairedMemory(mainWindow), nameMapper);
            mainWindow.SetMemoryTreeViewBinding(brain.GetUnRepairedMemory(mainWindow), nameMapper);

            Alterator alterator = new Alterator(brain, nameMapper);
            answerer = new Answerer(brain, nameMapper, mainWindow, alterator, aiVisibleCommentBuilder, statementListFactory, aiName, humanName);

            mainWindow.Title = GetCurrentProgramTitle();

            AddEventHandlers(mainWindow);
            humanVisibleCommentBuilder.UserClickConcept += UserClickConceptHandler;
            aiVisibleCommentBuilder.UserClickConcept += UserClickConceptHandler;
        }

        /// <summary>
        /// Add events to the main window
        /// </summary>
        /// <param name="mainWindow">main window to add event handlers to</param>
        private void AddEventHandlers(MainWindow mainWindow)
        {
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
            mainWindow.UserExtractCategoryListFromWiki += UserStartExtractingCategoryListFromWiki;
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
            mainWindow.UserScanMemory += UserScanMemoryHandler;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Start the AI program and start background thinker
        /// </summary>
        public void Start()
        {
            Application application = new Application();
            application.Run(mainWindow);
            StartBackgroundThinker();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// When the AI listens to a pre-parsed statement,
        /// the ai performs actions about it and answer to human
        /// </summary>
        /// <param name="statement">pre-parsed statement from the human</param>
        private void ListenTo(Statement statement)
        {
            answerer.ChooseDecisionFromStatement(statement);

            //If the statement can modify memory
            if (!statement.IsInterrogative && !statement.IsAnalogy && !statement.IsStatCross && !statement.IsSelect && !statement.IsUpdate && (statement.GetConceptName(2) != null || statement.MetaOperatorName != null))
            {
                saverLoader.FileNeedSave = true;
                mainWindow.Title = GetCurrentProgramTitle();
            }
        }

        /// <summary>
        /// Allow the human to change his name (logoff + login)
        /// </summary>
        private void ResetHumanName()
        {
            string newName = mainWindow.GetNewHumanNameFromInputBox(humanName.Value);
            if (newName != "" && newName != null)
                humanName.Value = newName;
        }

        /// <summary>
        /// Allow the human to load a script file
        /// </summary>
        private void AllowUserToLoadScriptFile()
        {
            string fileName = mainWindow.GetInputFile();

            if (fileName != null)
                RunScript(fileName);
        }

        /// <summary>
        /// Allow the human to load a memory file and load it if it's valid
        /// </summary>
        private void TryLoadMemoryFile()
        {
            saverLoader.FileName = mainWindow.GetInputFile(saverLoader.DefaultFileTypeFilter);

            if (saverLoader.FileName != null)
            {
                VerbMetaConnectionCache.Clear();
                RepairedFlatBranchCache.Clear();
                brain.ResetBackgroundThinkerTheoryMemory();
                saverLoader.Load();
                Memory.TotalVerbList = saverLoader.TotalVerbList;
                brain.SetMemoryToLoad(saverLoader.Memory, saverLoader);
                nameMapper = saverLoader.NameMapper;
                brain.SetRejectedTheoriesToLoad(saverLoader.RejectedTheories, saverLoader);
                humanName.Value = nameMapper.HumanName.Value;
                aiName.Value = nameMapper.AiName.Value;
                nameMapper.HumanName = humanName;
                nameMapper.AiName = aiName;
                mainWindow.RefreshAutoCompleteFrom(brain.GetUnRepairedMemory(mainWindow), this.nameMapper);
                mainWindow.SetMemoryTreeViewBinding(brain.GetUnRepairedMemory(mainWindow), nameMapper);
                mainWindow.VerbNameListForHelp = nameMapper.GetTotalVerbNameList(brain.GetUnRepairedMemory(mainWindow));
                saverLoader.FileNeedSave = false;
                Alterator alterator = new Alterator(brain, nameMapper);
                answerer = new Answerer(brain, nameMapper, mainWindow, alterator, aiVisibleCommentBuilder, statementListFactory, aiName, humanName);
                mainWindow.Title = GetCurrentProgramTitle();

                //Ugly hack: we reload memory as long as there are less inconsistency than last time
                int inconsistencyCount = -1, previousInconsistencyCount = -1;
                int tryCount = 0;
                do
                {
                    previousInconsistencyCount = inconsistencyCount;
                    inconsistencyCount = brain.RepairAndScanMemoryInconsistencyCount();
                    tryCount++;
                } while (inconsistencyCount > 0 && (previousInconsistencyCount == -1 || inconsistencyCount < previousInconsistencyCount));
                mainWindow.AddToOutputText("Total inconsistencies found in memory: " + inconsistencyCount + ", total try: " + tryCount);
            }
        }

        /// <summary>
        /// Start new Ai memory from scratch, forget everything
        /// </summary>
        private void ResetMemory()
        {
            VerbMetaConnectionCache.Clear();
            RepairedFlatBranchCache.Clear();
            saverLoader.FileName = null;
            saverLoader.FileNeedSave = false;
            mainWindow.Title = GetCurrentProgramTitle();
            Memory.TotalVerbList = new HashSet<Concept>();
            brain.SetMemoryToLoad(new Memory(),saverLoader);
            nameMapper = new NameMapper(aiName, humanName);
            brain.ResetBackgroundThinkerTheoryMemory();
            brain.ResetRejectedTheories();
            mainWindow.RefreshAutoCompleteFrom(brain.GetUnRepairedMemory(mainWindow), this.nameMapper);
            mainWindow.SetMemoryTreeViewBinding(brain.GetUnRepairedMemory(mainWindow), nameMapper);
            mainWindow.VerbNameListForHelp = nameMapper.GetTotalVerbNameList(brain.GetUnRepairedMemory(mainWindow));
            Alterator alterator = new Alterator(brain, nameMapper);
            answerer = new Answerer(brain, nameMapper, mainWindow, alterator, aiVisibleCommentBuilder, statementListFactory, aiName, humanName);
        }

        /// <summary>
        /// Save if it's possible, or save as if not
        /// </summary>
        private void AllowUserToSaveOrSaveAs()
        {
            string currentFileName = saverLoader.FileName;

            if (saverLoader.FileName == null)
                currentFileName = mainWindow.GetOutputFile(saverLoader.DefaultFileTypeFilter);

            if (currentFileName != null)
            {
                saverLoader.FileName = currentFileName;
                SaveMemoryToFile(saverLoader.FileName);
            }
        }

        /// <summary>
        /// Save ai memory to file
        /// </summary>
        /// <param name="fileName">file's name</param>
        private void SaveMemoryToFile(string fileName)
        {
            if (fileName != null)
            {
                if (brain.RepairAndScanMemoryGetOutput(nameMapper) == null || mainWindow.GetBoolInput("Warning", "Warning, inconsistent connections exist in memory. You you wish to save anyway?"))
                {
                    saverLoader.FileName = fileName;
                    saverLoader.Memory = brain.GetUnRepairedMemory(saverLoader);
                    saverLoader.NameMapper = nameMapper;
                    saverLoader.TotalVerbList = Memory.TotalVerbList;
                    saverLoader.RejectedTheories = brain.GetRejectedTheoriesToSave(saverLoader);
                    saverLoader.TotalTheoryList = brain.GetTheoryListToSave();
                    saverLoader.Save();
                    saverLoader.FileName = fileName;//we set file name again for next save
                    saverLoader.FileNeedSave = false;
                    mainWindow.Title = GetCurrentProgramTitle();
                }
            }
        }

        /// <summary>
        /// Run script file (something similar to a batch file or a SH file) but with HIMML code
        /// </summary>
        /// <param name="fileName">file name</param>
        private void RunScript(string fileName)
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
                        ListenTo(statement);
                }
            } while (line != null);

            handle.Close();

            mainWindow.RefreshAutoCompleteFrom(brain.GetUnRepairedMemory(mainWindow), nameMapper);
        }

        /// <summary>
        /// Deploy connections from provided instinct
        /// </summary>
        /// <param name="instinct">instinct</param>
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
                    ListenTo(statement);
                    statementCounter++;
                }
                lineCounter++;
            }
        }

        /// <summary>
        /// Returns the program's current title (file name, * if need to save) to show it in the GUI
        /// </summary>
        /// <returns>program's current title to show it in the GUI</returns>
        private string GetCurrentProgramTitle()
        {
            string title = "";
            if (saverLoader.FileName == null)
                title += "untitled.xml";
            else
                title += saverLoader.FileName;

            if (title.Contains('\\'))
                title = title.Substring(title.LastIndexOf('\\') + 1);

            if (saverLoader.FileNeedSave)
                title += "*";

            title += " - Riasce";

            return title;
        }

        /// <summary>
        /// Start background thinker
        /// </summary>
        private void StartBackgroundThinker()
        {
            brain.StartBackgroundThinker(nameMapper);
        }

        /// <summary>
        /// Brain stop background thinker and answer if found trauma
        /// </summary>
        private void StopBackgroundThinker()
        {
            Stack<Trauma> traumaStack = brain.StopBackgroundThinker();

            while (traumaStack.Count > 0)
                answerer.AnswerWithTrauma(traumaStack.Pop());
        }
        #endregion
        
        #region Handlers
        /// <summary>
        /// When the human sends comment to the AI (main use case)
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
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
                    ListenTo(statement);
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
                    ListenTo(conversation.GetCurrentStatementForAiToAnswerTo());

                answerer.TellAboutFeelings();
                StartBackgroundThinker();
            }
        }

        /// <summary>
        /// Handle use case when user wants to exit program
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserWantsToExitHandler(object sender, CancelEventArgs e)
        {
            StopBackgroundThinker();

            if (saverLoader.FileNeedSave)
            {
                MessageBoxResult result = mainWindow.AskToSave();
                if (result == MessageBoxResult.Yes)
                {
                    UserSaveFileHandler(this, new EventArgs());
                    if (saverLoader.FileNeedSave)
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

        /// <summary>
        /// When user wants to switch user
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserSignOutHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();
            ResetHumanName();
            StartBackgroundThinker();
        }

        /// <summary>
        /// When user wants to load a script file
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserRunScriptHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();
            AllowUserToLoadScriptFile();
            StartBackgroundThinker();
        }

        /// <summary>
        /// When user wants to load memory from file
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserOpenFileHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            if (saverLoader.FileNeedSave)
            {
                MessageBoxResult result = mainWindow.AskToSave();
                if (result == MessageBoxResult.Yes)
                {
                    AllowUserToSaveOrSaveAs();
                    if (saverLoader.FileNeedSave)
                        return;
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            TryLoadMemoryFile();
            StartBackgroundThinker();
        }

        /// <summary>
        /// When user wants to clean memory and start from scratch
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserFileNewHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            if (saverLoader.FileNeedSave)
            {
                MessageBoxResult result = mainWindow.AskToSave();
                if (result == MessageBoxResult.Yes)
                {
                    UserSaveFileHandler(this, new EventArgs());
                    if (saverLoader.FileNeedSave)
                        return;
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            ResetMemory();
            mainWindow.ResetTextFields();

            //brain.StartBackgroundThinker(nameMapper);
        }
        
        /// <summary>
        /// When user wants to save memory to a current file or new file
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserSaveFileHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();
            AllowUserToSaveOrSaveAs();
            StartBackgroundThinker();
        }

        /// <summary>
        /// When user wants to save memory to a new file
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserSaveAsFileHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            string fileName = mainWindow.GetOutputFile(saverLoader.DefaultFileTypeFilter);

            if (fileName != null)
            {
                saverLoader.FileName = fileName;
                SaveMemoryToFile(fileName);
            }

            StartBackgroundThinker();
        }

        /// <summary>
        /// When user wants to view help
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserViewHelpHandler(object sender, EventArgs e)
        {
            mainWindow.tabControl.SelectedIndex = 2;
        }

        /// <summary>
        /// When user wants to start conversation
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserConversationStartHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            conversation = new Conversation();
            ListenTo(conversation.GetCurrentStatementForAiToAnswerTo());

            StartBackgroundThinker();
        }

        /// <summary>
        /// When user wants to stop conversation
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserConversationStopHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            conversation = null;

            StartBackgroundThinker();
        }

        /// <summary>
        /// When user wants to load standard instinct
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserStandardInstinctHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            StandardInstinct standardInstinct = new StandardInstinct();
            DeployInstinct(standardInstinct);

            StartBackgroundThinker();
        }

        /// <summary>
        /// When user wants to load standard ego
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserEgoHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            Ego ego = new Ego();
            DeployInstinct(ego);

            StartBackgroundThinker();
        }

        /// <summary>
        /// When user click on a concept to visualize it
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
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
                ListenTo(statement);

            StartBackgroundThinker();
        }

        /// <summary>
        /// Start extracting wikipedia category list
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserStartExtractingCategoryListFromWiki(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            categoryListExtractor = new CategoryListExtractor();

            categoryListExtractor.FileName = mainWindow.GetOutputFile("Raw Category List|*.RawCategoryList",false);
            if (categoryListExtractor.FileName == null)
                return;

            categoryListExtractor.Show();

            Thread thread = new Thread(categoryListExtractor.Start);
            thread.Start();

            StartBackgroundThinker();
        }

        /// <summary>
        /// User wants to sort an extracte wiki category list
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserSortCategoryListHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            string inputFileName = mainWindow.GetInputFile("Raw Category List|*.RawCategoryList");

            if (inputFileName == null)
                return;

            string outputFileName = mainWindow.GetOutputFile("Sorted Category List|*.SortedCategoryList");

            if (outputFileName != null)
                categoryListSorterTrimmer.SortAndTrimCategoryListFile(inputFileName, outputFileName);

            StartBackgroundThinker();
        }

        /// <summary>
        /// Open category extractor to parse extracted category and teach the Ai
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserOpenCategoryExtractorHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            categoryExtractor = new CategoryExtractor();
            categoryExtractor.OnGeneralAffectation += CategoryExtractorGeneralAffectationHandler;
            categoryExtractor.Start();

            StartBackgroundThinker();
        }

        /// <summary>
        /// Allow user to extract occurent matrix from text file
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
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

            StartBackgroundThinker();
        }

        /// <summary>
        /// Allow user to extract phonetic matrix from text file
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
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

            StartBackgroundThinker();
        }

        /// <summary>
        /// Allow user to convert occurent matrix to semantic matrix
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
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

            StartBackgroundThinker();
        }

        /// <summary>
        /// Parse a general affectation list from a wikipedia category
        /// in order to uniformally affect connections to a long list of similar concepts
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void CategoryExtractorGeneralAffectationHandler(object sender, EventArgs e)
        {
            StopBackgroundThinker();

            HashSet<string> affectedConceptNameList = categoryExtractor.CategoryElementNameList;            
            string generalAffectationStatement = categoryExtractor.GeneralAffectation;
            answerer.ParseGeneralAffectation(affectedConceptNameList, generalAffectationStatement);

            StartBackgroundThinker();
        }

        /// <summary>
        /// Visualize concept that got clicked on
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void VisualizerClickConceptHandler(object sender, StringEventArgs e)
        {
            StopBackgroundThinker();

            List<Statement> statementList = statementListFactory.GetInterpretedHumanStatementList(humanName.Value, "visualize " + nameMapper.InvertYouAndMePov(e.Name));

            foreach (Statement statement in statementList)
                ListenTo(statement);

            StartBackgroundThinker();
        }

        /// <summary>
        /// Ask "why" according to a "why [connection]" link that got clicked on
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void VisualizerClickWhyLinkHandler(object sender, StringEventArgs e)
        {
            StopBackgroundThinker();

            string[] words = e.Name.Split(' ');

            List<Statement> statementList = statementListFactory.GetInterpretedHumanStatementList(humanName.Value, "why " + nameMapper.InvertYouAndMePov(words[0]) + " " + words[1] + " " + nameMapper.InvertYouAndMePov(words[2]));

            foreach (Statement statement in statementList)
                ListenTo(statement);

            StartBackgroundThinker();
        }

        /// <summary>
        /// Define a link according to a "define" link that got clicked on
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void VisualizerClickDefineLinkHandler(object sender, StringEventArgs e)
        {
            StopBackgroundThinker();

            List<Statement> statementList = statementListFactory.GetInterpretedHumanStatementList(humanName.Value, "define " + nameMapper.InvertYouAndMePov(e.Name));

            foreach (Statement statement in statementList)
                ListenTo(statement);

            StartBackgroundThinker();
        }

        /// <summary>
        /// Visualize standard ego's source code
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserViewEgoHandler(object sender, EventArgs e)
        {
            //StopBackgroundThinker();

            Ego ego = new Ego();
            Paragraph paragraph;

            foreach (string line in ego)
            {
                paragraph = new Paragraph();
                paragraph.Inlines.Add("//" + line);
                mainWindow.AddToOutputText(paragraph);
            }

            //brain.StartBackgroundThinker(nameMapper);
        }

        /// <summary>
        /// Visualize standard instinct's source code
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserViewStandardInstinctHandler(object sender, EventArgs e)
        {
            //StopBackgroundThinker();

            StandardInstinct instinct = new StandardInstinct();
            Paragraph paragraph;

            foreach (string line in instinct)
            {
                paragraph = new Paragraph();
                paragraph.Inlines.Add("//" + line);
                mainWindow.AddToOutputText(paragraph);
            }

            //brain.StartBackgroundThinker(nameMapper);
        }

        /// <summary>
        /// Scan optimized memory for inconsistencies
        /// </summary>
        /// <param name="sender">source UI object</param>
        /// <param name="e">event</param>
        private void UserScanMemoryHandler(object sender, EventArgs e)
        {
            string output;
            StopBackgroundThinker();
            mainWindow.AddToOutputText("Scanning memory for inconsistencies, please wait");
            output = brain.RepairAndScanMemoryGetOutput(nameMapper);
            if (output != null)
                mainWindow.AddToOutputText(output);
            else
                mainWindow.AddToOutputText("No inconsistency found");
            StartBackgroundThinker();
        }
        #endregion
    }
}