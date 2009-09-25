using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace AntiCulture.Kid
{
    abstract class AbstractHumanVisibleCommentBuilder : AbstractVisibleCommentBuilder
    {
        #region Fields
        private bool isInterrogative;

        private bool isAskingWhy;
        
        private bool isAskingVisualizeWhy;

        private bool isNegative;

        private bool isAnalogy;

        private bool isStatCross;

        private bool isSelect;

        private bool isUpdate;

        private bool isImply;

        private string humanName;

        private List<string> conceptOrOperatorNameList;

        private string originalStatementThatGotAltered;

        private string unparsableComment;

        private string selectCondition;

        private string updateAction;

        private string probability;
        #endregion

        #region Methods
        public override sealed Paragraph Build()
        {
            if (humanName == null)
                throw new VisibleCommentConstructionException("HumanName not specified. Specify Human's name before creating visible comment");

            Paragraph outputVisibleComment;
            try
            {
                outputVisibleComment = ConcreteBuild();
            }
            catch (VisibleCommentConstructionException)
            {  
                throw;
            }
            finally
            {
                ResetConstructionSettings();
            }

            return outputVisibleComment;
        }

        public override sealed void ResetConstructionSettings()
        {
            conceptOrOperatorNameList = null;

            isAskingWhy = false;

            isAskingVisualizeWhy = false;

            isInterrogative = false;

            isNegative = false;

            unparsableComment = null;

            originalStatementThatGotAltered = null;

            selectCondition = null;

            updateAction = null;

            isAnalogy = false;

            isStatCross = false;

            isSelect = false;

            isUpdate = false;

            isImply = false;

            probability = null;
        }

        public void SetConstructionSettingsAccordingToStatement(Statement statement)
        {
            HumanName = statement.AuthorName;
            IsAskingWhy = statement.IsAskingWhy;
            IsAskingVisualizeWhy = statement.IsAskingVisualizeWhy;
            IsInterrogative = statement.IsInterrogative;
            IsNegative = statement.IsNegative;
            IsAnalogy = statement.IsAnalogy;
            IsStatCross = statement.IsStatCross;
            IsSelect = statement.IsSelect;
            IsUpdate = statement.IsUpdate;
            isImply = statement.IsImply;
            SelectCondition = statement.SelectCondition;
            UpdateAction = statement.UpdateAction;
            probability = statement.Probability.ToString();

            if (statement.NullaryOrUnaryOperatorName != null)
                ConceptOrOperatorNameList.Add(statement.NullaryOrUnaryOperatorName);

            if (statement.GetConceptName(0) != null)
                ConceptOrOperatorNameList.Add(statement.GetConceptName(0));

            if (statement.NamingAssociationOperatorName != null)
                ConceptOrOperatorNameList.Add(statement.NamingAssociationOperatorName);

            if (statement.MetaOperatorName != null)
                ConceptOrOperatorNameList.Add(statement.MetaOperatorName);

            if (statement.GetConceptName(1) != null)
                ConceptOrOperatorNameList.Add(statement.GetConceptName(1));

            if (statement.GetConceptName(2) != null)
                ConceptOrOperatorNameList.Add(statement.GetConceptName(2));

            if (statement.GetConceptName(3) != null)
                ConceptOrOperatorNameList.Add(statement.GetConceptName(3));

            originalStatementThatGotAltered = statement.OriginalStatement;
        }
        #endregion

        #region Properties
        public bool IsInterrogative
        {
            get { return isInterrogative; }
            set { isInterrogative = value; }
        }

        public bool IsNegative
        {
            get { return isNegative; }
            set { isNegative = value; }
        }

        public bool IsAskingWhy
        {
            get { return isAskingWhy; }
            set { isAskingWhy = value; }
        }

        public bool IsAskingVisualizeWhy
        {
            get { return isAskingVisualizeWhy; }
            set { isAskingVisualizeWhy = value; }
        }

        public bool IsAnalogy
        {
            get { return isAnalogy; }
            set { isAnalogy = value; }
        }

        public bool IsStatCross
        {
            get { return isStatCross; }
            set { isStatCross = value; }
        }

        public bool IsSelect
        {
            get { return isSelect; }
            set { isSelect = value; }
        }

        public bool IsUpdate
        {
            get { return isUpdate; }
            set { isUpdate = value; }
        }

        public bool IsImply
        {
            get { return isImply; }
            set { isImply = value; }
        }

        public string HumanName
        {
            get { return humanName; }
            set { humanName = value; }
        }

        public List<string> ConceptOrOperatorNameList
        {
            get
            {
                if (conceptOrOperatorNameList == null)
                    conceptOrOperatorNameList = new List<string>();
                return conceptOrOperatorNameList;
            }
            set { conceptOrOperatorNameList = value; }
        }

        public string UnparsableComment
        {
            get { return unparsableComment; }
            set { unparsableComment = value; }
        }

        public string OriginalStatementThatGotAltered
        {
            get { return originalStatementThatGotAltered; }
            set { originalStatementThatGotAltered = value; }
        }

        public string SelectCondition
        {
            get { return selectCondition; }
            set { selectCondition = value; }
        }

        public string UpdateAction
        {
            get { return updateAction; }
            set { updateAction = value; }
        }

        public string Probability
        {
            get { return probability; }
            set { probability = value; }
        }
        #endregion
    }
}
