﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Riasce
{
    class CategoryExtractor
    {
        #region Fields
        private CategoryExtractorModel categoryExtractorModel;

        private CategoryExtractorViewer categoryExtractorViewer;

        private HashSet<string> categoryElementNameList;

        private static readonly int BUTTON_MODE_WAITING_FOR_FILES = 1;

        private static readonly int BUTTON_MODE_WAITING_FOR_START = 2;

        private static readonly int BUTTON_MODE_STARTED = 3;
        #endregion

        #region Events
        public event EventHandler OnGeneralAffectation;
        #endregion

        #region Constructor
        public CategoryExtractor()
        {
            categoryExtractorModel = new CategoryExtractorModel();
            categoryExtractorViewer = new CategoryExtractorViewer();
            SetButtonEnabling(BUTTON_MODE_WAITING_FOR_FILES);
            categoryExtractorViewer.OnButtonSelectSourceFileClick += ButtonSelectSourceFileHandler;
            categoryExtractorViewer.OnButtonSelectCheckListFileClick += ButtonSelectCheckListFileHandler;
            categoryExtractorViewer.OnButtonStartClick += ButtonStartHandler;
            categoryExtractorViewer.OnButtonNextClick += ButtonNextHandler;
            categoryExtractorViewer.OnButtonPreviousClick += ButtonPreviousHandler;
            categoryExtractorViewer.OnButtonIgnoreClick += ButtonIgnoreHandler;
            categoryExtractorViewer.OnButtonApplyClick += ButtonApplyHandler;
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            categoryExtractorViewer.Show();
        }
        #endregion

        #region Private Methods
        private void ExtractCategory(string categoryName)
        {
            if (categoryName == null)
                return;

            categoryExtractorViewer.textBlockCategoryName.Text = categoryName;
            categoryElementNameList = categoryExtractorModel.GetCategoryElementNameList(categoryName);
            categoryExtractorViewer.ElementNameList = categoryElementNameList;
        }

        private void ExtractNextCategory()
        {
            string categoryName = categoryExtractorModel.GetNextCategoryName();

            if (categoryName != null)
            {
                categoryExtractorModel.CurrentCategoryName = categoryName;
                ExtractCategory(categoryExtractorModel.CurrentCategoryName);
            }
        }

        private void ExtractPreviousCategory()
        {
            string categoryName = categoryExtractorModel.GetPreviousCategoryName();

            if (categoryName != null)
            {
                categoryExtractorModel.CurrentCategoryName = categoryName;
                ExtractCategory(categoryExtractorModel.CurrentCategoryName);
            }
        }

        private void SetButtonEnabling(int buttonMode)
        {
            if (buttonMode == BUTTON_MODE_WAITING_FOR_FILES)
            {
                categoryExtractorViewer.buttonStart.IsEnabled = false;
                categoryExtractorViewer.buttonApply.IsEnabled = false;
                categoryExtractorViewer.buttonIgnore.IsEnabled = false;
                categoryExtractorViewer.buttonNext.IsEnabled = false;
                categoryExtractorViewer.buttonPrevious.IsEnabled = false;
            }
            else if (buttonMode == BUTTON_MODE_WAITING_FOR_START)
            {
                categoryExtractorViewer.buttonStart.IsEnabled = true;
                categoryExtractorViewer.buttonApply.IsEnabled = false;
                categoryExtractorViewer.buttonIgnore.IsEnabled = false;
                categoryExtractorViewer.buttonNext.IsEnabled = false;
                categoryExtractorViewer.buttonPrevious.IsEnabled = false;
            }
            else if (buttonMode == BUTTON_MODE_STARTED)
            {
                categoryExtractorViewer.buttonStart.IsEnabled = false;
                categoryExtractorViewer.buttonApply.IsEnabled = true;
                categoryExtractorViewer.buttonIgnore.IsEnabled = true;
                categoryExtractorViewer.buttonNext.IsEnabled = true;
                categoryExtractorViewer.buttonPrevious.IsEnabled = true;
            }
        }

        private void ApplyCurrentCategory()
        {
            categoryExtractorModel.MarkAsDone(categoryExtractorModel.CurrentCategoryName);
            if (OnGeneralAffectation != null) OnGeneralAffectation(this, new EventArgs());
            ExtractNextCategory();
        }

        private void IgnoreCurrentCategory()
        {
            categoryExtractorModel.MarkAsDone(categoryExtractorModel.CurrentCategoryName);
            ExtractNextCategory();
        }
        #endregion

        #region Handlers
        private void ButtonSelectSourceFileHandler(object source, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Sorted Category List|*.SortedCategoryList";
            dialog.ShowDialog();
            if (dialog.FileName != null && dialog.FileName != "")
            {
                if (dialog.FileName != categoryExtractorModel.SourceFileName)
                    SetButtonEnabling(BUTTON_MODE_WAITING_FOR_FILES);

                categoryExtractorModel.SourceFileName = dialog.FileName;
                string trimmedSourceFileName = categoryExtractorModel.SourceFileName;
                if (trimmedSourceFileName.Contains('\\'))
                    trimmedSourceFileName = trimmedSourceFileName.Substring(trimmedSourceFileName.LastIndexOf('\\') + 1);
                categoryExtractorViewer.textBlockSourceFile.Text = trimmedSourceFileName;
                if (categoryExtractorModel.CheckListFileName != null)
                    SetButtonEnabling(BUTTON_MODE_WAITING_FOR_START);
            }
        }

        private void ButtonSelectCheckListFileHandler(object source, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Checklist file|*.CategoryCheckList";
            dialog.OverwritePrompt = false;
            dialog.ShowDialog();
            if (dialog.FileName != null && dialog.FileName != "")
            {
                if (dialog.FileName != categoryExtractorModel.CheckListFileName)
                    SetButtonEnabling(BUTTON_MODE_WAITING_FOR_FILES);

                categoryExtractorModel.CheckListFileName = dialog.FileName;
                string trimmedFileName = categoryExtractorModel.CheckListFileName;
                if (trimmedFileName.Contains('\\'))
                    trimmedFileName = trimmedFileName.Substring(trimmedFileName.LastIndexOf('\\') + 1);
                categoryExtractorViewer.textBlockCheckListFile.Text = trimmedFileName;
                if (categoryExtractorModel.SourceFileName != null)
                    SetButtonEnabling(BUTTON_MODE_WAITING_FOR_START);
            }
        }

        private void ButtonStartHandler(object source, EventArgs e)
        {
            SetButtonEnabling(BUTTON_MODE_STARTED);
            ButtonNextHandler(source, e);
        }

        private void ButtonNextHandler(object source, EventArgs e)
        {
            ExtractNextCategory();
        }

        private void ButtonPreviousHandler(object source, EventArgs e)
        {
            ExtractPreviousCategory();
        }

        private void ButtonApplyHandler(object source, EventArgs e)
        {
            ApplyCurrentCategory();
        }

        private void ButtonIgnoreHandler(object source, EventArgs e)
        {
            IgnoreCurrentCategory();
        }
        #endregion

        #region Properties
        public HashSet<string> CategoryElementNameList
        {
            get { return categoryElementNameList; }
        }

        public string GeneralAffectation
        {
            get { return categoryExtractorViewer.textBoxGeneralAffectation.Text; }
        }
        #endregion

    }
}
