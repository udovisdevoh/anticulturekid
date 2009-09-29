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
            Analogizer analogizer = new Analogizer();
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

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "madeof", isa);

            Repairer.Repair(apple, fruit);
            ConnectionManager.Plug(apple, isa, fruit);
            Repairer.Repair(banana, fruit);
            ConnectionManager.Plug(banana, isa, fruit);
            Repairer.Repair(appletree, tree);
            ConnectionManager.Plug(appletree, isa, tree);
            Repairer.Repair(bananatree, tree);
            ConnectionManager.Plug(bananatree, isa, tree);
            Repairer.Repair(appletree, apple);
            ConnectionManager.Plug(appletree,madeof,apple);
            Repairer.Repair(bananatree, banana);
            ConnectionManager.Plug(bananatree, madeof, banana);

            Repairer.Repair(fruit, apple, banana, tree, appletree, bananatree);

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
