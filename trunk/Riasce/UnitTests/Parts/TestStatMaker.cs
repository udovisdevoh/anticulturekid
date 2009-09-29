using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestStatMaker
    {
        public static void Test()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            StatMaker statMaker = new StatMaker();         

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept palmtree = new Concept("palm tree");
            Concept willow = new Concept("willow");
            Concept cactus = new Concept("cactus");
            Concept plant = new Concept("plant");
            Concept wood = new Concept("wood");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);

            Repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);
            Repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);

            ConnectionManager.Plug(tree, isa, plant);

            Repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);
            Repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);

            ConnectionManager.Plug(tree, madeof, wood);

            Repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);
            Repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);

            ConnectionManager.Plug(pine,isa,tree);

            Repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);
            Repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);

            ConnectionManager.Plug(palmtree, isa, tree);

            Repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);
            Repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);

            ConnectionManager.Plug(willow, isa, tree);

            Repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);
            Repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);

            ConnectionManager.Plug(cactus, isa, plant);

            Repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);
            Repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);

            double expectedStat = 0.8;
            double stat = statMaker.GetStatOn(isa, plant, madeof, wood).Ratio;

            if (stat != expectedStat)
                throw new Exception("Stat doesn't match expectation");

            if (statMaker.GetStatOn(madeof, tree, isa, plant).Ratio != -1)
                throw new Exception("Stat should be -1 because of division by 0");
        }
    }
}
