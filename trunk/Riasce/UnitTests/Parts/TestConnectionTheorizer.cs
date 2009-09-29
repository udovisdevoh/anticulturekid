using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestConnectionTheorizer
    {
        public static void Test()
        {
            TestPineMadeOfWood();
        }

        private static void TestPineMadeOfWood()
        {
            RejectedTheories rejectedTheories = new RejectedTheories();
            
            BrotherHoodManager brotherHoodManager = new BrotherHoodManager();
            ConnectionTheorizer connectionTheorizer = new ConnectionTheorizer(rejectedTheories, brotherHoodManager);
            Repairer repairer = new Repairer();
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept tree = new Concept("tree");
            Concept pine = new Concept("pine");
            Concept willow = new Concept("willow");
            Concept palmtree = new Concept("palm tree");
            Concept wood = new Concept("wood");
            Concept forest = new Concept("forest");
            Concept plant = new Concept("plant");
            Concept water = new Concept("water");

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);

            repairer.Repair(pine, tree);
            ConnectionManager.Plug(pine,isa,tree);

            repairer.Repair(pine, water);
            ConnectionManager.Plug(pine, madeof, water);

            repairer.Repair(willow, tree);
            ConnectionManager.Plug(willow, isa, tree);

            repairer.Repair(palmtree, tree);
            ConnectionManager.Plug(palmtree, isa, tree);

            repairer.Repair(palmtree, wood);
            ConnectionManager.Plug(palmtree, madeof, wood);

            repairer.Repair(willow, wood);
            ConnectionManager.Plug(willow, madeof, wood);

            repairer.Repair(pine,tree,willow,palmtree,wood);

            //Real test here

            Theory theory;
            theory = connectionTheorizer.GetBestTheoryAboutConcept(pine,true);

            if (theory.GetConcept(0) != pine)
                throw new Exception("Wrong subject");

            if (theory.GetConcept(1) != madeof)
                throw new Exception("Wrong verb");

            if (theory.GetConcept(2) != wood)
                throw new Exception("Wrong complement");

            if (theory.PredictedProbability > 0.7 || theory.PredictedProbability < 0.6)
                throw new Exception("Wrong probability");

            if (theory.CountConnectionArgument != 6)
                throw new Exception("Wrong argument count");

            if (theory.GetConnectionArgumentAt(0)[0] != pine || theory.GetConnectionArgumentAt(0)[1] != isa || theory.GetConnectionArgumentAt(0)[2] != tree)
                throw new Exception("Wrong argument");

            if (theory.GetConnectionArgumentAt(1)[0] != willow || theory.GetConnectionArgumentAt(1)[1] != isa || theory.GetConnectionArgumentAt(1)[2] != tree)
                throw new Exception("Wrong argument");

            if (theory.GetConnectionArgumentAt(2)[0] != willow || theory.GetConnectionArgumentAt(2)[1] != madeof || theory.GetConnectionArgumentAt(2)[2] != wood)
                throw new Exception("Wrong argument");

            if (theory.GetConnectionArgumentAt(3)[0] != pine || theory.GetConnectionArgumentAt(3)[1] != isa || theory.GetConnectionArgumentAt(3)[2] != tree)
                throw new Exception("Wrong argument");

            if (theory.GetConnectionArgumentAt(4)[0] != palmtree || theory.GetConnectionArgumentAt(4)[1] != isa || theory.GetConnectionArgumentAt(4)[2] != tree)
                throw new Exception("Wrong argument");

            if (theory.GetConnectionArgumentAt(5)[0] != palmtree || theory.GetConnectionArgumentAt(5)[1] != madeof || theory.GetConnectionArgumentAt(5)[2] != wood)
                throw new Exception("Wrong argument");
        }
    }
}
