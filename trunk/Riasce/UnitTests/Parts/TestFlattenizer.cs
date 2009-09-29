using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestFlattenizer
    {
        public static void Test()
        {
            TestTreeMadeofMaterial();
            PineMadeofMaterial();
            PineMadeofEnergy();
            TestFlattenizerInverseOfWithMuct();
            TestLongDiversifiedRecursivity();
            TestXmasPineIsaThing();
            TestFlattenizerGeneralStuff();
            TestPostConnectionMetaConnection();
            TestOrbitDirectImplySuckInGoodOrder();
            TestOrbitDirectImplySuckInBadOrderWithPermutalbleSide();
            TestOrbitDirectImplySuckInBadOrder();
            TestNoIsaMuctIsa();
            TestGmNotCreateCarGmMakeCar();
            #warning Test is disabled because subject == complement is not currently allowed
            //TestDoubleSidedLove();
            TestDoubleSidedLoveYouLovePie();
        }

        private static void TestFlattenizerInverseOfWithMuct()
        {
            ConnectionManager connectionManager = new ConnectionManager();
            Repairer repairer = new Repairer();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept plant = new Concept("plant");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(isa, "muct", isa);

            if (connectionManager.TestConnection(pine, isa, plant))
                throw new Exception("Connection shouldn't exist yet");

            repairer.Repair(pine, isa, tree, plant);

            connectionManager.Plug(tree, isa, plant);

            repairer.Repair(pine, isa, tree, plant);

            connectionManager.Plug(pine, isa, tree);

            repairer.Repair(pine, isa, tree, plant);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "inverse_of", someare))
                throw new Exception("MetaConnection should exist because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("MetaConnection should exist because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("MetaConnection should exist because it's implicit");

            if (!connectionManager.TestConnection(tree, isa, plant))
                throw new Exception("Connection should exist because it's explicit");

            if (!connectionManager.TestConnection(plant, someare, tree))
                throw new Exception("Connection should exist because it's explicit");

            if (!connectionManager.TestConnection(pine, isa, tree))
                throw new Exception("Connection should exist because it's explicit");

            if (!connectionManager.TestConnection(tree, someare, pine))
                throw new Exception("Connection should exist because it's explicit");

            //Real test

            if (!connectionManager.TestConnection(pine, isa, plant))
                throw new Exception("Connection should exist because it's implicit");

            if (!connectionManager.TestConnection(plant, someare, pine))
                throw new Exception("Connection should exist because it's implicit");

        }

        private static void TestFlattenizerGeneralStuff()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            ConnectionManager connectionManager = new ConnectionManager();
            SerialFlattenizer flattenizer = new SerialFlattenizer();
            Optimizer optimizer = new Optimizer();

            #region We create the verbs that we will need
            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept contradict = new Concept("contradict");
            Concept love = new Concept("love");
            Concept lovedby = new Concept("lovedby");
            Concept make = new Concept("make");
            Concept madeby = new Concept("madeby");
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
            Concept solid = new Concept("solid");
            Concept material = new Concept("material");
            Concept mother = new Concept("mother");
            Concept child = new Concept("child");
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

            #region Testing the argumentless proof resulting from flattening the 1st layer
            Proof proof;
            connectionManager.Plug(pine, isa, tree);

            proof = pine.GetFlatConnectionBranch(isa).GetProofTo(tree);
            if (proof != null)
                throw new Exception("Proof should be null");

            flattenizer.Repair(pine);

            proof = pine.GetFlatConnectionBranch(isa).GetProofTo(tree);
            if (proof == null)
                throw new Exception("Proof shouldn't be null");

            if (proof.Count != 0)
                throw new Exception("Proof should have no argument because it comes from first layer");

            flattenizer.Repair(tree);

            if (!connectionManager.TestConnection(pine, isa, tree))
                throw new Exception("Connection should exist");

            if (!connectionManager.TestConnection(tree, someare, pine))
                throw new Exception("Connection should exist");
            #endregion

            #region Testing recursive implicit connections
            #region Testing direct implications
            //The child loves the mother
            MetaConnectionManager.AddMetaConnection(make, "inverse_of", madeby);
            MetaConnectionManager.AddMetaConnection(love, "inverse_of", lovedby);
            MetaConnectionManager.AddMetaConnection(make, "inverse_implication", love);

            if (!MetaConnectionManager.IsFlatMetaConnected(make, "inverse_implication", love))
                throw new Exception("Verbs should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(madeby, "inverse_implication", lovedby))
                throw new Exception("Verbs should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(make, "direct_implication", lovedby))
                throw new Exception("Verbs should be flat metaConnected because it's explicit");

            connectionManager.Plug(mother, make, child);

            flattenizer.Repair(mother);
            optimizer.Repair(mother);
            flattenizer.Repair(child);
            optimizer.Repair(child);

            if (!connectionManager.TestConnection(child, love, mother))
                throw new Exception("Concepts should be connected because it's implicit");

            if (!connectionManager.TestConnection(mother, lovedby, child))
                throw new Exception("Concepts should be connected because it's implicit");

            Proof testProof;
            testProof = new Proof();
            testProof.AddArgument(mother, make, child);
            if (mother.GetFlatConnectionBranch(lovedby).GetProofTo(child) != testProof)
                throw new Exception("Proof should match");

            testProof = new Proof();
            testProof.AddArgument(child, madeby, mother);
            if (child.GetFlatConnectionBranch(love).GetProofTo(mother) != testProof)
                throw new Exception("Proof should match");

            #endregion

            flattenizer.Repair(tree);
            optimizer.Repair(tree);
            flattenizer.Repair(pine);
            optimizer.Repair(pine);
            flattenizer.Repair(plant);
            optimizer.Repair(plant);

            if (connectionManager.TestConnection(pine, isa, plant))
                throw new Exception("Connection shouldn't exist yet exist");

            if (connectionManager.TestConnection(plant, someare, pine))
                throw new Exception("Connection shouldn't exist yet exist");
            connectionManager.Plug(tree, isa, plant);

            flattenizer.Repair(tree);
            optimizer.Repair(tree);
            flattenizer.Repair(pine);
            optimizer.Repair(pine);
            flattenizer.Repair(plant);
            optimizer.Repair(plant);

            if (!connectionManager.TestConnection(tree, isa, plant))
                throw new Exception("Connection should exist");

            if (!connectionManager.TestConnection(pine, isa, plant))
                throw new Exception("Connection should exist");

            if (!connectionManager.TestConnection(plant, someare, pine))
                throw new Exception("Connection should exist");

            if (connectionManager.TestConnection(plant, isa, pine))
                throw new Exception("Connection shouldn't exist exist because it's illogical");

            if (connectionManager.TestConnection(lifeform, someare, pine))
                throw new Exception("Connection shouldn't exist yet");

            if (connectionManager.TestConnection(pine, isa, lifeform))
                throw new Exception("Connection shouldn't exist yet");

            connectionManager.Plug(plant, isa, lifeform);

            flattenizer.Repair(tree);
            optimizer.Repair(tree);
            flattenizer.Repair(plant);
            optimizer.Repair(plant);
            flattenizer.Repair(lifeform);
            optimizer.Repair(lifeform);
            flattenizer.Repair(pine);
            optimizer.Repair(pine);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("MetaConnection should exist");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("MetaConnection should exist");

            //Real test

            if (!connectionManager.TestConnection(lifeform, someare, pine))
                throw new Exception("Connection should exist");

            if (!connectionManager.TestConnection(pine, isa, lifeform))
                throw new Exception("Connection should exist");

            flattenizer.Repair(plant);
            optimizer.Repair(plant);
            flattenizer.Repair(lifeform);
            optimizer.Repair(lifeform);
            flattenizer.Repair(tree);
            optimizer.Repair(tree);

            if (connectionManager.TestConnection(pine, madeof, wood))
                throw new Exception("Connection shouldn't exist yet");

            connectionManager.Plug(tree, madeof, wood);

            flattenizer.Repair(wood);
            optimizer.Repair(wood);
            flattenizer.Repair(pine);
            optimizer.Repair(pine);
            flattenizer.Repair(tree);
            optimizer.Repair(tree);

            if (!connectionManager.TestConnection(pine, madeof, wood))
                throw new Exception("Connection should exist");

            if (connectionManager.TestConnection(pine, madeof, material))
                throw new Exception("Connection shouldn't exist");

            connectionManager.Plug(wood, isa, material);

            flattenizer.Repair(pine);
            optimizer.Repair(pine);
            flattenizer.Repair(material);
            optimizer.Repair(material);

            if (!connectionManager.TestConnection(pine, madeof, material))
                throw new Exception("Connection should exist");

            if (!connectionManager.TestConnection(material, partof, pine))
                throw new Exception("Connection should exist");


            flattenizer.Repair(water);
            optimizer.Repair(water);
            flattenizer.Repair(solid);
            optimizer.Repair(solid);
            flattenizer.Repair(liquid);
            optimizer.Repair(liquid);
            flattenizer.Repair(material);
            optimizer.Repair(material);
            flattenizer.Repair(wood);
            optimizer.Repair(wood);
            flattenizer.Repair(pine);
            optimizer.Repair(pine);

            if (connectionManager.TestConnection(wood, contradict, water))
                throw new Exception("Connection shouldn't exist yet");

            connectionManager.Plug(liquid, contradict, solid);

            flattenizer.Repair(water);
            optimizer.Repair(water);
            flattenizer.Repair(solid);
            optimizer.Repair(solid);
            flattenizer.Repair(liquid);
            optimizer.Repair(liquid);
            flattenizer.Repair(material);
            optimizer.Repair(material);
            flattenizer.Repair(wood);
            optimizer.Repair(wood);
            flattenizer.Repair(pine);
            optimizer.Repair(pine);

            connectionManager.Plug(solid, isa, material);

            flattenizer.Repair(water);
            optimizer.Repair(water);
            flattenizer.Repair(solid);
            optimizer.Repair(solid);
            flattenizer.Repair(liquid);
            optimizer.Repair(liquid);
            flattenizer.Repair(material);
            optimizer.Repair(material);
            flattenizer.Repair(wood);
            optimizer.Repair(wood);
            flattenizer.Repair(pine);
            optimizer.Repair(pine);

            connectionManager.Plug(liquid, isa, material);

            flattenizer.Repair(water);
            optimizer.Repair(water);
            flattenizer.Repair(solid);
            optimizer.Repair(solid);
            flattenizer.Repair(liquid);
            optimizer.Repair(liquid);
            flattenizer.Repair(material);
            optimizer.Repair(material);
            flattenizer.Repair(wood);
            optimizer.Repair(wood);
            flattenizer.Repair(pine);
            optimizer.Repair(pine);

            connectionManager.Plug(wood, isa, solid);

            flattenizer.Repair(water);
            optimizer.Repair(water);
            flattenizer.Repair(solid);
            optimizer.Repair(solid);
            flattenizer.Repair(liquid);
            optimizer.Repair(liquid);
            flattenizer.Repair(material);
            optimizer.Repair(material);
            flattenizer.Repair(wood);
            optimizer.Repair(wood);
            flattenizer.Repair(pine);
            optimizer.Repair(pine);

            connectionManager.Plug(water, isa, liquid);

            flattenizer.Repair(water);
            optimizer.Repair(water);
            flattenizer.Repair(solid);
            optimizer.Repair(solid);
            flattenizer.Repair(liquid);
            optimizer.Repair(liquid);
            flattenizer.Repair(material);
            optimizer.Repair(material);
            flattenizer.Repair(wood);
            optimizer.Repair(wood);
            flattenizer.Repair(pine);
            optimizer.Repair(pine);

            if (!connectionManager.TestConnection(solid, contradict, liquid))
                throw new Exception("Connection should exist");

            if (!connectionManager.TestConnection(liquid, contradict, solid))
                throw new Exception("Connection should exist");

            if (!connectionManager.TestConnection(water, contradict, solid))
                throw new Exception("Connection should exist");

            if (!connectionManager.TestConnection(solid, contradict, water))
                throw new Exception("Connection should exist");

            if (!connectionManager.TestConnection(wood, contradict, liquid))
                throw new Exception("Connection should exist");

            if (!connectionManager.TestConnection(liquid, contradict, wood))
                throw new Exception("Connection should exist");

            if (!connectionManager.TestConnection(water, contradict, wood))
                throw new Exception("Connection should exist");

            if (!connectionManager.TestConnection(wood, contradict, water))
                throw new Exception("Connection should exist");

            if (connectionManager.TestConnection(pine, contradict, liquid))
                throw new Exception("Connection shouldn't exist because it's illogical");

            if (connectionManager.TestConnection(pine, madeof, water))
                throw new Exception("Connection shouldn't exist yet");

            flattenizer.Repair(lifeform);
            optimizer.Repair(lifeform);

            connectionManager.Plug(lifeform, madeof, water);

            flattenizer.Repair(lifeform);
            optimizer.Repair(lifeform);

            flattenizer.Repair(pine);
            optimizer.Repair(pine);

            flattenizer.Repair(water);
            optimizer.Repair(water);

            if (!connectionManager.TestConnection(pine, madeof, water))
                throw new Exception("Connection should exist");
            #endregion
        }

        private static void TestLongDiversifiedRecursivity()
        {
            ConnectionManager connectionManager = new ConnectionManager();
            Repairer repairer = new Repairer();

            Memory.TotalVerbList = new HashSet<Concept>();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            Concept mygarden = new Concept("my garden");
            Concept garden = new Concept("garden");
            Concept plant = new Concept("plant");
            Concept lifeform = new Concept("lifeform");
            Concept water = new Concept("water");
            Concept liquid = new Concept("liquid");
            Concept matter = new Concept("matter");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);

            connectionManager.Plug(mygarden, isa, garden);
            
            repairer.Repair(garden, plant);

            connectionManager.Plug(garden, madeof, plant);
            
            repairer.Repair(plant, lifeform);
            
            connectionManager.Plug(plant, isa, lifeform);

            repairer.Repair(lifeform, water);

            connectionManager.Plug(lifeform, madeof, water);

            repairer.Repair(water, liquid);

            connectionManager.Plug(water, isa, liquid);

            repairer.Repair(liquid, water);

            connectionManager.Plug(liquid, madeof, matter);

            //pre-conditions

            repairer.Repair(mygarden, garden);

            if (!connectionManager.TestConnection(mygarden, isa, garden))
                throw new Exception("Should be connected because it's explicit");

            //real test

            repairer.Repair(mygarden, plant);

            if (!connectionManager.TestConnection(mygarden, madeof, plant))
                throw new Exception("Should be connected because it's implicit");

            repairer.Repair(mygarden, lifeform);

            if (!connectionManager.TestConnection(mygarden, madeof, lifeform))
                throw new Exception("Should be connected because it's implicit");

            repairer.Repair(mygarden, water);

            if (!connectionManager.TestConnection(mygarden, madeof, water))
                throw new Exception("Should be connected because it's implicit");

            repairer.Repair(mygarden, liquid);

            if (!connectionManager.TestConnection(mygarden, madeof, liquid))
                throw new Exception("Should be connected because it's implicit");

            repairer.Repair(mygarden, matter);

            if (!connectionManager.TestConnection(mygarden, madeof, matter))
                throw new Exception("Should be connected because it's implicit");
        }

        private static void TestXmasPineIsaThing()
        {
            ConnectionManager connectionManager = new ConnectionManager();
            Repairer repairer = new Repairer();

            Memory.TotalVerbList = new HashSet<Concept>();

            Concept xmaspine = new Concept("christmas pine");
            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept plant = new Concept("plant");
            Concept lifeform = new Concept("lifeform");
            Concept thing = new Concept("thing");

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");

            MetaConnectionManager.AddMetaConnection(isa, "muct", isa);
            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);

            connectionManager.Plug(xmaspine, isa, pine);

            repairer.Repair(pine, tree);

            connectionManager.Plug(pine, isa, tree);
            
            repairer.Repair(tree, plant);

            connectionManager.Plug(tree, isa, plant);

            repairer.Repair(plant, lifeform);

            connectionManager.Plug(plant, isa, lifeform);

            repairer.Repair(lifeform, thing);

            connectionManager.Plug(lifeform, isa, thing);

            repairer.Repair(xmaspine, pine, tree, plant, lifeform, thing);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(someare,"liffid",someare))
                throw new Exception("Should be flat metaConnected because it's implicit");

            //Real test

            if (!connectionManager.TestConnection(xmaspine, isa, pine))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(xmaspine, isa, tree))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(xmaspine, isa, plant))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(xmaspine, isa, lifeform))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(xmaspine, isa, thing))
                throw new Exception("Should be connected because it's implicit");
        }

        private static void TestTreeMadeofMaterial()
        {
            ConnectionManager connectionManager = new ConnectionManager();
            SerialFlattenizer flattenizer = new SerialFlattenizer();
            Optimizer optimizer = new Optimizer();
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");
            Concept material = new Concept("material");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);

            flattenizer.Repair(tree);
            optimizer.Repair(tree);
            flattenizer.Repair(wood);
            optimizer.Repair(wood);
            flattenizer.Repair(material);
            optimizer.Repair(material);

            if (connectionManager.TestConnection(tree, madeof, material))
                throw new Exception("Connection shouldn't exist yet");


            connectionManager.Plug(tree, madeof, wood);

            flattenizer.Repair(tree);
            optimizer.Repair(tree);
            flattenizer.Repair(wood);
            optimizer.Repair(wood);
            flattenizer.Repair(material);
            optimizer.Repair(material);

            connectionManager.Plug(wood, isa, material);

            flattenizer.Repair(tree);
            optimizer.Repair(tree);
            flattenizer.Repair(wood);
            optimizer.Repair(wood);
            flattenizer.Repair(material);
            optimizer.Repair(material);

            if (!connectionManager.TestConnection(tree, madeof, material))
                throw new Exception("Connection should exist because it's implicit");
        }

        private static void PineMadeofMaterial()
        {
            ConnectionManager connectionManager = new ConnectionManager();
            Repairer repairer = new Repairer();
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            Concept pine = new Concept("tree");
            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");
            Concept material = new Concept("material");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(isa, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            

            repairer.Repair(pine, tree);

            connectionManager.Plug(pine, isa, tree);

            repairer.Repair(tree,wood);

            connectionManager.Plug(tree,madeof,wood);

            repairer.Repair(wood, material);

            connectionManager.Plug(wood,isa,material);

            repairer.Repair(pine,tree,wood,material);

            //Pre-conditions

            if (!connectionManager.TestConnection(pine, isa, tree))
                throw new Exception("Concepts should be connected because it's explicit");

            if (!connectionManager.TestConnection(tree,madeof,wood))
                throw new Exception("Concepts should be connected because it's explicit");

            if (!connectionManager.TestConnection(wood, isa, material))
                throw new Exception("Concepts should be connected because it's explicit");

            //Real test

            if (!connectionManager.TestConnection(pine, madeof, wood))
                throw new Exception("Concepts should be connected because it's implicit");

            if (!connectionManager.TestConnection(tree, madeof, material))
                throw new Exception("Concepts should be connected because it's implicit");

            if (!connectionManager.TestConnection(pine, madeof, material))
                throw new Exception("Concepts should be connected because it's implicit");
        }

        private static void PineMadeofEnergy()
        {
            
            ConnectionManager connectionManager = new ConnectionManager();
            Repairer repairer = new Repairer();
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            Concept pine = new Concept("tree");
            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");
            Concept material = new Concept("material");
            Concept matter = new Concept("matter");
            Concept energy = new Concept("energy");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(isa, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);


            repairer.Repair(pine, tree);

            connectionManager.Plug(pine, isa, tree);

            repairer.Repair(tree, wood);

            connectionManager.Plug(tree, madeof, wood);

            repairer.Repair(wood, material);

            connectionManager.Plug(wood, isa, material);

            repairer.Repair(material, matter);

            connectionManager.Plug(material,madeof,matter);

            repairer.Repair(matter, energy);

            connectionManager.Plug(matter, madeof, energy);

            repairer.Repair(pine, tree, wood, material,matter,energy);

            //Pre-conditions

            if (!connectionManager.TestConnection(pine, isa, tree))
                throw new Exception("Concepts should be connected because it's explicit");

            if (!connectionManager.TestConnection(tree, madeof, wood))
                throw new Exception("Concepts should be connected because it's explicit");

            if (!connectionManager.TestConnection(wood, isa, material))
                throw new Exception("Concepts should be connected because it's explicit");

            //Real test

            if (!connectionManager.TestConnection(pine, madeof, wood))
                throw new Exception("Concepts should be connected because it's implicit");

            if (!connectionManager.TestConnection(tree, madeof, material))
                throw new Exception("Concepts should be connected because it's implicit");

            if (!connectionManager.TestConnection(pine, madeof, material))
                throw new Exception("Concepts should be connected because it's implicit");

            if (!connectionManager.TestConnection(pine, madeof, energy))
                throw new Exception("Concepts should be connected because it's implicit");
        }

        private static void TestPostConnectionMetaConnection()
        {
            /*Problème:
            lorsqu'on rajoute un "permutable_side" entre isa et someare et que des connections isa
            existent déjà, les connections someare contraposées ne sont pas crées*/


            Memory.TotalVerbList = new HashSet<Concept>();
            ConnectionManager connectionManager = new ConnectionManager();
            
            Reciprocator reciprocator = new Reciprocator();
            Repairer repairer = new Repairer();

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");

            /*
            MetaConnectionManager.AddMetaConnection(isa, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            */

            repairer.Repair(pine, tree);

            connectionManager.Plug(pine, isa, tree);

            repairer.Repair(tree, wood);

            connectionManager.Plug(tree, madeof, wood);

            repairer.Repair(pine, tree, wood);

            MetaConnectionManager.AddMetaConnection(isa, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);

            repairer.Repair(pine, tree, wood);

            reciprocator.Reciprocate(pine);

            repairer.Repair(pine, tree, wood);

            reciprocator.Reciprocate(tree);

            repairer.Repair(pine, tree, wood);

            reciprocator.Reciprocate(wood);

            repairer.Repair(pine, tree, wood);

            //Pre-conditions

            if (!connectionManager.TestConnection(tree, madeof, wood))
                throw new Exception("Should be connected because it's explicit");

            //Test begins here

            if (!connectionManager.TestConnection(pine, madeof, wood))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(pine, isa, tree))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(tree,someare,pine))
                throw new Exception("Should be connected because it's implicit");
        }

        private static void TestOrbitDirectImplySuckInGoodOrder()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            ConnectionManager connectionManager = new ConnectionManager();
            
            Reciprocator reciprocator = new Reciprocator();
            Repairer repairer = new Repairer();

            Concept gravity = new Concept("gravity");
            Concept attract = new Concept("attract");
            Concept suck = new Concept("suck");
            Concept orbit = new Concept("orbit");

            Concept moon = new Concept("moon");
            Concept earth = new Concept("earth");


            MetaConnectionManager.AddMetaConnection(orbit, "permutable_side", orbit);
            MetaConnectionManager.AddMetaConnection(gravity, "permutable_side", gravity);
            MetaConnectionManager.AddMetaConnection(attract, "permutable_side", attract);
            MetaConnectionManager.AddMetaConnection(suck, "permutable_side", suck);
            MetaConnectionManager.AddMetaConnection(orbit, "direct_implication", gravity);
            MetaConnectionManager.AddMetaConnection(gravity, "direct_implication", attract);
            MetaConnectionManager.AddMetaConnection(attract, "direct_implication", suck);

            connectionManager.Plug(moon, orbit, earth);

            repairer.Repair(moon, earth);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(orbit, "direct_implication", gravity))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(orbit, "direct_implication", attract))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(orbit, "direct_implication", suck))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!connectionManager.TestConnection(moon, orbit, earth))
                throw new Exception("Should be flat connected because it's implicit");

            if (!connectionManager.TestConnection(moon, gravity, earth))
                throw new Exception("Should be flat connected because it's implicit");

            if (!connectionManager.TestConnection(moon, attract, earth))
                throw new Exception("Should be flat connected because it's implicit");

            //Real test begins here

            if (!connectionManager.TestConnection(moon,suck,earth))
                throw new Exception("Should be flat connected because it's implicit");
        }

        private static void TestOrbitDirectImplySuckInBadOrderWithPermutalbleSide()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            ConnectionManager connectionManager = new ConnectionManager();
            
            Reciprocator reciprocator = new Reciprocator();
            Repairer repairer = new Repairer();

            Concept gravity = new Concept("gravity");
            Concept attract = new Concept("attract");
            Concept suck = new Concept("suck");
            Concept orbit = new Concept("orbit");

            Concept moon = new Concept("moon");
            Concept earth = new Concept("earth");

            MetaConnectionManager.AddMetaConnection(orbit, "permutable_side", orbit);
            MetaConnectionManager.AddMetaConnection(gravity, "permutable_side", gravity);
            MetaConnectionManager.AddMetaConnection(attract, "permutable_side", attract);
            MetaConnectionManager.AddMetaConnection(suck, "permutable_side", suck);

            MetaConnectionManager.AddMetaConnection(gravity, "direct_implication", attract);
            MetaConnectionManager.AddMetaConnection(attract, "direct_implication", suck);
            MetaConnectionManager.AddMetaConnection(orbit, "direct_implication", gravity);
            
            connectionManager.Plug(moon, orbit, earth);

            repairer.Repair(moon, earth);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(orbit, "direct_implication", gravity))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(orbit, "direct_implication", attract))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(orbit, "direct_implication", suck))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!connectionManager.TestConnection(moon, orbit, earth))
                throw new Exception("Should be flat connected because it's implicit");

            if (!connectionManager.TestConnection(moon, gravity, earth))
                throw new Exception("Should be flat connected because it's implicit");

            if (!connectionManager.TestConnection(moon, attract, earth))
                throw new Exception("Should be flat connected because it's implicit");

            //Real test begins here

            if (!connectionManager.TestConnection(moon, suck, earth))
                throw new Exception("Should be flat connected because it's implicit");
        }

        private static void TestOrbitDirectImplySuckInBadOrder()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            ConnectionManager connectionManager = new ConnectionManager();
            
            Reciprocator reciprocator = new Reciprocator();
            Repairer repairer = new Repairer();

            Concept gravity = new Concept("gravity");
            Concept attract = new Concept("attract");
            Concept suck = new Concept("suck");
            Concept orbit = new Concept("orbit");

            Concept moon = new Concept("moon");
            Concept earth = new Concept("earth");


            MetaConnectionManager.AddMetaConnection(orbit, "permutable_side", orbit);
            MetaConnectionManager.AddMetaConnection(gravity, "permutable_side",gravity);
            MetaConnectionManager.AddMetaConnection(attract, "permutable_side", attract);
            MetaConnectionManager.AddMetaConnection(suck, "permutable_side", suck);
            MetaConnectionManager.AddMetaConnection(gravity, "direct_implication", attract);
            MetaConnectionManager.AddMetaConnection(attract, "direct_implication", suck);
            MetaConnectionManager.AddMetaConnection(orbit, "direct_implication", gravity);

            connectionManager.Plug(moon, orbit, earth);

            repairer.Repair(moon, earth);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(orbit, "direct_implication", gravity))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(orbit, "direct_implication", attract))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(orbit, "direct_implication", suck))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!connectionManager.TestConnection(moon, orbit, earth))
                throw new Exception("Should be flat connected because it's implicit");

            if (!connectionManager.TestConnection(moon, gravity, earth))
                throw new Exception("Should be flat connected because it's implicit");

            if (!connectionManager.TestConnection(moon, attract, earth))
                throw new Exception("Should be flat connected because it's implicit");

            //Real test begins here

            if (!connectionManager.TestConnection(moon, suck, earth))
                throw new Exception("Should be flat connected because it's implicit");
        }

        private static void TestNoIsaMuctIsa()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            ConnectionManager connectionManager = new ConnectionManager();
            
            Reciprocator reciprocator = new Reciprocator();
            Repairer repairer = new Repairer();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept plant = new Concept("plant");
            Concept water = new Concept("water");
            List<Concept> memory = new List<Concept>() { isa, someare, madeof, partof, pine, tree, plant, water };

            Purifier purifier = new Purifier(repairer, connectionManager);

            repairer.RepairRange(memory);
            repairer.ReciprocateRange(memory);
            purifier.PurifyRangeOptimized(memory);
            repairer.RepairRange(memory);
            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);

            repairer.RepairRange(memory);
            repairer.ReciprocateRange(memory);
            purifier.PurifyRangeOptimized(memory);
            repairer.RepairRange(memory);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);

            repairer.RepairRange(memory);
            repairer.ReciprocateRange(memory);
            purifier.PurifyRangeOptimized(memory);
            repairer.RepairRange(memory);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);

            repairer.RepairRange(memory);
            repairer.ReciprocateRange(memory);
            purifier.PurifyRangeOptimized(memory);
            repairer.RepairRange(memory);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);

            /*repairer.RepairRange(memory);
            repairer.ReciprocateRange(memory);
            purifier.PurifyRange(memory);
            repairer.RepairRange(memory);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);*/

            repairer.Repair(pine,isa,tree);
            connectionManager.Plug(pine, isa, tree);

            repairer.Repair(tree, isa, plant);
            connectionManager.Plug(tree,isa,plant);

            repairer.Repair(plant, madeof, water);
            connectionManager.Plug(plant,madeof,water);

            repairer.Repair(water,tree,plant,pine);

            //pre-conditions about metaConnections

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Should be metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(partof, "muct", someare))
                throw new Exception("Should be metaConnected because it's implicit");

            //pre-conditions about connections

            if (!connectionManager.TestConnection(pine,isa,tree))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(plant, madeof, water))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(tree, madeof, water))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(water,partof,tree))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(tree,someare,pine))
                throw new Exception("Should be connected because it's implicit");

            //real test here

            if (!connectionManager.TestConnection(pine, madeof, water))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(water, partof, pine))
                throw new Exception("Should be connected because it's implicit");
        }

        private static void TestGmNotCreateCarGmMakeCar()
        {
            
            ConnectionManager connectionManager = new ConnectionManager();
            Repairer repairer = new Repairer();
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept make = new Concept("make");
            Concept madeby = new Concept("madeby");
            Concept create = new Concept("create");
            Concept createdby = new Concept("createdby");

            Concept car = new Concept("car");
            Concept gm = new Concept("gm");

            MetaConnectionManager.AddMetaConnection(make, "inverse_of", madeby);
            MetaConnectionManager.AddMetaConnection(create, "inverse_of", createdby);
            MetaConnectionManager.AddMetaConnection(create, "direct_implication", make);

            //Real test here

            repairer.Repair(gm, create, car);
            connectionManager.Plug(gm, create, car);
            repairer.Repair(gm, create, car);

            repairer.Repair(gm, create, car);
            connectionManager.UnPlug(gm, create, car);
            repairer.Repair(gm, create, car);
            repairer.Repair(gm, create, car);
            
            repairer.Repair(gm, make, car);
            connectionManager.Plug(gm, make, car);
            repairer.Repair(gm, make, car);

            repairer.Repair(gm);
            repairer.Repair(car);
            if (!connectionManager.TestConnection(gm, make, car))
                throw new Exception("Should be connected because it's explicit");
        }

        private static void TestDoubleSidedLove()
        {
            
            ConnectionManager connectionManager = new ConnectionManager();
            Repairer repairer = new Repairer();
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept love = new Concept("love");
            Concept lovedby = new Concept("lovedby");

            Concept me = new Concept("me");
            Concept you = new Concept("you");

            MetaConnectionManager.AddMetaConnection(love, "inverse_of", lovedby);
            MetaConnectionManager.AddMetaConnection(love, "muct", love);

            //Real test here

            repairer.Repair(me, love, you);
            connectionManager.Plug(me, love, you);
            repairer.Repair(me, love, you);
            connectionManager.Plug(you, love, me);
            repairer.Repair(me, love, you);

            Proof expectedProof1 = new Proof();
            expectedProof1.AddArgument(me, love, you);
            expectedProof1.AddArgument(you, love, me);

            Proof expectedProof2 = new Proof();
            expectedProof2.AddArgument(you, love, me);
            expectedProof2.AddArgument(me, love, you);

            Proof proof = connectionManager.GetProofToConnection(me, love, me);

            if (proof != expectedProof1 && proof != expectedProof2)
                throw new Exception("Proof should match");
        }

        private static void TestDoubleSidedLoveYouLovePie()
        {
            
            ConnectionManager connectionManager = new ConnectionManager();
            Repairer repairer = new Repairer();
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept love = new Concept("love");
            Concept lovedby = new Concept("lovedby");

            Concept me = new Concept("me");
            Concept you = new Concept("you");
            Concept pie = new Concept("pie");

            MetaConnectionManager.AddMetaConnection(love, "inverse_of", lovedby);
            MetaConnectionManager.AddMetaConnection(love, "muct", love);

            repairer.Repair(me, love, you);
            connectionManager.Plug(me, love, you);
            repairer.Repair(me, love, you);
            connectionManager.Plug(you, love, me);
            repairer.Repair(me, love, you, pie);

            //real test here

            Proof proof;

            connectionManager.Plug(you, love, pie);
            //repairer.Repair(you, pie, me, love);
            repairer.Repair(pie, you, me, love);

            proof = connectionManager.GetProofToConnection(you, love, pie);

            repairer.Repair(you, pie, me, love);

            if (!connectionManager.TestConnection(you, love, pie))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(me, love, pie))
                throw new Exception("Should be connected because it's implicit");

            //Advanced test here

            repairer.Repair(pie, you, me, love);

            connectionManager.UnPlug(you, love, pie);

            repairer.Repair(pie, you, me, love);

            if (connectionManager.TestConnection(you, love, pie))
                throw new Exception("Shouldn't be connected");

            if (connectionManager.TestConnection(me, love, pie))
                throw new Exception("Shouldn't be connected");
        }
    }
}
