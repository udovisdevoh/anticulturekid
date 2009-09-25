using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractMatrix
    {
        /// <summary>
        /// Add 1 to existing statistics count
        /// </summary>
        /// <param name="fromValue">from value</param>
        /// <param name="toValue">to value</param>
        public abstract void AddStatistics(string fromValue, string toValue);

        /// <summary>
        /// Add a number to existing statistics count
        /// </summary>
        /// <param name="fromValue">from value</param>
        /// <param name="toValue">to value</param>
        /// <param name="toAdd">add to existing count</param>
        public abstract void AddStatistics(string fromValue, string toValue, float toAdd);

        /// <summary>
        /// Set statistics count number for values
        /// </summary>
        /// <param name="fromValue">from value</param>
        /// <param name="toValue">to value</param>
        /// <param name="newCount">new count</param>
        public abstract void SetStatistics(string fromValue, string toValue, float newCount);

        /// <summary>
        /// Whether a key name is present in the matrix
        /// </summary>
        /// <param name="keyName">key name</param>
        /// <returns>whether a key name is present in the matrix</returns>
        public abstract bool ContainsKey(string keyName);
    }
}
