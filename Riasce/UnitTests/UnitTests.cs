using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Text;

namespace AntiCulture.Kid
{
    static class UnitTests
    {
        public static void TestAll()
        {
            DateTime startTime = DateTime.Now;

            TestFlattenizer.Test(new SerialFlattenizer()); //SLOW!!!!
            TestFlattenizer.Test(new ChaoticSerialFlattenizer()); //SLOW!!!!
            //TestFlattenizer.Test(new ParallelFlattenizer()); //SLOW!!!!
            TestStatementListFactory.Test();
            TestConditionBuilder.Test();
            TestWikiCategoryPage.Test();
            TestPurifier.Test();
            TestStatementFactory.Test();
            TestNameMapper.Test();
            TestInstinct.Test();
            TestConcept.Test();
            TestMemory.Test();
            TestMetaConnectionManager.Test();
            TestConnectionManager.Test();
            TestOptimizer.Test();
            TestMetaConnectionFlattenizer.Test(); //SLOW!!!!
            TestBrain.Test();
            TestReciprocator.Test();
            TestProof.Test();
            TestAssimilator.Test();
            TestPurifier.Test();
            TestRejectedTheories.Test();
            TestMctFromMetaConnection.Test();
            TestConnectionTheorizer.Test();
            TestAnalogizer.Test();
            TestDisambiguationNamer.Test();
            TestStatMaker.Test();
            TestQuerySplitter.Test();
            TestAiSqlWrapper.Test();
            TestCategoryListExtractorModel.Test();
            string randomString = Markov.GenerateRandomWord(new List<string>() { "43y5re", "546ryhre6" }, 7);

            DateTime stopTime = DateTime.Now;
            TimeSpan duration = stopTime - startTime;
            MessageBox.Show("Unit tests completed in " + duration.TotalMilliseconds + " milliseconds.");
        }
    }
}