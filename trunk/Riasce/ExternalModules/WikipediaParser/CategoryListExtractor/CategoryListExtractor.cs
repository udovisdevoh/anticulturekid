using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Category list extractor's controller
    /// </summary>
    class CategoryListExtractor
    {
        #region Fields
        /// <summary>
        /// Current file name
        /// </summary>
        private string fileName;

        /// <summary>
        /// Chunk size
        /// </summary>
        private int chunkSize = 5000;

        /// <summary>
        /// Category list extractor model
        /// </summary>
        private CategoryListExtractorModel categoryListExtractorModel = new CategoryListExtractorModel(new WikiCategoryListPage());

        /// <summary>
        /// Category list extractor viewer
        /// </summary>
        private CategoryListExtractorViewer categoryListExtractorViewer = new CategoryListExtractorViewer();

        /// <summary>
        /// Whether category extractor must stop
        /// </summary>
        private bool mustStop = false;
        #endregion

        #region Constructor
        public CategoryListExtractor()
        {
            categoryListExtractorViewer.UserCancelButtonClick += UserCancelButtonClickHandler;
        }
        #endregion

        #region Handlers
        private void UserCancelButtonClickHandler(object sender, EventArgs e)
        {
            mustStop = true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Show the window
        /// </summary>
        public void Show()
        {
            categoryListExtractorViewer.Show();
        }

        /// <summary>
        /// Start category list extractor subprogram
        /// </summary>
        public void Start()
        {
            while (categoryListExtractorModel.AppendNextChunkToFile(chunkSize, fileName) && !mustStop)
            {
                categoryListExtractorViewer.UpdateProgressBar(categoryListExtractorModel.GetProgressPercentage(fileName));
            }
            categoryListExtractorViewer.CloseWhenPossible();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Current file name
        /// </summary>
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }

        /// <summary>
        /// Chunk size
        /// </summary>
        public int ChunkSize
        {
            get
            {
                return chunkSize;
            }
            set
            {
                chunkSize = value;
            }
        }
        #endregion
    }
}