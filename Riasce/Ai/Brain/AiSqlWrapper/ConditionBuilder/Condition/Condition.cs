using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents conditions that can be used by "Select", "Update" and "Imply"
    /// inside stored "Imply" connections
    /// It is a conditional binary tree similar to Specification design pattern
    /// </summary>
    public class Condition : IEquatable<Condition>
    {
        #region Fields
        /// <summary>
        /// Left child connection (only used when condition is a node)
        /// </summary>
        private Condition leftChild;

        /// <summary>
        /// Right child connection (only used when condition is a node)
        /// </summary>
        private Condition rightChild;

        /// <summary>
        /// Logical operator (and, or, and not) in between left child and right child (only used when condition is a node)
        /// </summary>
        private string logicOperator;

        /// <summary>
        /// Atomic condition (leaf)'s verb (only used when condition is a leaf)
        /// </summary>
        private Concept verb;

        /// <summary>
        /// Atomic complement (leaf)'s verb (only used when condition is a leaf)
        /// </summary>
        private Concept complement;

        /// <summary>
        /// When condition is true, add connection using action verb
        /// </summary>
        private Concept actionVerb;

        /// <summary>
        /// When condition is true, add connection using action complement
        /// </summary>
        private Concept actionComplement;

        /// <summary>
        /// Whether condition is direct (true) or contraposate (false (for inverse_of verbs))
        /// </summary>
        private bool isPositive;
        #endregion

        #region Constructors
        /// <summary>
        /// Create composite condition from existing component conditions
        /// </summary>
        /// <param name="leftChild">left condition</param>
        /// <param name="logicOperator">and, or, and not</param>
        /// <param name="rightChild">right condition</param>
        public Condition(Condition leftChild, string logicOperator, Condition rightChild)
        {
            this.leftChild = leftChild;
            this.logicOperator = logicOperator;
            this.rightChild = rightChild;
        }

        /// <summary>
        /// Create atomic condition from verb and complement
        /// </summary>
        /// <param name="verb">verb</param>
        /// <param name="complement">complement</param>
        public Condition(Concept verb, Concept complement)
        {
            this.verb = verb;
            this.complement = complement;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Whether the conditions are equivalent
        /// </summary>
        /// <param name="other">other condition</param>
        /// <returns>Whether the conditions are equivalent</returns>
        public bool Equals(Condition other)
        {
            Condition otherCondition = (Condition)other;

            if (verb != null && complement != null)
            {
                if (verb == otherCondition.verb && complement == otherCondition.complement)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (leftChild != null && rightChild != null && logicOperator != null)
            {
                if (logicOperator == otherCondition.logicOperator && leftChild.Equals(otherCondition.leftChild) && rightChild.Equals(otherCondition.rightChild))
                {
                    return true;
                }
                else if (logicOperator == otherCondition.logicOperator && logicOperator != "and not" && leftChild.Equals(otherCondition.rightChild) && rightChild.Equals(otherCondition.leftChild))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new ConditionException("Invalid exception (null verb or complement or childs)");
            }
        }

        /// <summary>
        /// Returns true if condition equals or contains verb and complement
        /// </summary>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <returns>true if condition equals or contains verb and complement, else: false</returns>
        public bool IsOrContains(Concept verb, Concept complement)
        {
            if (this.verb != null && this.complement != null)
            {
                if (verb == this.verb && complement == this.complement)
                    return true;
                else
                    return false;
            }
            else
            {
                if (leftChild.IsOrContains(verb, complement))
                    return true;
                else if (rightChild.IsOrContains(verb, complement))
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Returns the list of verbs for which their connection branches must be
        /// flattenized before testing whether the condition is satisfied or not
        /// </summary>
        /// <returns>the list of verbs for which their connection branches must be
        /// flattenized before testing whether the condition is satisfied or not</returns>
        public HashSet<Concept> GetVerbListToFlattenize()
        {
            HashSet<Concept> verbListToFlattenize = new HashSet<Concept>();
            if (verb != null)
            {
                verbListToFlattenize.Add(verb);
            }
            else
            {
                verbListToFlattenize.UnionWith(leftChild.GetVerbListToFlattenize());
                verbListToFlattenize.UnionWith(rightChild.GetVerbListToFlattenize());
            }

            return verbListToFlattenize;
        }

        /// <summary>
        /// For negative imply, return a list of connection branches signature (subject + verb)
        /// that should be flattenized before flattening the imply connection
        /// </summary>
        /// <returns>a list of connection branches signature (subject + verb)
        /// that should be flattenized before flattening the implt connection</returns>
        public List<List<Concept>> GetBranchSignatureListToFlattenize()
        {
            List<List<Concept>> branchSignatureListToFlattenize = new List<List<Concept>>();
            if (verb != null)
            {
                branchSignatureListToFlattenize.Add(new List<Concept>(){verb, complement});
            }
            else
            {
                branchSignatureListToFlattenize.AddRange(leftChild.GetBranchSignatureListToFlattenize());
                branchSignatureListToFlattenize.AddRange(rightChild.GetBranchSignatureListToFlattenize());
            }
            return branchSignatureListToFlattenize;
        }

        /// <summary>
        /// Return a list of argument prototypes as a proof prototype
        /// or null if condition is not satisfied
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="flatConnectionSetList">flat connection set list</param>
        /// <returns>Return a list of argument prototypes as a proof prototype
        /// or null if condition is not satisfied</returns>
        public HashSet<ArgumentPrototype> GetProofPrototype(Concept subject, Dictionary<Concept, HashSet<Concept>> flatConnectionSetList)
        {
            HashSet<ArgumentPrototype> proofPrototype = new HashSet<ArgumentPrototype>();

            if (verb != null && complement != null)
            {
                if (ContainsConnection(flatConnectionSetList, verb, complement))
                {
                    proofPrototype.Add(new ArgumentPrototype(subject, verb, complement));
                    return proofPrototype;
                }
                else
                {
                    return null;
                }
            }
            else if (leftChild != null && rightChild != null && logicOperator != null)
            {
                if (logicOperator == "or")
                {
                    proofPrototype = leftChild.GetProofPrototype(subject, flatConnectionSetList);
                    if (proofPrototype == null)
                        proofPrototype = rightChild.GetProofPrototype(subject, flatConnectionSetList);
                    return proofPrototype;
                }
                else if (logicOperator == "and")
                {
                    HashSet<ArgumentPrototype> leftProofPrototype = leftChild.GetProofPrototype(subject, flatConnectionSetList);
                    HashSet<ArgumentPrototype> rightProofPrototype = rightChild.GetProofPrototype(subject, flatConnectionSetList);
                    if (leftProofPrototype != null && rightProofPrototype != null)
                    {
                        leftProofPrototype.UnionWith(rightProofPrototype);
                        return leftProofPrototype;
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (logicOperator == "and not")
                {
                    HashSet<ArgumentPrototype> leftProofPrototype = leftChild.GetProofPrototype(subject, flatConnectionSetList);
                    HashSet<ArgumentPrototype> rightProofPrototype = rightChild.GetProofPrototype(subject, flatConnectionSetList);
                    if (leftProofPrototype != null && rightProofPrototype == null)
                    {
                        return leftProofPrototype;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    throw new ConditionException("Invalid logic operator: " + logicOperator);
                }
            }
            else
            {
                throw new ConditionException("Invalid condition");
            }
        }

        /// <summary>
        /// Get a string representation of condition
        /// </summary>
        /// <param name="nameMapper">name mapper to look into</param>
        /// <param name="memory">memory to look into</param>
        /// <param name="isPositive">whether the condition should be positive</param>
        /// <returns>string representation of condition</returns>
        public string ToString(NameMapper nameMapper, Memory memory, bool isPositive)
        {
            if (verb != null && complement != null)
            {
                int verbId, complementId;
                string verbName, complementName;

                verbId = memory.GetIdFromConcept(verb);
                complementId = memory.GetIdFromConcept(complement);

                verbName = nameMapper.GetConceptNames(verbId)[0];
                complementName = nameMapper.GetConceptNames(complementId)[0];

                if (isPositive)
                    return verbName + " " + complementName;
                else
                    return complementName + " " + verbName;
            }
            else if (leftChild != null && rightChild != null && logicOperator != null)
            {
                return "(" + leftChild.ToString(nameMapper, memory,isPositive) + ") " + logicOperator + " (" + rightChild.ToString(nameMapper, memory,isPositive) + ")";
            }
            else
            {
                throw new ConditionException("Invalid condition");
            }
        }

        /// <summary>
        /// Return a reversed version of the condition
        /// </summary>
        /// <returns>Reversed condition</returns>
        public Condition Reverse()
        {
            if (verb != null && complement != null)
            {
                HashSet<Concept> inverseVerbList = verb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("inverse_of");
                inverseVerbList.UnionWith(verb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("permutable_side"));

                foreach (Concept inverseVerb in inverseVerbList)
                {
                    Condition reverseCondition = new Condition(inverseVerb, complement);
                    reverseCondition.ActionVerb = ActionVerb;
                    reverseCondition.ActionComplement = ActionComplement;
                    return reverseCondition;
                }
                 
                throw new ConditionException("Cannot create conditions that contain operators with no metaConnection of type permutable_side or inverse_of");
            }
            else if (leftChild != null && rightChild != null && logicOperator != null)
            {
                return new Condition(leftChild.Reverse(), logicOperator, rightChild.Reverse());
            }
            else
            {
                throw new ConditionException("Invalid condition");
            }
        }

        /// <summary>
        /// Returns the list of concepts and operators that are present in the condition
        /// </summary>
        /// <returns>the list of concepts and operators that are present in the condition</returns>
        public HashSet<Concept> GetConcernedConceptList()
        {
            HashSet<Concept> concernedConceptList = new HashSet<Concept>();

            if (verb != null && complement != null)
            {
                concernedConceptList.Add(verb);
                concernedConceptList.Add(complement);
            }
            else if (leftChild != null && rightChild != null && logicOperator != null)
            {
                concernedConceptList.UnionWith(leftChild.GetConcernedConceptList());
                concernedConceptList.UnionWith(rightChild.GetConcernedConceptList());
            }
            else
            {
                throw new ConditionException("Invalid condition");
            }

            if (actionComplement == null || actionVerb == null)
                throw new ConditionException("Invalid condition, missing action verb or action complement");

            concernedConceptList.Add(actionComplement);
            concernedConceptList.Add(actionVerb);

            return concernedConceptList;
        }

        /// <summary>
        /// Return a new condition with replaced old concept to new concept
        /// </summary>
        /// <param name="oldConcept">old concept</param>
        /// <param name="newConcept">new concept</param>
        /// <returns>a new condition with replaced old concept to new concept</returns>
        public Condition ReplaceConcept(Concept oldConcept, Concept newConcept)
        {
            Condition newCondition;
            Concept newVerb, newComplement;

            if (verb != null && complement != null)
            {
                newVerb = verb;
                newComplement = complement;

                if (newVerb == oldConcept)
                    newVerb = newConcept;

                if (newComplement == oldConcept)
                    newComplement = newConcept;

                newCondition = new Condition(newVerb, newComplement);
            }
            else if (leftChild != null && rightChild != null && logicOperator != null)
            {
                newCondition = new Condition(leftChild.ReplaceConcept(oldConcept, newConcept), logicOperator, rightChild.ReplaceConcept(oldConcept, newConcept));
            }
            else
            {
                throw new ConditionException("Invalid condition");
            }

            newCondition.isPositive = this.isPositive;
            newCondition.actionVerb = this.actionVerb;
            newCondition.actionComplement = this.actionComplement;

            if (newCondition.actionVerb == oldConcept)
                newCondition.actionVerb = newConcept;

            if (newCondition.actionComplement == oldConcept)
                newCondition.actionComplement = newConcept;

            return newCondition;
        }

        /// <summary>
        /// Get serializable representation of condition
        /// </summary>
        /// <param name="memory">memory to look into</param>
        /// <returns>Serializable representation of condition</returns>
        public ConditionInfo GetSerializableInfo(Memory memory)
        {
            int verbId;
            int complementId;
            int actionVerbId = 0;
            int actionComplementId = 0;
            bool isActionVerbExist = false;
            bool isActionComplementExist = false;

            if (verb != null && complement != null)
            {
                verbId = memory.GetIdFromConcept(verb);
                complementId = memory.GetIdFromConcept(complement);

                if (actionVerb != null)
                {
                    isActionVerbExist = true;
                    actionVerbId = memory.GetIdFromConcept(actionVerb);
                }

                if (actionComplement != null)
                {
                    isActionComplementExist = true;
                    actionComplementId = memory.GetIdFromConcept(actionComplement);
                }

                return new ConditionInfo(verbId, complementId, actionVerbId, actionComplementId, isActionVerbExist, isActionComplementExist, isPositive);
            }
            else if (leftChild != null && rightChild != null && logicOperator != null)
            {
                return new ConditionInfo(leftChild.GetSerializableInfo(memory), logicOperator, rightChild.GetSerializableInfo(memory),isPositive);
            }
            else
            {
                throw new ConditionException("Invalid condition");
            }
        }

        /// <summary>
        /// Returns a list of concept from selection in pair with a proof prototype for connection to exist
        /// Negative means that the complement in condition will be considered "subject" when doing the selection
        /// </summary>
        /// <returns>a list of concept from selection in pair with a proof prototype for connection to exist</returns>
        public Dictionary<Concept, HashSet<ArgumentPrototype>> GetNegativeSelectionWithProofPrototype()
        {
            Dictionary<Concept, HashSet<ArgumentPrototype>> centerNegativeSelection = new Dictionary<Concept, HashSet<ArgumentPrototype>>();
            Dictionary<Concept, HashSet<ArgumentPrototype>> leftNegativeSelection;
            Dictionary<Concept, HashSet<ArgumentPrototype>> rightNegativeSelection;

            if (complement != null && verb != null)
            {
                foreach (Concept selectedConcept in complement.GetFlatConnectionBranch(verb).ComplementConceptList)
                    centerNegativeSelection.Add(selectedConcept, new HashSet<ArgumentPrototype>() { new ArgumentPrototype(complement, verb, selectedConcept) });
            }
            else if (logicOperator == "and not")
            {
                leftNegativeSelection = leftChild.GetNegativeSelectionWithProofPrototype();
                rightNegativeSelection = rightChild.GetNegativeSelectionWithProofPrototype();

                foreach (Concept conceptToRemove in rightNegativeSelection.Keys)
                    leftNegativeSelection.Remove(conceptToRemove);

                centerNegativeSelection = leftNegativeSelection;
            }
            else if (logicOperator == "or")
            {
                leftNegativeSelection = leftChild.GetNegativeSelectionWithProofPrototype();
                rightNegativeSelection = rightChild.GetNegativeSelectionWithProofPrototype();

                foreach (KeyValuePair<Concept, HashSet<ArgumentPrototype>> selectedConceptAndProofPrototype in rightNegativeSelection)
                    if (!leftNegativeSelection.ContainsKey(selectedConceptAndProofPrototype.Key))
                        leftNegativeSelection.Add(selectedConceptAndProofPrototype.Key, selectedConceptAndProofPrototype.Value);
                

                centerNegativeSelection = leftNegativeSelection;
            }
            else if (logicOperator == "and")
            {
                leftNegativeSelection = leftChild.GetNegativeSelectionWithProofPrototype();
                rightNegativeSelection = rightChild.GetNegativeSelectionWithProofPrototype();

                foreach (KeyValuePair<Concept, HashSet<ArgumentPrototype>> selectedConceptAndProofPrototype in leftNegativeSelection)
                {
                    Concept selectedConcept = selectedConceptAndProofPrototype.Key;
                    HashSet<ArgumentPrototype> leftProofPrototype = selectedConceptAndProofPrototype.Value;

                    if (rightNegativeSelection.ContainsKey(selectedConcept))
                    {
                        HashSet<ArgumentPrototype> rightProofPrototype = rightNegativeSelection[selectedConcept];
                        rightProofPrototype.UnionWith(leftProofPrototype);

                        centerNegativeSelection.Add(selectedConcept, rightProofPrototype);
                    }
                }
            }
            else
            {
                throw new ConditionException("invalid condition");
            }

            return centerNegativeSelection;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Used to determine whether an atomic condition is satisfied by a bunch of flat connections
        /// </summary>
        /// <param name="flatConnectionSetList">bunch of flat connections</param>
        /// <param name="verb">atomic condtion's verb</param>
        /// <param name="complement">atomic condtion's complement</param>
        /// <returns>true if it's satisfied, else: false</returns>
        private bool ContainsConnection(Dictionary<Concept, HashSet<Concept>> flatConnectionSetList, Concept verb, Concept complement)
        {
            HashSet<Concept> complementList;
            if (flatConnectionSetList.TryGetValue(verb, out complementList))
                if (complementList.Contains(complement))
                    return true;
            return false;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Indexed verb for the condition's action
        /// </summary>
        public Concept ActionVerb
        {
            get { return actionVerb; }
            set
            {
                actionVerb = value;
                if (leftChild != null)
                    leftChild.ActionVerb = value;
                if (rightChild != null)
                    rightChild.ActionVerb = value;
            }
        }

        /// <summary>
        /// Indexed complement for the condition's action
        /// </summary>
        public Concept ActionComplement
        {
            get { return actionComplement; }
            set
            {
                actionComplement = value;
                if (leftChild != null)
                    leftChild.ActionComplement = value;
                if (rightChild != null)
                    rightChild.ActionComplement = value;
            }
        }

        /// <summary>
        /// Whether the condition is in positive imply connection or not
        /// </summary>
        public bool IsPositive
        {
            get { return isPositive; }
            set { isPositive = value; }
        }
        #endregion
    }
}