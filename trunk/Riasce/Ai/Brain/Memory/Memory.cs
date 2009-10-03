using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents the brain's semantic memory
    /// </summary>
    public class Memory : ICollection<Concept>
    {
        #region Fields
        /// <summary>
        /// key: conceptId
        /// value: concept
        /// </summary>
        private Dictionary<int, Concept> conceptList = new Dictionary<int, Concept>();

        /// <summary>
        /// key: concept
        /// value: conceptId
        /// </summary>
        private Dictionary<Concept, int> idList = new Dictionary<Concept, int>();

        /// <summary>
        /// Episodic memory
        /// </summary>
        private EpisodicMemory episodicMemory = new EpisodicMemory();

        /// <summary>
        /// Total verb list
        /// </summary>
        private static HashSet<Concept> totalVerbList = new HashSet<Concept>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns existing concept or create it
        /// </summary>
        /// <param name="conceptId">concept's id</param>
        /// <returns>concept</returns>
        public Concept GetOrCreateConcept(int conceptId)
        {
            Concept concept;
            if (!conceptList.TryGetValue(conceptId, out concept))
            {
                concept = new Concept(conceptId.ToString());
                conceptList.Add(conceptId, concept);
                idList.Add(concept, conceptId);
            }
            return concept;
        }

        /// <summary>
        /// Get an existing concept's ID from memory
        /// </summary>
        /// <param name="concept">concept</param>
        /// <returns>concept's id</returns>
        public int GetIdFromConcept(Concept concept)
        {
            int conceptId;
            if (idList.TryGetValue(concept, out conceptId))
                return conceptId;
            else
                throw new MemoryException("Couldn't find concept in memory");            
        }

        /// <summary>
        /// From specified ID, set concept as provided concept
        /// (use it after cloning a concept only)
        /// </summary>
        /// <param name="id">concept's id</param>
        /// <param name="concept">provided concept</param>
        public void SetConcept(int targetConcepId, Concept concept)
        {
            if (conceptList.ContainsKey(targetConcepId) || idList.ContainsKey(concept))
                throw new MemoryException("Concept already assigned in memory for id " + targetConcepId);

            conceptList.Add(targetConcepId, concept);
            idList.Add(concept, targetConcepId);
        }

        /// <summary>
        /// Delete a concept from memory. ONLY USED BY THE DESTROYER
        /// </summary>
        /// <param name="concept">concept do delete</param>
        /// <returns>true if succesful, else: false</returns>
        public bool Remove(Concept concept)
        {
            int conceptId;
            if (idList.TryGetValue(concept, out conceptId))
            {
                conceptList.Remove(conceptId);
                idList.Remove(concept);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remember text comment in episodic memory
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="textComment">text comment to remember</param>
        public void EpisodicRemember(string authorName, string textComment)
        {
            episodicMemory.RememberString(authorName, textComment);
        }

        /// <summary>
        /// Remember operation in episodic memory
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="conceptList">subject, verb, complement</param>
        /// <param name="isNegative">whether the operation is negative</param>
        /// <param name="isInterrogative">whether the operation is interrogative</param>
        public void EpisodicRemember(string authorName, List<Concept> conceptList, bool isNegative, bool isInterrogative)
        {
            episodicMemory.RememberOperation(authorName, conceptList, isNegative, isInterrogative);
        }

        /// <summary>
        /// Remember quaternary operation in episodic memory
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="stringList">why, how, etc...</param>
        /// <param name="conceptList">subject, verb, complement</param>
        /// <param name="isNegative">whether the operation is negative</param>
        public void EpisodicRemember(string authorName, List<string> stringList, List<Concept> conceptList, bool isNegative)
        {
            episodicMemory.RememberListStringAndConcept(authorName, stringList, conceptList, isNegative);
        }

        /// <summary>
        /// Remember metaOperation in episodic memory
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="conceptList">operator1, operator2</param>
        /// <param name="metaOperator">metaOperator</param>
        /// <param name="isNegative">whether the metaOperation is negative</param>
        /// <param name="isInterrogative">whether the metaOperation is interrogative</param>
        public void EpisodicRemember(string authorName, List<Concept> conceptList, string metaOperator, bool isNegative, bool isInterrogative)
        {
            episodicMemory.RememberMetaOperation(authorName, conceptList, metaOperator, isNegative, isInterrogative);
        }

        /// <summary>
        /// Remember statement in episodic memory
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="statement">statement to remember</param>
        public void EpisodicRemember(string authorName, Statement statement)
        {
            episodicMemory.RememberStatement(authorName, statement);
        }

        /// <summary>
        /// Count how many concept in memory
        /// </summary>
        public int Count
        {
            get { return conceptList.Count; }
        }

        public void CopyTo(Concept[] array, int arrayIndex)
        {
            conceptList.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Concept> GetEnumerator()
        {
            return conceptList.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Properties
        public static HashSet<Concept> TotalVerbList
        {
            get { return totalVerbList; }
            set { totalVerbList = value; }
        }

        public List<Concept> ReversedConceptList
        {
            get
            {
                List<Concept> conceptList = new List<Concept>(this);
                conceptList.Reverse();
                return conceptList;
            }
        }
        #endregion

        #region ICollection<Concept> Members
        public void Add(Concept item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Whether the memory contains concept
        /// </summary>
        /// <param name="concept">concept</param>
        /// <returns>Whether the memory contains concept</returns>
        public bool Contains(Concept concept)
        {
            return idList.ContainsKey(concept);
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }
        #endregion
    }
}
