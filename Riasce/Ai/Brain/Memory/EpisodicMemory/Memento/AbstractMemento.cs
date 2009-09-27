using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents the base class of specialized mementos
    /// </summary>
    abstract class AbstractMemento : IComparable<AbstractMemento>
    {
        #region Fields
        /// <summary>
        /// Event's time
        /// </summary>
        protected DateTime time;

        /// <summary>
        /// Author's name (agent inducing the memento)
        /// </summary>
        protected string authorName;
        #endregion

        #region Public Methods
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
        #endregion
    }
}
