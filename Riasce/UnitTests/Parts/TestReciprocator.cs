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

            Repairer.Repair(pine, tree);

            ConnectionManager.Plug(pine, isa, tree);

            Repairer.Repair(tree, plant);

            ConnectionManager.Plug(tree, isa, plant);

            Repairer.Repair(plant, water);

            ConnectionManager.Plug(plant, madeof, water);

            Repairer.Repair(water, liquid);

            ConnectionManager.Plug(water, isa, liquid);

            Repairer.Repair(pine, tree, plant, water, liquid);

            MetaConnectionManager.AddMetaConnection(isa, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);

            Repairer.Repair(pine, tree, plant, water, liquid);

            Repairer.Reciprocate(pine);
            Repairer.Reciprocate(tree);
            Repairer.Reciprocate(plant);
            Repairer.Reciprocate(water);
            Repairer.Reciprocate(liquid);

            Repairer.Repair(pine, tree, plant, water, liquid);

            //Pre-conditions

            if (!ConnectionManager.TestConnection(plant, madeof, water))
                throw new Exception("Should be connected because it's explicit");

            //Real test

            if (!ConnectionManager.TestConnection(tree, madeof, water))
                throw new Exception("Should be connected because it's implicit");

            if (!ConnectionManager.TestConnection(pine, madeof, water))
                throw new Exception("Should be connected because it's implicit");

            if (!ConnectionManager.TestConnection(pine, madeof, liquid))
                throw new Exception("Should be connected because it's implicit");

            if (!ConnectionManager.TestConnection(liquid, partof, pine))
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
