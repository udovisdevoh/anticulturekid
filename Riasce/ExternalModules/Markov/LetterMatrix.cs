using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Letter Matrix
    /// </summary>
    class LetterMatrix
    {
        #region Fields
        /// <summary>
        /// Matrix's data
        /// </summary>
        private Dictionary<string, Dictionary<string, int>> data;

        /// <summary>
        /// Random number generator
        /// </summary>
        private static Random random;
        #endregion

        #region Constants
        /// <summary>
        /// Sample size
        /// </summary>
        private const int sampleSize = 2000;

        /// <summary>
        /// Max word count
        /// </summary>
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
        /// <summary>
        /// Learn from the word or sentence and add statistics to the matrix
        /// </summary>
        /// <param name="source">source word or sentence</param>
        public void Learn(string source)
        {
            source = source.Replace('_', ' ');

            if (source.Contains(' '))
                LearnSentence(source);
            else
                LearnWord(source);
        }

        /// <summary>
        /// Learn from the word group and add content to the matrix
        /// </summary>
        /// <param name="source">word group</param>
        public void Learn(IEnumerable<string> source)
        {
            foreach (string element in source)
                Learn(element);
        }

        /// <summary>
        /// Return a random word from matrix which will not be longer than longest word yet
        /// </summary>
        /// <returns>a random word from matrix</returns>
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

        /// <summary>
        /// Generate word
        /// </summary>
        /// <returns>randomly generated word</returns>
        public string GenerateWord()
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

        /// <summary>
        /// Get utter random character
        /// </summary>
        /// <returns>utter random character</returns>
        private string GetUtterRandomChar()
        {
            List<string> charList = new List<string>(data.Keys);
            return charList[random.Next(charList.Count)];
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Learn word
        /// </summary>
        /// <param name="word">word to learn from</param>
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

        /// <summary>
        /// Learn sentence
        /// </summary>
        /// <param name="source">text source</param>
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

        /// <summary>
        /// Add statistics
        /// </summary>
        /// <param name="from">from word</param>
        /// <param name="to">to word</param>
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

        /// <summary>
        /// Get next char
        /// </summary>
        /// <param name="twoChars">two chars preceeding next char</param>
        /// <returns>next char</returns>
        private string GetNextChar(string twoChars)
        {
            Dictionary<string, int> availableCharacterList;

            if (!data.TryGetValue(twoChars, out availableCharacterList))
                return " ";

            return GetPonderatedRandomValue(GetProbabilityMatrix(availableCharacterList));
        }

        /// <summary>
        /// Get probability matrix
        /// </summary>
        /// <param name="charAndCountList">char and count list</param>
        /// <returns>probability matrix</returns>
        private Dictionary<string, float> GetProbabilityMatrix(Dictionary<string, int> charAndCountList)
        {
            Dictionary<string, float> probabilityMatrix = new Dictionary<string, float>();

            int totalCount = 0;
            foreach (int count in charAndCountList.Values)
                totalCount += count;

            foreach (KeyValuePair<string,int> charAndCount in charAndCountList)
                probabilityMatrix.Add(charAndCount.Key, (float)(charAndCount.Value) / (float)(totalCount));

            return probabilityMatrix;
        }

        /// <summary>
        /// Get ponderated random value
        /// </summary>
        /// <param name="charAndProbabilityList">char and probability list</param>
        /// <returns>ponderated random value</returns>
        private string GetPonderatedRandomValue(Dictionary<string, float> charAndProbabilityList)
        {
            if (charAndProbabilityList.Count < 1)
                return " ";

            float randomFloat = (float)random.NextDouble();
            float floatCounter = 0;
            
            string character;
            string previousCharacter = null;
            float probability;
            foreach (KeyValuePair<string, float> charAndProbability in charAndProbabilityList)
            {
                character = charAndProbability.Key;
                probability = charAndProbability.Value;
                floatCounter += probability;

                if (previousCharacter == null)
                    previousCharacter = character;

                character = charAndProbability.Key;
                probability = charAndProbability.Value;
                floatCounter += probability;

                if (floatCounter >= randomFloat)
                    break;
                
                previousCharacter = character;
            }

            return previousCharacter;
        }
        #endregion
    }
}