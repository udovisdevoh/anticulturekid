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
            RejectedTheories rejectedTheories = new RejectedTheories();
            MctFromMetaConnection mctFromMetaConnection = new MctFromMetaConnection(rejectedTheories);
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

            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(isi, "inverse_of", someire);
            MetaConnectionManager.AddMetaConnection(iso, "inverse_of", someore);
            MetaConnectionManager.AddMetaConnection(ise, "inverse_of", someere);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", iso);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isi);
            MetaConnectionManager.AddMetaConnection(isa, "direct_implication", iso);
            MetaConnectionManager.AddMetaConnection(isa, "direct_implication", isi);
            MetaConnectionManager.AddMetaConnection(isa, "direct_implication", ise);

            //Pre-condtions

            if (MetaConnectionManager.IsFlatMetaConnected(isa,"muct",isa))
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
            RejectedTheories rejectedTheories = new RejectedTheories();
            MctFromMetaConnection mctFromMetaConnection = new MctFromMetaConnection(rejectedTheories);
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

            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(isi, "inverse_of", someire);
            MetaConnectionManager.AddMetaConnection(iso, "inverse_of", someore);
            MetaConnectionManager.AddMetaConnection(ise, "inverse_of", someere);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", iso);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isi);
            MetaConnectionManager.AddMetaConnection(isa, "direct_implication", iso);
            MetaConnectionManager.AddMetaConnection(isa, "direct_implication", isi);
            MetaConnectionManager.AddMetaConnection(isa, "direct_implication", ise);

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
            
            RejectedTheories rejectedTheories = new RejectedTheories();
            MctFromMetaConnection mctFromMetaConnection = new MctFromMetaConnection(rejectedTheories);
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

            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(isi, "inverse_of", someire);
            MetaConnectionManager.AddMetaConnection(ise, "inverse_of", someere);
            MetaConnectionManager.AddMetaConnection(iso, "inverse_of", someore);
            MetaConnectionManager.AddMetaConnection(madeof, "sublar", iso);
            MetaConnectionManager.AddMetaConnection(madeof, "sublar", isi);
            MetaConnectionManager.AddMetaConnection(isa, "direct_implication", iso);
            MetaConnectionManager.AddMetaConnection(isa, "direct_implication", isi);
            MetaConnectionManager.AddMetaConnection(isa, "direct_implication", ise);

            Theory theory;

            theory = mctFromMetaConnection.GetBestTheoryAboutOperator(madeof);
            if (theory.GetConcept(0) != madeof || theory.GetConcept(1) != someare || theory.MetaOperatorName != "muct")
                throw new Exception("Should match theory");
            if (theory < 0.6 || theory > 0.7)
                throw new Exception("Should match probability");
        }

        private static void TestTryingToFindConsicsMetaConnectionTheory()
        {
            
            RejectedTheories rejectedTheories = new RejectedTheories();
            MctFromMetaConnection mctFromMetaConnection = new MctFromMetaConnection(rejectedTheories);
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

            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(isi, "inverse_of", someire);
            MetaConnectionManager.AddMetaConnection(ise, "inverse_of", someere);
            MetaConnectionManager.AddMetaConnection(iso, "inverse_of", someore);
            MetaConnectionManager.AddMetaConnection(madeof, "consics", iso);
            MetaConnectionManager.AddMetaConnection(madeof, "consics", isi);
            MetaConnectionManager.AddMetaConnection(isa, "direct_implication", iso);
            MetaConnectionManager.AddMetaConnection(isa, "direct_implication", isi);
            MetaConnectionManager.AddMetaConnection(isa, "direct_implication", ise);

            Theory theory;

            theory = mctFromMetaConnection.GetBestTheoryAboutOperator(madeof);
            if (theory.GetConcept(0) != madeof || theory.GetConcept(1) != someare || theory.MetaOperatorName != "liffid")
                throw new Exception("Should match theory");
            if (theory < 0.6 || theory > 0.7)
                throw new Exception("Should match probability");
        }
    }
}
