using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestConcept
    {
        public static void Test()
        {
            Concept pine = new Concept();
            Concept isa = new Concept();
            Concept tree = new Concept();

            #region Testing flat connections/deconnection and connection testing
            if (pine.IsFlatConnectedTo(isa, tree))
                throw new Exception("Flat connection shouldn't exist");

            pine.AddFlatConnection(isa, tree);

            if (!pine.IsFlatConnectedTo(isa, tree))
                throw new Exception("Flat connection should exist");

            pine.AddFlatConnection(isa, tree);

            if (!pine.IsFlatConnectedTo(isa, tree))
                throw new Exception("Flat connection should exist");

            pine.AddFlatConnection(isa, tree);
            pine.RemoveFlatConnection(isa, tree);

            if (pine.IsFlatConnectedTo(isa, tree))
                throw new Exception("Flat connection shouldn't exist");

            pine.RemoveFlatConnection(isa, tree);
            pine.RemoveFlatConnection(isa, tree);

            if (pine.IsFlatConnectedTo(isa, tree))
                throw new Exception("Flat connection shouldn't exist");
            #endregion

            #region Testing optimized connection/deconnection and connection testing
            pine.AddFlatConnection(isa, tree);
            if (pine.IsOptimizedConnectedTo(isa, tree))
                throw new Exception("Optimized connection shouldn't exist");

            pine.AddOptimizedConnection(isa, tree);
            RepairedFlatBranchCache.Clear();

            if (!pine.IsOptimizedConnectedTo(isa, tree))
                throw new Exception("Optimized connection should exist");

            pine.AddOptimizedConnection(isa, tree);
            RepairedFlatBranchCache.Clear();

            if (!pine.IsOptimizedConnectedTo(isa, tree))
                throw new Exception("Optimized connection should exist");

            pine.AddOptimizedConnection(isa, tree);
            RepairedFlatBranchCache.Clear();
            pine.RemoveOptimizedConnection(isa, tree);
            RepairedFlatBranchCache.Clear();

            if (pine.IsOptimizedConnectedTo(isa, tree))
                throw new Exception("Optimized connection shouldn't exist");

            pine.RemoveOptimizedConnection(isa, tree);
            RepairedFlatBranchCache.Clear();
            pine.RemoveOptimizedConnection(isa, tree);
            RepairedFlatBranchCache.Clear();

            if (pine.IsOptimizedConnectedTo(isa, tree))
                throw new Exception("Optimized connection shouldn't exist");
            #endregion

            #region Testing metaOperation connection, deconnection and connection testing
            Concept madeof = new Concept();
            #region Testing positive sens
            if (isa.IsMetaConnectedTo("dummyMetaOperator", isa, true))
                throw new Exception("MetaConnection shouldn't exist");

            isa.AddMetaConnection("dummyMetaOperator", isa, true);

            if (!isa.IsMetaConnectedTo("dummyMetaOperator", isa, true))
                throw new Exception("MetaConnection should exist");

            isa.AddMetaConnection("dummyMetaOperator", isa, true);
            isa.AddMetaConnection("dummyMetaOperator", isa, true);

            if (!isa.IsMetaConnectedTo("dummyMetaOperator", isa, true))
                throw new Exception("MetaConnection should exist");

            isa.RemoveMetaConnection("dummyMetaOperator", isa, true);

            if (isa.IsMetaConnectedTo("dummyMetaOperator", isa, true))
                throw new Exception("MetaConnection shouldn't exist");

            isa.RemoveMetaConnection("dummyMetaOperator", isa, true);
            isa.RemoveMetaConnection("dummyMetaOperator", isa, true);
            isa.AddMetaConnection("dummyMetaOperator", isa, true);
            #endregion

            #region Testing negative sens
            if (isa.IsMetaConnectedTo("dummyMetaOperator", isa, false))
                throw new Exception("MetaConnection shouldn't exist");

            isa.AddMetaConnection("dummyMetaOperator", isa, false);

            if (!isa.IsMetaConnectedTo("dummyMetaOperator", isa, false))
                throw new Exception("MetaConnection should exist");

            isa.AddMetaConnection("dummyMetaOperator", isa, false);
            isa.AddMetaConnection("dummyMetaOperator", isa, false);

            if (!isa.IsMetaConnectedTo("dummyMetaOperator", isa, false))
                throw new Exception("MetaConnection should exist");

            isa.RemoveMetaConnection("dummyMetaOperator", isa, false);

            if (isa.IsMetaConnectedTo("dummyMetaOperator", isa, false))
                throw new Exception("MetaConnection shouldn't exist");

            isa.RemoveMetaConnection("dummyMetaOperator", isa, false);
            isa.RemoveMetaConnection("dummyMetaOperator", isa, false);
            #endregion
            #endregion
        }
    }
}
