using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Riasce
{
    /// <summary>
    /// Represents the Ai's main controller.
    /// </summary>
    abstract class AbstractAi
    {
        /// <summary>
        /// Give a parsed statement so the AI can assimilate, test and learn from it, argument about it,
        /// and reply on it. This is the main controller's main use case selection box
        /// </summary>
        /// <param name="statement">interpreted statement object</param>
        protected abstract void ChooseDecisionFromStatement(Statement statement);

        /// <summary>
        /// Start the AI program
        /// </summary>
        public abstract void Start();
    }
}
