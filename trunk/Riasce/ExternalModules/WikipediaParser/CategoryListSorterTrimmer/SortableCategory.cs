using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Sortable category
    /// </summary>
    class SortableCategory : IComparable<SortableCategory>
    {
        #region Fields
        /// <summary>
        /// Count
        /// </summary>
        private int count;

        /// <summary>
        /// Name
        /// </summary>
        private string name;
        #endregion

        #region Constructor
        public SortableCategory(int count, string name)
        {
            this.count = count;
            this.name = name;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Compare category to another
        /// </summary>
        /// <param name="other">other category</param>
        /// <returns>comparison for sorting</returns>
        public int CompareTo(SortableCategory other)
        {
            return other.count - this.count;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Count
        /// </summary>
        public int Count
        {
            get { return count; }
        }
        #endregion
    }
}
