using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Text;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Build ternary implication statements
    /// </summary>
    class MetaOperationTernaryImplicationStatementFactory : AbstractStatementFactory
    {
        #region Fields and parts
        /// <summary>
        /// if pine isa tree and tree madeof wood then pine madeof wood
        /// </summary>
        private static readonly Regex muctA = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and \3 ([a-z0-9_()]+) ([a-z0-9_()]+) then \1 \4 \5");

        /// <summary>
        /// if tree madeof wood and pine isa tree then pine madeof wood
        /// </summary>
        private static readonly Regex muctB = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and ([a-z0-9_()]+) ([a-z0-9_()]+) \1 then \4 \2 \3");

        /// <summary>
        /// if wood isa material and pine madeof wood then pine madeof material
        /// </summary>
        private static readonly Regex liffidA = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and ([a-z0-9_()]+) ([a-z0-9_()]+) \1 then \4 \5 \3");
        
        /// <summary>
        /// if pine madeof wood and wood isa material then pine madeof material
        /// </summary>
        private static readonly Regex liffidB = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and \3 ([a-z0-9_()]+) ([a-z0-9_()]+) then \1 \2 \5");
        
        /// <summary>
        /// if tree someare pine and tree madeof wood then pine madeof wood
        /// </summary>
        private static readonly Regex sublarA = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and \1 ([a-z0-9_()]+) ([a-z0-9_()]+) then \3 \4 \5");
        
        /// <summary>
        /// if tree madeof wood and tree someare pine then pine madeof wood
        /// </summary>
        private static readonly Regex sublarB = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and \1 ([a-z0-9_()]+) ([a-z0-9_()]+) then \5 \2 \3");
        
        /// <summary>
        /// if pine isa tree and wood partof tree then wood partof pine
        /// </summary>
        private static readonly Regex consicsA = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and ([a-z0-9_()]+) ([a-z0-9_()]+) \3 then \4 \5 \1");

        /// <summary>
        /// if wood partof tree and pine isa tree then wood partof pine
        /// </summary>
        private static readonly Regex consicsB = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and ([a-z0-9_()]+) ([a-z0-9_()]+) \3 then \1 \2 \4");

        /// <summary>
        /// if bird need tree and tree need bird then bird synergize tree
        /// </summary>
        private static readonly Regex xor_to_andA = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and \3 \2 \1 then \1 ([a-z0-9_()]+) \3");

        /// <summary>
        /// if bird need tree and tree need bird then tree synergize bird
        /// </summary>
        private static readonly Regex xor_to_andB = new Regex(@"if ([a-z0-9_()]+) ([a-z0-9_()]+) ([a-z0-9_()]+) and \3 \2 \1 then \3 ([a-z0-9_()]+) \1");

        /// <summary>
        /// Imply statement factory
        /// </summary>
        private ImplyStatementFactory implyStatementFactory = new ImplyStatementFactory();
        #endregion

        #region Public Methods
        /// <summary>
        /// Build ternary implication statement
        /// </summary>
        /// <param name="humanName">human's name</param>
        /// <param name="humanStatement">human's raw statement</param>
        /// <returns>ternary implication statement</returns>
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
            //if bird need tree and tree need bird then bird synergize tree
            else if (xor_to_andA.IsMatch(humanStatement))
            {
                return new Statement(humanName, Statement.MODE_META_OPERATION, "xor_to_and", words[10], words[2], isNegative, humanStatement);
            }
            //if bird need tree and tree need bird then tree synergize bird
            else if (xor_to_andB.IsMatch(humanStatement))
            {
                return new Statement(humanName, Statement.MODE_META_OPERATION, "xor_to_and", words[10], words[2], isNegative, humanStatement);
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
    }
}