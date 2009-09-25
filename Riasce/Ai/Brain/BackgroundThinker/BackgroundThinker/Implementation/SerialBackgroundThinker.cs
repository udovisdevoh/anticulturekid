using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This abstract class represents a serial background thinker (no parallel programming)
    /// </summary>
    class SerialBackgroundThinker : AbstractBackgroundThinker
    {
        #region Constructors
        /// <summary>
        /// Create a serial background thinker
        /// </summary>
        /// <param name="purifier">purifier</param>
        /// <param name="theorizer">theorizer</param>
        /// <param name="repairer">repairer</param>
        /// <param name="rejectedTheories">rejected theories</param>
        public SerialBackgroundThinker(Purifier purifier, Theorizer theorizer, Repairer repairer, RejectedTheories rejectedTheories)
        {
            this.purifier = purifier;
            this.theorizer = theorizer;
            this.repairer = repairer;
            this.rejectedTheories = rejectedTheories;
        }
        #endregion

        #region Public Override Methods
        /// <summary>
        /// Start the serial background thinker
        /// </summary>
        public override void Start()
        {
            int taskTypeCounter = 0;

            if (memory == null)
                throw new Exception("Memory must be set before starting background thinker");

            if (nameMapper == null)
                throw new Exception("Name Mapper must be set before starting background thinker");

            repairedBranches = new HashSet<ConnectionBranch>();
            verbMetaConnectionCache = new VerbMetaConnectionCache();

            mustStopNow = false;

            Concept currentConcept;
            while (!mustStopNow)
            {
                currentConcept = memory.GetRandomItem();
                if (currentConcept == null)
                    break;

                if (taskTypeCounter == 0)
                {
                    repairer.Repair(currentConcept, repairedBranches, verbMetaConnectionCache);
                    TryAddTrauma(purifier.PurifyOptimized(currentConcept));
                }
                else if (taskTypeCounter == 1)
                {
                    repairer.Repair(currentConcept, repairedBranches, verbMetaConnectionCache);
                    TryAddTrauma(purifier.PurifyFlat(currentConcept));
                }
                else if (taskTypeCounter == 2)
                {
                    repairer.Repair(currentConcept, repairedBranches, verbMetaConnectionCache);
                    TheorizeAndRememberRandomTheoryAbout(currentConcept);
                }
                else //when no type is matched
                {
                    taskTypeCounter = -1;
                }

                taskTypeCounter++;
            }

            autoResetEvent.Set();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Theorize and remember random theory about concept
        /// </summary>
        /// <param name="conceptToThinkAbout">concept to think about</param>
        private void TheorizeAndRememberRandomTheoryAbout(Concept conceptToThinkAbout)
        {
            Theory theory = null;
            List<Theory> theoryList = null;
            if (Memory.TotalVerbList.Contains(conceptToThinkAbout))
            {
                int randomInt = random.Next(4);

                if (randomInt == 0)
                {
                    theoryList = theorizer.GetRandomConnectionTheoryListAbout(conceptToThinkAbout);
                    if (theoryList != null)
                    {
                        foreach (Theory currentTheory in theoryList)
                        {
                            theoryMemory.RememberNewTheory(currentTheory);
                        }
                    }
                }
                else if (randomInt == 1)
                {
                    theoryList = theorizer.GetRandomLinguisticTheoryListAbout(memory, nameMapper, conceptToThinkAbout);
                    if (theoryList != null)
                    {
                        foreach (Theory currentTheory in theoryList)
                        {
                            theoryMemory.RememberNewTheory(currentTheory);
                        }
                    }
                }
                else if (randomInt == 2)
                {
                    theoryList = theorizer.GetRandomPhoneticTheoryListAbout(memory, nameMapper, conceptToThinkAbout);
                    if (theoryList != null)
                    {
                        foreach (Theory currentTheory in theoryList)
                        {
                            theoryMemory.RememberNewTheory(currentTheory);
                        }
                    }
                }
                else
                {
                    theory = theorizer.GetRandomMetaConnectionTheoryAbout(conceptToThinkAbout);
                    theoryMemory.RememberNewTheory(theory);
                }
            }
            else
            {
                int randomInt = random.Next(3);

                if (randomInt == 0)
                {
                    theoryList = theorizer.GetRandomConnectionTheoryListAbout(conceptToThinkAbout);
                    if (theoryList != null)
                    {
                        foreach (Theory currentTheory in theoryList)
                        {
                            theoryMemory.RememberNewTheory(currentTheory);
                        }
                    }
                }
                else if (randomInt == 1)
                {
                    theoryList = theorizer.GetRandomPhoneticTheoryListAbout(memory, nameMapper, conceptToThinkAbout);
                    if (theoryList != null)
                    {
                        foreach (Theory currentTheory in theoryList)
                        {
                            theoryMemory.RememberNewTheory(currentTheory);
                        }
                    }
                }
                else
                {
                    theoryList = theorizer.GetRandomLinguisticTheoryListAbout(memory, nameMapper, conceptToThinkAbout);
                    if (theoryList != null)
                    {
                        foreach (Theory currentTheory in theoryList)
                        {
                            theoryMemory.RememberNewTheory(currentTheory);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
