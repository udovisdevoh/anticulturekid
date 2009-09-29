using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace AntiCulture.Kid
{
    /// <summary>
    /// AutoComplete controller
    /// </summary>
    class AutoComplete
    {
        #region Fields
        /// <summary>
        /// Model
        /// </summary>
        private AbstractAutoCompleteModel autoCompleteModel;

        /// <summary>
        /// View
        /// </summary>
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

        #region Public Methods
        /// <summary>
        /// Refresh content from memory and name mapper
        /// </summary>
        /// <param name="memory">memory</param>
        /// <param name="nameMapper">name mapper</param>
        public void RefreshFrom(Memory memory, NameMapper nameMapper)
        {
            autoCompleteModel.RefreshFrom(memory, nameMapper);
        }

        /// <summary>
        /// Try select autoComplete words
        /// </summary>
        /// <param name="word">begin with</param>
        /// <param name="bottomPosition">window bottom position</param>
        /// <param name="leftPosition">window left position</param>
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

        /// <summary>
        /// Hide autocomplete viewer
        /// </summary>
        public void HideViewer()
        {
            autoCompleteViewer.Hide();
        }

        /// <summary>
        /// Try move selection up
        /// </summary>
        public void TryMoveSelectionUp()
        {
            autoCompleteViewer.TryMoveSelectionUp();
            autoCompleteViewer.CenterSelection();
        }

        /// <summary>
        /// Try mode selection down
        /// </summary>
        public void TryMoveSelectionDown()
        {
            autoCompleteViewer.TryMoveSelectionDown();
            autoCompleteViewer.CenterSelection();
        }

        /// <summary>
        /// Get selection value
        /// </summary>
        /// <returns>selection value</returns>
        public string GetSelectionValue()
        {
            return autoCompleteModel.GetSelectionValue(autoCompleteViewer.GetSelectionKey());
        }

        /// <summary>
        /// Whether string matches one and ONLY one regex
        /// </summary>
        /// <param name="stringToMatch">string to match</param>
        /// <returns>whether string matches one and ONLY one regex</returns>
        public bool MatchesOneAndOnlyOneRegex(string stringToMatch)
        {
            return autoCompleteModel.MatchesOneAndOnlyOneRegex(stringToMatch);
        }

        /// <summary>
        /// Bring autoComplete viewer to front
        /// </summary>
        public void BringToFront()
        {
            autoCompleteViewer.Topmost = true;
        }
        
        /// <summary>
        /// Get current word that contains typing carret
        /// </summary>
        /// <param name="textBoxInput">text box to look into</param>
        /// <returns>word that contains typing carret</returns>
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

        /// <summary>
        /// Get starting position of word containing typing carret
        /// </summary>
        /// <param name="textBoxInput">text box to look into</param>
        /// <returns>starting position of word containing typing carret</returns>
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

        /// <summary>
        /// Get ending position of word containing typing carret
        /// </summary>
        /// <param name="textBoxInput">text box to look into</param>
        /// <returns>ending position of word containing typing carret</returns>
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
        /// <summary>
        /// Whether the autocomplete's viewer is visible
        /// </summary>
        public bool IsVisible
        {
            get { return autoCompleteViewer.IsVisible; }
        }
        #endregion
    }
}
