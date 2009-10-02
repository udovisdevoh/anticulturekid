using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents the feeling monitor
    /// It monitors the Ai's state and logs stuff that happens
    /// </summary>
    class FeelingMonitor
    {
        #region Static Fields
        /// <summary>
        /// List of current feelings, (use constants)
        /// </summary>
        private static HashSet<int> feelingList = new HashSet<int>();

        /// <summary>
        /// Totology
        /// </summary>
        public static readonly int TOTOLOGY = 1;

        /// <summary>
        /// Purification
        /// </summary>
        public static readonly int PURIFICATION = 2;

        /// <summary>
        /// Flattenizing
        /// </summary>
        public static readonly int FLATTENIZING = 3;

        /// <summary>
        /// Optimizing
        /// </summary>
        public static readonly int OPTIMIZING = 4;

        /// <summary>
        /// Reciprocating
        /// </summary>
        public static readonly int RECIPROCATING = 5;

        /// <summary>
        /// Emptiness
        /// </summary>
        public static readonly int EMPTINESS = 6;

        /// <summary>
        /// Single side connection
        /// </summary>
        public static readonly int SINGLE_SIDE_CONNECTION = 7;

        /// <summary>
        /// Unlikely connection
        /// </summary>
        public static readonly int UNLIKELY_CONNECTION = 8;

        /// <summary>
        /// Connection exception
        /// </summary>
        public static readonly int CONNECTION_EXCEPTION = 9;

        /// <summary>
        /// Stop background thinker
        /// </summary>
        public static readonly int STOP_BACKGROUND_THINKER = 10;

        /// <summary>
        /// Start background thinker
        /// </summary>
        public static readonly int START_BACKGROUND_THINKER = 11;
        #endregion

        #region Methods
        /// <summary>
        /// Add feeling to feeling list
        /// </summary>
        /// <param name="feelingType">feeling type (use this class's constants)</param>
        public static void Add(int feelingType)
        {
            feelingList.Add(feelingType);
        }

        /// <summary>
        /// Get current feeling and reset feeling list
        /// </summary>
        /// <returns>current feeling</returns>
        public static HashSet<string> GetCurrentFeelingListAndReset()
        {
            HashSet<string> currentFeelingDescriptionList = new HashSet<string>();

            if (feelingList.Contains(STOP_BACKGROUND_THINKER))
            {
                currentFeelingDescriptionList.Add("I think about stuff");
            }
            else if (feelingList.Contains(START_BACKGROUND_THINKER))
            {
                currentFeelingDescriptionList.Add("I stop thinking about stuff");
            }
            else if (feelingList.Contains(PURIFICATION))
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
