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

namespace Riasce
{
    /// <summary>
    /// Interaction logic for CategoryExtractorViewer.xaml
    /// </summary>
    public partial class CategoryExtractorViewer : Window
    {
        #region Constructor
        public CategoryExtractorViewer()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        public event EventHandler OnButtonSelectSourceFileClick;
        
        public event EventHandler OnButtonSelectCheckListFileClick;

        public event EventHandler OnButtonStartClick;

        public event EventHandler OnButtonPreviousClick;

        public event EventHandler OnButtonNextClick;

        public event EventHandler OnButtonApplyClick;

        public event EventHandler OnButtonIgnoreClick;
        #endregion

        #region Handlers
        private void buttonSelectSourceFile_Click(object sender, RoutedEventArgs e)
        {
            if (OnButtonSelectSourceFileClick != null) OnButtonSelectSourceFileClick(this,e);
        }

        private void buttonSelectCheckListFile_Click(object sender, RoutedEventArgs e)
        {
            if (OnButtonSelectCheckListFileClick != null) OnButtonSelectCheckListFileClick(this, e);
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            if (OnButtonStartClick != null) OnButtonStartClick(this, e);
        }

        private void buttonPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (OnButtonPreviousClick != null) OnButtonPreviousClick(this, e);
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (OnButtonNextClick != null) OnButtonNextClick(this, e);
        }

        private void buttonApply_Click(object sender, RoutedEventArgs e)
        {
            if (OnButtonApplyClick != null) OnButtonApplyClick(this, e);
        }

        private void buttonIgnore_Click(object sender, RoutedEventArgs e)
        {
            if (OnButtonIgnoreClick != null) OnButtonIgnoreClick(this, e);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Properties
        public IEnumerable<string> ElementNameList
        {
            set
            {
                listBoxConcept.Items.Clear();
                foreach (string element in value)
                    listBoxConcept.Items.Add(element);
            }
        }
        #endregion
    }
}
