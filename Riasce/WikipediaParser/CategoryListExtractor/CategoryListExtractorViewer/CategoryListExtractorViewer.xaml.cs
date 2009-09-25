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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CategoryListExtractorViewer : Window
    {
        #region Fields
        private double progressBarPercentage;

        private OtherThreadDelegate updateWpfProgressBarDelegate;

        private OtherThreadDelegate closeDelegate;
        #endregion

        #region Constructor
        public CategoryListExtractorViewer()
        {
            InitializeComponent();
            
            updateWpfProgressBarDelegate = new OtherThreadDelegate(UpdateWpfProgressBarMethod);
            closeDelegate = new OtherThreadDelegate(Close);
        }
        #endregion

        #region Events
        public event EventHandler UserCancelButtonClick;
        #endregion

        #region Handlers
        private void cancelButton_click(object sender, EventArgs e)
        {
            buttonCancel.IsEnabled = false;
            Title = "closing...";

            if (UserCancelButtonClick != null) UserCancelButtonClick(this, e);
        }
        #endregion

        #region Delegate types
        private delegate void OtherThreadDelegate();
        #endregion

        #region Methods
        public void UpdateProgressBar(double progressBarPercentage)
        {
            this.progressBarPercentage = progressBarPercentage;
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, updateWpfProgressBarDelegate);
        }

        public void CloseWhenPossible()
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, closeDelegate);
        }

        private void UpdateWpfProgressBarMethod()
        {
            progressBar.Value = progressBarPercentage;
            displayedPercent.Inlines.Clear();
            displayedPercent.Inlines.Add(Math.Round(progressBarPercentage,2) + "%");
        }
        #endregion
    }
}
