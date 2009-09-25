using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class CommandHistory : AbstractCommandHistory
    {
        #region Fields
        private int pointer = 0;

        private List<string> commandList = new List<string>();
        #endregion

        #region Methods
        public override void Add(string command)
        {
            if (commandList.Count > 0 && commandList[commandList.Count - 1] == command)
                return;

            commandList.Add(command);
            pointer = commandList.Count;
        }

        public override string GetMoveUp()
        {
            if (commandList.Count == 0)
                return "";

            pointer--;
            if (pointer < 0)
                pointer = 0;

            return commandList[pointer];
        }

        public override string GetMoveDown()
        {
            if (commandList.Count == 0)
                return "";

            pointer++;

            if (pointer >= commandList.Count)
                pointer = commandList.Count - 1;

            return commandList[pointer];
        }
        #endregion
    }
}
