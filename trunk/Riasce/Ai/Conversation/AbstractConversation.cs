using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractConversation
    {
        /// <summary>
        /// Returns the statement the Ai wishes to say right now
        /// </summary>
        /// <returns>the statement the Ai wishes to say right now</returns>
        public abstract Statement GetCurrentStatementForAiToAnswerTo();
    }
}
