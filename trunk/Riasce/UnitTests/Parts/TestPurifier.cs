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
            Repairer repairer = new Repairer();
            ConnectionManager connectionManager = new ConnectionManager();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Memory memory = new Memory();
            Purifier purifier = new Purifier(repairer, connectionManager);
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

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);
            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            metaConnectionManager.AddMetaConnection(isa, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "muct", madeof);

            repairer.Repair(cat, animal, lifeform, water);
            repairer.Reciprocate(cat);
            repairer.Reciprocate(animal);
            repairer.Reciprocate(lifeform);
            repairer.Reciprocate(water);
            repairer.Repair(cat, animal, lifeform, water);

            connectionManager.Plug(cat, contradict, lifeform);

            repairer.Repair(cat, animal);

            connectionManager.Plug(cat, isa, animal);

            repairer.Repair(animal, lifeform);

            connectionManager.Plug(animal, isa, lifeform);

            repairer.Repair(lifeform, water);

            connectionManager.Plug(lifeform, madeof, water);

            repairer.Repair(cat, animal, lifeform, water);

            //Pre-conditions

            if (!connectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(cat, contradict, lifeform))
                throw new Exception("Should be connected because it's explicit");

            //Real test here

            metaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            repairer.Repair(cat, animal, lifeform, water);
            repairer.Reciprocate(cat);
            repairer.Reciprocate(animal);
            repairer.Reciprocate(lifeform);
            repairer.Reciprocate(water);
            repairer.Repair(cat, animal, lifeform, water);

            purifier.PurifyOptimized(cat);

            repairer.Repair(cat, animal, lifeform, water);
            repairer.Reciprocate(cat);
            repairer.Reciprocate(animal);
            repairer.Reciprocate(lifeform);
            repairer.Reciprocate(water);
            repairer.Repair(cat, animal, lifeform, water);

            if (!connectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected");

            if (connectionManager.TestConnection(cat, contradict, animal))
                throw new Exception("Shouldn't be connected");
        }

        private static void TestPurifyWithPostContradict()
        {
            Repairer repairer = new Repairer();
            ConnectionManager connectionManager = new ConnectionManager();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Purifier purifier = new Purifier(repairer, connectionManager);
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

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);
            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            metaConnectionManager.AddMetaConnection(isa, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "muct", madeof);

            repairer.Repair(cat, animal, lifeform, water);
            repairer.Reciprocate(cat);
            repairer.Reciprocate(animal);
            repairer.Reciprocate(lifeform);
            repairer.Reciprocate(water);
            repairer.Repair(cat, animal, lifeform, water);

            connectionManager.Plug(cat, isa, animal);

            repairer.Repair(animal, lifeform);

            connectionManager.Plug(animal, isa, lifeform);

            repairer.Repair(lifeform, water);

            connectionManager.Plug(lifeform, madeof, water);

            repairer.Repair(cat,lifeform);

            connectionManager.Plug(cat, contradict, lifeform);

            repairer.Repair(cat, animal, lifeform, water);

            //Pre-conditions

            if (!connectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(cat, contradict, lifeform))
                throw new Exception("Should be connected because it's explicit");

            //Real test here

            metaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            repairer.Repair(cat, animal, lifeform, water);
            repairer.Reciprocate(cat);
            repairer.Reciprocate(animal);
            repairer.Reciprocate(lifeform);
            repairer.Reciprocate(water);
            repairer.Repair(cat, animal, lifeform, water);

            purifier.PurifyOptimized(cat);

            repairer.Repair(cat, animal, lifeform, water);
            repairer.Reciprocate(cat);
            repairer.Reciprocate(animal);
            repairer.Reciprocate(lifeform);
            repairer.Reciprocate(water);
            repairer.Repair(cat, animal, lifeform, water);

            if (!connectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected");

            if (connectionManager.TestConnection(cat, contradict, animal))
                throw new Exception("Shouldn't be connected");
        }

        private static void TestPurifyRangeWithPreContradict()
        {
            Repairer repairer = new Repairer();
            ConnectionManager connectionManager = new ConnectionManager();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Purifier purifier = new Purifier(repairer, connectionManager);
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

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);
            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            metaConnectionManager.AddMetaConnection(isa, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "muct", madeof);

            repairer.Repair(cat, animal, lifeform, water);
            repairer.Reciprocate(cat);
            repairer.Reciprocate(animal);
            repairer.Reciprocate(lifeform);
            repairer.Reciprocate(water);
            repairer.Repair(cat, animal, lifeform, water);

            connectionManager.Plug(cat, contradict, lifeform);

            repairer.Repair(cat, animal);

            connectionManager.Plug(cat, isa, animal);

            repairer.Repair(animal, lifeform);

            connectionManager.Plug(animal, isa, lifeform);

            repairer.Repair(lifeform, water);

            connectionManager.Plug(lifeform, madeof, water);

            repairer.Repair(cat, animal, lifeform, water);

            //Pre-conditions

            if (!connectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(cat, contradict, lifeform))
                throw new Exception("Should be connected because it's explicit");

            //Real test here

            metaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            repairer.Repair(cat, animal, lifeform, water);
            repairer.Reciprocate(cat);
            repairer.Reciprocate(animal);
            repairer.Reciprocate(lifeform);
            repairer.Reciprocate(water);
            repairer.Repair(cat, animal, lifeform, water);

            purifier.PurifyRangeOptimized(new List<Concept>() { cat, animal, lifeform, water });

            repairer.Repair(cat, animal, lifeform, water);
            repairer.Reciprocate(cat);
            repairer.Reciprocate(animal);
            repairer.Reciprocate(lifeform);
            repairer.Reciprocate(water);
            repairer.Repair(cat, animal, lifeform, water);

            if (!connectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected");

            if (connectionManager.TestConnection(cat, contradict, animal))
                throw new Exception("Shouldn't be connected");
        }

        private static void TestPurifyRangeWithPostContradict()
        {
            Repairer repairer = new Repairer();
            ConnectionManager connectionManager = new ConnectionManager();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Purifier purifier = new Purifier(repairer, connectionManager);
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

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);
            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            metaConnectionManager.AddMetaConnection(isa, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "muct", madeof);

            repairer.Repair(cat, animal, lifeform, water);
            repairer.Reciprocate(cat);
            repairer.Reciprocate(animal);
            repairer.Reciprocate(lifeform);
            repairer.Reciprocate(water);
            repairer.Repair(cat, animal, lifeform, water);

            connectionManager.Plug(cat, isa, animal);

            repairer.Repair(animal, lifeform);

            connectionManager.Plug(animal, isa, lifeform);

            repairer.Repair(lifeform, water);

            connectionManager.Plug(lifeform, madeof, water);

            repairer.Repair(cat, lifeform);

            connectionManager.Plug(cat, contradict, lifeform);

            repairer.Repair(cat, animal, lifeform, water);

            //Pre-conditions

            if (!connectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(cat, contradict, lifeform))
                throw new Exception("Should be connected because it's explicit");

            //Real test here

            metaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            repairer.Repair(cat, animal, lifeform, water);
            repairer.Reciprocate(cat);
            repairer.Reciprocate(animal);
            repairer.Reciprocate(lifeform);
            repairer.Reciprocate(water);
            repairer.Repair(cat, animal, lifeform, water);

            purifier.PurifyRangeOptimized(new List<Concept>() { cat, animal, lifeform, water });

            repairer.Repair(cat, animal, lifeform, water);
            repairer.Reciprocate(cat);
            repairer.Reciprocate(animal);
            repairer.Reciprocate(lifeform);
            repairer.Reciprocate(water);
            repairer.Repair(cat, animal, lifeform, water);

            if (!connectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected");

            if (connectionManager.TestConnection(cat, contradict, animal))
                throw new Exception("Shouldn't be connected");
        }
    }
}
