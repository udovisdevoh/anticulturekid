using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestStatementListFactory
    {
        public static void Test()
        {
            AbstractStatementFactory sentenceInterpreter = new StatementFactory();
            AbstractStatementListFactory paragraphInterpreter = new StatementListFactory();
            List<Statement> statementList;

            #region Testing single operation
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "pine isa tree");
            if (statementList.Count != 1)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "pine isa tree"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[0] == sentenceInterpreter.GetInterpretedHumanStatement("Rich", "pine someare tree"))
                throw new Exception("Shouldn't match splitted statement to statement");
            if (statementList[0] == sentenceInterpreter.GetInterpretedHumanStatement("Rich", "pine isa mofo"))
                throw new Exception("Shouldn't match splitted statement to statement");
            if (statementList[0] == sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree isa mofo"))
                throw new Exception("Shouldn't match splitted statement to statement");
            if (statementList[0] == sentenceInterpreter.GetInterpretedHumanStatement("Rich", "pine not isa tree"))
                throw new Exception("Shouldn't match splitted statement to statement");
            #endregion

            #region Testing double operation
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "pine isa tree madeof wood");
            if (statementList.Count != 2)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "pine isa tree"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "pine madeof wood"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] == sentenceInterpreter.GetInterpretedHumanStatement("Rich", "cat isa animal"))
                throw new Exception("Shouldn't match splitted statement to statement");
            #endregion

            #region Testing triple operation
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "pine isa tree madeof wood someare christmas_pine");
            if (statementList.Count != 3)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "pine isa tree"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "pine madeof wood"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "pine someare christmas_pine"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] == sentenceInterpreter.GetInterpretedHumanStatement("Rich", "cat isa animal"))
                throw new Exception("Shouldn't match splitted statement to statement");
            #endregion

            #region Testing quadruple operation
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "tree isa plant madeof wood someare pine someare palm_tree");
            if (statementList.Count != 4)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree isa plant"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree madeof wood"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree someare pine"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[3] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree someare palm_tree"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region Testing quadruple operation with and
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "tree isa plant madeof wood someare pine and palm_tree");
            if (statementList.Count != 4)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree isa plant"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree madeof wood"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree someare pine"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[3] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree someare palm_tree"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region tree isa plant (which isa lifeform)
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "tree isa plant (which isa lifeform)");
            if (statementList.Count != 2)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree isa plant"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "plant isa lifeform"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region tree isa plant (which isa lifeform) and long_thing
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "tree isa plant (which isa lifeform) and long_thing");
            if (statementList.Count != 3)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree isa plant"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree isa long_thing"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "plant isa lifeform"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region tree isa plant (which isa lifeform) madeof wood
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "tree isa plant (which isa lifeform) madeof wood");
            if (statementList.Count != 3)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree isa plant"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree madeof wood"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "plant isa lifeform"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region tree isa plant (which isa lifeform) and long_thing madeof wood
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "tree isa plant (which isa lifeform) and long_thing madeof wood");
            if (statementList.Count != 4)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree isa plant"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree isa long_thing"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree madeof wood"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[3] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "plant isa lifeform"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region tree isa plant (which isa lifeform (which madeof water))
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "tree isa plant (which isa lifeform (which madeof water))");
            if (statementList.Count != 3)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree isa plant"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "plant isa lifeform"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "lifeform madeof water"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region tree isa plant (which isa lifeform (which madeof water (which isa liquid madeof oxygen and hydrogen)))
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "tree isa plant (which isa lifeform (which madeof water (which isa liquid madeof oxygen and hydrogen)))");
            if (statementList.Count != 6)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree isa plant"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "plant isa lifeform"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "lifeform madeof water"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[3] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water isa liquid"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[4] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water madeof oxygen"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[5] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water madeof hydrogen"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region tree isa plant (which isa lifeform (which madeof water (which isa liquid madeof oxygen (which isa gas) and hydrogen (which isa gas))))
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "tree isa plant (which isa lifeform (which madeof water (which isa liquid madeof oxygen (which isa gas) and hydrogen (which isa gas))))");
            if (statementList.Count != 8)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree isa plant"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "plant isa lifeform"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "lifeform madeof water"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[3] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water isa liquid"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[4] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water madeof oxygen"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[5] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water madeof hydrogen"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[6] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "oxygen isa gas"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[7] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "hydrogen isa gas"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region tree isa plant (which isa lifeform (which madeof water (which isa liquid madeof h_minus (which isa negative_ion) and oh_plus (which isa positive_ion))))
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "tree isa plant (which isa lifeform (which madeof water (which isa liquid madeof h_minus (which isa negative_ion) and oh_plus (which isa positive_ion))))");
            if (statementList.Count != 8)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree isa plant"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "plant isa lifeform"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "lifeform madeof water"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[3] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water isa liquid"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[4] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water madeof h_minus"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[5] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water madeof oh_plus"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[6] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "h_minus isa negative_ion"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[7] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "oh_plus isa positive_ion"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region tree isa plant (which isa lifeform (which madeof water (which isa liquid madeof h_minus (which isa negative_ion (which isa ion)) and oh_plus (which isa positive_ion (which isa ion)))))
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "tree isa plant (which isa lifeform (which madeof water (which isa liquid madeof h_minus (which isa negative_ion (which isa ion)) and oh_plus (which isa positive_ion (which isa ion)))))");
            if (statementList.Count != 10)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree isa plant"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "plant isa lifeform"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "lifeform madeof water"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[3] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water isa liquid"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[4] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water madeof h_minus"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[5] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water madeof oh_plus"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[6] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "h_minus isa negative_ion"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[7] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "negative_ion isa ion"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[8] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "oh_plus isa positive_ion"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[9] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "positive_ion isa ion"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region tree_(plant) isa plant (which isa lifeform (which madeof water_(liquid) (which isa liquid madeof h_minus (which isa negative_ion_(ion) (which isa ion)) and oh_plus (which isa positive_ion (which isa ion)))))
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "tree_(plant) isa plant (which isa lifeform (which madeof water_(liquid) (which isa liquid madeof h_minus (which isa negative_ion_(ion) (which isa ion)) and oh_plus (which isa positive_ion (which isa ion)))))");
            if (statementList.Count != 10)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree_(plant) isa plant"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "plant isa lifeform"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "lifeform madeof water_(liquid)"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[3] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water_(liquid) isa liquid"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[4] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water_(liquid) madeof h_minus"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[5] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water_(liquid) madeof oh_plus"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[6] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "h_minus isa negative_ion_(ion)"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[7] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "negative_ion_(ion) isa ion"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[8] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "oh_plus isa positive_ion"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[9] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "positive_ion isa ion"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region water isa liquid madeof oxygen and hydrogen isa beverage (which isa liquid)
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "water isa liquid madeof oxygen and hydrogen isa beverage (which isa liquid)");
            if (statementList.Count != 5)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water isa liquid"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water madeof oxygen"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water madeof hydrogen"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[3] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "water isa beverage"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[4] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "beverage isa liquid"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region petroleum isa liquid not madeof sugar
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "petroleum isa liquid not madeof sugar");
            if (statementList.Count != 2)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum isa liquid"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum not madeof sugar"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region petroleum isa liquid not isa beverage not madeof sugar
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "petroleum isa liquid not isa beverage not madeof sugar");
            if (statementList.Count != 3)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum isa liquid"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum not isa beverage"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum not madeof sugar"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region petroleum isa liquid not isa beverage not madeof sugar and salt
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "petroleum isa liquid not isa beverage not madeof sugar and salt");
            if (statementList.Count != 4)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum isa liquid"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum not isa beverage"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum not madeof sugar"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[3] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum not madeof salt"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region petroleum isa liquid not isa beverage not madeof sugar nor salt
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "petroleum isa liquid not isa beverage not madeof sugar nor salt");
            if (statementList.Count != 4)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum isa liquid"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum not isa beverage"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum not madeof sugar"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[3] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum not madeof salt"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region oil someare petroleum (which isa liquid not isa beverage not madeof sugar nor salt)
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "oil someare petroleum (which isa liquid not isa beverage not madeof sugar nor salt)");
            if (statementList.Count != 5)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "oil someare petroleum"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum isa liquid"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum not isa beverage"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[3] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum not madeof sugar"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[4] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum not madeof salt"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region food not someare petroleum (which isa liquid not isa beverage not madeof sugar nor salt madeof energy)
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "food not someare petroleum (which isa liquid not isa beverage not madeof sugar nor salt madeof energy)");
            if (statementList.Count != 6)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "food not someare petroleum"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum isa liquid"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum not isa beverage"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[3] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum not madeof sugar"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[4] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum not madeof salt"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[5] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "petroleum madeof energy"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region pine isa tree and madeof wood
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "pine isa tree and madeof wood");
            if (statementList.Count != 2)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "pine isa tree"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "pine madeof wood"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region pine isa tree and not madeof leaf
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "pine isa tree and not madeof leaf");
            if (statementList.Count != 2)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "pine isa tree"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "pine not madeof leaf"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region pine isa tree (which isa plant and madeof wood) and not madeof leaf
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "pine isa tree (which isa plant and madeof wood) and not madeof leaf");
            if (statementList.Count != 4)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "pine isa tree"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "pine not madeof leaf"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree isa plant"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[3] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "tree madeof wood"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region pine not isa tree and not isa plant
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "pine not isa tree and not isa plant (which contradict animal and isa lifeform not madeof blood)");
            if (statementList.Count != 5)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "pine not isa tree"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "pine not isa plant"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "plant contradict animal"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[3] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "plant isa lifeform"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[4] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "plant not madeof blood"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region country someare egypt, france, italy and cambodia (which partof asia (which madeof china (which isa country), japan (which isa country) and vietnam (which isa country)))
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "country someare egypt, france, italy and cambodia (which partof asia (which madeof china (which isa country), japan (which isa country) and vietnam (which isa country)))");
            if (statementList.Count != 11)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "country someare egypt"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "country someare france"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "country someare italy"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[3] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "country someare cambodia"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[4] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "cambodia partof asia"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[5] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "asia madeof china"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[6] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "asia madeof japan"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[7] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "asia madeof vietnam"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[8] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "china isa country"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[9] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "japan isa country"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[10] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "vietnam isa country"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region red, blue and green isa color partof chromatic_circle (which isa circle) and light
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "red, blue and green isa color partof chromatic_circle (which isa circle) and light");
            if (statementList.Count != 10)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "green isa color"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "green partof chromatic_circle"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "green partof light"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[3] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "chromatic_circle isa circle"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[4] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "red isa color"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[5] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "red partof chromatic_circle"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[6] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "red partof light"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[7] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "blue isa color"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[8] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "blue partof chromatic_circle"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[9] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "blue partof light"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region cold and hot isa temperature
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "cold and hot isa temperature");
            if (statementList.Count != 2)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "hot isa temperature"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "cold isa temperature"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion

            #region cold, warm and hot isa temperature
            statementList = paragraphInterpreter.GetInterpretedHumanStatementList("Rich", "cold, warm and hot isa temperature");
            if (statementList.Count != 3)
                throw new Exception("Statement list count doesn't match");
            if (statementList[0] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "hot isa temperature"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[1] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "cold isa temperature"))
                throw new Exception("Couldn't match splitted statement to statement");
            if (statementList[2] != sentenceInterpreter.GetInterpretedHumanStatement("Rich", "warm isa temperature"))
                throw new Exception("Couldn't match splitted statement to statement");
            #endregion
        }
    }
}