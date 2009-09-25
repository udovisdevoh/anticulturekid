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
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            Concept isa = new Concept();
            Concept madeof = new Concept();

            #region Testing asymmetric metaConnection
            if (metaConnectionManager.IsMetaConnected(isa, "muct", madeof))
                throw new Exception("Operators shouldn't be metaConnected");

            metaConnectionManager.AddMetaConnection(isa, "muct", madeof);

            if (!metaConnectionManager.IsMetaConnected(isa, "muct", madeof))
                throw new Exception("Operators should be metaConnected");

            metaConnectionManager.RemoveMetaConnection(isa, "muct", madeof);

            if (metaConnectionManager.IsMetaConnected(isa, "muct", madeof))
                throw new Exception("Operators shouldn't be metaConnected");

            metaConnectionManager.RemoveMetaConnection(isa, "muct", madeof);

            metaConnectionManager.AddMetaConnection(isa, "muct", madeof);
            metaConnectionManager.AddMetaConnection(isa, "muct", madeof);

            if (!metaConnectionManager.IsMetaConnected(isa, "muct", madeof))
                throw new Exception("Operators should be metaConnected");

            if (metaConnectionManager.IsMetaConnected(madeof, "muct", isa))
                throw new Exception("Operators shouldn't be metaConnected");
            #endregion

            #region Testing symmetric metaConnection
            if (metaConnectionManager.IsMetaConnected(isa, "permutable_side", isa))
                throw new Exception("Operators shouldn't be metaConnected");

            metaConnectionManager.RemoveMetaConnection(isa, "permutable_side", isa);
            metaConnectionManager.AddMetaConnection(isa, "permutable_side", isa);

            if (!metaConnectionManager.IsMetaConnected(isa, "permutable_side", isa))
                throw new Exception("Operators should be metaConnected");

            metaConnectionManager.RemoveMetaConnection(isa, "permutable_side", isa);

            if (metaConnectionManager.IsMetaConnected(isa, "permutable_side", isa))
                throw new Exception("Operators shouldn't be metaConnected");

            if (metaConnectionManager.IsMetaConnected(isa, "permutable_side", isa))
                throw new Exception("Operators shouldn't be metaConnected");

            #endregion
        }
    }
}
