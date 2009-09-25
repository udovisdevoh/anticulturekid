using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractTheoryList : ICollection<Theory>
    {
        /// <summary>
        /// Remove the oldest postulated theory
        /// </summary>
        /// <returns>true if succeed, else false</returns>
        public abstract bool RemoveOldestTheory();

        /// <summary>
        /// Remove the least probable postulated theory
        /// </summary>
        /// <returns>true if succeed, else false</returns>
        public abstract bool RemoveLeastProbableTheory();

        public abstract void Sort();

        /// <summary>
        /// Get the most probable theory
        /// </summary>
        /// <param name="rejectedTheories">rejected theories</param>
        /// <returns>most probable theory</returns>
        public abstract Theory GetBestLogicTheory(RejectedTheories rejectedTheories);

        public abstract Theory GetBestLinguisticTheory(RejectedTheories rejectedTheories);

        public abstract Theory GetBestPhoneticTheory(RejectedTheories rejectedTheories);

        public abstract Theory GetBestMetaTheory(RejectedTheories rejectedTheories);

        #region ICollection<Theory> members
        public abstract void Add(Theory item);

        public abstract void Clear();

        public abstract bool Contains(Theory item);

        public void CopyTo(Theory[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public abstract int Count
        {
            get;
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public abstract bool Remove(Theory item);

        public abstract IEnumerator<Theory> GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
