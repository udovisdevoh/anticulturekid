using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents info about an "imply" condition connection
    /// </summary>
    [Serializable]
    public class ConditionInfo
    {
        #region Fields
        /// <summary>
        /// Condition verb's id
        /// </summary>
        private int verbId;

        /// <summary>
        /// Condition complement's id
        /// </summary>
        private int complementId;

        /// <summary>
        /// Action verb id
        /// </summary>
        private int actionVerbId;

        /// <summary>
        /// Action complement id
        /// </summary>
        private int actionComplementId;

        /// <summary>
        /// Whether action verb exists
        /// </summary>
        private bool isActionVerbExist;
        
        /// <summary>
        /// Whether action complement exisits
        /// </summary>
        private bool isActionComplementExist;

        /// <summary>
        /// Imply condition's sense
        /// </summary>
        private bool isPositive;

        /// <summary>
        /// Left child (condition is a binary tree, specification pattern)
        /// </summary>
        private ConditionInfo leftChild;

        /// <summary>
        /// Right child (condition is a binary tree, specification pattern)
        /// </summary>
        private ConditionInfo rightChild;

        /// <summary>
        /// Logic operator ("and", "or", "and not")
        /// </summary>
        private string logicOperator;
        #endregion

        #region Constructor
        public ConditionInfo()
        {
            this.verbId = -1;
            this.complementId = -1;
            this.actionVerbId = -1;
            this.actionComplementId = -1;
            this.isActionVerbExist = true;
            this.isActionComplementExist = true;
            this.isPositive = true;
        }

        public ConditionInfo(int verbId, int complementId, int actionVerbId, int actionComplementId, bool isActionVerbExist, bool isActionComplementExist, bool isPositive)
        {
            this.verbId = verbId;
            this.complementId = complementId;
            this.actionVerbId = actionVerbId;
            this.actionComplementId = actionComplementId;
            this.isActionVerbExist = isActionVerbExist;
            this.isActionComplementExist = isActionComplementExist;
            this.isPositive = isPositive;
        }

        public ConditionInfo(ConditionInfo leftChild, string logicOperator, ConditionInfo rightChild,bool isPositive)
        {
            this.leftChild = leftChild;
            this.logicOperator = logicOperator;
            this.rightChild = rightChild;
            this.isPositive = isPositive;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Regenerate real condition object in memory
        /// </summary>
        /// <param name="memory">provided memory</param>
        /// <returns>Regenerated real condition object in memory</returns>
        public Condition RegenerateRealCondition(Memory memory)
        {
            Condition condition;
            if (leftChild == null && rightChild == null && logicOperator == null)
            {
                condition = new Condition(memory.GetOrCreateConcept(verbId), memory.GetOrCreateConcept(complementId));
            }
            else
            {
                condition = new Condition(leftChild.RegenerateRealCondition(memory), logicOperator, rightChild.RegenerateRealCondition(memory));
            }

            condition.IsPositive = isPositive;
            if (isActionVerbExist)
                condition.ActionVerb = memory.GetOrCreateConcept(actionVerbId);
            if (isActionComplementExist)
                condition.ActionComplement = memory.GetOrCreateConcept(actionComplementId);

            return condition;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Condition verb's id
        /// </summary>
        public int VerbId
        {
            get { return verbId; }
            set { verbId = value; }
        }

        /// <summary>
        /// Condition complement's id
        /// </summary>
        public int ComplementId
        {
            get { return complementId; }
            set { complementId = value; }
        }

        /// <summary>
        /// Action verb id
        /// </summary>
        public int ActionVerbId
        {
            get { return actionVerbId; }
            set { actionVerbId = value; isActionVerbExist = true; }
        }

        /// <summary>
        /// Action complement id
        /// </summary>
        public int ActionComplementId
        {
            get { return actionComplementId; }
            set { actionComplementId = value; isActionComplementExist = true; }
        }

        /// <summary>
        /// Whether action verb exists
        /// </summary>
        public bool IsActionVerbExist
        {
            get { return isActionVerbExist; }
            set { isActionVerbExist = value; }
        }

        /// <summary>
        /// Whether action complement exisits
        /// </summary>
        public bool IsActionComplementExist
        {
            get { return isActionComplementExist; }
            set { isActionComplementExist = value; }
        }

        /// <summary>
        /// Imply condition's sense
        /// </summary>
        public bool IsPositive
        {
            get { return isPositive; }
            set { isPositive = value; }
        }

        /// <summary>
        /// Logic operator ("and", "or", "and not")
        /// </summary>
        public string LogicOperator
        {
            get { return logicOperator; }
            set { logicOperator = value; }
        }

        /// <summary>
        /// Left child (condition is a binary tree, specification pattern)
        /// </summary>
        public ConditionInfo LeftChild
        {
            get { return leftChild; }
            set { leftChild = value; }
        }

        /// <summary>
        /// Right child (condition is a binary tree, specification pattern)
        /// </summary>
        public ConditionInfo RightChild
        {
            get { return rightChild; }
            set { rightChild = value; }
        }
        #endregion
    }
}
