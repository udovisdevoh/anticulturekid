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

            ConnectionManager.Plug(tree, isa, plant);

            if (!tree.IsFlatDirty)
                throw new Exception("Concept should be dirty");

            if (!plant.IsFlatDirty)
                throw new Exception("Concept should be dirty");

            tree.IsFlatDirty = false;
            plant.IsFlatDirty = false;

            ConnectionManager.Plug(animal, contradict, plant);
            animal.IsFlatDirty = false;

            tree.AddFlatConnection(isa, plant);

            ConnectionManager.Plug(pine, isa, tree);

            if (!plant.IsFlatDirty)
                throw new Exception("Concept should be dirty");

            if (animal.IsFlatDirty)
                throw new Exception("Concept shouldn't be dirty");

            pine.IsFlatDirty = false;
            plant.IsFlatDirty = false;

            if (ConnectionManager.TestConnection(pine, isa, plant))
                throw new Exception("Connection shouldn't exist");

            pine.AddFlatConnection(isa, plant);
            plant.AddFlatConnection(someare, pine);
            if (!ConnectionManager.TestConnection(pine, isa, plant))
                throw new Exception("Connection should exist");

            ConnectionManager.Plug(cat, isa, animal);
            cat.IsFlatDirty = false;
            animal.IsFlatDirty = false;

            if (!cat.IsOptimizedConnectedTo(isa, animal))
                throw new Exception("Connection should exist");

            if (!animal.IsOptimizedConnectedTo(someare, cat))
                throw new Exception("Connection should exist");

            ConnectionManager.UnPlug(cat, isa, animal);

            if (cat.IsOptimizedConnectedTo(isa, animal))
                throw new Exception("Connection shouldn't exist");

            if (animal.IsOptimizedConnectedTo(someare, cat))
                throw new Exception("Connection shouldn't exist");

            #endregion

            #region Testing finding obstruction
            Proposition obstruction;
            animal.IsFlatDirty = false;
            plant.IsFlatDirty = false;
            cat.IsFlatDirty = false;

            animal.AddFlatConnection(contradict, plant);
            plant.AddFlatConnection(contradict, animal);

            cat.AddFlatConnection(isa, animal);
            animal.AddFlatConnection(someare, cat);

            cat.AddFlatConnection(contradict, plant);
            plant.AddFlatConnection(contradict, cat);

            ConnectionManager.TestConnection(cat, contradict, plant);


            animal.IsFlatDirty = false;
            plant.IsFlatDirty = false;
            cat.IsFlatDirty = false;

            obstruction = ConnectionManager.FindObstructionToPlug(cat, isa, plant, true);

            if (obstruction == null)
                throw new Exception("Obstruction should be found");

            if (obstruction.Subject != cat && obstruction.Verb != contradict && obstruction.Complement != plant)
                throw new Exception("Wrong obstruction");

            obstruction = ConnectionManager.FindObstructionToPlug(animal, isa, cat, true);

            if (obstruction == null)
                throw new Exception("Obstruction should be found");

            if (obstruction.Subject != animal && obstruction.Verb != someare && obstruction.Complement != cat)
                throw new Exception("Wrong obstruction");

            #endregion
        }
    }
}
