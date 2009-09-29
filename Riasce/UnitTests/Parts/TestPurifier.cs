using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestPurifier
    {
        public static void Test()
        {
            TestPurifyWithPreContradict();
            TestPurifyWithPostContradict();
            TestPurifyRangeWithPreContradict();
            TestPurifyRangeWithPostContradict();
        }

        private static void TestPurifyWithPreContradict()
        {
            Memory memory = new Memory();
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept cat = new Concept("cat");
            Concept animal = new Concept("animal");
            Concept lifeform = new Concept("lifeform");
            Concept water = new Concept("water");

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept contradict = new Concept("contradict");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(isa, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);

            Repairer.Repair(cat, animal, lifeform, water);
            Repairer.Reciprocate(cat);
            Repairer.Reciprocate(animal);
            Repairer.Reciprocate(lifeform);
            Repairer.Reciprocate(water);
            Repairer.Repair(cat, animal, lifeform, water);

            ConnectionManager.Plug(cat, contradict, lifeform);

            Repairer.Repair(cat, animal);

            ConnectionManager.Plug(cat, isa, animal);

            Repairer.Repair(animal, lifeform);

            ConnectionManager.Plug(animal, isa, lifeform);

            Repairer.Repair(lifeform, water);

            ConnectionManager.Plug(lifeform, madeof, water);

            Repairer.Repair(cat, animal, lifeform, water);

            //Pre-conditions

            if (!ConnectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(cat, contradict, lifeform))
                throw new Exception("Should be connected because it's explicit");

            //Real test here

            MetaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            Repairer.Repair(cat, animal, lifeform, water);
            Repairer.Reciprocate(cat);
            Repairer.Reciprocate(animal);
            Repairer.Reciprocate(lifeform);
            Repairer.Reciprocate(water);
            Repairer.Repair(cat, animal, lifeform, water);

            Purifier.PurifyOptimized(cat);

            Repairer.Repair(cat, animal, lifeform, water);
            Repairer.Reciprocate(cat);
            Repairer.Reciprocate(animal);
            Repairer.Reciprocate(lifeform);
            Repairer.Reciprocate(water);
            Repairer.Repair(cat, animal, lifeform, water);

            if (!ConnectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected");

            if (ConnectionManager.TestConnection(cat, contradict, animal))
                throw new Exception("Shouldn't be connected");
        }

        private static void TestPurifyWithPostContradict()
        {
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept cat = new Concept("cat");
            Concept animal = new Concept("animal");
            Concept lifeform = new Concept("lifeform");
            Concept water = new Concept("water");

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept contradict = new Concept("contradict");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(isa, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);

            Repairer.Repair(cat, animal, lifeform, water);
            Repairer.Reciprocate(cat);
            Repairer.Reciprocate(animal);
            Repairer.Reciprocate(lifeform);
            Repairer.Reciprocate(water);
            Repairer.Repair(cat, animal, lifeform, water);

            ConnectionManager.Plug(cat, isa, animal);

            Repairer.Repair(animal, lifeform);

            ConnectionManager.Plug(animal, isa, lifeform);

            Repairer.Repair(lifeform, water);

            ConnectionManager.Plug(lifeform, madeof, water);

            Repairer.Repair(cat,lifeform);

            ConnectionManager.Plug(cat, contradict, lifeform);

            Repairer.Repair(cat, animal, lifeform, water);

            //Pre-conditions

            if (!ConnectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(cat, contradict, lifeform))
                throw new Exception("Should be connected because it's explicit");

            //Real test here

            MetaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            Repairer.Repair(cat, animal, lifeform, water);
            Repairer.Reciprocate(cat);
            Repairer.Reciprocate(animal);
            Repairer.Reciprocate(lifeform);
            Repairer.Reciprocate(water);
            Repairer.Repair(cat, animal, lifeform, water);

            Purifier.PurifyOptimized(cat);

            Repairer.Repair(cat, animal, lifeform, water);
            Repairer.Reciprocate(cat);
            Repairer.Reciprocate(animal);
            Repairer.Reciprocate(lifeform);
            Repairer.Reciprocate(water);
            Repairer.Repair(cat, animal, lifeform, water);

            if (!ConnectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected");

            if (ConnectionManager.TestConnection(cat, contradict, animal))
                throw new Exception("Shouldn't be connected");
        }

        private static void TestPurifyRangeWithPreContradict()
        {
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept cat = new Concept("cat");
            Concept animal = new Concept("animal");
            Concept lifeform = new Concept("lifeform");
            Concept water = new Concept("water");

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept contradict = new Concept("contradict");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(isa, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);

            Repairer.Repair(cat, animal, lifeform, water);
            Repairer.Reciprocate(cat);
            Repairer.Reciprocate(animal);
            Repairer.Reciprocate(lifeform);
            Repairer.Reciprocate(water);
            Repairer.Repair(cat, animal, lifeform, water);

            ConnectionManager.Plug(cat, contradict, lifeform);

            Repairer.Repair(cat, animal);

            ConnectionManager.Plug(cat, isa, animal);

            Repairer.Repair(animal, lifeform);

            ConnectionManager.Plug(animal, isa, lifeform);

            Repairer.Repair(lifeform, water);

            ConnectionManager.Plug(lifeform, madeof, water);

            Repairer.Repair(cat, animal, lifeform, water);

            //Pre-conditions

            if (!ConnectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(cat, contradict, lifeform))
                throw new Exception("Should be connected because it's explicit");

            //Real test here

            MetaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            Repairer.Repair(cat, animal, lifeform, water);
            Repairer.Reciprocate(cat);
            Repairer.Reciprocate(animal);
            Repairer.Reciprocate(lifeform);
            Repairer.Reciprocate(water);
            Repairer.Repair(cat, animal, lifeform, water);

            Purifier.PurifyRangeOptimized(new List<Concept>() { cat, animal, lifeform, water });

            Repairer.Repair(cat, animal, lifeform, water);
            Repairer.Reciprocate(cat);
            Repairer.Reciprocate(animal);
            Repairer.Reciprocate(lifeform);
            Repairer.Reciprocate(water);
            Repairer.Repair(cat, animal, lifeform, water);

            if (!ConnectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected");

            if (ConnectionManager.TestConnection(cat, contradict, animal))
                throw new Exception("Shouldn't be connected");
        }

        private static void TestPurifyRangeWithPostContradict()
        {
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept cat = new Concept("cat");
            Concept animal = new Concept("animal");
            Concept lifeform = new Concept("lifeform");
            Concept water = new Concept("water");

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept contradict = new Concept("contradict");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(isa, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);

            Repairer.Repair(cat, animal, lifeform, water);
            Repairer.Reciprocate(cat);
            Repairer.Reciprocate(animal);
            Repairer.Reciprocate(lifeform);
            Repairer.Reciprocate(water);
            Repairer.Repair(cat, animal, lifeform, water);

            ConnectionManager.Plug(cat, isa, animal);

            Repairer.Repair(animal, lifeform);

            ConnectionManager.Plug(animal, isa, lifeform);

            Repairer.Repair(lifeform, water);

            ConnectionManager.Plug(lifeform, madeof, water);

            Repairer.Repair(cat, lifeform);

            ConnectionManager.Plug(cat, contradict, lifeform);

            Repairer.Repair(cat, animal, lifeform, water);

            //Pre-conditions

            if (!ConnectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(cat, contradict, lifeform))
                throw new Exception("Should be connected because it's explicit");

            //Real test here

            MetaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            Repairer.Repair(cat, animal, lifeform, water);
            Repairer.Reciprocate(cat);
            Repairer.Reciprocate(animal);
            Repairer.Reciprocate(lifeform);
            Repairer.Reciprocate(water);
            Repairer.Repair(cat, animal, lifeform, water);

            Purifier.PurifyRangeOptimized(new List<Concept>() { cat, animal, lifeform, water });

            Repairer.Repair(cat, animal, lifeform, water);
            Repairer.Reciprocate(cat);
            Repairer.Reciprocate(animal);
            Repairer.Reciprocate(lifeform);
            Repairer.Reciprocate(water);
            Repairer.Repair(cat, animal, lifeform, water);

            if (!ConnectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected");

            if (ConnectionManager.TestConnection(cat, contradict, animal))
                throw new Exception("Shouldn't be connected");
        }
    }
}
