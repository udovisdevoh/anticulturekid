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
        }

        private static void TestMuctSublarLiffidConsics()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

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
            metaConnectionManager.AddMetaConnection(isa, "muct", isa);
            metaConnectionManager.AddMetaConnection(isa, "cant", someare);
            metaConnectionManager.AddMetaConnection(isa, "cant", madeof);
            metaConnectionManager.AddMetaConnection(isa, "cant", partof);
            metaConnectionManager.AddMetaConnection(madeof, "muct", madeof);

            metaConnectionManager.AddMetaConnection(madeof, "unlikely", isa);
            metaConnectionManager.AddMetaConnection(madeof, "cant", partof);
            metaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);
            metaConnectionManager.AddMetaConnection(contradict, "muct", isa);
            metaConnectionManager.AddMetaConnection(contradict, "cant", isa);



            metaConnectionManager.AddMetaConnection(isa2, "muct", isa2);
            metaConnectionManager.AddMetaConnection(isa2, "cant", someare2);
            metaConnectionManager.AddMetaConnection(isa2, "cant", madeof2);
            metaConnectionManager.AddMetaConnection(isa2, "cant", partof2);
            metaConnectionManager.AddMetaConnection(madeof2, "muct", madeof2);
            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);

            metaConnectionManager.AddMetaConnection(madeof2, "unlikely", isa2);
            metaConnectionManager.AddMetaConnection(madeof2, "cant", partof2);
            metaConnectionManager.AddMetaConnection(contradict2, "permutable_side", contradict2);
            metaConnectionManager.AddMetaConnection(contradict2, "muct", isa2);
            metaConnectionManager.AddMetaConnection(contradict2, "cant", isa2);
            metaConnectionManager.AddMetaConnection(isa2, "inverse_of", someare2);
            metaConnectionManager.AddMetaConnection(madeof2, "inverse_of", partof2);

            metaConnectionManager.AddMetaConnection(isa09, "inverse_of", someare09);
            metaConnectionManager.AddMetaConnection(madeof09, "inverse_of", partof09);

            metaConnectionManager.AddMetaConnection(isa10, "inverse_of", someare10);
            metaConnectionManager.AddMetaConnection(madeof10, "inverse_of", partof10);
            #endregion

            #region Testing muct, sublar, liffid and consics
            #region Testing muct
            if (metaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(madeof, "sublar", someare))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(partof, "consics", isa))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(partof, "liffid", someare))
                throw new Exception("Operators shouldn't be flat metaConnected");

            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);

            if (!metaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Operators should be flat metaConnected");

            if (!metaConnectionManager.IsFlatMetaConnected(madeof, "sublar", someare))
                throw new Exception("Operators should be flat metaConnected");

            if (!metaConnectionManager.IsFlatMetaConnected(partof, "consics", isa))
                throw new Exception("Operators should be flat metaConnected");

            if (!metaConnectionManager.IsFlatMetaConnected(partof, "liffid", someare))
                throw new Exception("Operators should be flat metaConnected");


            if (metaConnectionManager.IsFlatMetaConnected(madeof, "sublar", isa))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(madeof, "liffid", isa))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(madeof, "consics", isa))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");
            #endregion

            #region Testing liffid
            if (metaConnectionManager.IsFlatMetaConnected(madeof09, "liffid", isa09))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(madeof09, "consics", someare09))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(partof09, "sublar", isa09))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(partof09, "muct", someare09))
                throw new Exception("Operators shouldn't be flat metaConnected");

            metaConnectionManager.AddMetaConnection(madeof09, "liffid", isa09);

            if (!metaConnectionManager.IsFlatMetaConnected(madeof09, "liffid", isa09))
                throw new Exception("Operators should be flat metaConnected");

            if (!metaConnectionManager.IsFlatMetaConnected(madeof09, "consics", someare09))
                throw new Exception("Operators should be flat metaConnected");

            if (!metaConnectionManager.IsFlatMetaConnected(partof09, "sublar", isa09))
                throw new Exception("Operators should be flat metaConnected");

            if (!metaConnectionManager.IsFlatMetaConnected(partof09, "muct", someare09))
                throw new Exception("Operators should be flat metaConnected");


            if (metaConnectionManager.IsFlatMetaConnected(madeof09, "muct", isa09))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(madeof09, "consics", isa09))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(madeof09, "sublar", isa09))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");
            #endregion

            #region Testing consics
            if (metaConnectionManager.IsFlatMetaConnected(madeof2, "consics", someare2))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(madeof2, "liffid", isa2))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(partof2, "sublar", isa2))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(partof2, "muct", someare2))
                throw new Exception("Operators shouldn't be flat metaConnected");

            metaConnectionManager.AddMetaConnection(madeof2, "consics", someare2);

            if (!metaConnectionManager.IsFlatMetaConnected(madeof2, "consics", someare2))
                throw new Exception("Operators should be flat metaConnected");

            if (!metaConnectionManager.IsFlatMetaConnected(madeof2, "liffid", isa2))
                throw new Exception("Operators should be flat metaConnected");

            if (!metaConnectionManager.IsFlatMetaConnected(partof2, "sublar", isa2))
                throw new Exception("Operators should be flat metaConnected");

            if (!metaConnectionManager.IsFlatMetaConnected(partof2, "muct", someare2))
                throw new Exception("Operators should be flat metaConnected");


            if (metaConnectionManager.IsFlatMetaConnected(madeof2, "muct", someare2))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(madeof2, "sublar", someare2))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(madeof2, "liffid", someare2))
                throw new Exception("Operators shouldn't be accidentally flat metaConnected");
            #endregion

            #region Testing sublar
            if (metaConnectionManager.IsFlatMetaConnected(madeof10, "sublar", someare10))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(madeof10, "muct", isa10))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(partof10, "consics", isa10))
                throw new Exception("Operators shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(partof10, "liffid", someare10))
                throw new Exception("Operators shouldn't be flat metaConnected");

            metaConnectionManager.AddMetaConnection(madeof10, "sublar", someare10);

            if (!metaConnectionManager.IsFlatMetaConnected(madeof10, "sublar", someare10))
                throw new Exception("Operators should be flat metaConnected");

            if (!metaConnectionManager.IsFlatMetaConnected(madeof10, "muct", isa10))
                throw new Exception("Operators should be flat metaConnected");

            if (!metaConnectionManager.IsFlatMetaConnected(partof10, "consics", isa10))
                throw new Exception("Operators should be flat metaConnected");

            if (!metaConnectionManager.IsFlatMetaConnected(partof10, "liffid", someare10))
                throw new Exception("Operators should be flat metaConnected");


            if (metaConnectionManager.IsFlatMetaConnected(madeof10, "muct", someare10))
                throw new Exception("Operators shouldnn't accidentally be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(madeof10, "liffid", someare10))
                throw new Exception("Operators shouldnn't accidentally be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(madeof10, "consics", someare10))
                throw new Exception("Operators shouldnn't accidentally be flat metaConnected");
            #endregion
            #endregion
        }

        private static void TestCant()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

            #region We create the verbs that we will need
            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept contradict = new Concept("contradict");
            #endregion

            if (metaConnectionManager.IsFlatMetaConnected(isa, "cant", contradict))
                throw new Exception("Shouldn't be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(contradict, "cant", isa))
                throw new Exception("Shouldn't be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "cant", contradict))
                throw new Exception("Shouldn't be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(contradict, "cant", someare))
                throw new Exception("Shouldn't be metaConnected yet");

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "cant", contradict))
                throw new Exception("Should be metaConnected because we told so");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "cant", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "cant", contradict))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "cant", someare))
                throw new Exception("Should be metaConnected because it's implicit");
        }

        private static void TestUnlikely()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

            #region We create the verbs that we will need
            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept contradict = new Concept("contradict");
            #endregion

            if (metaConnectionManager.IsFlatMetaConnected(isa, "unlikely", contradict))
                throw new Exception("Shouldn't be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", isa))
                throw new Exception("Shouldn't be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "unlikely", contradict))
                throw new Exception("Shouldn't be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", someare))
                throw new Exception("Shouldn't be metaConnected yet");

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(isa, "unlikely", contradict);

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "unlikely", contradict))
                throw new Exception("Should be metaConnected because we told so");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "unlikely", contradict))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", someare))
                throw new Exception("Should be metaConnected because it's implicit");
        }

        private static void TestUnlikelyFromImplicitCant()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

            #region We create the verbs that we will need
            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept contradict = new Concept("contradict");
            #endregion

            if (metaConnectionManager.IsFlatMetaConnected(isa, "unlikely", contradict))
                throw new Exception("Shouldn't be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", isa))
                throw new Exception("Shouldn't be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "unlikely", contradict))
                throw new Exception("Shouldn't be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", someare))
                throw new Exception("Shouldn't be metaConnected yet");

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "unlikely", contradict))
                throw new Exception("Should be metaConnected because we told so");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "unlikely", contradict))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", someare))
                throw new Exception("Should be metaConnected because it's implicit");
        }

        private static void TestDirectImplication()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            //The child loves the mother because he's made by her

            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

            #region We create some verbs
            Concept make = new Concept("make");
            Concept madeby = new Concept("madeby");
            Concept love = new Concept("love");
            Concept lovedby = new Concept("lovedby");
            #endregion

            #region We plug the verbs together
            metaConnectionManager.AddMetaConnection(make, "inverse_of", madeby);
            metaConnectionManager.AddMetaConnection(love, "inverse_of", lovedby);
            metaConnectionManager.AddMetaConnection(make, "inverse_implication", love);
            #endregion

            if (!metaConnectionManager.IsFlatMetaConnected(make, "inverse_implication", love))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(madeby, "inverse_implication", lovedby))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(make, "direct_implication", lovedby))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(madeby, "direct_implication", love))
                throw new Exception("Should be flat metaConnected because it's implicit");
        }

        private static void TestInverseImplication()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            //The mother loves the child because she made it

            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

            #region We create some verbs
            Concept make = new Concept("make");
            Concept madeby = new Concept("madeby");
            Concept love = new Concept("love");
            Concept lovedby = new Concept("lovedby");
            #endregion

            #region We plug the verbs together
            metaConnectionManager.AddMetaConnection(make, "inverse_of", madeby);
            metaConnectionManager.AddMetaConnection(love, "inverse_of", lovedby);
            metaConnectionManager.AddMetaConnection(make, "direct_implication", love);
            #endregion

            if (!metaConnectionManager.IsFlatMetaConnected(make, "direct_implication", love))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(madeby, "direct_implication", lovedby))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(make, "inverse_implication", lovedby))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(madeby, "inverse_implication", love))
                throw new Exception("Should be flat metaConnected because it's implicit");
        }

        private static void TestRecursiveMuctSublarLiffidConsics()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

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
            metaConnectionManager.AddMetaConnection(isa, "muct", isa);
            metaConnectionManager.AddMetaConnection(isa, "cant", someare);
            metaConnectionManager.AddMetaConnection(isa, "cant", madeof);
            metaConnectionManager.AddMetaConnection(isa, "cant", partof);
            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(madeof, "muct", madeof);
            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);

            metaConnectionManager.AddMetaConnection(specializedMaterialMadeof, "direct_implication", materialMadeof);
            metaConnectionManager.AddMetaConnection(materialMadeof, "direct_implication", madeof);
            metaConnectionManager.AddMetaConnection(isa, "direct_implication", generalIsa);

            metaConnectionManager.AddMetaConnection(generalSomeare, "inverse_of", generalIsa);
            metaConnectionManager.AddMetaConnection(materialPartof, "inverse_of", materialMadeof);
            metaConnectionManager.AddMetaConnection(specializedMaterialPartof, "inverse_of", specializedMaterialMadeof);
            #endregion

            #region Testing muct
            if (!metaConnectionManager.IsFlatMetaConnected(madeof, "muct", isa))
                throw new Exception("Operators should be metaconnected because we told so");

            if (!metaConnectionManager.IsFlatMetaConnected(materialMadeof, "muct", isa))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(specializedMaterialMadeof, "muct", isa))
                throw new Exception("Operators should be metaconnected because it's implicit");


            if (!metaConnectionManager.IsFlatMetaConnected(madeof, "muct", generalIsa))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(materialMadeof, "muct", generalIsa))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(specializedMaterialMadeof, "muct", generalIsa))
                throw new Exception("Operators should be metaconnected because it's implicit");
            #endregion

            #region Testing liffid
            if (!metaConnectionManager.IsFlatMetaConnected(partof, "liffid", someare))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(materialPartof, "liffid", someare))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(specializedMaterialPartof, "liffid", someare))
                throw new Exception("Operators should be metaconnected because it's implicit");


            if (!metaConnectionManager.IsFlatMetaConnected(partof, "liffid", generalSomeare))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(materialPartof, "liffid", generalSomeare))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(specializedMaterialPartof, "liffid", generalSomeare))
                throw new Exception("Operators should be metaconnected because it's implicit");
            #endregion

            #region Testing sublar
            if (!metaConnectionManager.IsFlatMetaConnected(madeof, "sublar", someare))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(materialMadeof, "sublar", someare))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(specializedMaterialMadeof, "sublar", someare))
                throw new Exception("Operators should be metaconnected because it's implicit");


            if (!metaConnectionManager.IsFlatMetaConnected(madeof, "sublar", generalSomeare))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(materialMadeof, "sublar", generalSomeare))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(specializedMaterialMadeof, "sublar", generalSomeare))
                throw new Exception("Operators should be metaconnected because it's implicit");
            #endregion

            #region Testing consics
            if (!metaConnectionManager.IsFlatMetaConnected(partof, "consics", isa))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(materialPartof, "consics", isa))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(specializedMaterialPartof, "consics", isa))
                throw new Exception("Operators should be metaconnected because it's implicit");


            if (!metaConnectionManager.IsFlatMetaConnected(partof, "consics", generalIsa))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(materialPartof, "consics", generalIsa))
                throw new Exception("Operators should be metaconnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(specializedMaterialPartof, "consics", generalIsa))
                throw new Exception("Operators should be metaconnected because it's implicit");
            #endregion
        }

        private static void TestRecursiveDirectImplication()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

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

            metaConnectionManager.AddMetaConnection(likeabit, "inverse_of", likedabit);
            metaConnectionManager.AddMetaConnection(likealittlebit, "inverse_of", likedalittlebit);
            metaConnectionManager.AddMetaConnection(love, "inverse_of", lovedby);
            metaConnectionManager.AddMetaConnection(like, "inverse_of", likedby);
            metaConnectionManager.AddMetaConnection(love, "direct_implication", like);
            metaConnectionManager.AddMetaConnection(like, "direct_implication", likeabit);
            metaConnectionManager.AddMetaConnection(likeabit, "direct_implication", likealittlebit);

            if (!metaConnectionManager.IsFlatMetaConnected(love, "direct_implication", like))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(love, "direct_implication", likeabit))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(love, "direct_implication", likealittlebit))
                throw new Exception("Should be flat metaConnected because it's implicit");
        }

        private static void TestRecursiveDirectImplicationFromDoubleReverse()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            //Person A loves person B, therefore, person B respect person A and person A appreciates the respect
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

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

            metaConnectionManager.AddMetaConnection(appreciaterespectalittlebit, "inverse_of", appreciatedrespectalittlebit);
            metaConnectionManager.AddMetaConnection(likealittlebit, "inverse_of", likedalittlebit);
            metaConnectionManager.AddMetaConnection(love, "inverse_of", lovedby);
            metaConnectionManager.AddMetaConnection(like, "inverse_of", likedby);
            metaConnectionManager.AddMetaConnection(likeabit, "inverse_of", likedabit);
            metaConnectionManager.AddMetaConnection(respectalittlebit, "inverse_of", respectedalittlebit);
            metaConnectionManager.AddMetaConnection(appreciaterespect, "inverse_of", appreciaterespected);
            metaConnectionManager.AddMetaConnection(love, "direct_implication", like);
            metaConnectionManager.AddMetaConnection(like, "direct_implication", likeabit);
            metaConnectionManager.AddMetaConnection(likeabit, "direct_implication", likealittlebit);
            metaConnectionManager.AddMetaConnection(likeabit, "inverse_implication", respectalittlebit);
            metaConnectionManager.AddMetaConnection(respectalittlebit, "inverse_implication", appreciaterespect);
            metaConnectionManager.AddMetaConnection(appreciaterespect, "direct_implication", appreciaterespectalittlebit);

            if (!metaConnectionManager.IsFlatMetaConnected(love, "direct_implication", like))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(love, "direct_implication", likeabit))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(love, "direct_implication", likealittlebit))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(likeabit, "inverse_implication", respectalittlebit))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(like, "inverse_implication", respectalittlebit))
                throw new Exception("Should be flat metaConnected because it's implicit");

            //More intensive test

            if (!metaConnectionManager.IsFlatMetaConnected(love, "inverse_implication", respectalittlebit))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(respectalittlebit, "inverse_implication", appreciaterespect))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(love, "direct_implication", appreciaterespect))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(love, "direct_implication", appreciaterespectalittlebit))
                throw new Exception("Should be flat metaConnected because it's implicit");
        }

        private static void TestRecursiveInverseImplication()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

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

            metaConnectionManager.AddMetaConnection(love, "inverse_of", lovedby);
            metaConnectionManager.AddMetaConnection(like, "inverse_of", likedby);
            metaConnectionManager.AddMetaConnection(likeabit, "inverse_of", likedabit);
            metaConnectionManager.AddMetaConnection(respect, "inverse_of", respected);
            metaConnectionManager.AddMetaConnection(respectabit, "inverse_of", respectedabit);
            metaConnectionManager.AddMetaConnection(love, "direct_implication", like);
            metaConnectionManager.AddMetaConnection(like, "direct_implication", likeabit);
            metaConnectionManager.AddMetaConnection(likeabit, "inverse_implication", respect);
            metaConnectionManager.AddMetaConnection(respect, "direct_implication", respectabit);

            if (!metaConnectionManager.IsFlatMetaConnected(likeabit, "inverse_implication", respect))
                throw new Exception("Should be metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(like, "inverse_implication", respect))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(love, "inverse_implication", respect))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(likeabit, "inverse_implication", respectabit))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(like, "inverse_implication", respectabit))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(love, "inverse_implication", respectabit))
                throw new Exception("Should be metaConnected because it's implicit");
        }

        private static void TestPreRecursiveCant()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

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

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(specializedisa, "inverse_of", specializedsomeare);
            metaConnectionManager.AddMetaConnection(sspecializedisa, "inverse_of", sspecializedsomeare);
            metaConnectionManager.AddMetaConnection(ssspecializedisa, "inverse_of", ssspecializedsomeare);
            metaConnectionManager.AddMetaConnection(sssspecializedisa, "inverse_of", sssspecializedsomeare);

            metaConnectionManager.AddMetaConnection(sssspecializedisa, "direct_implication", ssspecializedisa);
            metaConnectionManager.AddMetaConnection(ssspecializedisa, "direct_implication", sspecializedisa);
            metaConnectionManager.AddMetaConnection(sspecializedisa, "direct_implication", specializedisa);
            metaConnectionManager.AddMetaConnection(specializedisa, "direct_implication", isa);
            metaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            //testing pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "cant", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "cant", isa))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "cant", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "cant", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "direct_implication", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            //testing pre-recursivity on cant

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "cant", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "cant", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "cant", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "cant", sssspecializedsomeare))
                throw new Exception("should be flat metaConnected because it's explicit");
        }

        private static void TestPreRecursiveUnlikely()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

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

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(specializedisa, "inverse_of", specializedsomeare);
            metaConnectionManager.AddMetaConnection(sspecializedisa, "inverse_of", sspecializedsomeare);
            metaConnectionManager.AddMetaConnection(ssspecializedisa, "inverse_of", ssspecializedsomeare);
            metaConnectionManager.AddMetaConnection(sssspecializedisa, "inverse_of", sssspecializedsomeare);

            metaConnectionManager.AddMetaConnection(sssspecializedisa, "direct_implication", ssspecializedisa);
            metaConnectionManager.AddMetaConnection(ssspecializedisa, "direct_implication", sspecializedisa);
            metaConnectionManager.AddMetaConnection(sspecializedisa, "direct_implication", specializedisa);
            metaConnectionManager.AddMetaConnection(specializedisa, "direct_implication", isa);
            metaConnectionManager.AddMetaConnection(isa, "unlikely", contradict);

            //testing pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", isa))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "direct_implication", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            //testing pre-recursivity on unlikely

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", sssspecializedsomeare))
                throw new Exception("should be flat metaConnected because it's explicit");
        }

        private static void TestPreRecursiveUnlikelyFromImplicitCant()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

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

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(specializedisa, "inverse_of", specializedsomeare);
            metaConnectionManager.AddMetaConnection(sspecializedisa, "inverse_of", sspecializedsomeare);
            metaConnectionManager.AddMetaConnection(ssspecializedisa, "inverse_of", ssspecializedsomeare);
            metaConnectionManager.AddMetaConnection(sssspecializedisa, "inverse_of", sssspecializedsomeare);

            metaConnectionManager.AddMetaConnection(sssspecializedisa, "direct_implication", ssspecializedisa);
            metaConnectionManager.AddMetaConnection(ssspecializedisa, "direct_implication", sspecializedisa);
            metaConnectionManager.AddMetaConnection(sspecializedisa, "direct_implication", specializedisa);
            metaConnectionManager.AddMetaConnection(specializedisa, "direct_implication", isa);
            metaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            //testing pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", isa))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "direct_implication", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            //testing pre-recursivity on unlikely

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", sssspecializedsomeare))
                throw new Exception("should be flat metaConnected because it's explicit");
        }

        private static void TestPostRecursiveCant()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

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

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(specializedisa, "inverse_of", specializedsomeare);
            metaConnectionManager.AddMetaConnection(sspecializedisa, "inverse_of", sspecializedsomeare);
            metaConnectionManager.AddMetaConnection(ssspecializedisa, "inverse_of", ssspecializedsomeare);
            metaConnectionManager.AddMetaConnection(sssspecializedisa, "inverse_of", sssspecializedsomeare);
            metaConnectionManager.AddMetaConnection(sssspecializedcontradict, "permutable_side", sssspecializedcontradict);
            metaConnectionManager.AddMetaConnection(ssspecializedcontradict, "permutable_side", ssspecializedcontradict);
            metaConnectionManager.AddMetaConnection(sspecializedcontradict, "permutable_side", sspecializedcontradict);
            metaConnectionManager.AddMetaConnection(specializedcontradict, "permutable_side", specializedcontradict);
            metaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);

            metaConnectionManager.AddMetaConnection(sssspecializedcontradict, "direct_implication", ssspecializedcontradict);
            metaConnectionManager.AddMetaConnection(ssspecializedcontradict, "direct_implication", sspecializedcontradict);
            metaConnectionManager.AddMetaConnection(sspecializedcontradict, "direct_implication", specializedcontradict);
            metaConnectionManager.AddMetaConnection(specializedcontradict, "direct_implication", contradict);

            metaConnectionManager.AddMetaConnection(sssspecializedisa, "direct_implication", ssspecializedisa);
            metaConnectionManager.AddMetaConnection(ssspecializedisa, "direct_implication", sspecializedisa);
            metaConnectionManager.AddMetaConnection(sspecializedisa, "direct_implication", specializedisa);
            metaConnectionManager.AddMetaConnection(specializedisa, "direct_implication", isa);
            metaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            //testing pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "cant", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "cant", isa))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "cant", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "cant", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "direct_implication", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            //testing pre-recursivity on cant

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "cant", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "cant", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "cant", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "cant", sssspecializedsomeare))
                throw new Exception("should be flat metaConnected because it's explicit");

            //testing post-recursivity on cant

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "cant", sssspecializedcontradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "cant", sssspecializedcontradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedcontradict, "cant", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedcontradict, "cant", sssspecializedsomeare))
                throw new Exception("should be flat metaConnected because it's explicit");
        }

        private static void TestPostRecursiveUnlikely()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

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

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(specializedisa, "inverse_of", specializedsomeare);
            metaConnectionManager.AddMetaConnection(sspecializedisa, "inverse_of", sspecializedsomeare);
            metaConnectionManager.AddMetaConnection(ssspecializedisa, "inverse_of", ssspecializedsomeare);
            metaConnectionManager.AddMetaConnection(sssspecializedisa, "inverse_of", sssspecializedsomeare);
            metaConnectionManager.AddMetaConnection(sssspecializedcontradict, "permutable_side", sssspecializedcontradict);
            metaConnectionManager.AddMetaConnection(ssspecializedcontradict, "permutable_side", ssspecializedcontradict);
            metaConnectionManager.AddMetaConnection(sspecializedcontradict, "permutable_side", sspecializedcontradict);
            metaConnectionManager.AddMetaConnection(specializedcontradict, "permutable_side", specializedcontradict);
            metaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);

            metaConnectionManager.AddMetaConnection(sssspecializedcontradict, "direct_implication", ssspecializedcontradict);
            metaConnectionManager.AddMetaConnection(ssspecializedcontradict, "direct_implication", sspecializedcontradict);
            metaConnectionManager.AddMetaConnection(sspecializedcontradict, "direct_implication", specializedcontradict);
            metaConnectionManager.AddMetaConnection(specializedcontradict, "direct_implication", contradict);

            metaConnectionManager.AddMetaConnection(sssspecializedisa, "direct_implication", ssspecializedisa);
            metaConnectionManager.AddMetaConnection(ssspecializedisa, "direct_implication", sspecializedisa);
            metaConnectionManager.AddMetaConnection(sspecializedisa, "direct_implication", specializedisa);
            metaConnectionManager.AddMetaConnection(specializedisa, "direct_implication", isa);
            metaConnectionManager.AddMetaConnection(isa, "unlikely", contradict);

            //testing pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", isa))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "direct_implication", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            //testing pre-recursivity on unlikely

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", sssspecializedsomeare))
                throw new Exception("should be flat metaConnected because it's explicit");

            //testing post-recursivity on unlikely

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "unlikely", sssspecializedcontradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "unlikely", sssspecializedcontradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedcontradict, "unlikely", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedcontradict, "unlikely", sssspecializedsomeare))
                throw new Exception("should be flat metaConnected because it's explicit");
        }

        private static void TestPostRecursiveUnlikelyFromImplicitCant()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

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

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(specializedisa, "inverse_of", specializedsomeare);
            metaConnectionManager.AddMetaConnection(sspecializedisa, "inverse_of", sspecializedsomeare);
            metaConnectionManager.AddMetaConnection(ssspecializedisa, "inverse_of", ssspecializedsomeare);
            metaConnectionManager.AddMetaConnection(sssspecializedisa, "inverse_of", sssspecializedsomeare);
            metaConnectionManager.AddMetaConnection(sssspecializedcontradict, "permutable_side", sssspecializedcontradict);
            metaConnectionManager.AddMetaConnection(ssspecializedcontradict, "permutable_side", ssspecializedcontradict);
            metaConnectionManager.AddMetaConnection(sspecializedcontradict, "permutable_side", sspecializedcontradict);
            metaConnectionManager.AddMetaConnection(specializedcontradict, "permutable_side", specializedcontradict);
            metaConnectionManager.AddMetaConnection(contradict, "permutable_side", contradict);

            metaConnectionManager.AddMetaConnection(sssspecializedcontradict, "direct_implication", ssspecializedcontradict);
            metaConnectionManager.AddMetaConnection(ssspecializedcontradict, "direct_implication", sspecializedcontradict);
            metaConnectionManager.AddMetaConnection(sspecializedcontradict, "direct_implication", specializedcontradict);
            metaConnectionManager.AddMetaConnection(specializedcontradict, "direct_implication", contradict);

            metaConnectionManager.AddMetaConnection(sssspecializedisa, "direct_implication", ssspecializedisa);
            metaConnectionManager.AddMetaConnection(ssspecializedisa, "direct_implication", sspecializedisa);
            metaConnectionManager.AddMetaConnection(sspecializedisa, "direct_implication", specializedisa);
            metaConnectionManager.AddMetaConnection(specializedisa, "direct_implication", isa);
            metaConnectionManager.AddMetaConnection(isa, "cant", contradict);

            //testing pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", isa))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "direct_implication", someare))
                throw new Exception("should be flat metaConnected because it's implicit");

            //testing pre-recursivity on unlikely

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "unlikely", contradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(contradict, "unlikely", sssspecializedsomeare))
                throw new Exception("should be flat metaConnected because it's explicit");

            //testing post-recursivity on unlikely

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedisa, "unlikely", sssspecializedcontradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedsomeare, "unlikely", sssspecializedcontradict))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedcontradict, "unlikely", sssspecializedisa))
                throw new Exception("should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(sssspecializedcontradict, "unlikely", sssspecializedsomeare))
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
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");

            //Connections that shouldn't be present yet

            if (metaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "sublar", someare))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "consics", isa))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "sublar", isa))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "consics", someare))
                throw new Exception("Should be metaConnected yet");

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(isa, "muct", isa);

            //Connections that should be present

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "sublar", someare))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "consics", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            //Connections that shouldn't be present
            /*if (metaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected because it's illogical");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected because it's illogical");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "sublar", isa))
                throw new Exception("Should be metaConnected because it's illogical");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "consics", someare))
                throw new Exception("Should be metaConnected because it's illogical");*/
        }

        private static void TestRecursiveSelfSublar()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");

            //Connections that shouldn't be present yet

            if (metaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "sublar", someare))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "consics", isa))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "sublar", isa))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "consics", someare))
                throw new Exception("Should be metaConnected yet");

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(isa, "sublar", someare);

            //Connections that should be present

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "sublar", someare))
                throw new Exception("Should be metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "consics", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            //Connections that shouldn't be present
            /*if (metaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected because it's illogical");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected because it's illogical");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "sublar", isa))
                throw new Exception("Should be metaConnected because it's illogical");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "consics", someare))
                throw new Exception("Should be metaConnected because it's illogical");*/
        }

        private static void TestRecursiveSelfLiffid()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");

            //Connections that shouldn't be present yet

            if (metaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "sublar", someare))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "consics", isa))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "sublar", isa))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "consics", someare))
                throw new Exception("Should be metaConnected yet");

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(someare, "muct", someare);

            //Connections that should be present

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "consics", someare))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "sublar", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            //Connections that shouldn't be present
            /*if (metaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected because it's illogical");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected because it's illogical");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "consics", isa))
                throw new Exception("Should be metaConnected because it's illogical");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "sublar", someare))
                throw new Exception("Should be metaConnected because it's illogical");*/
        }

        private static void TestRecursiveSelfConsics()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");

            //Connections that shouldn't be present yet

            if (metaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "sublar", someare))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "consics", isa))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "sublar", isa))
                throw new Exception("Should be metaConnected yet");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "consics", someare))
                throw new Exception("Should be metaConnected yet");

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(isa, "consics", someare);

            //Connections that should be present

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "consics", someare))
                throw new Exception("Should be metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "sublar", isa))
                throw new Exception("Should be metaConnected because it's implicit");

            //Connections that shouldn't be present
            /*if (metaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected because it's illogical");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected because it's illogical");

            if (metaConnectionManager.IsFlatMetaConnected(someare, "consics", isa))
                throw new Exception("Should be metaConnected because it's illogical");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "sublar", someare))
                throw new Exception("Should be metaConnected because it's illogical");*/
        }

        private static void TestIsaMuctIsaFromMadeofMuctIsa()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(madeof,"muct",isa);

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected because it's implicit and very logical");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected because it's implicit and very logical and implicit from preceding expression");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected because self muct is equivalent to self liffid");

            if (!metaConnectionManager.IsFlatMetaConnected(isa,"liffid",isa))
                throw new Exception("Should be metaConnected because self muct is equivalent to self liffid");
        }

        private static void TestIsaMuctIsaFromMadeofLiffidIsa()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(madeof, "liffid", isa);

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "liffid", isa))
                throw new Exception("Should be metaConnected because it's implicit and very logical");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "muct", someare))
                throw new Exception("Should be metaConnected because it's implicit and very logical and implicit from preceding expression");

            if (!metaConnectionManager.IsFlatMetaConnected(someare, "liffid", someare))
                throw new Exception("Should be metaConnected because self muct is equivalent to self liffid");

            if (!metaConnectionManager.IsFlatMetaConnected(isa, "muct", isa))
                throw new Exception("Should be metaConnected because self muct is equivalent to self liffid");
        }

        private static void TestMadeofMustNotMuctNorLiffidNeed()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept need = new Concept("need");
            Concept allow = new Concept("allow");

            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(need, "inverse_of", allow);
            metaConnectionManager.AddMetaConnection(madeof, "muct", madeof);
            metaConnectionManager.AddMetaConnection(madeof, "direct_implication", need);

            //Real test here

            if (metaConnectionManager.IsFlatMetaConnected(madeof, "muct", need))
                throw new Exception("Shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(madeof, "liffid", need))
                throw new Exception("Shouldn't be flat metaConnected");
        }

        private static void TestMadeofMustNotMuctNorLiffidNeedHarderTestWithInverseOf()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept need = new Concept("need");
            Concept allow = new Concept("allow");

            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(need, "inverse_of", allow);
            metaConnectionManager.AddMetaConnection(madeof, "muct", madeof);
            metaConnectionManager.AddMetaConnection(madeof, "direct_implication", need);

            //Pre-conditions

            if (metaConnectionManager.IsFlatMetaConnected(madeof, "direct_implication", allow))
                throw new Exception("Shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(madeof, "inverse_implication", need))
                throw new Exception("Shouldn't be flat metaConnected");

            //Real test here

            if (metaConnectionManager.IsFlatMetaConnected(madeof, "sublar", allow))
                throw new Exception("Shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(madeof, "muct", need))
                throw new Exception("Shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(madeof, "liffid", need))
                throw new Exception("Shouldn't be flat metaConnected");
        }

        private static void TestMakeMustNotLiffidNeed()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");
            Concept madeof = new Concept("madeof");
            Concept partof = new Concept("partof");
            Concept need = new Concept("need");
            Concept allow = new Concept("allow");
            Concept make = new Concept("make");
            Concept madeby = new Concept("madeby");

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);
            metaConnectionManager.AddMetaConnection(madeof, "inverse_of", partof);
            metaConnectionManager.AddMetaConnection(need, "inverse_of", allow);
            metaConnectionManager.AddMetaConnection(make, "inverse_of", madeby);
            metaConnectionManager.AddMetaConnection(madeof, "muct", isa);
            metaConnectionManager.AddMetaConnection(madeof, "liffid", isa);
            metaConnectionManager.AddMetaConnection(madeof, "muct", madeof);
            metaConnectionManager.AddMetaConnection(make, "liffid", isa);
            metaConnectionManager.AddMetaConnection(need, "liffid", need);
            metaConnectionManager.AddMetaConnection(madeof, "direct_implication", need);

            metaConnectionManager.AddMetaConnection(isa, "direct_implication", need);

            //Pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(need, "liffid", need))
                throw new Exception("Should be flat metaConnected");

            if (!metaConnectionManager.IsFlatMetaConnected(need, "muct", need))
                throw new Exception("Should be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(isa, "liffid", need))
                throw new Exception("Shouldn't be flat metaConnected");

            //Real test here           

            if (metaConnectionManager.IsFlatMetaConnected(make, "liffid", need))
                throw new Exception("Shouldn't be flat metaConnected");
        }

        private static void TestCantFromImplicitInverseOf()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

            Concept isa = new Concept("isa");
            Concept someare = new Concept("someare");

            metaConnectionManager.AddMetaConnection(isa, "inverse_of", someare);

            //Real test here

            if (!metaConnectionManager.IsFlatMetaConnected(isa,"cant",someare))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(someare,"cant",isa))
                throw new Exception("Should be flat metaConnected because it's implicit");
        }

        private static void TestNoSelfCant()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

            Concept make = new Concept("make");
            Concept madeby = new Concept("madeby");
            Concept create = new Concept("create");
            Concept createdby = new Concept("createdby");

            metaConnectionManager.AddMetaConnection(make, "inverse_of", madeby);
            metaConnectionManager.AddMetaConnection(make, "cant", madeby);
            metaConnectionManager.AddMetaConnection(create, "inverse_of", createdby);
            metaConnectionManager.AddMetaConnection(create, "direct_implication", make);

            //Pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(createdby, "direct_implication", madeby))
                throw new Exception("Should be flat metaConnected");

            //Real test here

            if (metaConnectionManager.IsFlatMetaConnected(make, "cant", make))
                throw new Exception("Shouldn't be flat metaConnected");


            if (metaConnectionManager.IsFlatMetaConnected(madeby, "cant", madeby))
                throw new Exception("Shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(make, "cant", create))
                throw new Exception("Shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(create, "cant", make))
                throw new Exception("Shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(make, "unlikely", make))
                throw new Exception("Shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(make, "unlikely", create))
                throw new Exception("Shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(createdby, "cant", madeby))
                throw new Exception("Shouldn't be flat metaConnected");

            if (metaConnectionManager.IsFlatMetaConnected(createdby, "unlikely", madeby))
                throw new Exception("Shouldn't be flat metaConnected");
        }

        private static void TestOrbitedByDirectImplicationGravity()
        {
            Memory.TotalVerbList = new HashSet<Concept>();
            MetaConnectionManager metaConnectionManager = new MetaConnectionManager();

            Concept orbit = new Concept("orbit");
            Concept orbitedby = new Concept("orbited by");
            Concept gravity = new Concept("gravity");

            metaConnectionManager.AddMetaConnection(orbit, "inverse_of", orbitedby);
            metaConnectionManager.AddMetaConnection(gravity, "permutable_side", gravity);
            metaConnectionManager.AddMetaConnection(orbit, "direct_implication", gravity);

            //Pre-conditions

            if (!metaConnectionManager.IsFlatMetaConnected(orbit, "direct_implication", gravity))
                throw new Exception("Should be flat metaConnected because it's explicit");

            if (!metaConnectionManager.IsFlatMetaConnected(orbitedby, "inverse_implication", gravity))
                throw new Exception("Should be flat metaConnected because it's explicit");

            //Real test here

            if (!metaConnectionManager.IsFlatMetaConnected(orbit, "inverse_implication", gravity))
                throw new Exception("Should be flat metaConnected because it's implicit");

            if (!metaConnectionManager.IsFlatMetaConnected(orbitedby, "direct_implication", gravity))
                throw new Exception("Should be flat metaConnected because it's implicit");
        }
    }
}
