using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using Text;

namespace AntiCulture.Kid
{
    public class Statement
    {
        #region Fields
        /// <summary>
        /// Concept name associated to the human who initiated the statement
        /// </summary>
        private string authorName;

        /// <summary>
        /// Represents the list of concept used for a connection, metaconnect or a unary statement (0, 1, 2 or 3, maybe 4)
        /// </summary>
        private List<string> conceptNameList = new List<string>();

        private string metaOperatorName;

        private string nullaryOrUnaryOperatorName;

        private string namingAssociationOperatorName;

        private string selectCondition;

        private string updateAction;

        private bool isInterrogative;

        private bool isNegative;

        private bool isAskingWhy;

        private bool isAskingVisualizeWhy;

        private bool isAnalogy;

        private bool isStatCross;

        private bool isSelect;

        private bool isUpdate;
        
        private bool isImply;

        private string originalStatement = null;

        private int probability = 100;
        #endregion

        #region Constants
        public const int MODE_NULLARY_OPERATOR = 1;

        public const int MODE_UNARY_OPERATOR = 2;

        public const int MODE_OPERATION = 3;

        public const int MODE_NAMING_ASSOCIATION = 4;

        public const int MODE_META_OPERATION = 5;

        public const int MODE_ANALOGY = 6;

        public const int MODE_STATCROSS = 7;

        public const int MODE_SELECT = 8;

        public const int MODE_UPDATE = 9;

        public const int MODE_EXPLY = 10;
        #endregion

        #region Constructors
        public Statement(string authorName, int mode)
        {
            if (mode == MODE_ANALOGY)
            {
                this.authorName = authorName;
                this.isAnalogy = true;
                this.nullaryOrUnaryOperatorName = "analogize";
            }
            else if (mode == MODE_STATCROSS)
            {
                this.authorName = authorName;
                this.isStatCross = true;
            }
            else
            {
                throw new StatementParsingException("Couln't parse statement mode");
            }
        }

        public Statement(string authorName, string nullaryOrUnaryOperatorName, int probability)
        {
            this.authorName = authorName;
            this.nullaryOrUnaryOperatorName = nullaryOrUnaryOperatorName;
            this.probability = probability;
        }

        public Statement(string authorName, int mode, string name1)
        {
            if (mode == MODE_NULLARY_OPERATOR)
            {
                this.authorName = authorName;
                this.nullaryOrUnaryOperatorName = name1;
            }
            else if (mode == MODE_ANALOGY)
            {
                this.authorName = authorName;
                this.conceptNameList.Add(name1);
                this.isAnalogy = true;
                this.nullaryOrUnaryOperatorName = "analogize";
            }
            else if (mode == MODE_SELECT)
            {
                this.authorName = authorName;
                this.nullaryOrUnaryOperatorName = "select";
                this.isSelect = true;
                this.selectCondition = name1;
            }
            else
            {
                throw new StatementParsingException("Couln't parse statement mode");
            }
        }

        public Statement(string authorName,int mode, string name1, string name2)
        {
            if (mode == MODE_UNARY_OPERATOR)
            {
                this.authorName = authorName;
                this.nullaryOrUnaryOperatorName = name1;
                this.conceptNameList.Add(name2);
            }
            else if (mode == MODE_ANALOGY)
            {
                this.authorName = authorName;
                this.conceptNameList.Add(name1);
                this.conceptNameList.Add(name2);
                this.isAnalogy = true;
            }
            else if (mode == MODE_STATCROSS)
            {
                this.authorName = authorName;
                this.conceptNameList.Add(name1);
                this.conceptNameList.Add(name2);
                this.isStatCross = true;
            }
            else if (mode == MODE_UPDATE)
            {
                this.authorName = authorName;
                this.nullaryOrUnaryOperatorName = "update";
                this.isUpdate = true;
                this.updateAction = name1;
                this.selectCondition = name2;
            }
            else
            {
                throw new StatementParsingException("Couln't parse statement mode");
            }
        }

        public Statement(string authorName, int mode, string name1, string name2, bool isNegative)
        {
            if (mode == MODE_EXPLY)
            {
                this.authorName = authorName;
                this.nullaryOrUnaryOperatorName = "imply";
                this.isImply = true;
                this.updateAction = name1;
                this.selectCondition = name2;
                this.isNegative = isNegative;
            }
            else
            {
                throw new StatementParsingException("Couln't parse statement mode");
            }
        }

        public Statement(string authorName, int mode, string name1, string name2, string name3)
        {
            if (mode == MODE_ANALOGY)
            {
                this.authorName = authorName;
                this.conceptNameList.Add(name1);
                this.conceptNameList.Add(name2);
                this.conceptNameList.Add(name3);
                this.isAnalogy = true;
            }
        }

        public Statement(string authorName, int mode, string name1, string name2, string name3, string name4)
        {
            if (mode == MODE_STATCROSS)
            {
                this.authorName = authorName;
                this.conceptNameList.Add(name1);
                this.conceptNameList.Add(name2);
                this.conceptNameList.Add(name3);
                this.conceptNameList.Add(name4);
                this.isStatCross = true;
            }
        }

        public Statement(string authorName, int mode, string concept1Name, string concept2Name, string concept3Name, bool isInterrogative, bool isNegative, bool isAskingWhy, bool isAskingVisualizeWhy, string originalStatement)
        {
            if (mode != MODE_OPERATION)
                throw new StatementParsingException("Couln't parse statement mode");

            this.authorName = authorName;
            this.conceptNameList.Add(concept1Name);
            this.conceptNameList.Add(concept2Name);
            this.conceptNameList.Add(concept3Name);
            this.isInterrogative = isInterrogative;
            this.isNegative = isNegative;
            this.isAskingWhy = isAskingWhy;
            this.isAskingVisualizeWhy = isAskingVisualizeWhy;
        }

