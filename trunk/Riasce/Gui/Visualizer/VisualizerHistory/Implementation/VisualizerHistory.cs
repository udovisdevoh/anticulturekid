using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class VisualizerHistory : AbstractVisualizerHistory
    {
        #region Fields
        private int index = 0;

        private List<string> nameList = new List<string>();

        private HashSet<string> checkList = new HashSet<string>();
        #endregion

        #region Public Methods
        public override void Push(string name)
        {
            if (!checkList.Contains(name))
            {
                checkList.Add(name);
                nameList.Add(name);
                index = nameList.Count - 1;
            }
        }

        public override bool CanPrevious()
        {
            return nameList.Count > 1 && index >= 1;
        }

        public override string GetPrevious()
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

        public override bool CanNext()
        {
            return index < nameList.Count - 1;
        }

        public override string GetNext()
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
