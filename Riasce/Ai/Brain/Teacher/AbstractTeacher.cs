using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractTeacher
    {
        /// <summary>
        /// Returns a random connection and its proof about subject concept
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns>random connection and proof about subject concept</returns>
        public abstract KeyValuePair<List<Concept>, Proof> TeachAbout(Concept subject);

        /// <summary>
        /// Returns a random connection and its proof about random concept from conceptCollection
        /// </summary>
        /// <param name="conceptCollection">conceptCollection</param>
        /// <returns>random connection and proof about random concept from conceptCollection</returns>
        public abstract KeyValuePair<List<Concept>, Proof> TeachAboutRandomConcept(IEnumerable<Concept> conceptCollection);
    }
}
