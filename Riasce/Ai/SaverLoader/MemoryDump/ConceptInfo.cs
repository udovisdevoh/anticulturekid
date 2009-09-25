using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    [Serializable]
    public class ConceptInfo
    {
        #region Fields
        private HashSet<string> nameList = new HashSet<string>();

        private Dictionary<string, HashSet<int>> positiveMetaConnectionList = new Dictionary<string, HashSet<int>>();

        private Dictionary<string, HashSet<int>> negativeMetaConnectionList = new Dictionary<string, HashSet<int>>();

        private Dictionary<int, HashSet<int>> connectionInfo = new Dictionary<int, HashSet<int>>();

        private Dictionary<int, HashSet<ConditionInfo>> implyConditionInfoListPositive = new Dictionary<int, HashSet<ConditionInfo>>();

        private Dictionary<int, HashSet<ConditionInfo>> implyConditionInfoListNegative = new Dictionary<int, HashSet<ConditionInfo>>();

        private int id;
        #endregion

        #region Methods
        public void AddNameRange(List<string> nameList)
        {
            this.nameList.UnionWith(nameList);
        }

        public void AddConnectionInfo(int verbId, int complementId)
        {
            HashSet<int> complementIdList;
            if (!connectionInfo.TryGetValue(verbId, out complementIdList))
            {
                complementIdList = new HashSet<int>();
                connectionInfo.Add(verbId, complementIdList);
            }
            complementIdList.Add(complementId);
        }

        public void AddMetaConnectionInfo(string metaOperatorName, int farVerbId, bool connectionSide)
        {
            Dictionary<string, HashSet<int>> trunk;
            if (connectionSide)
                trunk = positiveMetaConnectionList;
            else
                trunk = negativeMetaConnectionList;

            HashSet<int> farVerbList;

            if (!trunk.TryGetValue(metaOperatorName, out farVerbList))
            {
                farVerbList = new HashSet<int>();
                trunk.Add(metaOperatorName, farVerbList);
            }

            farVerbList.Add(farVerbId);
        }

        public void AddImplyConditionInfo(int complementId, ConditionInfo conditionInfo, bool isPositive)
        {
            Dictionary<int, HashSet<ConditionInfo>> conditionInfoList;

            if (isPositive)
                conditionInfoList = implyConditionInfoListPositive;
            else
                conditionInfoList = implyConditionInfoListNegative;

            HashSet<ConditionInfo> conditionList;

            if (!conditionInfoList.TryGetValue(complementId, out conditionList))
            {
                conditionList = new HashSet<ConditionInfo>();
                conditionInfoList.Add(complementId,conditionList);
            }

            conditionList.Add(conditionInfo);
        }
        #endregion

        #region Properties
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public HashSet<string> NameList
        {
            get { return nameList; }
        }

        public Dictionary<int, HashSet<int>> ConnectionInfo
        {
            get { return connectionInfo; }
        }

        public Dictionary<string, HashSet<int>> PositiveMetaConnectionList
        {
            get { return positiveMetaConnectionList; }
        }

        public Dictionary<string, HashSet<int>> NegativeMetaConnectionList
        {
            get { return negativeMetaConnectionList; }
        }

        public Dictionary<int, HashSet<ConditionInfo>> ImplyConditionInfoListPositive
        {
            get { return implyConditionInfoListPositive; }
        }

        public Dictionary<int, HashSet<ConditionInfo>> ImplyConditionInfoListNegative
        {
            get { return implyConditionInfoListNegative; }
        }
        #endregion
    }
}
