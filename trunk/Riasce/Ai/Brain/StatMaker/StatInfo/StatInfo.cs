using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class StatInfo
    {
        #region Fields
        private int denominatorVerbId;

        private int denominatorComplementId;

        private int numeratorVerbId;

        private int numeratorComplementId;

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
        public double Ratio
        {
            get
            {
                return ratio;
            }
        }

        public int DenominatorVerbId
        {
            get
            {
                return denominatorVerbId;
            }
        }

        public int DenominatorComplementId
        {
            get
            {
                return denominatorComplementId;
            }
        }

        public int NumeratorVerbId
        {
            get
            {
                return numeratorVerbId;
            }
        }

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
