using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestProof
    {
        public static void Test()
        {
            TestBasicOperations();
            TestProofFromDirectImplicationFlattenization();
            TestPineMadeofWood();
            TestWoodPartofPine();
            TestTreeMadeofMaterial();
            TestMaterialPartofTree();
            TestPineMadeofMaterial();
            TestMaterialPartofPine();
            TestPineMadeofMatter();
            TestMatterPartofPine();
            TestPostMuctDirectImplication();
        }

        private static void TestBasicOperations()
        {
            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept plant = new Concept("plant");
            Concept lifeform = new Concept("lifeform");
            Concept animal = new Concept("animal");
            Concept cat = new Concept("cat");

            Concept isa = new Concept("isa");
            Concept madeof = new Concept("madeof");
            Concept contradict = new Concept("contradict");

            Proof proof1 = new Proof();
            Proof proof2 = new Proof();
            Proof proof3 = new Proof();
            Proof proof4 = new Proof();
            Proof proof5 = null;
            Proof proof6 = null;

            proof1.AddArgument(pine, isa, tree);
            proof2.AddArgument(pine, isa, tree);


            if (proof1 == proof3)
                throw new Exception("Proofs shouldn't be equal");

            if (proof1 == proof3)
                throw new Exception("Proofs shouldn't be equal");

            if (proof1 == proof5)
                throw new Exception("Proofs shouldn't be equal");

            if (proof5 != proof6)
                throw new Exception("Proofs should be equal");

            if (!(proof5 == proof6))
                throw new Exception("Proofs should be equal");

            if (proof1 != proof2)
                throw new Exception("Proofs should be equal");

            if (!(proof1 == proof2))
                throw new Exception("Proofs should be equal");


            proof3.AddArgument(pine, isa, plant);

            if (proof1 == proof3)
                throw new Exception("Proofs shouldn't be equal");

            proof1.AddArgument(tree, isa, plant);

            if (proof1 == proof2)
                throw new Exception("Proofs shouldn't be equal");
        }

        private static void TestProofFromDirectImplicationFlattenization()
        {
            ConnectionManager connectionManager = new ConnectionManager();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Memory.TotalVerbList = new HashSet<Concept>();
            Repairer repairer = new Repairer();

            Concept orbit = new Concept("orbit");
            Concept orbitedby = new Concept("orbitedby");
            Concept gravity = new Concept("gravity");
            Concept attract = new Concept("attract");
            Concept interaction = new Concept("interaction");

            Concept moon = new Concept("moon");
            Concept earth = new Concept("earth");

            metaConnectionManager.AddMetaConnection(orbit, "inverse_of", orbitedby);
            metaConnectionManager.AddMetaConnection(gravity, "permutable_side", gravity);
            metaConnectionManager.AddMetaConnection(attract, "permutable_side", attract);
            metaConnectionManager.AddMetaConnection(interaction, "permutable_side", interaction);
            metaConnectionManager.AddMetaConnection(orbit, "direct_implication", gravity);
            metaConnectionManager.AddMetaConnection(orbitedby, "direct_implication", gravity);
            metaConnectionManager.AddMetaConnection(gravity, "direct_implication", attract);
            metaConnectionManager.AddMetaConnection(attract, "direct_implication", interaction);

            connectionManager.Plug(moon, orbit, earth);

            repairer.Repair(moon, earth);

            //Pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(orbit, "direct_implication", interaction))
                throw new Exception("Should be flat MetaConnected because it's implicit");

            if (!connectionManager.TestConnection(moon, orbit, earth))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(moon, gravity, earth))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(moon, attract, earth))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(moon, interaction, earth))
                throw new Exception("Should be connected because it's implicit");

            //Real test

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(moon, orbit, earth);
            expectedProof.AddArgument(moon, gravity, earth);
            expectedProof.AddArgument(moon, attract, earth);

            Proof proofFound = moon.GetFlatConnectionBranch(interaction).GetProofTo(earth);

            if (proofFound != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestPineMadeofWood()
        {
            ConnectionManager connectionManager = new ConnectionManager();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Memory.TotalVerbList = new HashSet<Concept>();
            Repairer repairer = new Repairer();

            Concept isa = new Concept("isa");
            Concept madeof = new Concept("madeof");

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");

            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);

            connectionManager.Plug(pine, isa, tree);
            
            repairer.Repair(pine, tree);

            connectionManager.Plug(tree, madeof, wood);

            repairer.Repair(pine, tree, wood);

            //Pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!connectionManager.TestConnection(pine, isa, tree))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(tree, madeof, wood))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(pine, madeof, wood))
                throw new Exception("Should be connected because it's implicit");

            //Real test begins here

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(pine, isa, tree);
            expectedProof.AddArgument(tree, madeof, wood);

            Proof foundProof = pine.GetFlatConnectionBranch(madeof).GetProofTo(wood);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestWoodPartofPine()
        {
            ConnectionManager connectionManager = new ConnectionManager();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Memory.TotalVerbList = new HashSet<Concept>();
            Repairer repairer = new Repairer();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");

            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);

            connectionManager.Plug(pine, isa, tree);

            repairer.Repair(pine, tree);

            connectionManager.Plug(tree, madeof, wood);

            repairer.Repair(pine, tree, wood);

            //Pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!connectionManager.TestConnection(tree, someare, pine))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(wood, partof, tree))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(wood, partof, pine))
                throw new Exception("Should be connected because it's implicit");

            //Real test begins here

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(wood, partof, tree);
            expectedProof.AddArgument(tree, someare, pine);

            Proof foundProof = wood.GetFlatConnectionBranch(partof).GetProofTo(pine);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestTreeMadeofMaterial()
        {
            ConnectionManager connectionManager = new ConnectionManager();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Memory.TotalVerbList = new HashSet<Concept>();
            Repairer repairer = new Repairer();

            Concept isa = new Concept("isa");
            Concept madeof = new Concept("madeof");

            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");
            Concept material = new Concept("material");

            metaConnectionManager.AddMetaConnection(madeof, "liffid", isa);

            repairer.Repair(tree,wood);

            connectionManager.Plug(tree, madeof, wood);

            repairer.Repair(wood,material);

            connectionManager.Plug(wood, isa, material);

            repairer.Repair(tree, wood, material);

            //Pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(madeof, "liffid", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!connectionManager.TestConnection(tree, madeof, wood))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(wood, isa, material))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(tree, madeof, material))
                throw new Exception("Should be connected because it's implicit");

            //Real test begins here

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(tree, madeof, wood);
            expectedProof.AddArgument(wood, isa, material);

            Proof foundProof = tree.GetFlatConnectionBranch(madeof).GetProofTo(material);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestMaterialPartofTree()
        {
            ConnectionManager connectionManager = new ConnectionManager();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Memory.TotalVerbList = new HashSet<Concept>();
            Repairer repairer = new Repairer();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");
            Concept material = new Concept("material");

            metaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);

            repairer.Repair(tree, wood);

            connectionManager.Plug(tree, madeof, wood);

            repairer.Repair(wood, material);

            connectionManager.Plug(wood, isa, material);

            repairer.Repair(tree, wood, material);

            //Pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(madeof, "liffid", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!connectionManager.TestConnection(wood, partof, tree))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(material, someare, wood))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(material, partof, tree))
                throw new Exception("Should be connected because it's implicit");

            //Real test begins here

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(material, someare, wood);
            expectedProof.AddArgument(wood, partof, tree);

            Proof foundProof = material.GetFlatConnectionBranch(partof).GetProofTo(tree);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestPineMadeofMaterial()
        {
            ConnectionManager connectionManager = new ConnectionManager();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Memory.TotalVerbList = new HashSet<Concept>();
            Repairer repairer = new Repairer();

            Concept isa = new Concept("isa");
            Concept madeof = new Concept("madeof");

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");
            Concept material = new Concept("material");

            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "liffid", isa);

            connectionManager.Plug(pine, isa, tree);

            repairer.Repair(pine, tree);

            connectionManager.Plug(tree, madeof, wood);

            repairer.Repair(tree, wood);

            connectionManager.Plug(wood, isa, material);

            repairer.Repair(pine, tree, wood, material);

            //Pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!connectionManager.TestConnection(pine, isa, tree))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(tree, madeof, wood))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(pine, madeof, wood))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(tree, madeof, material))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(pine, madeof, material))
                throw new Exception("Should be connected because it's implicit");

            //Real test begins here

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(pine, isa, tree);
            expectedProof.AddArgument(tree, madeof, wood);
            expectedProof.AddArgument(wood, isa, material);

            Proof foundProof = pine.GetFlatConnectionBranch(madeof).GetProofTo(material);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestMaterialPartofPine()
        {
            ConnectionManager connectionManager = new ConnectionManager();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Memory.TotalVerbList = new HashSet<Concept>();
            Repairer repairer = new Repairer();
            Proof expectedProof;
            Proof foundProof;

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");
            Concept material = new Concept("material");

            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);

            connectionManager.Plug(pine, isa, tree);

            repairer.Repair(pine, tree);

            connectionManager.Plug(tree, madeof, wood);

            repairer.Repair(tree, wood);

            connectionManager.Plug(wood, isa, material);

            repairer.Repair(pine, tree, wood, material);

            repairer.Repair(wood);

            //Pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!connectionManager.TestConnection(tree, someare, pine))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(wood, partof, tree))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(wood, partof, pine))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(material, partof, tree))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(material, partof, pine))
                throw new Exception("Should be connected because it's implicit");

            //Other pre-condition

            expectedProof = new Proof();

            foundProof = tree.GetFlatConnectionBranch(someare).GetProofTo(pine);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");

            //Other pre-condition

            expectedProof = new Proof();

            foundProof = wood.GetFlatConnectionBranch(partof).GetProofTo(tree);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");

            //Other pre-condition

            expectedProof = new Proof();
            expectedProof.AddArgument(wood, partof, tree);
            expectedProof.AddArgument(tree, someare, pine);

            foundProof = wood.GetFlatConnectionBranch(partof).GetProofTo(pine);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");

            //Other pre-condition

            expectedProof = new Proof();
            expectedProof.AddArgument(material, someare, wood);
            expectedProof.AddArgument(wood, partof, tree);

            foundProof = material.GetFlatConnectionBranch(partof).GetProofTo(tree);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");

            //Real test begins here

            expectedProof = new Proof();
            expectedProof.AddArgument(material, someare, wood);
            expectedProof.AddArgument(wood, partof, tree);
            expectedProof.AddArgument(tree, someare, pine);

            foundProof = material.GetFlatConnectionBranch(partof).GetProofTo(pine);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestPineMadeofMatter()
        {
            ConnectionManager connectionManager = new ConnectionManager();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Memory.TotalVerbList = new HashSet<Concept>();
            Repairer repairer = new Repairer();

            Concept isa = new Concept("isa");
            Concept madeof = new Concept("madeof");

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");
            Concept material = new Concept("material");
            Concept matter = new Concept("matter");

            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            metaConnectionManager.AddMetaConnection(madeof, "muct", madeof);

            connectionManager.Plug(pine, isa, tree);

            repairer.Repair(pine, tree);

            connectionManager.Plug(tree, madeof, wood);

            repairer.Repair(tree, wood);

            connectionManager.Plug(wood, isa, material);

            repairer.Repair(material, matter);

            connectionManager.Plug(material, madeof, matter);

            repairer.Repair(pine, tree, wood, material, matter);

            //Pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!connectionManager.TestConnection(pine, isa, tree))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(tree, madeof, wood))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(pine, madeof, wood))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(tree, madeof, material))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(pine, madeof, material))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(pine, madeof, matter))
                throw new Exception("Should be connected because it's implicit");

            //Real test begins here

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(pine, isa, tree);
            expectedProof.AddArgument(tree, madeof, wood);
            expectedProof.AddArgument(wood, isa, material);
            expectedProof.AddArgument(material, madeof, matter);

            Proof foundProof = pine.GetFlatConnectionBranch(madeof).GetProofTo(matter);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestMatterPartofPine()
        {
            ConnectionManager connectionManager = new ConnectionManager();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Memory.TotalVerbList = new HashSet<Concept>();
            Repairer repairer = new Repairer();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");
            Concept material = new Concept("material");
            Concept matter = new Concept("matter");

            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            metaConnectionManager.AddMetaConnection(madeof, "muct", madeof);
            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);

            connectionManager.Plug(pine, isa, tree);

            repairer.Repair(pine, tree);

            connectionManager.Plug(tree, madeof, wood);

            repairer.Repair(tree, wood);

            connectionManager.Plug(wood, isa, material);

            repairer.Repair(material, matter);

            connectionManager.Plug(material, madeof, matter);

            repairer.Repair(pine, tree, wood, material, matter);

            //Pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!connectionManager.TestConnection(tree,someare,pine))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(wood,partof,tree))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(wood,partof,pine))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(material,partof,tree))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(material,partof,pine))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(matter,partof,pine))
                throw new Exception("Should be connected because it's implicit");

            //Real test begins here

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(matter, partof, material);
            expectedProof.AddArgument(material, someare, wood);
            expectedProof.AddArgument(wood, partof, tree);
            expectedProof.AddArgument(tree, someare, pine);

            Proof foundProof = matter.GetFlatConnectionBranch(partof).GetProofTo(pine);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestPostMuctDirectImplication()
        {
            ConnectionManager connectionManager = new ConnectionManager();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Memory.TotalVerbList = new HashSet<Concept>();
            Repairer repairer = new Repairer();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept composition = new Concept("composition");
            Concept composed = new Concept("composed");
            Concept interaction = new Concept("interaction");

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");

            metaConnectionManager.AddMetaConnection(interaction, "permutable_side", interaction);
            metaConnectionManager.AddMetaConnection(composition, "inverse_of", composed);
            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "direct_implication", composition);
            metaConnectionManager.AddMetaConnection(composition, "direct_implication", interaction);

            connectionManager.Plug(pine, isa, tree);

            repairer.Repair(pine, tree);

            connectionManager.Plug(tree, madeof, wood);

            repairer.Repair(pine, tree, wood);

            //Pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!connectionManager.TestConnection(pine, isa, tree))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(tree, madeof, wood))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(pine, madeof, wood))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(pine, composition, wood))
                throw new Exception("Should be connected because it's implicit");

            //Real test begins here

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(pine, isa, tree);
            expectedProof.AddArgument(tree, madeof, wood);
            expectedProof.AddArgument(pine, madeof, wood);

            Proof foundProof = pine.GetFlatConnectionBranch(composition).GetProofTo(wood);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");

            //Advanced test begins here

            expectedProof = new Proof();
            expectedProof.AddArgument(pine, isa, tree);
            expectedProof.AddArgument(tree, madeof, wood);
            expectedProof.AddArgument(pine, madeof, wood);
            //expectedProof.AddArgument(pine, composition, wood);

            foundProof = pine.GetFlatConnectionBranch(interaction).GetProofTo(wood);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }
    }
}