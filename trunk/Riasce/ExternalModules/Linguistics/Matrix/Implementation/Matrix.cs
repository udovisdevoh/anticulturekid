using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class Matrix : AbstractMatrix
    {
        #region Fields
        /// <summary>
        /// Normal matrix
        /// </summary>
        private Dictionary<string, Dictionary<string, float>> normalData = new Dictionary<string, Dictionary<string, float>>();

        /// <summary>
        /// 90 degree rotated matrix
        /// </summary>
        private Dictionary<string, Dictionary<string, float>> reversedData = new Dictionary<string, Dictionary<string, float>>();
        #endregion

        #region Public Methods
        public void MultiplyStatistics(string fromValue, string toValue, float toMultiply)
        {
            MultiplyStatisticsTo(normalData, fromValue, toValue, toMultiply);
            MultiplyStatisticsTo(reversedData, toValue, fromValue, toMultiply);
        }

        public override void AddStatistics(string fromValue, string toValue)
        {
            AddStatistics(fromValue, toValue, 1);
        }

        public override void AddStatistics(string fromValue, string toValue, float toAdd)
        {
            AddStatisticsTo(normalData, fromValue, toValue, toAdd);
            AddStatisticsTo(reversedData, toValue, fromValue, toAdd);
        }

        public override void SetStatistics(string fromValue, string toValue, float newCount)
        {
            SetStatisticsTo(normalData, fromValue, toValue, newCount);
            SetStatisticsTo(reversedData, toValue, fromValue, newCount);
        }

        public override bool ContainsKey(string keyName)
        {
            if (normalData.ContainsKey(keyName))
                return true;
            else if (reversedData.ContainsKey(keyName))
                return true;
            else
                return false;
        }

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
        public Dictionary<string, Dictionary<string, float>> NormalData
        {
            get { return normalData; }
            set { normalData = value; }
        }

        public Dictionary<string, Dictionary<string, float>> ReversedData
        {
            get { return reversedData; }
            set { reversedData = value; }
        }
        #endregion
    }
}