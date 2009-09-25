using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractStatMaker
    {
        /// <summary>
        /// From all concept having connection denominatorVerb denominatorComplement,
        /// returns stat object with ratio (from 0 to 1) of concept having also connection numeratorVerb numeratorComplement
        /// </summary>
        /// <param name="denominatorVerb">denominator verb</param>
        /// <param name="denominatorComplement">denominator complement</param>
        /// <param name="numeratorVerb">numerator verb</param>
        /// <param name="numeratorComplement">numerator complement</param>
        /// <returns>From all concept having connection denominatorVerb denominatorComplement,
        /// returns stat object with ratio (from 0 to 1) of concept having also connection numeratorVerb numeratorComplement</returns>
        public abstract Stat GetStatOn(Concept denominatorVerb, Concept denominatorComplement, Concept numeratorVerb, Concept numeratorComplement);

        /// <summary>
        /// From all concept having connection denominatorVerb denominatorComplement,
        /// returns stat object with ratio (from 0 to 1) of concept having also connection to random numeratorVerb numeratorComplement
        /// </summary>
        /// <param name="denominatorVerb">denominator verb</param>
        /// <param name="denominatorComplement">denominator complement</param>
        /// <returns>From all concept having connection denominatorVerb denominatorComplement,
        /// returns stat object with ratio (from 0 to 1) of concept having also connection to random numeratorVerb numeratorComplement</returns>
        public abstract Stat GetStatOn(Concept denominatorVerb, Concept denominatorComplement);

        /// <summary>
        /// Return a statistic on a random concept from concept collection
        /// </summary>
        /// <param name="conceptCollection">concept collection</param>
        /// <returns>a statistic on a random concept from concept collection</returns>
        public abstract Stat GetRandomStat(IEnumerable<Concept> conceptCollection);
    }
}
