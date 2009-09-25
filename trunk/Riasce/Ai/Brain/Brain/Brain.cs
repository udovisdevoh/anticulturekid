using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Application's main model
    /// This class represents the Ai's facade to concept rendering procedures
    /// and Ai's memory
    /// </summary>
    class Brain
    {
        #region Fields
        private Memory memory = new Memory();

        private ConnectionManager connectionManager = new ConnectionManager();

        private MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

        private ImplyConnectionManager implyConnectionManager = new ImplyConnectionManager();

        private Repairer repairer = new Repairer();

        private DefinitionMaker definitionMaker = new DefinitionMaker();

        private Teacher teacher = new Teacher();

        private Aliaser aliaser;

        private Asker asker;

        private Analogizer analogizer;

        private BrotherHoodManager brotherHoodManager;

        private DisambiguationNamer disambiguationNamer;

        private StatMaker statMaker;

        private AiSqlWrapper aiSqlWrapper;

        private ConditionBuilder conditionBuilder = new ConditionBuilder();

        private AbstractBackgroundThinker backgroundThinker;

        private Thread backgroundThinkerThread = null;

        private bool disableRepairAfterMetaConnection = false;
        #endregion

        #region Constructor
        public Brain()
        {
            aliaser = new Aliaser(memory,connectionManager, repairer);
            asker = new Asker(metaConnectionManager);
            brotherHoodManager = new BrotherHoodManager(metaConnectionManager);
            RejectedTheories rejectedTheories = new RejectedTheories();
            analogizer = new Analogizer(metaConnectionManager,brotherHoodManager);
            disambiguationNamer = new DisambiguationNamer(brotherHoodManager, repairer);
            statMaker = new StatMaker(metaConnectionManager);
            aiSqlWrapper = new AiSqlWrapper(metaConnectionManager);
            backgroundThinker = new SerialBackgroundThinker(new Purifier(repairer, connectionManager), new Theorizer(metaConnectionManager, connectionManager, brotherHoodManager, rejectedTheories), repairer, rejectedTheories);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Create a connection from subject to verb to complement
        /// </summary>
        /// <param name="subjectConceptId">subject concept id</param>
        /// <param name="verbConceptId">verb concept id</param>
        /// <param name="complementConceptId">complement concept id</param>
        /// <returns>Trauma (mostly be null, but sometimes not null)</returns>
        public Trauma TryAddConnection(int subjectConceptId, int verbConceptId, int complementConceptId)
        {
            Concept subject = memory.GetOrCreateConcept(subjectConceptId);
            Concept verb = memory.GetOrCreateConcept(verbConceptId);
            Concept complement = memory.GetOrCreateConcept(complementConceptId);
            Trauma trauma = null;

            if (!connectionManager.DisableFlattenizeAndOptimizeAndPurify)
                repairer.Repair(subject, verb, complement);

            List<Concept> obstruction = connectionManager.FindObstructionToPlug(subject,verb,complement,false);

            if (obstruction == null)
            {
                if (connectionManager.FindObstructionToPlug(subject, verb, complement, true) != null)
                    FeelingMonitor.Add(FeelingMonitor.UNLIKELY_CONNECTION);

                connectionManager.Plug(subject, verb, complement);

                if (!connectionManager.DisableFlattenizeAndOptimizeAndPurify)
                {
                    repairer.Repair(subject, verb, complement);    
                    #warning PurifyFlat is temporarly disabled
                    /*
                    trauma = purifier.PurifyFlat(subject);
                    repairer.Repair(subject, complement);
                    */
                }
            }
            return trauma;
        }

        /// <summary>
        /// Test a connection
        /// </summary>
        /// <param name="subjectConceptId">subject concept id</param>
        /// <param name="verbConceptId">verb concept id</param>
        /// <param name="complementConceptId">complement concept id</param>
        /// <returns>True if connection exist, else: false</returns>
        public bool TestConnection(int subjectConceptId, int verbConceptId, int complementConceptId)
        {
            Concept subject = memory.GetOrCreateConcept(subjectConceptId);
            Concept verb = memory.GetOrCreateConcept(verbConceptId);
            Concept complement = memory.GetOrCreateConcept(complementConceptId);

            if (!connectionManager.DisableFlattenizeAndOptimizeAndPurify)
                repairer.Repair(subject, verb, complement);

            if (connectionManager.TestConnection(subject, verb, complement))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Get the connection that prevents subject to be plugged to complement through verb
        /// </summary>
        /// <param name="subjectConceptId">subject concept id</param>
        /// <param name="verbConceptId">verb concept id</param>
        /// <param name="complementConceptId">complement concept id</param>
        /// <returns>Obstructing connection</returns>
        public List<int> FindObstructionToConnection(int subjectConceptId, int verbConceptId, int complementConceptId)
        {
            return FindObstructionToConnection(subjectConceptId, verbConceptId, complementConceptId, false);
        }

        /// <summary>
        /// Get the connection that prevents subject to be plugged to complement through verb
        /// </summary>
        /// <param name="subjectConceptId">subject concept id</param>
        /// <param name="verbConceptId">verb concept id</param>
        /// <param name="complementConceptId">complement concept id</param>
        /// <param name="strictMode">whether metaOperator "unlikely" is considered good enough to match obstruction</param>
        /// <returns>Obstructing connection</returns>
        public List<int> FindObstructionToConnection(int subjectConceptId, int verbConceptId, int complementConceptId, bool strictMode)
        {
            Concept subject = memory.GetOrCreateConcept(subjectConceptId);
            Concept verb = memory.GetOrCreateConcept(verbConceptId);
            Concept complement = memory.GetOrCreateConcept(complementConceptId);

            if (!connectionManager.DisableFlattenizeAndOptimizeAndPurify)
                repairer.Repair(subject, verb, complement);

            List<Concept> obstruction = connectionManager.FindObstructionToPlug(subject, verb, complement, strictMode);

            if (obstruction == null)
                return null;

            List<int> obstructionByInt = new List<int>();

            foreach (Concept concept in obstruction)
                obstructionByInt.Add(memory.GetIdFromConcept(concept));

            return obstructionByInt;
        }

        /// <summary>
        /// Remove a connection from subject to verb to complement
        /// </summary>
        /// <param name="subjectConceptId">subject concept id</param>
        /// <param name="verbConceptId">verb concept id</param>
        /// <param name="complementConceptId">complement concept id</param>
        public void TryRemoveConnection(int subjectConceptId, int verbConceptId, int complementConceptId)
        {
            Concept subject = memory.GetOrCreateConcept(subjectConceptId);
            Concept verb = memory.GetOrCreateConcept(verbConceptId);
            Concept complement = memory.GetOrCreateConcept(complementConceptId);

            if (!connectionManager.DisableFlattenizeAndOptimizeAndPurify)
                repairer.Repair(subject, verb, complement);
            
            connectionManager.UnPlug(subject, verb, complement);

            if (!connectionManager.DisableFlattenizeAndOptimizeAndPurify)
                repairer.Repair(subject, verb, complement);
        }

        /// <summary>
        /// Find a proof to connection
        /// </summary>
        /// <param name="subjectConceptId">subject concept id</param>
        /// <param name="verbConceptId">verb concept id</param>
        /// <param name="complementConceptId">complement concept id</param>
        /// <returns>Proof (arguments are concept ids)</returns>
        public List<List<int>> GetProofToConnection(int subjectConceptId, int verbConceptId, int complementConceptId)
        {
            Concept subject = memory.GetOrCreateConcept(subjectConceptId);
            Concept verb = memory.GetOrCreateConcept(verbConceptId);
            Concept complement = memory.GetOrCreateConcept(complementConceptId);

            if (!connectionManager.DisableFlattenizeAndOptimizeAndPurify)
                repairer.Repair(subject, verb, complement);

            Proof proof = connectionManager.GetProofToConnection(subject, verb, complement);

            return ProofToInt(proof);
        }

        /// <summary>
        /// Create a metaconnection 
        /// </summary>
        /// <param name="operator1Id">operator concept 1</param>
        /// <param name="metaOperatorName">meta connection name</param>
        /// <param name="operator2Id">operator concept 2</param>
        /// <returns>Trauma from new metaConnections</returns>
        public Trauma TryAddMetaConnection(int operator1Id, string metaOperatorName, int operator2Id)
        {
            Concept operator1 = memory.GetOrCreateConcept(operator1Id);
            Concept operator2 = memory.GetOrCreateConcept(operator2Id);
            TotologyException totologyException = null;
            Trauma trauma = null;

            metaConnectionManager.AddMetaConnection(operator1, metaOperatorName, operator2);

            if (disableRepairAfterMetaConnection == true)//Only when loading instinct!!!
                return null;

            try
            {
                repairer.RepairRange(memory);

            }
            catch (TotologyException e)
            {
                totologyException = e;
                FeelingMonitor.Add(FeelingMonitor.TOTOLOGY);
                metaConnectionManager.RemoveMetaConnection(operator1, metaOperatorName, operator2);
                repairer.RepairRange(memory);
            }
            finally
            {
                repairer.ReciprocateRange(memory);
                #warning Disabled purifyRange because it's done in BackgroundThinker
                //trauma = purifier.PurifyRangeOptimized(memory);
                repairer.RepairRange(memory);

                if (totologyException != null)
                {
                    throw totologyException;
                }
            }
            return trauma;
        }

        /// <summary>
        /// Remove a metaconnection
        /// </summary>
        /// <param name="operator1Id">operator concept 1</param>
        /// <param name="metaOperatorName">meta connection name</param>
        /// <param name="operator2Id">operator concept 2</param>
        public void TryRemoveMetaConnection(int operator1Id, string metaOperatorName, int operator2Id)
        {
            Concept operator1 = memory.GetOrCreateConcept(operator1Id);
            Concept operator2 = memory.GetOrCreateConcept(operator2Id);

            metaConnectionManager.RemoveMetaConnection(operator1, metaOperatorName, operator2);

            repairer.RepairRange(memory);
            repairer.ReciprocateRange(memory);
        }

        /// <summary>
        /// Returns true if MetaConnection exists
        /// </summary>
        /// <param name="operator1Id">operator concept 1</param>
        /// <param name="metaOperatorName">meta connection name</param>
        /// <param name="operator2Id">operator concept 2</param>
        /// <returns>true if MetaConnection exists, else: false</returns>
        public bool IsMetaConnected(int operator1Id, string metaOperatorName, int operator2Id)
        {
            Concept operator1 = memory.GetOrCreateConcept(operator1Id);
            Concept operator2 = memory.GetOrCreateConcept(operator2Id);

            return metaConnectionManager.IsFlatMetaConnected(operator1, metaOperatorName, operator2);
        }

        /// <summary>
        /// Returns the whatis definition of a concept
        /// </summary>
        /// <param name="conceptId">concept's id</param>
        /// <returns>whatis definition</returns>
        public Dictionary<int, List<int>> GetShortDefinition(int conceptId)
        {
            Concept concept = memory.GetOrCreateConcept(conceptId);

            repairer.Repair(concept);

            Dictionary<Concept, List<Concept>> definitionByConcept = definitionMaker.GetShortDefinition(concept);
            Dictionary<int, List<int>> definitionByInt = new Dictionary<int, List<int>>();

            foreach (KeyValuePair<Concept, List<Concept>> verbAndBranch in definitionByConcept)
            {
                List<int> branchInInt = new List<int>();
                foreach (Concept complement in verbAndBranch.Value)
                {
                    branchInInt.Add(memory.GetIdFromConcept(complement));
                }
                definitionByInt.Add(memory.GetIdFromConcept(verbAndBranch.Key), branchInInt);
            }
            return definitionByInt;
        }

        /// <summary>
        /// Returns the define definition of a concept
        /// </summary>
        /// <param name="conceptId">concept's id</param>
        /// <returns>define definition</returns>
        public Dictionary<int, List<int>> GetLongDefinition(int conceptId)
        {
            Concept concept = memory.GetOrCreateConcept(conceptId);

            repairer.Repair(concept);

            Dictionary<Concept, List<Concept>> definitionByConcept = definitionMaker.GetLongDefinition(concept);
            Dictionary<int, List<int>> definitionByInt = new Dictionary<int, List<int>>();

            foreach (KeyValuePair<Concept, List<Concept>> verbAndBranch in definitionByConcept)
            {
                List<int> branchInInt = new List<int>();
                foreach (Concept complement in verbAndBranch.Value)
                {
                    branchInInt.Add(memory.GetIdFromConcept(complement));
                }
                definitionByInt.Add(memory.GetIdFromConcept(verbAndBranch.Key), branchInInt);
            }
            return definitionByInt;
        }

        /// <summary>
        /// Returns a list of verb id that are connected to provided concept's id through metaOperator
        /// </summary>
        /// <param name="conceptId">concept's id</param>
        /// <param name="metaOperator">desired metaOperator's name</param>
        /// <returns>list of verb id that are connected to provided concept's id through metaOperator</returns>
        public List<int> GetOptimizedMetaOperationVerbIdList(int verbId, string metaOperator)
        {
            List<int> flatMetaOperationVerbIdList = new List<int>();
            Concept verb = memory.GetOrCreateConcept(verbId);
            HashSet<Concept> farVerbList = verb.MetaConnectionTreePositive.GetAffectedOperatorsByMetaConnection(metaOperator);

            foreach (Concept farVerb in farVerbList)
                flatMetaOperationVerbIdList.Add(memory.GetIdFromConcept(farVerb));

            return flatMetaOperationVerbIdList;
        }

        /// <summary>
        /// Returns a list of verb id that are connected to provided concept's id through metaOperator
        /// </summary>
        /// <param name="conceptId">concept's id</param>
        /// <param name="metaOperator">desired metaOperator's name</param>
        /// <returns>list of verb id that are connected to provided concept's id through metaOperator</returns>
        public List<int> GetFlatMetaOperationVerbIdList(int verbId, string metaOperator)
        {
            List<int> flatMetaOperationVerbIdList = new List<int>();
            Concept verb = memory.GetOrCreateConcept(verbId);
            HashSet<Concept> farVerbList = metaConnectionManager.GetVerbFlatListFromMetaConnection(verb, metaOperator,true);

            foreach (Concept farVerb in farVerbList)
                flatMetaOperationVerbIdList.Add(memory.GetIdFromConcept(farVerb));

            return flatMetaOperationVerbIdList;
        }

        /// <summary>
        /// Returns a connection by id and a proof by id about conceptId
        /// </summary>
        /// <param name="conceptId">concept's id</param>
        /// <returns>connection by id and a proof by id about conceptId</returns>
        public KeyValuePair<List<int>, List<List<int>>> TeachAbout(int conceptId)
        {
            HashSet<string> ignoreList = new HashSet<string>();

            Concept concept = memory.GetOrCreateConcept(conceptId);

            KeyValuePair<List<Concept>,Proof> connectionAndProof = teacher.TeachAbout(concept);

            List<int> connectionByInt = new List<int>();
            List<List<int>> proofByInt = new List<List<int>>();

            foreach (Concept subjectVerbOrComplement in connectionAndProof.Key)
                connectionByInt.Add(memory.GetIdFromConcept(subjectVerbOrComplement));

            proofByInt = GetProofToConnection(connectionByInt[0], connectionByInt[1], connectionByInt[2]);
                
            return new KeyValuePair<List<int>, List<List<int>>>(connectionByInt, proofByInt);
        }

        /// <summary>
        /// Returns a connection by id and a proof by id about a random concept
        /// </summary>
        /// <returns>connection by id and a proof by id about a random concept</returns>
        public KeyValuePair<List<int>, List<List<int>>> TeachAboutRandomConcept()
        {
            HashSet<string> ignoreList = new HashSet<string>();

            KeyValuePair<List<Concept>, Proof> connectionAndProof = teacher.TeachAboutRandomConcept(memory);

            List<int> connectionByInt = new List<int>();
            List<List<int>> proofByInt = new List<List<int>>();

            foreach (Concept subjectVerbOrComplement in connectionAndProof.Key)
                connectionByInt.Add(memory.GetIdFromConcept(subjectVerbOrComplement));

            proofByInt = GetProofToConnection(connectionByInt[0], connectionByInt[1], connectionByInt[2]);

            return new KeyValuePair<List<int>, List<List<int>>>(connectionByInt, proofByInt);
        }

        /// <summary>
        /// Returns the best question about concept for which ID is conceptId
        /// </summary>
        /// <param name="conceptId">concept's id</param>
        /// <returns>best question about concept for which ID is conceptId</returns>
        public List<int> GetBestQuestionAbout(int conceptId)
        {
            Concept concept = memory.GetOrCreateConcept(conceptId);
            List<Concept> question = asker.GetBestQuestionAbout(concept);
            repairer.Repair(concept);
            List<int> questionById = new List<int>();
            questionById.Add(memory.GetIdFromConcept(question[0]));
            questionById.Add(memory.GetIdFromConcept(question[1]));
            return questionById;
        }

        /// <summary>
        /// Returns the best question about a random concept
        /// </summary>
        /// <returns>best question about a random concept</returns>
        public List<int> GetBestQuestionAboutRandomConcept()
        {
            repairer.RepairRange(memory);
            List<Concept> question = asker.GetBestQuestionAboutRandomConcept(memory);
            List<int> questionById = new List<int>();
            questionById.Add(memory.GetIdFromConcept(question[0]));
            questionById.Add(memory.GetIdFromConcept(question[1]));
            return questionById;
        }

        /// <summary>
        /// Adds an alias from new concept id to old concept id
        /// </summary>
        /// <param name="newConceptId">new concept id</param>
        /// <param name="oldConceptId">old concept id</param>
        public void AddAlias(int newConceptId, int oldConceptId)
        {
            if (newConceptId == oldConceptId)
                throw new NameMappingException("Cannot alias a concept to itself");

            Concept newConcept = memory.GetOrCreateConcept(newConceptId);
            Concept oldConcept = memory.GetOrCreateConcept(oldConceptId);
            repairer.Repair(newConcept, oldConcept);
            aliaser.AddAlias(newConcept, oldConcept);
            
            #warning Might cause problem, if so, reverse comments
            //repairer.Repair(newConcept, oldConcept);
            repairer.Repair(oldConcept);
        }

        /// <summary>
        /// UnAlias these concepts
        /// </summary>
        /// <param name="concept1">first concept's id</param>
        /// <param name="concept2">second concept's id</param>
        public void RemoveAlias(int concept1id, int concept2id)
        {
            Concept concept1 = memory.GetOrCreateConcept(concept1id);
            Concept concept2 = memory.GetOrCreateConcept(concept2id);
            repairer.Repair(concept1, concept2);
            aliaser.RemoveAlias(concept1, concept2);
            repairer.Repair(concept1, concept2);
        }

        /// <summary>
        /// Return the best connection theory about concept using statistical inference on existing connections
        /// </summary>
        /// <param name="conceptId">concept's id</param>
        /// <param name="nameMapper">name mapper</param>
        /// <returns>the best connection theory about concept</returns>
        public List<string> GetBestTheoryAboutConcept(int conceptId, NameMapper nameMapper)
        {
            Concept subject = memory.GetOrCreateConcept(conceptId);
            Theory bestTheory = backgroundThinker.GetBestTheoryAbout(subject);
            return bestTheory.ToStringList(nameMapper, memory);
        }

        /// <summary>
        /// Return the best connection theory about concept using contextual semantic proximity
        /// </summary>
        /// <param name="conceptId">concept's id</param>
        /// <param name="nameMapper">name mapper</param>
        /// <returns>the best connection theory about concept</returns>
        public List<string> GetBestLinguisticTheoryAboutConcept(int conceptId, NameMapper nameMapper)
        {
            Concept subject = memory.GetOrCreateConcept(conceptId);
            Theory bestTheory = backgroundThinker.GetBestLinguisticTheoryAbout(subject);

            List<string> stringTheory = bestTheory.ToStringList(nameMapper, memory);

            if (stringTheory != null)
            {
                string[] argument0Words = stringTheory[0].Split(' ');
                string[] argument1Words = stringTheory[1].Split(' ');
                stringTheory.Insert(1, argument0Words[0] + " similar_context " + argument1Words[0]);
            }

            return stringTheory;
        }

        /// <summary>
        /// Return the best connection theory about concept using concept name phonology
        /// </summary>
        /// <param name="conceptId">concept's id</param>
        /// <param name="nameMapper">name mapper</param>
        /// <returns>the best connection theory about concept</returns>
        public List<string> GetBestPhoneticTheoryAboutConcept(int conceptId, NameMapper nameMapper)
        {
            Concept subject = memory.GetOrCreateConcept(conceptId);
            Theory bestTheory = backgroundThinker.GetBestPhoneticTheoryAbout(subject);

            List<string> stringTheory = bestTheory.ToStringList(nameMapper, memory);

            if (stringTheory != null)
            {
                string[] argument0Words = stringTheory[0].Split(' ');
                string[] argument1Words = stringTheory[1].Split(' ');
                stringTheory.Insert(1, argument0Words[0] + " similar_sounding " + argument1Words[0]);
            }

            return stringTheory;
        }

        /// <summary>
        /// Return the best metaConnection theory about operator
        /// </summary>
        /// <param name="conceptId">concept's id</param>
        /// <param name="nameMapper">name mapper</param>
        /// <returns>the best metaConnection theory about operator</returns>
        public List<string> GetBestTheoryAboutOperator(int conceptId, NameMapper nameMapper)
        {
            Concept subject = memory.GetOrCreateConcept(conceptId);
            Theory bestTheory = backgroundThinker.GetBestMetaTheoryAbout(subject);
            return bestTheory.ToStringList(nameMapper, memory);
        }

        /// <summary>
        /// Return the best connection theory about random concept using statistical inference on existing connections
        /// </summary>
        /// <param name="nameMapper">name mapper</param>
        /// <returns>best connection theory about random concept</returns>
        public List<string> GetBestTheoryAboutRandomConcept(NameMapper nameMapper)
        {
            Theory bestTheory = backgroundThinker.GetBestTheory();
            return bestTheory.ToStringList(nameMapper, memory);
        }

        /// <summary>
        /// Return the best connection theory about random concept using contextual semantic proximity
        /// </summary>
        /// <param name="nameMapper">name mapper</param>
        /// <returns>best connection theory about random concept</returns>
        public List<string> GetBestLinguisticTheoryAboutRandomConcept(NameMapper nameMapper)
        {
            Theory bestTheory = backgroundThinker.GetBestLinguisticTheory();

            List<string> stringTheory = bestTheory.ToStringList(nameMapper, memory);

            if (stringTheory != null)
            {
                string[] argument0Words = stringTheory[0].Split(' ');
                string[] argument1Words = stringTheory[1].Split(' ');
                stringTheory.Insert(1, argument0Words[0] + " similar_context " + argument1Words[0]);
            }

            return stringTheory;
        }

        /// <summary>
        /// Return the best connection theory about random concept using concept name phonology
        /// </summary>
        /// <param name="nameMapper">name mapper</param>
        /// <returns>best connection theory about random concept</returns>
        public List<string> GetBestPhoneticTheoryAboutRandomConcept(NameMapper nameMapper)
        {
            Theory bestTheory = backgroundThinker.GetBestPhoneticTheory();

            List<string> stringTheory = bestTheory.ToStringList(nameMapper, memory);

            if (stringTheory != null)
            {
                string[] argument0Words = stringTheory[0].Split(' ');
                string[] argument1Words = stringTheory[1].Split(' ');
                stringTheory.Insert(1, argument0Words[0] + " similar_sounding " + argument1Words[0]);
            }

            return stringTheory;
        }

        /// <summary>
        /// Return the best connection metaConnection theory about random operator
        /// </summary>
        /// <param name="nameMapper">name mapper</param>
        /// <returns>best connection metaConnection theory about random operator</returns>
        public List<string> GetBestTheoryAboutRandomOperator(NameMapper nameMapper)
        {
            Theory bestTheory = backgroundThinker.GetBestMetaTheory();
            return bestTheory.ToStringList(nameMapper, memory);
        }

        /// <summary>
        /// Return the best analogy
        /// </summary>
        /// <returns>the best analogy</returns>
        public List<int> GetAnalogy()
        {
            repairer.RepairRange(memory);

            Analogy analogy = analogizer.GetBestRandomAnalogy(memory);

            if (analogy == null)
                return null;

            List<int> analogyByInt = new List<int>();

            analogyByInt.Add(memory.GetIdFromConcept(analogy.InputSubject));
            analogyByInt.Add(memory.GetIdFromConcept(analogy.InputVerb));
            analogyByInt.Add(memory.GetIdFromConcept(analogy.InputComplement));

            analogyByInt.Add(memory.GetIdFromConcept(analogy.OutputSubject));
            analogyByInt.Add(memory.GetIdFromConcept(analogy.OutputVerb));
            analogyByInt.Add(memory.GetIdFromConcept(analogy.OutputComplement));

            return analogyByInt;
        }

        /// <summary>
        /// Return the best analogy
        /// </summary>
        /// <param name="subjectId">subject's id</param>
        /// <returns>the best analogy</returns>
        public List<int> GetAnalogy(int subjectId)
        {
            repairer.RepairRange(memory);

            Concept subject = memory.GetOrCreateConcept(subjectId);

            Analogy analogy = analogizer.GetAnalogyOnSubject(subject);

            if (analogy == null)
                return null;

            List<int> analogyByInt = new List<int>();

            analogyByInt.Add(memory.GetIdFromConcept(analogy.InputSubject));
            analogyByInt.Add(memory.GetIdFromConcept(analogy.InputVerb));
            analogyByInt.Add(memory.GetIdFromConcept(analogy.InputComplement));

            analogyByInt.Add(memory.GetIdFromConcept(analogy.OutputSubject));
            analogyByInt.Add(memory.GetIdFromConcept(analogy.OutputVerb));
            analogyByInt.Add(memory.GetIdFromConcept(analogy.OutputComplement));

            return analogyByInt;
        }

        /// <summary>
        /// Return the best analogy
        /// </summary>
        /// <param name="subjectId">subject's id</param>
        /// <param name="verbId">verb's id</param>
        /// <returns>the best analogy</returns>
        public List<int> GetAnalogy(int subjectId, int verbId)
        {
            repairer.RepairRange(memory);

            Concept subject = memory.GetOrCreateConcept(subjectId);
            Concept verb = memory.GetOrCreateConcept(verbId);

            Analogy analogy = analogizer.GetAnalogyOnSubjectVerb(subject, verb);

            if (analogy == null)
                return null;

            List<int> analogyByInt = new List<int>();

            analogyByInt.Add(memory.GetIdFromConcept(analogy.InputSubject));
            analogyByInt.Add(memory.GetIdFromConcept(analogy.InputVerb));
            analogyByInt.Add(memory.GetIdFromConcept(analogy.InputComplement));

            analogyByInt.Add(memory.GetIdFromConcept(analogy.OutputSubject));
            analogyByInt.Add(memory.GetIdFromConcept(analogy.OutputVerb));
            analogyByInt.Add(memory.GetIdFromConcept(analogy.OutputComplement));

            return analogyByInt;
        }

        /// <summary>
        /// Return the best analogy
        /// </summary>
        /// <param name="subjectId">subject's id</param>
        /// <param name="verbId">verb's id</param>
        /// <param name="complementId">complement's id</param>
        /// <returns>the best analogy</returns>
        public List<int> GetAnalogy(int subjectId, int verbId, int complementId)
        {
            repairer.RepairRange(memory);

            Concept subject = memory.GetOrCreateConcept(subjectId);
            Concept verb = memory.GetOrCreateConcept(verbId);
            Concept complement = memory.GetOrCreateConcept(complementId);

            Analogy analogy = analogizer.GetAnalogyOnSubjectVerbComplement(subject, verb, complement);

            if (analogy == null)
                return null;

            List<int> analogyByInt = new List<int>();

            analogyByInt.Add(memory.GetIdFromConcept(analogy.InputSubject));
            analogyByInt.Add(memory.GetIdFromConcept(analogy.InputVerb));
            analogyByInt.Add(memory.GetIdFromConcept(analogy.InputComplement));

            analogyByInt.Add(memory.GetIdFromConcept(analogy.OutputSubject));
            analogyByInt.Add(memory.GetIdFromConcept(analogy.OutputVerb));
            analogyByInt.Add(memory.GetIdFromConcept(analogy.OutputComplement));

            return analogyByInt;
        }

        /// <summary>
        /// Add current theory to ignore list
        /// </summary>
        public void DiscardCurrentTheory()
        {
            backgroundThinker.DiscardCurrentTheory();
        }

        /// <summary>
        /// Wihtout discarding it, just forget current theory for now
        /// </summary>
        public void ForgetCurrentTheory()
        {
            backgroundThinker.CurrentTheory = null;
            backgroundThinker.CurrentTheory = null;
        }

        /// <summary>
        /// Return the list of concept id from a trauma (connection and obstruction)
        /// </summary>
        /// <param name="trauma">trauma</param>
        /// <returns>list of concept id from a trauma</returns>
        public List<int> GetTraumaByInt(Trauma trauma)
        {
            List<int> traumaByInt = new List<int>();
            foreach (List<Concept> connection in trauma)
                foreach (Concept concept in connection)
                    traumaByInt.Add(memory.GetIdFromConcept(concept));

            return traumaByInt;
        }

        /// <summary>
        /// Convert theory object to statement object
        /// </summary>
        /// <param name="currentTheory">provided theory</param>
        /// <param name="nameMapper">name mapper to look into</param>
        /// <returns>statement from theory</returns>
        public Statement GetStatementFromTheory(Theory currentTheory, NameMapper nameMapper)
        {
            return currentTheory.ToStatement(nameMapper, memory);
        }

        /// <summary>
        /// Clear the rejected theory list
        /// </summary>
        public void ResetRejectedTheories()
        {
            backgroundThinker.ResetRejectedTheories();
        }

        /// <summary>
        /// Convert a proof to a list of list of int
        /// </summary>
        /// <param name="proof">provided proof</param>
        /// <returns>proof as a list of list of int (as concept ids)</returns>
        private List<List<int>> ProofToInt(Proof proof)
        {
            HashSet<string> ignoreList = new HashSet<string>();
            List<List<int>> proofInInt = new List<List<int>>();
            List<int> argumentInInt;
            foreach (List<Concept> argument in proof)
            {
                argumentInInt = new List<int>();
                argumentInInt.Add(memory.GetIdFromConcept(argument[0]));
                argumentInInt.Add(memory.GetIdFromConcept(argument[1]));
                argumentInInt.Add(memory.GetIdFromConcept(argument[2]));

                if (!ignoreList.Contains(argumentInInt[0] + "-" + argumentInInt[1] + "-" + argumentInInt[2]))
                {
                    proofInInt.Add(argumentInInt);
                    ignoreList.Add(argumentInInt[0] + "-" + argumentInInt[1] + "-" + argumentInInt[2]);
                }
            }

            return proofInInt;
        }

        /// <summary>
        /// Return statistics on quaternary expression (0 to 1 probability)
        /// </summary>
        /// <param name="denominatorVerbId">denominator verb id</param>
        /// <param name="denominatorComplementId">denominator complement id</param>
        /// <param name="numeratorVerbId">numerator verb id</param>
        /// <param name="numeratorComplementId">numerator complement id</param>
        /// <returns>statistics on quaternary expression (0 to 1 probability)</returns>
        public StatInfo GetStatOn(int denominatorVerbId, int denominatorComplementId, int numeratorVerbId, int numeratorComplementId)
        {
            Concept denominatorVerb = memory.GetOrCreateConcept(denominatorVerbId);
            Concept denominatorComplement = memory.GetOrCreateConcept(denominatorComplementId);
            Concept numeratorVerb = memory.GetOrCreateConcept(numeratorVerbId);
            Concept numeratorComplement = memory.GetOrCreateConcept(numeratorComplementId);

            repairer.Repair(denominatorVerb, denominatorComplement, numeratorVerb, numeratorComplement);

            Stat stat = statMaker.GetStatOn(denominatorVerb, denominatorComplement, numeratorVerb, numeratorComplement);

            return new StatInfo(denominatorVerbId, denominatorComplementId, numeratorVerbId, numeratorComplementId, stat.Ratio);
        }

        /// <summary>
        /// Return statistics on quaternary expression (0 to 1 probability)
        /// </summary>
        /// <param name="denominatorVerbId">denominator verb id</param>
        /// <param name="denominatorComplementId">denominator complement id</param>
        /// <returns>statistics on quaternary expression (0 to 1 probability)</returns>
        public StatInfo GetStatOn(int denominatorVerbId, int denominatorComplementId)
        {
            Concept denominatorVerb = memory.GetOrCreateConcept(denominatorVerbId);
            Concept denominatorComplement = memory.GetOrCreateConcept(denominatorComplementId);
            Concept numeratorVerb;
            Concept numeratorComplement;

            repairer.RepairRange(memory);

            Stat stat = statMaker.GetStatOn(denominatorVerb, denominatorComplement);

            numeratorVerb = stat.Expression[2];
            numeratorComplement = stat.Expression[3];

            int numeratorVerbId = memory.GetIdFromConcept(numeratorVerb);
            int numeratorComplementId = memory.GetIdFromConcept(numeratorComplement);

            return new StatInfo(denominatorVerbId, denominatorComplementId, numeratorVerbId, numeratorComplementId, stat.Ratio);
        }

        /// <summary>
        /// Returns a random statistic info object on a random concept from memory
        /// </summary>
        /// <returns>a random statistic info object on a random concept from memory</returns>
        public StatInfo GetRandomStat()
        {
            Concept denominatorVerb;
            Concept denominatorComplement;
            Concept numeratorVerb;
            Concept numeratorComplement;

            repairer.RepairRange(memory);

            Stat stat = statMaker.GetRandomStat(memory);

            denominatorVerb = stat.Expression[0];
            denominatorComplement = stat.Expression[1];
            numeratorVerb = stat.Expression[2];
            numeratorComplement = stat.Expression[3];

            int denominatorVerbId = memory.GetIdFromConcept(denominatorVerb);
            int denominatorComplementId = memory.GetIdFromConcept(denominatorComplement);
            int numeratorVerbId = memory.GetIdFromConcept(numeratorVerb);
            int numeratorComplementId = memory.GetIdFromConcept(numeratorComplement);

            return new StatInfo(denominatorVerbId, denominatorComplementId, numeratorVerbId, numeratorComplementId, stat.Ratio);
        }

        /// <summary>
        /// Perform an AiSql selection
        /// </summary>
        /// <param name="conditions">conditions</param>
        /// <param name="nameMapperToUse">name mapper to use</param>
        /// <param name="isConsiderSelfMuct">whether it consider stuff like "pine" as "isa pine"</param>
        /// <returns>A bunch of concept Id that matches conditions</returns>
        public HashSet<int> Select(string conditions, NameMapper nameMapperToUse, bool isConsiderSelfMuct)
        {
            HashSet<int> selectionByInt = new HashSet<int>();
            //repairer.RepairRange(memory);

            HashSet<Concept> selection = aiSqlWrapper.Select(conditions, nameMapperToUse, memory,repairer,isConsiderSelfMuct);

            foreach (Concept concept in selection)
                selectionByInt.Add(memory.GetIdFromConcept(concept));

            return selectionByInt;
        }

        /// <summary>
        /// Create imply connection
        /// </summary>
        /// <param name="action">action to perform</param>
        /// <param name="textCondition">condition</param>
        /// <param name="nameMapper">name mapper to look into</param>
        public void AddImplyConnection(string action, string textCondition, NameMapper nameMapper)
        {
            Condition condition = conditionBuilder.ParseString(textCondition,nameMapper,memory);
            string actionVerbName, actionComplementName;
            string actionPreviousVerbName = null;
            int actionVerbId, actionComplementId;
            Concept actionVerb, actionComplement;

            string[] words = action.Trim().Split(' ');

            if (words.Length < 2 || words.Length % 2 != 0)
                throw new StatementParsingException("Bad word count in action to perform");

            for (int i = 0; i < words.Length; i+=2)
            {
                actionVerbName = words[i];
                if (actionVerbName == "and" && actionPreviousVerbName != null)
                    actionVerbName = actionPreviousVerbName;
                actionPreviousVerbName = actionVerbName;

                actionVerbId = nameMapper.GetOrCreateConceptId(actionVerbName);
                actionVerb = memory.GetOrCreateConcept(actionVerbId);
                actionComplementName = words[i + 1];
                actionComplementId = nameMapper.GetOrCreateConceptId(actionComplementName);
                actionComplement = memory.GetOrCreateConcept(actionComplementId);

                implyConnectionManager.AddImplyConnection(actionVerb, actionComplement, condition);
            }
        }

        /// <summary>
        /// Remove imply connection
        /// </summary>
        /// <param name="action">action to perform</param>
        /// <param name="textCondition">condition</param>
        /// <param name="nameMapper">name mapper to look into</param>
        public void RemoveImplyConnection(string action, string textCondition, NameMapper nameMapper)
        {
            Condition condition = conditionBuilder.ParseString(textCondition, nameMapper, memory);
            string actionVerbName, actionComplementName;
            string actionPreviousVerbName = null;
            int actionVerbId, actionComplementId;
            Concept actionVerb, actionComplement;

            string[] words = action.Trim().Split(' ');

            if (words.Length < 2 || words.Length % 2 != 0)
                throw new StatementParsingException("Bad word count in action to perform");

            for (int i = 0; i < words.Length; i += 2)
            {
                actionVerbName = words[i];
                if (actionVerbName == "and" && actionPreviousVerbName != null)
                    actionVerbName = actionPreviousVerbName;
                actionPreviousVerbName = actionVerbName;

                actionVerbId = nameMapper.GetOrCreateConceptId(actionVerbName);
                actionVerb = memory.GetOrCreateConcept(actionVerbId);
                actionComplementName = words[i + 1];
                actionComplementId = nameMapper.GetOrCreateConceptId(actionComplementName);
                actionComplement = memory.GetOrCreateConcept(actionComplementId);

                implyConnectionManager.RemoveImplyConnection(actionVerb, actionComplement, condition);
            }
        }

        /// <summary>
        /// Test imply metaConnection
        /// </summary>
        /// <param name="action">action to perform</param>
        /// <param name="textCondition">condition</param>
        /// <param name="nameMapper">name mapper to look into</param>
        /// <returns>true if it exists, else: false</returns>
        public bool TestImplyConnection(string action, string textCondition, NameMapper nameMapper)
        {
            Condition condition = conditionBuilder.ParseString(textCondition, nameMapper, memory);
            string actionVerbName, actionComplementName;
            string actionPreviousVerbName = null;
            int actionVerbId, actionComplementId;
            Concept actionVerb, actionComplement;

            string[] words = action.Trim().Split(' ');

            if (words.Length < 2 || words.Length % 2 != 0)
                throw new StatementParsingException("Bad word count in action to perform");

            for (int i = 0; i < words.Length; i += 2)
            {
                actionVerbName = words[i];
                if (actionVerbName == "and" && actionPreviousVerbName != null)
                    actionVerbName = actionPreviousVerbName;
                actionPreviousVerbName = actionVerbName;

                actionVerbId = nameMapper.GetOrCreateConceptId(actionVerbName);
                actionVerb = memory.GetOrCreateConcept(actionVerbId);
                actionComplementName = words[i + 1];
                actionComplementId = nameMapper.GetOrCreateConceptId(actionComplementName);
                actionComplement = memory.GetOrCreateConcept(actionComplementId);

                if (!implyConnectionManager.TestImplyConnection(actionVerb, actionComplement, condition))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns an "imply" connection list for provided concept
        /// </summary>
        /// <param name="verbId">verb concept's id</param>
        /// <param name="nameMapper">name mapper to look into</param>
        /// <param name="isPositive">look in positive or negative tree</param>
        /// <returns>an "imply" connection list for provided concept</returns>
        public Dictionary<string, List<string>> GetDefinitionListImply(int verbId, NameMapper nameMapper, bool isPositive)
        {
            Dictionary<string, List<string>> stringDefinition = new Dictionary<string, List<string>>();
            int complementId;
            string complementName;
            List<string> conditionListString;
            Concept verb = memory.GetOrCreateConcept(verbId);
            Concept complement;
            HashSet<Condition> conditionList;
            ImplyConnectionTree currentTree;

            if (isPositive)
                currentTree = verb.ImplyConnectionTreePositive;
            else
                currentTree = verb.ImplyConnectionTreeNegative;

            foreach (KeyValuePair<Concept, HashSet<Condition>> complementAndConditionList in currentTree)
            {
                complement = complementAndConditionList.Key;
                conditionList = complementAndConditionList.Value;
                complementId = memory.GetIdFromConcept(complement);
                complementName = nameMapper.GetConceptNames(complementId)[0];

                if (!stringDefinition.TryGetValue(complementName, out conditionListString))
                {
                    conditionListString = new List<string>();
                    stringDefinition.Add(complementName, conditionListString);
                }

                foreach (Condition currentCondition in conditionList)
                {
                    conditionListString.Add(currentCondition.ToString(nameMapper, memory, isPositive));
                }
            }

            return stringDefinition;
        }

        /// <summary>
        /// Force the memory to be repaired completely
        /// </summary>
        public void ForceRepairMemory()
        {
            repairer.RepairRange(memory);
        }

        /// <summary>
        /// Remember parsed statement in episodic memory
        /// </summary>
        /// <param name="authorName">author's name</param>
        /// <param name="statement">statement to remember</param>
        /// <param name="nameMapper">nameMapper to look into</param>
        public void EpisodicRemember(string authorName, Statement statement, NameMapper nameMapper)
        {
            if (statement.ConceptCount == 3 && !statement.IsImply && !statement.IsSelect && !statement.IsStatCross && !statement.IsUpdate)
            {
                Concept subject, verb, complement;
                int subjectId, verbId, complementId;
                subjectId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));
                verbId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(1));
                complementId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(2));
                subject = memory.GetOrCreateConcept(subjectId);
                verb = memory.GetOrCreateConcept(verbId);
                complement = memory.GetOrCreateConcept(complementId);

                if (statement.IsAskingWhy)
                {
                    memory.EpisodicRemember(authorName, new List<string>() { "ask", "why" }, new List<Concept>() { subject, verb, complement }, statement.IsNegative);
                }
                else if (statement.IsAnalogy)
                {
                    memory.EpisodicRemember(authorName, new List<string>() { "ask", "to", "analogize" }, new List<Concept>() { subject, verb, complement }, statement.IsNegative);
                }
                else
                {
                    memory.EpisodicRemember(authorName, new List<Concept>() { subject, verb, complement }, statement.IsNegative, statement.IsInterrogative);
                }
            }
            else if (statement.ConceptCount == 2 && statement.MetaOperatorName != null)
            {
                Concept operator1, operator2;
                int operator1Id, operator2Id;
                operator1Id = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));
                operator2Id = nameMapper.GetOrCreateConceptId(statement.GetConceptName(1));
                operator1 = memory.GetOrCreateConcept(operator1Id);
                operator2 = memory.GetOrCreateConcept(operator2Id);

                memory.EpisodicRemember(authorName, new List<Concept>() { operator1, operator2 }, statement.MetaOperatorName, statement.IsNegative, statement.IsInterrogative);
            }
            else if (statement.ConceptCount == 1 && statement.NullaryOrUnaryOperatorName != null)
            {
                Concept subject;
                int subjectId;
                subjectId = nameMapper.GetOrCreateConceptId(statement.GetConceptName(0));
                subject = memory.GetOrCreateConcept(subjectId);

                memory.EpisodicRemember(authorName, new List<string>() { "ask", "to", statement.NullaryOrUnaryOperatorName }, new List<Concept>() { subject }, statement.IsNegative);
            }
            else if (statement.ConceptCount == 0 && statement.NullaryOrUnaryOperatorName != null)
            {
                memory.EpisodicRemember(authorName, "ask to " + statement.NullaryOrUnaryOperatorName);
            }
            else
            {
                memory.EpisodicRemember(authorName, statement);
            }
        }

        /// <summary>
        /// Start the background thinker thread
        /// </summary>
        public void StartBackgroundThinker(NameMapper nameMapper)
        {
            backgroundThinker.Memory = memory;
            backgroundThinker.NameMapper = nameMapper;
            backgroundThinkerThread = new Thread(backgroundThinker.Start);
            backgroundThinkerThread.Priority = ThreadPriority.BelowNormal;
            backgroundThinkerThread.IsBackground = true;
            backgroundThinkerThread.Start();
        }

        /// <summary>
        /// Stop the background thinker thread
        /// </summary>
        /// <returns>A stack of trauma (can be empty)</returns>
        public Stack<Trauma> StopBackgroundThinker()
        {
            if (backgroundThinkerThread != null && backgroundThinkerThread.IsAlive)
            {
                backgroundThinker.Stop();
                backgroundThinker.AutoResetEvent.WaitOne();
                backgroundThinker.AutoResetEvent.Reset();
            }

            return backgroundThinker.TraumaStack;
        }

        /// <summary>
        /// Get the list of theories to save them in a file
        /// </summary>
        /// <returns>list of theories to save them in a file</returns>
        public TheoryList GetTheoryListToSave()
        {
            return backgroundThinker.GetTheoryListToSave();
        }

        /// <summary>
        /// Get list of theory matching minimum probability or higher
        /// </summary>
        /// <param name="minimumProbability">minimum probability (from 0 to 1)</param>
        /// <param name="maxNumber">max number of theories to get</param>
        /// <returns>list of theory matching minimum probability or higher</returns>
        public List<Theory> GetTheoryListWithMinimumProbability(double minimumProbability, int maxNumber)
        {
            List<Theory> theoryList = new List<Theory>();
            
            foreach (Theory currentTheory in backgroundThinker.GetTheoryListToSave())
                if (currentTheory.PredictedProbability >= minimumProbability && theoryList.Count < maxNumber)
                    theoryList.Add(currentTheory);

            return theoryList;
        }

        /// <summary>
        /// Reset background thinker's theories
        /// </summary>
        public void ResetBackgroundThinkerTheoryMemory()
        {
            backgroundThinker.ResetTheoryMemory();
        }

        /// <summary>
        /// NEVER EVER USE THIS UNLESS YOU WANNA EXPORT MEMORY TO A FILE
        /// </summary>
        /// <param name="visitor">visitor (source object)</param>
        /// <returns>memory to export</returns>
        public Memory GetRepairedMemoryToExport(object visitor)
        {
            if (visitor is SaverLoader)
            {
                repairer.RepairRange(memory);
                return memory;
            }
            else
            {
                throw new MemoryException("Unauthorized access to memory");
            }
        }

        /// <summary>
        /// NEVER EVER USE THIS UNLESS
        /// EITHER
        /// YOU WANNA UPDATE AUTOCOMPLETE
        /// OR
        /// YOU WANNA UPDATE TREEVIEW
        /// OR
        /// YOU WANNA UPDATE THE VERB NAME LIST IN HELP
        /// OR
        /// YOU WANNA COUNT A CONCEPT'S PROOF LENGTH TO A COMPLEMENT
        /// </summary>
        /// <param name="visitor">visitor (source object)</param>
        /// <returns>memory for autoCompletion update</returns>
        public Memory GetUnRepairedMemory(object visitor)
        {
            if (visitor is MainWindow)
                return memory;
            else
                throw new MemoryException("Unauthorized access to memory");
        }

        /// <summary>
        /// NEVER EVER USE THIS UNLESS YOU WANNA IMPORT FILE TO MEMORY
        /// </summary>
        /// <param name="memory">memory to import</param>
        /// <param name="visitor">visitor (source object)</param>
        public void SetMemoryToLoad(Memory memory, object visitor)
        {
            if (visitor is SaverLoader)
            {
                this.memory = memory;
                aliaser.SetMemoryToLoad(memory);
                repairer.RepairRange(this.memory);
            }
            else
            {
                throw new MemoryException("Unauthorized access to memory");
            }
        }

        /// <summary>
        /// NEVER EVER USE THIS UNLESS YOU WANNA EXPORT REJECTED THEORIES TO A FILE
        /// </summary>
        /// <param name="visitor">visitor (source object)</param>
        /// <returns>rejected theories to export</returns>
        public RejectedTheories GetRejectedTheoriesToSave(object visitor)
        {
            if (visitor is SaverLoader)
                return backgroundThinker.GetRejectedTheoriesToSave();
            else
                throw new MemoryException("Unauthorized access to memory");
        }

        /// <summary>
        /// NEVER EVER USE THIS UNLESS YOU WANNA IMPORT FILE TO MEMORY
        /// </summary>
        /// <param name="rejectedTheories">rejected theories to import</param>
        /// <param name="visitor">visitor (source object)</param>
        public void SetRejectedTheoriesToLoad(RejectedTheories rejectedTheories, object visitor)
        {
            if (visitor is SaverLoader)
                backgroundThinker.SetRejectedTheoriesToLoad(rejectedTheories);
            else
                throw new MemoryException("Unauthorized access to memory");
        }

        /// <summary>
        /// WARNING! DON'T EVER USE THIS UNLESS YOU WANT TO LOOK INSIDE A CONCEPT IN A READONLY WAY
        /// </summary>
        /// <param name="conceptId">concept's id</param>
        /// <param name="visitor">visitor (source object)</param>
        /// <returns>concept</returns>
        public Concept GetConceptToLookIntoIt(int conceptId, object visitor)
        {
            if (visitor is Answerer)
                return memory.GetOrCreateConcept(conceptId);
            else
                throw new MemoryException("Unauthorized access to memory");
        }

        /// <summary>
        /// Remove background thinker theories that are already considered to be true
        /// so the Ai doesn't make theories about stuff already considered true
        /// </summary>
        public void CleanBackgroundThinker()
        {
            backgroundThinker.RemoveExistingConnections();
        }
        #endregion

        #region Properties
        public Theory CurrentTheory
        {
            get { return backgroundThinker.CurrentTheory; }
        }

        public DisambiguationNamer DisambiguationNamer
        {
            get { return disambiguationNamer; }
        }

        /// <summary>
        /// When loading instinct, memory must not be repaired after each metaConnection
        /// </summary>
        public bool DisableRepairAfterMetaConnection
        {
            get { return disableRepairAfterMetaConnection; }
            set { disableRepairAfterMetaConnection = value; }
        }

        /// <summary>
        /// When loading a list of statement from Wiki, disable repair between each statement
        /// </summary>
        public bool DisableFlattenizeAndOptimize
        {
            get { return connectionManager.DisableFlattenizeAndOptimizeAndPurify; }
            set { connectionManager.DisableFlattenizeAndOptimizeAndPurify = value; }
        }
        #endregion
    }
}