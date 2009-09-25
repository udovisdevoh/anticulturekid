using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractInstinct : IEnumerable<string>
    {
        #region Methods
        public abstract IEnumerator<string> GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract int Count
        {
            get;
        }
        #endregion
    }
}
