using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class FeelingMonitor
    {
        #region Fields
        private static HashSet<int> feelingList = new HashSet<int>();

        public static readonly int TOTOLOGY = 1;

        public static readonly int PURIFICATION = 2;

        public static readonly int FLATTENIZING = 3;

        public static readonly int OPTIMIZING = 4;

        public static readonly int RECIPROCATING = 5;

        public static readonly int EMPTINESS = 6;

        public static readonly int SINGLE_SIDE_CONNECTION = 7;

        public static readonly int UNLIKELY_CONNECTION = 8;

        public static readonly int CONNECTION_EXCEPTION = 9;
        #endregion

        #region Methods
        public static void Add(int feelingType)
        {
            feelingList.Add(feelingType);
        }

        public static HashSet<string> GetCurrentFeelingListAndReset()
        {
            HashSet<string> currentFeelingDescriptionList = new HashSet<string>();

            if (feelingList.Contains(PURIFICATION))
            {
                currentFeelingDescriptionList.Add(":( I was traumatized by some nonsense connection that had to be removed, but not I'm ok! :|");
            }
            else if (feelingList.Contains(TOTOLOGY))
            {
                currentFeelingDescriptionList.Add(":( That statement made a totology in my head so I had to reject it. :/");
            }
            else if (feelingList.Contains(SINGLE_SIDE_CONNECTION))
            {
                currentFeelingDescriptionList.Add("Single sided connection detected. Some operator might have no permutable_side nor inverse_of metaConnection");
            }
            else if (feelingList.Contains(CONNECTION_EXCEPTION))
            {
                currentFeelingDescriptionList.Add("There was at least one connection that couldn't be altered probably because no concept can be plugged to itself");
            }
            else if (feelingList.Contains(RECIPROCATING))
            {
                currentFeelingDescriptionList.Add("Reciprocating connections ;)");
            }
            else if (feelingList.Contains(UNLIKELY_CONNECTION))
            {
                currentFeelingDescriptionList.Add("It seems rather unlikely but not entirely impossible...");
            }
            else if (feelingList.Contains(EMPTINESS))
            {
                currentFeelingDescriptionList.Add("-_- I feel so ignorant...");
            }
            else
            {
                currentFeelingDescriptionList.Add(":) Everything seems fine for now.");
            }

            feelingList = new HashSet<int>();
            return currentFeelingDescriptionList;
        }
        #endregion
    }
}
