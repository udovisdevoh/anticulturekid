using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestMetaConnectionFlattenizer
    {
        public static void Test()
        {
            TestMuctSublarLiffidConsics();

            TestCant();

            TestUnlikely();

            TestUnlikelyFromImplicitCant();

            TestDirectImplication();

            TestInverseImplication();

            TestRecursiveDirectImplication();

            TestRecursiveInverseImplication();

            TestRecursiveDirectImplicationFromDoubleReverse();

            TestPreRecursiveCant();

            TestPreRecursiveUnlikely();

            TestPreRecursiveUnlikelyFromImplicitCant();

            TestPostRecursiveCant();

            TestPostRecursiveUnlikely();

            TestPostRecursiveUnlikelyFromImplicitCant();

            TestRecursiveSelfMuctSublarLiffidConsics();

            TestIsaMuctIsaFromMadeofMuctIsa();

            TestIsaMuctIsaFromMadeofLiffidIsa();

            TestMadeofMustNotMuctNorLiffidNeed();

            TestMadeofMustNotMuctNorLiffidNeedHarderTestWithInverseOf();

            TestMakeMustNotLiffidNeed();

            TestCantFromImplicitInverseOf();

            TestNoSelfCant();

            TestOrbitedByDirectImplicationGravity();

            TestXorToAnd();
        }

        private static void TestMuctSublarLiffidConsics()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            #region We create the verbs that we will need
            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept contradict = new Concept("contradict");

            Concept isa2 = new Concept("isa2");
            Concept someare2 = new Concept("someare2");
            Concept madeof2 = new Concept("madeof2");
            Concept partof2 = new Concept("partof2");
            Concept contradict2 = new Concept("contradict2");

            Concept isa09 = new Concept("isa09");
            Concept someare09 = new Concept("someare09");
            Concept madeof09 = new Concept("madeof09");
            Concept partof09 = new Concept("partof09");
            Concept contradict09 = new Concept("contradict09");

            Concept isa10 = new Concept("isa10");
            Concept someare10 = new Concept("someare10");
            Concept madeof10 = new Concept("madeof10");
            Concept partof10 = new Concept("partof10");
            Concept contradict10 = new Concept("contradict10");

            #endregion

            #region We plug the verbs using metaconnections
            /* 
             * isa inverse_of someare
             * isa muct isa
             * isa cant someare
             * isa cant madeof
             * isa cant partof
             * madeof inverse_of partof
             * madeof muct madeof
             * madeof muct isa
             * madeof liffid isa
             * madeof unlikely isa
             * madeof cant partof
             * contradict permutable_side contradict
             * contradict muct isa
             * contradict cant isa
            */
            MetaConnectionManager.AddMetaConnection(isa, "muct", isa);
            MetaConnectionManager.AddMetaConnection(isa, "cant", someare);
            MetaConnectionManager.AddMetaConnection(isa, "cant", madeof);
            MetaConnectionManager.AddMetaConnection(isa, "cant", partof);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);

            MetaConnectionManager.AddMetaConnection(madeof, "unlikely", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "cant", partof);
            MetaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);
            MetaConnectionManager.AddMetaConnection(contradict, "muct", isa);
            MetaConnectionManager.AddMetaConnection(contradict, "cant", isa);



            MetaConnectionManager.AddMetaConnection(isa2, "muct", isa2);
            MetaConnectionManager.AddMetaConnection(isa2, "cant", someare2);
            MetaConnectionManager.AddMetaConnection(isa2, "cant", madeof2);
            MetaConnectionManager.AddMetaConnection(isa2, "cant", partof2);
            MetaConnectionManager.AddMetaConnection(madeof2, "muct", madeof2);
            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);

            MetaConnectionManager.AddMetaConnection(madeof2, "unlikely", isa2);
            MetaConnectionManager.AddMetaConnection(madeof2, "cant", partof2);
            MetaConnectionManager.AddMetaConnection(contradict2, "permutable_side", contradict2);
            MetaConnectionManager.AddMetaConnection(contradict2, "muct", isa2);
            MetaConnectionManager.AddMetaConnection(contradict2, "cant", isa2);
            MetaConnectionManager.AddMetaConnection(isa2, "inverse_of", someare2);
            MetaConnectionManager.AddMetaConnection(madeof2, "inverse_of", partof2);

            MetaConnectionManager.AddMetaConnection(isa09, "inverse_of", someare09);
            MetaConnectionManager.AddMetaConnection(madeof09, "inverse_of", partof09);

            MetaConnectionManager.AddMetaConnection(isa10, "inverse_of", someare10);
            MetaConnectionManager.AddMetaConnection(madeof10, "inverse_of", partof10);
            #endregion

            #region Testing muct, sublar, liffid and consics
            #region Testing muct
            if (MetaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(madeof, "sublar", someare))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(partof, "consics", isa))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(partof, "liffid", someare))
                throw new Exception("Operators shouldn't be flat metaConnected");

            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Operators should be flat metaConnected");

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof, "sublar", someare))
                throw new Exception("Operators should be flat metaConnected");

            if (!MetaConnectionManager.IsFlatMetaConnected(partof, "consics", isa))
                throw new Exception("Operators should be flat metaConnected");

            if (!MetaConnectionManager.IsFlatMetaConnected(partof, "liffid", someare))
                throw new Exception("Operators should be flat metaConnected");


            if (MetaConnectionManager.IsFlatMetaConnected(madeof, "sublar", isa))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(madeof, "liffid", isa))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(madeof, "consics", isa))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");
            #endregion

            #region Testing liffid
            if (MetaConnectionManager.IsFlatMetaConnected(madeof09, "liffid", isa09))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(madeof09, "consics", someare09))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(partof09, "sublar", isa09))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(partof09, "muct", someare09))
                throw new Exception("Operators shouldn't be flat metaConnected");

            MetaConnectionManager.AddMetaConnection(madeof09, "liffid", isa09);

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof09, "liffid", isa09))
                throw new Exception("Operators should be flat metaConnected");

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof09, "consics", someare09))
                throw new Exception("Operators should be flat metaConnected");

            if (!MetaConnectionManager.IsFlatMetaConnected(partof09, "sublar", isa09))
                throw new Exception("Operators should be flat metaConnected");

            if (!MetaConnectionManager.IsFlatMetaConnected(partof09, "muct", someare09))
                throw new Exception("Operators should be flat metaConnected");


            if (MetaConnectionManager.IsFlatMetaConnected(madeof09, "muct", isa09))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(madeof09, "consics", isa09))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(madeof09, "sublar", isa09))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");
            #endregion

            #region Testing consics
            if (MetaConnectionManager.IsFlatMetaConnected(madeof2, "consics", someare2))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(madeof2, "liffid", isa2))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(partof2, "sublar", isa2))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(partof2, "muct", someare2))
                throw new Exception("Operators shouldn't be flat metaConnected");

            MetaConnectionManager.AddMetaConnection(madeof2, "consics", someare2);

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof2, "consics", someare2))
                throw new Exception("Operators should be flat metaConnected");

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof2, "liffid", isa2))
                throw new Exception("Operators should be flat metaConnected");

            if (!MetaConnectionManager.IsFlatMetaConnected(partof2, "sublar", isa2))
                throw new Exception("Operators should be flat metaConnected");

            if (!MetaConnectionManager.IsFlatMetaConnected(partof2, "muct", someare2))
                throw new Exception("Operators should be flat metaConnected");


            if (MetaConnectionManager.IsFlatMetaConnected(madeof2, "muct", someare2))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(madeof2, "sublar", someare2))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(madeof2, "liffid", someare2))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");
            #endregion

            #region Testing sublar
            if (MetaConnectionManager.IsFlatMetaConnected(madeof10, "sublar", someare10))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(madeof10, "muct", isa10))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(partof10, "consics", isa10))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(partof10, "liffid", someare10))
                throw new Exception("Operators shouldn't be flat metaConnected");

            MetaConnectionManager.AddMetaConnection(madeof10, "sublar", someare10);

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof10, "sublar", someare10))
                throw new Exception("Operators should be flat metaConnected");

            if (!MetaConnectionManager.IsFlatMetaConnected(madeof10, "muct", isa10))
                throw new Exception("Operators should be flat metaConnected");

            if (!MetaConnectionManager.IsFlatMetaConnected(partof10, "consics", isa10))
                throw new Exception("Operators should be flat metaConnected");

            if (!MetaConnectionManager.IsFlatMetaConnected(partof10, "liffid", someare10))
                throw new Exception("Operators should be flat metaConnected");


            if (MetaConnectionManager.IsFlatMetaConnected(madeof10, "muct", someare10))
                throw new Exception("Operators shouldnn't accidentally be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(madeof10, "liffid", someare10))
                throw new Exception("Operators shouldnn't accidentally be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(madeof10, "consics", someare10))
                throw new Exception("Operators shouldnn't accidentally be flat metaConnected");
            #endregion
            #endregion
        }

        private static void TestCant()
        {
            Memory.TotalVerbList = new HashSet<Concept>();

            #region We create the verbs that we will need
            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept contradict = new Concept("contradict");
            #endregion

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "cant", contradict))
                throw new Exception("Shouldn't be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(contradict, "cant", isa))
                throw new Exception("Shouldn't be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "cant", contradict))
                throw new Exception("Shouldn't be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(contradict, "cant", someare))
                throw new Exception("Shouldn't be metaConnected yet");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "cant", contradict))
                throw new Exception("Should be metaConnected because we told so");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "cant", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "cant", contradict))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "cant", someare))
                throw new Exception("Should be metaConnected because it's implicit");
        }

        private static void TestXorToAnd()
        {
            Memory.TotalVerbList = new HashSet<Concept>();

            #region We create the verbs that we will need
            Concept need = new Concept("need");
            Concept allow = new Concept("allow");
            Concept synergize = new Concept("synergize");
            #endregion

            if (MetaConnectionManager.IsFlatMetaConnected(need, "inverse_of", allow))
                throw new Exception("Shouldn't be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(synergize, "permutable_side", synergize))
                throw new Exception("Shouldn't be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(synergize, "xor_to_and", allow))
                throw new Exception("Shouldn't be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(synergize, "xor_to_and", need))
                throw new Exception("Shouldn't be metaConnected yet");

            MetaConnectionManager.AddMetaConnection(need, "inverse_of", allow);
            MetaConnectionManager.AddMetaConnection(synergize, "permutable_side", synergize);
            MetaConnectionManager.AddMetaConnection(synergize, "xor_to_and", need);

            if (!MetaConnectionManager.IsFlatMetaConnected(synergize, "xor_to_and", need))
                throw new Exception("Should be metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(synergize, "xor_to_and", allow))
                throw new Exception("Should be metaConnected because it's implicit");
        }

        private static void TestUnlikely()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            #region We create the verbs that we will need
            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept contradict = new Concept("contradict");
            #endregion

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "unlikely", contradict))
                throw new Exception("Shouldn't be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", isa))
                throw new Exception("Shouldn't be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "unlikely", contradict))
                throw new Exception("Shouldn't be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", someare))
                throw new Exception("Shouldn't be metaConnected yet");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(isa, "unlikely", contradict);

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "unlikely", contradict))
                throw new Exception("Should be metaConnected because we told so");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "unlikely", contradict))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", someare))
                throw new Exception("Should be metaConnected because it's implicit");
        }

        private static void TestUnlikelyFromImplicitCant()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            #region We create the verbs that we will need
            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept contradict = new Concept("contradict");
            #endregion

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "unlikely", contradict))
                throw new Exception("Shouldn't be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", isa))
                throw new Exception("Shouldn't be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "unlikely", contradict))
                throw new Exception("Shouldn't be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", someare))
                throw new Exception("Shouldn't be metaConnected yet");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "unlikely", contradict))
                throw new Exception("Should be metaConnected because we told so");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "unlikely", contradict))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", someare))
                throw new Exception("Should be metaConnected because it's implicit");
        }

        private static void TestDirectImplication()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            //The child loves the mother because he's made by her

            

            #region We create some verbs
            Concept make = new Concept("make");
            Concept madeby = new Concept("madeby");
            Concept love = new Concept("love");
            Concept lovedby = new Concept("lovedby");
            #endregion

            #region We plug the verbs together
            MetaConnectionManager.AddMetaConnection(make, "inverse_of", madeby);
            MetaConnectionManager.AddMetaConnection(love, "inverse_of", lovedby);
            MetaConnectionManager.AddMetaConnection(make, "inverse_implication", love);
            #endregion

            if (!MetaConnectionManager.IsFlatMetaConnected(make, "inverse_implication", love))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(madeby, "inverse_implication", lovedby))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(make, "direct_implication", lovedby))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(madeby, "direct_implication", love))
                throw new Exception("Should be flat metaConnected because it's implicit");
        }

        private static void TestInverseImplication()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            //The mother loves the child because she made it

            

            #region We create some verbs
            Concept make = new Concept("make");
            Concept madeby = new Concept("madeby");
            Concept love = new Concept("love");
            Concept lovedby = new Concept("lovedby");
            #endregion

            #region We plug the verbs together
            MetaConnectionManager.AddMetaConnection(make, "inverse_of", madeby);
            MetaConnectionManager.AddMetaConnection(love, "inverse_of", lovedby);
            MetaConnectionManager.AddMetaConnection(make, "direct_implication", love);
            #endregion

            if (!MetaConnectionManager.IsFlatMetaConnected(make, "direct_implication", love))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(madeby, "direct_implication", lovedby))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(make, "inverse_implication", lovedby))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(madeby, "inverse_implication", love))
                throw new Exception("Should be flat metaConnected because it's implicit");
        }

        private static void TestRecursiveMuctSublarLiffidConsics()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            #region We create verbs we will need
            Concept generalIsa = new Concept("general isa");//more general
            Concept isa = new Concept("isa");//more specialized

            Concept madeof = new Concept("madeof");//more general
            Concept materialMadeof = new Concept("material madeof");//middle point
            Concept specializedMaterialMadeof = new Concept("specialized material madeof");//more specialized

            Concept someare = new Concept("someare");
            Concept partof = new Concept("partof");
            Concept generalSomeare = new Concept("generalSomeare");
            Concept materialPartof = new Concept("material partof");
            Concept specializedMaterialPartof = new Concept("specialized material partof");
            #endregion

            #region We plug the verbs uning metaconnections
            MetaConnectionManager.AddMetaConnection(isa, "muct", isa);
            MetaConnectionManager.AddMetaConnection(isa, "cant", someare);
            MetaConnectionManager.AddMetaConnection(isa, "cant", madeof);
            MetaConnectionManager.AddMetaConnection(isa, "cant", partof);
            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);

            MetaConnectionManager.AddMetaConnection(specializedMaterialMadeof, "direct_implication", materialMadeof);
            MetaConnectionManager.AddMetaConnection(materialMadeof, "direct_implication", madeof);
            MetaConnectionManager.AddMetaConnection(isa, "direct_implication", generalIsa);

            MetaConnectionManager.AddMetaConnection(generalSomeare, "inverse_of", generalIsa);
            MetaConnectionManager.AddMetaConnection(materialPartof, "inverse_of", materialMadeof);
            MetaConnectionManager.AddMetaConnection(specializedMaterialPartof, "inverse_of", specializedMaterialMadeof);
            #endregion

            #region Testing muct
            if (!MetaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Operators should be metaconnected because we told so");

            if (!MetaConnectionManager.IsFlatMetaConnected(materialMadeof, "muct", isa))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(specializedMaterialMadeof, "muct", isa))
                throw new Exception("Operators should be metaconnected because it's implicit");


            if (!MetaConnectionManager.IsFlatMetaConnected(madeof, "muct", generalIsa))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(materialMadeof, "muct", generalIsa))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(specializedMaterialMadeof, "muct", generalIsa))
                throw new Exception("Operators should be metaconnected because it's implicit");
            #endregion

            #region Testing liffid
            if (!MetaConnectionManager.IsFlatMetaConnected(partof, "liffid", someare))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(materialPartof, "liffid", someare))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(specializedMaterialPartof, "liffid", someare))
                throw new Exception("Operators should be metaconnected because it's implicit");


            if (!MetaConnectionManager.IsFlatMetaConnected(partof, "liffid", generalSomeare))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(materialPartof, "liffid", generalSomeare))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(specializedMaterialPartof, "liffid", generalSomeare))
                throw new Exception("Operators should be metaconnected because it's implicit");
            #endregion

            #region Testing sublar
            if (!MetaConnectionManager.IsFlatMetaConnected(madeof, "sublar", someare))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(materialMadeof, "sublar", someare))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(specializedMaterialMadeof, "sublar", someare))
                throw new Exception("Operators should be metaconnected because it's implicit");


            if (!MetaConnectionManager.IsFlatMetaConnected(madeof, "sublar", generalSomeare))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(materialMadeof, "sublar", generalSomeare))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(specializedMaterialMadeof, "sublar", generalSomeare))
                throw new Exception("Operators should be metaconnected because it's implicit");
            #endregion

            #region Testing consics
            if (!MetaConnectionManager.IsFlatMetaConnected(partof, "consics", isa))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(materialPartof, "consics", isa))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(specializedMaterialPartof, "consics", isa))
                throw new Exception("Operators should be metaconnected because it's implicit");


            if (!MetaConnectionManager.IsFlatMetaConnected(partof, "consics", generalIsa))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(materialPartof, "consics", generalIsa))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(specializedMaterialPartof, "consics", generalIsa))
                throw new Exception("Operators should be metaconnected because it's implicit");
            #endregion
        }

        private static void TestRecursiveDirectImplication()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            #region We create some verbs
            Concept love = new Concept("love");
            Concept like = new Concept("like");
            Concept likeabit = new Concept("like a bit");
            Concept likealittlebit = new Concept("like a little bit");
            Concept lovedby = new Concept("lovedby");
            Concept likedby = new Concept("likedby");
            Concept likedabit = new Concept("liked a bit");
            Concept likedalittlebit = new Concept("liked a little bit");
            #endregion

            MetaConnectionManager.AddMetaConnection(likeabit, "inverse_of", likedabit);
            MetaConnectionManager.AddMetaConnection(likealittlebit, "inverse_of", likedalittlebit);
            MetaConnectionManager.AddMetaConnection(love, "inverse_of", lovedby);
            MetaConnectionManager.AddMetaConnection(like, "inverse_of", likedby);
            MetaConnectionManager.AddMetaConnection(love, "direct_implication", like);
            MetaConnectionManager.AddMetaConnection(like, "direct_implication", likeabit);
            MetaConnectionManager.AddMetaConnection(likeabit, "direct_implication", likealittlebit);

            if (!MetaConnectionManager.IsFlatMetaConnected(love, "direct_implication", like))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(love, "direct_implication", likeabit))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(love, "direct_implication", likealittlebit))
                throw new Exception("Should be flat metaConnected because it's implicit");
        }

        private static void TestRecursiveDirectImplicationFromDoubleReverse()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            //Person A loves person B, therefore, person B respect person A and person A appreciates the respect
            

            #region We create some verbs
            Concept love = new Concept("love");
            Concept lovedby = new Concept("lovedby");
            Concept like = new Concept("like");
            Concept likedby = new Concept("likedby");
            Concept likeabit = new Concept("like a bit");
            Concept likedabit = new Concept("liked a bit");
            Concept likealittlebit = new Concept("like a little bit");
            Concept likedalittlebit = new Concept("liked a little bit");
            Concept respectalittlebit = new Concept("respect a little bit");
            Concept respectedalittlebit = new Concept("respected a little bit");
            Concept appreciaterespect = new Concept("appreciate respect");
            Concept appreciaterespected = new Concept("appreciate respected");
            Concept appreciaterespectalittlebit = new Concept("appreciate respect a little bit");
            Concept appreciatedrespectalittlebit = new Concept("appreciated respect a little bit");
            #endregion

            MetaConnectionManager.AddMetaConnection(appreciaterespectalittlebit, "inverse_of", appreciatedrespectalittlebit);
            MetaConnectionManager.AddMetaConnection(likealittlebit, "inverse_of", likedalittlebit);
            MetaConnectionManager.AddMetaConnection(love, "inverse_of", lovedby);
            MetaConnectionManager.AddMetaConnection(like, "inverse_of", likedby);
            MetaConnectionManager.AddMetaConnection(likeabit, "inverse_of", likedabit);
            MetaConnectionManager.AddMetaConnection(respectalittlebit, "inverse_of", respectedalittlebit);
            MetaConnectionManager.AddMetaConnection(appreciaterespect, "inverse_of", appreciaterespected);
            MetaConnectionManager.AddMetaConnection(love, "direct_implication", like);
            MetaConnectionManager.AddMetaConnection(like, "direct_implication", likeabit);
            MetaConnectionManager.AddMetaConnection(likeabit, "direct_implication", likealittlebit);
            MetaConnectionManager.AddMetaConnection(likeabit, "inverse_implication", respectalittlebit);
            MetaConnectionManager.AddMetaConnection(respectalittlebit, "inverse_implication", appreciaterespect);
            MetaConnectionManager.AddMetaConnection(appreciaterespect, "direct_implication", appreciaterespectalittlebit);

            if (!MetaConnectionManager.IsFlatMetaConnected(love, "direct_implication", like))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(love, "direct_implication", likeabit))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(love, "direct_implication", likealittlebit))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(likeabit, "inverse_implication", respectalittlebit))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(like, "inverse_implication", respectalittlebit))
                throw new Exception("Should be flat metaConnected because it's implicit");

            //More intensive test

            if (!MetaConnectionManager.IsFlatMetaConnected(love, "inverse_implication", respectalittlebit))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(respectalittlebit, "inverse_implication", appreciaterespect))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(love, "direct_implication", appreciaterespect))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(love, "direct_implication", appreciaterespectalittlebit))
                throw new Exception("Should be flat metaConnected because it's implicit");
        }

        private static void TestRecursiveInverseImplication()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept love = new Concept("love");
            Concept lovedby = new Concept("lovedby");
            Concept like = new Concept("like");
            Concept likedby = new Concept("likedby");
            Concept likeabit = new Concept("like a bit");
            Concept respect = new Concept("respect");
            Concept respected = new Concept("respected");
            Concept respectabit = new Concept("respect a bit");
            Concept respectedabit = new Concept("respected a bit");
            Concept likedabit = new Concept("liked a bit");

            MetaConnectionManager.AddMetaConnection(love, "inverse_of", lovedby);
            MetaConnectionManager.AddMetaConnection(like, "inverse_of", likedby);
            MetaConnectionManager.AddMetaConnection(likeabit, "inverse_of", likedabit);
            MetaConnectionManager.AddMetaConnection(respect, "inverse_of", respected);
            MetaConnectionManager.AddMetaConnection(respectabit, "inverse_of", respectedabit);
            MetaConnectionManager.AddMetaConnection(love, "direct_implication", like);
            MetaConnectionManager.AddMetaConnection(like, "direct_implication", likeabit);
            MetaConnectionManager.AddMetaConnection(likeabit, "inverse_implication", respect);
            MetaConnectionManager.AddMetaConnection(respect, "direct_implication", respectabit);

            if (!MetaConnectionManager.IsFlatMetaConnected(likeabit, "inverse_implication", respect))
                throw new Exception("Should be metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(like, "inverse_implication", respect))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(love, "inverse_implication", respect))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(likeabit, "inverse_implication", respectabit))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(like, "inverse_implication", respectabit))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(love, "inverse_implication", respectabit))
                throw new Exception("Should be metaConnected because it's implicit");
        }

        private static void TestPreRecursiveCant()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            #region We create some verbs
            Concept isa = new Concept("isa");
            Concept specializedisa = new Concept("specialized isa");
            Concept sspecializedisa = new Concept("sspecialized isa");
            Concept ssspecializedisa = new Concept("ssspecialized isa");
            Concept sssspecializedisa = new Concept("sssspecialized isa");
            Concept contradict = new Concept("contradict");

            Concept someare = new Concept("someare");
            Concept specializedsomeare = new Concept("specialized someare");
            Concept sspecializedsomeare = new Concept("sspecialized someare");
            Concept ssspecializedsomeare = new Concept("ssspecialized someare");
            Concept sssspecializedsomeare = new Concept("sssspecialized someare");
            #endregion

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(specializedisa, "inverse_of", specializedsomeare);
            MetaConnectionManager.AddMetaConnection(sspecializedisa, "inverse_of", sspecializedsomeare);
            MetaConnectionManager.AddMetaConnection(ssspecializedisa, "inverse_of", ssspecializedsomeare);
            MetaConnectionManager.AddMetaConnection(sssspecializedisa, "inverse_of", sssspecializedsomeare);

            MetaConnectionManager.AddMetaConnection(sssspecializedisa, "direct_implication", ssspecializedisa);
            MetaConnectionManager.AddMetaConnection(ssspecializedisa, "direct_implication", sspecializedisa);
            MetaConnectionManager.AddMetaConnection(sspecializedisa, "direct_implication", specializedisa);
            MetaConnectionManager.AddMetaConnection(specializedisa, "direct_implication", isa);
            MetaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            //testing pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "cant", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "cant", isa))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "cant", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "cant", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "direct_implication", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            //testing pre-recursivity on cant

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "cant", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "cant", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "cant", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "cant", sssspecializedsomeare))
                throw new Exception("should be flat metaConnected because it's explicit");
        }

        private static void TestPreRecursiveUnlikely()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            #region We create some verbs
            Concept isa = new Concept("isa");
            Concept specializedisa = new Concept("specialized isa");
            Concept sspecializedisa = new Concept("sspecialized isa");
            Concept ssspecializedisa = new Concept("ssspecialized isa");
            Concept sssspecializedisa = new Concept("sssspecialized isa");
            Concept contradict = new Concept("contradict");

            Concept someare = new Concept("someare");
            Concept specializedsomeare = new Concept("specialized someare");
            Concept sspecializedsomeare = new Concept("sspecialized someare");
            Concept ssspecializedsomeare = new Concept("ssspecialized someare");
            Concept sssspecializedsomeare = new Concept("sssspecialized someare");
            #endregion

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(specializedisa, "inverse_of", specializedsomeare);
            MetaConnectionManager.AddMetaConnection(sspecializedisa, "inverse_of", sspecializedsomeare);
            MetaConnectionManager.AddMetaConnection(ssspecializedisa, "inverse_of", ssspecializedsomeare);
            MetaConnectionManager.AddMetaConnection(sssspecializedisa, "inverse_of", sssspecializedsomeare);

            MetaConnectionManager.AddMetaConnection(sssspecializedisa, "direct_implication", ssspecializedisa);
            MetaConnectionManager.AddMetaConnection(ssspecializedisa, "direct_implication", sspecializedisa);
            MetaConnectionManager.AddMetaConnection(sspecializedisa, "direct_implication", specializedisa);
            MetaConnectionManager.AddMetaConnection(specializedisa, "direct_implication", isa);
            MetaConnectionManager.AddMetaConnection(isa, "unlikely", contradict);

            //testing pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", isa))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "direct_implication", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            //testing pre-recursivity on unlikely

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", sssspecializedsomeare))
                throw new Exception("should be flat metaConnected because it's explicit");
        }

        private static void TestPreRecursiveUnlikelyFromImplicitCant()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            #region We create some verbs
            Concept isa = new Concept("isa");
            Concept specializedisa = new Concept("specialized isa");
            Concept sspecializedisa = new Concept("sspecialized isa");
            Concept ssspecializedisa = new Concept("ssspecialized isa");
            Concept sssspecializedisa = new Concept("sssspecialized isa");
            Concept contradict = new Concept("contradict");

            Concept someare = new Concept("someare");
            Concept specializedsomeare = new Concept("specialized someare");
            Concept sspecializedsomeare = new Concept("sspecialized someare");
            Concept ssspecializedsomeare = new Concept("ssspecialized someare");
            Concept sssspecializedsomeare = new Concept("sssspecialized someare");
            #endregion

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(specializedisa, "inverse_of", specializedsomeare);
            MetaConnectionManager.AddMetaConnection(sspecializedisa, "inverse_of", sspecializedsomeare);
            MetaConnectionManager.AddMetaConnection(ssspecializedisa, "inverse_of", ssspecializedsomeare);
            MetaConnectionManager.AddMetaConnection(sssspecializedisa, "inverse_of", sssspecializedsomeare);

            MetaConnectionManager.AddMetaConnection(sssspecializedisa, "direct_implication", ssspecializedisa);
            MetaConnectionManager.AddMetaConnection(ssspecializedisa, "direct_implication", sspecializedisa);
            MetaConnectionManager.AddMetaConnection(sspecializedisa, "direct_implication", specializedisa);
            MetaConnectionManager.AddMetaConnection(specializedisa, "direct_implication", isa);
            MetaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            //testing pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", isa))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "direct_implication", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            //testing pre-recursivity on unlikely

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", sssspecializedsomeare))
                throw new Exception("should be flat metaConnected because it's explicit");
        }

        private static void TestPostRecursiveCant()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            #region We create some verbs
            Concept isa = new Concept("isa");
            Concept specializedisa = new Concept("specialized isa");
            Concept sspecializedisa = new Concept("sspecialized isa");
            Concept ssspecializedisa = new Concept("ssspecialized isa");
            Concept sssspecializedisa = new Concept("sssspecialized isa");
            Concept contradict = new Concept("contradict");
            Concept specializedcontradict = new Concept("specialized contradict");
            Concept sspecializedcontradict = new Concept("sspecialized contradict");
            Concept ssspecializedcontradict = new Concept("ssspecialized contradict");
            Concept sssspecializedcontradict = new Concept("sssspecialized contradict");

            Concept someare = new Concept("someare");
            Concept specializedsomeare = new Concept("specialized someare");
            Concept sspecializedsomeare = new Concept("sspecialized someare");
            Concept ssspecializedsomeare = new Concept("ssspecialized someare");
            Concept sssspecializedsomeare = new Concept("sssspecialized someare");
            #endregion

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(specializedisa, "inverse_of", specializedsomeare);
            MetaConnectionManager.AddMetaConnection(sspecializedisa, "inverse_of", sspecializedsomeare);
            MetaConnectionManager.AddMetaConnection(ssspecializedisa, "inverse_of", ssspecializedsomeare);
            MetaConnectionManager.AddMetaConnection(sssspecializedisa, "inverse_of", sssspecializedsomeare);
            MetaConnectionManager.AddMetaConnection(sssspecializedcontradict, "permutable_side", sssspecializedcontradict);
            MetaConnectionManager.AddMetaConnection(ssspecializedcontradict, "permutable_side", ssspecializedcontradict);
            MetaConnectionManager.AddMetaConnection(sspecializedcontradict, "permutable_side", sspecializedcontradict);
            MetaConnectionManager.AddMetaConnection(specializedcontradict, "permutable_side", specializedcontradict);
            MetaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);

            MetaConnectionManager.AddMetaConnection(sssspecializedcontradict, "direct_implication", ssspecializedcontradict);
            MetaConnectionManager.AddMetaConnection(ssspecializedcontradict, "direct_implication", sspecializedcontradict);
            MetaConnectionManager.AddMetaConnection(sspecializedcontradict, "direct_implication", specializedcontradict);
            MetaConnectionManager.AddMetaConnection(specializedcontradict, "direct_implication", contradict);

            MetaConnectionManager.AddMetaConnection(sssspecializedisa, "direct_implication", ssspecializedisa);
            MetaConnectionManager.AddMetaConnection(ssspecializedisa, "direct_implication", sspecializedisa);
            MetaConnectionManager.AddMetaConnection(sspecializedisa, "direct_implication", specializedisa);
            MetaConnectionManager.AddMetaConnection(specializedisa, "direct_implication", isa);
            MetaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            //testing pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "cant", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "cant", isa))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "cant", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "cant", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "direct_implication", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            //testing pre-recursivity on cant

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "cant", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "cant", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "cant", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "cant", sssspecializedsomeare))
                throw new Exception("should be flat metaConnected because it's explicit");

            //testing post-recursivity on cant

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "cant", sssspecializedcontradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "cant", sssspecializedcontradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedcontradict, "cant", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedcontradict, "cant", sssspecializedsomeare))
                throw new Exception("should be flat metaConnected because it's explicit");
        }

        private static void TestPostRecursiveUnlikely()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            #region We create some verbs
            Concept isa = new Concept("isa");
            Concept specializedisa = new Concept("specialized isa");
            Concept sspecializedisa = new Concept("sspecialized isa");
            Concept ssspecializedisa = new Concept("ssspecialized isa");
            Concept sssspecializedisa = new Concept("sssspecialized isa");
            Concept contradict = new Concept("contradict");
            Concept specializedcontradict = new Concept("specialized contradict");
            Concept sspecializedcontradict = new Concept("sspecialized contradict");
            Concept ssspecializedcontradict = new Concept("ssspecialized contradict");
            Concept sssspecializedcontradict = new Concept("sssspecialized contradict");

            Concept someare = new Concept("someare");
            Concept specializedsomeare = new Concept("specialized someare");
            Concept sspecializedsomeare = new Concept("sspecialized someare");
            Concept ssspecializedsomeare = new Concept("ssspecialized someare");
            Concept sssspecializedsomeare = new Concept("sssspecialized someare");
            #endregion

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(specializedisa, "inverse_of", specializedsomeare);
            MetaConnectionManager.AddMetaConnection(sspecializedisa, "inverse_of", sspecializedsomeare);
            MetaConnectionManager.AddMetaConnection(ssspecializedisa, "inverse_of", ssspecializedsomeare);
            MetaConnectionManager.AddMetaConnection(sssspecializedisa, "inverse_of", sssspecializedsomeare);
            MetaConnectionManager.AddMetaConnection(sssspecializedcontradict, "permutable_side", sssspecializedcontradict);
            MetaConnectionManager.AddMetaConnection(ssspecializedcontradict, "permutable_side", ssspecializedcontradict);
            MetaConnectionManager.AddMetaConnection(sspecializedcontradict, "permutable_side", sspecializedcontradict);
            MetaConnectionManager.AddMetaConnection(specializedcontradict, "permutable_side", specializedcontradict);
            MetaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);

            MetaConnectionManager.AddMetaConnection(sssspecializedcontradict, "direct_implication", ssspecializedcontradict);
            MetaConnectionManager.AddMetaConnection(ssspecializedcontradict, "direct_implication", sspecializedcontradict);
            MetaConnectionManager.AddMetaConnection(sspecializedcontradict, "direct_implication", specializedcontradict);
            MetaConnectionManager.AddMetaConnection(specializedcontradict, "direct_implication", contradict);

            MetaConnectionManager.AddMetaConnection(sssspecializedisa, "direct_implication", ssspecializedisa);
            MetaConnectionManager.AddMetaConnection(ssspecializedisa, "direct_implication", sspecializedisa);
            MetaConnectionManager.AddMetaConnection(sspecializedisa, "direct_implication", specializedisa);
            MetaConnectionManager.AddMetaConnection(specializedisa, "direct_implication", isa);
            MetaConnectionManager.AddMetaConnection(isa, "unlikely", contradict);

            //testing pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", isa))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "direct_implication", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            //testing pre-recursivity on unlikely

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", sssspecializedsomeare))
                throw new Exception("should be flat metaConnected because it's explicit");

            //testing post-recursivity on unlikely

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "unlikely", sssspecializedcontradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "unlikely", sssspecializedcontradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedcontradict, "unlikely", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedcontradict, "unlikely", sssspecializedsomeare))
                throw new Exception("should be flat metaConnected because it's explicit");
        }

        private static void TestPostRecursiveUnlikelyFromImplicitCant()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            #region We create some verbs
            Concept isa = new Concept("isa");
            Concept specializedisa = new Concept("specialized isa");
            Concept sspecializedisa = new Concept("sspecialized isa");
            Concept ssspecializedisa = new Concept("ssspecialized isa");
            Concept sssspecializedisa = new Concept("sssspecialized isa");
            Concept contradict = new Concept("contradict");
            Concept specializedcontradict = new Concept("specialized contradict");
            Concept sspecializedcontradict = new Concept("sspecialized contradict");
            Concept ssspecializedcontradict = new Concept("ssspecialized contradict");
            Concept sssspecializedcontradict = new Concept("sssspecialized contradict");

            Concept someare = new Concept("someare");
            Concept specializedsomeare = new Concept("specialized someare");
            Concept sspecializedsomeare = new Concept("sspecialized someare");
            Concept ssspecializedsomeare = new Concept("ssspecialized someare");
            Concept sssspecializedsomeare = new Concept("sssspecialized someare");
            #endregion

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(specializedisa, "inverse_of", specializedsomeare);
            MetaConnectionManager.AddMetaConnection(sspecializedisa, "inverse_of", sspecializedsomeare);
            MetaConnectionManager.AddMetaConnection(ssspecializedisa, "inverse_of", ssspecializedsomeare);
            MetaConnectionManager.AddMetaConnection(sssspecializedisa, "inverse_of", sssspecializedsomeare);
            MetaConnectionManager.AddMetaConnection(sssspecializedcontradict, "permutable_side", sssspecializedcontradict);
            MetaConnectionManager.AddMetaConnection(ssspecializedcontradict, "permutable_side", ssspecializedcontradict);
            MetaConnectionManager.AddMetaConnection(sspecializedcontradict, "permutable_side", sspecializedcontradict);
            MetaConnectionManager.AddMetaConnection(specializedcontradict, "permutable_side", specializedcontradict);
            MetaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);

            MetaConnectionManager.AddMetaConnection(sssspecializedcontradict, "direct_implication", ssspecializedcontradict);
            MetaConnectionManager.AddMetaConnection(ssspecializedcontradict, "direct_implication", sspecializedcontradict);
            MetaConnectionManager.AddMetaConnection(sspecializedcontradict, "direct_implication", specializedcontradict);
            MetaConnectionManager.AddMetaConnection(specializedcontradict, "direct_implication", contradict);

            MetaConnectionManager.AddMetaConnection(sssspecializedisa, "direct_implication", ssspecializedisa);
            MetaConnectionManager.AddMetaConnection(ssspecializedisa, "direct_implication", sspecializedisa);
            MetaConnectionManager.AddMetaConnection(sspecializedisa, "direct_implication", specializedisa);
            MetaConnectionManager.AddMetaConnection(specializedisa, "direct_implication", isa);
            MetaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            //testing pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", isa))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "direct_implication", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            //testing pre-recursivity on unlikely

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", sssspecializedsomeare))
                throw new Exception("should be flat metaConnected because it's explicit");

            //testing post-recursivity on unlikely

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "unlikely", sssspecializedcontradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "unlikely", sssspecializedcontradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedcontradict, "unlikely", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(sssspecializedcontradict, "unlikely", sssspecializedsomeare))
                throw new Exception("should be flat metaConnected because it's explicit");
        }

        private static void TestRecursiveSelfMuctSublarLiffidConsics()
        {
            TestRecursiveSelfMuct();
            TestRecursiveSelfLiffid();
            TestRecursiveSelfSublar();
            TestRecursiveSelfConsics();
        }

        private static void TestRecursiveSelfMuct()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");

            //Connections that shouldn't be present yet

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "sublar", someare))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "consics", isa))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "sublar", isa))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "consics", someare))
                throw new Exception("Should be metaConnected yet");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(isa, "muct", isa);

            //Connections that should be present

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "sublar", someare))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "consics", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            //Connections that shouldn't be present
            /*if (MetaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected because it's illogical");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected because it's illogical");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "sublar", isa))
                throw new Exception("Should be metaConnected because it's illogical");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "consics", someare))
                throw new Exception("Should be metaConnected because it's illogical");*/
        }

        private static void TestRecursiveSelfSublar()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");

            //Connections that shouldn't be present yet

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "sublar", someare))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "consics", isa))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "sublar", isa))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "consics", someare))
                throw new Exception("Should be metaConnected yet");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(isa, "sublar", someare);

            //Connections that should be present

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "sublar", someare))
                throw new Exception("Should be metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "consics", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            //Connections that shouldn't be present
            /*if (MetaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected because it's illogical");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected because it's illogical");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "sublar", isa))
                throw new Exception("Should be metaConnected because it's illogical");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "consics", someare))
                throw new Exception("Should be metaConnected because it's illogical");*/
        }

        private static void TestRecursiveSelfLiffid()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");

            //Connections that shouldn't be present yet

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "sublar", someare))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "consics", isa))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "sublar", isa))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "consics", someare))
                throw new Exception("Should be metaConnected yet");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(someare, "muct", someare);

            //Connections that should be present

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "consics", someare))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "sublar", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            //Connections that shouldn't be present
            /*if (MetaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected because it's illogical");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected because it's illogical");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "consics", isa))
                throw new Exception("Should be metaConnected because it's illogical");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "sublar", someare))
                throw new Exception("Should be metaConnected because it's illogical");*/
        }

        private static void TestRecursiveSelfConsics()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");

            //Connections that shouldn't be present yet

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "sublar", someare))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "consics", isa))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "sublar", isa))
                throw new Exception("Should be metaConnected yet");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "consics", someare))
                throw new Exception("Should be metaConnected yet");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(isa, "consics", someare);

            //Connections that should be present

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "consics", someare))
                throw new Exception("Should be metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "sublar", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            //Connections that shouldn't be present
            /*if (MetaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected because it's illogical");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected because it's illogical");

            if (MetaConnectionManager.IsFlatMetaConnected(someare, "consics", isa))
                throw new Exception("Should be metaConnected because it's illogical");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "sublar", someare))
                throw new Exception("Should be metaConnected because it's illogical");*/
        }

        private static void TestIsaMuctIsaFromMadeofMuctIsa()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(madeof,"muct",isa);

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected because it's implicit and very logical");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected because it's implicit and very logical and implicit from preceding expression");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected because self muct is equivalent to self liffid");

            if (!MetaConnectionManager.IsFlatMetaConnected(isa,"liffid",isa))
                throw new Exception("Should be metaConnected because self muct is equivalent to self liffid");
        }

        private static void TestIsaMuctIsaFromMadeofLiffidIsa()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected because it's implicit and very logical");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected because it's implicit and very logical and implicit from preceding expression");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected because self muct is equivalent to self liffid");

            if (!MetaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected because self muct is equivalent to self liffid");
        }

        private static void TestMadeofMustNotMuctNorLiffidNeed()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept need = new Concept("need");
            Concept allow = new Concept("allow");

            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(need, "inverse_of", allow);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);
            MetaConnectionManager.AddMetaConnection(madeof, "direct_implication", need);

            //Real test here

            if (MetaConnectionManager.IsFlatMetaConnected(madeof, "muct", need))
                throw new Exception("Shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(madeof, "liffid", need))
                throw new Exception("Shouldn't be flat metaConnected");
        }

        private static void TestMadeofMustNotMuctNorLiffidNeedHarderTestWithInverseOf()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept need = new Concept("need");
            Concept allow = new Concept("allow");

            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(need, "inverse_of", allow);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);
            MetaConnectionManager.AddMetaConnection(madeof, "direct_implication", need);

            //Pre-conditions

            if (MetaConnectionManager.IsFlatMetaConnected(madeof, "direct_implication", allow))
                throw new Exception("Shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(madeof, "inverse_implication", need))
                throw new Exception("Shouldn't be flat metaConnected");

            //Real test here

            if (MetaConnectionManager.IsFlatMetaConnected(madeof, "sublar", allow))
                throw new Exception("Shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(madeof, "muct", need))
                throw new Exception("Shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(madeof, "liffid", need))
                throw new Exception("Shouldn't be flat metaConnected");
        }

        private static void TestMakeMustNotLiffidNeed()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept need = new Concept("need");
            Concept allow = new Concept("allow");
            Concept make = new Concept("make");
            Concept madeby = new Concept("madeby");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            MetaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            MetaConnectionManager.AddMetaConnection(need, "inverse_of", allow);
            MetaConnectionManager.AddMetaConnection(make, "inverse_of", madeby);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(madeof, "muct", madeof);
            MetaConnectionManager.AddMetaConnection(make, "liffid", isa);
            MetaConnectionManager.AddMetaConnection(need, "liffid", need);
            MetaConnectionManager.AddMetaConnection(madeof, "direct_implication", need);

            MetaConnectionManager.AddMetaConnection(isa, "direct_implication", need);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(need, "liffid", need))
                throw new Exception("Should be flat metaConnected");

            if (!MetaConnectionManager.IsFlatMetaConnected(need, "muct", need))
                throw new Exception("Should be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(isa, "liffid", need))
                throw new Exception("Shouldn't be flat metaConnected");

            //Real test here           

            if (MetaConnectionManager.IsFlatMetaConnected(make, "liffid", need))
                throw new Exception("Shouldn't be flat metaConnected");
        }

        private static void TestCantFromImplicitInverseOf()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");

            MetaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);

            //Real test here

            if (!MetaConnectionManager.IsFlatMetaConnected(isa,"cant",someare))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(someare,"cant",isa))
                throw new Exception("Should be flat metaConnected because it's implicit");
        }

        private static void TestNoSelfCant()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept make = new Concept("make");
            Concept madeby = new Concept("madeby");
            Concept create = new Concept("create");
            Concept createdby = new Concept("createdby");

            MetaConnectionManager.AddMetaConnection(make, "inverse_of", madeby);
            MetaConnectionManager.AddMetaConnection(make, "cant", madeby);
            MetaConnectionManager.AddMetaConnection(create, "inverse_of", createdby);
            MetaConnectionManager.AddMetaConnection(create, "direct_implication", make);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(createdby, "direct_implication", madeby))
                throw new Exception("Should be flat metaConnected");

            //Real test here

            if (MetaConnectionManager.IsFlatMetaConnected(make, "cant", make))
                throw new Exception("Shouldn't be flat metaConnected");


            if (MetaConnectionManager.IsFlatMetaConnected(madeby, "cant", madeby))
                throw new Exception("Shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(make, "cant", create))
                throw new Exception("Shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(create, "cant", make))
                throw new Exception("Shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(make, "unlikely", make))
                throw new Exception("Shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(make, "unlikely", create))
                throw new Exception("Shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(createdby, "cant", madeby))
                throw new Exception("Shouldn't be flat metaConnected");

            if (MetaConnectionManager.IsFlatMetaConnected(createdby, "unlikely", madeby))
                throw new Exception("Shouldn't be flat metaConnected");
        }

        private static void TestOrbitedByDirectImplicationGravity()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            

            Concept orbit = new Concept("orbit");
            Concept orbitedby = new Concept("orbited by");
            Concept gravity = new Concept("gravity");

            MetaConnectionManager.AddMetaConnection(orbit, "inverse_of", orbitedby);
            MetaConnectionManager.AddMetaConnection(gravity, "permutable_side", gravity);
            MetaConnectionManager.AddMetaConnection(orbit, "direct_implication", gravity);

            //Pre-conditions

            if (!MetaConnectionManager.IsFlatMetaConnected(orbit, "direct_implication", gravity))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(orbitedby, "inverse_implication", gravity))
                throw new Exception("Should be flat metaConnected because it's explicit");

            //Real test here

            if (!MetaConnectionManager.IsFlatMetaConnected(orbit, "inverse_implication", gravity))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!MetaConnectionManager.IsFlatMetaConnected(orbitedby, "direct_implication", gravity))
                throw new Exception("Should be flat metaConnected because it's implicit");
        }
    }
}
