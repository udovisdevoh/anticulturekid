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
            
            
            Memory memory = new Memory();
            Purifier purifier = new Purifier(repairer);
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

            repairer.Repair(cat, animal, lifeform, water);
            repairer.Reciprocate(cat);
            repairer.Reciprocate(animal);
            repairer.Reciprocate(lifeform);
            repairer.Reciprocate(water);
            repairer.Repair(cat, animal, lifeform, water);

            ConnectionManager.Plug(cat, contradict, lifeform);

            repairer.Repair(cat, animal);

            ConnectionManager.Plug(cat, isa, animal);

            repairer.Repair(animal, lifeform);

            ConnectionManager.Plug(animal, isa, lifeform);

            repairer.Repair(lifeform, water);

            ConnectionManager.Plug(lifeform, madeof, water);

            repairer.Repair(cat, animal, lifeform, water);

            //Pre-conditions

            if (!ConnectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(cat, contradict, lifeform))
                throw new Exception("Should be connected because it's explicit");

            //Real test here

            MetaConnectionManager.AddMetaConnection(isa, "cant", contradict);

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

            if (!ConnectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected");

            if (ConnectionManager.TestConnection(cat, contradict, animal))
                throw new Exception("Shouldn't be connected");
        }

        private static void TestPurifyWithPostContradict()
        {
            Repairer repairer = new Repairer();
            
            
            Purifier purifier = new Purifier(repairer);
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

            repairer.Repair(cat, animal, lifeform, water);
            repairer.Reciprocate(cat);
            repairer.Reciprocate(animal);
            repairer.Reciprocate(lifeform);
            repairer.Reciprocate(water);
            repairer.Repair(cat, animal, lifeform, water);

            ConnectionManager.Plug(cat, isa, animal);

            repairer.Repair(animal, lifeform);

            ConnectionManager.Plug(animal, isa, lifeform);

            repairer.Repair(lifeform, water);

            ConnectionManager.Plug(lifeform, madeof, water);

            repairer.Repair(cat,lifeform);

            ConnectionManager.Plug(cat, contradict, lifeform);

            repairer.Repair(cat, animal, lifeform, water);

            //Pre-conditions

            if (!ConnectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(cat, contradict, lifeform))
                throw new Exception("Should be connected because it's explicit");

            //Real test here

            MetaConnectionManager.AddMetaConnection(isa, "cant", contradict);

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

            if (!ConnectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected");

            if (ConnectionManager.TestConnection(cat, contradict, animal))
                throw new Exception("Shouldn't be connected");
        }

        private static void TestPurifyRangeWithPreContradict()
        {
            Repairer repairer = new Repairer();
            
            
            Purifier purifier = new Purifier(repairer);
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

            repairer.Repair(cat, animal, lifeform, water);
            repairer.Reciprocate(cat);
            repairer.Reciprocate(animal);
            repairer.Reciprocate(lifeform);
            repairer.Reciprocate(water);
            repairer.Repair(cat, animal, lifeform, water);

            ConnectionManager.Plug(cat, contradict, lifeform);

            repairer.Repair(cat, animal);

            ConnectionManager.Plug(cat, isa, animal);

            repairer.Repair(animal, lifeform);

            ConnectionManager.Plug(animal, isa, lifeform);

            repairer.Repair(lifeform, water);

            ConnectionManager.Plug(lifeform, madeof, water);

            repairer.Repair(cat, animal, lifeform, water);

            //Pre-conditions

            if (!ConnectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(cat, contradict, lifeform))
                throw new Exception("Should be connected because it's explicit");

            //Real test here

            MetaConnectionManager.AddMetaConnection(isa, "cant", contradict);

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

            if (!ConnectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected");

            if (ConnectionManager.TestConnection(cat, contradict, animal))
                throw new Exception("Shouldn't be connected");
        }

        private static void TestPurifyRangeWithPostContradict()
        {
            Repairer repairer = new Repairer();
            
            
            Purifier purifier = new Purifier(repairer);
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

            repairer.Repair(cat, animal, lifeform, water);
            repairer.Reciprocate(cat);
            repairer.Reciprocate(animal);
            repairer.Reciprocate(lifeform);
            repairer.Reciprocate(water);
            repairer.Repair(cat, animal, lifeform, water);

            ConnectionManager.Plug(cat, isa, animal);

            repairer.Repair(animal, lifeform);

            ConnectionManager.Plug(animal, isa, lifeform);

            repairer.Repair(lifeform, water);

            ConnectionManager.Plug(lifeform, madeof, water);

            repairer.Repair(cat, lifeform);

            ConnectionManager.Plug(cat, contradict, lifeform);

            repairer.Repair(cat, animal, lifeform, water);

            //Pre-conditions

            if (!ConnectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(cat, contradict, lifeform))
                throw new Exception("Should be connected because it's explicit");

            //Real test here

            MetaConnectionManager.AddMetaConnection(isa, "cant", contradict);

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

            if (!ConnectionManager.TestConnection(cat, isa, animal))
                throw new Exception("Should be connected");

            if (ConnectionManager.TestConnection(cat, contradict, animal))
                throw new Exception("Shouldn't be connected");
        }
    }
}
