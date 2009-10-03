using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class Theory : AbstractTheory
    {
        #region Fields
        private double predictedProbability;

        private List<Concept> conceptList;

        private string metaOperatorName;

        private List<List<Concept>> connectionArgumentList = new List<List<Concept>>();

        private List<MetaConnectionArgument> metaConnectionArgumentList = new List<MetaConnectionArgument>();

        private bool isSemanticLinguistic = false;

        private bool isPhonetic = false;
        #endregion

        #region Constructors
        public Theory(double predictedProbability, Concept operator1, string metaOperatorName, Concept operator2)
        {
            this.predictedProbability = predictedProbability;
            conceptList = new List<Concept>() { operator1, operator2 };
            this.metaOperatorName = metaOperatorName;
        }

        public Theory(double predictedProbability, Concept subject, Concept verb, Concept complement)
        {
            this.predictedProbability = predictedProbability;
            conceptList = new List<Concept>() { subject, verb, complement };
            this.metaOperatorName = null;
        }

        public Theory(double predictedProbability, TheoryInfo theoryInfo)
        {
            this.predictedProbability = predictedProbability;
            conceptList = new List<Concept>() { theoryInfo.Subject, theoryInfo.Verb, theoryInfo.Complement };
            foreach (List<Concept> argument in theoryInfo)
                this.AddConnectionArgument(argument[0], argument[1], argument[2]);
        }
        #endregion

        #region Methods
        public override void AddConnectionArgument(Concept subject, Concept verb, Concept complement)
        {
            connectionArgumentList.Add(new List<Concept>() { subject, verb, complement });
        }

        public override void AddMetaConnectionArgument(Concept operator1, string metaOperatorName, Concept operator2)
        {
            metaConnectionArgumentList.Add(new MetaConnectionArgument(operator1, metaOperatorName, operator2));
        }

        public override void AddMetaConnectionArgumentRange(List<MetaConnectionArgument> argumentList)
        {
            metaConnectionArgumentList.AddRange(argumentList);
        }

        public override bool Equals(Theory otherTheory)
        {
            if (this.conceptList.Count != otherTheory.conceptList.Count)
                return false;

            if (this.isPhonetic != otherTheory.isPhonetic)
                return false;

            if (this.isSemanticLinguistic != otherTheory.isSemanticLinguistic)
                return false;

            for (int i = 0; i < this.conceptList.Count; i++)
                if (this.conceptList[i] != otherTheory.conceptList[i])
                    return false;

            if (this.metaOperatorName != otherTheory.metaOperatorName)
                return false;

            return true;
        }

        public override Concept GetConcept(int conceptId)
        {
            if (conceptList.Count <= conceptId || conceptId < 0)
                return null;
            else
                return conceptList[conceptId];
        }

        public override List<string> ToStringList(NameMapper nameMapper, Memory memory)
        {
            List<string> theoryByString = new List<string>();
            string subjectName;
            string verbName;
            string complementName;
            string metaOperatorName;
            int subjectId;
            int verbId;
            int complementId;
            Concept subject;
            Concept verb;
            Concept complement;

            #region For metaConnection theory
            if (this.CountMetaConnectionArgument > 0)
            {
                MetaConnectionArgument currentMetaConnectionArgument;
                for (int i = 0; i < this.CountMetaConnectionArgument; i++)
                {
                    currentMetaConnectionArgument = this.GetMetaConnectionArgumentAt(i);
                    subject = currentMetaConnectionArgument.Operator1;
                    metaOperatorName = currentMetaConnectionArgument.MetaOperator;
                    complement = currentMetaConnectionArgument.Operator2;

                    subjectId = memory.GetIdFromConcept(subject);
                    subjectName = nameMapper.GetConceptNames(subjectId)[0];
                    complementId = memory.GetIdFromConcept(complement);
                    complementName = nameMapper.GetConceptNames(complementId)[0];

                    theoryByString.Add(subjectName + " " + metaOperatorName + " " + complementName);
                }

                subject = this.GetConcept(0);
                subjectId = memory.GetIdFromConcept(subject);
                subjectName = nameMapper.GetConceptNames(subjectId)[0];
                complement = this.GetConcept(1);
                complementId = memory.GetIdFromConcept(complement);
                complementName = nameMapper.GetConceptNames(complementId)[0];

                theoryByString.Add(subjectName + " " + this.MetaOperatorName + " " + complementName);
            }
            #endregion

            #region Connection theory
            if (this.CountConnectionArgument > 0)
            {
                List<Concept> currentConnectionArgument;
                for (int i = 0; i < this.CountConnectionArgument; i++)
                {
                    currentConnectionArgument = this.GetConnectionArgumentAt(i);
                    subject = currentConnectionArgument[0];
                    verb = currentConnectionArgument[1];
                    complement = currentConnectionArgument[2];

                    subjectId = memory.GetIdFromConcept(subject);
                    verbId = memory.GetIdFromConcept(verb);
                    complementId = memory.GetIdFromConcept(complement);

                    subjectName = nameMapper.GetConceptNames(subjectId)[0];
                    verbName = nameMapper.GetConceptNames(verbId)[0];
                    complementName = nameMapper.GetConceptNames(complementId)[0];

                    if (!theoryByString.Contains(subjectName + " " + verbName + " " + complementName))
                        theoryByString.Add(subjectName + " " + verbName + " " + complementName);
                }

                subject = this.GetConcept(0);
                verb = this.GetConcept(1);
                complement = this.GetConcept(2);

                subjectId = memory.GetIdFromConcept(subject);
                verbId = memory.GetIdFromConcept(verb);
                complementId = memory.GetIdFromConcept(complement);

                subjectName = nameMapper.GetConceptNames(subjectId)[0];
                verbName = nameMapper.GetConceptNames(verbId)[0];
                complementName = nameMapper.GetConceptNames(complementId)[0];

                theoryByString.Add(subjectName + " " + verbName + " " + complementName);
            }
            #endregion

            return theoryByString; 
        }

        public override Statement ToStatement(NameMapper nameMapper, Memory memory)
        {
            if (conceptList.Count == 3)
            {
                int concept1Id = memory.GetIdFromConcept(conceptList[0]);
                int concept2Id = memory.GetIdFromConcept(conceptList[1]);
                int concept3Id = memory.GetIdFromConcept(conceptList[2]);
                string concept1Name = nameMapper.InvertYouAndMePov(nameMapper.GetConceptNames(concept1Id)[0]);
                string concept2Name = nameMapper.InvertYouAndMePov(nameMapper.GetConceptNames(concept2Id)[0]);
                string concept3Name = nameMapper.InvertYouAndMePov(nameMapper.GetConceptNames(concept3Id)[0]);



                return new Statement("theory", Statement.MODE_OPERATION, concept1Name, concept2Name, concept3Name, false, false, false,false, concept1Name + " " + concept2Name + " " + concept3Name);
            }
            else if (conceptList.Count == 2 && this.metaOperatorName != null)
            {
                int concept1Id = memory.GetIdFromConcept(conceptList[0]);
                int concept2Id = memory.GetIdFromConcept(conceptList[1]);

                string concept1Name = nameMapper.GetConceptNames(concept1Id)[0];
                string concept2Name = nameMapper.GetConceptNames(concept2Id)[0];
                return new Statement("theory", Statement.MODE_META_OPERATION, metaOperatorName, concept1Name, concept2Name, false, "");
            }

            throw new TheoryException("Couldn't convert theory to statement");
        }

        public override int CompareTo(Theory other)
        {
            return (int)(other.predictedProbability * 10000) - (int)(this.predictedProbability * 10000);
        }

        public HashSet<Concept> GetFeaturedConceptList()
        {
            HashSet<Concept> featuredConceptList = new HashSet<Concept>();

            featuredConceptList.UnionWith(conceptList);

            foreach (List<Concept> argument in connectionArgumentList)
                featuredConceptList.UnionWith(argument);

            return featuredConceptList;
        }
        #endregion

        #region Operator Overloading
        public static bool operator >(Theory theory1, Theory theory2)
        {
            return theory1.predictedProbability > theory2.predictedProbability;
        }

        public static bool operator <(Theory theory1, Theory theory2)
        {
            return theory1.predictedProbability < theory2.predictedProbability;
        }

        public static bool operator >=(Theory theory1, Theory theory2)
        {
            return theory1.predictedProbability >= theory2.predictedProbability;
        }

        public static bool operator <=(Theory theory1, Theory theory2)
        {
            return theory1.predictedProbability <= theory2.predictedProbability;
        }

        public static bool operator >(Theory theory1, double otherPredictedProbability)
        {
            return theory1.predictedProbability > otherPredictedProbability;
        }

        public static bool operator <(Theory theory1, double otherPredictedProbability)
        {
            return theory1.predictedProbability < otherPredictedProbability;
        }

        public static bool operator >=(Theory theory1, double otherPredictedProbability)
        {
            return theory1.predictedProbability >= otherPredictedProbability;
        }

        public static bool operator <=(Theory theory1, double otherPredictedProbability)
        {
            return theory1.predictedProbability <= otherPredictedProbability;
        }

        public static bool operator >(double otherPredictedProbability, Theory theory2)
        {
            return otherPredictedProbability > theory2.predictedProbability;
        }

        public static bool operator <(double otherPredictedProbability, Theory theory2)
        {
            return otherPredictedProbability < theory2.predictedProbability;
        }

        public static bool operator >=(double otherPredictedProbability, Theory theory2)
        {
            return otherPredictedProbability >= theory2.predictedProbability;
        }

        public static bool operator <=(double otherPredictedProbability, Theory theory2)
        {
            return otherPredictedProbability <= theory2.predictedProbability;
        }
        #endregion

        #region Properties
        public override string MetaOperatorName
        {
            get { return metaOperatorName; }
        }

        public override int CountConnectionArgument
        {
            get { return connectionArgumentList.Count; }
        }

        public override int CountMetaConnectionArgument
        {
            get { return metaConnectionArgumentList.Count; }
        }

        public override List<Concept> GetConnectionArgumentAt(int argumentId)
        {
            return connectionArgumentList[argumentId];
        }

        public override MetaConnectionArgument GetMetaConnectionArgumentAt(int id)
        {
            if (metaConnectionArgumentList.Count <= id || id < 0)
                return null;

            return metaConnectionArgumentList[id];
        }

        public override double PredictedProbability
        {
            get { return predictedProbability; }
        }

        public bool IsSemanticLinguistic
        {
            get { return isSemanticLinguistic;}
            set { isSemanticLinguistic = value; }
        }

        public bool IsPhonetic
        {
            get { return isPhonetic; }
            set { isPhonetic = value; }
        }
        #endregion
    }
}
