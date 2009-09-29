using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestAssimilator
    {
        public static void Test()
        {
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept isa = new Concept("isa");

            Concept borg = new Concept("borg");
            Concept human = new Concept("human");

            Concept machine = new Concept("machine");
            Concept animal = new Concept("animal");

            Repairer.Repair(borg, machine);

            ConnectionManager.Plug(borg, isa, machine);
            ConnectionManager.Plug(human, isa, animal);

            Repairer.Repair(borg, machine, human, animal);

            //Pre-conditions

            if (!ConnectionManager.TestConnection(borg, isa, machine))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(human, isa, animal))
                throw new Exception("Should be connected because it's explicit");

            //Real test begins here

            Assimilator.Assimilate(borg, human);

            Repairer.Repair(borg, human);

            if (!ConnectionManager.TestConnection(borg, isa, machine))
                throw new Exception("Should be connected because it's explicit");

            if (!ConnectionManager.TestConnection(borg, isa, animal))
                throw new Exception("Should be connected because it's implicit");
        }
    }
}
