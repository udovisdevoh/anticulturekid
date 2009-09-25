using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public class MetaConnectionTree : AbstractMetaConnectionTree
    {
        #region Fields
        private Dictionary<string, HashSet<Concept>> metaConnections = new Dictionary<string, HashSet<Concept>>();
        #endregion

        #region Methods
        public override void AddMetaConnection(string metaOperatorName, Concept concept)
        {
            HashSet<Concept> connectionSubSet;
            if (!metaConnections.TryGetValue(metaOperatorName, out connectionSubSet))
            {
                connectionSubSet = new HashSet<Concept>();
                metaConnections.Add(metaOperatorName,connectionSubSet);
            }
            connectionSubSet.Add(concept);
        }

        public override void RemoveMetaConnection(string metaOperatorName, Concept concept)
        {
            HashSet<Concept> connectionSubSet;
            if (metaConnections.TryGetValue(metaOperatorName, out connectionSubSet))
                connectionSubSet.Remove(concept);
        }

        public override bool IsMetaConnectedTo(string metaOperatorName, Concept concept)
        {
            HashSet<Concept> connectionSubSet;
            if (metaConnections.TryGetValue(metaOperatorName, out connectionSubSet))
                if (connectionSubSet.Contains(concept))
                    return true;
            return false;
        }

        public override HashSet<Concept> GetAffectedOperatorsByMetaConnection(string metaOperatorName)
        {
            HashSet<Concept> affectedOperators;

            if (metaConnections.TryGetValue(metaOperatorName, out affectedOperators))
                return affectedOperators;
            else
                return new HashSet<Concept>();
        }
        #endregion

        #region Properties
        public bool IsEmpty
        {
            get
            {
                if (metaConnections.Count > 0)
                    return false;
                else
                    return true;
            }
        }

        public bool IsConnectedToSomething
        {
            get
            {
                foreach (HashSet<Concept> verbList in metaConnections.Values)
                    if (verbList.Count > 0)
                        return true;
                return false;
            }
        }
        #endregion
    }
}