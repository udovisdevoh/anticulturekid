using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestStatementFactory
    {
        public static void Test()
        {
            AbstractStatementFactory interpreter = new StatementFactory();
            Statement statement;
            Statement statement2;

            #region Testing nullary operations
            statement = interpreter.GetInterpretedHumanStatement("Louis", "yes");
            if (statement.AuthorName != "Louis")
                throw new Exception("Author name is wrong");
            if (statement.NullaryOrUnaryOperatorName != "yes")
                throw new Exception("nullary operator name is wrong");
            if (statement.GetConceptName(0) != null)
                throw new Exception("concept name is wrong, it should be null");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.IsInterrogative != false)
                throw new Exception("Interrogativity is wrong");
            #endregion

            #region Testing unary operations
            statement = interpreter.GetInterpretedHumanStatement("JohnDoe", "whatis mofo");
            if (statement.AuthorName != "JohnDoe")
                throw new Exception("Author name is wrong");
            if (statement.NullaryOrUnaryOperatorName != "whatis")
                throw new Exception("unary operator name is wrong");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.IsInterrogative != false)
                throw new Exception("Interrogativity is wrong");
            if (statement.GetConceptName(0) != "mofo")
                throw new Exception("Concept name is wrong");
            statement = interpreter.GetInterpretedHumanStatement("JohnDoe", "define mofo?");
            if (statement.GetConceptName(0) != "mofo")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.NullaryOrUnaryOperatorName != "define")
                throw new Exception("unary operator name " + statement.NullaryOrUnaryOperatorName + " is wrong");
            #endregion

            #region Testing binary operators
            statement = interpreter.GetInterpretedHumanStatement("Boris", "apple isa fruit");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.IsInterrogative != false)
                throw new Exception("Interrogativity is wrong");
            if (statement.GetConceptName(0) != "apple")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "isa")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.GetConceptName(2) != "fruit")
                throw new Exception("Concept name " + statement.GetConceptName(2) + " is wrong");
            if (statement.GetConceptName(3) != null)
                throw new Exception("Concept name " + statement.GetConceptName(3) + " is wrong");
            if (statement.NullaryOrUnaryOperatorName != null)
                throw new Exception("unary operator name is wrong");
            if (statement.IsAskingWhy != false)
                throw new Exception("IsAskingWhy is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Boris", "apple not isa fruit");
            if (statement.IsNegative != true)
                throw new Exception("Negativity is wrong");
            if (statement.IsInterrogative != false)
                throw new Exception("Interrogativity is wrong");
            if (statement.GetConceptName(0) != "apple")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "isa")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.GetConceptName(2) != "fruit")
                throw new Exception("Concept name " + statement.GetConceptName(2) + " is wrong");
            if (statement.GetConceptName(3) != null)
                throw new Exception("Concept name " + statement.GetConceptName(3) + " is wrong");
            if (statement.NullaryOrUnaryOperatorName != null)
                throw new Exception("unary operator name is wrong");
            if (statement.IsAskingWhy != false)
                throw new Exception("IsAskingWhy is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Boris", "apple isa fruit?");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.IsInterrogative != true)
                throw new Exception("Interrogativity is wrong");
            if (statement.GetConceptName(0) != "apple")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "isa")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.GetConceptName(2) != "fruit")
                throw new Exception("Concept name " + statement.GetConceptName(2) + " is wrong");
            if (statement.GetConceptName(3) != null)
                throw new Exception("Concept name " + statement.GetConceptName(3) + " is wrong");
            if (statement.NullaryOrUnaryOperatorName != null)
                throw new Exception("unary operator name is wrong");
            if (statement.IsAskingWhy != false)
                throw new Exception("IsAskingWhy is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Boris", "does apple isa fruit?");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.IsInterrogative != true)
                throw new Exception("Interrogativity is wrong");
            if (statement.GetConceptName(0) != "apple")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "isa")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.GetConceptName(2) != "fruit")
                throw new Exception("Concept name " + statement.GetConceptName(2) + " is wrong");
            if (statement.GetConceptName(3) != null)
                throw new Exception("Concept name " + statement.GetConceptName(3) + " is wrong");
            if (statement.NullaryOrUnaryOperatorName != null)
                throw new Exception("unary operator name is wrong");
            if (statement.IsAskingWhy != false)
                throw new Exception("IsAskingWhy is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Boris", "does apple isa fruit");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.IsInterrogative != true)
                throw new Exception("Interrogativity is wrong");
            if (statement.GetConceptName(0) != "apple")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "isa")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.GetConceptName(2) != "fruit")
                throw new Exception("Concept name " + statement.GetConceptName(2) + " is wrong");
            if (statement.GetConceptName(3) != null)
                throw new Exception("Concept name " + statement.GetConceptName(3) + " is wrong");
            if (statement.NullaryOrUnaryOperatorName != null)
                throw new Exception("unary operator name is wrong");
            if (statement.IsAskingWhy != false)
                throw new Exception("IsAskingWhy is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Mike", "why pine isa tree?");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.IsInterrogative != true)
                throw new Exception("Interrogativity is wrong");
            if (statement.IsAskingWhy != true)
                throw new Exception("IsAskingWhy is wrong");
            if (statement.GetConceptName(0) != "pine")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "isa")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.GetConceptName(2) != "tree")
                throw new Exception("Concept name " + statement.GetConceptName(2) + " is wrong");
            if (statement.GetConceptName(3) != null)
                throw new Exception("Concept name " + statement.GetConceptName(3) + " is wrong");
            if (statement.NullaryOrUnaryOperatorName != null)
                throw new Exception("unary operator name is wrong");
            if (statement.AuthorName != "Mike")
                throw new Exception("Author name is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Mike", "how pine isa tree");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.IsInterrogative != true)
                throw new Exception("Interrogativity is wrong");
            if (statement.IsAskingWhy != true)
                throw new Exception("IsAskingWhy is wrong");
            #endregion

            #region Testing naming association
            #region Testing aliasof and not aliasof
            statement = interpreter.GetInterpretedHumanStatement("Mike", "circle aliasof round");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.IsInterrogative == true)
                throw new Exception("Interrogativity is wrong");
            if (statement.GetConceptName(0) != "circle")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "round")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.NamingAssociationOperatorName != "aliasof")
                throw new Exception("naming association operator name is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Mike", "   aliasof  circle  round ");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.IsInterrogative == true)
                throw new Exception("Interrogativity is wrong");
            if (statement.GetConceptName(0) != "circle")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "round")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.NamingAssociationOperatorName != "aliasof")
                throw new Exception("naming association operator name is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Mike", "circle not aliasof round");
            if (statement.IsNegative != true)
                throw new Exception("Negativity is wrong");
            if (statement.IsInterrogative == true)
                throw new Exception("Interrogativity is wrong");
            if (statement.GetConceptName(0) != "circle")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "round")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.NamingAssociationOperatorName != "aliasof")
                throw new Exception("naming association operator name is wrong");
            if (statement.AuthorName != "Mike")
                throw new Exception("Author name is wrong");
            #endregion

            #region Testing rename
            statement = interpreter.GetInterpretedHumanStatement("Mike", "rename round circle");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.IsInterrogative == true)
                throw new Exception("Interrogativity is wrong");
            if (statement.GetConceptName(0) != "round")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "circle")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.NamingAssociationOperatorName != "rename")
                throw new Exception("naming association operator name is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Mike", " round      rename   circle    ");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.IsInterrogative == true)
                throw new Exception("Interrogativity is wrong");
            if (statement.GetConceptName(0) != "round")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "circle")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.NamingAssociationOperatorName != "rename")
                throw new Exception("naming association operator name is wrong");
            #endregion
            #endregion

            #region Testing metaOperations
            #region Testing equivalence expressions
            #region permutableSide
            statement = interpreter.GetInterpretedHumanStatement("Steve", "love contradict hate = hate contradict love");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "contradict")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "contradict")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "permutable_side")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Steve", "not love contradict hate = hate contradict love");
            if (statement.IsNegative != true)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "contradict")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "contradict")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "permutable_side")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Steve", "love not contradict hate = hate contradict love");
            if (statement.IsNegative != true)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "contradict")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "contradict")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "permutable_side")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");
            #endregion

            #region inverseOf
            statement = interpreter.GetInterpretedHumanStatement("George", "small smallerthan big = big biggerthan small");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "smallerthan")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "biggerthan")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "inverse_of")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");


            statement = interpreter.GetInterpretedHumanStatement("George", "small smallerthan big = big not biggerthan small");
            if (statement.IsNegative != true)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "smallerthan")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "biggerthan")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "inverse_of")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");



            statement = interpreter.GetInterpretedHumanStatement("George", "small? smallerthan? big? = big? biggerthan? small?");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "smallerthan")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "biggerthan")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "inverse_of")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");
            #endregion
            #endregion

            #region Testing implication expressions
            #region Testing directImplication expression
            statement = interpreter.GetInterpretedHumanStatement("Steve", "if me make you then me love you");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "make")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "love")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "direct_implication")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Steve", "not if me make you then me love you");
            if (statement.IsNegative != true)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "make")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "love")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "direct_implication")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");
            #endregion

            #region Testing inverseImplication expression
            statement = interpreter.GetInterpretedHumanStatement("Steve", "if me make you then you love me");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "make")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "love")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "inverse_implication")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Steve", "not if me make you then you love me");
            if (statement.IsNegative != true)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "make")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "love")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "inverse_implication")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");
            #endregion
            #endregion

            #region Testing cant expressions
            #region Testing directCant expression
            statement = interpreter.GetInterpretedHumanStatement("Steve", "if me make you then me cant hate you");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "make")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "hate")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "cant")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Steve", "not if me make you then me cant hate you");
            if (statement.IsNegative != true)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "make")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "hate")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "cant")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");
            #endregion

            #region Testing inverseCant expression
            statement = interpreter.GetInterpretedHumanStatement("Steve", "if me make you then you cant hate me");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "make")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "hate")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "cant")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Steve", "not if me make you then you cant hate me");
            if (statement.IsNegative != true)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "make")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "hate")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "cant")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");
            #endregion
            #endregion

            #region Testing unlikely expressions
            #region Testing unlikely expression
            statement = interpreter.GetInterpretedHumanStatement("Steve", "if me make you then me unlikely hate you");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "make")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "hate")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "unlikely")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Steve", "not if me make you then me unlikely hate you");
            if (statement.IsNegative != true)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "make")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "hate")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "unlikely")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");
            #endregion

            #region Testing unlikely expression
            statement = interpreter.GetInterpretedHumanStatement("Steve", "if me make you then you unlikely hate me");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "make")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "hate")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "unlikely")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Steve", "not if me make you then you unlikely hate me");
            if (statement.IsNegative != true)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "make")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "hate")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "unlikely")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");
            #endregion
            #endregion

            #region Texting explicit metaoprations
            statement = interpreter.GetInterpretedHumanStatement("Steve", "madeof muct isa");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");
            if (statement.GetConceptName(0) != "madeof")
                throw new Exception("Concept name " + statement.GetConceptName(0) + " is wrong");
            if (statement.GetConceptName(1) != "isa")
                throw new Exception("Concept name " + statement.GetConceptName(1) + " is wrong");
            if (statement.MetaOperatorName != "muct")
                throw new Exception("MetaOperator name " + statement.MetaOperatorName + " is wrong");
            #endregion

            #region Testing ternary metaOperations
            #region Testing whether equivalent expressions are recognized as equivalent or not
            statement = interpreter.GetInterpretedHumanStatement("Luke", "if pine isa tree and tree madeof wood then pine madeof wood");
            statement2 = interpreter.GetInterpretedHumanStatement("Luke", "if tree madeof wood and pine isa tree then pine madeof wood");
            if (statement != statement2)
                throw new Exception("Equivalent expression not parsed as equivalent");

            statement = interpreter.GetInterpretedHumanStatement("Luke", "if wood isa material and pine madeof wood then pine madeof material");
            statement2 = interpreter.GetInterpretedHumanStatement("Luke", "if pine madeof wood and wood isa material then pine madeof material");
            if (statement != statement2)
                throw new Exception("Equivalent expression not parsed as equivalent");

            statement = interpreter.GetInterpretedHumanStatement("Luke", "if tree someare pine and tree madeof wood then pine madeof wood");
            statement2 = interpreter.GetInterpretedHumanStatement("Luke", "if tree madeof wood and tree someare pine then pine madeof wood");
            if (statement != statement2)
                throw new Exception("Equivalent expression not parsed as equivalent");

            statement = interpreter.GetInterpretedHumanStatement("Luke", "if pine isa tree and wood partof tree then wood partof pine");
            statement2 = interpreter.GetInterpretedHumanStatement("Luke", "if wood partof tree and pine isa tree then wood partof pine");
            if (statement != statement2)
                throw new Exception("Equivalent expression not parsed as equivalent");
            #endregion

            #region Testing for metaOperator and concept names
            statement = interpreter.GetInterpretedHumanStatement("Luke", "if pine isa tree and tree madeof wood then pine madeof wood");
            if (statement.MetaOperatorName != "muct")
                throw new Exception(statement.MetaOperatorName + " metaOperator name is wrong");
            if (statement.GetConceptName(0) != "madeof")
                throw new Exception(statement.GetConceptName(0) + " concept name is wrong");
            if (statement.GetConceptName(1) != "isa")
                throw new Exception(statement.GetConceptName(1) + " concept name is wrong");
            if (statement.GetConceptName(2) != null)
                throw new Exception(statement.GetConceptName(2) + " concept name is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Luke", "if wood isa material and pine madeof wood then pine madeof material");
            if (statement.MetaOperatorName != "liffid")
                throw new Exception(statement.MetaOperatorName + " metaOperator name is wrong");
            if (statement.GetConceptName(0) != "madeof")
                throw new Exception(statement.GetConceptName(0) + " concept name is wrong");
            if (statement.GetConceptName(1) != "isa")
                throw new Exception(statement.GetConceptName(1) + " concept name is wrong");
            if (statement.GetConceptName(2) != null)
                throw new Exception(statement.GetConceptName(2) + " concept name is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Luke", "if tree someare pine and tree madeof wood then pine madeof wood");
            if (statement.MetaOperatorName != "sublar")
                throw new Exception(statement.MetaOperatorName + " metaOperator name is wrong");
            if (statement.GetConceptName(0) != "madeof")
                throw new Exception(statement.GetConceptName(0) + " concept name is wrong");
            if (statement.GetConceptName(1) != "someare")
                throw new Exception(statement.GetConceptName(1) + " concept name is wrong");
            if (statement.GetConceptName(2) != null)
                throw new Exception(statement.GetConceptName(2) + " concept name is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Luke", "if pine isa tree and wood partof tree then wood partof pine");
            if (statement.MetaOperatorName != "consics")
                throw new Exception(statement.MetaOperatorName + " metaOperator name is wrong");
            if (statement.GetConceptName(0) != "partof")
                throw new Exception(statement.GetConceptName(0) + " concept name is wrong");
            if (statement.GetConceptName(1) != "isa")
                throw new Exception(statement.GetConceptName(1) + " concept name is wrong");
            if (statement.GetConceptName(2) != null)
                throw new Exception(statement.GetConceptName(2) + " concept name is wrong");
            #endregion

            #region Testing for negativity
            statement = interpreter.GetInterpretedHumanStatement("Luke", "if pine isa tree and wood partof tree then wood partof pine");
            if (statement.IsNegative != false)
                throw new Exception("Negativity is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Luke", "not if pine isa tree and wood partof tree then wood partof pine");
            if (statement.IsNegative != true)
                throw new Exception("Negativity is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Luke", "not if pine isa tree and wood partof tree then wood partof pine");
            if (statement.IsNegative != true)
                throw new Exception("Negativity is wrong");

            statement = interpreter.GetInterpretedHumanStatement("Luke", "not if pine isa tree and wood partof tree then wood partof pine not");
            if (statement.IsNegative != true)
                throw new Exception("Negativity is wrong");
            #endregion
            #endregion
            #endregion
        }
    }
}
