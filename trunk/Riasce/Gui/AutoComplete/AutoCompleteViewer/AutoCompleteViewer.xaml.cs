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

namespace AntiCulture.Kid
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class AutoCompleteViewer : Window
    {
        #region Constructors
        public AutoCompleteViewer()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        public void FillList(IEnumerable<string> selectionList)
        {
            listBox.ItemsSource = selectionList;
        }

        private string UnparseSymbol(string element)
        {
            if (element.Length < 2)
                return element;

            return element.Substring(2, element.Length - 2) +" "+ element.Substring(0, 1);
        }

        public void SelectFirstWord()
        {
            if (listBox.Items.Count > 0)
                listBox.SelectedIndex = 0;
        }

        public void TryMoveSelectionDown()
        {
            if (listBox.Items.Count < 1)
                return;

            if (listBox.SelectedIndex < listBox.Items.Count - 1)
                listBox.SelectedIndex++;
        }

        public void TryMoveSelectionUp()
        {
            if (listBox.Items.Count < 1)
                return;

            if (listBox.SelectedIndex > 0)
                listBox.SelectedIndex--;
        }

        public string GetSelectionKey()
        {
            /*
            Grid currentItem = (Grid)(listBox.SelectedItem);
            Label currentLabel = (Label)(currentItem.Children[1]);
            string labelValue = (string)(currentLabel.Content);
            Image currentImage = (Image)(currentItem.Children[0]);
            return labelValue + " " + (string)(currentImage.Tag);
            */
            return UnparseSymbol((string)(listBox.SelectedItem));
        }

        public void CenterSelection()
        {
            listBox.ScrollIntoView(listBox.SelectedItem);
        }
        #endregion

        #region Events
        public event EventHandler UserDoubleClickItemView;

        public event EventHandler UserMouseClickView;
        #endregion

        #region Handlers
        private void doubleClickItem(object sender, RoutedEventArgs e)
        {
            if (UserDoubleClickItemView != null) UserDoubleClickItemView(this, e);
        }

        private void mouseClick(object sender, RoutedEventArgs e)
        {
            if (UserMouseClickView != null) UserMouseClickView(this, e);
        }
        #endregion
    }
}
