using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Basic knowlege the Ai has about himself
    /// </summary>
    class Ego : AbstractInstinct
    {
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
    }
}
