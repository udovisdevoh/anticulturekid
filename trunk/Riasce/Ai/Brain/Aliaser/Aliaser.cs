using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class is used to create and remove concept name aliases
    /// </summary>
    class Aliaser
    {
        #region Fields
        /// <summary>
        /// Persistant reference to brain's memory
        /// </summary>
        private Memory memory;
        #endregion

        #region Concstructors
        public Aliaser(Memory memory)
        {
            this.memory = memory;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Add an alias from new concept to old concept so new points to old
        /// </summary>
        /// <param name="newConcept">new concept</param>
        /// <param name="oldConcept">old concept</param>
        public void AddAlias(Concept newConcept, Concept oldConcept)
        {
            if (Memory.TotalVerbList.Contains(newConcept) || Memory.TotalVerbList.Contains(oldConcept))
                throw new NameMappingException("Cannot alias/unalias operators");

            Repairer.RepairRange(memory);
            Assimilator.Assimilate(oldConcept, newConcept);
            Repairer.Repair(oldConcept, newConcept);
            Destroyer.Insulate(newConcept, memory);
            Repairer.Repair(oldConcept);
            Repairer.Reciprocate(oldConcept);
            Repairer.Repair(oldConcept);
        }

        /// <summary>
        /// Separate concept1 and concept2
        /// </summary>
        /// <param name="concept1">concept1</param>
        /// <param name="concept2">concept2</param>
        public void RemoveAlias(Concept oldConcept, Concept newConcept)
        {
            if (Memory.TotalVerbList.Contains(newConcept) || Memory.TotalVerbList.Contains(oldConcept))
                throw new NameMappingException("Cannot alias/unalias operators");

            Assimilator.Assimilate(newConcept, oldConcept);
            Repairer.Repair(newConcept, oldConcept);
            Repairer.Reciprocate(newConcept);
        }

        /// <summary>
        /// NEVER EVER USE THIS UNLESS YOU WANNA IMPORT FILE TO MEMORY
        /// </summary>
        /// <param name="memory">memory to import</param>
        public void SetMemoryToLoad(Memory memory)
        {
            this.memory = memory;
        }
        #endregion
    }
}
