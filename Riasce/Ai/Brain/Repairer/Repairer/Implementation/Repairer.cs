using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public class Repairer : AbstractRepairer
    {
        #region Fields
        private SerialFlattenizer flattenizer = new SerialFlattenizer();

        private Optimizer optimizer = new Optimizer();

        private Reciprocator reciprocator = new Reciprocator();
        #endregion

        #region Methods
        public override void Repair(Concept conceptToRepair)
        {
            Repair(conceptToRepair, new HashSet<ConnectionBranch>(), new VerbMetaConnectionCache());
        }

        public override void Repair(Concept conceptToRepair1, Concept conceptToRepair2)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2 });
        }

        public override void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2, conceptToRepair3});
        }

        public override void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2, conceptToRepair3, conceptToRepair4});
        }

        public override void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2, conceptToRepair3, conceptToRepair4, conceptToRepair5});
        }

        public override void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6)
        {
            RepairRange(new List<Concept> {conceptToRepair1,conceptToRepair2,conceptToRepair3,conceptToRepair4,conceptToRepair5,conceptToRepair6});
        }

        public override void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6, Concept conceptToRepair7)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2, conceptToRepair3, conceptToRepair4, conceptToRepair5, conceptToRepair6, conceptToRepair7 });
        }

        public override void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6, Concept conceptToRepair7, Concept conceptToRepair8)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2, conceptToRepair3, conceptToRepair4, conceptToRepair5, conceptToRepair6, conceptToRepair7, conceptToRepair8 });
        }

        public override void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6, Concept conceptToRepair7, Concept conceptToRepair8, Concept conceptToRepair9)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2, conceptToRepair3, conceptToRepair4, conceptToRepair5, conceptToRepair6, conceptToRepair7, conceptToRepair8, conceptToRepair9 });
        }

        public override void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6, Concept conceptToRepair7, Concept conceptToRepair8, Concept conceptToRepair9, Concept conceptToRepair10)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2, conceptToRepair3, conceptToRepair4, conceptToRepair5, conceptToRepair6, conceptToRepair7, conceptToRepair8, conceptToRepair9, conceptToRepair10 });
        }

        public override void Repair(Concept conceptToRepair1, Concept conceptToRepair2, Concept conceptToRepair3, Concept conceptToRepair4, Concept conceptToRepair5, Concept conceptToRepair6, Concept conceptToRepair7, Concept conceptToRepair8, Concept conceptToRepair9, Concept conceptToRepair10, Concept conceptToRepair11)
        {
            RepairRange(new List<Concept> { conceptToRepair1, conceptToRepair2, conceptToRepair3, conceptToRepair4, conceptToRepair5, conceptToRepair6, conceptToRepair7, conceptToRepair8, conceptToRepair9, conceptToRepair10, conceptToRepair11 });
        }

        public override void RepairRange(IEnumerable<Concept> conceptCollection)
        {
            HashSet<ConnectionBranch> repairedBranches = new HashSet<ConnectionBranch>();
            VerbMetaConnectionCache verbConnectionCache = new VerbMetaConnectionCache();

            foreach (Concept conceptToRepair in conceptCollection)
                Repair(conceptToRepair, repairedBranches, verbConnectionCache);
        }

        public void Repair(Concept conceptToRepair, HashSet<ConnectionBranch> repairedBranches, VerbMetaConnectionCache verbConnectionCache)
        {
            flattenizer.Repair(conceptToRepair, repairedBranches, verbConnectionCache);
            optimizer.Repair(conceptToRepair);
        }

        public override void Reciprocate(Concept concept)
        {
            reciprocator.Reciprocate(concept);
        }

        public override void ReciprocateRange(IEnumerable<Concept> conceptCollection)
        {
            reciprocator.ReciprocateRange(conceptCollection);
        }

        public override void RepairConceptAndSurrounding(Concept concept)
        {
            HashSet<ConnectionBranch> repairedBranches = new HashSet<ConnectionBranch>();
            VerbMetaConnectionCache verbConnectionCache = new VerbMetaConnectionCache();

            Repair(concept, repairedBranches, verbConnectionCache);

            foreach (Concept complement in concept.ConceptFlatPluggedTo)
            {
                Repair(complement, repairedBranches, verbConnectionCache);
            }
        }
        #endregion
    }
}
