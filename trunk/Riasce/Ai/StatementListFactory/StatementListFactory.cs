using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    class StatementListFactory : AbstractStatementListFactory
    {
        #region Parts
        private StatementFactory statementFactory = new StatementFactory();

        private RelativeSubOrdinateExtractor relativeSubOrdinateExtractor = new RelativeSubOrdinateExtractor();

        private MultipleSubjectBetweenAndExtractor multipleSubjectBetweenAndExtractor = new MultipleSubjectBetweenAndExtractor();
        #endregion

        #region Public Methods
        public override List<Statement> GetInterpretedHumanStatementList(string humanName, string humanStatementParagraph)
        {
            List<Statement> statementList = new List<Statement>();
            Statement currentStatement;
            
            try
            {
                currentStatement = statementFactory.GetInterpretedHumanStatement(humanName, humanStatementParagraph);
                statementList.Add(currentStatement);
            }
            catch (StatementParsingException statementParsingException)
            {
                List<string> relativeSubOrdinateList;
                List<string> otherSubjectStatementList;

                humanStatementParagraph = humanStatementParagraph.FixStringForHimmlStatementParsing();
                humanStatementParagraph = humanStatementParagraph.ReplaceWord("nor", "and");
                humanStatementParagraph = relativeSubOrdinateExtractor.ExtractList(humanStatementParagraph, out relativeSubOrdinateList);
                humanStatementParagraph = multipleSubjectBetweenAndExtractor.ExtractList(humanStatementParagraph, out otherSubjectStatementList);
                humanStatementParagraph = RemoveUselessAnd(humanStatementParagraph);
                humanStatementParagraph = ReplaceAndWithPreviousOperator(humanStatementParagraph);

                List<string> words = new List<string>(humanStatementParagraph.Split(' '));

                string humanStatementParagraphNoNot = humanStatementParagraph.RemoveWord("not");
                List<string> wordsNoNot = new List<string>(humanStatementParagraphNoNot.Split(' '));

                if (LanguageDictionary.StringContainsSpecialKeyWordsOrMetaOperators(humanStatementParagraphNoNot))
                    throw new StatementParsingException(statementParsingException.Message + " Couldn't split expression list into smaller chunks because it contains primitive operators such as and, not, if etc...");
                else if (words.Count < 5 && relativeSubOrdinateList.Count < 1 && otherSubjectStatementList.Count < 1)
                    throw new StatementParsingException(statementParsingException.Message + " Human expression list must contain at least 5 words to be splittable");
                else if (wordsNoNot.Count % 2 != 1)
                    throw new StatementParsingException(statementParsingException.Message + " Word count must be odd, not even so the human statement can be splitted");

                List<string> humanStatementList = GetSplittedHumanStatementList(humanStatementParagraph);

                foreach (string currentHumanStatement in humanStatementList)
                    statementList.Add(statementFactory.GetInterpretedHumanStatement(humanName, currentHumanStatement));

                foreach (string relativeSubOrdinate in relativeSubOrdinateList)
                    statementList = mergeStatementList(statementList, GetInterpretedHumanStatementList(humanName, relativeSubOrdinate));

                foreach (string otherSubjectStatement in otherSubjectStatementList)
                    statementList = mergeStatementList(statementList, GetInterpretedHumanStatementList(humanName, otherSubjectStatement));
            }

            return statementList;
        }
        #endregion

        #region Private Methods
        private List<Statement> mergeStatementList(List<Statement> list1, List<Statement> list2)
        {
            List<Statement> list3 = new List<Statement>();

            foreach (Statement currentStatement in list1)
                list3.Add(currentStatement);

            foreach (Statement currentStatement in list2)
                list3.Add(currentStatement);

            return list3;
        }

        private string ReplaceAndWithPreviousOperator(string oldString)
        {
            string newString = "";
            List<string> wordList = new List<string>(oldString.Split(' '));
            wordList = ReplaceAndWithPreviousOperator(wordList);

            foreach (string word in wordList)
                newString += " " + word;

            return newString.Trim();
        }

        private List<string> ReplaceAndWithPreviousOperator(List<string> wordList)
        {
            for (int i = 0; i < wordList.Count; i++)
            {
                if ((wordList[i] == "and") && i > 2 && i % 2 == 1)
                {
                    wordList[i] = wordList[i - 2];

                    if (i - 3 > 0 && wordList[i - 3] == "not")
                        wordList.Insert(i, "not");
                }
            }

            return wordList;
        }

        private List<string> GetSplittedHumanStatementList(string humanStatementParagraph)
        {
            List<string> humanStatementList = new List<string>();
            List<string> words = new List<string>(humanStatementParagraph.Split(' '));
            string currentHumanStatement;

            string subjectConcept = words[0];

            for (int i = 1; i < words.Count; i += 2)
            {
                if (words[i] == "not")
                {
                    currentHumanStatement = subjectConcept + " " + words[i] + " " + words[i + 1] + " " + words[i + 2];
                    i++;
                }
                else
                {
                    currentHumanStatement = subjectConcept + " " + words[i] + " " + words[i + 1];
                }
                humanStatementList.Add(currentHumanStatement);
            }

            return humanStatementList;
        }

        private string RemoveUselessAnd(string text)
        {
            bool success = true;
                
            while (text.RemoveWord("not").CountWord() % 2 != 1 && text.ContainsWord("and") && success)
                text = RemoveFirstUselessAnd(text, out success);
            
            return text;
        }

        private string RemoveFirstUselessAnd(string text, out bool success)
        {
            success = false;
            string newText = string.Empty;
            string[] words = text.Split(' ');

            string newWord;
            for (int i = 0; i < words.Length; i++)
            {
                newWord = " " + words[i];
                if (words[i] == "and")
                {
                    if (i + 2 < words.Length)
                    {
                        if (words[i + 1] != "and" && words[i + 2] != "and")
                        {
                            success = true;
                            newWord = "";
                        }
                    }
                }
                newText += newWord;
            }
            newText = newText.Trim();
            return newText;
        }
        #endregion
    }
}
