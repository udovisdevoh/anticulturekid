using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Text;

namespace AntiCulture.Kid
{
    static class LanguageDictionary
    {
        #region Fields
        private static readonly List<string> primitiveOperatorList = new List<string> { "and",
                                                                                        "nor",
                                                                                        "if",
                                                                                        "then",
                                                                                        "not",
                                                                                        "why",
                                                                                        "how",
                                                                                        "does",
                                                                                        "where",
                                                                                        "which",
                                                                                        "visualize_why" };

        private static readonly List<string> nullaryOperatorList = new List<string> { "yes",
                                                                                      "no",
                                                                                      "think",
                                                                                      "linguisthink",
                                                                                      "phonothink",
                                                                                      "ask",
                                                                                      "talk",
                                                                                      "teach",
                                                                                      "think",
                                                                                      "metathink",
                                                                                      "analogize",
                                                                                      "exit",
                                                                                      "statcross",
                                                                                      "select",
                                                                                      "update",
                                                                                      "imply"};

        private static readonly List<string> unaryOperatorList = new List<string> { "askabout",
                                                                                    "define",
                                                                                    "talkabout",
                                                                                    "teachabout",
                                                                                    "thinkabout",
                                                                                    "linguisthinkabout",
                                                                                    "phonothinkabout",
                                                                                    "metathinkabout",
                                                                                    "whatis",
                                                                                    "visualize",
                                                                                    "neologize",
                                                                                    "start_psychosis"};

        private static readonly List<string> nameMappingOperatorList = new List<string> { "aliasof",
                                                                                          "rename" };

        private static readonly List<string> metaOperatorList = new List<string> { "permutable_side",
                                                                                   "inverse_of",
                                                                                   "direct_implication",
                                                                                   "inverse_implication",
                                                                                   "cant",
                                                                                   "unlikely",
                                                                                   "muct",
                                                                                   "sublar",
                                                                                   "liffid",
                                                                                   "consics",
                                                                                   "conservative_thinking",
                                                                                   "xor_to_and" };
        #endregion

        #region Methods
        #region Getters
        public static bool StringContainsSpecialKeyWords(string text)
        {
            foreach (string operatorName in PrimitiveOperatorList)
                if (text.ContainsWord(operatorName))
                    return true;

            foreach (string operatorName in NullaryOperatorList)
                if (text.ContainsWord(operatorName))
                    return true;

            foreach (string operatorName in UnaryOperatorList)
                if (text.ContainsWord(operatorName))
                    return true;

            foreach (string operatorName in NameMappingOperatorList)
                if (text.ContainsWord(operatorName))
                    return true;

            return false;
        }

        public static bool StringContainsSpecialKeyWordsOrMetaOperators(string text)
        {
            if (LanguageDictionary.StringContainsSpecialKeyWords(text))
                return true;

            foreach (string operatorName in MetaOperatorList)
                if (text.ContainsWord(operatorName))
                    return true;

            return false;
        }
        #endregion
        #endregion

        #region Properties
        public static List<string> PrimitiveOperatorList
        {
            get { return primitiveOperatorList; }
        }

        public static List<string> NullaryOperatorList
        {
            get { return nullaryOperatorList; }
        }

        public static List<string> UnaryOperatorList
        {
            get { return unaryOperatorList; }
        }

        public static List<string> MetaOperatorList
        {
            get { return metaOperatorList; }
        }

        public static List<string> NameMappingOperatorList
        {
            get { return nameMappingOperatorList; }
        }
        #endregion
    }
}