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
using System.Threading;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Interaction logic for VisualizerViewer.xaml
    /// </summary>
    public partial class VisualizerViewer : UserControl
    {
        #region Fields
        private ColorMaker colorMaker = new ColorMaker();

        private PictureCache pictureCache;

        private double zoomRatio = 1;      

        private string conceptName;

        private Dictionary<string, List<string>> conceptDefinition;

        private Dictionary<string, List<string>> connectionsNoWhy;

        private int currentMode;

        private TreeViewItem parentSubjectItem, parentVerbItem;

        private string latestSubjectName;

        private string latestVerbName;

        /// <summary>
        /// Will be called after visualizer has finished or got cancelled
        /// </summary>
        private Action callBackAction;
        #endregion

        #region Delegates types
        private delegate void OtherThreadDelegate();
        #endregion

        #region Constructor
        public VisualizerViewer(PictureCache pictureCache)
        {
            this.pictureCache = pictureCache;
            InitializeComponent();
        }
        #endregion

        #region Events
        public event EventHandler<StringEventArgs> OnClickConcept;

        public event EventHandler<StringEventArgs> OnClickWhyLink;

        public event EventHandler<StringEventArgs> OnClickDefineLink;

        public event EventHandler OnClickNext;

        public event EventHandler OnClickPrevious;

        public event EventHandler OnClickCheckBoxShowLongDefinition;

        public event EventHandler OnRefresh;

        public event EventHandler OnStop;

        public event EventHandler OnPrint;
        #endregion

        #region Public Methods
        public void ShowPreSelectedConceptVisualTree()
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new OtherThreadDelegate(SetLoadingCursor));

            pictureCache.GetCachedImage(conceptName);

            currentMode = 0;
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new OtherThreadDelegate(AddNewTreeViewItem));


            string verbName = null;
            List<string> complementNameList;
            foreach (KeyValuePair<string, List<string>> verbAndBranch in conceptDefinition)
            {
                verbName = verbAndBranch.Key;
                complementNameList = verbAndBranch.Value;

                conceptName = verbName;
                pictureCache.GetCachedImage(conceptName);

                currentMode = 1;
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new OtherThreadDelegate(AddNewTreeViewItem));

                foreach (string complementName in complementNameList)
                {
                    currentMode = 2;
                    conceptName = complementName;
                    pictureCache.GetCachedImage(conceptName);

                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new OtherThreadDelegate(AddNewTreeViewItem));
                }
            }

            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new OtherThreadDelegate(SetRegularCursor));
            
            //We fire the callback to tell controller visualizer thread has finished
            if (callBackAction != null) callBackAction();
        }

        public void ShowProof(List<string> proofDefinitionList)
        {
            TreeViewItem currentItem;

            string[] previousArgumentWords = new string[3];
            string[] currentArgumentWords;
            TreeViewItem previousItem = null;
            foreach (string argument in proofDefinitionList)
            {
                currentArgumentWords = argument.Split(' ');

                currentItem = new TreeViewItem();
                currentItem.Header = argument;


                if (previousItem != null && currentArgumentWords[0] == previousArgumentWords[0])
                {
                    previousItem.Items.Add(currentItem);
                }
                else if (previousItem != null && currentArgumentWords[1] == previousArgumentWords[1])
                {
                    previousItem.Items.Add(currentItem);
                }
                else if (previousItem != null && currentArgumentWords[2] == previousArgumentWords[2])
                {
                    previousItem.Items.Add(currentItem);
                }
                else
                {
                    treeView.Items.Add(currentItem);
                    previousItem = currentItem;
                }

                previousArgumentWords = currentArgumentWords;
            }
        }

        public void ClearItems()
        {
            treeView.Items.Clear();
        }
        #endregion

        #region Private methods
        private void ZoomIn()
        {
            if (zoomRatio > 0.001)
            {
                zoomRatio /= 1.5;
                scaleTransform.ScaleX = zoomRatio;
                scaleTransform.ScaleY = zoomRatio;
            }
        }

        private void ZoomOut()
        {
            if (zoomRatio < 800)
            {
                zoomRatio *= 1.5;
                scaleTransform.ScaleX = zoomRatio;
                scaleTransform.ScaleY = zoomRatio;
            }
        }

        private void AddNewTreeViewItem()
        {
            Image currentConceptImage = pictureCache.GetCachedImage(conceptName);
            Image currentCornerImage = pictureCache.GetCachedImage("treeCorner");

            TreeViewItem subjectItem = new TreeViewItem();
            Span spanItem = new Span();
            spanItem.FontSize = 36;

            #region We set the appropriate margin
            if (currentMode == 0)
                subjectItem.Margin = new Thickness(0, 16, 0, 0);
            else if (currentMode == 1)
                subjectItem.Margin = new Thickness(0, 0, 0, 0);
            else
                subjectItem.Margin = new Thickness(128, 0, 0, 0);
            #endregion

            #region We add the corner image if needed
            if (currentMode >= 1 && currentCornerImage != null)
            {
                currentCornerImage.Width = 128;
                currentCornerImage.Height = 128;
                spanItem.Inlines.Add(currentCornerImage);
                spanItem.Inlines.Add(" ");
            }
            #endregion

            if (currentConceptImage != null)
            {
                currentConceptImage.Width = 128;
                currentConceptImage.Height = 128;

                Span spanImage = new Span();
                spanImage.Inlines.Add(currentConceptImage);
                spanImage.Tag = conceptName;
                spanImage.MouseLeftButtonDown += ClickItemHandler;
                spanImage.MouseEnter += MouseEnterItemHandler;
                spanImage.MouseLeave += MouseLeaveItemHandler;
                spanImage.Cursor = Cursors.Hand;

                spanItem.Inlines.Add(spanImage);
                spanItem.Inlines.Add(" ");
            }

            Span spanConceptName = new Span();
            spanConceptName.Inlines.Add(conceptName);
            spanConceptName.Tag = conceptName;
            spanConceptName.MouseLeftButtonDown += ClickItemHandler;
            spanConceptName.MouseEnter += MouseEnterItemHandler;
            spanConceptName.MouseLeave += MouseLeaveItemHandler;
            spanConceptName.Cursor = Cursors.Hand;
            spanItem.Inlines.Add(spanConceptName);


            if (currentMode == 0)
                defineButton.Tag = conceptName;



            subjectItem.Header = spanItem;


                

            if (currentMode == 0)
            {
                #region Subject
                treeView.Items.Add(subjectItem);
                parentSubjectItem = subjectItem;
                parentSubjectItem.IsExpanded = true;
                latestSubjectName = conceptName;
                #endregion
            }
            else if (currentMode == 1 && parentSubjectItem != null)
            {
                #region Verb
                if ((bool)(isShowingLongDefinitionCheckBox.IsChecked))
                {
                    subjectItem.Header = null;
                    StackPanel stackPanel = new StackPanel();
                    ScrollViewer scrollViewer = new ScrollViewer();
                    TextBlock textBlock = new TextBlock();
                    textBlock.Inlines.Add(spanItem);
                    stackPanel.Children.Add(textBlock);
                    stackPanel.Children.Add(scrollViewer);
                    scrollViewer.Content = subjectItem;
                    scrollViewer.MaxHeight = 420;
                    scrollViewer.Padding = new Thickness(0, 0, 20, 0);
                    scrollViewer.Margin = new Thickness(-20, 0, 0, 0);
                    scrollViewer.Background = colorMaker.GetWhiteToColorLinearGradientFromString(conceptName, 208, 255);
                    stackPanel.Margin = new Thickness(0, 0, 0, 0);

                    parentSubjectItem.Items.Add(stackPanel);
                    parentVerbItem = subjectItem;
                    parentVerbItem.IsExpanded = true;
                    latestVerbName = conceptName;
                }
                else
                {
                    parentSubjectItem.Items.Add(subjectItem);
                    parentVerbItem = subjectItem;
                    parentVerbItem.IsExpanded = true;
                    latestVerbName = conceptName;
                }
                #endregion
            }
            else if (currentMode == 2 && parentVerbItem != null)
            {
                #region Complement
                parentVerbItem.Items.Add(subjectItem);

                if ((bool)(isShowingLongDefinitionCheckBox.IsChecked))
                {
                    if (connectionsNoWhy != null)
                    {
                        bool mustShowWhyLink = false;

                        List<string> noWhyComplementNameList;
                        if (connectionsNoWhy.TryGetValue(latestVerbName, out noWhyComplementNameList))
                        {
                            if (!noWhyComplementNameList.Contains(conceptName))
                            {
                                mustShowWhyLink = true;
                            }
                        }
                        else
                        {
                            mustShowWhyLink = true;
                        }

                        if (mustShowWhyLink)
                        {
                            Span whyLink = new Span();
                            whyLink.Inlines.Add("why?");
                            whyLink.FontSize = 13;
                            whyLink.Tag += latestSubjectName + " " + latestVerbName + " " + conceptName;
                            whyLink.MouseEnter += MouseEnterItemHandler;
                            whyLink.MouseLeave += MouseLeaveItemHandler;
                            whyLink.Cursor = Cursors.Hand;
                            whyLink.MouseLeftButtonDown += ClickWhyLinkHandler;
                            spanItem.Inlines.Add(" ");
                            spanItem.Inlines.Add(whyLink);
                        }
                    }
                }
                #endregion
            }

            subjectItem.Selected += SelectTreeViewItemHandler;
        }

        private void SetRegularCursor()
        {
            this.Cursor = Cursors.Arrow;
        }

        private void SetLoadingCursor()
        {
            this.Cursor = Cursors.ArrowCD;
        }
        #endregion

        #region Handlers
        private void ClickItemHandler(object source, EventArgs e)
        {
            Span span = (Span)(source);

            StringEventArgs stringEventArgs = new StringEventArgs((string)(span.Tag));

            if (OnClickConcept != null) OnClickConcept(source, stringEventArgs);
        }

        private void ClickWhyLinkHandler(object source, EventArgs e)
        {
            Span span = (Span)(source);

            StringEventArgs stringEventArgs = new StringEventArgs((string)(span.Tag));

            if (OnClickWhyLink != null) OnClickWhyLink(source, stringEventArgs);
        }

        private void SelectTreeViewItemHandler(object source, EventArgs e)
        {
            TreeViewItem item = (TreeViewItem)(source);
            item.IsSelected = false;
        }

        private void defineButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)(sender);

            StringEventArgs stringEventArgs = new StringEventArgs((string)(button.Tag));

            if (OnClickDefineLink != null) OnClickDefineLink(sender, stringEventArgs);
        }

        private void MouseEnterItemHandler(object source, EventArgs e)
        {
            Span span = (Span)(source);
            span.TextDecorations = TextDecorations.Underline;
        }

        private void MouseLeaveItemHandler(object source, EventArgs e)
        {
            Span span = (Span)(source);
            span.TextDecorations = null;
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (OnClickNext != null) OnClickNext(sender,e);
        }

        private void buttonPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (OnClickPrevious != null) OnClickPrevious(sender, e);
        }

        private void isShowingLongDefinitionCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (OnClickCheckBoxShowLongDefinition != null) OnClickCheckBoxShowLongDefinition(sender, e);
        }

        private void buttonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            ZoomIn();
        }

        private void buttonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            ZoomOut();
        }

        private void buttonNormalZoomSize_Click(object sender, RoutedEventArgs e)
        {
            zoomRatio = 1;
            scaleTransform.ScaleX = zoomRatio;
            scaleTransform.ScaleY = zoomRatio;
        }

        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (OnRefresh != null) OnRefresh(sender, e);
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            if (OnStop != null) OnStop(sender, e);
        }

        private void buttonPrint_Click(object sender, RoutedEventArgs e)
        {
            if (OnPrint != null) OnPrint(sender, e);
        }
        #endregion
        
        #region Properties
        public string ConceptName
        {
            get { return conceptName; }
            set { conceptName = value; }
        }

        public Dictionary<string, List<string>> ConceptDefinition
        {
            get { return conceptDefinition; }
            set { conceptDefinition = value; }
        }

        public Dictionary<string, List<string>> ConnectionsNoWhy
        {
            get { return connectionsNoWhy; }
            set { connectionsNoWhy = value; }
        }

        public Action VisualizerFinishedCallBack
        {
            get { return callBackAction; }
            set { callBackAction = value; }
        }
        #endregion
    }
}
