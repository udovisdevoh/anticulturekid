using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractMemento : IComparable<AbstractMemento>
    {
        #region Fields
        protected DateTime time;

        protected string authorName;
        #endregion

        /// <summary>
        /// Compare time of two mementos
        /// </summary>
        /// <param name="other">other memento</param>
        /// <returns>Compared time of two mementos</returns>
        public int CompareTo(AbstractMemento other)
        {
            TimeSpan duration = this.time - other.time;
            return (int)duration.TotalMilliseconds;
        }
    }
}
