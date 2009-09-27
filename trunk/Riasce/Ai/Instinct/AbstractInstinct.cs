using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents how instincs work
    /// instincs are lists of statements to parse
    /// like a batch file
    /// </summary>
    abstract class AbstractInstinct : IEnumerable<string>
    {
        #region Fields
        /// <summary>
        /// Statement list
        /// </summary>
        protected List<string> statementList = new List<string>();
        #endregion

        #region Public Methods
        public IEnumerator<string> GetEnumerator()
        {
            return statementList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Add statement to list
        /// </summary>
        /// <param name="statement">statement to add</param>
        protected void Add(string statement)
        {
            statementList.Add(statement.ToLower().Trim());
        }
        #endregion

        #region Properties
        /// <summary>
        /// Count how many statement are available
        /// </summary>
        public int Count
        {
            get { return statementList.Count; }
        }
        #endregion   
    }
}
