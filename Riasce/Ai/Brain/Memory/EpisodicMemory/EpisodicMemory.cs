using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents the episodic memory
    /// </summary>
    class EpisodicMemory
    {
        #region Fields
        /// <summary>
        /// Memento list
        /// </summary>
        private List<AbstractMemento> mementoList = new List<AbstractMemento>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Create a memento to remember an operation and its author
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="conceptList">concept list</param>
        /// <param name="isNegative">whether the connection is negative or not</param>
        /// <param name="isInterrogative">whether the connection is interrogative or not</param>
        public override void RememberOperation(string authorName, List<Concept> conceptList, bool isNegative, bool isInterrogative)
        {
            DateTime dateTime = DateTime.Now;
            OperationMemento memento = new OperationMemento(dateTime, authorName, conceptList, isNegative, isInterrogative);
            mementoList.Add(memento);
        }

        /// <summary>
        /// Create a memento to remember an metaOperation and its author
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="conceptList">concept list</param>
        /// <param name="metaOperator">metaOperator's name</param>
        /// <param name="isNegative">whether the metaConnection is negative or not</param>
        /// <param name="isInterrogative">whether the metaConnection is interrogative or not</param>
        public override void RememberMetaOperation(string authorName, List<Concept> conceptList, string metaOperator, bool isNegative, bool isInterrogative)
        {
            DateTime dateTime = DateTime.Now;
            MetaOperationMemento memento = new MetaOperationMemento(dateTime, authorName, conceptList, metaOperator, isNegative, isInterrogative);
            mementoList.Add(memento);
        }

        /// <summary>
        /// Create a memento to remember a statement and its author
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="stringList">a list of string (could be unary operators or stuff like that)</param>
        /// <param name="conceptList">a list of concept</param>
        /// <param name="isNegative">whether the "connection" is negative or not</param>
        public override void RememberListStringAndConcept(string authorName, IEnumerable<string> stringList, IEnumerable<Concept> conceptList, bool isNegative)
        {
            DateTime dateTime = DateTime.Now;
            ListStringAndConceptMemento memento = new ListStringAndConceptMemento(dateTime, authorName, stringList, conceptList, isNegative);
            mementoList.Add(memento);
        }

        /// <summary>
        /// Create a memento to remember a statement and its author
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="statement">statement to remember</param>
        public override void RememberStatement(string authorName, Statement statement)
        {
            DateTime dateTime = DateTime.Now;
            StatementMemento memento = new StatementMemento(dateTime, authorName, statement);
            mementoList.Add(memento);
        }

        /// <summary>
        /// Create a memento to remember an unspecified comment
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="comment">unspecified string comment</param>
        public override void RememberString(string authorName, string comment)
        {
            DateTime dateTime = DateTime.Now;
            StringMemento memento = new StringMemento(dateTime, authorName, comment);
            mementoList.Add(memento);
        }
        #endregion
    }
}
