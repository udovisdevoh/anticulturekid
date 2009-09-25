using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace AntiCulture.Kid
{
    abstract class AbstractColorMaker
    {
        /// <summary>
        /// Return a color from provided string
        /// </summary>
        /// <param name="text">provided string</param>
        /// <returns>color from provided string</returns>
        public abstract SolidColorBrush GetColorFromString(string text);

        /// <summary>
        /// Return a color from provided string
        /// </summary>
        /// <param name="text">provided string</param>
        /// <param name="minStrength">minimum channel strength (default: 0)</param>
        /// <param name="maxStrength">maximum channel strength (default: 255)</param>
        /// <returns>color from provided string</returns>
        public abstract SolidColorBrush GetColorFromString(string text, byte minStrength, byte maxStrength);

        /// <summary>
        /// Returns a gradient from provided string
        /// </summary>
        /// <param name="text">provided string</param>
        /// <param name="minStrength">minimum channel strength (default: 0)</param>
        /// <param name="maxStrength">maximum channel strength (default: 255)</param>
        /// <returns>gradient from provided string</returns>
        public abstract LinearGradientBrush GetLinearGradientFromString(string text, byte minStrength, byte maxStrength);

        /// <summary>
        /// Returns a gradient from provided string
        /// </summary>
        /// <param name="text">provided string</param>
        /// <param name="minStrength">minimum channel strength (default: 0)</param>
        /// <param name="maxStrength">maximum channel strength (default: 255)</param>
        /// <returns>gradient from provided string</returns>
        public abstract LinearGradientBrush GetWhiteToColorLinearGradientFromString(string text, byte minStrength, byte maxStrength);
    }
}
