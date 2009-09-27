using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents conversations between Ai and Human
    /// </summary>
    class Conversation
    {
        #region Fields
        /// <summary>
        /// Random number generator
        /// </summary>
        private Random random = new Random();
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the statement the Ai wishes to say right now
        /// </summary>
        /// <returns>the statement the Ai wishes to say right now</returns>
        public Statement GetCurrentStatementForAiToAnswerTo()
        {
            int statementType = random.Next(0,3);

            switch (statementType)
            {
                case 0:
                    return new Statement("conversation", Statement.MODE_NULLARY_OPERATOR, "talk");
                case 1:
                    return new Statement("conversation", Statement.MODE_UNARY_OPERATOR, "talkabout","you");
                case 2:
                    return new Statement("conversation", Statement.MODE_UNARY_OPERATOR, "talkabout", "me");
            }
            return null;
        }
        #endregion
    }
}
