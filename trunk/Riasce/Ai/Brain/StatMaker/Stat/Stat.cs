using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class Stat
    {
        #region Fields
        private List<Concept> expression;

        private double ratio;
        #endregion

        #region Constructor
        public Stat(Concept denominatorVerb, Concept denominatorComplement, Concept numeratorVerb, Concept numeratorComplement, double ratio)
        {
            expression = new List<Concept>() { denominatorVerb, denominatorComplement, numeratorVerb, numeratorComplement };
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

        public List<Concept> Expression
        {
            get
            {
                return expression;
            }
        }
        #endregion
    }
}
