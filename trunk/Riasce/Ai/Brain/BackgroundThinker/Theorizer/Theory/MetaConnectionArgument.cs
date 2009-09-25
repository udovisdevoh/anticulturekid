using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class MetaConnectionArgument : IEquatable<MetaConnectionArgument>
    {
        #region Fields
        private Concept operator1;

        private string metaOperator;

        private Concept operator2;
        #endregion

        #region Constructor
        public MetaConnectionArgument(Concept operator1, string metaOperator, Concept operator2)
        {
            this.operator1 = operator1;
            this.metaOperator = metaOperator;
            this.operator2 = operator2;
        }
        #endregion

        #region Methods
        public bool Equals(MetaConnectionArgument other)
        {
            if (this.operator1 == other.operator1 && this.operator2 == other.operator2 && this.metaOperator == other.metaOperator)
                return true;
            else
                return false;
        }
        #endregion

        #region Properties
        public Concept Operator1
        {
            get { return operator1; }
        }

        public string MetaOperator
        {
            get { return metaOperator; }
        }

        public Concept Operator2
        {
            get { return operator2; }
        }
        #endregion
    }
}
