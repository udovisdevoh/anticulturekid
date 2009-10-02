using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Windows.Controls;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a WPF implementation of concept visualizer
    /// </summary>
    class WpfVisualizer : AbstractVisualizer
    {
        #region Fields
        /// <summary>
        /// Picture cache
        /// </summary>
        private PictureCache pictureCache;

        /// <summary>
        /// Visualizer viewer
        /// </summary>
        private VisualizerViewer visualizerViewer;

        /// <summary>
        /// Visualizer history
        /// </summary>
        private VisualizerHistory visualizerHistory;

        /// <summary>
        /// Concept name
        /// </summary>
        private string conceptName;

        /// <summary>
        /// Is visualizer showing long concept definition
        /// </summary>
        private bool isShowingLongDefinition;

        /// <summary>
        /// Visualizer viewer's thread
        /// </summary>
        private Thread viewerThread;
        #endregion

        #region Constructor
        public WpfVisualizer()
        {
            pictureCache = new PictureCache();
            visualizerViewer = new VisualizerViewer(pictureCache);
            visualizerHistory = new VisualizerHistory();
            visualizerViewer.OnClickConcept += ClickConceptHandler;
            visualizerViewer.OnClickWhyLink += ClickWhyLinkHandler;
            visualizerViewer.OnClickDefineLink += ClickDefineLinkHandler;
            visualizerViewer.OnClickNext += ClickNextHandler;
            visualizerViewer.OnClickPrevious += ClickPreviousHandler;
            visualizerViewer.OnClickCheckBoxShowLongDefinition += ClickCheckBoxShowLongDefinitionHandler;
            visualizerViewer.OnRefresh += RefreshHandler;
            visualizerViewer.OnStop += StopHandler;
            visualizerViewer.OnPrint += PrintHandler;
        }
        #endregion

        #region Events
        public event EventHandler<StringEventArgs> OnClickConcept;

        public event EventHandler<StringEventArgs> OnClickWhyLink;

        public event EventHandler<StringEventArgs> OnClickDefineLink;

        public event EventHandler OnPrint;
        #endregion

        #region Public Methods
        /// <summary>
        /// Show a concept
        /// </summary>
        /// <param name="conceptName">concept name</param>
        /// <param name="conceptDefinition">concept definition</param>
        public override void ShowConcept(string conceptName, Dictionary<string, List<string>> conceptDefinition)
        {
            ShowConcept(conceptName, conceptDefinition, null);
        }

        /// <summary>
        /// Show a concept
        /// </summary>
        /// <param name="conceptName">concept name</param>
        /// <param name="conceptDefinition">concept definition</param>
        /// <param name="connectionsNoWhy">connenctions for which no why link will be shown (can be null)</param>
        public override void ShowConcept(string conceptName, Dictionary<string, List<string>> conceptDefinition, Dictionary<string, List<string>> connectionsNoWhy)
        {
            StopViewer();
            this.conceptName = conceptName;
            visualizerHistory.Push(conceptName);
            visualizerViewer.buttonNext.IsEnabled = visualizerHistory.CanNext();
            visualizerViewer.buttonPrevious.IsEnabled = visualizerHistory.CanPrevious();
            visualizerViewer.isShowingLongDefinitionCheckBox.IsChecked = isShowingLongDefinition;

            visualizerViewer.ConceptName = conceptName;
            visualizerViewer.ConceptDefinition = conceptDefinition;
            visualizerViewer.ConnectionsNoWhy = connectionsNoWhy;

            visualizerViewer.ClearItems();

            viewerThread = new Thread(visualizerViewer.ShowPreSelectedConceptVisualTree);
            viewerThread.Priority = ThreadPriority.AboveNormal;
            viewerThread.SetApartmentState(ApartmentState.STA);
            viewerThread.IsBackground = true;

            viewerThread.Start();
        }

        /// <summary>
        /// Get Visualizer Viewer
        /// </summary>
        public void ShowProof(List<string> proofDefinitionList)
        {
            StopViewer();
            visualizerViewer.ClearItems();
            visualizerViewer.ShowProof(proofDefinitionList);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Stop the viewer as soon as possible
        /// </summary>
        private void StopViewer()
        {
            if (viewerThread != null && viewerThread.IsAlive)
            {
                viewerThread.Abort();
                while (viewerThread.IsAlive)
                    Thread.Sleep(10);

                if (visualizerViewer.VisualizerFinishedCallBack != null)
                    visualizerViewer.VisualizerFinishedCallBack();
            }
        }
        #endregion

        #region Handlers
        public void ClickConceptHandler(object source, StringEventArgs stringEventArgs)
        {
            if (OnClickConcept != null) OnClickConcept(source, stringEventArgs);
        }

        public void ClickWhyLinkHandler(object source, StringEventArgs stringEventArgs)
        {
            if (OnClickWhyLink != null) OnClickWhyLink(source, stringEventArgs);
        }

        public void ClickDefineLinkHandler(object source, StringEventArgs stringEventArgs)
        {
            if (OnClickDefineLink != null) OnClickDefineLink(source, stringEventArgs);
        }

        public void ClickNextHandler(object source, EventArgs e)
        {
            if (visualizerHistory.CanNext() && OnClickConcept != null)
                OnClickConcept(source, new StringEventArgs(visualizerHistory.GetNext()));
        }

        public void ClickPreviousHandler(object source, EventArgs e)
        {
            if (visualizerHistory.CanPrevious() && OnClickConcept != null)
                OnClickConcept(source, new StringEventArgs(visualizerHistory.GetPrevious()));
        }

        public void ClickCheckBoxShowLongDefinitionHandler(object source, EventArgs e)
        {
            isShowingLongDefinition = (bool)(visualizerViewer.isShowingLongDefinitionCheckBox.IsChecked);
            OnClickConcept(source, new StringEventArgs(conceptName));
        }

        public void RefreshHandler(object source, EventArgs e)
        {
            if (OnClickConcept != null) OnClickConcept(source, new StringEventArgs(conceptName));
        }

        public void StopHandler(object source, EventArgs e)
        {
            StopViewer();
        }

        public void PrintHandler(object source, EventArgs e)
        {
            if (OnPrint != null) OnPrint(source, e);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Whether the visualizer is visible
        /// </summary>
        public bool IsVisible
        {
            get { return visualizerViewer.IsVisible;}
        }

        /// <summary>
        /// Visualizer viewer
        /// </summary>
        public override VisualizerViewer VisualizerViewer
        {
            get { return visualizerViewer; }
        }

        /// <summary>
        /// Is visualizer showing long concept definition
        /// </summary>
        public bool IsShowingLongDefinition
        {
            get { return isShowingLongDefinition; }
            set { isShowingLongDefinition = value; }
        }

        /// <summary>
        /// Tree View
        /// </summary>
        public TreeView TreeView
        {
            get { return visualizerViewer.treeView; }
        }

        /// <summary>
        /// Action called when finished visualizing concept
        /// </summary>
        public Action VisualizerFinishedCallBack
        {
            get { return visualizerViewer.VisualizerFinishedCallBack; }
            set { visualizerViewer.VisualizerFinishedCallBack = value;}
        }
        #endregion
    }
}