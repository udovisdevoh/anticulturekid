using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riasce
{
    class SortableCategory : IComparable<SortableCategory>
    {
        #region Fields
        private int count;

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
        public int CompareTo(SortableCategory other)
        {
            return other.count - this.count;
        }
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
        }

        public int Count
        {
            get { return count; }
        }
        #endregion
    }
}
