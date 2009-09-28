using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    class MultipleSubjectBetweenAndExtractor
    {
        #region Public Methods
        public string ExtractList(string humanStatementParagraph, out List<string> otherSubjectStatementList)
        {
            otherSubjectStatementList = new List<string>();

            if (!humanStatementParagraph.ContainsWord("and"))
                return humanStatementParagraph;

            HashSet<string> otherSubjectList = GetOtherSubjectList(humanStatementParagraph);

            humanStatementParagraph = RemoveOtherSubjects(humanStatementParagraph);

            foreach (string otherSubject in otherSubjectList)
                otherSubjectStatementList.Add(ReplaceSubject(humanStatementParagraph, otherSubject));

            return humanStatementParagraph;
        }
        #endregion

        #region Private Methods
        private HashSet<string> GetOtherSubjectList(string humanStatementParagraph)
        {
            HashSet<string> subjectList = new HashSet<string>();

            string[] words = humanStatementParagraph.Split(' ');

            for (int i = 0; i + 1 < words.Length; i+= 2)
            {
                if (words[i + 1] == "and")
                {
                    subjectList.Add(words[i]);
                }
                else
                {
                    break;
                }
            }

            return subjectList;
        }

        private string RemoveOtherSubjects(string oldString)
        {
            string[] words = oldString.Split(' ');

            int i;

            for (i = 0; i + 1 < words.Length; i++)
                if (words[i] != "and" && i % 2 == 1)
                    break;

            string newString = string.Empty;

            i--;
            for (; i < words.Length; i++)
                newString += " " + words[i];
            
            newString = newString.Trim();
            return newString;
        }

        private string ParseMultipleSubjectBetweenAnd(string oldString)
        {
            string newString = "";
            List<string> wordList = new List<string>(oldString.Split(' '));
            wordList = ParseMultipleSubjectBetweenAnd(wordList);
            foreach (string word in wordList)
                newString += " " + word;
            return newString.Trim();
        }

        private string ReplaceSubject(string sentence, string to)
        {
            if (!sentence.Contains(' '))
                return sentence;

            sentence = to + " " + sentence.Substring(sentence.IndexOf(' '));

            return sentence;
        }

        private List<string> ParseMultipleSubjectBetweenAnd(List<string> wordList)
        {
            //red and blue and green isa color partof chromatic_circle and light
            bool couldFindAndWhenExpected = true;

            HashSet<int> andListToReplace = new HashSet<int>();
            string verbAndComplement = string.Empty;

            for (int i = 0; i < wordList.Count; i++)
            {
                if (couldFindAndWhenExpected && i % 2 == 1)
                {
                    if (wordList[i] == "and")
                    {
                        andListToReplace.Add(i);
                    }
                    else
                    {
                        verbAndComplement += " " + wordList[i];
                        couldFindAndWhenExpected = false;
                    }
                }
                else if (!couldFindAndWhenExpected)
                {
                    verbAndComplement += " " + wordList[i];
                }
            }
            verbAndComplement = verbAndComplement.Trim();

            foreach (int i in andListToReplace)
                wordList[i] = verbAndComplement;

            return wordList;
        }
        #endregion
    }
}
