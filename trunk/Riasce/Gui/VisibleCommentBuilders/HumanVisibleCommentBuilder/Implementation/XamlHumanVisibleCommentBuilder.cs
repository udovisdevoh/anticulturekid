using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;
using Text;

namespace AntiCulture.Kid
{
    class XamlHumanVisibleCommentBuilder : AbstractHumanVisibleCommentBuilder
    {
        #region Fields
        private Brush humanOperatorColor = Brushes.DarkOrange;

        private Brush humanConceptColor = Brushes.Olive;
        #endregion

        #region Methods
        protected override Paragraph ConcreteBuild()
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(HumanName + ": ");

            if (UnparsableComment != null)
                paragraph.Inlines.Add(UnparsableComment);
            else if (ConceptOrOperatorNameList == null || (ConceptOrOperatorNameList.Count < 1 && !IsStatCross && !IsSelect) || (ConceptOrOperatorNameList.Count > 3 && !IsStatCross && !IsSelect))
                throw new VisibleCommentConstructionException("Expecting from 1 to 3 concept or operator words in ConceptOrOperatorNameList");

            if (IsAskingWhy)
                paragraph.Inlines.Add("why ");
            else if (IsAskingVisualizeWhy)
                paragraph.Inlines.Add("visualize_why ");

            #region For StatCross
            if (IsStatCross)
            {
                paragraph.Inlines.Add("statcross");

                if (ConceptOrOperatorNameList.Count >= 2)
                {
                    Span denominatorVerb = new Span();
                    denominatorVerb.Foreground = humanOperatorColor;
                    denominatorVerb.Inlines.Add(ConceptOrOperatorNameList[0]);
                    AddLink(denominatorVerb, "visualize " + ConceptOrOperatorNameList[0]);

                    Span denominatorComplement = new Span();
                    denominatorComplement.Foreground = humanConceptColor;
                    denominatorComplement.Inlines.Add(ConceptOrOperatorNameList[1]);
                    AddLink(denominatorComplement, "visualize " + ConceptOrOperatorNameList[1]);

                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(denominatorVerb);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(denominatorComplement);
                }
                
                if (ConceptOrOperatorNameList.Count >= 4)
                {
                    Span numeratorVerb = new Span();
                    numeratorVerb.Foreground = humanOperatorColor;
                    numeratorVerb.Inlines.Add(ConceptOrOperatorNameList[2]);
                    AddLink(numeratorVerb, "visualize " + ConceptOrOperatorNameList[2]);

                    Span numeratorComplement = new Span();
                    numeratorComplement.Foreground = humanConceptColor;
                    numeratorComplement.Inlines.Add(ConceptOrOperatorNameList[3]);
                    AddLink(numeratorComplement, "visualize " + ConceptOrOperatorNameList[3]);

                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(numeratorVerb);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(numeratorComplement);
                }

                return paragraph;
            }
            #endregion

            #region For Select or Update
            if (IsSelect)
            {
                paragraph.Inlines.Add("select where ");
            }
            else if (IsUpdate || IsImply)
            {
                if (IsNegative)
                    paragraph.Inlines.Add("not ");

                if (IsUpdate)
                    paragraph.Inlines.Add("update ");
                else if (IsImply)
                    paragraph.Inlines.Add("imply ");

                string action = UpdateAction;
                string[] words = action.Split(' ');
                int oddOrEven = 0;

                Span span;
                foreach (string word in words)
                {
                    span = new Span();
                    string currentWord = word.Trim();
                    if (currentWord == "or" || currentWord == "and" || currentWord == "not")
                    {
                        oddOrEven = 0;
                        span.Inlines.Add(word);
                    }
                    else
                    {
                        if (oddOrEven == 0)
                        {
                            span.Foreground = humanOperatorColor;
                            span = BlackenParantheses(span, word);
                            AddLink(span, "visualize " + word.Trim());
                            oddOrEven++;
                        }
                        else
                        {
                            span.Foreground = humanConceptColor;
                            span = BlackenParantheses(span, word);
                            AddLink(span, "visualize " + word.Trim());
                            oddOrEven = 0;
                        }
                    }

                    paragraph.Inlines.Add(span);

                    paragraph.Inlines.Add(" ");
                }

                paragraph.Inlines.Add("where ");
            }

