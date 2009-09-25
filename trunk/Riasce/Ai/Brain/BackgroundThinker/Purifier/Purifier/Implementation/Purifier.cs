using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class Purifier : AbstractPurifier
    {
        #region Fields
        private Repairer repairer;

        private ConnectionManager connectionManager;

        private OptimizedPurifier optimizedPurifier;

        private FlatPurifier flatPurifier;
        #endregion

        #region Constructor
        public Purifier(Repairer repairer, ConnectionManager connectionManager)
        {
            this.repairer = repairer;
            this.connectionManager = connectionManager;
            flatPurifier = new FlatPurifier(this.connectionManager,repairer);
            optimizedPurifier = new OptimizedPurifier(this.connectionManager,repairer);
        }
        #endregion

        #region Public Methods
        public override Trauma PurifyOptimized(Concept concept)
        {
            return optimizedPurifier.Purify(concept);
        }

        public override Trauma PurifyFlat(Concept concept)
        {
            return flatPurifier.Purify(concept);
        }

        public override Trauma PurifyRangeOptimized(IEnumerable<Concept> conceptCollection)
        {
            return optimizedPurifier.PurifiyRange(conceptCollection);
        }
        #endregion
    }
}
