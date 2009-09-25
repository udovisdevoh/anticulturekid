using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AntiCulture.Kid
{
    class CategoryListExtractor : AbstractCategoryListExtractor
    {
        #region Fields
        private string fileName;

        private int chunkSize = 5000;

        private CategoryListExtractorModel categoryListExtractorModel = new CategoryListExtractorModel(new WikiCategoryListPage());

        private CategoryListExtractorViewer categoryListExtractorViewer = new CategoryListExtractorViewer();

        private bool mustStop = false;
        #endregion

        #region Constructor
        public CategoryListExtractor()
        {
            categoryListExtractorViewer.UserCancelButtonClick += UserCancelButtonClickHandler;
        }
        #endregion

        #region Properties
        public override string FileName
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

        public override int ChunkSize
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

        #region Handlers
        private void UserCancelButtonClickHandler(object sender, EventArgs e)
        {
            mustStop = true;
        }
        #endregion

        #region Methods
        public override void Show()
        {
            categoryListExtractorViewer.Show();
        }

        public void Start()
        {
            while (categoryListExtractorModel.AppendNextChunkToFile(chunkSize, fileName) && !mustStop)
            {
                categoryListExtractorViewer.UpdateProgressBar(categoryListExtractorModel.GetProgressPercentage(fileName));
            }
            categoryListExtractorViewer.CloseWhenPossible();
        }
        #endregion
    }
}
