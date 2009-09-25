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
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Memory.TotalVerbList = new HashSet<Concept>();
            StatMaker statMaker = new StatMaker(metaConnectionManager);
            ConnectionManager connectionManager = new ConnectionManager();
            Repairer repairer = new Repairer();

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

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            metaConnectionManager.AddMetaConnection(madeof, "muct", madeof);

            repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);
            repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);

            connectionManager.Plug(tree, isa, plant);

            repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);
            repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);

            connectionManager.Plug(tree, madeof, wood);

            repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);
            repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);

            connectionManager.Plug(pine,isa,tree);

            repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);
            repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);

            connectionManager.Plug(palmtree, isa, tree);

            repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);
            repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);

            connectionManager.Plug(willow, isa, tree);

            repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);
            repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);

            connectionManager.Plug(cactus, isa, plant);

            repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);
            repairer.Repair(pine, tree, palmtree, willow, cactus, plant, wood, isa, someare, madeof, partof);

            double expectedStat = 0.8;
            double stat = statMaker.GetStatOn(isa, plant, madeof, wood).Ratio;

            if (stat != expectedStat)
                throw new Exception("Stat doesn't match expectation");

            if (statMaker.GetStatOn(madeof, tree, isa, plant).Ratio != -1)
                throw new Exception("Stat should be -1 because of division by 0");
        }
    }
}
