﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents the single threaded version of the connection flattenizer
    /// </summary>
    class SerialFlattenizer : AbstractFlattenizer
    {
        #region Fields
        /// <summary>
        /// MetaConnection manager
        /// </summary>
        private MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

        /// <summary>
        /// Remember which branches are already repaired to improve performance a lot
        /// </summary>
        private HashSet<ConnectionBranch> repairedBranches;

        /// <summary>
        /// Remember metaConnections to improve performances
        /// </summary>
        private VerbMetaConnectionCache verbConnectionCache;
        #endregion

        #region Public Methods
        /// <summary>
        /// Repair a concept's flat representation and regenerate its optimized representation
        /// so no useless connection persist
        /// </summary>
        /// <param name="concept">Concept to repair</param>
        public override void Repair(Concept subject)
        {
            Repair(subject, new HashSet<ConnectionBranch>(), new VerbMetaConnectionCache());
        }

        /// <summary>
        /// Repair a concept's flat representation and regenerate its optimized representation
        /// so no useless connection persist.
        /// THIS METHOD MUST ONLY BE USED BY REPAIRER CLASS!!!
        /// </summary>
        /// <param name="concept">concept to repair</param>
        /// <param name="repairedBranches">provided HashSet to rememebr which branches were repaired</param>
        /// <param name="verbConnectionCache">provided cache to remember flattenized metaConnections</param>
        public override void Repair(Concept subject, HashSet<ConnectionBranch> repairedBranches, VerbMetaConnectionCache verbConnectionCache)
        {
            this.repairedBranches = repairedBranches;
            this.verbConnectionCache = verbConnectionCache;

            ConnectionBranch flatBranch;
            ConnectionBranch optimizedBranch;
            
            foreach (Concept verb in Memory.TotalVerbList)
            {
                flatBranch = subject.GetFlatConnectionBranch(verb);
                optimizedBranch = subject.GetOptimizedConnectionBranch(verb);
                flatBranch = RepairFlatBranch(flatBranch, optimizedBranch, subject, verb);
            }

            subject.IsFlatDirty = false;
            subject.IsOptimizedDirty = true;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Use internally to repair flat branches from optimized branches
        /// </summary>
        /// <param name="optimizedBranch">source optimized branch</param>
        /// <param name="optimizedBranch">flat branch to repair</param>
        /// <returns>repaired flat branch</returns>
        private ConnectionBranch RepairFlatBranch(ConnectionBranch flatBranch, ConnectionBranch optimizedBranch, Concept subject, Concept verb)
        {
            int complementCount;

            repairedBranches.Add(flatBranch);

            flatBranch.ComplementConceptList.Clear();

            foreach (Concept complement in optimizedBranch.ComplementConceptList)
            {
                flatBranch.AddConnection(complement);
                flatBranch.SetProofTo(complement, new Proof(subject,verb,complement));
            }

            do
            {
                complementCount = flatBranch.ComplementConceptList.Count;
                FlattenDirectImplication(flatBranch, subject, verb);
                FlattenLiffid(flatBranch, subject, verb);
                FlattenMuct(flatBranch, subject, verb);
                FlattenPositiveImply(flatBranch, subject, verb);
                FlattenNegativeImply(flatBranch, subject, verb);
            }
            while (flatBranch.ComplementConceptList.Count != complementCount);

            flatBranch.IsDirty = false;

            return flatBranch;
        }

        /// <summary>
        /// Flatten connections depending on direct implication metaConnections
        /// </summary>
        /// <param name="flatBranch">flat branch</param>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        private void FlattenDirectImplication(ConnectionBranch flatBranch, Concept subject, Concept verb)
        {
            HashSet<Concept> directImplicationVerbList = verbConnectionCache.GetVerbFlatListFromCache(verb, "direct_implication", false);
            if (directImplicationVerbList == null)
            {
                directImplicationVerbList = metaConnectionManager.GetVerbFlatListFromMetaConnection(verb, "direct_implication", false);
                verbConnectionCache.Remember(verb, "direct_implication", false, directImplicationVerbList);
            }
            foreach (Concept directlyImpliedVerb in directImplicationVerbList)
            {
                ConnectionBranch farFlatBranch = subject.GetFlatConnectionBranch(directlyImpliedVerb);
                ConnectionBranch farOptimizedBranch = subject.GetOptimizedConnectionBranch(directlyImpliedVerb);

                if (!repairedBranches.Contains(farFlatBranch))
                    RepairFlatBranch(farFlatBranch, farOptimizedBranch, subject, directlyImpliedVerb);

                foreach (Concept complement in farFlatBranch.ComplementConceptList)
                {
                    if (!subject.GetFlatConnectionBranch(directlyImpliedVerb).GetProofTo(complement).ContainsArgument(subject, verb, complement) &&
                        !subject.GetFlatConnectionBranch(directlyImpliedVerb).GetProofTo(complement).ContainsArgument(subject, directlyImpliedVerb, complement) &&
                        subject != complement)
                    {
                        flatBranch.AddConnection(complement);

                        flatBranch.SetProofTo(complement, new Proof(subject, verb, complement));

                        if (subject.GetFlatConnectionBranch(directlyImpliedVerb).GetProofTo(complement).Count > 0)
                        {
                            if (subject.GetFlatConnectionBranch(directlyImpliedVerb).GetProofTo(complement) != flatBranch.GetProofTo(complement))
                            {
                                flatBranch.GetProofTo(complement).AddProof(subject.GetFlatConnectionBranch(directlyImpliedVerb).GetProofTo(complement));
                            }
                            if (verb != directlyImpliedVerb)
                                flatBranch.GetProofTo(complement).AddArgument(subject, directlyImpliedVerb, complement);
                        }
                        else
                        {
                            if (verb != directlyImpliedVerb)
                                flatBranch.GetProofTo(complement).AddArgument(subject, directlyImpliedVerb, complement);
                        }
                    }

                    if (flatBranch.GetProofTo(complement).ContainsArgument(subject, verb, complement))
                        throw new TotologyException("Trying to prove something with itself");

                    if (flatBranch.GetProofTo(complement).Count > 1)
                    {
                        if (flatBranch.GetProofTo(complement).GetLastWord() != complement || flatBranch.GetProofTo(complement).GetFirstWord() != subject)
                        {
                            throw new TotologyException("This proof is defect");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Flatten connections depending on liffid metaConnections
        /// </summary>
        /// <param name="flatBranch">flat branch</param>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        private void FlattenLiffid(ConnectionBranch flatBranch, Concept subject, Concept verb)
        {
            HashSet<Concept> liffidVerbList = verbConnectionCache.GetVerbFlatListFromCache(verb, "liffid", true);
            if (liffidVerbList == null)
            {
                liffidVerbList = metaConnectionManager.GetVerbFlatListFromMetaConnection(verb, "liffid", true);
                verbConnectionCache.Remember(verb, "liffid", true, liffidVerbList);
            }
            foreach (Concept liffidVerb in liffidVerbList)
            {
                HashSet<Concept> complementList = new HashSet<Concept>(subject.GetFlatConnectionBranch(verb).ComplementConceptList);

                foreach (Concept complement in complementList)
                {
                    ConnectionBranch farFlatBranch = complement.GetFlatConnectionBranch(liffidVerb);
                    ConnectionBranch farOptimizedBranch = complement.GetOptimizedConnectionBranch(liffidVerb);

                    if (!repairedBranches.Contains(farFlatBranch))//if (liffidVerb != verb && subject != complement)
                        RepairFlatBranch(farFlatBranch, farOptimizedBranch, complement, liffidVerb);


                    HashSet<Concept> conceptAffectedByLiffidVerb = farFlatBranch.ComplementConceptList;

                    foreach (Concept metaComplement in conceptAffectedByLiffidVerb)
                    {
                        Proof closeProof = subject.GetFlatConnectionBranch(verb).GetProofTo(complement);
                        Proof farProof = farFlatBranch.GetProofTo(metaComplement);
                        if (!closeProof.ContainsArgument(subject, verb, metaComplement) &&
                            !farProof.ContainsArgument(subject, verb, metaComplement) &&
                            !closeProof.ContainsArgument(subject, verb, complement) &&
                            !farProof.ContainsArgument(complement, liffidVerb, metaComplement) &&
                            subject != metaComplement)
                        {
                            flatBranch.AddConnection(metaComplement);
                            flatBranch.SetProofTo(metaComplement, new Proof(subject, verb, metaComplement));

                            if (closeProof.Count > 1 && closeProof != flatBranch.GetProofTo(metaComplement))
                            {
                                flatBranch.GetProofTo(metaComplement).AddProof(closeProof);
                            }
                            else
                            {
                                if (complement != metaComplement)
                                    flatBranch.GetProofTo(metaComplement).AddArgument(subject, verb, complement);
                            }

                            if (farProof.Count > 1 && farProof != flatBranch.GetProofTo(metaComplement))
                            {
                                flatBranch.GetProofTo(metaComplement).AddProof(farProof);
                            }
                            else
                            {
                                if (verb != liffidVerb)
                                    flatBranch.GetProofTo(metaComplement).AddArgument(complement, liffidVerb, metaComplement);
                            }

                            if (flatBranch.GetProofTo(metaComplement).ContainsArgument(subject, verb, metaComplement))
                                throw new TotologyException("Trying to prove something with itself");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Flatten connections depending on muct metaConnections
        /// </summary>
        /// <param name="flatBranch">flat branch</param>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        private void FlattenMuct(ConnectionBranch flatBranch, Concept subject, Concept verb)
        {
            HashSet<Concept> muctVerbList = verbConnectionCache.GetVerbFlatListFromCache(verb, "muct", true);
            if (muctVerbList == null)
            {
                muctVerbList = metaConnectionManager.GetVerbFlatListFromMetaConnection(verb, "muct", true);
                verbConnectionCache.Remember(verb, "muct", true, muctVerbList);
            }
            foreach (Concept muctVerb in muctVerbList)
            {
                HashSet<Concept> complementList = new HashSet<Concept>(subject.GetFlatConnectionBranch(muctVerb).ComplementConceptList);

                foreach (Concept complement in complementList)
                {
                    ConnectionBranch farFlatBranch = complement.GetFlatConnectionBranch(verb);
                    ConnectionBranch farOptimizedBranch = complement.GetOptimizedConnectionBranch(verb);
                    if (!repairedBranches.Contains(farFlatBranch))//if (muctVerb != verb && subject != complement)
                        RepairFlatBranch(farFlatBranch, farOptimizedBranch, complement, verb);

                    HashSet<Concept> conceptAffectedByMuctVerb = farFlatBranch.ComplementConceptList;

                    foreach (Concept metaComplement in conceptAffectedByMuctVerb)
                    {
                        Proof closeProof = subject.GetFlatConnectionBranch(muctVerb).GetProofTo(complement);
                        Proof farProof = farFlatBranch.GetProofTo(metaComplement);

                        if (!closeProof.ContainsArgument(subject, verb, metaComplement) &&
                            !farProof.ContainsArgument(subject, verb, metaComplement) &&
                            !closeProof.ContainsArgument(subject, muctVerb, complement) &&
                            !farProof.ContainsArgument(complement, verb, metaComplement) &&
                            subject != metaComplement)
                        {
                            flatBranch.AddConnection(metaComplement);

                            flatBranch.SetProofTo(metaComplement, new Proof(subject, verb, metaComplement));

                            if (closeProof.Count > 1 && closeProof != flatBranch.GetProofTo(metaComplement))
                            {
                                flatBranch.GetProofTo(metaComplement).AddProof(closeProof);
                            }
                            else
                            {
                                if (verb != muctVerb || complement != metaComplement)
                                    flatBranch.GetProofTo(metaComplement).AddArgument(subject, muctVerb, complement);
                            }

                            if (farProof.Count > 1 && farProof != flatBranch.GetProofTo(metaComplement))
                            {
                                if (complement != metaComplement)
                                    flatBranch.GetProofTo(metaComplement).AddProof(farProof);
                            }
                            else
                            {
                                if (complement != subject)
                                {
                                    flatBranch.GetProofTo(metaComplement).AddArgument(complement, verb, metaComplement);
                                }
                            }

                            if (flatBranch.GetProofTo(metaComplement).ContainsArgument(subject, verb, metaComplement))
                                throw new TotologyException("Trying to prove something with itself");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Flatten connections depending on positive "Imply" connections
        /// </summary>
        /// <param name="flatBranch">flat branch</param>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        private void FlattenPositiveImply(ConnectionBranch flatBranch, Concept subject, Concept verb)
        {
            Concept complement;
            HashSet<Condition> conditionList;
            foreach (KeyValuePair<Concept, HashSet<Condition>> complementAndConditionList in verb.ImplyConnectionTreePositive)
            {
                complement = complementAndConditionList.Key;
                conditionList = complementAndConditionList.Value;

                foreach (Condition condition in conditionList)
                {
                    TryFlattenConditionPositiveImply(flatBranch, subject, verb, condition);
                }
            }
        }

        /// <summary>
        /// Flatten connections depending on negative "Imply" connections
        /// </summary>
        /// <param name="flatBranch">flat branch</param>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        private void FlattenNegativeImply(ConnectionBranch flatBranch, Concept subject, Concept verb)
        {
            Concept complement;
            HashSet<Condition> conditionList;
            foreach (KeyValuePair<Concept, HashSet<Condition>> complementAndConditionList in verb.ImplyConnectionTreeNegative)
            {
                complement = complementAndConditionList.Key;
                conditionList = complementAndConditionList.Value;

                foreach (Condition condition in conditionList)
                {
                    TryFlattenConditionNegativeImply(flatBranch, subject, verb, condition);
                }
            }
        }

        /// <summary>
        /// Try flatten positive imply condition
        /// </summary>
        /// <param name="flatBranch">flat branch</param>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="condition">imply condition</param>
        private void TryFlattenConditionPositiveImply(ConnectionBranch flatBranch, Concept subject, Concept verb, Condition condition)
        {
            if (subject == condition.ActionComplement) //Cannot plug a concept to itself
                return;

            Proof proof;

            Dictionary<Concept, HashSet<Concept>> flatConnectionSetList = new Dictionary<Concept, HashSet<Concept>>();

            //Get list of branch on which the condition is dependent on
            HashSet<Concept> verbListToFlattenize = condition.GetVerbListToFlattenize();

            //Flatten that list of branches
            foreach (Concept dependantVerb in verbListToFlattenize)
            {
                ConnectionBranch farFlatBranch = subject.GetFlatConnectionBranch(dependantVerb);
                ConnectionBranch farOptimizedBranch = subject.GetOptimizedConnectionBranch(dependantVerb);

                if (!repairedBranches.Contains(farFlatBranch))
                    RepairFlatBranch(farFlatBranch, farOptimizedBranch, subject, dependantVerb);

                flatConnectionSetList.Add(dependantVerb, farFlatBranch.ComplementConceptList);
            }
            
            //If condition is satisfied, add flat connection
            HashSet<ArgumentPrototype> proofPrototype = condition.GetProofPrototype(subject, flatConnectionSetList);
            if (proofPrototype != null)
            {
                flatBranch.AddConnection(condition.ActionComplement);

                proof = GetProofFromPrototype(proofPrototype);

                #warning Next line might cause problem
                if (!proof.ContainsArgument(subject,verb,condition.ActionComplement))
                    flatBranch.SetProofTo(condition.ActionComplement, proof);
                else
                    flatBranch.SetProofTo(condition.ActionComplement, new Proof());
            }
        }
        
        /// <summary>
        /// Try flatten negative imply condition
        /// </summary>
        /// <param name="flatBranch">flat branch</param>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="condition">imply condition</param>
        private void TryFlattenConditionNegativeImply(ConnectionBranch flatBranch, Concept subject, Concept verb, Condition condition)
        {
            if (subject != condition.ActionComplement) //Ignore flattenization if subject is not concerned
                return;

            Proof proof;

            //Get list of branch on which the condition is dependent on
            List<List<Concept>> branchPrototypeToFlattenizeList = condition.GetBranchSignatureListToFlattenize();

            //Flatten that list of branches
            foreach (List<Concept> branchSignature in branchPrototypeToFlattenizeList)
            {
                Concept farVerb = branchSignature[0];
                Concept farComplement = branchSignature[1];

                ConnectionBranch farFlatBranch = farComplement.GetFlatConnectionBranch(farVerb);
                ConnectionBranch farOptimizedBranch = farComplement.GetOptimizedConnectionBranch(farVerb);

                if (!repairedBranches.Contains(farFlatBranch))
                    RepairFlatBranch(farFlatBranch, farOptimizedBranch, farComplement, farVerb);
            }

            //We get the selection from the condition
            Dictionary<Concept,HashSet<ArgumentPrototype>> selection = condition.GetNegativeSelectionWithProofPrototype();

            //We add flat connections to concepts from selection
            foreach (KeyValuePair<Concept,HashSet<ArgumentPrototype>> complementAndProofPrototype in selection)
            {
                Concept complement = complementAndProofPrototype.Key;
                HashSet<ArgumentPrototype> proofPrototype = complementAndProofPrototype.Value;

                if (subject != complement) //Cannot add a connection to itself
                {
                    flatBranch.AddConnection(complement);

                    proof = GetProofFromPrototype(proofPrototype);

                    #warning Next line might cause problem
                    if (!proof.ContainsArgument(subject,verb,complement))
                        flatBranch.SetProofTo(complement, proof);
                    else
                        flatBranch.SetProofTo(complement, new Proof());
                }
            }
        }

        /// <summary>
        /// Get proof object from proof prototype
        /// </summary>
        /// <param name="proofPrototype">provided proof prototype</param>
        /// <returns>proof object</returns>
        private Proof GetProofFromPrototype(HashSet<ArgumentPrototype> proofPrototype)
        {
            Proof proof = new Proof();

            foreach (ArgumentPrototype argumentPrototype in proofPrototype)
            {
                Proof argumentProof = argumentPrototype.Subject.GetFlatConnectionBranch(argumentPrototype.Verb).GetProofTo(argumentPrototype.Complement);
                if (argumentProof.Count > 0)
                    proof.AddProof(argumentProof);

                proof.AddArgument(argumentPrototype.Subject, argumentPrototype.Verb, argumentPrototype.Complement);
            }
            return proof;
        }
        #endregion
    }
}