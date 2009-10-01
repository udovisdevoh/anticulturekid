using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used to store flat branches so we don't have
    /// to reflatten, until we make a change in connections, metaConnection or imply connections
    /// </summary>
    static class RepairedFlatBranchCache
    {
        #region Fields
        /// <summary>
        /// List of connection branches
        /// </summary>
        private static HashSet<ConnectionBranch> data = new HashSet<ConnectionBranch>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Add connection branch to cache
        /// </summary>
        /// <param name="connectionBranch">connection branch to add</param>
        public static void Add(ConnectionBranch connectionBranch)
        {
            data.Add(connectionBranch);
        }

        /// <summary>
        /// Clear flat branch cache
        /// </summary>
        public static void Clear()
        {
            data.Clear();
        }

        /// <summary>
        /// Whether the cache contains connection branch
        /// </summary>
        /// <param name="connectionBranch">connection branch</param>
        /// <returns>whether the cache contains connection branch</returns>
        public static bool Contains(ConnectionBranch connectionBranch)
        {
            return data.Contains(connectionBranch);
        }
        #endregion
    }
}
