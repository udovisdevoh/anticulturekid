﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestDisambiguationNamer
    {
        public static void Test()
        {
            ConnectionManager connectionManager  = new ConnectionManager();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            BrotherHoodManager brotherHoodManger = new BrotherHoodManager(metaConnectionManager);
            Repairer repairer = new Repairer();
            DisambiguationNamer disambiguationNamer = new DisambiguationNamer(brotherHoodManger,repairer);
            

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept plant = new Concept("plant");

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(isa, "muct", isa);

            connectionManager.Plug(pine, isa, tree);
            repairer.Repair(pine, tree);
            connectionManager.Plug(tree, isa, plant);
            repairer.Repair(pine, tree, plant);

            //pre-conditions

            if (!connectionManager.TestConnection(pine, isa, plant))
                throw new Exception("Should be connected because it's implicit");

            //real test begins here

            if (disambiguationNamer.GetContextConcept(pine) != tree)
                throw new Exception("Wrong context concept");

            if (disambiguationNamer.GetContextConcept(pine) != tree) //Testing with cache
                throw new Exception("Wrong context concept");
        }
    }
}