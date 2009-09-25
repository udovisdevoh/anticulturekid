using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    [Serializable]
    public class ConditionInfo
    {
        #region Fields
        private int verbId;

        private int complementId;

        private int actionVerbId;

        private int actionComplementId;

        private bool isActionVerbExist;
        
        private bool isActionComplementExist;

        private bool isPositive;

        private ConditionInfo leftChild;

        private ConditionInfo rightChild;

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
        public int VerbId
        {
            get { return verbId; }
            set { verbId = value; }
        }

        public int ComplementId
        {
            get { return complementId; }
            set { complementId = value; }
        }

        public int ActionVerbId
        {
            get { return actionVerbId; }
            set { actionVerbId = value; isActionVerbExist = true; }
        }

        public int ActionComplementId
        {
            get { return actionComplementId; }
            set { actionComplementId = value; isActionComplementExist = true; }
        }

        public bool IsActionVerbExist
        {
            get { return isActionVerbExist; }
            set { isActionVerbExist = value; }
        }

        public bool IsActionComplementExist
        {
            get { return isActionComplementExist; }
            set { isActionComplementExist = value; }
        }

        public bool IsPositive
        {
            get { return isPositive; }
            set { isPositive = value; }
        }

        public string LogicOperator
        {
            get { return logicOperator; }
            set { logicOperator = value; }
        }

        public ConditionInfo LeftChild
        {
            get { return leftChild; }
            set { leftChild = value; }
        }

        public ConditionInfo RightChild
        {
            get { return rightChild; }
            set { rightChild = value; }
        }
        #endregion
    }
}
