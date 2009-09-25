using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractTheory : IEquatable<Theory>, IComparable<Theory>
    {
        /// <summary>
        /// Add a connection argument to theory
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <param name="complement">complement concept</param>
        public abstract void AddConnectionArgument(Concept subject, Concept verb, Concept complement);

        /// <summary>
        /// Add a metaConnection argument to theory
        /// </summary>
        /// <param name="operator1">operator 1</param>
        /// <param name="metaOperatorName">metaOperator name</param>
        /// <param name="operator2">operator 2</param>
        public abstract void AddMetaConnectionArgument(Concept operator1, string metaOperatorName, Concept operator2);

        /// <summary>
        /// Add a range of metaConneciton argument to theory
        /// </summary>
        /// <param name="argumentList">argument list</param>
        public abstract void AddMetaConnectionArgumentRange(List<MetaConnectionArgument> argumentList);

        /// <summary>
        /// Return true if both theories are identical (they may not have the same arguments though)
        /// </summary>
        /// <param name="other">other theory</param>
        /// <returns>true if both theories are identical, else: false</returns>
        public abstract bool Equals(Theory other);

        /// <summary>
        /// Returns the concept at id conceptId
        /// </summary>
        /// <param name="conceptId">concept's id</param>
        /// <returns>concept at id or null if doesn't have that concept count</returns>
        public abstract Concept GetConcept(int conceptId);

        /// <summary>
        /// Returns the metaOperator name. Can be null if it's a connection theory
        /// </summary>
        public abstract string MetaOperatorName
        {
            get;
        }

        /// <summary>
        /// Count how many connection arguments are in theory
        /// </summary>
        public abstract int CountConnectionArgument
        {
            get;
        }

        /// <summary>
        /// Count how many metaConnection arguments are in theory
        /// </summary>
        public abstract int CountMetaConnectionArgument
        {
            get;
        }

        /// <summary>
        /// Returns Connection argument from provided id
        /// </summary>
        /// <param name="argumentId">argument's id</param>
        /// <returns>Connection argument from provided id</returns>
        public abstract List<Concept> GetConnectionArgumentAt(int argumentId);

        /// <summary>
        /// Returns metaConnection argument from provided id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>metaConnection argument from provided id</returns>
        public abstract MetaConnectionArgument GetMetaConnectionArgumentAt(int id);

        /// <summary>
        /// The expected probability
        /// </summary>
        public abstract double PredictedProbability
        {
            get;
        }

        /// <summary>
        /// Return a string list representation of theory
        /// </summary>
        /// <param name="nameMapper">name mapper</param>
        /// <param name="memory">memory</param>
        /// <returns>string list representation of theory</returns>
        public abstract List<string> ToStringList(NameMapper nameMapper, Memory memory);

        /// <summary>
        /// Convert a theory to a statement
        /// </summary>
        /// <param name="nameMapper">name mapper</param>
        /// <param name="memory">memory</param>
        /// <returns>Statement object</returns>
        public abstract Statement ToStatement(NameMapper nameMapper, Memory memory);

        public abstract int CompareTo(Theory other);
    }
}
