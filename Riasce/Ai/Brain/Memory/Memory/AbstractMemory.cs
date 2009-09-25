using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public abstract class AbstractMemory : ICollection<Concept>
    {
        /// <summary>
        /// Returns existing concept or create it
        /// </summary>
        /// <param name="conceptId">concept's id</param>
        /// <returns>concept</returns>
        public abstract Concept GetOrCreateConcept(int conceptId);

        /// <summary>
        /// Get an existing concept's ID from memory
        /// </summary>
        /// <param name="concept">concept</param>
        /// <returns>concept's id</returns>
        public abstract int GetIdFromConcept(Concept concept);

        /// <summary>
        /// From specified ID, set concept as provided concept
        /// (use it after cloning a concept only)
        /// </summary>
        /// <param name="id">concept's id</param>
        /// <param name="concept">provided concept</param>
        public abstract void SetConcept(int targetConcepId, Concept concept);

        /// <summary>
        /// Delete a concept from memory. ONLY USED BY THE DESTROYER
        /// </summary>
        /// <param name="concept">concept do delete</param>
        /// <returns>true if succesful, else: false</returns>
        public abstract bool Remove(Concept concept);

        /// <summary>
        /// Remember text comment in episodic memory
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="textComment">text comment to remember</param>
        public abstract void EpisodicRemember(string authorName, string textComment);

        /// <summary>
        /// Remember operation in episodic memory
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="conceptList">subject, verb, complement</param>
        /// <param name="isNegative">whether the operation is negative</param>
        /// <param name="isInterrogative">whether the operation is interrogative</param>
        public abstract void EpisodicRemember(string authorName, List<Concept> conceptList, bool isNegative, bool isInterrogative);

        /// <summary>
        /// Remember quaternary operation in episodic memory
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="stringList">why, how, etc...</param>
        /// <param name="conceptList">subject, verb, complement</param>
        /// <param name="isNegative">whether the operation is negative</param>
        public abstract void EpisodicRemember(string authorName, List<string> stringList, List<Concept> conceptList, bool isNegative);

        /// <summary>
        /// Remember metaOperation in episodic memory
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="conceptList">operator1, operator2</param>
        /// <param name="metaOperator">metaOperator</param>
        /// <param name="isNegative">whether the metaOperation is negative</param>
        /// <param name="isInterrogative">whether the metaOperation is interrogative</param>
        public abstract void EpisodicRemember(string authorName, List<Concept> conceptList, string metaOperator, bool isNegative, bool isInterrogative);

        /// <summary>
        /// Remember statement in episodic memory
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="statement">statement to remember</param>
        public abstract void EpisodicRemember(string authorName, Statement statement);

        /// <summary>
        /// Return how many concepts are present in memory
        /// </summary>
        public abstract int Count
        {
            get;
        }

        public abstract IEnumerator<Concept> GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Concept item)
        {
            throw new NotImplementedException("Add is not implemented in memory");
        }

        public void Clear()
        {
            throw new NotImplementedException("Clear is not implemented in memory");
        }

        public bool Contains(Concept item)
        {
            throw new NotImplementedException("Contains is not implemented in memory");
        }

        public abstract void CopyTo(Concept[] array, int arrayIndex);

        public bool IsReadOnly
        {
            get { return false; }
        }
    }
}
