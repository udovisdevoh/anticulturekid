using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class MctFromConnection : AbstractMctFromConnection
    {
        #region Fields
        private static readonly int samplingSize = 20;

        private RejectedTheories rejectedTheories;
        #endregion

        #region Constructor
        public MctFromConnection(RejectedTheories rejectedTheories)
        {
            this.rejectedTheories = rejectedTheories;
        }
        #endregion

        #region Methods
        public override Theory GetBestTheoryAboutOperator(Concept verb)
        {
            List<Theory> listTheoryAboutVerb = GetAllTheoriesAboutOperatorFromConnections(verb);

            Theory bestTheory = null;
            foreach (Theory currentTheory in listTheoryAboutVerb)
                if (bestTheory == null || (currentTheory != null && currentTheory > bestTheory))
                    bestTheory = currentTheory;
            return bestTheory;
        }

        private List<Theory> GetAllTheoriesAboutOperatorFromConnections(Concept verb)
        {
            #warning Implement GetAllTheoriesAboutOperatorFromConnections()
            throw new NotImplementedException();
        }

        public override Theory GetBestTheoryAboutOperatorsInMemory()
        {
            HashSet<Concept> conceptSample = Memory.TotalVerbList.GetRandomSample(samplingSize);

            if (conceptSample.Count < 1)
                return null;

            Theory bestTheory = null;
            Theory currentTheory;
            foreach (Concept currentConcept in conceptSample)
            {
                currentTheory = GetBestTheoryAboutOperator(currentConcept);
                if (bestTheory == null || currentTheory > bestTheory)
                    bestTheory = currentTheory;
            }
            return bestTheory;
        }
        #endregion
    }
}