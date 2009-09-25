using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestAnalogizer
    {
        public static void Test()
        {
            MetaConnectionManager metaConnectionManager  = new MetaConnectionManager();
            ConnectionManager connectionManager = new ConnectionManager();
            BrotherHoodManager brotherHoodManager = new BrotherHoodManager(metaConnectionManager);
            Analogizer analogizer = new Analogizer(metaConnectionManager,brotherHoodManager);
            Repairer repairer = new Repairer();
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept fruit = new Concept("fruit");
            Concept apple = new Concept("apple");
            Concept banana = new Concept("banana");

            Concept tree = new Concept("tree");
            Concept appletree = new Concept("appletree");
            Concept bananatree = new Concept("bananatree");

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            metaConnectionManager.AddMetaConnection(madeof, "madeof", isa);

            repairer.Repair(apple, fruit);
            connectionManager.Plug(apple, isa, fruit);
            repairer.Repair(banana, fruit);
            connectionManager.Plug(banana, isa, fruit);
            repairer.Repair(appletree, tree);
            connectionManager.Plug(appletree, isa, tree);
            repairer.Repair(bananatree, tree);
            connectionManager.Plug(bananatree, isa, tree);
            repairer.Repair(appletree, apple);
            connectionManager.Plug(appletree,madeof,apple);
            repairer.Repair(bananatree, banana);
            connectionManager.Plug(bananatree, madeof, banana);

            repairer.Repair(fruit, apple, banana, tree, appletree, bananatree);

            //Real test here

            Analogy analogy;
            analogy = analogizer.GetAnalogyOnSubjectVerbComplement(appletree, madeof, apple);

            if (analogy.OutputSubject != bananatree)
                throw new Exception("Wrong subject");

            if (analogy.OutputVerb != madeof)
                throw new Exception("Wrong verb");

            if (analogy.OutputComplement != banana)
                throw new Exception("Wrong complement");

            analogy = analogizer.GetAnalogyOnSubjectVerb(appletree, madeof);

            if (analogy.OutputSubject != bananatree)
                throw new Exception("Wrong subject");

            if (analogy.OutputVerb != madeof)
                throw new Exception("Wrong verb");

            if (analogy.OutputComplement != banana)
                throw new Exception("Wrong complement");

            analogy = analogizer.GetAnalogyOnSubject(appletree);

            if (analogy.OutputSubject != bananatree)
                throw new Exception("Wrong subject");

            if (analogy.OutputVerb != madeof)
                throw new Exception("Wrong verb");

            if (analogy.OutputComplement != banana)
                throw new Exception("Wrong complement");

            analogy = analogizer.GetBestRandomAnalogy(new List<Concept>() { apple,tree,appletree,banana,bananatree,fruit});

            if (analogy.OutputSubject != apple && analogy.OutputSubject != banana && analogy.OutputSubject != appletree && analogy.OutputSubject != bananatree)
                throw new Exception("Wrong subject");

            if (analogy.OutputVerb != madeof && analogy.OutputVerb != partof)
                throw new Exception("Wrong verb");

            if (analogy.OutputComplement != apple && analogy.OutputComplement != banana && analogy.OutputComplement != appletree && analogy.OutputComplement != bananatree)
                throw new Exception("Wrong complement");
        }
    }
}
