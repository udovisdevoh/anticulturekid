using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestMemory
    {
        public static void Test()
        {
            Memory memory = new Memory();
            Concept apple = memory.GetOrCreateConcept(1);
            Concept apple2 = memory.GetOrCreateConcept(1);
            Concept grape = memory.GetOrCreateConcept(2);

            if (apple != apple2)
                throw new Exception("Concept should match");

            if (apple == grape)
                throw new Exception("Concept shouldn't match");

            if (memory.GetIdFromConcept(apple) != 1)
                throw new Exception("Concept ID should match");

            if (memory.GetIdFromConcept(grape) != 2)
                throw new Exception("Concept ID should match");
        }
    }
}
