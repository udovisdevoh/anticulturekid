﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used to repair concepts. It is a faceade to component concept repairers such
    /// as Flattenizer, Optimizer and Reciprocator
    /// </summary>
    public class Repairer
    {
        #region Fields
        /// <summary>
        /// Flattenizer
        /// </summary>
        private AbstractFlattenizer flattenizer = new SerialFlattenizer();

        /// <summary>
        /// Optimizer
        /// </summary>
        private Optimizer optimizer = new Optimizer();
        #endregion

        #region Methods
        /// <summary>
        /// Repair a concept
        /// </summary>
        /// <param name="conceptToRepair">concept</param>
        public void Repair(Concept conceptToRepair)
        {
            Repair(conceptToRepair, new HashSet<ConnectionBranch>(), new VerbMetaConnectionCache());
        }

        /// <summary>
        /// Repair two concepts
        /// </summary>
        /// <param name="conceptToRepair1">concept1</param>
        /// <param name="conceptToRepair2">concept2</param>
        public void Repair(Concept conceptToRepair1, Concept conceptToRepair2)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2 });
        }

        /// <summary>
        /// Repair tree concepts
        /// </summary>
        /// <param name="conceptToRepair1">concept1</param>
        /// <param name="conceptToRepair2">concept2</param>
        /// <param name="conceptToRepair3">concept3</param>
        public void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2, conceptToRepair3});
        }

        /// <summary>
        /// Repair four concepts
        /// </summary>
        /// <param name="conceptToRepair1">concept1</param>
        /// <param name="conceptToRepair2">concept2</param>
        /// <param name="conceptToRepair3">concept3</param>
        /// <param name="conceptToRepair4">concept4</param>
        public void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2, conceptToRepair3, conceptToRepair4});
        }

        /// <summary>
        /// Repair five concepts
        /// </summary>
        /// <param name="conceptToRepair1">concept1</param>
        /// <param name="conceptToRepair2">concept2</param>
        /// <param name="conceptToRepair3">concept3</param>
        /// <param name="conceptToRepair4">concept4</param>
        /// <param name="conceptToRepair5">concept5</param>
        public void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2, conceptToRepair3, conceptToRepair4, conceptToRepair5});
        }

        /// <summary>
        /// Repair six concepts
        /// </summary>
        /// <param name="conceptToRepair1">concept1</param>
        /// <param name="conceptToRepair2">concept2</param>
        /// <param name="conceptToRepair3">concept3</param>
        /// <param name="conceptToRepair4">concept4</param>
        /// <param name="conceptToRepair5">concept5</param>
        /// <param name="conceptToRepair6">concept6</param>
        public void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6)
        {
            RepairRange(new List<Concept> {conceptToRepair1,conceptToRepair2,conceptToRepair3,conceptToRepair4,conceptToRepair5,conceptToRepair6});
        }

        /// <summary>
        /// Repair seven concepts
        /// </summary>
        /// <param name="conceptToRepair1">concept1</param>
        /// <param name="conceptToRepair2">concept2</param>
        /// <param name="conceptToRepair3">concept3</param>
        /// <param name="conceptToRepair4">concept4</param>
        /// <param name="conceptToRepair5">concept5</param>
        /// <param name="conceptToRepair6">concept6</param>
        /// <param name="conceptToRepair7">concept7</param>
        public void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6, Concept conceptToRepair7)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2, conceptToRepair3, conceptToRepair4, conceptToRepair5, conceptToRepair6, conceptToRepair7 });
        }

        /// <summary>
        /// Repair eight concepts
        /// </summary>
        /// <param name="conceptToRepair1">concept1</param>
        /// <param name="conceptToRepair2">concept2</param>
        /// <param name="conceptToRepair3">concept3</param>
        /// <param name="conceptToRepair4">concept4</param>
        /// <param name="conceptToRepair5">concept5</param>
        /// <param name="conceptToRepair6">concept6</param>
        /// <param name="conceptToRepair7">concept7</param>
        /// <param name="conceptToRepair8">concept8</param>
        public void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6, Concept conceptToRepair7, Concept conceptToRepair8)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2, conceptToRepair3, conceptToRepair4, conceptToRepair5, conceptToRepair6, conceptToRepair7, conceptToRepair8 });
        }

        /// <summary>
        /// Repair nine concepts
        /// </summary>
        /// <param name="conceptToRepair1">concept1</param>
        /// <param name="conceptToRepair2">concept2</param>
        /// <param name="conceptToRepair3">concept3</param>
        /// <param name="conceptToRepair4">concept4</param>
        /// <param name="conceptToRepair5">concept5</param>
        /// <param name="conceptToRepair6">concept6</param>
        /// <param name="conceptToRepair7">concept7</param>
        /// <param name="conceptToRepair8">concept8</param>
        /// <param name="conceptToRepair8">concept9</param>
        public void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6, Concept conceptToRepair7, Concept conceptToRepair8, Concept conceptToRepair9)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2, conceptToRepair3, conceptToRepair4, conceptToRepair5, conceptToRepair6, conceptToRepair7, conceptToRepair8, conceptToRepair9 });
        }

        /// <summary>
        /// Repair ten concepts
        /// </summary>
        /// <param name="conceptToRepair1">concept1</param>
        /// <param name="conceptToRepair2">concept2</param>
        /// <param name="conceptToRepair3">concept3</param>
        /// <param name="conceptToRepair4">concept4</param>
        /// <param name="conceptToRepair5">concept5</param>
        /// <param name="conceptToRepair6">concept6</param>
        /// <param name="conceptToRepair7">concept7</param>
        /// <param name="conceptToRepair8">concept8</param>
        /// <param name="conceptToRepair8">concept9</param>
        /// <param name="conceptToRepair8">concept10</param>
        public void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6, Concept conceptToRepair7, Concept conceptToRepair8, Concept conceptToRepair9, Concept conceptToRepair10)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2, conceptToRepair3, conceptToRepair4, conceptToRepair5, conceptToRepair6, conceptToRepair7, conceptToRepair8, conceptToRepair9, conceptToRepair10 });
        }

        /// <summary>
        /// Repair eleven concepts
        /// </summary>
        /// <param name="conceptToRepair1">concept1</param>
        /// <param name="conceptToRepair2">concept2</param>
        /// <param name="conceptToRepair3">concept3</param>
        /// <param name="conceptToRepair4">concept4</param>
        /// <param name="conceptToRepair5">concept5</param>
        /// <param name="conceptToRepair6">concept6</param>
        /// <param name="conceptToRepair7">concept7</param>
        /// <param name="conceptToRepair8">concept8</param>
        /// <param name="conceptToRepair8">concept9</param>
        /// <param name="conceptToRepair8">concept10</param>
        /// <param name="conceptToRepair8">concept11</param>
        public void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6, Concept conceptToRepair7, Concept conceptToRepair8, Concept conceptToRepair9, Concept conceptToRepair10, Concept conceptToRepair11)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2, conceptToRepair3, conceptToRepair4, conceptToRepair5, conceptToRepair6, conceptToRepair7, conceptToRepair8, conceptToRepair9, conceptToRepair10, conceptToRepair11 });
        }

        /// <summary>
        /// Flatten and optimize a range of concepts
        /// </summary>
        /// <param name="conceptCollection">collection of concept</param>
        public void RepairRange(IEnumerable<Concept> conceptCollection)
        {
            HashSet<ConnectionBranch> repairedBranches = new HashSet<ConnectionBranch>();
            VerbMetaConnectionCache verbConnectionCache = new VerbMetaConnectionCache();

            foreach (Concept conceptToRepair in conceptCollection)
                Repair(conceptToRepair, repairedBranches, verbConnectionCache);
        }

        /// <summary>
        /// Repair a concept
        /// </summary>
        /// <param name="conceptToRepair">concept to repair</param>
        /// <param name="repairedBranches">repaired branches</param>
        /// <param name="verbMetaConnectionCache">verb metaConnection cache</param>
        public void Repair(Concept conceptToRepair, HashSet<ConnectionBranch> repairedBranches, VerbMetaConnectionCache verbMetaConnectionCache)
        {
            flattenizer.Repair(conceptToRepair, repairedBranches, verbMetaConnectionCache);
            optimizer.Repair(conceptToRepair);
        }

        /// <summary>
        /// Repair a concept's reciprocal connections, (verbs metaConnected with inverse_of or permutable_side)
        /// (The only connections affected will be the optimized connections)
        /// </summary>
        /// <param name="concept">Concept to repair</param>
        public void Reciprocate(Concept concept)
        {
            Reciprocator.Reciprocate(concept);
        }

        /// <summary>
        /// Repair a collection of concepts's reciprocal connections
        /// </summary>
        /// <param name="conceptCollection">collection of concepts</param>
        public void ReciprocateRange(IEnumerable<Concept> conceptCollection)
        {
            Reciprocator.ReciprocateRange(conceptCollection);
        }

        /// <summary>
        /// Repair a concept and all concepts flat plugged to it
        /// </summary>
        /// <param name="concept">concept</param>
        public void RepairConceptAndSurrounding(Concept concept)
        {
            HashSet<ConnectionBranch> repairedBranches = new HashSet<ConnectionBranch>();
            VerbMetaConnectionCache verbConnectionCache = new VerbMetaConnectionCache();

            Repair(concept, repairedBranches, verbConnectionCache);

            foreach (Concept complement in concept.ConceptFlatPluggedTo)
            {
                Repair(complement, repairedBranches, verbConnectionCache);
            }
        }
        #endregion
    }
}
