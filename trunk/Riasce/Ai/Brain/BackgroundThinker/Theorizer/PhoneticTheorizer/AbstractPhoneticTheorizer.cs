using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractPhoneticTheorizer
    {
        /// <summary>
        /// Using statistic on occurence of phoneme in words, return a list of theory about concept to think about
        /// </summary>
        /// <param name="memory">memory to look into</param>
        /// <param name="nameMapper">name mapper to look into</param>
        /// <param name="subject">concept to think about</param>
        /// <returns>a list of theory about concept to think about</returns>
        public abstract List<Theory> GetRandomTheoryListAbout(Memory memory, NameMapper nameMapper, Concept subject);
    }
}
