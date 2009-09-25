using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace AntiCulture.Kid
{
    class MemoryTreeViewItem : TreeViewItem
    {
        #region Events
        public event RoutedEventHandler Expanding;
        #endregion

        #region Methods
        protected override void OnExpanded(RoutedEventArgs e)
        {
            if (Expanding != null)
                Expanding(this, e);
            //base.OnExpanded(e);
        }
        #endregion
    }
}