        public Statement(string authorName, int mode, string operatorName, string concept1Name, string concept2Name, bool isNegative, string originalStatement)
        {
            if (mode == MODE_META_OPERATION)
            {
                this.authorName = authorName;
                this.conceptNameList.Add(concept1Name);
                this.conceptNameList.Add(concept2Name);
                this.isNegative = isNegative;
                this.metaOperatorName = operatorName;
                this.originalStatement = originalStatement;
                if (this.isNegative)
                    this.originalStatement = "not " + this.originalStatement;
            }
            else if (mode == MODE_NAMING_ASSOCIATION)
            {
                this.authorName = authorName;
                this.conceptNameList.Add(concept1Name);
                this.conceptNameList.Add(concept2Name);
                this.isNegative = isNegative;
                this.namingAssociationOperatorName = operatorName;
            }
            else
            {
                throw new StatementParsingException("Couln't parse statement mode");
            }
        }
        #endregion

        #region Public Methods
        public string GetConceptName(int id)
        {
            if (conceptNameList.Count < id + 1)
                return null;

            return conceptNameList[id];
        }

        public override bool Equals(Object obj)
        {
            Statement otherStatement = (Statement)obj;
            return this == otherStatement;
        }

        public void RenameConcept(string oldConceptName, string newConceptName)
        {
            for (int i = 0; i < conceptNameList.Count; i++)
                if (conceptNameList[i] == oldConceptName)
                    conceptNameList[i] = newConceptName;
        }

        public override int GetHashCode()
        {
            int value = this.ConceptCount * 10;

            value *= this.conceptNameList.GetHashCode() * 100;

            value ^= this.MetaOperatorName.GetHashCode() * 1000;

            return value;
        }
        #endregion

        #region Operator overloads
        public static bool operator ==(Statement statement1, Statement statement2)
        {
            if (statement1.AuthorName != statement2.AuthorName)
                return false;
            else if (statement1.IsAskingWhy != statement2.IsAskingWhy)
                return false;
            else if (statement1.IsAskingVisualizeWhy != statement2.IsAskingVisualizeWhy)
                return false;
            else if (statement1.IsInterrogative != statement2.IsInterrogative)
                return false;
            else if (statement1.IsNegative != statement2.IsNegative)
                return false;
            else if (statement1.MetaOperatorName != statement2.MetaOperatorName)
                return false;
            else if (statement1.NullaryOrUnaryOperatorName != statement2.NullaryOrUnaryOperatorName)
                return false;
            else if (statement1.ConceptCount != statement2.ConceptCount)
                return false;
            else if (statement1.IsAnalogy != statement2.IsAnalogy)
                return false;
            else if (statement1.IsImply != statement2.IsImply)
                return false;
            else if (statement1.IsSelect != statement2.IsSelect)
                return false;
            else if (statement1.IsStatCross != statement2.IsStatCross)
                return false;
            else if (statement1.IsUpdate != statement2.IsUpdate)
                return false;

            for (int i = 0; i < statement1.ConceptCount; i++)
                if (statement1.GetConceptName(i) != statement2.GetConceptName(i))
                    return false;
            

            return true;
        }

        public static bool operator !=(Statement statement1, Statement statement2)
        {
            return !(statement1 == statement2);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Concept name associated to the human who initiated the statement
        /// </summary>
        public string AuthorName
        {
            get { return authorName; }
        }

        /// <summary>
        /// Could be "yes", "no", "think", "ask", "start", "talk", "teach", "think"
        /// or "askabout", "define", "talkabout", "teachabout", "thinkabout", "whatis"
        /// </summary>
        public string NullaryOrUnaryOperatorName
        {
            get { return nullaryOrUnaryOperatorName; }
        }

        public bool IsInterrogative
        {
            get { return isInterrogative; }
        }

        public bool IsNegative
        {
            get { return isNegative; }
        }

        public bool IsAskingWhy
        {
            get { return isAskingWhy; }
        }

        public bool IsAskingVisualizeWhy
        {
            get { return isAskingVisualizeWhy; }
        }

        public bool IsAnalogy
        {
            get { return isAnalogy;}
        }

        public bool IsStatCross
        {
            get { return isStatCross; }
        }

        public bool IsSelect
        {
            get { return isSelect; }
        }

        public bool IsUpdate
        {
            get { return isUpdate; }
        }

        public bool IsImply
        {
            get { return isImply; }
        }

        /// <summary>
        /// Could be either permutableSide, inverseOf, directImplication, inverseImplication
        /// directCant, inverseCant, directUnlikely, inverseUnlikely
        /// </summary>
        public string MetaOperatorName
        {
            get { return metaOperatorName; }
        }

        /// <summary>
        /// Could be either aliasof or rename
        /// </summary>
        public string NamingAssociationOperatorName
        {
            get { return namingAssociationOperatorName; }
        }

        public int ConceptCount
        {
            get { return this.conceptNameList.Count; }
        }

        public string OriginalStatement
        {
            get { return originalStatement; }
        }

        public string SelectCondition
        {
            get { return selectCondition; }
        }

        public string UpdateAction
        {
            get { return updateAction; }
        }

        public int Probability
        {
            get { return probability; }
        }
        #endregion
    }
}