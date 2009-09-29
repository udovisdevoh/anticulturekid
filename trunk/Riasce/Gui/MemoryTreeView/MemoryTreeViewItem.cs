using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a memory treeview item
    /// </summary>
    class MemoryTreeViewItem : TreeViewItem
    {
        #region Events
        /// <summary>
        /// When treeview item is expanding
        /// </summary>
        public event RoutedEventHandler Expanding;
        #endregion

        #region Methods
        /// <summary>
        /// When treeview item is expanding
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExpanded(RoutedEventArgs e)
        {
            if (Expanding != null)
                Expanding(this, e);
            //base.OnExpanded(e);
        }
        #endregion
    }
}
