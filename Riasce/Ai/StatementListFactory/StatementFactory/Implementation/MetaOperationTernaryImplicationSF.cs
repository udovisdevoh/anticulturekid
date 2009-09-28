using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Text;

namespace AntiCulture.Kid
{
    class MetaOperationTernaryImplicationStatementFactory : AbstractStatementFactory
    {
        #region Fields and parts
        //if pine isa tree and tree madeof wood then pine madeof wood
        private static readonly Regex muctA = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and \3 ([a-z0-9_()]+) ([a-z0-9_()]+) then \1 \4 \5");

        //if tree madeof wood and pine isa tree then pine madeof wood
        private static readonly Regex muctB = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and ([a-z0-9_()]+) ([a-z0-9_()]+) \1 then \4 \2 \3");
        
        //if wood isa material and pine madeof wood then pine madeof material
        private static readonly Regex liffidA = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and ([a-z0-9_()]+) ([a-z0-9_()]+) \1 then \4 \5 \3");
        
        //if pine madeof wood and wood isa material then pine madeof material
        private static readonly Regex liffidB = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and \3 ([a-z0-9_()]+) ([a-z0-9_()]+) then \1 \2 \5");
        
        //if tree someare pine and tree madeof wood then pine madeof wood
        private static readonly Regex sublarA = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and \1 ([a-z0-9_()]+) ([a-z0-9_()]+) then \3 \4 \5");
        
        //if tree madeof wood and tree someare pine then pine madeof wood
        private static readonly Regex sublarB = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and \1 ([a-z0-9_()]+) ([a-z0-9_()]+) then \5 \2 \3");
        
        //if pine isa tree and wood partof tree then wood partof pine
        private static readonly Regex consicsA = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and ([a-z0-9_()]+) ([a-z0-9_()]+) \3 then \4 \5 \1");

        //if wood partof tree and pine isa tree then wood partof pine
        private static readonly Regex consicsB = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and ([a-z0-9_()]+) ([a-z0-9_()]+) \3 then \1 \2 \4");

        private ImplyStatementFactory implyStatementFactory = new ImplyStatementFactory();
        #endregion

        #region Methods
        #region Getters
        public sealed override Statement GetInterpretedHumanStatement(string humanName, string humanStatement)
        {
            bool isNegative = humanStatement.StartsWith("not");
            if (isNegative)
                humanStatement = humanStatement.Substring(4);

            List<string> words = new List<string>(humanStatement.Split(' '));

            //if tree madeof wood and pine isa tree then pine madeof wood
            //Abscent subject in result (muct)
            //inverse
            if (muctA.IsMatch(humanStatement))
            {
                return new Statement(humanName, Statement.MODE_META_OPERATION, "muct", words[6], words[2], isNegative, humanStatement);
            }
            else if (muctB.IsMatch(humanStatement))
            {
                return new Statement(humanName, Statement.MODE_META_OPERATION, "muct", words[2], words[6], isNegative, humanStatement);
            }
            //if pine madeof wood and wood isa material then pine madeof material
            //Present subject in result (Liffid)
            //inverse
            else if (liffidA.IsMatch(humanStatement))
            {
                return new Statement(humanName, Statement.MODE_META_OPERATION, "liffid", words[6], words[2], isNegative, humanStatement);
            }
            else if (liffidB.IsMatch(humanStatement))
            {
                return new Statement(humanName, Statement.MODE_META_OPERATION, "liffid", words[2], words[6], isNegative, humanStatement);
            }
            //if tree madeof wood and tree someare pine then pine madeof wood
            //Abscent subject in result (Sublar)
            //direct
            else if (sublarA.IsMatch(humanStatement))
            {
                return new Statement(humanName, Statement.MODE_META_OPERATION, "sublar", words[6], words[2], isNegative, humanStatement);
            }
            else if (sublarB.IsMatch(humanStatement))
            {
                return new Statement(humanName, Statement.MODE_META_OPERATION, "sublar", words[2], words[6], isNegative, humanStatement);
            }
            //if wood partof tree and pine isa tree then wood partof pine
            //Present subject in result (Consics)
            //direct
            else if (consicsA.IsMatch(humanStatement))
            {
                return new Statement(humanName, Statement.MODE_META_OPERATION, "consics", words[6], words[2], isNegative, humanStatement);
            }
            else if (consicsB.IsMatch(humanStatement))
            {
                return new Statement(humanName, Statement.MODE_META_OPERATION, "consics", words[2], words[6], isNegative, humanStatement);
            }
            else
            {
                if (isNegative)
                {
                    return implyStatementFactory.GetInterpretedHumanStatement(humanName, "not " + humanStatement);
                }
                else
                {
                    return implyStatementFactory.GetInterpretedHumanStatement(humanName, humanStatement);
                }
            }
        }
        #endregion
        #endregion
    }
}