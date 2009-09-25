using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestRejectedTheories
    {
        public static void Test()
        {
            RejectedTheories rejectedTheories = new RejectedTheories();
            Concept pine = new Concept("pine");
            Concept tree = new Concept("tree");
            Concept plant = new Concept("plant");
            Concept wood = new Concept("wood");

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            if (rejectedTheories.Contains(new Theory(0.5, pine, isa, tree)))
                throw new NotImplementedException("Shouldn't contain theory");

            rejectedTheories.Add(new Theory(0.5, pine, isa, tree));

            if (!rejectedTheories.Contains(new Theory(0.4, pine, isa, tree)))
                throw new NotImplementedException("Should contain theory");

            for (int i = 0; i < 60; i++)
                rejectedTheories.Add(new Theory(0.2, tree,isa,plant));

            if (rejectedTheories.Contains(new Theory(0.5, pine, isa, tree)))
                throw new NotImplementedException("Shouldn't contain theory anymore because it's more probable than the one in the for loop");
        }
    }
}
