using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents information that can be saved about concept
    /// </summary>
    [Serializable]
    public class ConceptInfo
    {
        #region Fields
        /// <summary>
        /// Name list for concept
        /// </summary>
        private HashSet<string> nameList = new HashSet<string>();

        /// <summary>
        /// Positive metaConnection list
        /// </summary>
        private Dictionary<string, HashSet<int>> positiveMetaConnectionList = new Dictionary<string, HashSet<int>>();

        /// <summary>
        /// Negative metaConnection list
        /// </summary>
        private Dictionary<string, HashSet<int>> negativeMetaConnectionList = new Dictionary<string, HashSet<int>>();

        /// <summary>
        /// Connection info
        /// </summary>
        private Dictionary<int, HashSet<int>> connectionInfo = new Dictionary<int, HashSet<int>>();

        /// <summary>
        /// Positive Imply condition info list
        /// </summary>
        private Dictionary<int, HashSet<ConditionInfo>> implyConditionInfoListPositive = new Dictionary<int, HashSet<ConditionInfo>>();

        /// <summary>
        /// Negative Imply condition info list
        /// </summary>
        private Dictionary<int, HashSet<ConditionInfo>> implyConditionInfoListNegative = new Dictionary<int, HashSet<ConditionInfo>>();

        /// <summary>
        /// Concept id
        /// </summary>
        private int id;
        #endregion

        #region Public Methods
        /// <summary>
        /// Add a list of names to concept info
        /// </summary>
        /// <param name="nameList"></param>
        public void AddNameRange(List<string> nameList)
        {
            this.nameList.UnionWith(nameList);
        }

        /// <summary>
        /// Add a connection info
        /// </summary>
        /// <param name="verbId">verb id</param>
        /// <param name="complementId">complement id</param>
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

        /// <summary>
        /// Add a metaConnection info
        /// </summary>
        /// <param name="metaOperatorName">metaOperator name</param>
        /// <param name="farVerbId">far verb id</param>
        /// <param name="connectionSide">metaConnection side</param>
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

        /// <summary>
        /// Add imply condition info
        /// </summary>
        /// <param name="complementId">complement id</param>
        /// <param name="conditionInfo">condition info</param>
        /// <param name="isPositive">whether the imply connection is positive</param>
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
        /// <summary>
        /// Concept id
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Name list for concept
        /// </summary>
        public HashSet<string> NameList
        {
            get { return nameList; }
        }

        /// <summary>
        /// Connection info
        /// </summary>
        public Dictionary<int, HashSet<int>> ConnectionInfo
        {
            get { return connectionInfo; }
        }

        /// <summary>
        /// Positive metaConnection info list
        /// </summary>
        public Dictionary<string, HashSet<int>> PositiveMetaConnectionList
        {
            get { return positiveMetaConnectionList; }
        }

        /// <summary>
        /// Negative metaConnection info list
        /// </summary>
        public Dictionary<string, HashSet<int>> NegativeMetaConnectionList
        {
            get { return negativeMetaConnectionList; }
        }

        /// <summary>
        /// Positive imply condition info list
        /// </summary>
        public Dictionary<int, HashSet<ConditionInfo>> ImplyConditionInfoListPositive
        {
            get { return implyConditionInfoListPositive; }
        }

        /// <summary>
        /// Negative imply condition info list
        /// </summary>
        public Dictionary<int, HashSet<ConditionInfo>> ImplyConditionInfoListNegative
        {
            get { return implyConditionInfoListNegative; }
        }
        #endregion
    }
}
