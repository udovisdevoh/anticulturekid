using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestInstinct
    {
        public static void Test()
        {
            AbstractInstinct instinct = new StandardInstinct();
            int counter = 0;
            foreach (string currentLine in instinct)
            {
                if (counter == 0 && currentLine != "pine isa tree = tree someare pine")
                    throw new Exception("Line from instinct memory doesn't match");
                counter++;
            }
        }
    }
}
