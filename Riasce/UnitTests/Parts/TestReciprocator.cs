﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestReciprocator
    {
        public static void Test()
        {
            TestPostConnectionInverseOf();
        }

        private static void TestPostConnectionInverseOf()
        {
            ConnectionManager connectionManager = new ConnectionManager();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Repairer repairer = new Repairer();
            Memory.TotalVerbList = new HashSet<Concept>();

            
            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept plant = new Concept("plant");
            Concept water = new Concept("water");
            Concept liquid = new Concept("liquid");

            repairer.Repair(pine, tree);

            connectionManager.Plug(pine, isa, tree);

            repairer.Repair(tree, plant);

            connectionManager.Plug(tree, isa, plant);

            repairer.Repair(plant, water);

            connectionManager.Plug(plant, madeof, water);

            repairer.Repair(water, liquid);

            connectionManager.Plug(water, isa, liquid);

            repairer.Repair(pine, tree, plant, water, liquid);

            metaConnectionManager.AddMetaConnection(isa, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            metaConnectionManager.AddMetaConnection(madeof, "muct", madeof);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);

            repairer.Repair(pine, tree, plant, water, liquid);

            repairer.Reciprocate(pine);
            repairer.Reciprocate(tree);
            repairer.Reciprocate(plant);
            repairer.Reciprocate(water);
            repairer.Reciprocate(liquid);

            repairer.Repair(pine, tree, plant, water, liquid);

            //Pre-conditions

            if (!connectionManager.TestConnection(plant, madeof, water))
                throw new Exception("Should be connected because it's explicit");

            //Real test

            if (!connectionManager.TestConnection(tree, madeof, water))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(pine, madeof, water))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(pine, madeof, liquid))
                throw new Exception("Should be connected because it's implicit");

            if (!connectionManager.TestConnection(liquid, partof, pine))
                throw new Exception("Should be connected because it's implicit");

            /*pine isa tree
            tree isa plant
            plant madeof water
            water isa liquid
            isa muct isa
            madeof muct isa
            madeof liffid isa
            madeof muct madeof
            pine madeof liquid? Yes
            liquid partof pine? no
            madeof inverse_of partof
            liquid partof pine? ---------> Inconsistency
            pine madeof liquid? ---------> Inconsistency
            (solution may be related to the lack of isa inverse_of someare)*/
        }
    }
}