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
            ConnectionManager connectionManager = new ConnectionManager();
            Assimilator assimilator = new Assimilator(connectionManager);
            Repairer repairer = new Repairer();
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept isa = new Concept("isa");

            Concept borg = new Concept("borg");
            Concept human = new Concept("human");

            Concept machine = new Concept("machine");
            Concept animal = new Concept("animal");

            repairer.Repair(borg, machine);

            connectionManager.Plug(borg, isa, machine);
            connectionManager.Plug(human, isa, animal);

            repairer.Repair(borg, machine, human, animal);

            //Pre-conditions

            if (!connectionManager.TestConnection(borg, isa, machine))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(human, isa, animal))
                throw new Exception("Should be connected because it's explicit");

            //Real test begins here

            assimilator.Assimilate(borg, human);

            repairer.Repair(borg, human);

            if (!connectionManager.TestConnection(borg, isa, machine))
                throw new Exception("Should be connected because it's explicit");

            if (!connectionManager.TestConnection(borg, isa, animal))
                throw new Exception("Should be connected because it's implicit");
        }
    }
}
