using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class PsychosisStatementFactory : AbstractStatementFactory
    {
        #region Public Methods
        public override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            int probability;

            string[] words = humanStatement.Split(' ');

            if (words.Length == 2)
            {
                int.TryParse(words[1], out probability);
            }
            else
            {
                probability = 97;
            }

            return new Statement(humanName, "start_psychosis", probability);
        }
        #endregion
    }
}
