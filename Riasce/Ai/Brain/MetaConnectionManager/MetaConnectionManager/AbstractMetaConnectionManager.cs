using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public abstract class AbstractMetaConnectionManager
    {
        /// <summary>
        /// Add a metaconnection of type metaOperatorName between concept 1 and concept 2
        /// </summary>
        /// <param name="concept1">concept 1</param>
        /// <param name="metaOperatorName">metaOperator's name</param>
        /// <param name="concept2">concept 2</param>
        public abstract void AddMetaConnection(Concept concept1, string metaOperatorName, Concept concept2);

        /// <summary>
        /// Remove a metaconnection of type metaOperatorName between concept 1 and concept 2
        /// </summary>
        /// <param name="concept1">concept 1</param>
        /// <param name="metaOperatorName">metaOperator's name</param>
        /// <param name="concept2">concept 2</param>
        public abstract void RemoveMetaConnection(Concept concept1, string metaOperatorName, Concept concept2);

        /// <summary>
        /// Test whether concept1 and concept2 are connected using metaoperatorname
        /// </summary>
        /// <param name="concept1">concept 1</param>
        /// <param name="metaOperatorName">metaOperator's name</param>
        /// <param name="concept2">concept 2</param>
        /// <returns>true if connected, else: false</returns>
        public abstract bool IsMetaConnected(Concept concept1, string metaOperatorName, Concept concept2);

        /// <summary>
        /// Test whether concept1 and concept2 are connected using metaoperatorname
        /// The metaConnectionFlattenizer must check recursively whether the connection exist or not
        /// </summary>
        /// <param name="concept1">concept 1</param>
        /// <param name="metaOperatorName">metaOperator's name</param>
        /// <param name="concept2">concept 2</param>
        /// <returns>true if connected, else: false</returns>
        public abstract bool IsFlatMetaConnected(Concept concept1, string metaOperatorName, Concept concept2);

        /// <summary>
        /// Returns a list of verb that are incompatible with verb
        /// </summary>
        /// <param name="verb">verb for which you wish to retrieve incompatible verb list</param>
        /// <param name="strictMode">whether want to consider "unlikely" metaOperator as an incompatibility</param>
        /// <returns>a list of verbs which are incompatible with verb</returns>
        public abstract HashSet<Concept> GetIncompatibleVerbList(Concept verb, bool strictMode);

        /// <summary>
        /// Return a list of verbs that have a metaconnection from verb to x
        /// </summary>
        /// <param name="verb">verb</param>
        /// <param name="metaOperatorName">metaConnection</param>
        /// <param name="ConnectionPositivity">whether the connection from verb to x is positive</param>
        /// <returns>List of metaconnected verbs</returns>
        public abstract HashSet<Concept> GetVerbListFromMetaConnection(Concept verb, string metaOperatorName, bool ConnectionPositivity);

        /// <summary>
        /// Return a list of verbs that have a metaconnection from verb to x
        /// Use the metaConnectionFlattenizer to get a full list recursively
        /// </summary>
        /// <param name="verb">verb</param>
        /// <param name="metaOperatorName">metaConnection</param>
        /// <param name="isMetaConnectionPositive">whether the connection from verb to x is positive</param>
        /// <returns>List of metaconnected verbs</returns>
        public abstract HashSet<Concept> GetVerbFlatListFromMetaConnection(Concept verb, string metaOperatorName, bool isMetaConnectionPositive);

        /// <summary>
        /// Returns a list of verb that are dependant on provided verb.
        /// Basically, if another verb's connection branch might be modified if verb is modified,
        /// the other verb is dependant on verb
        /// </summary>
        /// <param name="verb">verb</param>
        /// <returns>List of verb that are dependant on verb</returns>
        public abstract HashSet<Concept> GetVerbListDependantOn(Concept verb);
    }
}
