using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TheoryStatistics : AbstractTheoryStatistics
    {
        #region Fields
        private HashSet<TheoryInfo> theoryInfoList = new HashSet<TheoryInfo>();
        #endregion

        #region Methods
        public override IEnumerator<TheoryInfo> GetEnumerator()
        {
            return theoryInfoList.GetEnumerator();
        }
        #endregion

        public TheoryInfo GetOrCreateTheoryInfo(Concept subject, Concept verb, Concept complement)
        {
            foreach (TheoryInfo currentTheoryInfo in theoryInfoList)
                if (currentTheoryInfo.Subject == subject && currentTheoryInfo.Verb == verb && currentTheoryInfo.Complement == complement)
                    return currentTheoryInfo;

            TheoryInfo newTheoryInfo = new TheoryInfo(subject, verb, complement);
            theoryInfoList.Add(newTheoryInfo);
            return newTheoryInfo;
        }
    }
}
