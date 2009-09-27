using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Concept ID based statistic info
    /// </summary>
    class StatInfo
    {
        #region Fields
        /// <summary>
        /// Denominator verb id
        /// </summary>
        private int denominatorVerbId;

        /// <summary>
        /// Denominator complement id
        /// </summary>
        private int denominatorComplementId;

        /// <summary>
        /// Numerator verb id
        /// </summary>
        private int numeratorVerbId;

        /// <summary>
        /// Numerator complement id
        /// </summary>
        private int numeratorComplementId;

        /// <summary>
        /// Ratio (from 0 to 1)
        /// </summary>
        private double ratio;
        #endregion

        #region Constructor
        public StatInfo(int denominatorVerbId,int denominatorComplementId,int numeratorVerbId,int numeratorComplementId,double ratio)
        {
            this.denominatorVerbId = denominatorVerbId;
            this.denominatorComplementId = denominatorComplementId;
            this.numeratorVerbId = numeratorVerbId;
            this.numeratorComplementId = numeratorComplementId;
            this.ratio = ratio;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Ratio (from 0 to 1)
        /// </summary>
        public double Ratio
        {
            get
            {
                return ratio;
            }
        }

        /// <summary>
        /// Denominator verb id
        /// </summary>
        public int DenominatorVerbId
        {
            get
            {
                return denominatorVerbId;
            }
        }

        /// <summary>
        /// Denominator complement id
        /// </summary>
        public int DenominatorComplementId
        {
            get
            {
                return denominatorComplementId;
            }
        }

        /// <summary>
        /// Numerator verb id
        /// </summary>
        public int NumeratorVerbId
        {
            get
            {
                return numeratorVerbId;
            }
        }

        /// <summary>
        /// Numerator complement id
        /// </summary>
        public int NumeratorComplementId
        {
            get
            {
                return numeratorComplementId;
            }
        }
        #endregion
    }
}
