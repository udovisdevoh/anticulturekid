using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class LinguisticTheorizer : AbstractLinguisticTheorizer
    {
        #region Fields
        private Matrix semanticProximityMatrix;
        #endregion

        #region Constants
        private static readonly string matrixFileName = "EnglishSemanticMatrix5000short.xml";
        #endregion

        #region Constructor
        public LinguisticTheorizer()
        {
            XmlMatrixSaverLoader xmlMatrixSaverLoader = new XmlMatrixSaverLoader();
            this.semanticProximityMatrix = xmlMatrixSaverLoader.Load(matrixFileName);
        }
        #endregion

        #region Public methods
        public override List<Theory> GetRandomTheoryListAbout(Memory memory, NameMapper nameMapper, Concept subject)
        {
            if (subject.IsFlatDirty)
                throw new TheoryException("Repair concept first");

            string subjectName = GetConceptName(subject, memory, nameMapper);

            if (!semanticProximityMatrix.ContainsKey(subjectName))
            {
                if (subjectName.Contains('_') && subjectName.Length > 2)
                    subjectName = subjectName.Substring(subjectName.LastIndexOf('_') + 1);
                else if (subjectName.Substring((subjectName.Length - 1),1) == "s" && subjectName.Length > 2)
                    subjectName = subjectName.Substring(0, subjectName.Length - 1);
                else if (subjectName.Length > 2)
                    subjectName = subjectName + "s";
                else
                    return null;

                if (!semanticProximityMatrix.ContainsKey(subjectName))
                    return null;
            }

            Dictionary<string, float> semanticVector = GetSementicVector(subjectName);

            List<Theory> theoryList = new List<Theory>();

            string otherConceptName;
            double probability;
            Concept otherConcept;
            Theory currentTheory;
            foreach (KeyValuePair<string, float> otherConceptNameAndProbability in semanticVector)
            {
                otherConceptName = otherConceptNameAndProbability.Key;
                probability = otherConceptNameAndProbability.Value;


                if (otherConceptName == subjectName)
                    continue;


                if (otherConceptName.Length > 2 && !nameMapper.Contains(otherConceptName))
                {
                    if (otherConceptName.Contains('_'))
                        otherConceptName = otherConceptName.Substring(subjectName.LastIndexOf('_') + 1);
                    else if (otherConceptName.Substring(otherConceptName.Length - 1, 1) == "s")
                        otherConceptName = otherConceptName.Substring(0, otherConceptName.Length - 1);
                    else
                        otherConceptName += "s";
                }

                if (nameMapper.Contains(otherConceptName))
                {
                    otherConcept = GetConcept(otherConceptName, memory,nameMapper);

                    foreach (Concept verb in otherConcept.OptimizedConnectionBranchList.Keys)
                    {
                        Concept complement = otherConcept.GetOptimizedConnectionBranch(verb).ComplementConceptList.GetRandomItem();
                        if (complement != null)
                        {
                            if (otherConcept != subject)
                            {
                                if (ConnectionManager.FindObstructionToPlug(subject, verb, complement, true) == null)
                                {
                                    if (subject != complement)
                                    {
                                        currentTheory = new Theory(probability, subject, verb, complement);
                                        currentTheory.IsSemanticLinguistic = true;
                                        currentTheory.AddConnectionArgument(otherConcept, verb, complement);
                                        theoryList.Add(currentTheory);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (theoryList.Count == 0)
                return null;
            else
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

        private Dictionary<string, float> GetSementicVector(string conceptName)
        {
            return semanticProximityMatrix.NormalData[conceptName];
        }
        #endregion
    }
}
