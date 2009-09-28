using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class SemanticLikenessMatrixBuilder
    {
        #region Parts
        private Dictionary<string, Dictionary<string, float>> reducedRowCache = new Dictionary<string, Dictionary<string, float>>();
        #endregion

        #region Constants
        private static readonly int defaultSourceWordCount = 2000;

        private static readonly int defaultTargetWordCount = 60;
        #endregion

        #region Public Methods
        /// <summary>
        /// Build semantic likeness matrix from occurence matrix
        /// </summary>
        /// <param name="occurenceMatrix">word occurence matrix</param>
        /// <returns>semantic likeness matrix</returns>
        public Matrix BuildAndBreakSourceMatrix(Matrix occurenceMatrix)
        {
            return Build(occurenceMatrix, defaultSourceWordCount, defaultTargetWordCount);
        }

        /// <summary>
        /// Build semantic likeness matrix from occurence matrix
        /// </summary>
        /// <param name="occurenceMatrix">word occurence matrix</param>
        /// <param name="sourceWordCount">max source word count</param>
        /// <param name="targetWordCount">max target word count</param>
        /// <returns>semantic likeness matrix</returns>
        public Matrix Build(Matrix occurenceMatrix, int sourceWordCount, int targetWordCount)
        {
            Matrix semanticLikenessMatrix = new Matrix();

            HalfLearnFromData(semanticLikenessMatrix, occurenceMatrix.NormalData, sourceWordCount, targetWordCount);
            occurenceMatrix.NormalData = null;
            HalfLearnFromData(semanticLikenessMatrix, occurenceMatrix.ReversedData, sourceWordCount, targetWordCount);
            occurenceMatrix.ReversedData = null;
            occurenceMatrix = null;
            GC.Collect();

            semanticLikenessMatrix = NormalizeToMaxOne(semanticLikenessMatrix);
            semanticLikenessMatrix = SortByToNameValue(semanticLikenessMatrix);
            semanticLikenessMatrix = Trim(semanticLikenessMatrix, targetWordCount);

            return semanticLikenessMatrix;
        }
        #endregion

        #region Private Methods
        private Matrix SortByToNameValue(Matrix semanticLikenessMatrix)
        {
            IEnumerable<KeyValuePair<string, float>> orderedRow;
            Dictionary<string, float> row;
            string fromName;
            Dictionary<string, Dictionary<string, float>> newNormalData = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> newReversedData = new Dictionary<string, Dictionary<string, float>>();
            foreach (KeyValuePair<string, Dictionary<string, float>> fromNameAndRow in semanticLikenessMatrix.NormalData)
            {
                fromName = fromNameAndRow.Key;
                row = fromNameAndRow.Value;
                if (row == null)
                    continue;

                orderedRow = row.OrderByDescending(pair => pair.Value);
                row = orderedRow.ToDictionary(k => k.Key, v => v.Value);
                newNormalData.Add(fromName, row);
            }
            semanticLikenessMatrix.NormalData = newNormalData;

            foreach (KeyValuePair<string, Dictionary<string, float>> fromNameAndRow in semanticLikenessMatrix.ReversedData)
            {
                fromName = fromNameAndRow.Key;
                row = fromNameAndRow.Value;
                if (row == null)
                    continue;

                orderedRow = row.OrderByDescending(pair => pair.Value);
                row = orderedRow.ToDictionary(k => k.Key, v => v.Value);
                newReversedData.Add(fromName, row);
            }
            semanticLikenessMatrix.ReversedData = newReversedData;

            return semanticLikenessMatrix;
        }

        private Matrix Trim(Matrix semanticLikenessMatrix, int maxToValueCount)
        {
            IEnumerable<KeyValuePair<string, float>> trimmedRow;
            Dictionary<string, float> row;
            string fromName;
            Dictionary<string, Dictionary<string, float>> newNormalData = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> newReversedData = new Dictionary<string, Dictionary<string, float>>();
            foreach (KeyValuePair<string, Dictionary<string, float>> fromNameAndRow in semanticLikenessMatrix.NormalData)
            {
                fromName = fromNameAndRow.Key;
                row = fromNameAndRow.Value;
                if (row == null)
                    continue;

                trimmedRow = row.Take(maxToValueCount);
                row = trimmedRow.ToDictionary(k => k.Key, k => k.Value);
                newNormalData.Add(fromName, row);
            }
            semanticLikenessMatrix.NormalData = newNormalData;

            foreach (KeyValuePair<string, Dictionary<string, float>> fromNameAndRow in semanticLikenessMatrix.ReversedData)
            {
                fromName = fromNameAndRow.Key;
                row = fromNameAndRow.Value;
                if (row == null)
                    continue;

                trimmedRow = row.Take(maxToValueCount);
                row = trimmedRow.ToDictionary(k => k.Key, k => k.Value);
                newReversedData.Add(fromName, row);
            }
            semanticLikenessMatrix.ReversedData = newReversedData;

            return semanticLikenessMatrix;
        }

        private void HalfLearnFromData(Matrix semanticLikenessMatrix, Dictionary<string, Dictionary<string, float>> data, int sourceWordCount, int targetWordCount)
        {
            data = GetReducedData(data, sourceWordCount);

            HashSet<string> availableWordList = new HashSet<string>(data.Keys);

            string word1, word2;
            Dictionary<string, float> row1, row2;
            foreach (KeyValuePair<string, Dictionary<string, float>> fromAndRow1 in data)
            {
                word1 = fromAndRow1.Key;
                if (word1.Length == 0)
                    continue;
                row1 = fromAndRow1.Value;
                row1 = IntersetcWith(row1, availableWordList);
                row1 = NormalizeToTotalOne(row1);

                foreach (KeyValuePair<string, Dictionary<string, float>> fromAndRow2 in data)
                {
                    word2 = fromAndRow2.Key;
                    if (word2.Length == 0)
                        continue;
                    row2 = fromAndRow2.Value;
                    row2 = IntersetcWith(row2, availableWordList);
                    row2 = NormalizeToTotalOne(row2);

                    //semanticLikenessMatrix.MultiplyStatistics(word1, word2, CompareRows(row1, row2, word1, word2) / (float)(2.0));
                    semanticLikenessMatrix.AddStatistics(word1, word2, CompareRows(row1, row2, word1, word2) / (float)(2.0));

                    //semanticLikenessMatrix.NormalData[word1] = TrimExceedingContent(semanticLikenessMatrix.NormalData[word1], targetWordCount);
                    //semanticLikenessMatrix.ReversedData[word2] = TrimExceedingContent(semanticLikenessMatrix.ReversedData[word2], targetWordCount);
                }
                TrimExceedingContent(semanticLikenessMatrix, targetWordCount);
                //GC.Collect();
            }
        }

        private void TrimExceedingContent(Matrix semanticLikenessMatrix, int targetWordCount)
        {
            TrimExceedingContent(semanticLikenessMatrix.NormalData, targetWordCount);
            //TrimExceedingContent(semanticLikenessMatrix.ReversedData, targetWordCount);
            semanticLikenessMatrix.ReversedData.Clear();
        }

        private void TrimExceedingContent(Dictionary<string, Dictionary<string, float>> data, int targetWordCount)
        {
            List<KeyValuePair<string, Dictionary<string, float>>> oldData = new List<KeyValuePair<string, Dictionary<string, float>>>(data);

            string fromName;
            Dictionary<string, float> row;
            foreach (KeyValuePair<string, Dictionary<string, float>> fromNameAndRow in oldData)
            {
                fromName = fromNameAndRow.Key;
                row = fromNameAndRow.Value;
                data[fromName] = TrimExceedingContent(row, targetWordCount);
            }
        }

        private Dictionary<string, float> TrimExceedingContent(Dictionary<string, float> oldRow, int targetWordCount)
        {
            if (oldRow.Count > targetWordCount * 6)
            {

                IEnumerable<KeyValuePair<string, float>> trimmedRow;
                Dictionary<string, float> newRow;
                trimmedRow = oldRow.OrderByDescending(pair => pair.Value);
                trimmedRow = trimmedRow.Take(targetWordCount * 4);
                newRow = trimmedRow.ToDictionary(k => k.Key, v => v.Value);
                return newRow;
            }
            else
            {
                return oldRow;
            }
        }

        private Dictionary<string, float> IntersetcWith(Dictionary<string, float> row, HashSet<string> availableWordList)
        {
            Dictionary<string, float> trimmedDow = new Dictionary<string, float>();

            foreach (KeyValuePair<string, float> keyAndValue in row)
                if (availableWordList.Contains(keyAndValue.Key))
                    trimmedDow.Add(keyAndValue.Key, keyAndValue.Value);

            return trimmedDow;
        }

        private Dictionary<string, Dictionary<string, float>> GetReducedData(Dictionary<string, Dictionary<string, float>> data, int sourceWordCount)
        {
            IEnumerable<KeyValuePair<string, Dictionary<string, float>>> orderedData = data.OrderByDescending(pair => pair.Value.Count);
            orderedData = orderedData.Take(sourceWordCount);
            data = orderedData.ToDictionary(k => k.Key, v => v.Value);


            return data;
        }

        private Matrix NormalizeToMaxOne(Matrix rawMatrix)
        {
            Matrix normalizedMatrix = new Matrix();

            foreach (KeyValuePair<string, Dictionary<string, float>> fromAndRow in rawMatrix.NormalData)
                normalizedMatrix.NormalData.Add(fromAndRow.Key, NormalizeToMaxOne(fromAndRow.Value));

            foreach (KeyValuePair<string, Dictionary<string, float>> fromAndRow in rawMatrix.ReversedData)
                normalizedMatrix.ReversedData.Add(fromAndRow.Key, NormalizeToMaxOne(fromAndRow.Value));

            return normalizedMatrix;
        }

        private Dictionary<string, float> NormalizeToMaxOne(Dictionary<string, float> rawRow)
        {
            Dictionary<string, float> normalizedRow = new Dictionary<string, float>();

            float maxOccurence = rawRow.Values.Max();

            if (maxOccurence == 0.0)
                return null;

            foreach (KeyValuePair<string, float> toWordAndOccurence in rawRow)
                normalizedRow.Add(toWordAndOccurence.Key, toWordAndOccurence.Value / maxOccurence);

            return normalizedRow;
        }

        private Dictionary<string, float> NormalizeToTotalOne(Dictionary<string, float> rawRow)
        {
            Dictionary<string, float> normalizedRow = new Dictionary<string, float>();

            float totalOccurence = rawRow.Values.Sum();

            if (totalOccurence == 0.0)
                return null;

            foreach (KeyValuePair<string, float> toWordAndOccurence in rawRow)
                normalizedRow.Add(toWordAndOccurence.Key, toWordAndOccurence.Value / totalOccurence);

            return normalizedRow;
        }

        private float CompareRows(Dictionary<string, float> row1, Dictionary<string, float> row2, string word1, string word2)
        {
            if (row1 == null || row2 == null)
                return 0.0f;

            float valueInRow1, valueInRow2;

            float product = 0.0f;

            HashSet<string> possibleWordList = new HashSet<string>(row1.Keys);
            possibleWordList.UnionWith(row2.Keys);


            foreach (string word in possibleWordList)
            {
                if (!row1.TryGetValue(word, out valueInRow1))
                    valueInRow1 = 0;

                if (!row2.TryGetValue(word, out valueInRow2))
                    valueInRow2 = 0;

                product += (float)(Math.Sqrt(valueInRow1) * Math.Sqrt(valueInRow2));
            }


            return product;
        }
        #endregion
    }
}