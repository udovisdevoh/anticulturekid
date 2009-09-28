using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Xml matrix saver loader
    /// </summary>
    class XmlMatrixSaverLoader
    {
        #region Public Methods
        /// <summary>
        /// Save matrix to XML file
        /// </summary>
        /// <param name="matrix">matrix to save</param>
        /// <param name="fileName">file name</param>
        public void Save(Matrix matrix, string fileName)
        {
            XmlTextWriter textWriter = new XmlTextWriter(fileName, Encoding.UTF8);
            textWriter.Formatting = Formatting.Indented;
            textWriter.Indentation = 4;
            textWriter.WriteStartDocument();

            textWriter.WriteStartElement("wordMatrix");

            XmlWriteData(textWriter, matrix.NormalData);

            textWriter.WriteEndElement();

            textWriter.WriteEndDocument();
            textWriter.Flush();
            textWriter.Close();
        }

        /// <summary>
        /// Load matrix from XML file
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>Loaded matrix</returns>
        public Matrix Load(string fileName)
        {
            XmlTextReader textReader = new XmlTextReader(fileName);
            Matrix matrix = new Matrix();
            string fromWord = null;
            string toWord;
            float statisticValue;

            while (textReader.Read())
            {
                if (textReader.NodeType == XmlNodeType.Element)
                {
                    if (textReader.Name == "fromWord")
                    {
                        fromWord = textReader.GetAttribute("name");
                    }
                    else if (textReader.Name == "toWord")
                    {
                        if (fromWord != null)
                        {
                            toWord = textReader.GetAttribute("name");
                            float.TryParse(textReader.GetAttribute("statisticValue"), out statisticValue);
                            matrix.SetStatistics(fromWord, toWord, statisticValue);
                        }
                    }
                }
            }

            textReader.Close();

            return matrix;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Write xml data
        /// </summary>
        /// <param name="textWriter">text writer</param>
        /// <param name="data">data to write</param>
        private void XmlWriteData(XmlTextWriter textWriter, Dictionary<string, Dictionary<string, float>> data)
        {
            string from;
            Dictionary<string, float> row;
            foreach (KeyValuePair<string, Dictionary<string, float>> fromAndRow in data)
            {
                from = fromAndRow.Key;
                row = fromAndRow.Value;

                XmlWriteRow(textWriter, from, row);
            }
        }

        /// <summary>
        /// Write xml row
        /// </summary>
        /// <param name="textWriter">text writer</param>
        /// <param name="from">from word</param>
        /// <param name="row">row to write</param>
        private void XmlWriteRow(XmlTextWriter textWriter, string from, Dictionary<string, float> row)
        {
            textWriter.WriteStartElement("fromWord");
            textWriter.WriteAttributeString("name", from);

            foreach (KeyValuePair<string, float> wordAndOccurence in row)
            {
                textWriter.WriteStartElement("toWord");
                textWriter.WriteAttributeString("name", wordAndOccurence.Key);
                textWriter.WriteAttributeString("statisticValue", wordAndOccurence.Value.ToString());
                textWriter.WriteEndElement();
            }

            textWriter.WriteEndElement();
        }
        #endregion
    }
}