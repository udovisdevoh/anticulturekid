using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractVisualizerHistory
    {

        public abstract void Push(string previousConceptName);


        public abstract bool CanPrevious();


        public abstract string GetPrevious();


        public abstract bool CanNext();


        public abstract string GetNext();
    }
}
