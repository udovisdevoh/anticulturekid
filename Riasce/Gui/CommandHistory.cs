using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a command history for the console
    /// </summary>
    class CommandHistory
    {
        #region Fields
        /// <summary>
        /// Position in history
        /// </summary>
        private int pointer = 0;

        /// <summary>
        /// Command list
        /// </summary>
        private List<string> commandList = new List<string>();
        #endregion

        #region Methods
        /// <summary>
        /// Add latest user input at the end of history
        /// and move pointer to the end of the history
        /// </summary>
        /// <param name="expression"></param>
        public void Add(string command)
        {
            if (commandList.Count > 0 && commandList[commandList.Count - 1] == command)
                return;

            commandList.Add(command);
            pointer = commandList.Count;
        }

        /// <summary>
        /// Move the pointer up and return the command
        /// </summary>
        /// <returns>command from history</returns>
        public string GetMoveUp()
        {
            if (commandList.Count == 0)
                return "";

            pointer--;
            if (pointer < 0)
                pointer = 0;

            return commandList[pointer];
        }

        /// <summary>
        /// Move the pointer down and return the command
        /// </summary>
        /// <returns>command from history</returns>
        public string GetMoveDown()
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
