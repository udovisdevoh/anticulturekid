﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class ConnectionManager : AbstractConnectionManager
    {
        #region Fields
        private MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

        private bool disableFlattenizeAndOptimizeAndPurify = false;
        #endregion

        #region Plugging and Unplugging
        public override void Plug(Concept subject, Concept verb, Concept complement)
        {
            #region We throw exception if concepts are dirty or if subject is verb or is complement
            if (!disableFlattenizeAndOptimizeAndPurify && (subject.IsFlatDirty || verb.IsFlatDirty || complement.IsFlatDirty))
                throw new ConnectionException("Repair concepts first");
            if (subject == complement)
                throw new ConnectionException("Cannot plug a concept to itself");
            #endregion

            subject.AddOptimizedConnection(verb, complement);
            Memory.TotalVerbList.Add(verb);

            HashSet<Concept> dependantVerbList = metaConnectionManager.GetVerbListDependantOn(verb);

            subject.IsFlatDirty = true;
            complement.IsFlatDirty = true;

            #region We dirthen concepts's plugged to subject for branches which verbs are affected verb
            foreach (ConnectionBranch currentBranch in subject.FlatConnectionBranchList.Values)
                foreach (Concept connectedConcept in currentBranch.ComplementConceptList)
                    connectedConcept.IsFlatDirty = true;
            #endregion

            #region We dirthen concepts's plugged to complement for branches which verbs are affected verb
            foreach (ConnectionBranch currentBranch in complement.FlatConnectionBranchList.Values)
                foreach (Concept connectedConcept in currentBranch.ComplementConceptList)
                    connectedConcept.IsFlatDirty = true;
            #endregion

            #region We add the contraposate connection on complement if subject permutable_side complement
            HashSet<Concept> permutableSideVerbList = metaConnectionManager.GetVerbListFromMetaConnection(verb, "permutable_side", true);
            foreach (Concept permutableSideVerb in permutableSideVerbList)
                complement.AddOptimizedConnection(permutableSideVerb, subject);
            #endregion

            #region We add the contraposate connection on complement if subject inverse_of complement
            HashSet<Concept> inverseOfVerbList = metaConnectionManager.GetVerbListFromMetaConnection(verb, "inverse_of", true);
            foreach (Concept inverseVerb in inverseOfVerbList)
                complement.AddOptimizedConnection(inverseVerb, subject);
            #endregion
        }

        public override void UnPlug(Concept subject, Concept verb, Concept complement)
        {
            #region We throw exception if concepts are dirty or if subject is verb or is complement
            if (!disableFlattenizeAndOptimizeAndPurify && (subject.IsFlatDirty || verb.IsFlatDirty || complement.IsFlatDirty))
                throw new ConnectionException("Repair concepts first");
            if (subject == complement)
                throw new ConnectionException("Cannot plug a concept to itself");
            #endregion

            subject.RemoveOptimizedConnection(verb, complement);

            HashSet<Concept> dependantVerbList = metaConnectionManager.GetVerbListDependantOn(verb);

            #region We dirthen subject concept's branch on verb and dependant verb
            subject.GetFlatConnectionBranch(verb).IsDirty = true;
            foreach (Concept dependantVerb in dependantVerbList)
                subject.GetFlatConnectionBranch(dependantVerb).IsDirty = true;
            #endregion

            #region We dirthen concepts's plugged to subject for branches which verbs are affected verb
            foreach (ConnectionBranch currentBranch in subject.FlatConnectionBranchList.Values)
                foreach (Concept connectedConcept in currentBranch.ComplementConceptList)
                    foreach (Concept dependantVerb in dependantVerbList)
                        connectedConcept.GetFlatConnectionBranch(dependantVerb).IsDirty = true;
            #endregion

            #region We remove the contraposate connection from complement if subject permutable_side complement and we dirthen complement's branches
            HashSet<Concept> permutableSideVerbList = metaConnectionManager.GetVerbListFromMetaConnection(verb, "permutable_side", true);
            foreach (Concept permutableSideVerb in permutableSideVerbList)
            {
                complement.RemoveOptimizedConnection(permutableSideVerb, subject);
                complement.GetFlatConnectionBranch(permutableSideVerb).IsDirty = true;
                dependantVerbList = metaConnectionManager.GetVerbListDependantOn(permutableSideVerb);
                foreach (Concept dependantVerb in dependantVerbList)
                    complement.GetFlatConnectionBranch(dependantVerb).IsDirty = true;
            }
            #endregion

            #region We remove the contraposate connection from complement if subject inverse_of complement and we dirthen complement's branches
            HashSet<Concept> inverseOfVerbList = metaConnectionManager.GetVerbListFromMetaConnection(verb, "inverse_of", true);
            foreach (Concept inverseVerb in inverseOfVerbList)
            {
                complement.RemoveOptimizedConnection(inverseVerb, subject);
                complement.GetFlatConnectionBranch(inverseVerb).IsDirty = true;
                dependantVerbList = metaConnectionManager.GetVerbListDependantOn(inverseVerb);
                foreach (Concept dependantVerb in dependantVerbList)
                    complement.GetFlatConnectionBranch(dependantVerb).IsDirty = true;
            }
            #endregion
        }
        #endregion

        #region Finding Obstructions
        public override List<Concept> FindObstructionToPlug(Concept subject, Concept verb, Concept complement, bool strictMode)
        {
            if (!disableFlattenizeAndOptimizeAndPurify && (subject.IsFlatDirty /*|| verb.IsFlatDirty || complement.IsFlatDirty*/))
                throw new ConnectionException("Repair concepts first");

            foreach (Concept incompatibleVerb in metaConnectionManager.GetIncompatibleVerbList(verb,strictMode))
                if (subject.IsFlatConnectedTo(incompatibleVerb, complement))
                    return new List<Concept>() { subject, incompatibleVerb, complement };

            return null;
        }

        public override List<Concept> FindObstructionToPlugAllowInverseAndPermutableSide(Concept subject, Concept verb, Concept complement, bool strictMode)
        {
            if (!disableFlattenizeAndOptimizeAndPurify && (subject.IsFlatDirty /*|| verb.IsFlatDirty || complement.IsFlatDirty*/))
                throw new ConnectionException("Repair concepts first");

            HashSet<Concept> incompatibleVerbList = metaConnectionManager.GetIncompatibleVerbList(verb, strictMode);

            incompatibleVerbList.ExceptWith(verb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("inverse_of"));
            incompatibleVerbList.ExceptWith(verb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("permutable_side"));

            foreach (Concept incompatibleVerb in incompatibleVerbList)
                if (subject.IsFlatConnectedTo(incompatibleVerb, complement))
                    return new List<Concept>() { subject, incompatibleVerb, complement };

            return null;
        }

        public override int CountObstructionToPlug(Concept subject, Concept verb, Concept complement, bool strictMode)
        {
            int obstructionCount = 0;
            if (!disableFlattenizeAndOptimizeAndPurify && subject.IsFlatDirty)
                throw new ConnectionException("Repair concepts first");

            foreach (Concept incompatibleVerb in metaConnectionManager.GetIncompatibleVerbList(verb, strictMode))
                if (subject.IsFlatConnectedTo(incompatibleVerb, complement))
                    obstructionCount++;

            return obstructionCount;
        }
        #endregion

        #region Testing connection
        public override bool TestConnection(Concept subject, Concept verb, Concept complement)
        {
            bool isConnected;
            if (!disableFlattenizeAndOptimizeAndPurify && (subject.IsFlatDirty || verb.IsFlatDirty || complement.IsFlatDirty))
                throw new ConnectionException("Repair concepts first");

            isConnected = subject.IsFlatConnectedTo(verb, complement);

            HashSet<Concept> permutableSideVerbList = metaConnectionManager.GetVerbListFromMetaConnection(verb, "permutable_side", true);
            foreach (Concept currentVerb in permutableSideVerbList)
                if (complement.IsFlatConnectedTo(currentVerb, subject) != isConnected)
                    throw new ConnectionException("Connection inconsistency between subject, verb and complement");

            HashSet<Concept> inverseOfVerbList = metaConnectionManager.GetVerbListFromMetaConnection(verb, "inverse_of", true);
            foreach (Concept currentVerb in inverseOfVerbList)
                if (complement.IsFlatConnectedTo(currentVerb, subject) != isConnected)
                    throw new ConnectionException("Connection inconsistency between subject, verb and complement");
            
            return isConnected;
        }

        public override Proof GetProofToConnection(Concept subject, Concept verb, Concept complement)
        {
            if (!TestConnection(subject, verb, complement))
                return null;

            return subject.GetFlatConnectionBranch(verb).GetProofTo(complement);
        }
        #endregion

        #region Properties
        public bool DisableFlattenizeAndOptimizeAndPurify
        {
            get { return disableFlattenizeAndOptimizeAndPurify; }
            set { disableFlattenizeAndOptimizeAndPurify = value; }
        }
        #endregion
    }
}