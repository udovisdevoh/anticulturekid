using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Xaml Ai visible comment builder
    /// </summary>
    class XamlAiVisibleCommentBuilder : AbstractAiVisibleCommentBuilder
    {
        #region Fields
        private Brush exceptionColor = Brushes.Red;

        private Brush aiConceptColor;

        private Brush aiOperatorColor = Brushes.Blue;

        private Brush aiCommentColor;

        private Brush aiFeelingColor = Brushes.Indigo;
        #endregion

        #region Constructor
        public XamlAiVisibleCommentBuilder()
        {
            aiConceptColor = new SolidColorBrush(Color.FromRgb(43,145,175));
            aiCommentColor = new SolidColorBrush(Color.FromRgb(0, 128, 0));
        }
        #endregion

        #region Methods
        protected override Paragraph ConcreteBuild()
        {
            if (ExceptionMessage != null)
            {
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(AiName + ": ");
                Span exceptionMessage = new Span();
                exceptionMessage.Foreground = exceptionColor;
                exceptionMessage.Inlines.Add(ExceptionMessage);

                paragraph.Inlines.Add(exceptionMessage);

                return paragraph;
            }
            else if (StatCrossExpression != null)
            {
                return GetAnswerStatCrossExpression(StatCrossExpression, StatCrossValue);
            }
            else if (ImplyConnectionMessage != null)
            {
                return GetAnswerImply(ImplyConnectionMessage);
            }
            else if (FeelingMessage != null)
            {
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(AiName + ": ");
                Span feelingMessage = new Span();
                feelingMessage.Foreground = aiFeelingColor;
                feelingMessage.Inlines.Add(FeelingMessage);

                paragraph.Inlines.Add(feelingMessage);

                return paragraph;
            }
            else if (Trauma != null)
            {
                return GetAnswerTrauma(Trauma);
            }
            else if (Dream != null)
            {
                return GetAnswerToDream(Dream);
            }
            else if (Selection != null)
            {
                return GetAnswerSelect(Selection);
            }
            else if (StatementStubAskingForRandomConnection != null)
            {
                return GetAnswerStubAskingForRandomConnection(StatementStubAskingForRandomConnection);
            }
            else if (StatementAcknowledge != null)
            {
                return GetAnswerStatementAcknowledge(StatementAcknowledge);
            }
            else if (StatementAcknowledgeAndLearn != null)
            {
                return GetAnswerStatementAcknowledgeAndLearn(StatementAcknowledgeAndLearn);
            }
            else if (StatementNotAcknowledging != null)
            {
                return GetAnswerStatementNotAcknowledging(StatementNotAcknowledging);
            }
            else if (Analogy != null)
            {
                return GetAnswerToAnalogy(Analogy);
            }
            else if (ProofDefinitionListRegular != null)
            {
                return GetAnswerProof(ProofDefinitionListRegular, "I will explain you why", true, false);
            }
            else if (ProofDefinitionListRefutation != null)
            {
                return GetAnswerProof(ProofDefinitionListRefutation, "I disagree because", false, false);
            }
            else if (ProofDefinitionListPositiveRefutation != null)
            {
                return GetAnswerProof(ProofDefinitionListPositiveRefutation, "I disagree because", true, false);
            }
            else if (ProofDefinitionListTeachabout != null)
            {
                return GetAnswerProof(ProofDefinitionListTeachabout, "I will teach you why", true, true);
            }
            else if (ProofDefinitionListRefutationTeachabout != null)
            {
                return GetAnswerProof(ProofDefinitionListRefutationTeachabout, "I will teach you why", false, true);
            }
            else if (DefinitionListWhatis != null && ConceptToDefine != null)
            {
                return GetAnswerDefinitionWhatis(ConceptToDefine, DefinitionListWhatis, DefinitionListPositiveImply, DefinitionListNegativeImply);
            }
            else if (DefinitionListDefine != null && ConceptToDefine != null)
            {
                return GetAnswerDefinitionDefine(ConceptToDefine, DefinitionListDefine, DefinitionListPositiveImply, DefinitionListNegativeImply);
            }
            else if (Theory != null)
            {
                return GetAnswerTheory(Theory);
            }
            else if (IsDiscardingTheory)
            {
                return GetAnswerDiscardingTheory();
            }

            throw new VisibleCommentConstructionException("Couldn't build answer message from construction settings");
        }

        private Paragraph GetAnswerImply(string implyConnectionMessage)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(AiName + ": ");

            if (implyConnectionMessage == "No changes made")
            {
                Span changes = new Span();
                changes.Inlines.Add("changes");
                changes.Foreground = aiConceptColor;

                Span made = new Span();
                made.Inlines.Add("made");
                made.Foreground = aiOperatorColor;

                paragraph.Inlines.Add("no");
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(changes);
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(made);
            }
            else if (implyConnectionMessage == "Imply connection created")
            {
                Span connection = new Span();
                connection.Inlines.Add("connection");
                connection.Foreground = aiConceptColor;

                Span created = new Span();
                created.Inlines.Add("created");
                created.Foreground = aiOperatorColor;

                paragraph.Inlines.Add("imply");
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(connection);
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(created);
            }
            else if (implyConnectionMessage == "Imply connection removed")
            {
                Span connection = new Span();
                connection.Inlines.Add("connection");
                connection.Foreground = aiConceptColor;

                Span removed = new Span();
                removed.Inlines.Add("removed");
                removed.Foreground = aiOperatorColor;

                paragraph.Inlines.Add("imply");
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(connection);
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(removed);
            }
            else
            {
                paragraph.Inlines.Add(implyConnectionMessage);
            }

            return paragraph;
        }

        private Paragraph GetAnswerSelect(HashSet<string> selection)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(AiName + ": ");

            if (selection.Count < 1)
            {
                paragraph.Inlines.Add("no match found");
                return paragraph;
            }

            int counter = 0;
            foreach (string conceptName in selection)
            {
                Span span = new Span();
                span.Foreground = aiConceptColor;
                span.Inlines.Add(conceptName);
                AddLink(span, "InvertYouAndMePov visualize " + conceptName);
                paragraph.Inlines.Add(span);
                if (counter == selection.Count - 2)
                    paragraph.Inlines.Add(" and ");
                else if (counter == selection.Count - 1)
                    paragraph.Inlines.Add(".");
                else
                    paragraph.Inlines.Add(", ");
                counter++;
            }
            return paragraph;
        }

        private Paragraph GetAnswerTrauma(List<string> Trauma)
        {
            Paragraph paragraph = new Paragraph();

            paragraph.Inlines.Add(AiName + ": ");

            Span me = new Span();
            me.Foreground = aiConceptColor;
            me.Inlines.Add("me");

            Span think = new Span();
            think.Foreground = aiOperatorColor;
            think.Inlines.Add("think");

            paragraph.Inlines.Add(me);
            paragraph.Inlines.Add(" need to ");
            paragraph.Inlines.Add(think);
            paragraph.Inlines.Add(" now...");
            paragraph.Inlines.Add(new LineBreak());

            for (int i = 0; i < Trauma.Count; i += 6)
            {
                Span nonsenseSubject = new Span();
                nonsenseSubject.Foreground = aiConceptColor;
                nonsenseSubject.Inlines.Add(Trauma[i]);
                AddLink(nonsenseSubject, "InvertYouAndMePov visualize " + Trauma[i]);

                Span nonsenseVerb = new Span();
                nonsenseVerb.Foreground = aiOperatorColor;
                nonsenseVerb.Inlines.Add(Trauma[i + 1]);
                AddLink(nonsenseVerb, "InvertYouAndMePov visualize " + Trauma[i + 1]);

                Span nonsenseComplement = new Span();
                nonsenseComplement.Foreground = aiConceptColor;
                nonsenseComplement.Inlines.Add(Trauma[i + 2]);
                AddLink(nonsenseComplement, "InvertYouAndMePov visualize " + Trauma[i + 2]);


                Span obstructionSubject = new Span();
                obstructionSubject.Foreground = aiConceptColor;
                obstructionSubject.Inlines.Add(Trauma[i + 3]);
                AddLink(obstructionSubject, "InvertYouAndMePov visualize " + Trauma[i + 3]);

                Span obstructionVerb = new Span();
                obstructionVerb.Foreground = aiOperatorColor;
                obstructionVerb.Inlines.Add(Trauma[i + 4]);
                AddLink(obstructionVerb, "InvertYouAndMePov visualize " + Trauma[i + 4]);

                Span obstructionComplement = new Span();
                obstructionComplement.Foreground = aiConceptColor;
                obstructionComplement.Inlines.Add(Trauma[i + 5]);
                AddLink(obstructionComplement, "InvertYouAndMePov visualize " + Trauma[i + 5]);

                paragraph.Inlines.Add(nonsenseSubject);
                paragraph.Inlines.Add(" not ");
                paragraph.Inlines.Add(nonsenseVerb);
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(nonsenseComplement);
                paragraph.Inlines.Add(" because ");
                paragraph.Inlines.Add(obstructionSubject);
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(obstructionVerb);
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(obstructionComplement);
                paragraph.Inlines.Add(new LineBreak());
            }
            
            return paragraph;
        }

        private Paragraph GetAnswerDiscardingTheory()
        {
            Paragraph paragraph = new Paragraph();

            Span you = new Span();
            you.Foreground = aiConceptColor;
            you.Inlines.Add("you");

            Span tell = new Span();
            tell.Foreground = aiOperatorColor;
            tell.Inlines.Add("tell");

            Span me = new Span();
            me.Foreground = aiConceptColor;
            me.Inlines.Add("me");

            Span talk = new Span();
            talk.Foreground = aiOperatorColor;
            talk.Inlines.Add("talk");

            paragraph.Inlines.Add(AiName + ": Alright... ");
            paragraph.Inlines.Add(you);
            paragraph.Inlines.Add(" may ");
            paragraph.Inlines.Add(tell);
            paragraph.Inlines.Add(" ");
            paragraph.Inlines.Add(me);
            paragraph.Inlines.Add(" why or ");
            paragraph.Inlines.Add(talk);
            paragraph.Inlines.Add(" about something else.");

            return paragraph;
        }

        private Paragraph GetAnswerToAnalogy(List<string> Analogy)
        {
            if (Analogy.Count < 2)
            {
                FeelingMonitor.Add(FeelingMonitor.EMPTINESS);
                throw new VisibleCommentConstructionException("Couldn't create analogy");
            }

            string source;
            string[] sourceWords;

            Paragraph paragraph = new Paragraph();
            source = Analogy[0];
            sourceWords = source.Split();

            Span sourceSubject = new Span();
            sourceSubject.Foreground = aiConceptColor;
            sourceSubject.Inlines.Add(sourceWords[0]);
            AddLink(sourceSubject, "InvertYouAndMePov visualize " + sourceWords[0]);

            Span sourceVerb = new Span();
            sourceVerb.Foreground = aiOperatorColor;
            sourceVerb.Inlines.Add(sourceWords[1]);
            AddLink(sourceVerb, "InvertYouAndMePov visualize " + sourceWords[1]);

            Span sourceComplement = new Span();
            sourceComplement.Foreground = aiConceptColor;
            sourceComplement.Inlines.Add(sourceWords[2]);
            AddLink(sourceComplement, "InvertYouAndMePov visualize " + sourceWords[2]);

            source = Analogy[1];
            sourceWords = source.Split();

            Span firstPerson = new Span();
            firstPerson.Foreground = aiConceptColor;
            firstPerson.Inlines.Add("me");
            AddLink(firstPerson, "InvertYouAndMePov visualize " + "me");

            Span targetSubject = new Span();
            targetSubject.Foreground = aiConceptColor;
            targetSubject.Inlines.Add(sourceWords[0]);
            AddLink(targetSubject, "InvertYouAndMePov visualize " + sourceWords[0]);

            Span targetVerb = new Span();
            targetVerb.Foreground = aiOperatorColor;
            targetVerb.Inlines.Add(sourceWords[1]);
            AddLink(targetVerb, "InvertYouAndMePov visualize " + sourceWords[1]);

            Span targetComplement = new Span();
            targetComplement.Foreground = aiConceptColor;
            targetComplement.Inlines.Add(sourceWords[2]);
            AddLink(targetComplement, "InvertYouAndMePov visualize " + sourceWords[2]);

            paragraph.Inlines.Add(AiName + ": Considering ");
            paragraph.Inlines.Add(sourceSubject);
            paragraph.Inlines.Add(" ");
            paragraph.Inlines.Add(sourceVerb);
            paragraph.Inlines.Add(" ");
            paragraph.Inlines.Add(sourceComplement);
            paragraph.Inlines.Add("...");
            paragraph.Inlines.Add(new LineBreak());
            paragraph.Inlines.Add("It makes ");
            paragraph.Inlines.Add(firstPerson);
            paragraph.Inlines.Add(" think ");
            paragraph.Inlines.Add(targetSubject);
            paragraph.Inlines.Add(" ");
            paragraph.Inlines.Add(targetVerb);
            paragraph.Inlines.Add(" ");
            paragraph.Inlines.Add(targetComplement);
            paragraph.Inlines.Add("...");
            return paragraph;
        }

        private Paragraph GetAnswerTheory(List<string> theory)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(AiName + ": Because");
            paragraph.Inlines.Add(new LineBreak());

            string[] words;
            int counter = 0;
            foreach (string argumentOrHypothesis in theory)
            {
                words = argumentOrHypothesis.Split(' ');

                if (counter == theory.Count - 1)
                    paragraph.Inlines.Add("Maybe ");

                if (LanguageDictionary.MetaOperatorList.Contains(words[1]))
                {
                    Paragraph metaConnectionSourceCodeParagraph = GetMetaConnectionSourceCode(words[0], words[1], words[2]);
                    paragraph.Inlines.AddRange(new List<Inline>(metaConnectionSourceCodeParagraph.Inlines));
                }
                else
                {
                    Span subject = new Span();
                    subject.Foreground = aiConceptColor;
                    subject.Inlines.Add(words[0]);
                    AddLink(subject, "InvertYouAndMePov visualize " + words[0]);

                    Span verb = new Span();
                    verb.Foreground = aiOperatorColor;
                    verb.Inlines.Add(words[1]);
                    AddLink(verb, "InvertYouAndMePov visualize " + words[1]);

                    Span complement = new Span();
                    complement.Foreground = aiConceptColor;
                    complement.Inlines.Add(words[2]);
                    AddLink(complement, "InvertYouAndMePov visualize " + words[2]);

                    paragraph.Inlines.Add(subject);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(verb);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(complement);
                }

                if (counter == theory.Count - 1)
                    paragraph.Inlines.Add(" too... what do you think?");
                else
                    paragraph.Inlines.Add(new LineBreak());

                counter++;
            }

            return paragraph;
        }

        private Paragraph GetMetaConnectionSourceCode(string verbName, string metaOperatorName, string farVerbName)
        {
            Paragraph paragraph = new Paragraph();

            paragraph.Inlines.Add(new LineBreak());

            Span comment = new Span();
            comment.Foreground = aiCommentColor;
            comment.Inlines.Add("//" + verbName + " " + metaOperatorName + " " + farVerbName);

            paragraph.Inlines.Add(comment);
            paragraph.Inlines.Add(new LineBreak());

            Span alpha1 = new Span();
            alpha1.Foreground = aiConceptColor;
            alpha1.Inlines.Add("alpha");

            Span alpha2 = new Span();
            alpha2.Foreground = aiConceptColor;
            alpha2.Inlines.Add("alpha");

            Span alpha3 = new Span();
            alpha3.Foreground = aiConceptColor;
            alpha3.Inlines.Add("alpha");

            Span beta1 = new Span();
            beta1.Foreground = aiConceptColor;
            beta1.Inlines.Add("beta");

            Span beta2 = new Span();
            beta2.Foreground = aiConceptColor;
            beta2.Inlines.Add("beta");

            Span beta3 = new Span();
            beta3.Foreground = aiConceptColor;
            beta3.Inlines.Add("beta");

            Span gamma1 = new Span();
            gamma1.Foreground = aiConceptColor;
            gamma1.Inlines.Add("gamma");

            Span gamma2 = new Span();
            gamma2.Foreground = aiConceptColor;
            gamma2.Inlines.Add("gamma");

            Span realOperationVerb1 = new Span();
            realOperationVerb1.Foreground = aiConceptColor;
            realOperationVerb1.Inlines.Add(verbName);
            AddLink(realOperationVerb1, "InvertYouAndMePov visualize " + verbName);

            Span realOperationVerb2 = new Span();
            realOperationVerb2.Foreground = aiConceptColor;
            realOperationVerb2.Inlines.Add(farVerbName);
            AddLink(realOperationVerb2, "InvertYouAndMePov visualize " + farVerbName);

            Span realOperationMetaOperator = new Span();
            realOperationMetaOperator.Foreground = aiOperatorColor;
            realOperationMetaOperator.Inlines.Add(metaOperatorName);

            Span verb1 = new Span();
            verb1.Foreground = aiOperatorColor;
            verb1.Inlines.Add(verbName);
            AddLink(verb1, "InvertYouAndMePov visualize " + verbName);

            Span verb2 = new Span();
            verb2.Foreground = aiOperatorColor;
            verb2.Inlines.Add(verbName);
            AddLink(verb2, "InvertYouAndMePov visualize " + verbName);

            Span farVerb1 = new Span();
            farVerb1.Foreground = aiOperatorColor;
            farVerb1.Inlines.Add(farVerbName);
            AddLink(farVerb1, "InvertYouAndMePov visualize " + farVerbName);

            Span farVerb2 = new Span();
            farVerb2.Foreground = aiOperatorColor;
            farVerb2.Inlines.Add(farVerbName);
            AddLink(farVerb2, "InvertYouAndMePov visualize " + farVerbName);

            switch (metaOperatorName)
            {
                case "permutable_side":
                case "inverse_of":
                    paragraph.Inlines.Add(alpha1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(verb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(beta1);
                    paragraph.Inlines.Add(" = ");
                    paragraph.Inlines.Add(beta2);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(farVerb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(alpha2);
                    break;
                case "direct_implication":
                    paragraph.Inlines.Add("if ");
                    paragraph.Inlines.Add(alpha1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(verb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(beta1);
                    paragraph.Inlines.Add(" then ");
                    paragraph.Inlines.Add(alpha2);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(farVerb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(beta2);
                    break;
                case "inverse_implication":
                    paragraph.Inlines.Add("if ");
                    paragraph.Inlines.Add(alpha1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(verb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(beta1);
                    paragraph.Inlines.Add(" then ");
                    paragraph.Inlines.Add(beta2);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(farVerb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(alpha2);
                    break;
                case "cant":
                    paragraph.Inlines.Add("if ");
                    paragraph.Inlines.Add(alpha1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(verb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(beta1);
                    paragraph.Inlines.Add(" then ");
                    paragraph.Inlines.Add(alpha2);
                    paragraph.Inlines.Add(" cant ");
                    paragraph.Inlines.Add(farVerb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(beta2);
                    break;
                case "unlikely":
                    paragraph.Inlines.Add("if ");
                    paragraph.Inlines.Add(alpha1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(verb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(beta1);
                    paragraph.Inlines.Add(" then ");
                    paragraph.Inlines.Add(alpha2);
                    paragraph.Inlines.Add(" unlikely ");
                    paragraph.Inlines.Add(farVerb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(beta2);
                    break;
                case "muct":
                    paragraph.Inlines.Add("if ");
                    paragraph.Inlines.Add(alpha1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(farVerb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(beta1);
                    paragraph.Inlines.Add(" and ");
                    paragraph.Inlines.Add(beta2);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(verb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(gamma1);
                    paragraph.Inlines.Add(" then ");
                    paragraph.Inlines.Add(alpha2);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(verb2);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(gamma2);
                    break;
                case "sublar":
                    paragraph.Inlines.Add("if ");
                    paragraph.Inlines.Add(beta1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(farVerb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(alpha1);
                    paragraph.Inlines.Add(" and ");
                    paragraph.Inlines.Add(beta2);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(verb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(gamma1);
                    paragraph.Inlines.Add(" then ");
                    paragraph.Inlines.Add(alpha2);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(verb2);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(gamma2);
                    break;
                case "liffid":
                    paragraph.Inlines.Add("if ");
                    paragraph.Inlines.Add(alpha1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(verb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(beta1);
                    paragraph.Inlines.Add(" and ");
                    paragraph.Inlines.Add(beta2);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(farVerb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(gamma1);
                    paragraph.Inlines.Add(" then ");
                    paragraph.Inlines.Add(alpha2);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(verb2);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(gamma2);
                    break;
                case "consics":
                    paragraph.Inlines.Add("if ");
                    paragraph.Inlines.Add(alpha1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(verb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(beta1);
                    paragraph.Inlines.Add(" and ");
                    paragraph.Inlines.Add(gamma1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(farVerb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(beta2);
                    paragraph.Inlines.Add(" then ");
                    paragraph.Inlines.Add(alpha2);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(verb2);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(gamma2);
                    break;
                case "conservative_thinking":
                    paragraph.Inlines.Add(realOperationVerb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(realOperationMetaOperator);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(realOperationVerb2);
                    break;
                case "xor_to_and":
                    paragraph.Inlines.Add("if ");
                    paragraph.Inlines.Add(alpha1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(farVerb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(beta1);
                    paragraph.Inlines.Add(" and ");
                    paragraph.Inlines.Add(beta2);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(farVerb2);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(alpha2);
                    paragraph.Inlines.Add(" then ");
                    paragraph.Inlines.Add(alpha3);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(verb1);
                    paragraph.Inlines.Add(" ");
                    paragraph.Inlines.Add(beta3);
                    break;
                default:
                    paragraph.Inlines.Add(farVerb1);
                    paragraph.Inlines.Add(" " + metaOperatorName + " ");
                    paragraph.Inlines.Add(farVerb2);
                    break;
            }
            return paragraph;
        }

        private Paragraph GetAnswerStubAskingForRandomConnection(string statementStubAskingForRandomConnection)
        {
            Paragraph paragraph = new Paragraph();


            List<string> words = new List<string>(statementStubAskingForRandomConnection.Split(' '));

            if (words.Count != 2)
                throw new VisibleCommentConstructionException("Couldn't build answer message from question stub. Expecting exactly 2 words.");


            if (DefinitionListWhatis != null && ConceptToDefine != null && DefinitionListWhatis.Count > 0)
            {
                Paragraph definitionWhatis = GetAnswerDefinitionWhatis(ConceptToDefine, DefinitionListWhatis, DefinitionListPositiveImply, DefinitionListNegativeImply);
                paragraph.Inlines.AddRange(new List<Inline>(definitionWhatis.Inlines));
                paragraph.Inlines.Add(new LineBreak());
            }
            

            Span conceptName = new Span();
            conceptName.Foreground = aiConceptColor;
            conceptName.Inlines.Add(words[0]);
            AddLink(conceptName, "InvertYouAndMePov visualize " + words[0]);
            


            Span operatorName = new Span();
            operatorName.Foreground = aiOperatorColor;
            operatorName.Inlines.Add(words[1]);
            AddLink(operatorName, "InvertYouAndMePov visualize " + words[1]);


            paragraph.Inlines.Add(AiName + ": ");
            paragraph.Inlines.Add(conceptName);
            paragraph.Inlines.Add(" ");
            paragraph.Inlines.Add(operatorName);
            paragraph.Inlines.Add(" what?");
            return paragraph;
        }

        private Paragraph GetAnswerStatementAcknowledge(string statement)
        {
            Paragraph paragraph = new Paragraph();

            string[] words = statement.Split(' ');
            if (words.Length == 3 || (words.Length == 4 && words[1] == "not"))
            {
                Span subject = new Span();
                subject.Foreground = aiConceptColor;
                subject.Inlines.Add(words[0]);
                AddLink(subject, "InvertYouAndMePov visualize " + words[0]);

                Span verb = new Span();
                verb.Foreground = aiOperatorColor;
                if (words[1] == "not")
                {
                    verb.Inlines.Add(words[2]);
                    if (!LanguageDictionary.MetaOperatorList.Contains(words[2]))
                        AddLink(verb, "InvertYouAndMePov visualize " + words[2]);
                }
                else
                {
                    verb.Inlines.Add(words[1]);
                    if (!LanguageDictionary.MetaOperatorList.Contains(words[1]))
                        AddLink(verb, "InvertYouAndMePov visualize " + words[1]);
                }

                Span complement = new Span();
                complement.Foreground = aiConceptColor;
                if (words[1] == "not")
                {
                    complement.Inlines.Add(words[3]);
                    AddLink(complement, "InvertYouAndMePov visualize " + words[3]);
                }
                else
                {
                    complement.Inlines.Add(words[2]);
                    AddLink(complement, "InvertYouAndMePov visualize " + words[2]);
                }

                if (words.Length == 4 && words[1] == "not")
                    paragraph.Inlines.Add(AiName + ": I agree, ");
                else
                    paragraph.Inlines.Add(AiName + ": yes, ");

                paragraph.Inlines.Add(subject);

                if (words.Length == 4 && words[1] == "not")
                    paragraph.Inlines.Add(" not");

                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(verb);

                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(complement);
            }
            else
            {
                throw new VisibleCommentConstructionException("Expecting a statement of exactly 3 or 4 words to acknowledge to (for instance: pine isa tree OR pine not isa tree)");
            }

            return paragraph;
        }

        private Paragraph GetAnswerStatementAcknowledgeAndLearn(string statement)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(AiName + ": alright, ");

            string[] words = statement.Split(' ');
            if (words.Length == 3)
            {
                Span subject = new Span();
                subject.Foreground = aiConceptColor;
                subject.Inlines.Add(words[0]);
                AddLink(subject, "InvertYouAndMePov visualize " + words[0]);

                Span verb = new Span();
                verb.Foreground = aiOperatorColor;
                verb.Inlines.Add(words[1]);
                if (!LanguageDictionary.MetaOperatorList.Contains(words[1]))
                    AddLink(verb, "InvertYouAndMePov visualize " + words[1]);

                Span complement = new Span();
                complement.Foreground = aiConceptColor;
                complement.Inlines.Add(words[2]);
                AddLink(complement, "InvertYouAndMePov visualize " + words[2]);

                paragraph.Inlines.Add(subject);
                paragraph.Inlines.Add(" now ");
                paragraph.Inlines.Add(verb);
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(complement);
            }
            else if (words.Length == 4 && words[1] == "not")
            {
                Span subject = new Span();
                subject.Foreground = aiConceptColor;
                subject.Inlines.Add(words[0]);
                AddLink(subject, "InvertYouAndMePov visualize " + words[0]);

                Span verb = new Span();
                verb.Foreground = aiOperatorColor;
                verb.Inlines.Add(words[2]);
                if (!LanguageDictionary.MetaOperatorList.Contains(words[2]))
                    AddLink(verb, "InvertYouAndMePov visualize " + words[2]);

                Span complement = new Span();
                complement.Foreground = aiConceptColor;
                complement.Inlines.Add(words[3]);
                AddLink(complement, "InvertYouAndMePov visualize " + words[3]);

                paragraph.Inlines.Add(subject);
                paragraph.Inlines.Add(" not ");
                paragraph.Inlines.Add(verb);
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(complement);
                paragraph.Inlines.Add(" anymore");
            }
            else
            {
                throw new VisibleCommentConstructionException("Expecting a statement of exactly 3 or 4 words to acknowledge to and learn from");
            }

            return paragraph;
        }

        private Paragraph GetAnswerStatementNotAcknowledging(string statement)
        {
            string[] words = statement.Split(' ');
            if (words.Length != 3)
                throw new VisibleCommentConstructionException("Expecting a statement of exactly 3 words not to acknowledge to");

            Paragraph paragraph = new Paragraph();

            Span firstPerson = new Span();
            firstPerson.Foreground = aiConceptColor;
            firstPerson.Inlines.Add("I");
            AddLink(firstPerson, "visualize you");

            Span know = new Span();
            know.Foreground = aiOperatorColor;
            know.Inlines.Add("know");

            Span subject = new Span();
            subject.Foreground = aiConceptColor;
            subject.Inlines.Add(words[0]);
            AddLink(subject, "InvertYouAndMePov visualize " + words[0]);

            Span verb = new Span();
            verb.Foreground = aiOperatorColor;
            verb.Inlines.Add(words[1]);
            AddLink(verb, "InvertYouAndMePov visualize " + words[1]);

            Span complement = new Span();
            complement.Foreground = aiConceptColor;
            complement.Inlines.Add(words[2]);
            AddLink(complement, "InvertYouAndMePov visualize " + words[2]);


            paragraph.Inlines.Add(AiName + ": ");
            paragraph.Inlines.Add(firstPerson);
            paragraph.Inlines.Add(" don't ");
            paragraph.Inlines.Add(know);
            paragraph.Inlines.Add(" whether ");
            paragraph.Inlines.Add(subject);
            paragraph.Inlines.Add(" ");
            paragraph.Inlines.Add(verb);
            paragraph.Inlines.Add(" ");
            paragraph.Inlines.Add(complement);
            paragraph.Inlines.Add(" or not");
            return paragraph;
        }

        private Paragraph GetAnswerStatCrossExpression(List<string> statCrossExpression, double statCrossValue)
        {
            Paragraph paragraph = new Paragraph();

            Span denominatorVerb = new Span();
            denominatorVerb.Inlines.Add(statCrossExpression[0]);
            denominatorVerb.Foreground = aiOperatorColor;
            AddLink(denominatorVerb, "InvertYouAndMePov visualize " + statCrossExpression[0]);

            Span denominatorComplement = new Span();
            denominatorComplement.Inlines.Add(statCrossExpression[1]);
            denominatorComplement.Foreground = aiConceptColor;
            AddLink(denominatorComplement, "InvertYouAndMePov visualize " + statCrossExpression[1]);

            Span numeratorVerb = new Span();
            numeratorVerb.Inlines.Add(statCrossExpression[2]);
            numeratorVerb.Foreground = aiOperatorColor;
            AddLink(numeratorVerb, "InvertYouAndMePov visualize " + statCrossExpression[2]);

            Span numeratorComplement = new Span();
            numeratorComplement.Inlines.Add(statCrossExpression[3]);
            numeratorComplement.Foreground = aiConceptColor;
            AddLink(numeratorComplement, "InvertYouAndMePov visualize " + statCrossExpression[3]);


            if (statCrossValue == -1)
            {
                paragraph.Inlines.Add(AiName + ": can't make statistics because nothing ");
                paragraph.Inlines.Add(denominatorVerb);
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(denominatorComplement);
                paragraph.Inlines.Add(" in my memory");
            }
            else
            {
                paragraph.Inlines.Add(AiName + ": from ");
                paragraph.Inlines.Add(denominatorVerb);
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(denominatorComplement);
                paragraph.Inlines.Add(", " + (int)(statCrossValue * 100) + "% ");
                paragraph.Inlines.Add(numeratorVerb);
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(numeratorComplement);
            }

            return paragraph;
        }

        private Paragraph GetAnswerToDream(List<string> dream)
        {
            Paragraph paragraph = new Paragraph();

            Span meSpan = new Span();
            meSpan.Foreground = aiConceptColor;
            meSpan.Inlines.Add("me");
            AddLink(meSpan, "InvertYouAndMePov visualize me");

            Span dreamSpan = new Span();
            dreamSpan.Foreground = aiOperatorColor;
            dreamSpan.Inlines.Add("dream");
            AddLink(dreamSpan, "InvertYouAndMePov visualize dream");

            paragraph.Inlines.Add(AiName + ": last night ");
            paragraph.Inlines.Add(meSpan);
            paragraph.Inlines.Add(" ");
            paragraph.Inlines.Add(dreamSpan);
            paragraph.Inlines.Add(" of ");

            bool fisrtShown = false;
            foreach (String line in dream)
            {
                string[] word = line.Split(' ');

                Span subjectSpan = new Span();
                subjectSpan.Foreground = aiConceptColor;
                subjectSpan.Inlines.Add(word[0]);
                AddLink(subjectSpan, "InvertYouAndMePov visualize " + word[0]);

                Span verbSpan = new Span();
                verbSpan.Foreground = aiOperatorColor;
                verbSpan.Inlines.Add(word[1]);
                AddLink(verbSpan, "InvertYouAndMePov visualize " + word[1]);

                Span complementSpan = new Span();
                complementSpan.Foreground = aiConceptColor;
                complementSpan.Inlines.Add(word[2]);
                AddLink(subjectSpan, "InvertYouAndMePov visualize " + word[2]);

                if (!fisrtShown)
                {
                    paragraph.Inlines.Add(new LineBreak());
                    paragraph.Inlines.Add(subjectSpan);
                }
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(verbSpan);
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(complementSpan);

                fisrtShown = true;
            }


            return paragraph;
        }

        #region Proofs
        private Paragraph GetAnswerProof(List<string> proofDefinitionList, string preProofComment, bool isPositiveProof, bool isTeachingWhy)
        {
            if (proofDefinitionList.Count < 1)
                throw new VisibleCommentConstructionException("The proof must contain at least 1 statement");


            Paragraph paragraph = new Paragraph();

            paragraph.Inlines.Add(AiName + ": " + preProofComment);

            if (isTeachingWhy)
                paragraph.Inlines.Add(" ");
            else
                paragraph.Inlines.Add(new LineBreak());

            if (isTeachingWhy)
            {
                Paragraph proofStatementParagraph = GetProofStatement(proofDefinitionList[proofDefinitionList.Count - 1] , true, false, isPositiveProof);
                paragraph.Inlines.AddRange(new List<Inline>(proofStatementParagraph.Inlines));
            }

            int counter = 0;
            foreach (string proofElement in proofDefinitionList)
            {
                Paragraph proofStatementParagraph = GetProofStatement(proofElement, false, (counter == proofDefinitionList.Count - 1), isPositiveProof);
                paragraph.Inlines.AddRange(new List<Inline>(proofStatementParagraph.Inlines));
                counter++;
            }

            return paragraph;
        }

        private Paragraph GetProofStatement(string statement, bool isStatementBeforeTeaching, bool isLastStatement, bool isPositiveProof)
        {
            Paragraph paragraph = new Paragraph();

            if (isLastStatement)
                paragraph.Inlines.Add("therefore, ");

            string[] words = statement.Split(' ');
            if (words.Length != 3)
                throw new VisibleCommentConstructionException("Expecting statements of exactly 3 words");

            Span subject = new Span();
            subject.Foreground = aiConceptColor;
            subject.Inlines.Add(words[0]);
            AddLink(subject, "InvertYouAndMePov visualize " + words[0]);

            Span verb = new Span();
            verb.Foreground = aiOperatorColor;
            verb.Inlines.Add(words[1]);
            AddLink(verb, "InvertYouAndMePov visualize " + words[1]);

            Span complement = new Span();
            complement.Foreground = aiConceptColor;
            complement.Inlines.Add(words[2]);
            AddLink(complement, "InvertYouAndMePov visualize " + words[2]);

            paragraph.Inlines.Add(subject);
            paragraph.Inlines.Add(" ");

            if ((isLastStatement || isStatementBeforeTeaching) && !isPositiveProof)
                paragraph.Inlines.Add("can't ");

            paragraph.Inlines.Add(verb);
            paragraph.Inlines.Add(" ");
            paragraph.Inlines.Add(complement);

            paragraph.Inlines.Add(new LineBreak());
            return paragraph;
        }
        #endregion

        #region Definition answers
        #region Whatis
        private Paragraph GetAnswerDefinitionWhatis(string conceptName, Dictionary<string, List<string>> definitionList, Dictionary<string, List<string>> definitionPositiveImply, Dictionary<string, List<string>> definitionNegativeImply)
        {
            Paragraph paragraph = new Paragraph();
            Span subject = new Span();
            subject.Foreground = aiConceptColor;
            subject.Inlines.Add(conceptName);
            AddLink(subject, "InvertYouAndMePov visualize " + conceptName);


            paragraph.Inlines.Add(AiName + ": ");
            paragraph.Inlines.Add(subject);
            paragraph.Inlines.Add(" ");


            foreach (KeyValuePair<string, List<string>> connectionSet in definitionList)
            {
                Paragraph whatisLine = GetWhatisLine(connectionSet.Key, connectionSet.Value);
                paragraph.Inlines.AddRange(new List<Inline>(whatisLine.Inlines));
                paragraph.Inlines.Add(" ");
            }

            if (definitionPositiveImply != null)
            {
                foreach (KeyValuePair<string, List<string>> connectionSet in definitionPositiveImply)
                {
                    Paragraph implyLine = GetImplyBlock(conceptName, connectionSet.Key, connectionSet.Value, true);
                    paragraph.Inlines.AddRange(new List<Inline>(implyLine.Inlines));
                    paragraph.Inlines.Add(new LineBreak());
                }
            }

            if (definitionNegativeImply != null)
            {
                foreach (KeyValuePair<string, List<string>> connectionSet in definitionNegativeImply)
                {
                    Paragraph implyLine = GetImplyBlock(conceptName, connectionSet.Key, connectionSet.Value, false);
                    paragraph.Inlines.AddRange(new List<Inline>(implyLine.Inlines));
                    paragraph.Inlines.Add(new LineBreak());
                }
            }

            return paragraph;
        }

        private Paragraph GetWhatisLine(string operatorName, List<string> complementConceptNameList)
        {
            Paragraph paragraph = new Paragraph();

            Span verb = new Span();
            verb.Foreground = aiOperatorColor;
            verb.Inlines.Add(operatorName);
            if (!LanguageDictionary.MetaOperatorList.Contains(operatorName.ToLower()))
                AddLink(verb, "InvertYouAndMePov visualize " + operatorName);
            paragraph.Inlines.Add(verb);
            paragraph.Inlines.Add(" ");
            

            Span complement;
            int counter = 0;
            foreach (string currentConceptName in complementConceptNameList)
            {
                complement = new Span();
                complement.Foreground = aiConceptColor;
                complement.Inlines.Add(currentConceptName);
                AddLink(complement, "InvertYouAndMePov visualize " + currentConceptName);
                paragraph.Inlines.Add(complement);

                if (counter == complementConceptNameList.Count - 2)
                    paragraph.Inlines.Add(" and ");
                else if (counter == complementConceptNameList.Count - 1)
                    paragraph.Inlines.Add("");
                else
                    paragraph.Inlines.Add(", ");
                counter++;
            }

            return paragraph;
        }
        #endregion

        #region Define
        private Paragraph GetAnswerDefinitionDefine(string conceptName, Dictionary<string, List<string>> definitionList, Dictionary<string, List<string>> definitionPositiveImply, Dictionary<string, List<string>> definitionNegativeImply)
        {
            Paragraph paragraph = new Paragraph();

            Span subject = new Span();
            subject.Foreground = aiConceptColor;
            subject.Inlines.Add(conceptName);
            AddLink(subject, "InvertYouAndMePov visualize " + conceptName);

            paragraph.Inlines.Add(AiName + ": ");
            paragraph.Inlines.Add(subject);
            paragraph.Inlines.Add(":");
            paragraph.Inlines.Add(new LineBreak());

            foreach (KeyValuePair<string, List<string>> connectionSet in definitionList)
            {
                if (LanguageDictionary.MetaOperatorList.Contains(connectionSet.Key.ToLower()))
                {
                    Paragraph sourceCodeBlock = GetSourceCodeBlock(conceptName, connectionSet.Key, connectionSet.Value);
                    paragraph.Inlines.AddRange(new List<Inline>(sourceCodeBlock.Inlines));
                }
                else
                {
                    Paragraph defineBlock = GetDefineBlock(connectionSet.Key, connectionSet.Value);
                    paragraph.Inlines.AddRange(new List<Inline>(defineBlock.Inlines));
                }
            }

            if (definitionPositiveImply != null)
            {
                foreach (KeyValuePair<string, List<string>> connectionSet in definitionPositiveImply)
                {
                    Paragraph implyLine = GetImplyBlock(conceptName, connectionSet.Key, connectionSet.Value,true);
                    paragraph.Inlines.AddRange(new List<Inline>(implyLine.Inlines));
                    paragraph.Inlines.Add(new LineBreak());
                }
            }

            if (definitionNegativeImply != null)
            {
                foreach (KeyValuePair<string, List<string>> connectionSet in definitionNegativeImply)
                {
                    Paragraph implyLine = GetImplyBlock(conceptName, connectionSet.Key, connectionSet.Value, false);
                    paragraph.Inlines.AddRange(new List<Inline>(implyLine.Inlines));
                    paragraph.Inlines.Add(new LineBreak());
                }
            }

            return paragraph;
        }

        private Paragraph GetDefineBlock(string operatorName, List<string> complementConceptNameList)
        {
            Paragraph paragraph = new Paragraph();

            Span verb = new Span();
            verb.Foreground = aiOperatorColor;
            verb.Inlines.Add(operatorName);
            AddLink(verb, "InvertYouAndMePov visualize " + operatorName);

            Span complement;

            paragraph.Inlines.Add(verb);
            paragraph.Inlines.Add(new LineBreak());
            paragraph.Inlines.Add("{");
            paragraph.Inlines.Add(new LineBreak());
            foreach (string currentConceptName in complementConceptNameList)
            {
                complement = new Span();
                complement.Foreground = aiConceptColor;
                complement.Inlines.Add(currentConceptName);
                AddLink(complement, "InvertYouAndMePov visualize " + currentConceptName);

                paragraph.Inlines.Add("    ");
                paragraph.Inlines.Add(complement);
                paragraph.Inlines.Add(new LineBreak());
            }
            paragraph.Inlines.Add("}");
            paragraph.Inlines.Add(new LineBreak());
            paragraph.Inlines.Add(new LineBreak());

            return paragraph;
        }

        private Paragraph GetSourceCodeBlock(string verbName, string metaOperatorName, List<string> farVerbList)
        {
            metaOperatorName = metaOperatorName.ToLower();

            Paragraph paragraph = new Paragraph();
            foreach (string farVerbName in farVerbList)
            {
                Paragraph codeLine = GetMetaConnectionSourceCode(verbName, metaOperatorName, farVerbName);
                paragraph.Inlines.AddRange(new List<Inline>(codeLine.Inlines));
                paragraph.Inlines.Add(new LineBreak());
            }

            return paragraph;
        }
        #endregion

        #region Imply
        private Paragraph GetImplyBlock(string verbName, string complementName, List<string> conditionList, bool isPositive)
        {
            Paragraph paragraph = new Paragraph();

            Span verb = new Span();
            verb.Foreground = aiOperatorColor;
            verb.Inlines.Add(verbName);
            AddLink(verb, "InvertYouAndMePov visualize " + verbName);

            Span complement = new Span();
            complement.Foreground = aiConceptColor;
            complement.Inlines.Add(complementName);
            AddLink(complement, "InvertYouAndMePov visualize " + complementName);

            Span comment = new Span();
            comment.Foreground = aiCommentColor;
            if (isPositive)
                comment.Inlines.Add("//positive imply connection");
            else
                comment.Inlines.Add("//negative imply connection");

            paragraph.Inlines.Add(new LineBreak());
            paragraph.Inlines.Add(comment);
            paragraph.Inlines.Add(new LineBreak());
            if (isPositive)
            {
                paragraph.Inlines.Add(verb);
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(complement);
            }
            else
            {
                paragraph.Inlines.Add(complement);
                paragraph.Inlines.Add(" ");
                paragraph.Inlines.Add(verb);
            }
            paragraph.Inlines.Add(" where");
            paragraph.Inlines.Add(new LineBreak());
            paragraph.Inlines.Add("{");
            paragraph.Inlines.Add(new LineBreak());

           
            foreach (string conditionString in conditionList)
            {
                string[] words = conditionString.Split(' ');

                paragraph.Inlines.Add("    ");

                bool isOdd = true;
                foreach (string currentWordIteration in words)
                {
                    string currentWord = currentWordIteration;

                    Span word = new Span();
                    word.Inlines.Add(currentWord);

                    if (isPositive)
                    {
                        if (isOdd)
                        {
                            word.Foreground = aiOperatorColor;
                        }
                        else
                        {
                            word.Foreground = aiConceptColor;
                        }
                    }
                    else
                    {
                        if (isOdd)
                        {
                            word.Foreground = aiConceptColor;
                        }
                        else
                        {
                            word.Foreground = aiOperatorColor;
                        }
                    }

                    if (currentWord != "or" && currentWord != "and" && currentWord != "not")
                    {
                        isOdd = !isOdd;
                        word = BlackenParantheses(word, currentWord);
                        AddLink(word, "InvertYouAndMePov visualize " + currentWord.Replace("(", "").Replace(")", ""));
                        paragraph.Inlines.Add(word);
                    }
                    else
                    {
                        isOdd = true;
                        paragraph.Inlines.Add(currentWord);
                    }
                    
                    paragraph.Inlines.Add(" ");
                }

                paragraph.Inlines.Add(new LineBreak());
            }

            paragraph.Inlines.Add("}");

            return paragraph;
        }
        #endregion

        #endregion
        #endregion
    }
}