using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractCommandHistory
    {
        /// <summary>
        /// Add latest user input at the end of history
        /// and move pointer to the end of the history
        /// </summary>
        /// <param name="expression"></param>
        public abstract void Add(string command);

        /// <summary>
        /// Move the pointer up and return the command
        /// </summary>
        /// <returns>command from history</returns>
        public abstract string GetMoveUp();

        /// <summary>
        /// Move the pointer down and return the command
        /// </summary>
        /// <returns>command from history</returns>
        public abstract string GetMoveDown();
    }
}
