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

            proof1.AddArgument(new Proposition(pine, isa, tree));
            proof2.AddArgument(new Proposition(pine, isa, tree));


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


            proof3.AddArgument(new Proposition(pine, isa, plant));

            if (proof1 == proof3)
                throw new Exception("Proofs shouldn't be equal");

            proof1.AddArgument(new Proposition(tree, isa, plant));

            if (proof1 == proof2)
                throw new Exception("Proofs shouldn't be equal");
        }

        private static void TestProofFromDirectImplicationFlattenization()
        {
            
            
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept orbit = new Concept("orbit");
            Concept orbitedby = new Concept("orbitedby");
            Concept gravity = new Concept("gravity");
            Concept attract = new Concept("attract");
            Concept interaction = new Concept("interaction");

            Concept moon = new Concept("moon");
            Concept earth = new Concept("earth");

            MetaConnectionManager.AddMetaConnection(orbit, "inverse_of", orbitedby);
            MetaConnectionManager.AddMetaConnection(gravity, "permutable_side", gravity);
            MetaConnectionManager.AddMetaConnection(attract, "permutable_side", attract);
            MetaConnectionManager.AddMetaConnection(interaction, "permutable_side", interaction);
            MetaConnectionManager.AddMetaConnection(orbit, "direct_implication", gravity);
            MetaConnectionManager.AddMetaConnection(orbitedby, "direct_implication", gravity);
            MetaConnectionManager.AddMetaConnection(gravity, "direct_implication", attract);
            MetaConnectionManager.AddMetaConnection(attract, "direct_implication", interaction);

            ConnectionManager.Plug(moon, orbit, earth);

            Repairer.Repair(moon, earth);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(orbit, "direct_implication", interaction))
                throw new Exception("Should be flat MetaConnected because it's implicit");

            if (!ConnectionManager.TestConnection(moon, orbit, earth))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(moon, gravity, earth))
                throw new Exception("Should be connected because it's implicit");

            if (!ConnectionManager.TestConnection(moon, attract, earth))
                throw new Exception("Should be connected because it's implicit");

            if (!ConnectionManager.TestConnection(moon, interaction, earth))
                throw new Exception("Should be connected because it's implicit");

            //Real test

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(new Proposition(moon, orbit, earth));
            expectedProof.AddArgument(new Proposition(moon, gravity, earth));
            expectedProof.AddArgument(new Proposition(moon, attract, earth));

            Proof proofFound = moon.GetFlatConnectionBranch(interaction).GetProofTo(earth);

            if (proofFound != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestPineMadeofWood()
        {
            
            
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept isa = new Concept("isa");
            Concept madeof = new Concept("madeof");

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");

            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);

            ConnectionManager.Plug(pine, isa, tree);
            
            Repairer.Repair(pine, tree);

            ConnectionManager.Plug(tree, madeof, wood);

            Repairer.Repair(pine, tree, wood);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!ConnectionManager.TestConnection(pine, isa, tree))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(tree, madeof, wood))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(pine, madeof, wood))
                throw new Exception("Should be connected because it's implicit");

            //Real test begins here

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(new Proposition(pine, isa, tree));
            expectedProof.AddArgument(new Proposition(tree, madeof, wood));

            Proof foundProof = pine.GetFlatConnectionBranch(madeof).GetProofTo(wood);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestWoodPartofPine()
        {
            
            
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");

            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);

            ConnectionManager.Plug(pine, isa, tree);

            Repairer.Repair(pine, tree);

            ConnectionManager.Plug(tree, madeof, wood);

            Repairer.Repair(pine, tree, wood);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!ConnectionManager.TestConnection(tree, someare, pine))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(wood, partof, tree))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(wood, partof, pine))
                throw new Exception("Should be connected because it's implicit");

            //Real test begins here

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(new Proposition(wood, partof, tree));
            expectedProof.AddArgument(new Proposition(tree, someare, pine));

            Proof foundProof = wood.GetFlatConnectionBranch(partof).GetProofTo(pine);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestTreeMadeofMaterial()
        {
            
            
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept isa = new Concept("isa");
            Concept madeof = new Concept("madeof");

            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");
            Concept material = new Concept("material");

            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);

            Repairer.Repair(tree,wood);

            ConnectionManager.Plug(tree, madeof, wood);

            Repairer.Repair(wood,material);

            ConnectionManager.Plug(wood, isa, material);

            Repairer.Repair(tree, wood, material);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof, "liffid", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!ConnectionManager.TestConnection(tree, madeof, wood))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(wood, isa, material))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(tree, madeof, material))
                throw new Exception("Should be connected because it's implicit");

            //Real test begins here

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(new Proposition(tree, madeof, wood));
            expectedProof.AddArgument(new Proposition(wood, isa, material));

            Proof foundProof = tree.GetFlatConnectionBranch(madeof).GetProofTo(material);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestMaterialPartofTree()
        {
            
            
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");
            Concept material = new Concept("material");

            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);

            Repairer.Repair(tree, wood);

            ConnectionManager.Plug(tree, madeof, wood);

            Repairer.Repair(wood, material);

            ConnectionManager.Plug(wood, isa, material);

            Repairer.Repair(tree, wood, material);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof, "liffid", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!ConnectionManager.TestConnection(wood, partof, tree))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(material, someare, wood))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(material, partof, tree))
                throw new Exception("Should be connected because it's implicit");

            //Real test begins here

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(new Proposition(material, someare, wood));
            expectedProof.AddArgument(new Proposition(wood, partof, tree));

            Proof foundProof = material.GetFlatConnectionBranch(partof).GetProofTo(tree);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestPineMadeofMaterial()
        {
            
            
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept isa = new Concept("isa");
            Concept madeof = new Concept("madeof");

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");
            Concept material = new Concept("material");

            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);

            ConnectionManager.Plug(pine, isa, tree);

            Repairer.Repair(pine, tree);

            ConnectionManager.Plug(tree, madeof, wood);

            Repairer.Repair(tree, wood);

            ConnectionManager.Plug(wood, isa, material);

            Repairer.Repair(pine, tree, wood, material);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!ConnectionManager.TestConnection(pine, isa, tree))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(tree, madeof, wood))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(pine, madeof, wood))
                throw new Exception("Should be connected because it's implicit");

            if (!ConnectionManager.TestConnection(tree, madeof, material))
                throw new Exception("Should be connected because it's implicit");

            if (!ConnectionManager.TestConnection(pine, madeof, material))
                throw new Exception("Should be connected because it's implicit");

            //Real test begins here

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(new Proposition(pine, isa, tree));
            expectedProof.AddArgument(new Proposition(tree, madeof, wood));
            expectedProof.AddArgument(new Proposition(wood, isa, material));

            Proof foundProof = pine.GetFlatConnectionBranch(madeof).GetProofTo(material);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestMaterialPartofPine()
        {
            
            
            Memory.TotalVerbList = new HashSet<Concept>();
            
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

            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);

            ConnectionManager.Plug(pine, isa, tree);

            Repairer.Repair(pine, tree);

            ConnectionManager.Plug(tree, madeof, wood);

            Repairer.Repair(tree, wood);

            ConnectionManager.Plug(wood, isa, material);

            Repairer.Repair(pine, tree, wood, material);

            Repairer.Repair(wood);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!ConnectionManager.TestConnection(tree, someare, pine))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(wood, partof, tree))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(wood, partof, pine))
                throw new Exception("Should be connected because it's implicit");

            if (!ConnectionManager.TestConnection(material, partof, tree))
                throw new Exception("Should be connected because it's implicit");

            if (!ConnectionManager.TestConnection(material, partof, pine))
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
            expectedProof.AddArgument(new Proposition(wood, partof, tree));
            expectedProof.AddArgument(new Proposition(tree, someare, pine));

            foundProof = wood.GetFlatConnectionBranch(partof).GetProofTo(pine);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");

            //Other pre-condition

            expectedProof = new Proof();
            expectedProof.AddArgument(new Proposition(material, someare, wood));
            expectedProof.AddArgument(new Proposition(wood, partof, tree));

            foundProof = material.GetFlatConnectionBranch(partof).GetProofTo(tree);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");

            //Real test begins here

            expectedProof = new Proof();
            expectedProof.AddArgument(new Proposition(material, someare, wood));
            expectedProof.AddArgument(new Proposition(wood, partof, tree));
            expectedProof.AddArgument(new Proposition(tree, someare, pine));

            foundProof = material.GetFlatConnectionBranch(partof).GetProofTo(pine);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestPineMadeofMatter()
        {
            
            
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept isa = new Concept("isa");
            Concept madeof = new Concept("madeof");

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");
            Concept material = new Concept("material");
            Concept matter = new Concept("matter");

            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);

            ConnectionManager.Plug(pine, isa, tree);

            Repairer.Repair(pine, tree);

            ConnectionManager.Plug(tree, madeof, wood);

            Repairer.Repair(tree, wood);

            ConnectionManager.Plug(wood, isa, material);

            Repairer.Repair(material, matter);

            ConnectionManager.Plug(material, madeof, matter);

            Repairer.Repair(pine, tree, wood, material, matter);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!ConnectionManager.TestConnection(pine, isa, tree))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(tree, madeof, wood))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(pine, madeof, wood))
                throw new Exception("Should be connected because it's implicit");

            if (!ConnectionManager.TestConnection(tree, madeof, material))
                throw new Exception("Should be connected because it's implicit");

            if (!ConnectionManager.TestConnection(pine, madeof, material))
                throw new Exception("Should be connected because it's implicit");

            if (!ConnectionManager.TestConnection(pine, madeof, matter))
                throw new Exception("Should be connected because it's implicit");

            //Real test begins here

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(new Proposition(pine, isa, tree));
            expectedProof.AddArgument(new Proposition(tree, madeof, wood));
            expectedProof.AddArgument(new Proposition(wood, isa, material));
            expectedProof.AddArgument(new Proposition(material, madeof, matter));


            Proof foundProof = pine.GetFlatConnectionBranch(madeof).GetProofTo(matter);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestMatterPartofPine()
        {
            
            
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept wood = new Concept("wood");
            Concept material = new Concept("material");
            Concept matter = new Concept("matter");

            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);
            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);

            ConnectionManager.Plug(pine, isa, tree);

            Repairer.Repair(pine, tree);

            ConnectionManager.Plug(tree, madeof, wood);

            Repairer.Repair(tree, wood);

            ConnectionManager.Plug(wood, isa, material);

            Repairer.Repair(material, matter);

            ConnectionManager.Plug(material, madeof, matter);

            Repairer.Repair(pine, tree, wood, material, matter);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!ConnectionManager.TestConnection(tree,someare,pine))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(wood,partof,tree))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(wood,partof,pine))
                throw new Exception("Should be connected because it's implicit");

            if (!ConnectionManager.TestConnection(material,partof,tree))
                throw new Exception("Should be connected because it's implicit");

            if (!ConnectionManager.TestConnection(material,partof,pine))
                throw new Exception("Should be connected because it's implicit");

            if (!ConnectionManager.TestConnection(matter,partof,pine))
                throw new Exception("Should be connected because it's implicit");

            //Real test begins here

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(new Proposition(matter, partof, material));
            expectedProof.AddArgument(new Proposition(material, someare, wood));
            expectedProof.AddArgument(new Proposition(wood, partof, tree));
            expectedProof.AddArgument(new Proposition(tree, someare, pine));

            Proof foundProof = matter.GetFlatConnectionBranch(partof).GetProofTo(pine);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }

        private static void TestPostMuctDirectImplication()
        {
            
            
            Memory.TotalVerbList = new HashSet<Concept>();
            

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

            MetaConnectionManager.AddMetaConnection(interaction, "permutable_side", interaction);
            MetaConnectionManager.AddMetaConnection(composition, "inverse_of", composed);
            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "direct_implication", composition);
            MetaConnectionManager.AddMetaConnection(composition, "direct_implication", interaction);

            ConnectionManager.Plug(pine, isa, tree);

            Repairer.Repair(pine, tree);

            ConnectionManager.Plug(tree, madeof, wood);

            Repairer.Repair(pine, tree, wood);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!ConnectionManager.TestConnection(pine, isa, tree))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(tree, madeof, wood))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(pine, madeof, wood))
                throw new Exception("Should be connected because it's implicit");

            if (!ConnectionManager.TestConnection(pine, composition, wood))
                throw new Exception("Should be connected because it's implicit");

            //Real test begins here

            Proof expectedProof = new Proof();
            expectedProof.AddArgument(new Proposition(pine, isa, tree));
            expectedProof.AddArgument(new Proposition(tree, madeof, wood));
            expectedProof.AddArgument(new Proposition(pine, madeof, wood));

            Proof foundProof = pine.GetFlatConnectionBranch(composition).GetProofTo(wood);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");

            //Advanced test begins here

            expectedProof = new Proof();
            expectedProof.AddArgument(new Proposition(pine, isa, tree));
            expectedProof.AddArgument(new Proposition(tree, madeof, wood));
            expectedProof.AddArgument(new Proposition(pine, madeof, wood));
            //expectedProof.AddArgument(pine, composition, wood);

            foundProof = pine.GetFlatConnectionBranch(interaction).GetProofTo(wood);

            if (foundProof != expectedProof)
                throw new Exception("Proof doesn't match expected proof");
        }
    }
}