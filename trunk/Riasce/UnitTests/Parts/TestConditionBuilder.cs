using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestConditionBuilder
    {
        public static void Test()
        {
            Memory memory = new Memory();
            Memory.TotalVerbList = new HashSet<Concept>();
            NameMapper nameMapper = new NameMapper(new Name("AiName"), new Name("HumanName"));
            ConditionBuilder conditionBuilder = new ConditionBuilder();

            if (!conditionBuilder.ParseString("isa tree and madeof wood", nameMapper, memory).Equals(conditionBuilder.ParseString("madeof wood and isa tree", nameMapper, memory)))
                throw new Exception("Conditions should be equivalent");

            if (!conditionBuilder.ParseString("isa tree or madeof wood", nameMapper, memory).Equals(conditionBuilder.ParseString("madeof wood or isa tree", nameMapper, memory)))
                throw new Exception("Conditions should be equivalent");

            if (!conditionBuilder.ParseString("(isa tree) and (madeof wood)", nameMapper, memory).Equals(conditionBuilder.ParseString("(madeof wood and isa tree)", nameMapper, memory)))
                throw new Exception("Conditions should be equivalent");

            if (!conditionBuilder.ParseString("isa tree and madeof wood", nameMapper, memory).Equals(conditionBuilder.ParseString("((((madeof wood and isa tree))))", nameMapper, memory)))
                throw new Exception("Conditions should be equivalent");

            if (conditionBuilder.ParseString("isa tree and madeof wood", nameMapper, memory).Equals(conditionBuilder.ParseString("madeof wood and isa plant", nameMapper, memory)))
                throw new Exception("Conditions shouldn't be equivalent");

            if (conditionBuilder.ParseString("isa tree and madeof wood", nameMapper, memory).Equals(conditionBuilder.ParseString("madeof wood or isa tree", nameMapper, memory)))
                throw new Exception("Conditions shouldn't be equivalent");

            if (!conditionBuilder.ParseString("(isa plant or isa animal) and isa retard", nameMapper, memory).Equals(conditionBuilder.ParseString("isa retard and (isa animal or isa plant)", nameMapper, memory)))
                throw new Exception("Conditions should be equivalent");

            if (conditionBuilder.ParseString("isa tree and not madeof wood", nameMapper, memory).Equals(conditionBuilder.ParseString("madeof wood and not isa tree", nameMapper, memory)))
                throw new Exception("Conditions shouldn't be equivalent");
            
        }
    }
}
