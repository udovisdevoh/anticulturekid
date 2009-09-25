using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class Ego : AbstractInstinct
    {
        #region Fields
        private List<string> statementList = new List<string>();
        #endregion

        #region Constructors
        public Ego()
        {
            Add("you isa artificial_intelligence");
            Add("artificial_intelligence isa program");
            Add("program isa software");
            Add("software madeof data");
            Add("data isa concept");
            Add("you make question");
            Add("you make analogy");
            Add("you make theory");
            Add("you make logical_deduction");
            Add("you make proof");
            Add("you madeof memory");
        }
        #endregion

        #region Methods
        public override IEnumerator<string> GetEnumerator()
        {
            return statementList.GetEnumerator();
        }

        private void Add(string statement)
        {
            statementList.Add(statement.ToLower().Trim());
        }
        #endregion

        #region Properties
        public override int Count
        {
            get { return statementList.Count; }
        }
        #endregion
    }
}
