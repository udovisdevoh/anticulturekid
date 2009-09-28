using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Builds metaOperation implication statement factory
    /// </summary>
    class MetaOperationImplicationStatementFactory : AbstractStatementFactory
    {
        #region Parts
        /// <summary>
        /// MetaOperation binary implication statement factory
        /// </summary>
        private MetaOperationBinaryImplicationStatementFactory metaOperationBinaryImplicationStatementFactory = new MetaOperationBinaryImplicationStatementFactory();

        /// <summary>
        /// MetaOperation ternary implication statement factory
        /// </summary>
        private MetaOperationTernaryImplicationStatementFactory metaOperationTernaryImplicationStatementFactory = new MetaOperationTernaryImplicationStatementFactory();
        #endregion

        #region Public Methods
        /// <summary>
        /// Build interpreted human statement
        /// </summary>
        /// <param name="humanName">human's name</param>
        /// <param name="humanStatement">human's raw statement</param>
        /// <returns>interpreted human statement</returns>
        public sealed override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            if (humanStatement.Contains(" and "))
                return metaOperationTernaryImplicationStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
            else
                return metaOperationBinaryImplicationStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
        }
        #endregion
    }
}