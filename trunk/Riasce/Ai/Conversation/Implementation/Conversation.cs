using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class Conversation : AbstractConversation
    {
        #region Fields
        private Random random = new Random();
        #endregion

        #region Methods
        public override Statement GetCurrentStatementForAiToAnswerTo()
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
