using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class PhoneticTheorizer : AbstractPhoneticTheorizer
    {
        #region Fields
        private Matrix phoneticMatrix;

        private PhoneticMatrixManager phoneticMatrixManager;
        #endregion

        #region Constructor
        public PhoneticTheorizer()
        {
            this.phoneticMatrix = new Matrix();
            this.phoneticMatrixManager = new PhoneticMatrixManager();
        }
        #endregion

        #region Public Methods
        public override List<Theory> GetRandomTheoryListAbout(Memory memory, NameMapper nameMapper, Concept subject)
        {
            string subjectName;
            Concept otherConcept;
            float predictedProbability;
            List<Theory> theoryList = new List<Theory>();
            Theory currentTheory;

            subjectName = GetConceptName(subject,memory,nameMapper);
            phoneticMatrixManager.LearnNewWord(phoneticMatrix, subjectName);


            foreach (string otherConceptName in phoneticMatrix.NormalData[subjectName].Keys)
            {
                if (otherConceptName == subjectName || otherConceptName == null || otherConceptName.Length < 1 || !nameMapper.Contains(otherConceptName))
                    continue;

                otherConcept = GetConcept(otherConceptName, memory, nameMapper);

                phoneticMatrixManager.LearnNewWord(phoneticMatrix, otherConceptName);

                predictedProbability = phoneticMatrix.TryGetNormalValue(subjectName, otherConceptName);

                if (predictedProbability == 0.0f)
                    continue;

                foreach (Concept verb in otherConcept.OptimizedConnectionBranchList.Keys)
                {
                    Concept complement = otherConcept.GetOptimizedConnectionBranch(verb).ComplementConceptList.GetRandomItem();
                    if (complement != null)
                    {
                        if (ConnectionManager.FindObstructionToPlug(subject, verb, complement, true) == null)
                        {
                            if (subject != complement)
                            {
                                currentTheory = new Theory(predictedProbability, subject, verb, complement);
                                currentTheory.IsPhonetic = true;
                                currentTheory.AddConnectionArgument(otherConcept, verb, complement);
                                theoryList.Add(currentTheory);
                            }
                        }
                    }
                }
            }

            if (theoryList.Count == 0)
                return null;

            return theoryList;
        }
        #endregion

        #region Private Methods
        private string GetConceptName(Concept concept, Memory memory, NameMapper nameMapper)
        {
            int conceptId = memory.GetIdFromConcept(concept);
            return nameMapper.GetConceptNames(conceptId)[0];
        }

        private Concept GetConcept(string concept, Memory memory, NameMapper nameMapper)
        {
            int conceptId = nameMapper.GetOrCreateConceptId(concept);
            return memory.GetOrCreateConcept(conceptId);
        }
        #endregion
    }
}
