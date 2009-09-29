using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestMetaConnectionManager
    {
        public static void Test()
        {
            
            Concept isa = new Concept();
            Concept madeof = new Concept();

            #region Testing asymmetric metaConnection
            if (MetaConnectionManager.IsMetaConnected(isa, "muct", madeof))
                throw new Exception("Operators shouldn't be metaConnected");

            MetaConnectionManager.AddMetaConnection(isa, "muct", madeof);

            if (!MetaConnectionManager.IsMetaConnected(isa, "muct", madeof))
                throw new Exception("Operators should be metaConnected");

            MetaConnectionManager.RemoveMetaConnection(isa, "muct", madeof);

            if (MetaConnectionManager.IsMetaConnected(isa, "muct", madeof))
                throw new Exception("Operators shouldn't be metaConnected");

            MetaConnectionManager.RemoveMetaConnection(isa, "muct", madeof);

            MetaConnectionManager.AddMetaConnection(isa, "muct", madeof);
            MetaConnectionManager.AddMetaConnection(isa, "muct", madeof);

            if (!MetaConnectionManager.IsMetaConnected(isa, "muct", madeof))
                throw new Exception("Operators should be metaConnected");

            if (MetaConnectionManager.IsMetaConnected(madeof, "muct", isa))
                throw new Exception("Operators shouldn't be metaConnected");
            #endregion

            #region Testing symmetric metaConnection
            if (MetaConnectionManager.IsMetaConnected(isa, "permutable_side", isa))
                throw new Exception("Operators shouldn't be metaConnected");

            MetaConnectionManager.RemoveMetaConnection(isa, "permutable_side", isa);
            MetaConnectionManager.AddMetaConnection(isa, "permutable_side", isa);

            if (!MetaConnectionManager.IsMetaConnected(isa, "permutable_side", isa))
                throw new Exception("Operators should be metaConnected");

            MetaConnectionManager.RemoveMetaConnection(isa, "permutable_side", isa);

            if (MetaConnectionManager.IsMetaConnected(isa, "permutable_side", isa))
                throw new Exception("Operators shouldn't be metaConnected");

            if (MetaConnectionManager.IsMetaConnected(isa, "permutable_side", isa))
                throw new Exception("Operators shouldn't be metaConnected");

            #endregion
        }
    }
}
