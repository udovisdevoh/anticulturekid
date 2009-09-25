using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestConnectionManager
    {
        public static void Test()
        {
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            ConnectionManager connectionManager = new ConnectionManager();
            #region We create the verbs that we will need
            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept contradict = new Concept("contradict");
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
            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(isa, "muct", isa);
            metaConnectionManager.AddMetaConnection(isa, "cant", someare);
            metaConnectionManager.AddMetaConnection(isa, "cant", madeof);
            metaConnectionManager.AddMetaConnection(isa, "cant", partof);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(madeof, "muct", madeof);
            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            metaConnectionManager.AddMetaConnection(madeof, "unlikely", isa);
            metaConnectionManager.AddMetaConnection(madeof, "cant", partof);
            metaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);
            metaConnectionManager.AddMetaConnection(contradict, "muct", isa);
            metaConnectionManager.AddMetaConnection(contradict, "cant", isa);
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
            #endregion

            #region Testing for plugging, unplugging and dirthening of concepts
            if (tree.IsFlatDirty)
                throw new Exception("Concept shouldn't be dirty");

            connectionManager.Plug(tree, isa, plant);

            if (!tree.IsFlatDirty)
                throw new Exception("Concept should be dirty");

            if (!plant.IsFlatDirty)
                throw new Exception("Concept should be dirty");

            tree.IsFlatDirty = false;
            plant.IsFlatDirty = false;

            connectionManager.Plug(animal, contradict, plant);
            animal.IsFlatDirty = false;

            tree.AddFlatConnection(isa, plant);

            connectionManager.Plug(pine, isa, tree);

            if (!plant.IsFlatDirty)
                throw new Exception("Concept should be dirty");

            if (animal.IsFlatDirty)
                throw new Exception("Concept shouldn't be dirty");

            pine.IsFlatDirty = false;
            plant.IsFlatDirty = false;

            if (connectionManager.TestConnection(pine, isa, plant))
                throw new Exception("Connection shouldn't exist");

            pine.AddFlatConnection(isa, plant);
            plant.AddFlatConnection(someare, pine);
            if (!connectionManager.TestConnection(pine, isa, plant))
                throw new Exception("Connection should exist");

            connectionManager.Plug(cat, isa, animal);
            cat.IsFlatDirty = false;
            animal.IsFlatDirty = false;

            if (!cat.IsOptimizedConnectedTo(isa, animal))
                throw new Exception("Connection should exist");

            if (!animal.IsOptimizedConnectedTo(someare, cat))
                throw new Exception("Connection should exist");

            connectionManager.UnPlug(cat, isa, animal);

            if (cat.IsOptimizedConnectedTo(isa, animal))
                throw new Exception("Connection shouldn't exist");

            if (animal.IsOptimizedConnectedTo(someare, cat))
                throw new Exception("Connection shouldn't exist");

            #endregion

            #region Testing finding obstruction
            List<Concept> obstruction;
            animal.IsFlatDirty = false;
            plant.IsFlatDirty = false;
            cat.IsFlatDirty = false;

            animal.AddFlatConnection(contradict, plant);
            plant.AddFlatConnection(contradict, animal);

            cat.AddFlatConnection(isa, animal);
            animal.AddFlatConnection(someare, cat);

            cat.AddFlatConnection(contradict, plant);
            plant.AddFlatConnection(contradict, cat);

            connectionManager.TestConnection(cat, contradict, plant);


            animal.IsFlatDirty = false;
            plant.IsFlatDirty = false;
            cat.IsFlatDirty = false;

            obstruction = connectionManager.FindObstructionToPlug(cat, isa, plant, true);

            if (obstruction == null)
                throw new Exception("Obstruction should be found");

            if (obstruction[0] != cat && obstruction[1] != contradict && obstruction[2] != plant)
                throw new Exception("Wrong obstruction");

            obstruction = connectionManager.FindObstructionToPlug(animal, isa, cat, true);

            if (obstruction == null)
                throw new Exception("Obstruction should be found");

            if (obstruction[0] != animal && obstruction[1] != someare && obstruction[2] != cat)
                throw new Exception("Wrong obstruction");

            #endregion
        }
    }
}
