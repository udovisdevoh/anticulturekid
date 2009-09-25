using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestBrain
    {
        public static void Test()
        {
            TestBrainMuct();
            TestBrainMuctWithInverseOf();
            TestBrainInverseOfWithMuct();
            TestBrainPostConnectionMetaConnectionMuct();
            TestBrainPostConnectionMetaConnectionInverseOfTestReciprocatorToo();
        }

        private static void TestBrainMuct()
        {
            Brain brain = new Brain();

            int isa = 1;

            int pine = 3;
            int tree = 4;
            int plant = 5;

            if (brain.TestConnection(pine, isa, plant))
                throw new Exception("Connection shouldn't exist yet");


            brain.TryAddMetaConnection(isa, "muct", isa); //if pine isa tree and tree isa plant then pine isa plant
            brain.TryAddConnection(tree, isa, plant);
            brain.TryAddConnection(pine, isa, tree);

            //Pre-conditions

            if (!brain.TestConnection(tree, isa, plant))
                throw new Exception("Connection should exist because it's explicit");

            if (!brain.TestConnection(pine, isa, tree))
                throw new Exception("Connection should exist because it's explicit");

            //Real test

            if (!brain.TestConnection(pine, isa, plant))
                throw new Exception("Connection should exist because it's implicit");
        }

        private static void TestBrainMuctWithInverseOf()
        {
            Brain brain = new Brain();

            int isa = 1;
            int someare = 2;

            int pine = 3;
            int tree = 4;
            int plant = 5;

            if (brain.TestConnection(pine, isa, plant))
                throw new Exception("Connection shouldn't exist yet");

            brain.TryAddMetaConnection(isa, "muct", isa); //if pine isa tree and tree isa plant then pine isa plant
            brain.TryAddMetaConnection(isa, "inverse_of", someare);
            brain.TryAddConnection(tree, isa, plant);
            brain.TryAddConnection(pine, isa, tree);

            //Pre-conditions

            if (!brain.TestConnection(tree, isa, plant))
                throw new Exception("Connection should exist because it's explicit");

            if (!brain.TestConnection(pine, isa, tree))
                throw new Exception("Connection should exist because it's explicit");

            if (!brain.IsMetaConnected(isa, "inverse_of", someare))
                throw new Exception("MetaConnection should exist because it's explicit");

            if (!brain.IsMetaConnected(isa, "muct", isa))
                throw new Exception("MetaConnection should exist because it's explicit");

            //Real test

            if (!brain.TestConnection(pine, isa, plant))
                throw new Exception("Connection should exist because it's implicit");

            if (!brain.TestConnection(plant, someare, pine))
                throw new Exception("Connection should exist because it's implicit");
        }

        private static void TestBrainInverseOfWithMuct()
        {
            Brain brain = new Brain();

            int isa = 1;
            int someare = 2;

            int pine = 3;
            int tree = 4;
            int plant = 5;

            if (brain.TestConnection(pine, isa, plant))
                throw new Exception("Connection shouldn't exist yet");

            brain.TryAddMetaConnection(isa, "inverse_of", someare);
            brain.TryAddMetaConnection(isa, "muct", isa); //if pine isa tree and tree isa plant then pine isa plant
            brain.TryAddConnection(tree, isa, plant);
            brain.TryAddConnection(pine, isa, tree);

            //Pre-conditions

            if (!brain.IsMetaConnected(isa, "inverse_of", someare))
                throw new Exception("MetaConnection should exist because it's explicit");

            if (!brain.IsMetaConnected(isa, "muct", isa))
                throw new Exception("MetaConnection should exist because it's explicit");

            if (!brain.IsMetaConnected(someare, "liffid", someare))
                throw new Exception("MetaConnection should exist because it's implicit");

            if (!brain.TestConnection(tree, isa, plant))
                throw new Exception("Connection should exist because it's explicit");

            if (!brain.TestConnection(plant, someare, tree))
                throw new Exception("Connection should exist because it's explicit");

            if (!brain.TestConnection(pine, isa, tree))
                throw new Exception("Connection should exist because it's explicit");

            if (!brain.TestConnection(tree, someare, pine))
                throw new Exception("Connection should exist because it's explicit");

            //Real test

            if (!brain.TestConnection(pine, isa, plant))
                throw new Exception("Connection should exist because it's implicit");

            if (!brain.TestConnection(plant, someare, pine))
                throw new Exception("Connection should exist because it's implicit");
        }

        private static void TestBrainPostConnectionMetaConnectionMuct()
        {
            Brain brain = new Brain();
            Memory.TotalVerbList = new HashSet<Concept>();

            int isa = 1;
            //int someare = 2;

            int pine = 3;
            int tree = 4;
            int plant = 5;

            brain.TryAddConnection(tree, isa, plant);
            brain.TryAddConnection(pine, isa, tree);

            if (brain.TestConnection(pine, isa, plant))
                throw new Exception("Connection shouldn't exist yet");

            brain.TryAddMetaConnection(isa, "muct", isa); //if pine isa tree and tree isa plant then pine isa plant

            //Pre-conditions

            if (!brain.TestConnection(tree, isa, plant))
                throw new Exception("Connection should exist because it's explicit");

            if (!brain.TestConnection(pine, isa, tree))
                throw new Exception("Connection should exist because it's explicit");

            if (!brain.IsMetaConnected(isa, "muct", isa))
                throw new Exception("MetaConnection should exist because it's explicit");

            //Real test

            if (!brain.TestConnection(pine, isa, plant))
                throw new Exception("Connection should exist because it's implicit");
        }

        private static void TestBrainPostConnectionMetaConnectionInverseOfTestReciprocatorToo()
        {
            Brain brain = new Brain();

            int isa = 1;
            int someare = 2;

            int pine = 3;
            int tree = 4;

            brain.TryAddConnection(pine, isa, tree);
            brain.TryAddMetaConnection(isa, "inverse_of", someare);

            if (!brain.TestConnection(tree, someare, pine))
                throw new Exception("Connection should exist because it's implicit");
        }
    }
}
