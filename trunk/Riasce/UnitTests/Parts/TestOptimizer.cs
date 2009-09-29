using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestOptimizer
    {
        public static void Test()
        {
            GeneralTest();
        }

        private static void GeneralTest()
        {          
            #region We create the verbs that we will need
            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept contradict = new Concept("contradict");
            #endregion

            #region We plug the verbs using metaconnections
            /* 
             * isa inverse_of someare
             * isa muct isa
             * isa cant someare
             * isa cant madeof
             * isa cant partof
             * madeof inverse_of partof
             * madeof muct madeof
             * madeof muct isa
             * madeof liffid isa
             * madeof unlikely isa
             * madeof cant partof
             * contradict permutable_side contradict
             * contradict muct isa
             * contradict cant isa
            */
            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(isa, "muct", isa);
            MetaConnectionManager.AddMetaConnection(isa, "cant", someare);
            MetaConnectionManager.AddMetaConnection(isa, "cant", madeof);
            MetaConnectionManager.AddMetaConnection(isa, "cant", partof);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "unlikely", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "cant", partof);
            MetaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);
            MetaConnectionManager.AddMetaConnection(contradict, "muct", isa);
            MetaConnectionManager.AddMetaConnection(contradict, "cant", isa);
            #endregion

            #region We create some concepts
            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");
            Concept forest = new Concept("forest");
            Concept branch = new Concept("branch");
            Concept leaf = new Concept("leaf");
            Concept cat = new Concept("cat");
            Concept animal = new Concept("animal");
            Concept plant = new Concept("plant");
            Concept lifeform = new Concept("lifeform");
            Concept water = new Concept("water");
            Concept liquid = new Concept("liquid");
            #endregion

            Proof proof;
            proof = new Proof();
            pine.AddFlatConnection(isa, tree);
            pine.GetFlatConnectionBranch(isa).SetProofTo(tree, proof);

            pine.AddFlatConnection(isa, plant);
            proof = new Proof();
            proof.AddArgument(pine, isa, tree);
            pine.GetFlatConnectionBranch(isa).SetProofTo(plant, proof);

            Optimizer.Repair(pine);

            if (!pine.IsOptimizedConnectedTo(isa, tree))
                throw new Exception("Connection should exist");

            if (pine.IsOptimizedConnectedTo(isa, plant))
                throw new Exception("Connection shouldn't exist");
        }
    }
}
