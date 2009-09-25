using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace AntiCulture.Kid
{
    abstract class AbstractAiVisibleCommentBuilder : AbstractVisibleCommentBuilder
    {
        #region Fields
        private string aiName;

        private string conceptToDefine;

        private string exceptionMessage;

        private string statementStubAskingForRandomConnection;        

        private string statementAcknowledgeAndLearn;

        private string statementAcknowledge;

        private string statementNotAcknowledging;

        private Dictionary<string, List<string>> definitionListWhatis;

        private Dictionary<string, List<string>> definitionListDefine;

        private Dictionary<string, List<string>> definitionListPositiveImply;

        private Dictionary<string, List<string>> definitionListNegativeImply;

        private List<string> proofDefinitionListRegular;

        private List<string> proofDefinitionListPositiveRefutation;

        private List<string> proofDefinitionListRefutation;

        private List<string> proofDefinitionListTeachabout;

        private List<string> proofDefinitionListRefutationTeachabout;

        private List<string> theory;

        private List<string> analogy;

        private bool isDiscardingTheory = false;

        private string feelingMessage;

        private List<string> trauma;

        private double statCrossValue;

        private List<string> statCrossExpression;

        private HashSet<string> selection;

        private string implyConnectionMessage;
        #endregion

        #region Methods
        public override sealed Paragraph Build()
        {
            if (AiName == null)
                throw new VisibleCommentConstructionException("AiName not specified. Specify Ai's name before creating visible comment");

            Paragraph outputVisibleComment;
            try
            {
                outputVisibleComment = ConcreteBuild();
            }
            catch (VisibleCommentConstructionException visibleCommentConstructionException)
            {
                ResetConstructionSettings();
                throw visibleCommentConstructionException;
            }
            ResetConstructionSettings();
            return outputVisibleComment;
        }

        public override void ResetConstructionSettings()
        {
            exceptionMessage = null;
            statementStubAskingForRandomConnection = null;
            statementAcknowledgeAndLearn = null;
            statementAcknowledge = null;
            statementNotAcknowledging = null;
            definitionListWhatis = null;
            definitionListDefine = null;
            definitionListPositiveImply = null;
            definitionListNegativeImply = null;
            proofDefinitionListRegular = null;
            proofDefinitionListRefutation = null;
            proofDefinitionListTeachabout = null;
            proofDefinitionListRefutationTeachabout = null;
            proofDefinitionListPositiveRefutation = null;
            conceptToDefine = null;
            theory = null;
            analogy = null;
            isDiscardingTheory = false;
            feelingMessage = null;
            trauma = null;
            statCrossValue = 0;
            statCrossExpression = null;
            selection = null;
            implyConnectionMessage = null;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Represents the Ai's name
        /// </summary>
        public string AiName
        {
            get { return aiName; }
            set { aiName = value; }
        }

        /// <summary>
        /// Represents the concept to define
        /// (Mandatory for definition answers)
        /// </summary>
        public string ConceptToDefine
        {
            get { return conceptToDefine; }
            set { conceptToDefine = value; }
        }

        /// <summary>
        /// Represents an exception message that can be used to create an answer
        /// </summary>
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
            set { exceptionMessage = value; }
        }

        /// <summary>
        /// Represents a satement stub ([conceptName] [conceptName])
        /// for which the Ai will be asking human to complete with a concept
        /// </summary>
        public string StatementStubAskingForRandomConnection
        {
            get { return statementStubAskingForRandomConnection; }
            set { statementStubAskingForRandomConnection = value; }
        }

        /// <summary>
        /// Represents a statement which the Ai just learned
        /// </summary>
        public string StatementAcknowledgeAndLearn
        {
            get { return statementAcknowledgeAndLearn; }
            set { statementAcknowledgeAndLearn = value; }
        }

        /// <summary>
        /// Represents a statement for which the Ai is acknowledging validity
        /// </summary>
        public string StatementAcknowledge
        {
            get { return statementAcknowledge; }
            set { statementAcknowledge = value; }
        }

        /// <summary>
        /// Represents a statement for which the Ai doesn't acknowledge validity
        /// </summary>
        public string StatementNotAcknowledging
        {
            get { return statementNotAcknowledging; }
            set { statementNotAcknowledging = value; }
        }

        /// <summary>
        /// Represents a formatable "whatis" definition list
        /// Key: operatorConcept
        /// Value: List of concept
        /// Requires property ConceptToDefine
        /// </summary>
        public Dictionary<string, List<string>> DefinitionListWhatis
        {
            get { return definitionListWhatis; }
            set { definitionListWhatis = value; }
        }

        /// <summary>
        /// Represents a formatable "define" definition list
        /// Key: operatorConcept
        /// Value: List of concept
        /// Requires property ConceptToDefine
        /// </summary>
        public Dictionary<string, List<string>> DefinitionListDefine
        {
            get { return definitionListDefine; }
            set { definitionListDefine = value; }
        }

        /// <summary>
        /// Represents a formatable definition list about "Imply" connections
        /// Key: complement Concept
        /// Value: condition list
        /// Requires property ConceptToDefine
        /// </summary>
        public Dictionary<string, List<string>> DefinitionListPositiveImply
        {
            get { return definitionListPositiveImply; }
            set { definitionListPositiveImply = value; }
        }

        /// <summary>
        /// Represents a formatable definition list about "Imply" connections
        /// Key: complement Concept
        /// Value: condition list
        /// Requires property ConceptToDefine
        /// </summary>
        public Dictionary<string, List<string>> DefinitionListNegativeImply
        {
            get { return definitionListNegativeImply; }
            set { definitionListNegativeImply = value; }
        }

        /// <summary>
        /// Represents a formatable list of arguments for a proof
        /// </summary>
        public List<string> ProofDefinitionListRegular
        {
            get { return proofDefinitionListRegular; }
            set { proofDefinitionListRegular = value; }
        }

        /// <summary>
        /// Represents a formatable list of arguments for a refutation proof
        /// </summary>
        public List<string> ProofDefinitionListRefutation
        {
            get { return proofDefinitionListRefutation; }
            set { proofDefinitionListRefutation = value; }
        }

        /// <summary>
        /// Represents a formatable list of arguments for a positive refutation proof
        /// </summary>
        public List<string> ProofDefinitionListPositiveRefutation
        {
            get { return proofDefinitionListPositiveRefutation; }
            set { proofDefinitionListPositiveRefutation = value; }
        }

        /// <summary>
        /// Represents a formatable list of arguments for a teaching proof
        /// </summary>
        public List<string> ProofDefinitionListTeachabout
        {
            get { return proofDefinitionListTeachabout; }
            set { proofDefinitionListTeachabout = value; }
        }

        /// <summary>
        /// Represents a formatable list of arguments for a teaching refutation
        /// </summary>
        public List<string> ProofDefinitionListRefutationTeachabout
        {
            get { return proofDefinitionListRefutationTeachabout; }
            set { proofDefinitionListRefutationTeachabout = value; }
        }

        /// <summary>
        /// Represents a list of argument an the final hypothesis
        /// </summary>
        public List<string> Theory
        {
            get { return theory; }
            set { theory = value; }
        }

        /// <summary>
        /// Represents a connection and an analogy to it
        /// </summary>
        public List<string> Analogy
        {
            get { return analogy; }
            set { analogy = value; }
        }
        
        /// <summary>
        /// Whether we are discarding the current theory or not
        /// </summary>
        public bool IsDiscardingTheory
        {
            get { return isDiscardingTheory; }
            set {isDiscardingTheory = value; }
        }

        /// <summary>
        /// How the Ai feels
        /// </summary>
        public string FeelingMessage
        {
            get { return feelingMessage; }
            set { feelingMessage = value; }
        }

        /// <summary>
        /// Trauma
        /// </summary>
        public List<string> Trauma
        {
            get { return trauma; }
            set { trauma = value; }
        }

        /// <summary>
        /// Probability from cross statistics
        /// </summary>
        public double StatCrossValue
        {
            get { return statCrossValue; }
            set { statCrossValue = value; }
        }

        /// <summary>
        /// Stat cross expression
        /// </summary>
        public List<string> StatCrossExpression
        {
            get { return statCrossExpression; }
            set { statCrossExpression = value; }
        }

        /// <summary>
        /// AiSql select output (concept name list)
        /// </summary>
        public HashSet<string> Selection
        {
            get { return selection; }
            set { selection = value; }
        }

        public string ImplyConnectionMessage
        {
            get { return implyConnectionMessage; }
            set { implyConnectionMessage = value; }
        }
        #endregion
    }
}
