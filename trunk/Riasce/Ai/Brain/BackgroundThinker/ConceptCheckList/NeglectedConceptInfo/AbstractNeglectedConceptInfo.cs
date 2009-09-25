using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riasce
{
    abstract class AbstractNeglectedConceptInfo : IComparable<NeglectedConceptInfo>
    {
        public abstract int CompareTo(NeglectedConceptInfo other);
    }
}
