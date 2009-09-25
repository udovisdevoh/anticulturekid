using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class LetterMatrix : AbstractLetterMatrix
    {
        #region Fields
        private Dictionary<string, Dictionary<string, int>> data;

        private static Random random;
        #endregion

        #region Constants
        private const int sampleSize = 2000;

        private static int maxWordCount = 20;
        #endregion

        #region Constructors
        static LetterMatrix()
        {
            random = new Random();
        }

        public LetterMatrix()
        {
            data = new Dictionary<string, Dictionary<string, int>>();
        }

        public LetterMatrix(string source)
        {
            data = new Dictionary<string, Dictionary<string, int>>();
            Learn(source);
        }

        public LetterMatrix(IEnumerable<string> source)
        {
            data = new Dictionary<string, Dictionary<string, int>>();
            Learn(source);
        }
        #endregion

        #region Public Methods
        public override void Learn(string source)
        {
            source = source.Replace('_', ' ');

            if (source.Contains(' '))
                LearnSentence(source);
            else
                LearnWord(source);
        }

        public override void Learn(IEnumerable<string> source)
        {
            foreach (string element in source)
                Learn(element);
        }

        public string GenerateWord(int desiredLength)
        {
            maxWordCount = desiredLength + 5;

            List<string> wordList = new List<string>();

            for (int i = 0; i < sampleSize; i++)
                wordList.Add(GenerateWord());

            string bestWord = "";
            int bestDifference = -1;
            int currentDifference;
            foreach (string currentWord in wordList)
            {
                currentDifference = Math.Abs(currentWord.Length - desiredLength) + random.Next(4); 

                if (bestWord == "" || currentDifference < bestDifference)
                {
                    bestDifference = currentDifference;
                    bestWord = currentWord;
                }
            }

            return bestWord;
        }

        public override string GenerateWord()
        {
            string word = string.Empty;
            string currentChar;
            string previousChar = GetUtterRandomChar();

            do
            {
                currentChar = GetNextChar(previousChar);
                word += currentChar;
                previousChar = currentChar;

            } while (currentChar != " " && word.Length < maxWordCount);

            word = word.Trim();

            return word;
        }

        private string GetUtterRandomChar()
        {
            List<string> charList = new List<string>(data.Keys);
            return charList[random.Next(charList.Count)];
        }
        #endregion

        #region Private Methods
        private void LearnWord(string word)
        {
            word = word.ToLower();

            word = " " + word.Trim() + " ";

            string previousChar = null;
            foreach (Char currentChar in word)
            {
                if (previousChar != null)
                    AddStatistics(previousChar, currentChar.ToString());

                previousChar = currentChar.ToString();
            }
        }

        private void LearnSentence(string source)
        {
            string[] wordList = source.Split(' ');

            string trimmedWord;
            foreach (string word in wordList)
            {
                trimmedWord = word.Trim();
                if (trimmedWord.Length > 0)
                {
                    LearnWord(trimmedWord);
                }
            }
        }

        private void AddStatistics(string from, string to)
        {
            Dictionary<string, int> row;
            if (!data.TryGetValue(from, out row))
            {
                row = new Dictionary<string, int>();
                data.Add(from, row);
            }

            int totalOccurence;
            if (row.TryGetValue(to, out totalOccurence))
            {
                row[to] = totalOccurence + 1;
            }
            else
            {
                row.Add(to, 1);
            }
        }

        private string GetNextChar(string twoChars)
        {
            Dictionary<string, int> availableCharacterList;

            if (!data.TryGetValue(twoChars, out availableCharacterList))
                return " ";

            return GetPonderatedRandomValue(GetProbabilityMatrix(availableCharacterList));
        }

        private Dictionary<string, double> GetProbabilityMatrix(Dictionary<string, int> charAndCountList)
        {
            Dictionary<string, double> probabilityMatrix = new Dictionary<string, double>();

            int totalCount = 0;
            foreach (int count in charAndCountList.Values)
                totalCount += count;

            foreach (KeyValuePair<string,int> charAndCount in charAndCountList)
                probabilityMatrix.Add(charAndCount.Key, (double)(charAndCount.Value) / (double)(totalCount));

            return probabilityMatrix;
        }

        private string GetPonderatedRandomValue(Dictionary<string, double> charAndProbabilityList)
        {
            if (charAndProbabilityList.Count < 1)
                return " ";

            double randomDouble = random.NextDouble();
            double doubleCounter = 0;
            
            string character;
            string previousCharacter = null;
            double probability;
            foreach (KeyValuePair<string,double> charAndProbability in charAndProbabilityList)
            {
                character = charAndProbability.Key;
                probability = charAndProbability.Value;
                doubleCounter += probability;

                if (previousCharacter == null)
                    previousCharacter = character;

                character = charAndProbability.Key;
                probability = charAndProbability.Value;
                doubleCounter += probability;

                if (doubleCounter >= randomDouble)
                    break;
                
                previousCharacter = character;
            }

            return previousCharacter;
        }
        #endregion
    }
}