﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestAiSqlWrapper
    {
        public static void Test()
        {
            TestSelect();   
        }

        private static void TestSelect()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            Memory memory = new Memory();
            NameMapper nameMapper = new NameMapper(new Name("aiName"), new Name("humanName"));
            ConnectionManager connectionManager = new ConnectionManager();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Repairer repairer = new Repairer();
            AiSqlWrapper aiSqlWrapper = new AiSqlWrapper(metaConnectionManager);
            HashSet<Concept> selection;

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            Concept pine = new Concept("pine");
            Concept willow = new Concept("willow");
            Concept palmtree = new Concept("palm tree");
            Concept tree = new Concept("tree");
            Concept plant = new Concept("plant");
            Concept animal = new Concept("animal");
            Concept cat = new Concept("cat");
            Concept lifeform = new Concept("lifeform");
            Concept cactus = new Concept("cactus");

            memory.SetConcept(nameMapper.GetOrCreateConceptId("isa"), isa);
            memory.SetConcept(nameMapper.GetOrCreateConceptId("someare"), someare);
            memory.SetConcept(nameMapper.GetOrCreateConceptId("madeof"), madeof);
            memory.SetConcept(nameMapper.GetOrCreateConceptId("partof"), partof);
            memory.SetConcept(nameMapper.GetOrCreateConceptId("pine"), pine);
            memory.SetConcept(nameMapper.GetOrCreateConceptId("willow"), willow);
            memory.SetConcept(nameMapper.GetOrCreateConceptId("palmtree"), palmtree);
            memory.SetConcept(nameMapper.GetOrCreateConceptId("tree"), tree);
            memory.SetConcept(nameMapper.GetOrCreateConceptId("plant"), plant);
            memory.SetConcept(nameMapper.GetOrCreateConceptId("animal"), animal);
            memory.SetConcept(nameMapper.GetOrCreateConceptId("cat"), cat);
            memory.SetConcept(nameMapper.GetOrCreateConceptId("lifeform"), lifeform);
            memory.SetConcept(nameMapper.GetOrCreateConceptId("cactus"), cactus);

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            metaConnectionManager.AddMetaConnection(madeof, "muct", madeof);

            repairer.RepairRange(memory);
            repairer.RepairRange(memory);
            connectionManager.Plug(lifeform, someare, plant);
            repairer.RepairRange(memory);
            repairer.RepairRange(memory);
            connectionManager.Plug(lifeform, someare, animal);
            repairer.RepairRange(memory);
            repairer.RepairRange(memory);
            connectionManager.Plug(tree, isa, plant);
            repairer.RepairRange(memory);
            repairer.RepairRange(memory);
            connectionManager.Plug(cat, isa, animal);
            repairer.RepairRange(memory);
            repairer.RepairRange(memory);
            connectionManager.Plug(pine, isa, tree);
            repairer.RepairRange(memory);
            repairer.RepairRange(memory);
            connectionManager.Plug(willow, isa, tree);
            repairer.RepairRange(memory);
            repairer.RepairRange(memory);
            connectionManager.Plug(palmtree, isa, tree);
            repairer.RepairRange(memory);
            repairer.RepairRange(memory);
            connectionManager.Plug(cactus, isa, plant);
            repairer.RepairRange(memory);
            repairer.RepairRange(memory);

            //1st real tests here

            selection = aiSqlWrapper.Select("isa plant", nameMapper, memory, repairer, true);

            if (selection.Count != 6)
                throw new Exception("Wrong selection count");
            if (!selection.Contains(pine))
                throw new Exception("Selection should contain element");
            if (!selection.Contains(willow))
                throw new Exception("Selection should contain element");
            if (!selection.Contains(tree))
                throw new Exception("Selection should contain element");
            if (!selection.Contains(cactus))
                throw new Exception("Selection should contain element");
            if (!selection.Contains(palmtree))
                throw new Exception("Selection should contain element");
            if (!selection.Contains(plant))
                throw new Exception("Selection should contain element");

            //2nd real test here

            selection = aiSqlWrapper.Select("  isa   plant and not (isa tree ) ", nameMapper, memory, repairer, true);

            if (selection.Count != 2)
                throw new Exception("Wrong selection count");
            if (!selection.Contains(plant))
                throw new Exception("Selection should contain element");
            if (!selection.Contains(cactus))
                throw new Exception("Selection should contain element");

            //3rd test here

            selection = aiSqlWrapper.Select("(isa plant or isa animal) and not isa tree", nameMapper, memory, repairer, true);

            if (selection.Count != 4)
                throw new Exception("Wrong selection count");

            if (!selection.Contains(plant))
                throw new Exception("Selection should contain element");
            if (!selection.Contains(cactus))
                throw new Exception("Selection should contain element");
            if (!selection.Contains(cat))
                throw new Exception("Selection should contain element");
            if (!selection.Contains(animal))
                throw new Exception("Selection should contain element");

            //4th test here

            selection = aiSqlWrapper.Select("isa plant and isa animal", nameMapper, memory, repairer, true);

            if (selection.Count != 0)
                throw new Exception("Wrong selection count");

            //5th test here

            selection = aiSqlWrapper.Select("isa lifeform and isa animal", nameMapper, memory, repairer, true);

            if (selection.Count != 2)
                throw new Exception("Wrong selection count");
            if (!selection.Contains(cat))
                throw new Exception("Selection should contain element");
            if (!selection.Contains(animal))
                throw new Exception("Selection should contain element");

            //6th test here

            selection = aiSqlWrapper.Select("(((not isa animal)))", nameMapper, memory, repairer, true);

            if (selection.Count != 11)
                throw new Exception("Wrong selection count");
            if (selection.Contains(cat))
                throw new Exception("Selection shouldn't contain element");
            if (selection.Contains(animal))
                throw new Exception("Selection shouldn't contain element");
        }
    }
}