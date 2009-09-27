using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Class used by Ai to teach stuff to human abour concepts
    /// </summary>
    class Teacher
    {
        #region Fields
        /// <summary>
        /// Random number generator
        /// </summary>
        private Random random = new Random();
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a random connection and its proof about subject concept
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>random connection and proof about subject concept</returns>
        public KeyValuePair<List<Concept>, Proof> TeachAbout(Concept subject)
        {
            Concept verb;
            Concept complement;
            List<Concept> connection;
            Proof proof;
            List<Concept> arguableVerbList = GetArguableVerbList(subject);

            if (arguableVerbList.Count < 1)
            {
                FeelingMonitor.Add(FeelingMonitor.EMPTINESS);
                throw new TeachingException("Couldn't find anything about that");
            }

            verb = arguableVerbList[random.Next(0, arguableVerbList.Count)];

            List<Concept> arguableComplementList = GetArguableComplementList(subject, verb);

            if (arguableComplementList.Count < 1)
            {
                FeelingMonitor.Add(FeelingMonitor.EMPTINESS);
                throw new TeachingException("Couldn't find anything about that");
            }

            complement = arguableComplementList[random.Next(0, arguableComplementList.Count)];

            connection = new List<Concept>() { subject, verb, complement };
            proof = subject.GetFlatConnectionBranch(verb).GetProofTo(complement);

            return new KeyValuePair<List<Concept>, Proof>(connection, proof);
        }

        /// <summary>
        /// Returns a random connection and its proof about random concept from conceptCollection
        /// </summary>
        /// <param name="conceptCollection">conceptCollection</param>
        /// <returns>random connection and proof about random concept from conceptCollection</returns>
        public KeyValuePair<List<Concept>, Proof> TeachAboutRandomConcept(IEnumerable<Concept> conceptCollection)
        {
            int randomIndex;

            List<Concept> conceptList = new List<Concept>(conceptCollection);
            Concept concept;

            while (conceptList.Count > 0)
            {
                randomIndex = random.Next(0, conceptList.Count);
                concept = conceptList[randomIndex];
                conceptList.RemoveAt(randomIndex);
                if (IsConceptArguable(concept))
                    return TeachAbout(concept);
            }
            FeelingMonitor.Add(FeelingMonitor.EMPTINESS);
            throw new TeachingException("Couldn't find anything to teach about");
        }
        #endregion 

        #region Private Methods
        /// <summary>
        /// Return true if concept has proof with arguments
        /// </summary>
        /// <param name="concept">concept</param>
        /// <returns>true if concept has proof with arguments, else: false</returns>
        private bool IsConceptArguable(Concept concept)
        {
            Concept verb;
            ConnectionBranch flatBranch;
            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in concept.FlatConnectionBranchList)
            {
                verb = verbAndBranch.Key;
                flatBranch = verbAndBranch.Value;

                foreach (Concept complement in flatBranch.ComplementConceptList)
                    if (flatBranch.GetProofTo(complement) != null && flatBranch.GetProofTo(complement).Count > 0)
                        return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the list of verbs that contain connections from subject to at least one
        /// complement that has at least one argument in proof
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>list of verbs that contain connections from subject to at least one
        /// complement that has at least one argument in proof</returns>
        private List<Concept> GetArguableVerbList(Concept subject)
        {
            List<Concept> arguableVerbList = new List<Concept>();

            Concept verb;
            ConnectionBranch flatBranch;
            foreach (KeyValuePair<Concept, ConnectionBranch> verbAndBranch in subject.FlatConnectionBranchList)
            {
                verb = verbAndBranch.Key;
                flatBranch = verbAndBranch.Value;

                foreach (Concept complement in flatBranch.ComplementConceptList)
                    if (flatBranch.GetProofTo(complement) != null && flatBranch.GetProofTo(complement).Count > 0)
                        arguableVerbList.Add(verb);
            }
            return arguableVerbList;
        }

        /// <summary>
        /// Returns a list of complement concept for which there is a proof with at least one argument
        /// from subject to complement through verb
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <param name="verb">verb concept</param>
        /// <returns>list of complement concept for which there is a proof with at least one argument
        /// from subject to complement through verb</returns>
        private List<Concept> GetArguableComplementList(Concept subject, Concept verb)
        {
            List<Concept> arguableComplementList = new List<Concept>();

            ConnectionBranch flatBranch = subject.GetFlatConnectionBranch(verb);
            foreach (Concept complement in flatBranch.ComplementConceptList)
                if (flatBranch.GetProofTo(complement) != null && flatBranch.GetProofTo(complement).Count > 0)
                    arguableComplementList.Add(complement);

            return arguableComplementList;
        }
        #endregion
    }
}
