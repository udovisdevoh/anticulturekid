using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Exception thrown when flattenizing branches and making infinite oop
    /// </summary>
    class CyclicFlatBranchDependencyException : Exception
    {
        #region Fields
        private List<List<Concept>> proofStackTrace = new List<List<Concept>>();

        private List<List<int>> proofStackTraceById;
        #endregion

        #region Constructor
        public CyclicFlatBranchDependencyException(string info)
            : base(info)
        {
        }

        public CyclicFlatBranchDependencyException(string info, Concept subject, Concept verb)
            : base(info)
        {
            AddToProofStackTrace(subject, verb);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add connection stub to proof stack
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        public void AddToProofStackTrace(Concept subject, Concept verb)
        {
            proofStackTrace.Add(new List<Concept>() { subject, verb });
        }

        /// <summary>
        /// Build proof stack trace by id
        /// </summary>
        /// <param name="memory">memory to look into</param>
        /// <returns>proof stack trace by id</returns>
        public List<List<int>> GetProofStackTraceById(Memory memory)
        {
            List<List<int>> proofStackById = new List<List<int>>();

            foreach (List<Concept> stub in proofStackTrace)
            {
                proofStackById.Add(new List<int>() { memory.GetIdFromConcept(stub[0]), memory.GetIdFromConcept(stub[1]) });
            }

            return proofStackById;
        }

        /// <summary>
        /// Proof stack trace by string
        /// </summary>
        /// <param name="nameMapper">name mapper to look into</param>
        /// <returns></returns>
        public string GetProofStackTraceByString(NameMapper nameMapper)
        {
            string stackTrace = string.Empty;
            foreach (List<int> stub in proofStackTraceById)
            {
                stackTrace += "\n" + nameMapper.GetConceptNames(stub[0])[0] + " ";
                stackTrace += nameMapper.GetConceptNames(stub[1])[0];
            }

            return stackTrace;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Proof stack trace
        /// </summary>
        public List<List<Concept>> ProofStackTrace
        {
            get { return proofStackTrace; }
        }

        /// <summary>
        /// Proof stack trace by Id
        /// </summary>
        public List<List<int>> ProofStackTraceById
        {
            get { return proofStackTraceById; }
            set { proofStackTraceById = value; }
        }
        #endregion
    }
}
