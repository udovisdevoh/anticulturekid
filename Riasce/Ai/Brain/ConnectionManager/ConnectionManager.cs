﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Connection manager
    /// (handles connections/disconnections)
    /// </summary>
    static class ConnectionManager
    {
        #region Fields
        /// <summary>
        /// By default: false. Use this to disable repair after adding/removing connections
        /// </summary>
        private static bool disableFlattenizeAndOptimizeAndPurify = false;
        #endregion

        #region Plugging and Unplugging
        /// <summary>
        /// Plug subject concept to complement concept using verb concept
        /// Perform subsequent connection to contraposates
        /// Dirten affected concepts
        /// Doesn't check whether connection can be done or not (please use FindObstructionToPlug() method before)
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        public static void Plug(Concept subject, Concept verb, Concept complement)
        {
            #region We throw exception if concepts are dirty or if subject is verb or is complement
            if (!disableFlattenizeAndOptimizeAndPurify && (subject.IsFlatDirty || verb.IsFlatDirty || complement.IsFlatDirty))
                throw new ConnectionException("Repair concepts first");
            if (subject == complement)
                throw new ConnectionException("Cannot plug a concept to itself");
            #endregion

            if (subject.AddOptimizedConnection(verb, complement))
                RepairedFlatBranchCache.Clear();

            Memory.TotalVerbList.Add(verb);

            HashSet<Concept> dependantVerbList = MetaConnectionManager.GetVerbListDependantOn(verb);

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
            HashSet<Concept> permutableSideVerbList = MetaConnectionManager.GetVerbListFromMetaConnection(verb, "permutable_side", true);
            foreach (Concept permutableSideVerb in permutableSideVerbList)
                if (complement.AddOptimizedConnection(permutableSideVerb, subject))
                    RepairedFlatBranchCache.Clear();
            #endregion

            #region We add the contraposate connection on complement if subject inverse_of complement
            HashSet<Concept> inverseOfVerbList = MetaConnectionManager.GetVerbListFromMetaConnection(verb, "inverse_of", true);
            foreach (Concept inverseVerb in inverseOfVerbList)
                if (complement.AddOptimizedConnection(inverseVerb, subject))
                    RepairedFlatBranchCache.Clear();
            #endregion
        }

        /// <summary>
        /// UnPlug subject concept from complement concept using verb concept
        /// Unplug subsequent connection to contraposates
        /// Dirten affected concepts
        /// Doesn't check whether disconnection can be done or not (please use FindObstructionToUnPlug() method before)
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        public static void UnPlug(Concept subject, Concept verb, Concept complement)
        {
            #region We throw exception if concepts are dirty or if subject is verb or is complement
            if (!disableFlattenizeAndOptimizeAndPurify && (subject.IsFlatDirty || verb.IsFlatDirty || complement.IsFlatDirty))
                throw new ConnectionException("Repair concepts first");
            if (subject == complement)
                throw new ConnectionException("Cannot plug a concept to itself");
            #endregion

            if (subject.RemoveOptimizedConnection(verb, complement))
                RepairedFlatBranchCache.Clear();

            HashSet<Concept> dependantVerbList = MetaConnectionManager.GetVerbListDependantOn(verb);

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
            HashSet<Concept> permutableSideVerbList = MetaConnectionManager.GetVerbListFromMetaConnection(verb, "permutable_side", true);
            foreach (Concept permutableSideVerb in permutableSideVerbList)
            {
                if (complement.RemoveOptimizedConnection(permutableSideVerb, subject))
                    RepairedFlatBranchCache.Clear();
                complement.GetFlatConnectionBranch(permutableSideVerb).IsDirty = true;
                dependantVerbList = MetaConnectionManager.GetVerbListDependantOn(permutableSideVerb);
                foreach (Concept dependantVerb in dependantVerbList)
                    complement.GetFlatConnectionBranch(dependantVerb).IsDirty = true;
            }
            #endregion

            #region We remove the contraposate connection from complement if subject inverse_of complement and we dirthen complement's branches
            HashSet<Concept> inverseOfVerbList = MetaConnectionManager.GetVerbListFromMetaConnection(verb, "inverse_of", true);
            foreach (Concept inverseVerb in inverseOfVerbList)
            {
                if (complement.RemoveOptimizedConnection(inverseVerb, subject))
                    RepairedFlatBranchCache.Clear();
                complement.GetFlatConnectionBranch(inverseVerb).IsDirty = true;
                dependantVerbList = MetaConnectionManager.GetVerbListDependantOn(inverseVerb);
                foreach (Concept dependantVerb in dependantVerbList)
                    complement.GetFlatConnectionBranch(dependantVerb).IsDirty = true;
            }
            #endregion
        }
        #endregion

        #region Finding Obstructions
        /// <summary>
        /// Returns a connection that prevents to plug specified concepts
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <param name="strictMode">if true, consider unlikeliness as an obstruction, if not, don't</param>
        /// <returns>List of 3 concepts (subject, verb, complement) that prevents to connect specified concepts
        /// Return null if couldn't find anything obstructing specified connection</returns>
        public static List<Concept> FindObstructionToPlug(Concept subject, Concept verb, Concept complement, bool strictMode)
        {
            if (!disableFlattenizeAndOptimizeAndPurify && (subject.IsFlatDirty /*|| verb.IsFlatDirty || complement.IsFlatDirty*/))
                throw new ConnectionException("Repair concepts first");

            foreach (Concept incompatibleVerb in MetaConnectionManager.GetIncompatibleVerbList(verb,strictMode))
                if (subject.IsFlatConnectedTo(incompatibleVerb, complement))
                    return new List<Concept>() { subject, incompatibleVerb, complement };

            return null;
        }

        /// <summary>
        /// Returns a connection that prevents to plug specified concepts, but ignore obstruction for verbs that are merely inverse_of the other
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <param name="strictMode">if true, consider unlikeliness as an obstruction, if not, don't</param>
        /// <returns>List of 3 concepts (subject, verb, complement) that prevents to connect specified concepts
        /// Return null if couldn't find anything obstructing specified connection</returns>
        public static List<Concept> FindObstructionToPlugAllowInverseAndPermutableSide(Concept subject, Concept verb, Concept complement, bool strictMode)
        {
            if (!disableFlattenizeAndOptimizeAndPurify && (subject.IsFlatDirty /*|| verb.IsFlatDirty || complement.IsFlatDirty*/))
                throw new ConnectionException("Repair concepts first");

            HashSet<Concept> incompatibleVerbList = MetaConnectionManager.GetIncompatibleVerbList(verb, strictMode);

            incompatibleVerbList.ExceptWith(verb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("inverse_of"));
            incompatibleVerbList.ExceptWith(verb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection("permutable_side"));

            foreach (Concept incompatibleVerb in incompatibleVerbList)
                if (subject.IsFlatConnectedTo(incompatibleVerb, complement))
                    return new List<Concept>() { subject, incompatibleVerb, complement };

            return null;
        }

        /// <summary>
        /// Returns the amount of possible flat connections that prevent provided connection to be possible
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <param name="strictMode">if true, consider unlikeliness as an obstruction, if not, don't</param>
        /// <returns>the amount of possible flat connections that prevent provided connection to be possible</returns>
        public static int CountObstructionToPlug(Concept subject, Concept verb, Concept complement, bool strictMode)
        {
            int obstructionCount = 0;
            if (!disableFlattenizeAndOptimizeAndPurify && subject.IsFlatDirty)
                throw new ConnectionException("Repair concepts first");

            foreach (Concept incompatibleVerb in MetaConnectionManager.GetIncompatibleVerbList(verb, strictMode))
                if (subject.IsFlatConnectedTo(incompatibleVerb, complement))
                    obstructionCount++;

            return obstructionCount;
        }
        #endregion

        #region Testing connection
        /// <summary>
        /// Returns true if connection currently exist (in flat representation)
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <returns>True if connection exist, false if not</returns>
        public static bool TestConnection(Concept subject, Concept verb, Concept complement)
        {
            bool isConnected;
            if (!disableFlattenizeAndOptimizeAndPurify && (subject.IsFlatDirty || verb.IsFlatDirty || complement.IsFlatDirty))
                throw new ConnectionException("Repair concepts first");

            isConnected = subject.IsFlatConnectedTo(verb, complement);

            HashSet<Concept> permutableSideVerbList = MetaConnectionManager.GetVerbListFromMetaConnection(verb, "permutable_side", true);
            foreach (Concept currentVerb in permutableSideVerbList)
                if (complement.IsFlatConnectedTo(currentVerb, subject) != isConnected)
                    throw new ConnectionException("Connection inconsistency between subject, verb and complement");

            HashSet<Concept> inverseOfVerbList = MetaConnectionManager.GetVerbListFromMetaConnection(verb, "inverse_of", true);
            foreach (Concept currentVerb in inverseOfVerbList)
                if (complement.IsFlatConnectedTo(currentVerb, subject) != isConnected)
                    throw new ConnectionException("Connection inconsistency between subject, verb and complement");
            
            return isConnected;
        }

        /// <summary>
        /// Returns a proof to provided connection
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        /// <returns>Proof object, null if connection doesn't exist</returns>
        public static Proof GetProofToConnection(Concept subject, Concept verb, Concept complement)
        {
            if (!TestConnection(subject, verb, complement))
                return null;

            return subject.GetFlatConnectionBranch(verb).GetProofTo(complement);
        }
        #endregion

        #region Properties
        /// <summary>
        /// By default: false. Use this to disable repair after adding/removing connections
        /// </summary>
        public static bool DisableFlattenizeAndOptimizeAndPurify
        {
            get { return disableFlattenizeAndOptimizeAndPurify; }
            set { disableFlattenizeAndOptimizeAndPurify = value; }
        }
        #endregion
    }
}