using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractEpisodicMemory
    {
        /// <summary>
        /// Create a memento to remember an operation and its author
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="conceptList">concept list</param>
        /// <param name="isNegative">whether the connection is negative or not</param>
        /// <param name="isInterrogative">whether the connection is interrogative or not</param>
        public abstract void RememberOperation(string authorName, List<Concept> conceptList, bool isNegative, bool isInterrogative);

        /// <summary>
        /// Create a memento to remember an metaOperation and its author
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="conceptList">concept list</param>
        /// <param name="metaOperator">metaOperator's name</param>
        /// <param name="isNegative">whether the metaConnection is negative or not</param>
        /// <param name="isInterrogative">whether the metaConnection is interrogative or not</param>
        public abstract void RememberMetaOperation(string authorName, List<Concept> conceptList, string metaOperator, bool isNegative, bool isInterrogative);

        /// <summary>
        /// Create a memento to remember a statement and its author
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="statement">statement to remember</param>
        public abstract void RememberStatement(string authorName, Statement statement);

        /// <summary>
        /// Create a memento to remember a statement and its author
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="stringList">a list of string (could be unary operators or stuff like that)</param>
        /// <param name="conceptList">a list of concept</param>
        /// <param name="isNegative">whether the "connection" is negative or not</param>
        public abstract void RememberListStringAndConcept(string authorName, IEnumerable<string> stringList, IEnumerable<Concept> conceptList, bool isNegative);

        /// <summary>
        /// Create a memento to remember an unspecified comment
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="comment">unspecified string comment</param>
        public abstract void RememberString(string authorName, string comment);
    }
}
