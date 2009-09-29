using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Visualizer's history
    /// </summary>
    class VisualizerHistory
    {
        #region Fields
        /// <summary>
        /// Position in history
        /// </summary>
        private int index = 0;

        /// <summary>
        /// Name list
        /// </summary>
        private List<string> nameList = new List<string>();

        /// <summary>
        /// Check list to make sure there is no double entry
        /// </summary>
        private HashSet<string> checkList = new HashSet<string>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Set previous concept
        /// </summary>
        /// <param name="previousConceptName">previous concept name</param>
        public void Push(string name)
        {
            if (!checkList.Contains(name))
            {
                checkList.Add(name);
                nameList.Add(name);
                index = nameList.Count - 1;
            }
        }

        /// <summary>
        /// Can get previous element or not
        /// </summary>
        /// <returns>Can get previous element or not</returns>
        public bool CanPrevious()
        {
            return nameList.Count > 1 && index >= 1;
        }

        /// <summary>
        /// Get previous concept from history
        /// </summary>
        /// <returns>previous concept from history</returns>
        public string GetPrevious()
        {
            if (CanPrevious())
            {
                index--;
                return nameList[index];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Can get next element or not
        /// </summary>
        /// <returns>Can get next element or not</returns>
        public bool CanNext()
        {
            return index < nameList.Count - 1;
        }

        /// <summary>
        /// Get next concept from history
        /// </summary>
        /// <returns>next concept from history</returns>
        public string GetNext()
        {
            if (CanNext())
            {
                index++;
                return nameList[index];
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
