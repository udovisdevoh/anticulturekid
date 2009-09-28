using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace AntiCulture.Kid
{
    class AutoComplete
    {
        #region Fields
        private AbstractAutoCompleteModel autoCompleteModel;

        private AutoCompleteViewer autoCompleteViewer = new AutoCompleteViewer();
        #endregion

        #region Constructor
        public AutoComplete(DisambiguationNamer disambiguationNamer)
        {
            autoCompleteModel = new AutoCompleteModelBinarySearch(disambiguationNamer);
            autoCompleteViewer.UserDoubleClickItemView += UserDoubleClickItemViewHandler;
            autoCompleteViewer.UserMouseClickView += UserMouseClickViewHandler;
        }
        #endregion

        #region Methods
        public void RefreshFrom(Memory memory, NameMapper nameMapper)
        {
            autoCompleteModel.RefreshFrom(memory, nameMapper);
        }

        public void TrySelectBeginWith(string word, double bottomPosition, double leftPosition)
        {
            if (word == null)
                return;

            List<string> selectionList = autoCompleteModel.GetSelection(word);

            while (selectionList.Count < 1 && word.Length > 1)
            {
                if (word.EndsWith(")"))
                    word = word.Substring(0, word.Length - 1);
                else if (word.StartsWith("("))
                    word = word.Substring(1);
                else
                    break;

                selectionList = autoCompleteModel.GetSelection(word);
                if (selectionList.Count == 1 && (selectionList[0]+" ").Contains(word + " "))
                {
                    selectionList.Clear();
                    break;
                }
            }

            autoCompleteViewer.FillList(selectionList);

            autoCompleteViewer.SelectFirstWord();

            autoCompleteViewer.Top = bottomPosition;

            autoCompleteViewer.Left = leftPosition;

            if (selectionList.Count > 0)
                autoCompleteViewer.Show();
            else
                autoCompleteViewer.Hide();
        }

        public void HideViewer()
        {
            autoCompleteViewer.Hide();
        }

        public void TryMoveSelectionUp()
        {
            autoCompleteViewer.TryMoveSelectionUp();
            autoCompleteViewer.CenterSelection();
        }

        public void TryMoveSelectionDown()
        {
            autoCompleteViewer.TryMoveSelectionDown();
            autoCompleteViewer.CenterSelection();
        }

        public string GetSelectionValue()
        {
            return autoCompleteModel.GetSelectionValue(autoCompleteViewer.GetSelectionKey());
        }

        public bool MatchesOneAndOnlyOneRegex(string stringToMatch)
        {
            return autoCompleteModel.MatchesOneAndOnlyOneRegex(stringToMatch);
        }

        public void BringToFront()
        {
            autoCompleteViewer.Topmost = true;
        }

        public string GetWordContainingCarret(TextBox textBoxInput)
        {
            string wordContainingCarret = string.Empty;
            int letterCounter = 0;
            foreach (char letter in textBoxInput.Text)
            {
                if (letter == ' ')
                {
                    if (letterCounter >= textBoxInput.CaretIndex)
                    {
                        break;
                    }
                    else
                    {
                        wordContainingCarret = string.Empty;
                    }
                }
                wordContainingCarret += letter;
                letterCounter++;
            }

            wordContainingCarret = wordContainingCarret.Trim();

            if (wordContainingCarret == string.Empty)
                wordContainingCarret = null;

            return wordContainingCarret;
        }

        public int GetWordContainingCarretStartPosition(TextBox textBoxInput)
        {
            int letterCounter = 0;
            int lastSpacePosition = 0;
            foreach (char letter in textBoxInput.Text)
            {
                if (letter == ' ')
                {
                    if (letterCounter >= textBoxInput.CaretIndex)
                    {
                        return lastSpacePosition;
                    }
                    lastSpacePosition = letterCounter;
                }
                letterCounter++;
            }
            return lastSpacePosition;
        }

        public int GetWordContainingCarretEndPosition(TextBox textBoxInput)
        {
            int letterCounter = 0;
            foreach (char letter in textBoxInput.Text)
            {
                if (letter == ' ')
                {
                    if (letterCounter >= textBoxInput.CaretIndex)
                    {
                        break;
                    }
                }
                letterCounter++;
            }
            return letterCounter;
        }
        #endregion

        #region Events
        public event EventHandler UserDoubleClickItem;

        public event EventHandler UserMouseClick;
        #endregion

        #region Handlers
        public void UserDoubleClickItemViewHandler(object sender, EventArgs e)
        {
            if (UserDoubleClickItem != null) UserDoubleClickItem(this, e);
        }

        public void UserMouseClickViewHandler(object sender, EventArgs e)
        {
            if (UserMouseClick != null) UserMouseClick(this, e);
        }
        #endregion

        #region Properties
        public bool IsVisible
        {
            get { return autoCompleteViewer.IsVisible; }
        }
        #endregion
    }
}
