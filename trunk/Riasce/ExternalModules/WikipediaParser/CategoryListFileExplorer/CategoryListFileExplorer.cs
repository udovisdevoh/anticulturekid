using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AntiCulture.Kid
{
    class CategoryListFileExplorer
    {
        #region Public methods
        public static string GetFirstEntry(string fileName)
        {
            string entry = null;

            StreamReader reader = new StreamReader(fileName);
            try
            {
                entry = reader.ReadLine();
            }
            catch (Exception)
            {
                entry = null;
            }
            finally
            {
                reader.Close();
            }

            return entry;
        }

        public static string GetLastEntry(string fileName)
        {
            string line = "";
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            {
                if (fileStream.Length < 2)
                    return null;

                fileStream.Position = fileStream.Length - 2;
                while (fileStream.CanSeek && fileStream.Position > 0)
                {
                    fileStream.Position--;
                    char currentChar = (char)(fileStream.ReadByte());
                    fileStream.Position--;
                    if (currentChar == '\n' || currentChar == '\r')
                        break;
                    line = "" + currentChar + line;
                }
            }
            line = line.Trim();
            if (line == "")
                return null;
            else
                return line;
        }
        #endregion
    }
}
