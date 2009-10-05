using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class respresents a module used to alter the brain from statements
    /// </summary>
    class Alterator
    {
        #region Fields
        /// <summary>
        /// Persistant reference to the brain (must reflect reference in Ai)
        /// </summary>
        private Brain brain;

        /// <summary>
        /// Persistant reference to nameMapper (must reflect reference in Ai)
        /// </summary>
        private NameMapper nameMapper;
        #endregion

        #region Constructor
        public Alterator(Brain brain, NameMapper nameMapper)
        {
            this.brain = brain;
            this.nameMapper = nameMapper;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Use provided statement to alter brain
        /// </summary>
        /// <param name="statement">provided statement (from human)</param>
        /// <returns>null || Trauma (if alteration cause a trauma, else: null)</returns>
        public Trauma TryAlterBrainFromStatement(Statement statement)
        {
            Trauma trauma = null; //Trauma is null by default (we will return this variable)

            if (statement.IsInterrogative || statement.IsAnalogy || statement.IsStatCross || statement.IsSelect || statement.IsUpdate)
                return null;

            if (statement.IsImply) //If statement is imply
            {
                TryAlterBrainFromStatementImply(statement);
            }
            else if (statement.GetConceptName(2) != null) //If statement is operation
            {
                trauma = TryAlterBrainFromStatementOperation(statement);
            }
            else if (statement.MetaOperatorName != null) //If statement is metaoperation
            {
                trauma = TryAlterBrainFromStatementMetaOperation(statement);
            }
            else if (statement.NamingAssociationOperatorName == "aliasof")
            {
                TryAlterBrainFromStatementAliasOf(statement);
            }
            else if (statement.NamingAssociationOperatorName == "rename")
            {
                TryAlterBrainFromStatementRename(statement);
            }
            return trauma;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Use provided statement to alter brain by adding or removing a connection
        /// </summary>
        /// <param name="statement">provided operation statement (from human)</param>
        /// <returns>null || Trauma (if alteration cause a trauma, else: null)</returns>
        private Trauma TryAlterBrainFromStatementOperation(Statement statement)
        {
            Trauma trauma = null;
            if (statement.IsNegative)
            {
                brain.TryRemoveConnection(
                    nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                    nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)),
                    nameMapper.GetOrCreateConceptId(statement.GetConceptName(2))
                    );
            }
            else
            {
                try
                {
                    trauma = brain.TryAddConnection(
                        nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                        nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)),
                        nameMapper.GetOrCreateConceptId(statement.GetConceptName(2))
                        );
                }
                catch (CyclicFlatBranchDependencyException e)
                {
                    string newMessage = "Connection: " + statement.GetConceptName(0) + " " + statement.GetConceptName(1) + " " + statement.GetConceptName(2) + " was rejected because it creates an infinite dependency loop";
                    if (e.ProofStackTraceById != null)
                        newMessage += "\nTrace: " + e.GetProofStackTraceByString(nameMapper);

                    throw new CyclicFlatBranchDependencyException(newMessage);
                }
            }
            return trauma;
        }

        /// <summary>
        /// Use provided statement to alter brain by adding or removing a metaConnection
        /// </summary>
        /// <param name="statement">provided operation statement (from human)</param>
        /// <returns>null || Trauma (if alteration cause a trauma, else: null)</returns>
        private Trauma TryAlterBrainFromStatementMetaOperation(Statement statement)
        {
            Trauma trauma = null;
            if (statement.IsNegative)
            {
                brain.TryRemoveMetaConnection(
                    nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                    statement.MetaOperatorName,
                    nameMapper.GetOrCreateConceptId(statement.GetConceptName(1))
                    );
            }
            else
            {
                try
                {
                    trauma = brain.TryAddMetaConnection(
                        nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)),
                        statement.MetaOperatorName,
                        nameMapper.GetOrCreateConceptId(statement.GetConceptName(1))
                        );
                }
                catch (CyclicFlatBranchDependencyException e)
                {
                    string newMessage = "MetaConnection: " + statement.GetConceptName(0) + " " + statement.MetaOperatorName + " " + statement.GetConceptName(1) + " was rejected because it creates an infinite dependency loop";
                    if (e.ProofStackTraceById != null)
                        newMessage += "\nTrace: " + e.GetProofStackTraceByString(nameMapper);

                    throw new CyclicFlatBranchDependencyException(newMessage);
                }
            }
            return trauma;
        }

        /// <summary>
        /// Use provided statement to rename a concept
        /// </summary>
        /// <param name="statement">provided operation statement (from human)</param>
        private void TryAlterBrainFromStatementRename(Statement statement)
        {
            if (!statement.IsNegative)
            {
                nameMapper.Rename(statement.GetConceptName(0), statement.GetConceptName(1));
            }
        }

        /// <summary>
        /// Use provided statement to alias or unalias 2 concepts
        /// </summary>
        /// <param name="statement">provided operation statement (from human)</param>
        private void TryAlterBrainFromStatementAliasOf(Statement statement)
        {
            if (statement.GetConceptName(0) == statement.GetConceptName(1))
                throw new StatementParsingException("Cannot use alias with exact same name. (Press ESC to close autocomplete)");

            if (statement.IsNegative)
            {
                int conceptId0 = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));
                int conceptId1 = nameMapper.RemoveAliasAndGetSecondConceptId(statement.GetConceptName(0), statement.GetConceptName(1));
                brain.RemoveAlias(conceptId0, conceptId1);
            }
            else
            {
                brain.AddAlias(nameMapper.GetOrCreateConceptId(statement.GetConceptName(0)), nameMapper.GetOrCreateConceptId(statement.GetConceptName(1)));
                nameMapper.AddAlias(statement.GetConceptName(0), statement.GetConceptName(1));
            }
        }

        /// <summary>
        /// Use provided statement to alter brain by adding or removing an "imply" connection
        /// </summary>
        /// <param name="statement">provided operation statement (from human)</param>
        private void TryAlterBrainFromStatementImply(Statement statement)
        {
            if (statement.IsNegative)
                brain.RemoveImplyConnection(statement.UpdateAction, statement.SelectCondition, nameMapper);
            else
                brain.AddImplyConnection(statement.UpdateAction, statement.SelectCondition, nameMapper);
        }
        #endregion
    }
}
