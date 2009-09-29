using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestDisambiguationNamer
    {
        public static void Test()
        {
            DisambiguationNamer disambiguationNamer = new DisambiguationNamer();

            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept plant = new Concept("plant");

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(isa, "muct", isa);

            ConnectionManager.Plug(pine, isa, tree);
            Repairer.Repair(pine, tree);
            ConnectionManager.Plug(tree, isa, plant);
            Repairer.Repair(pine, tree, plant);

            //pre-conditions

            if (!ConnectionManager.TestConnection(pine, isa, plant))
                throw new Exception("Should be connected because it's implicit");

            //real test begins here

            if (disambiguationNamer.GetContextConcept(pine) != tree)
                throw new Exception("Wrong context concept");

            if (disambiguationNamer.GetContextConcept(pine) != tree) //Testing with cache
                throw new Exception("Wrong context concept");
        }
    }
}
