using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.ComponentModel;
using System.IO;
using System.Xml;
using Microsoft.Win32;
using Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Interaction logic for Gui.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        private static readonly int maxCommentCount = 1024;

        private static readonly int maxFeelingCommentCount = 48;

        private CommandHistory commandHistory = new CommandHistory();

        private AutoComplete autoComplete;

        private MemoryTreeView memoryTreeView = new MemoryTreeView();

        private WpfVisualizer visualizer;
        #endregion

        #region Events
        public event EventHandler UserSendComment;

        public event EventHandler<CancelEventArgs> UserWantsToExit;

        public event EventHandler UserSignOut;

        public event EventHandler UserRunScript;

        public event EventHandler UserSaveFile;

        public event EventHandler UserSaveAsFile;

        public event EventHandler UserOpenFile;

        public event EventHandler UserViewHelp;

        public event EventHandler UserFileNew;

        public event EventHandler UserConversationStart;

        public event EventHandler UserConversationStop;

        public event EventHandler UserStandardInstinct;

        public event EventHandler UserViewStandardInstinct;

        public event EventHandler UserEgo;

        public event EventHandler UserViewEgo;

        public event EventHandler UserExtractCategoryListFromWiki;

        public event EventHandler UserOpenCategoryExtractor;

        public event EventHandler UserSortCategoryList;

        public event EventHandler UserExtractOccurenceMatrix;

        public event EventHandler UserExtractPhoneticMatrix;

        public event EventHandler UserOccurenceToSemantic;

        public event EventHandler UserScanMemory;

        public event EventHandler UserScanFlatMemory;

        public event EventHandler<StringEventArgs> OnVisualizerClickConcept;

        public event EventHandler<StringEventArgs> OnVisualizerClickWhyLink;

        public event EventHandler<StringEventArgs> OnVisualizerClickDefineLink;
        #endregion

        #region Constructors
        public MainWindow(DisambiguationNamer disambiguationNamer)
        {
            InitializeComponent();
            textBoxInput.Focus();
            autoComplete = new AutoComplete(disambiguationNamer);
            autoComplete.UserDoubleClickItem += UserDoubleClickItemHandler;
            autoComplete.UserMouseClick += UserMouseClickAutoCompleteHandler;
            scrollViewerForTreeView.Content = memoryTreeView;
            memoryTreeView.OnClickItem += VisualizerClickConceptHandler;
            helpTab.Content = new Help();
            visualizer = new WpfVisualizer();
            visualizer.OnClickConcept += VisualizerClickConceptHandler;
            visualizer.OnClickDefineLink += VisualizerClickDefineLinkHandler;
            visualizer.OnClickWhyLink += VisualizerClickWhyLinkHandler;
            visualizer.OnPrint += PrintHandler;
        }
        #endregion

        #region Public Methods
        public void AddToOutputText(string text)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(text);
            AddToOutputText(paragraph);
        }

        public void AddToOutputText(Paragraph paragraph)
        {
            if (tabControl.SelectedIndex == 1)
            {
                richTextBoxFeelingOutput.Document.Blocks.Add(paragraph);

                if (richTextBoxFeelingOutput.Document.Blocks.Count > maxFeelingCommentCount)
                    richTextBoxFeelingOutput.Document.Blocks.Remove(richTextBoxFeelingOutput.Document.Blocks.FirstBlock);

                feelingScrollViewer.ScrollToEnd();
            }
            else
            {
                richTextBoxOutput.Document.Blocks.Add(paragraph);
                if (richTextBoxOutput.Document.Blocks.Count > maxCommentCount)
                    richTextBoxOutput.Document.Blocks.Remove(richTextBoxOutput.Document.Blocks.FirstBlock);
                scrollViewer.ScrollToEnd();
            }
        }

        public void AddToFeelings(Paragraph paragraph)
        {
            if (tabControl.SelectedIndex == 1)
                return;

            richTextBoxFeelingOutput.Document.Blocks.Add(paragraph);

            if (richTextBoxFeelingOutput.Document.Blocks.Count > maxFeelingCommentCount)
                richTextBoxFeelingOutput.Document.Blocks.Remove(richTextBoxFeelingOutput.Document.Blocks.FirstBlock);

            feelingScrollViewer.ScrollToEnd();
        }

        public void ResetTextFields()
        {
            richTextBoxOutput.Document.Blocks.Clear();
            richTextBoxFeelingOutput.Document.Blocks.Clear();
        }

        public string GetNewHumanNameFromInputBox(string currentName)
        {
            return Microsoft.VisualBasic.Interaction.InputBox("What is your unique concept name?", "Name input", currentName, 0, 0).ConceptNameFormat();
        }

        public bool GetBoolInput(string title, string question)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
                return true;
            return false;
        }

        public int GetIntegerInput(string question, int minValue, int defaultValue, int maxValue)
        {
            question += " (min: " + minValue.ToString() + " max: " + maxValue.ToString() + ")";

            string result;
            int numberResult = -1;
            do
            {
                result = Microsoft.VisualBasic.Interaction.InputBox(question, question, defaultValue.ToString(), 0, 0);
                int.TryParse(result, out numberResult);
            } while (numberResult > maxValue || numberResult < minValue);

            return numberResult;
        }

        public string GetInputFile()
        {
            return GetInputFile(null);
        }

        public string GetInputFile(string filter)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (filter != null)
                dialog.Filter = filter;
            dialog.FilterIndex = 0;
            dialog.RestoreDirectory = true;

            dialog.ShowDialog();

            if (dialog.FileName == "")
                return null;

            return dialog.FileName;
        }

        public string GetOutputFile(string filter)
        {
            return GetOutputFile(filter, true);
        }

        public string GetOutputFile(string filter, bool overWritePrompt)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            if (filter != null)
                dialog.Filter = filter;
            dialog.FilterIndex = 0;
            dialog.RestoreDirectory = true;
            dialog.OverwritePrompt = overWritePrompt;

            dialog.ShowDialog();

            if (dialog.FileName == "")
                return null;

            return dialog.FileName;
        }

        public void RefreshAutoCompleteFrom(Memory memory, NameMapper nameMapper)
        {
            autoComplete.RefreshFrom(memory, nameMapper);
        }

        public void SetMemoryTreeViewBinding(Memory memory, NameMapper nameMapper)
        {
            memoryTreeView.SetBinding(memory,nameMapper);
        }

        public void UpdateTreeViewIfNeeded()
        {
            memoryTreeView.UpdateIfNeeded();
        }

        public MessageBoxResult AskToSave()
        {
            string messageBoxText = "Memory has been altered. Do you want to save changes?";
            string caption = Title;
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            return MessageBox.Show(messageBoxText, caption, button, icon);
        }

        public void VisualizeConcept(string conceptName, Dictionary<string, List<string>> definition, Dictionary<string, List<string>> connectionsNoWhy)
        {
            visualizer.ShowConcept(conceptName, definition, connectionsNoWhy);
            visualizerTab.Content = visualizer.VisualizerViewer;
        }

        public void VisualizeProof(List<string> proofDefinitionList)
        {
            visualizer.ShowProof(proofDefinitionList);
            visualizerTab.Content = visualizer.VisualizerViewer;
        }
        #endregion

        #region Private Methods
        private void ReplaceWordContainingCarret(TextBox textBoxInput, string wordFromAutoComplete)
        {
            string wordContainingCarret = autoComplete.GetWordContainingCarret(textBoxInput);

            int endingParanthesesCount = wordContainingCarret.CountEndingChar(")");
            int startingParanthesesCount = wordContainingCarret.CountStartingChar("(");

            endingParanthesesCount -= (wordContainingCarret.CountChar('(') - startingParanthesesCount);

            for (int i = 0 ; i < endingParanthesesCount ; i++)
                wordFromAutoComplete += ')';

            for (int i = 0; i < startingParanthesesCount; i++)
                wordFromAutoComplete = '(' + wordFromAutoComplete;

            //add wordFromAutoComplete to textBoxInput.Text
            int wordContainingCarretStartPosition = autoComplete.GetWordContainingCarretStartPosition(textBoxInput);
            int wordContainingCarretEndPosition = autoComplete.GetWordContainingCarretEndPosition(textBoxInput);

            string beforeWord = textBoxInput.Text.Substring(0, wordContainingCarretStartPosition);
            string afterWord = textBoxInput.Text.Substring(wordContainingCarretEndPosition);

            textBoxInput.Text = wordFromAutoComplete;

            beforeWord = beforeWord.Trim();

            if (beforeWord.Length > 0)
                textBoxInput.Text = beforeWord + " " + textBoxInput.Text;

            afterWord = afterWord.Trim();
            
            if (afterWord.Length > 0)
                textBoxInput.Text += " " + afterWord;
                

            autoComplete.HideViewer();
            textBoxInput.CaretIndex = beforeWord.Length + wordFromAutoComplete.Length;

            textBoxInput.CaretIndex++;
            for (int i = 0; i < endingParanthesesCount; i++)
                textBoxInput.CaretIndex--;


            //if (!isOriginalWordEndingWithClosingParantheses)
            //    textBoxInput.CaretIndex++;

            if (autoComplete.MatchesOneAndOnlyOneRegex(textBoxInput.Text))
            {
                commandHistory.Add(textBoxInput.Text);
                if (UserSendComment != null)
                {
                    UserSendComment(this, new EventArgs());
                }
            }
        }
        #endregion

        #region Handlers
        private void UserDoubleClickItemHandler(object sender, EventArgs e)
        {
            //AppendAutoCompleteSelectedWordToInputTextBox();
            ReplaceWordContainingCarret(textBoxInput, autoComplete.GetSelectionValue());
        }

        private void UserMouseClickAutoCompleteHandler(object sender, EventArgs e)
        {
            this.textBoxInput.Focus();
        }

        private void menuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void menuItemSignOut_Click(object sender, RoutedEventArgs e)
        {
            if (UserSignOut != null) UserSignOut(this, e);
        }

        private void menuItemRunScript_Click(object sender, RoutedEventArgs e)
        {
            if (UserRunScript != null) UserRunScript(this, e);
        }

        private void buttonEnter_Click(object sender, RoutedEventArgs e)
        {
            commandHistory.Add(textBoxInput.Text);
            if (UserSendComment != null) UserSendComment(this, e);
        }

        private void keyboard_Down(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                #region Enter
                if (autoComplete.IsVisible)
                {
                    //AppendAutoCompleteSelectedWordToInputTextBox();
                    ReplaceWordContainingCarret(textBoxInput, autoComplete.GetSelectionValue());
                }
                else
                {
                    commandHistory.Add(textBoxInput.Text);
                    if (UserSendComment != null)
                    {
                        UserSendComment(this, e);
                    }
                }
                #endregion
            }
            else if (e.Key == Key.Up)
            {
                #region Up
                if (autoComplete.IsVisible)
                {
                    autoComplete.TryMoveSelectionUp();
                }
                else
                {
                    textBoxInput.Text = commandHistory.GetMoveUp();
                    textBoxInput.CaretIndex = textBoxInput.Text.Length;
                }
                #endregion
            }
            else if (e.Key == Key.Down)
            {
                #region Down
                if (autoComplete.IsVisible)
                {
                    autoComplete.TryMoveSelectionDown();
                }
                else
                {
                    textBoxInput.Text = commandHistory.GetMoveDown();
                    textBoxInput.CaretIndex = textBoxInput.Text.Length;
                }
                #endregion
            }
            else if (e.Key == Key.Escape || e.Key == Key.Enter || e.Key == Key.Space)
            {
                #region Esc, Enter or Space
                autoComplete.HideViewer();
                #endregion
            }
        }

        private void keyboard_Up(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape && e.Key != Key.Enter && e.Key != Key.Space && e.Key != Key.Up && e.Key != Key.Down && e.Key != Key.PageUp && e.Key != Key.PageDown)
            {
                #region Pressing an input key
                //if (IsSpaceCharAfterCarret(textBoxInput))
                if (autoComplete.GetWordContainingCarret(textBoxInput) == null)
                {
                    autoComplete.HideViewer();
                }
                else
                {
                    //autoComplete.TrySelectBeginWith(textBoxInput.Text.GetLastWord(), Top + ActualHeight - bottomRow.MinHeight - 5, Left + treeViewRow.MinWidth + 5);
                    autoComplete.TrySelectBeginWith(autoComplete.GetWordContainingCarret(textBoxInput), Top + ActualHeight - bottomRow.MinHeight - 5, Left + treeViewRow.MinWidth + 5);

                    textBoxInput.Focus();
                    autoComplete.BringToFront();
                }
                #endregion
            }
        }

        private bool IsSpaceCharAfterCarret(TextBox textBoxInput)
        {
            for (int i = textBoxInput.CaretIndex; i < textBoxInput.Text.Length; i++)
                if (textBoxInput.Text[i] == ' ')
                    return true;
            return false;
        }

        private void menuItemSaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (UserSaveAsFile != null) UserSaveAsFile(this, e);
        }

        private void menuItemSave_Click(object sender, RoutedEventArgs e)
        {
            if (UserSaveFile != null) UserSaveFile(this, e);
        }

        private void menuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UserOpenFile != null) UserOpenFile(this, e);
            }
            catch (System.Runtime.Serialization.SerializationException)
            {
                MessageBox.Show("This file is incompatible.");
            }
        }

        private void menuItemNew_Click(object sender, RoutedEventArgs e)
        {
            if (UserFileNew != null) UserFileNew(this, e);
        }

        private void menuItemHelp_Click(object sender, RoutedEventArgs e)
        {
            if (UserViewHelp != null) UserViewHelp(this, e);
        }

        private void menuItemConversationStart_Click(object sender, RoutedEventArgs e)
        {
            if (UserConversationStart != null) UserConversationStart(this, e);
        }

        private void menuItemConversationStop_Click(object sender, RoutedEventArgs e)
        {
            if (UserConversationStop != null) UserConversationStop(this, e);
        }

        private void menuItemStandardInstinct_Click(object sender, RoutedEventArgs e)
        {
            if (UserStandardInstinct != null) UserStandardInstinct(this, e);
        }

        private void menuItemEgo_Click(object sender, RoutedEventArgs e)
        {
            if (UserEgo != null) UserEgo(this, e);
        }

        private void menuItemExtractCategoryListFromWiki_Click(object sender, RoutedEventArgs e)
        {
            if (UserExtractCategoryListFromWiki != null) UserExtractCategoryListFromWiki(this, e);
        }

        private void menuItemSortCategoryList_Click(object sender, RoutedEventArgs e)
        {
            if (UserSortCategoryList != null) UserSortCategoryList(this, e);
        }

        private void menuItemOpenCategoryExtractor_Click(object sender, RoutedEventArgs e)
        {
            if (UserOpenCategoryExtractor != null) UserOpenCategoryExtractor(this, e);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            autoComplete.HideViewer();
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            autoComplete.HideViewer();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            UserWantsToExit(this, e);
            base.OnClosing(e);
        }

        private void VisualizerClickConceptHandler(object sender, StringEventArgs e)
        {
            if (OnVisualizerClickConcept != null) OnVisualizerClickConcept(sender, e);
        }

        private void VisualizerClickWhyLinkHandler(object sender, StringEventArgs e)
        {
            if (OnVisualizerClickWhyLink != null) OnVisualizerClickWhyLink(sender, e);
        }

        private void VisualizerClickDefineLinkHandler(object sender, StringEventArgs e)
        {
            if (OnVisualizerClickDefineLink != null) OnVisualizerClickDefineLink(sender, e);
        }

        private void PrintHandler(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                TreeView elementToPrint = visualizer.TreeView;

                //get selected printer capabilities
                System.Printing.PrintCapabilities capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);

                //get scale of the print wrt to screen of WPF visual
                double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / elementToPrint.ActualWidth, capabilities.PageImageableArea.ExtentHeight / elementToPrint.ActualHeight);

                //Transform the Visual to scale
                Transform layoutTransformBackup = elementToPrint.LayoutTransform;
                elementToPrint.LayoutTransform = new ScaleTransform(scale, scale);

                //get the size of the printer page
                Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

                //update the layout of the visual to the printer page size.
                elementToPrint.Measure(sz);
                elementToPrint.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

                //now print the visual to printer to fit on the one page.
                printDialog.PrintVisual(elementToPrint, "Print");

                //restore layout transform
                elementToPrint.LayoutTransform = layoutTransformBackup;
            }
        }

        private void MenuItemViewStandardInstinct_Click(object sender, RoutedEventArgs e)
        {
            if (UserViewStandardInstinct != null) UserViewStandardInstinct(this, e);
        }

        private void menuItemViewEgo_Click(object sender, RoutedEventArgs e)
        {
            if (UserViewEgo != null) UserViewEgo(this, e);
        }

        private void menuItemExtractOccurenceMatrix_Click(object sender, RoutedEventArgs e)
        {
            if (UserExtractOccurenceMatrix != null) UserExtractOccurenceMatrix(this, e);
        }

        private void menuItemExtractPhoneticMatrix_Click(object sender, RoutedEventArgs e)
        {
            if (UserExtractPhoneticMatrix != null) UserExtractPhoneticMatrix(this, e);
        }

        private void menuItemOccurenceToSemantic_Click(object sender, RoutedEventArgs e)
        {
            if (UserOccurenceToSemantic != null) UserOccurenceToSemantic(this, e);
        }

        private void menuItem_ClickScanMemory(object sender, RoutedEventArgs e)
        {
            if (UserScanMemory != null) UserScanMemory(this, e);
        }
        #endregion

        #region Properties
        public string InputText
        {
            get { return textBoxInput.Text; }
            set { textBoxInput.Text = value; }
        }

        public bool IsVisualizerShowingLongDefinition
        {
            get { return visualizer.IsShowingLongDefinition; }
        }

        public List<string> VerbNameListForHelp
        {
            set
            {
                Help help = (Help)(helpTab.Content);
                help.VerbNameList = value;
            }
        }
        #endregion
    }
}