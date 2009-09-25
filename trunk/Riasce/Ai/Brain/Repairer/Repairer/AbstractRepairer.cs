using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public abstract class AbstractRepairer
    {
        /// <summary>
        /// Repair a concept's reciprocal connections, (verbs metaConnected with inverse_of or permutable_side)
        /// (The only connections affected will be the optimized connections)
        /// </summary>
        /// <param name="concept">Concept to repair</param>
        public abstract void Reciprocate(Concept concept);

        /// <summary>
        /// Repair a collection of concepts's reciprocal connections
        /// </summary>
        /// <param name="conceptCollection">collection of concepts</param>
        public abstract void ReciprocateRange(IEnumerable<Concept> conceptCollection);

        /// <summary>
        /// Flatten and optimize a range of concepts
        /// </summary>
        /// <param name="conceptCollection">collection of concept</param>
        public abstract void RepairRange(IEnumerable<Concept> conceptCollection);

        /// <summary>
        /// Repair a concept
        /// </summary>
        /// <param name="conceptToRepair">concept</param>
        public abstract void Repair(Concept conceptToRepair);

        /// <summary>
        /// Repair two concepts
        /// </summary>
        /// <param name="conceptToRepair1">concept1</param>
        /// <param name="conceptToRepair2">concept2</param>
        public abstract void Repair(Concept conceptToRepair1, Concept conceptToRepair2);

        /// <summary>
        /// Repair tree concepts
        /// </summary>
        /// <param name="conceptToRepair1">concept1</param>
        /// <param name="conceptToRepair2">concept2</param>
        /// <param name="conceptToRepair3">concept3</param>
        public abstract void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3);

        /// <summary>
        /// Repair four concepts
        /// </summary>
        /// <param name="conceptToRepair1">concept1</param>
        /// <param name="conceptToRepair2">concept2</param>
        /// <param name="conceptToRepair3">concept3</param>
        /// <param name="conceptToRepair4">concept4</param>
        public abstract void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4);

        /// <summary>
        /// Repair five concepts
        /// </summary>
        /// <param name="conceptToRepair1">concept1</param>
        /// <param name="conceptToRepair2">concept2</param>
        /// <param name="conceptToRepair3">concept3</param>
        /// <param name="conceptToRepair4">concept4</param>
        /// <param name="conceptToRepair5">concept5</param>
        public abstract void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5);

        /// <summary>
        /// Repair six concepts
        /// </summary>
        /// <param name="conceptToRepair1">concept1</param>
        /// <param name="conceptToRepair2">concept2</param>
        /// <param name="conceptToRepair3">concept3</param>
        /// <param name="conceptToRepair4">concept4</param>
        /// <param name="conceptToRepair5">concept5</param>
        /// <param name="conceptToRepair6">concept6</param>
        public abstract void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6);

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
        public abstract void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6, Concept conceptToRepair7);

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
        public abstract void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6, Concept conceptToRepair7, Concept conceptToRepair8);

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
        public abstract void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6, Concept conceptToRepair7, Concept conceptToRepair8, Concept conceptToRepair9);

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
        public abstract void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6, Concept conceptToRepair7, Concept conceptToRepair8, Concept conceptToRepair9, Concept conceptToRepair10);

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
        public abstract void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6, Concept conceptToRepair7, Concept conceptToRepair8, Concept conceptToRepair9, Concept conceptToRepair10, Concept conceptToRepair11);

        /// <summary>
        /// Repair a concept and all concepts flat plugged to it
        /// </summary>
        /// <param name="concept">concept</param>
        public abstract void RepairConceptAndSurrounding(Concept concept);
    }
}
