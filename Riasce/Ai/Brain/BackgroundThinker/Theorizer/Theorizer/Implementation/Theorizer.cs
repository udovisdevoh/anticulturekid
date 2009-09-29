using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class Theorizer : AbstractTheorizer
    {
        #region Fields and parts
        private ConnectionTheorizer connectionTheorizer;

        private MetaConnectionTheorizer metaConnectionTheorizer;

        private LinguisticTheorizer linguisticTheorizer;

        private PhoneticTheorizer phoneticTheorizer;

        private Theory currentTheory = null;

        private Random random = new Random();
        #endregion

        #region Constructor
        public Theorizer(RejectedTheories rejectedTheories)
        {
            connectionTheorizer = new ConnectionTheorizer(rejectedTheories);
            metaConnectionTheorizer = new MetaConnectionTheorizer(rejectedTheories);
            linguisticTheorizer = new LinguisticTheorizer();
            phoneticTheorizer = new PhoneticTheorizer();
        }
        #endregion

        #region Methods
        public override List<Theory> GetRandomConnectionTheoryListAbout(Concept conceptToThinkAbout)
        {
            return connectionTheorizer.GetRandomTheoryListAbout(conceptToThinkAbout);
        }

        public override List<Theory> GetRandomLinguisticTheoryListAbout(Memory memory, NameMapper nameMapper, Concept conceptToThinkAbout)
        {
            return linguisticTheorizer.GetRandomTheoryListAbout(memory, nameMapper, conceptToThinkAbout);
        }

        public List<Theory> GetRandomPhoneticTheoryListAbout(Memory memory, NameMapper nameMapper, Concept conceptToThinkAbout)
        {
            return phoneticTheorizer.GetRandomTheoryListAbout(memory, nameMapper, conceptToThinkAbout);
        }

        public override Theory GetRandomMetaConnectionTheoryAbout(Concept operatorToThinkAbout)
        {
            return metaConnectionTheorizer.GetRandomMetaConnectionTheoryAbout(operatorToThinkAbout);
        }

        public override Theory GetBestTheoryAboutConcept(Concept conceptOrOperator)
        {
            Theory connectionTheory = null;
            if (random.Next(0, 3) < 2)
            {
                connectionTheory = connectionTheorizer.GetBestTheoryAboutConcept(conceptOrOperator, false);
                if (connectionTheory == null)
                    connectionTheory = connectionTheorizer.GetBestTheoryAboutConcept(conceptOrOperator, true);
            }
            else
            {
                connectionTheory = connectionTheorizer.GetBestTheoryAboutConcept(conceptOrOperator, true);
            }

            if (connectionTheory == null)
            {
                FeelingMonitor.Add(FeelingMonitor.EMPTINESS);
                throw new TheoryException("Couldn't make any theory about that");
            }
            else
            {
                return connectionTheory;
            }
        }

        public override Theory GetBestTheoryFromConceptEnumeration(IEnumerable<Concept> conceptCollection)
        {
            Theory connectionTheory = null;

            if (random.Next(0, 3) < 2)
            {
                connectionTheory = connectionTheorizer.GetBestTheoryFromConceptCollection(conceptCollection, false);
                if (connectionTheory == null)
                    connectionTheory = connectionTheorizer.GetBestTheoryFromConceptCollection(conceptCollection, true);
            }
            else
            {
                connectionTheory = connectionTheorizer.GetBestTheoryFromConceptCollection(conceptCollection, true);
            }

            if (connectionTheory == null)
            {
                FeelingMonitor.Add(FeelingMonitor.EMPTINESS);
                throw new TheoryException("Couldn't make any theory for now");
            }
            else
            {
                return connectionTheory;
            }
        }

        public Theory GetBestTheoryAboutOperator(Concept subject)
        {
            Theory metaConnectionTheory = metaConnectionTheorizer.GetBestTheoryAboutOperator(subject);

            if (metaConnectionTheory == null)
            {
                FeelingMonitor.Add(FeelingMonitor.EMPTINESS);
                throw new TheoryException("Couldn't make any theory about that");
            }
            else
            {
                return metaConnectionTheory;
            }
        }

        public Theory GetBestTheoryAboutOperatorsInMemory()
        {
            Theory metaConnectionTheory = metaConnectionTheorizer.GetBestTheoryAboutOperatorsInMemory();

            if (metaConnectionTheory == null)
            {
                FeelingMonitor.Add(FeelingMonitor.EMPTINESS);
                throw new TheoryException("Couldn't make any theory for now");
            }
            else
            {
                return metaConnectionTheory;
            }
        }
        #endregion

        #region Properties
        public Theory CurrentTheory
        {
            get { return currentTheory; }
            set { currentTheory = value; }
        }
        #endregion
    }
}
