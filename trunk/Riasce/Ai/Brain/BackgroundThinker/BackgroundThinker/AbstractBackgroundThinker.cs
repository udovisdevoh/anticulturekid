using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AntiCulture.Kid
{
    /// <summary>
    /// This class represents abstract thinkers and how they are used by other classes
    /// It is the common implementation of all possible background thinker
    /// Which is a composite class of all background procedures (theories, repairs)
    /// </summary>
    abstract class AbstractBackgroundThinker
    {
        #region Protected Fields
        /// <summary>
        /// True: the thinker must stop as soon as possible
        /// False: the thinker can continue
        /// </summary>
        protected bool mustStopNow;

        /// <summary>
        /// We keep a reference to current theory
        /// so human can discard it or validate it
        /// </summary>
        protected Theory currentTheory = null;

        /// <summary>
        /// We keep a reference to theories created by background thinker
        /// </summary>
        protected TheoryMemory theoryMemory = new TheoryMemory();

        /// <summary>
        /// We keep a reference to rejected theories
        /// </summary>
        protected RejectedTheories rejectedTheories;

        /// <summary>
        /// We need a reference to brain's memory
        /// </summary>
        protected Memory memory = null;

        /// <summary>
        /// We need a reference to Ai's name mapper
        /// to make linguistic and phonetic theories
        /// </summary>
        protected NameMapper nameMapper = null;

        /// <summary>
        /// We keep a reference to all trauma produced by
        /// purification. We will retrieve that when background
        /// thinker stops
        /// </summary>
        protected Stack<Trauma> traumaStack = new Stack<Trauma>();

        /// <summary>
        /// We use this to wait till the background thinker has finished stopping
        /// </summary>
        protected AutoResetEvent autoResetEvent = new AutoResetEvent(false);

        protected Theorizer theorizer;

        protected Random random = new Random();
        #endregion

        #region Public Abstract Methods
        /// <summary>
        /// Start making theories (thinking, metathinking), purifying(flat/optimize)
        /// </summary>
        public abstract void Start();
        #endregion

        #region Public Implementation Methods
        /// <summary>
        /// Stop thinking! the user wants to do something
        /// </summary>
        public void Stop()
        {
            mustStopNow = true;
        }

        /// <summary>
        /// Get best theory from background thinker
        /// </summary>
        /// <returns>best theory from background thinker</returns>
        public Theory GetBestTheory()
        {
            currentTheory = theoryMemory.GetBestLogicTheory(rejectedTheories,memory);
            return currentTheory;
        }

        /// <summary>
        /// Get best metaTheory from background thinker
        /// </summary>
        /// <returns>best metaTheory from background thinker</returns>
        public Theory GetBestMetaTheory()
        {
            currentTheory = theoryMemory.GetBestMetaTheory(rejectedTheories);
            return currentTheory;
        }

        /// <summary>
        /// Get best theory about provided subject concept
        /// </summary>
        /// <param name="subject">subject concept</param>
        /// <returns></returns>
        public Theory GetBestTheoryAbout(Concept subject)
        {
            currentTheory = theoryMemory.GetBestLogicTheoryAbout(subject, rejectedTheories, memory);
            return currentTheory;
        }

        /// <summary>
        /// Get best metaTheory about provided verb concept
        /// </summary>
        /// <param name="verb">verb concept</param>
        /// <returns>best metaTheory about provided verb concept</returns>
        public Theory GetBestMetaTheoryAbout(Concept verb)
        {
            currentTheory = theoryMemory.GetBestMetaTheoryAbout(verb, rejectedTheories);
            return currentTheory;
        }

        /// <summary>
        /// We discard most recently extracted theory
        /// </summary>
        public void DiscardCurrentTheory()
        {
            if (currentTheory == null)
            {
                FeelingMonitor.Add(FeelingMonitor.EMPTINESS);
                throw new TheoryException("Couldn't find the theory to reject");
            }
            else
            {
                rejectedTheories.Add(currentTheory);
                currentTheory = null;
            }
        }

        /// <summary>
        /// We get rejected theories to save them to a file
        /// </summary>
        /// <returns>rejected theories to save them to a file</returns>
        public RejectedTheories GetRejectedTheoriesToSave()
        {
            return this.rejectedTheories;
        }

        /// <summary>
        /// We force thinker to have references to rejected theories
        /// that come from loaded file
        /// </summary>
        /// <param name="rejectedTheories">rejected theories from loaded file</param>
        public void SetRejectedTheoriesToLoad(RejectedTheories rejectedTheories)
        {
            this.rejectedTheories.Assimilate(rejectedTheories);
        }

        /// <summary>
        /// Discard all rejected theories so they can be made again
        /// </summary>
        public void ResetRejectedTheories()
        {
            rejectedTheories.Clear();
        }

        /// <summary>
        /// Discard all theories made by background thinker
        /// </summary>
        public void ResetTheoryMemory()
        {
            theoryMemory.Reset();
        }

        /// <summary>
        /// Get best linguistic theory
        /// </summary>
        /// <returns>best linguistic theory</returns>
        public Theory GetBestLinguisticTheory()
        {
            currentTheory = theoryMemory.GetBestLinguisticTheory(rejectedTheories, memory);
            return currentTheory;
        }

        /// <summary>
        /// Get best phonetic theory
        /// </summary>
        /// <returns>best phonetic theory</returns>
        public Theory GetBestPhoneticTheory()
        {
            currentTheory = theoryMemory.GetBestPhoneticTheory(rejectedTheories, memory);
            return currentTheory;
        }

        /// <summary>
        /// Get best linguistic theory about provided concept
        /// </summary>
        /// <param name="subject">provided concept</param>
        /// <returns>best linguistic theory about provided concept</returns>
        public Theory GetBestLinguisticTheoryAbout(Concept subject)
        {
            currentTheory = theoryMemory.GetBestLinguisticTheoryAbout(subject, rejectedTheories, memory);
            return currentTheory;
        }

        /// <summary>
        /// Get best phonetic theory about provided concept
        /// </summary>
        /// <param name="subject">provided concept</param>
        /// <returns>best phonetic theory about provided concept</returns>
        public Theory GetBestPhoneticTheoryAbout(Concept subject)
        {
            currentTheory = theoryMemory.GetBestPhoneticTheoryAbout(subject, rejectedTheories, memory);
            return currentTheory;
        }

        /// <summary>
        /// Get list of theories made by background thinker to save them to a file
        /// </summary>
        /// <returns>list of theories made by background thinker to save them to a file</returns>
        public TheoryList GetTheoryListToSave()
        {
            return theoryMemory.TotalTheoryList;
        }

        /// <summary>
        /// Remove all theories that match connections that already exist
        /// </summary>
        public void RemoveExistingConnections()
        {
            theoryMemory.RemoveExistingConnections();
        }
        #endregion

        #region protected Implementation Methods
        protected void TryAddTrauma(Trauma trauma)
        {
            if (trauma == null)
                return;

            traumaStack.Push(trauma);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Latest theory told by background thinker
        /// </summary>
        public Theory CurrentTheory
        {
            get { return currentTheory; }
            set { currentTheory = value; }
        }

        /// <summary>
        /// Memory to look into
        /// </summary>
        public Memory Memory
        {
            get { return memory; }
            set { memory = value; }
        }

        /// <summary>
        /// Name Mapper to look into
        /// </summary>
        public NameMapper NameMapper
        {
            get { return nameMapper; }
            set { nameMapper = value; }
        }

        /// <summary>
        /// List of trauma we can access after background thinker
        /// has stopped. These trauma are produced by purification
        /// </summary>
        public Stack<Trauma> TraumaStack
        {
            get { return traumaStack; }
        }

        /// <summary>
        /// We use this to wait till the background thinker has finished stopping
        /// </summary>
        public AutoResetEvent AutoResetEvent
        {
            get { return autoResetEvent; }
        }
        #endregion
    }
}
