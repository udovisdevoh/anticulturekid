﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Text;

namespace AntiCulture.Kid
{
    class ColorMaker : AbstractColorMaker
    {
        #region Public Methods
        public override SolidColorBrush GetColorFromString(string text)
        {
            return GetColorFromString(text, 0, 255);
        }

        public override SolidColorBrush GetColorFromString(string text, byte minStrength, byte maxStrength)
        {
            byte red, green, blue;

            red = GetChannelStrength(text, 4, 2, minStrength, maxStrength);
            green = GetChannelStrength(text, 2, 2, minStrength, maxStrength);
            blue = GetChannelStrength(text, 0, 2, minStrength, maxStrength);

            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(red, green, blue));
            return brush;
        }

        public override LinearGradientBrush GetLinearGradientFromString(string text, byte minStrength, byte maxStrength)
        {
            byte red, green, blue;
            Color startColor;
            Color endColor;
            double angle;

            red = GetChannelStrength(text, 2, 1, minStrength, maxStrength);
            green = GetChannelStrength(text, 1, 1, minStrength, maxStrength);
            blue = GetChannelStrength(text, 0, 1, minStrength, maxStrength);
            startColor = Color.FromRgb(red, green, blue);

            red = GetChannelStrength(text, 5, 1, minStrength, maxStrength);
            green = GetChannelStrength(text, 4, 1, minStrength, maxStrength);
            blue = GetChannelStrength(text, 3, 1, minStrength, maxStrength);
            endColor = Color.FromRgb(red, green, blue);

            angle = (double)(GetChannelStrength(text, 6, 1, 0, 90));

            LinearGradientBrush brush = new LinearGradientBrush(startColor, endColor, angle);

            return brush;
        }

        public override LinearGradientBrush GetWhiteToColorLinearGradientFromString(string text, byte minStrength, byte maxStrength)
        {
            byte red, green, blue;
            Color startColor = Colors.White;
            Color endColor;
            double angle = 0;

            red = GetChannelStrength(text, 4, 2, minStrength, maxStrength);
            green = GetChannelStrength(text, 2, 2, minStrength, maxStrength);
            blue = GetChannelStrength(text, 0, 2, minStrength, maxStrength);
            endColor = Color.FromRgb(red, green, blue);

            LinearGradientBrush brush = new LinearGradientBrush(startColor, endColor, angle);

            return brush;
        }
        #endregion

        #region Private methods
        private byte GetChannelStrength(string text, int fromChar, int length, byte minStrength, byte maxStrength)
        {
            text = text.FixStringForHimmlStatementParsing();
            text = text.ToLower();

            if (text.Length - fromChar < length)
                return minStrength;

            string chunk = text.Substring(fromChar, length);
            int maxPossibleValue = GetMaxPossibleValue(length);
            int currentValue = GetCurrentValue(chunk);

            if (currentValue > maxPossibleValue)
                currentValue = maxPossibleValue;
            else if (currentValue < 0)
                currentValue = 0;

            currentValue *= (maxStrength - minStrength);
            currentValue /= maxPossibleValue;
            currentValue += minStrength;

            if (currentValue < minStrength)
                currentValue = minStrength;
            else if (currentValue > maxStrength)
                currentValue = maxStrength;

            return (byte)(currentValue);
        }

        private int GetCurrentValue(string chunk)
        {
            int value = 0;
            int power = 1;
            foreach (char letter in chunk)
            {
                value += (int)(Math.Pow((double)(letter - 97), (double)(power)));
                power++;
            }

            return value;
        }

        private int GetMaxPossibleValue(int length)
        {
            return (int)(Math.Pow((double)(26), (double)(length)));
        }
        #endregion
    }
}