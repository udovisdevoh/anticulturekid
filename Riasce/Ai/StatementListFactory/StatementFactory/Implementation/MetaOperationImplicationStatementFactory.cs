using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AntiCulture.Kid
{
    class MetaOperationImplicationStatementFactory : AbstractStatementFactory
    {
        #region Parts
        private MetaOperationBinaryImplicationStatementFactory metaOperationBinaryImplicationStatementFactory = new MetaOperationBinaryImplicationStatementFactory();

        private MetaOperationTernaryImplicationStatementFactory metaOperationTernaryImplicationStatementFactory = new MetaOperationTernaryImplicationStatementFactory();
        #endregion

        #region Methods
        #region Getters
        public sealed override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            if (humanStatement.Contains(" and "))
                return metaOperationTernaryImplicationStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
            else
                return metaOperationBinaryImplicationStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
        }
        #endregion
        #endregion
    }
}