            if (IsSelect || IsUpdate || IsImply)
            {
                string conditions = SelectCondition;

                conditions = conditions.Replace(")or(", ") or (");
                conditions = conditions.Replace(" or(", " or (");
                conditions = conditions.Replace(")or ", ") or ");
                conditions = conditions.Replace(")and(", ") and (");
                conditions = conditions.Replace(" and(", " and (");
                conditions = conditions.Replace(")and ", ") and ");
                conditions = conditions.Replace(")and not(", ") and not (");
                conditions = conditions.Replace(" and not(", " and not (");
                conditions = conditions.Replace(")and not ", ") and not ");

                conditions = conditions.RemoveDoubleSpaces();
                conditions = conditions.Trim();

                string[] words = conditions.Split(' ');
                int oddOrEven = 0;
                Span span;
                foreach (string word in words)
                {
                    span = new Span();
                    string currentWord = word.Trim();
                    if (currentWord == "or" || currentWord == "and" || currentWord == "not")
                    {                        
                        oddOrEven = 0;
                        span.Inlines.Add(word);
                    }
                    else
                    {
                        if (oddOrEven == 0)
                        {
                            span.Foreground = humanOperatorColor;
                            span = BlackenParantheses(span, word);
                            AddLink(span, "visualize " + word.Replace("(", "").Replace(")", "").Trim());
                            oddOrEven++;
                        }
                        else
                        {
                            span.Foreground = humanConceptColor;
                            span = BlackenParantheses(span, word);
                            AddLink(span, "visualize " + word.Replace("(", "").Replace(")", "").Trim());
                        }
                    }

                    paragraph.Inlines.Add(span);

                    paragraph.Inlines.Add(" ");
                }
                return paragraph;
            }
            #endregion

            if (IsAnalogy && (ConceptOrOperatorNameList.Count < 1 || ConceptOrOperatorNameList[0] != "analogize"))
                paragraph.Inlines.Add("analogize ");

            if (ConceptOrOperatorNameList.Count == 1)
            {
                Span operatorNameHumanSide = new Span();
                operatorNameHumanSide.Foreground = humanOperatorColor;
                operatorNameHumanSide.Inlines.Add(ConceptOrOperatorNameList[0]);
                paragraph.Inlines.Add(operatorNameHumanSide);

                if (ConceptOrOperatorNameList[0] == "start_psychosis")
                {
                    paragraph.Inlines.Add(" ");
                    Span probability = new Span();
                    probability.Inlines.Add(this.Probability);
                    probability.Foreground = humanConceptColor;
                    paragraph.Inlines.Add(probability);
                }
            }
            else if (ConceptOrOperatorNameList.Count == 2)
            {
                Span operatorNameHumanSide = new Span();
                operatorNameHumanSide.Foreground = humanOperatorColor;
                operatorNameHumanSide.Inlines.Add(ConceptOrOperatorNameList[0]);

                Span conceptNameHumanSide = new Span();
                conceptNameHumanSide.Foreground = humanConceptColor;
                conceptNameHumanSide.Inlines.Add(ConceptOrOperatorNameList[1]);
                AddLink(conceptNameHumanSide, "visualize " + ConceptOrOperatorNameList[1]);

                paragraph.Inlines.Add(operatorNameHumanSide);
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(conceptNameHumanSide);
            }
            else if (ConceptOrOperatorNameList.Count == 3)
            {
                Span concept1NameHumanSide = new Span();
                concept1NameHumanSide.Foreground = humanConceptColor;
                concept1NameHumanSide.Inlines.Add(ConceptOrOperatorNameList[0]);
                AddLink(concept1NameHumanSide, "visualize " + ConceptOrOperatorNameList[0]);

                Span operatorNameHumanSide = new Span();
                operatorNameHumanSide.Foreground = humanOperatorColor;
                operatorNameHumanSide.Inlines.Add(ConceptOrOperatorNameList[1]);
                AddLink(operatorNameHumanSide, "visualize " + ConceptOrOperatorNameList[1]);

                Span concept2NameHumanSide = new Span();
                concept2NameHumanSide.Foreground = humanConceptColor;
                concept2NameHumanSide.Inlines.Add(ConceptOrOperatorNameList[2]);
                AddLink(concept2NameHumanSide, "visualize " + ConceptOrOperatorNameList[2]);

                paragraph.Inlines.Add(concept1NameHumanSide);
                paragraph.Inlines.Add(" ");

                if (IsNegative == true)
                    paragraph.Inlines.Add("not ");

                paragraph.Inlines.Add(operatorNameHumanSide);
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(concept2NameHumanSide);

                if (OriginalStatementThatGotAltered != null)
                    paragraph.Inlines.Add(" //" + OriginalStatementThatGotAltered);
            }

            if (IsInterrogative || IsAskingWhy || IsAskingVisualizeWhy)
                paragraph.Inlines.Add("?");

            return paragraph;
        }
        #endregion
    }
}
