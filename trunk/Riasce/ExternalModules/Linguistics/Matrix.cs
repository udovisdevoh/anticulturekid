using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Represents a matrix
    /// key: strings
    /// value: float
    /// </summary>
    class Matrix
    {
        #region Fields
        /// <summary>
        /// Normal matrix representation
        /// </summary>
        private Dictionary<string, Dictionary<string, float>> normalData = new Dictionary<string, Dictionary<string, float>>();

        /// <summary>
        /// 90 degree rotated matrix representation
        /// </summary>
        private Dictionary<string, Dictionary<string, float>> reversedData = new Dictionary<string, Dictionary<string, float>>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Multiply statistics
        /// </summary>
        /// <param name="fromValue">from value</param>
        /// <param name="toValue">to value</param>
        /// <param name="toMultiply">multiplicator</param>
        public void MultiplyStatistics(string fromValue, string toValue, float toMultiply)
        {
            MultiplyStatisticsTo(normalData, fromValue, toValue, toMultiply);
            MultiplyStatisticsTo(reversedData, toValue, fromValue, toMultiply);
        }

        /// <summary>
        /// Add 1 to existing statistics count
        /// </summary>
        /// <param name="fromValue">from value</param>
        /// <param name="toValue">to value</param>
        public void AddStatistics(string fromValue, string toValue)
        {
            AddStatistics(fromValue, toValue, 1);
        }

        /// <summary>
        /// Add a number to existing statistics count
        /// </summary>
        /// <param name="fromValue">from value</param>
        /// <param name="toValue">to value</param>
        /// <param name="toAdd">add to existing count</param>
        public void AddStatistics(string fromValue, string toValue, float toAdd)
        {
            AddStatisticsTo(normalData, fromValue, toValue, toAdd);
            AddStatisticsTo(reversedData, toValue, fromValue, toAdd);
        }

        /// <summary>
        /// Set statistics count number for values
        /// </summary>
        /// <param name="fromValue">from value</param>
        /// <param name="toValue">to value</param>
        /// <param name="newCount">new count</param>
        public void SetStatistics(string fromValue, string toValue, float newCount)
        {
            SetStatisticsTo(normalData, fromValue, toValue, newCount);
            SetStatisticsTo(reversedData, toValue, fromValue, newCount);
        }

        /// <summary>
        /// Whether a key name is present in the matrix
        /// </summary>
        /// <param name="keyName">key name</param>
        /// <returns>whether a key name is present in the matrix</returns>
        public bool ContainsKey(string keyName)
        {
            if (normalData.ContainsKey(keyName))
                return true;
            else if (reversedData.ContainsKey(keyName))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Try get normal value
        /// </summary>
        /// <param name="subjectName">subject concept name</param>
        /// <param name="otherConceptName">other concept name</param>
        /// <returns>current value</returns>
        public float TryGetNormalValue(string subjectName, string otherConceptName)
        {
            Dictionary<string,float> vector;
            float value;

            if (!normalData.TryGetValue(subjectName, out vector))
                return 0.0f;

            if (!vector.TryGetValue(otherConceptName, out value))
                return 0.0f;

            return value;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Multiply statistics
        /// </summary>
        /// <param name="data">data</param>
        /// <param name="fromValue">from value</param>
        /// <param name="toValue">to value</param>
        /// <param name="toMultiply">multiplicator</param>
        private void MultiplyStatisticsTo(Dictionary<string, Dictionary<string, float>> data, string fromValue, string toValue, float toMultiply)
        {
            Dictionary<string, float> row;
            if (!data.TryGetValue(fromValue, out row))
            {
                row = new Dictionary<string, float>();
                data.Add(fromValue, row);
            }

            float totalOccurence;
            if (row.TryGetValue(toValue, out totalOccurence))
            {
                row[toValue] = (float)(Math.Sqrt(totalOccurence) * Math.Sqrt(toMultiply));
            }
            else
            {
                row.Add(toValue, toMultiply);
            }
        }

        /// <summary>
        /// Add statistics
        /// </summary>
        /// <param name="data">data</param>
        /// <param name="fromValue">from value</param>
        /// <param name="toValue">to value</param>
        /// <param name="toAdd">number to add</param>
        private void AddStatisticsTo(Dictionary<string, Dictionary<string, float>> data, string fromValue, string toValue, float toAdd)
        {
            Dictionary<string, float> row;
            if (!data.TryGetValue(fromValue, out row))
            {
                row = new Dictionary<string, float>();
                data.Add(fromValue, row);
            }

            float totalOccurence;
            if (row.TryGetValue(toValue, out totalOccurence))
            {
                row[toValue] = totalOccurence + toAdd;
            }
            else
            {
                row.Add(toValue, toAdd);
            }
        }

        /// <summary>
        /// Set statistics
        /// </summary>
        /// <param name="data">data</param>
        /// <param name="fromValue">from value</param>
        /// <param name="toValue">to value</param>
        /// <param name="newCount">new number to set</param>
        private void SetStatisticsTo(Dictionary<string, Dictionary<string, float>> data, string fromValue, string toValue, float newCount)
        {
            Dictionary<string, float> row;
            if (!data.TryGetValue(fromValue, out row))
            {
                row = new Dictionary<string, float>();
                data.Add(fromValue, row);
            }

            float totalOccurence;
            if (row.TryGetValue(toValue, out totalOccurence))
            {
                row[toValue] = newCount;
            }
            else
            {
                row.Add(toValue, newCount);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Normal matrix representation
        /// </summary>
        public Dictionary<string, Dictionary<string, float>> NormalData
        {
            get { return normalData; }
            set { normalData = value; }
        }

        /// <summary>
        /// 90 degree rotated matrix representation
        /// </summary>
        public Dictionary<string, Dictionary<string, float>> ReversedData
        {
            get { return reversedData; }
            set { reversedData = value; }
        }
        #endregion
    }
}