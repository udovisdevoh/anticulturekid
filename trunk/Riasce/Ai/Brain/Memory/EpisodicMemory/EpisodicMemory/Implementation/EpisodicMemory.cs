using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class EpisodicMemory : AbstractEpisodicMemory
    {
        #region Fields
        private List<AbstractMemento> mementoList = new List<AbstractMemento>();
        #endregion

        #region Public Methods
        public override void RememberOperation(string authorName, List<Concept> conceptList, bool isNegative, bool isInterrogative)
        {
            DateTime dateTime = DateTime.Now;
            OperationMemento memento = new OperationMemento(dateTime, authorName, conceptList, isNegative, isInterrogative);
            mementoList.Add(memento);
        }

        public override void RememberMetaOperation(string authorName, List<Concept> conceptList, string metaOperator, bool isNegative, bool isInterrogative)
        {
            DateTime dateTime = DateTime.Now;
            MetaOperationMemento memento = new MetaOperationMemento(dateTime, authorName, conceptList, metaOperator, isNegative, isInterrogative);
            mementoList.Add(memento);
        }

        public override void RememberListStringAndConcept(string authorName, IEnumerable<string> stringList, IEnumerable<Concept> conceptList, bool isNegative)
        {
            DateTime dateTime = DateTime.Now;
            ListStringAndConceptMemento memento = new ListStringAndConceptMemento(dateTime, authorName, stringList, conceptList, isNegative);
            mementoList.Add(memento);
        }

        public override void RememberStatement(string authorName, Statement statement)
        {
            DateTime dateTime = DateTime.Now;
            StatementMemento memento = new StatementMemento(dateTime, authorName, statement);
            mementoList.Add(memento);
        }

        public override void RememberString(string authorName, string comment)
        {
            DateTime dateTime = DateTime.Now;
            StringMemento memento = new StringMemento(dateTime, authorName, comment);
            mementoList.Add(memento);
        }
        #endregion
    }
}
