using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;
using Misc;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents the implementation the statement factory
    /// Its purpose is to convert human generated strings into Statement objects that can be interpreted by an AI
    /// </summary>
    class StatementFactory : AbstractStatementFactory
    {
        #region Parts
        private NullaryStatementFactory nullaryStatementFactory = new NullaryStatementFactory();

        private UnaryStatementFactory unaryStatementFactory = new UnaryStatementFactory();

        private BinaryStatementFactory binaryStatementFactory = new BinaryStatementFactory();

        private MetaOperationStatementFactory metaOperationStatementFactory = new MetaOperationStatementFactory();

        private AliasStatementFactory aliasStatementFactory = new AliasStatementFactory();

        private RenameStatementFactory renameStatementFactory = new RenameStatementFactory();

        private AnalogyStatementFactory analogyStatementFactory = new AnalogyStatementFactory();

        private StatCrossStatementFactory statCrossStatementFactory = new StatCrossStatementFactory();

        private SelectStatementFactory selectStatementFactory = new SelectStatementFactory();

        private UpdateStatementFactory updateStatementFactory = new UpdateStatementFactory();

        private ImplyStatementFactory implyStatementFactory = new ImplyStatementFactory();

        private PsychosisStatementFactory psychosisStatementFactory = new PsychosisStatementFactory();
        #endregion

        #region Public Methods
        /// <summary>
        /// Build parsed statement of any kind from human's raw statement
        /// </summary>
        /// <param name="humanName">human's name</param>
        /// <param name="humanStatement">human's raw statement</param>
        /// <returns>parsed statement of any kind</returns>
        public sealed override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            FunctionArgument.Ensure(humanName, "Human name");
            FunctionArgument.Ensure(humanStatement, "Human statement");

            humanStatement = humanStatement.FixStringForHimmlStatementParsing();

            string[] words = humanStatement.Split(' ');

            if (humanStatement.ContainsWord("aliasof"))
            {
                return aliasStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
            }
            else if (humanStatement.ContainsWord("rename"))
            {
                return renameStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
            }
            else if (humanStatement.ContainsWord("start_psychosis"))
            {
                return psychosisStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
            }
            else if (humanStatement.ContainsWord("analogize"))
            {
                return analogyStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
            }
            else if (humanStatement.ContainsWord("statcross"))
            {
                return statCrossStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
            }
            else if (humanStatement.StartsWith("select where "))
            {
                return selectStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
            }
            else if (humanStatement.StartsWith("update") && humanStatement.ContainsWord("where"))
            {
                return updateStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
            }
            else if (humanStatement.RemoveWord("not").Trim().StartsWith("imply") && humanStatement.ContainsWord("where"))
            {
                return implyStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
            }
            else if (words.Length == 1)
            {
                return nullaryStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
            }
            else if (words.Length == 2)
            {
                return unaryStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
            }
            else if (words.Length > 2 && (humanStatement.Contains("=") || words[0] == "if" || humanStatement.Contains(" then ") || humanStatement.Contains(" and ")))
            {
                return metaOperationStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
            }
            else if (words.Length > 2)
            {
                return binaryStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
            }
            else
            {
                throw new StatementParsingException("Couldn't match human statement " + humanStatement + " to any parsable pattern");
            }
        }
        #endregion
    }
}