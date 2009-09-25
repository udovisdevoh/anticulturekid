using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class MetaConnectionTheorizer : AbstractMetaConnectionTheorizer
    {
        #region Fields and Parts
        private MctFromConnection mctFromConnection;

        private MctFromMetaConnection mctFromMetaConnection;

        private RejectedTheories rejectedTheories;

        private Random random = new Random();
        #endregion

        #region Constructor
        public MetaConnectionTheorizer(RejectedTheories rejectedTheories, MetaConnectionManager metaConnectionManager)
        {
            this.rejectedTheories = rejectedTheories;
            mctFromConnection = new MctFromConnection(rejectedTheories);
            mctFromMetaConnection = new MctFromMetaConnection(rejectedTheories, metaConnectionManager);
        }
        #endregion

        #region Methods
        public override Theory GetBestTheoryAboutOperator(Concept verb)
        {
            return mctFromMetaConnection.GetBestTheoryAboutOperator(verb);

            /*
            Theory theoryFromConnection = mctFromConnection.GetBestTheoryAboutOperator(verb);
            Theory theoryFromMetaConnection = mctFromMetaConnection.GetBestTheoryAboutOperator(verb);

            if (theoryFromConnection == null && theoryFromMetaConnection == null)
                return null;
            else if (theoryFromConnection == null)
                return theoryFromMetaConnection;
            else if (theoryFromMetaConnection == null)
                return theoryFromConnection;
            else if (theoryFromConnection > theoryFromMetaConnection)
                return theoryFromConnection;
            else
                return theoryFromMetaConnection;
             * 
            */
        }

        public override Theory GetBestTheoryAboutOperatorsInMemory()
        {
            return mctFromMetaConnection.GetBestTheoryAboutOperatorsInMemory();
            /*
            Theory theoryFromMetaConnection = mctFromMetaConnection.GetBestTheoryAboutOperatorsInMemory();
            Theory theoryFromConnection = mctFromConnection.GetBestTheoryAboutOperatorsInMemory();

            if (theoryFromConnection == null && theoryFromMetaConnection == null)
                return null;
            else if (theoryFromConnection == null)
                return theoryFromMetaConnection;
            else if (theoryFromMetaConnection == null)
                return theoryFromConnection;
            else if (theoryFromConnection > theoryFromMetaConnection)
                return theoryFromConnection;
            else
                return theoryFromMetaConnection;
            */
        }

        public override Theory GetRandomMetaConnectionTheoryAbout(Concept operatorToThinkAbout)
        {
            List<string> theorizableMetaOperatorNameList = new List<string>() { "muct", "liffid", "sublar", "consics", "cant", "unlikely" };
            return mctFromMetaConnection.GetRandomTheoryAbout(operatorToThinkAbout, theorizableMetaOperatorNameList[random.Next(theorizableMetaOperatorNameList.Count)]);
        }
        #endregion
    }
}