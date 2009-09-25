using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestMctFromMetaConnection
    {
        public static void Test()
        {
            TestTryingToFindMuctMetaConnectionTheory();
            TestTryingToFindLiffidMetaConnectionTheory();
            TestTryingToFindSublarMetaConnectionTheory();
            TestTryingToFindConsicsMetaConnectionTheory();
        }

        private static void TestTryingToFindMuctMetaConnectionTheory()
        {
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            RejectedTheories rejectedTheories = new RejectedTheories();
            MctFromMetaConnection mctFromMetaConnection = new MctFromMetaConnection(rejectedTheories, metaConnectionManager);
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept isa = new Concept("isa");
            Concept isi = new Concept("isi");
            Concept iso = new Concept("iso");
            Concept ise = new Concept("ise");
            Concept madeof = new Concept("madeof");
            Concept someare = new Concept("someare");
            Concept someire = new Concept("someire");
            Concept someore = new Concept("someore");
            Concept someere = new Concept("someere");
            Concept partof = new Concept("partof");

            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(isi, "inverse_of", someire);
            metaConnectionManager.AddMetaConnection(iso, "inverse_of", someore);
            metaConnectionManager.AddMetaConnection(ise, "inverse_of", someere);
            metaConnectionManager.AddMetaConnection(madeof, "muct", iso);
            metaConnectionManager.AddMetaConnection(madeof, "muct", isi);
            metaConnectionManager.AddMetaConnection(isa, "direct_implication", iso);
            metaConnectionManager.AddMetaConnection(isa, "direct_implication", isi);
            metaConnectionManager.AddMetaConnection(isa, "direct_implication", ise);

            //Pre-condtions

            if (metaConnectionManager.IsFlatMetaConnected(isa,"muct",isa))
                throw new Exception("Shouldn't be flat metaConnected");

            //Real test here

            Theory theory;

            theory = mctFromMetaConnection.GetBestTheoryAboutOperator(madeof);
            if (theory.GetConcept(0) != madeof || theory.GetConcept(1) != isa || theory.MetaOperatorName != "muct")
                throw new Exception("Should match theory");
            if (theory < 0.6 || theory > 0.7)
                throw new Exception("Should match probability");

            if (theory.CountMetaConnectionArgument != 4)
                throw new Exception("Wrong argument count");

            //Testing arguments

            if (!theory.GetMetaConnectionArgumentAt(0).Equals(new MetaConnectionArgument(isa, "direct_implication", iso)))
                throw new Exception("Argument mismatch");

            if (!theory.GetMetaConnectionArgumentAt(1).Equals(new MetaConnectionArgument(madeof, "muct", iso)))
                throw new Exception("Argument mismatch");

            if (!theory.GetMetaConnectionArgumentAt(2).Equals(new MetaConnectionArgument(isa, "direct_implication", isi)))
                throw new Exception("Argument mismatch");

            if (!theory.GetMetaConnectionArgumentAt(3).Equals(new MetaConnectionArgument(madeof, "muct", isi)))
                throw new Exception("Argument mismatch");

            //Testing with no specified operator

            theory = mctFromMetaConnection.GetBestTheoryAboutOperatorsInMemory();
            if (theory.GetConcept(0) != madeof && theory.GetConcept(0) != isa && theory.GetConcept(0) != partof)
                throw new Exception("Should match theory");
            if (theory.GetConcept(1) != isa && theory.GetConcept(1) != someare)
                throw new Exception("Should match theory");
            if (theory.MetaOperatorName != "muct" && theory.MetaOperatorName != "liffid")
                throw new Exception("Should match theory");


            if (theory < 0.6 || theory > 0.7)
                throw new Exception("Should match probability");
        }

        private static void TestTryingToFindLiffidMetaConnectionTheory()
        {
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            RejectedTheories rejectedTheories = new RejectedTheories();
            MctFromMetaConnection mctFromMetaConnection = new MctFromMetaConnection(rejectedTheories, metaConnectionManager);
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept isa = new Concept("isa");
            Concept isi = new Concept("isi");
            Concept iso = new Concept("iso");
            Concept ise = new Concept("ise");
            Concept madeof = new Concept("madeof");
            Concept someare = new Concept("someare");
            Concept someire = new Concept("someire");
            Concept someore = new Concept("someore");
            Concept someere = new Concept("someere");
            Concept partof = new Concept("partof");

            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(isi, "inverse_of", someire);
            metaConnectionManager.AddMetaConnection(iso, "inverse_of", someore);
            metaConnectionManager.AddMetaConnection(ise, "inverse_of", someere);
            metaConnectionManager.AddMetaConnection(madeof, "liffid", iso);
            metaConnectionManager.AddMetaConnection(madeof, "liffid", isi);
            metaConnectionManager.AddMetaConnection(isa, "direct_implication", iso);
            metaConnectionManager.AddMetaConnection(isa, "direct_implication", isi);
            metaConnectionManager.AddMetaConnection(isa, "direct_implication", ise);

            Theory theory;

            theory = mctFromMetaConnection.GetBestTheoryAboutOperator(madeof);
            if (theory.GetConcept(0) != madeof || theory.GetConcept(1) != isa || theory.MetaOperatorName != "liffid")
                throw new Exception("Should match theory");
            if (theory < 0.6 || theory > 0.7)
                throw new Exception("Should match probability");

            theory = mctFromMetaConnection.GetBestTheoryAboutOperatorsInMemory();
            if (theory.GetConcept(0) != madeof && theory.GetConcept(0) != isa && theory.GetConcept(0) != partof)
                throw new Exception("Should match theory");
            if (theory.GetConcept(1) != isa && theory.GetConcept(1) != someare)
                throw new Exception("Should match theory");
            if (theory.MetaOperatorName != "liffid" && theory.MetaOperatorName != "muct")
                throw new Exception("Should match theory");
            if (theory < 0.6 || theory > 0.7)
                throw new Exception("Should match probability");
        }

        private static void TestTryingToFindSublarMetaConnectionTheory()
        {
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            RejectedTheories rejectedTheories = new RejectedTheories();
            MctFromMetaConnection mctFromMetaConnection = new MctFromMetaConnection(rejectedTheories, metaConnectionManager);
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept isa = new Concept("isa");
            Concept isi = new Concept("isi");
            Concept iso = new Concept("iso");
            Concept ise = new Concept("ise");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept someare = new Concept("someare");
            Concept someire = new Concept("someire");
            Concept someere = new Concept("someere");
            Concept someore = new Concept("someore");

            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(isi, "inverse_of", someire);
            metaConnectionManager.AddMetaConnection(ise, "inverse_of", someere);
            metaConnectionManager.AddMetaConnection(iso, "inverse_of", someore);
            metaConnectionManager.AddMetaConnection(madeof, "sublar", iso);
            metaConnectionManager.AddMetaConnection(madeof, "sublar", isi);
            metaConnectionManager.AddMetaConnection(isa, "direct_implication", iso);
            metaConnectionManager.AddMetaConnection(isa, "direct_implication", isi);
            metaConnectionManager.AddMetaConnection(isa, "direct_implication", ise);

            Theory theory;

            theory = mctFromMetaConnection.GetBestTheoryAboutOperator(madeof);
            if (theory.GetConcept(0) != madeof || theory.GetConcept(1) != someare || theory.MetaOperatorName != "muct")
                throw new Exception("Should match theory");
            if (theory < 0.6 || theory > 0.7)
                throw new Exception("Should match probability");
        }

        private static void TestTryingToFindConsicsMetaConnectionTheory()
        {
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();
            RejectedTheories rejectedTheories = new RejectedTheories();
            MctFromMetaConnection mctFromMetaConnection = new MctFromMetaConnection(rejectedTheories, metaConnectionManager);
            Memory.TotalVerbList = new HashSet<Concept>();

            Concept isa = new Concept("isa");
            Concept isi = new Concept("isi");
            Concept iso = new Concept("iso");
            Concept ise = new Concept("ise");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept someare = new Concept("someare");
            Concept someire = new Concept("someire");
            Concept someere = new Concept("someere");
            Concept someore = new Concept("someore");

            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(isi, "inverse_of", someire);
            metaConnectionManager.AddMetaConnection(ise, "inverse_of", someere);
            metaConnectionManager.AddMetaConnection(iso, "inverse_of", someore);
            metaConnectionManager.AddMetaConnection(madeof, "consics", iso);
            metaConnectionManager.AddMetaConnection(madeof, "consics", isi);
            metaConnectionManager.AddMetaConnection(isa, "direct_implication", iso);
            metaConnectionManager.AddMetaConnection(isa, "direct_implication", isi);
            metaConnectionManager.AddMetaConnection(isa, "direct_implication", ise);

            Theory theory;

            theory = mctFromMetaConnection.GetBestTheoryAboutOperator(madeof);
            if (theory.GetConcept(0) != madeof || theory.GetConcept(1) != someare || theory.MetaOperatorName != "liffid")
                throw new Exception("Should match theory");
            if (theory < 0.6 || theory > 0.7)
                throw new Exception("Should match probability");
        }
    }
}
