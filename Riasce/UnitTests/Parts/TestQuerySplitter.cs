using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    class TestQuerySplitter
    {
        public static void Test()
        {
            TestNonSplittapleQuery();
            TestIsaPlantOrIsaAnimal();
            TestIsaPlantOrIsaAnimalWithExternalParantheses();
            TestIsaPlantOrIsaAnimalWithInternalParantheses();
            TestIsaPlantOrIsaAnimalWithExternalAndInternalParantheses();

            TestIsaTreeAndIsaPlant();
            TestIsaTreeAndIsaPlantWithInternalAndExternalParantheses();
            
            TestIsaPlantAndNotIsaTreeWithInternalAndExternalParantheses();

            TestPriority();
            TestPriorityWithParantheses();
        }

        private static void TestNonSplittapleQuery()
        {
            QuerySplitter querySplitter = new QuerySplitter();
            List<string> splittedQuery;
            string query;

            query = "isa plant";
            query = query.FixStringForHimmlStatementParsing();

            splittedQuery = querySplitter.TrySplit(query);
            if (splittedQuery != null)
                throw new Exception("Query shouldn't be splittable");
        }

        private static void TestIsaPlantOrIsaAnimal()
        {
            QuerySplitter querySplitter = new QuerySplitter();
            List<string> splittedQuery;
            string query;

            query = "isa plant or isa animal";
            query = query.FixStringForHimmlStatementParsing();
            splittedQuery = querySplitter.TrySplit(query);

            if (splittedQuery == null)
                throw new Exception("Query should be splittable");

            if (splittedQuery.Count != 3)
                throw new Exception("Query should be splitted in 3 chunks");

            if (!splittedQuery.Contains("isa plant"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("or"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("isa animal"))
                throw new Exception("Query should contain chunk");
        }

        private static void TestIsaPlantOrIsaAnimalWithExternalParantheses()
        {
            QuerySplitter querySplitter = new QuerySplitter();
            List<string> splittedQuery;
            string query;

            query = "(isa plant or isa animal)";
            query = query.FixStringForHimmlStatementParsing();
            splittedQuery = querySplitter.TrySplit(query);

            if (splittedQuery == null)
                throw new Exception("Query should be splittable");

            if (splittedQuery.Count != 3)
                throw new Exception("Query should be splitted in 3 chunks");

            if (!splittedQuery.Contains("isa plant"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("or"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("isa animal"))
                throw new Exception("Query should contain chunk");
        }

        private static void TestIsaPlantOrIsaAnimalWithInternalParantheses()
        {
            QuerySplitter querySplitter = new QuerySplitter();
            List<string> splittedQuery;
            string query;

            query = "(isa plant) or (isa animal)";
            query = query.FixStringForHimmlStatementParsing();
            splittedQuery = querySplitter.TrySplit(query);

            if (splittedQuery == null)
                throw new Exception("Query should be splittable");

            if (splittedQuery.Count != 3)
                throw new Exception("Query should be splitted in 3 chunks");

            if (!splittedQuery.Contains("isa plant"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("or"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("isa animal"))
                throw new Exception("Query should contain chunk");
        }

        private static void TestIsaPlantOrIsaAnimalWithExternalAndInternalParantheses()
        {
            QuerySplitter querySplitter = new QuerySplitter();
            List<string> splittedQuery;
            string query;

            query = "((isa plant) or (isa animal))";
            query = query.FixStringForHimmlStatementParsing();
            splittedQuery = querySplitter.TrySplit(query);

            if (splittedQuery == null)
                throw new Exception("Query should be splittable");

            if (splittedQuery.Count != 3)
                throw new Exception("Query should be splitted in 3 chunks");

            if (!splittedQuery.Contains("isa plant"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("or"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("isa animal"))
                throw new Exception("Query should contain chunk");
        }

        private static void TestIsaTreeAndIsaPlant()
        {
            QuerySplitter querySplitter = new QuerySplitter();
            List<string> splittedQuery;
            string query;

            query = "isa tree and isa plant";
            query = query.FixStringForHimmlStatementParsing();
            splittedQuery = querySplitter.TrySplit(query);

            if (splittedQuery == null)
                throw new Exception("Query should be splittable");

            if (splittedQuery.Count != 3)
                throw new Exception("Query should be splitted in 3 chunks");

            if (!splittedQuery.Contains("isa tree"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("and"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("isa plant"))
                throw new Exception("Query should contain chunk");
        }

        private static void TestIsaTreeAndIsaPlantWithInternalAndExternalParantheses()
        {
            QuerySplitter querySplitter = new QuerySplitter();
            List<string> splittedQuery;
            string query;

            query = "((isa tree) and (isa plant))";
            query = query.FixStringForHimmlStatementParsing();
            splittedQuery = querySplitter.TrySplit(query);

            if (splittedQuery == null)
                throw new Exception("Query should be splittable");

            if (splittedQuery.Count != 3)
                throw new Exception("Query should be splitted in 3 chunks");

            if (!splittedQuery.Contains("isa tree"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("and"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("isa plant"))
                throw new Exception("Query should contain chunk");
        }

        private static void TestIsaPlantAndNotIsaTreeWithInternalAndExternalParantheses()
        {
            QuerySplitter querySplitter = new QuerySplitter();
            List<string> splittedQuery;
            string query;

            query = "((isa plant) and not (isa tree))";
            query = query.FixStringForHimmlStatementParsing();
            splittedQuery = querySplitter.TrySplit(query);

            if (splittedQuery == null)
                throw new Exception("Query should be splittable");

            if (splittedQuery.Count != 3)
                throw new Exception("Query should be splitted in 3 chunks");

            if (!splittedQuery.Contains("isa plant"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("and not"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("isa tree"))
                throw new Exception("Query should contain chunk");
        }

        private static void TestPriority()
        {
            QuerySplitter querySplitter = new QuerySplitter();
            List<string> splittedQuery;
            string query;

            query = "isa tree and isa plant or isa animal and not isa human";
            query = query.FixStringForHimmlStatementParsing();
            splittedQuery = querySplitter.TrySplit(query);

            if (splittedQuery == null)
                throw new Exception("Query should be splittable");

            if (splittedQuery.Count != 3)
                throw new Exception("Query should be splitted in 3 chunks");

            if (!splittedQuery.Contains("isa tree and isa plant"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("or"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("isa animal and not isa human"))
                throw new Exception("Query should contain chunk");
        }

        private static void TestPriorityWithParantheses()
        {
            QuerySplitter querySplitter = new QuerySplitter();
            List<string> splittedQuery;
            string query;

            query = "(isa tree) and (isa plant or isa animal and not isa human)";
            query = query.FixStringForHimmlStatementParsing();
            splittedQuery = querySplitter.TrySplit(query);

            if (splittedQuery == null)
                throw new Exception("Query should be splittable");

            if (splittedQuery.Count != 3)
                throw new Exception("Query should be splitted in 3 chunks");

            if (!splittedQuery.Contains("isa tree"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("and"))
                throw new Exception("Query should contain chunk");

            if (!splittedQuery.Contains("isa plant or isa animal and not isa human"))
                throw new Exception("Query should contain chunk");
        }
    }
}
