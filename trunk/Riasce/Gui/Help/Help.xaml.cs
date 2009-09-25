using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : UserControl
    {
        #region Fields
        private Brush lineColor = Brushes.Black;

        private Paragraph paragraphVerbNameList = new Paragraph();
        #endregion

        #region Constructor
        public Help()
        {
            InitializeComponent();
            AddContent();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add all the content to the help
        /// </summary>
        private void AddContent()
        {
            AddTitle("Help");

            AddOperatorTopic("Regular statement",
                             "A regular statement is used to alter connections between concepts. It is important to understand that the concepts and operators are created by the user.",
                             new List<string>() { "[concept1] [operator] [concept2]", "[concept1] not [operator] [concept2]" },
                             "moon orbit earth",
                             "alright, moon now orbit earth");

            AddOperatorTopic("If statements",
                             "If statement are used to add or remove metaConnections or implyConnections",
                             new List<string>() { "if [condition] then [action]", "not if [condition] then [action]" },
                             "if pine isa tree and tree madeof wood then pine madeof wood",
                             "alright, madeof now muct isa");

            AddTitle("Language operator (custom or instinctive verbs)");

            AddVerbNameListParagraph();

            AddTitle("Pre-defined operators");

            AddOperatorTopic("Aliasof",
                     "Used to map more than one name to the same concept",
                     "[new_name_for_concept] aliasof [existing_name_for_concept]",
                     new List<string>() { "round aliasof circle", "whatis round" },
                     "circle isa shape");

            AddOperatorTopic("Analogize",
                     "Ask the AI to make an analogy on something",
                     new List<string>() { "analogize", "analogize [concept]", "analogize [concept] [operator]", "analogize [concept] [operator] [concept]" },
                     "analogize apple partof applefruit",
                     "banana partof bananafruit");

            AddOperatorTopic("Ask",
                     "Tell the AI to ask something about a random concept",
                     "ask",
                     "ask",
                     "plant madeof what?");

            AddOperatorTopic("Askabout",
                     "Tell the AI to ask something about a random concept",
                     "askabout [concept_name]",
                     "askabout pig",
                     "pig eat what?");

            AddOperatorTopic("Define",
                     "Define can be used to view a operator or concept's full recursive definition.",
                     "define [concept_or_operatorName]",
                     "define pine",
                     new List<string>() { "isa", "{", "    tree", "    plant", "    lifeform", "}", "madeof", "{", "    wood", "    water", "}" });

            AddOperatorTopic("Exit",
                     "Exit the program",
                     "exit",
                     "exit");

            AddOperatorTopic("Imply",
                     "Similar to AiSql update but will have a perpetual effect",
                     "imply [changes] where [conditions]",
                     new List<string>() { "imply isa animal madeof water where isa lifeform and not madeof wood or (isa tree and isa decoration)", "if alpha isa lifeform and alpha not madeof wood or (alpha isa tree and alpha isa decoration) then alpha isa animal madeof water" },
                     "Imply connection created");

            AddOperatorTopic("LinguisThink",
                     "Tells the AI to try to make a semantically based deduction about a random concept",
                     "linguisthink",
                     "linguisthink",
                     "maybe cat isa mammal...");

            AddOperatorTopic("LinguisThinkAbout",
                     "Tells the AI to try to make a semantically based deduction about a concept",
                     "linguisthinkabout [concept]",
                     "linguisthinkabout apple",
                     "maybe apple isa fruit...");

            AddOperatorTopic("Metathink",
                     "Tells the AI to try to make a logical deduction about a random operator",
                     "metathink",
                     "metathink",
                     "maybe isa muct madeof...");

            AddOperatorTopic("Metathinkabout",
                     "Tells the AI to try to make a logical deduction about an operator",
                     "thinkabout [operator]",
                     "thinkabout make",
                     "make muct isa...");

            AddOperatorTopic("Neologize",
                    "Ask the AI to try to create a logical layer to generalize common properties",
                    "neologize [operator]",
                    "neologize isa",
                    "what isa general name for isa plant madeof wood someare pine, willow and palmtree?");

            AddOperatorTopic("PhonoThink",
                     "Tells the AI to try to make a phonologically based deduction about a random concept",
                     "phonothink",
                     "phonothink",
                     "maybe cat isa mammal...");

            AddOperatorTopic("PhonoThinkAbout",
                     "Tells the AI to try to make a phonologically based deduction about a concept",
                     "phonothinkabout [concept]",
                     "phonothinkabout apple",
                     "maybe apple isa fruit...");

            AddOperatorTopic("Rename",
                     "Used to rename a concept from one name to another",
                     "rename [old_name] [new_name]",
                     "rename circle round");

            AddOperatorTopic("Select",
                     "Perform a AiSql selection",
                     "select where [conditions]",
                     "select where isa lifeform and not madeof wood or (isa tree and isa decoration)",
                     "[list of concepts having connections that conform to conditions]");

            AddOperatorTopic("Start_Psychosis",
                     "Tell the AI to validate theories that have probability >= [probability]",
                     "Start_Psychosis [probability from 0 to 100]",
                     "Start_Psychosis 97",
                     "alright, fish now madeof bone");

            AddOperatorTopic("StatCross",
                     "Make cross statistics on two connection type",
                     new List<string>() { "statcross", "statcross [denominator_verb] [denominator_complement]", "statcross [denominator_verb] [denominator_complement] [numerator_verb] [numerator_complement]" },
                     "statcross isa plant madeof wood",
                     "from isa plant, 80% madeof wood");

            AddOperatorTopic("Talk",
                     "Tells the AI to talk about a random concept",
                     "talk",
                     "talk",
                     "(could output something similar to output from teach, think, ask, analogize or statcross)");

            AddOperatorTopic("Talkabout",
                     "Tells the AI to talk about a concept",
                     "talkabout [concept_name]",
                     "talkabout cow",
                     "(could output something similar to teachabout cow, thinkabout cow, askabout cow or analogize cow)");

            AddOperatorTopic("Teach",
                     "Tells the AI to teach something about a random concept",
                     "teach",
                     "teach",
                     "[...]water madeof oxygen, therefore water madeof atom");

            AddOperatorTopic("Teachabout",
                     "Tells the AI to teach something about concept",
                     "teachabout [concept_name]",
                     "teachabout tree",
                     "[...]tree madeof wood, therefore tree madeof material");

            AddOperatorTopic("Think",
                     "Tells the AI to try to make a logical deduction about a random concept",
                     "think",
                     "think",
                     "maybe cat isa mammal...");

            AddOperatorTopic("Thinkabout",
                     "Tells the AI to try to make a logical deduction about a concept",
                     "thinkabout [concept]",
                     "thinkabout apple",
                     "maybe apple isa fruit...");

            AddOperatorTopic("Update",
                     "Perform a AiSql update",
                     "update [changes] where [conditions]",
                     "update isa animal madeof water where isa lifeform and not madeof wood or (isa tree and isa decoration)",
                     "alright, cat now isa animal, alright, dog now madeof water");

            AddOperatorTopic("Visualize",
                    "Visualize can be used to view a operator or concept's visual representation.",
                    "visualize [concept_or_operatorName]",
                    "visualize pine",
                    "[visualization window]");

            AddOperatorTopic("Whatis",
                     "Whatis can be used to ask the AI about immediate connections from a concept as opposed to \"define\" which returns a recursive definition about the concept.",
                     "whatis [concept_or_operator_name]",
                     "whatis tree",
                     "tree isa plant madeof wood");

            AddOperatorTopic("Which",
                     "Which is used to create statement involving complement operators",
                     "[subject] [verb] [complement] (which [otherVerb] [otherComplement])",
                     "tree isa plant (which isa lifeform (which madeof water (which isa liquid madeof oxygen (which isa gas) and hydrogen (which isa gas))))",
                     "alright, tree now isa plant [...] alright, oxygen now isa gas [...]etc");

            AddOperatorTopic("Why",
                     "Whatis can be used to ask the AI about immediate connections from a concept as opposed to \"define\" which returns a recursive definition about the concept.",
                     new List<string>() { "why [concept] [operator] [concept]", "why [concept] not [operator] [concept]" },
                     "whatis tree",
                     "tree isa plant madeof wood");

            AddTitle("MetaOperators");

            AddOperatorTopic("Direct_implication",
                             "Used to force an operator to imply another",
                             new List<string>(){"[operator1] direct_implication [operator2]","if [concept1] [operator1] [concept2] then [concept1] [operator2] [concept2]"},
                             "if mother make child then mother love child",
                             "alright, make now direct_implication love");

            AddOperatorTopic("Inverse_implication",
                             "Used to force an operator to imply another",
                             new List<string>() { "[operator1] inverse_implication [operator2]", "if [concept1] [operator1] [concept2] then [concept2] [operator2] [concept1]" },
                             "if mother make child then child love mother",
                             "alright, make now direct_implication love");

            AddOperatorTopic("Inverse_of",
                             "Design inverse operators",
                             new List<string>() { "[operator1] inverse_of [operator2]", "[concept1] [operator1] [concept2] = [concept2] [operator2] [concept1]" },
                             "pine madeof wood = wood partof pine",
                             "alright, madeof now inverse_of partof");

            AddOperatorTopic("Permutable_side",
                             "Make an operator do the same thing regardless the statement's side",
                             new List<string>() { "[operator] permutable_side [operator]", "[concept1] [operator1] [concept2] = [concept2] [operator1] [concept1]" },
                             "ice contradict fire = fire contradict ice",
                             "alright, contradict now permutable_side contradict");

            AddOperatorTopic("Cant",
                             "Make two operators mutually exclusive",
                             new List<string>() { "[operator1] cant [operator2]", "if [concept1] [operator1] [concept2] then [concept1] cant [operator2] [concept2]" },
                             "if tree contradict plant then tree cant isa plant",
                             "alright, contradict now cant isa");

            AddOperatorTopic("Unlikely",
                             "When the AI searches for new theories, discard theories implied by unlikely operators",
                             new List<string>() { "[operator1] unlikely [operator2]", "if [concept1] [operator1] [concept2] then [concept1] unlikely [operator2] [concept2]" },
                             "if tree isa plant then tree unlikely madeof plant",
                             "alright, isa now unlikely madeof");

            AddOperatorTopic("Conservative_Thinking",
                             "Prevent the AI to make theories about connections using verbs for which no connection with that verb exist yet for subject and complement",
                             "[operator1] conservative_thinking [operator2]",
                             "someare conservative_thinking someare",
                             "alright, someare now conservative_thinking someare");

            AddOperatorTopic("Muct",
                             "Allow [operator1] to muct connections from [operator2]",
                             new List<string>() { "[operator1] muct [operator2]", "if [concept1] [operator2] [concept2] and [concept2] [operator1] [concept3] then [concept1] [operator1] [concept3]" },
                             "if pine isa tree and tree madeof wood then pine madeof wood",
                             "alright, madeof now muct isa");

            AddOperatorTopic("Liffid",
                             "Allow [operator1] to liffid connections from [operator2]",
                             new List<string>() { "[operator1] liffid [operator2]", "if [concept1] [operator1] [concept2] and [concept2] [operator2] [concept3] then [concept1] [operator1] [concept3]" },
                             "if tree madeof wood and wood isa material then tree madeof material",
                             "alright, madeof now liffid isa");

            AddOperatorTopic("Sublar",
                             "Allow [operator1] to sublar connections from [operator2]",
                             new List<string>() { "[operator1] sublar [operator2]", "if [concept2] [operator2] [concept1] and [concept2] [operator1] [concept3] then [concept1] [operator1] [concept3]" },
                             "if tree someare pine and tree madeof wood then pine madeof wood",
                             "alright, madeof now sublar someare");

            AddOperatorTopic("Consics",
                             "Allow [operator1] to consics connections from [operator2]",
                             new List<string>() { "[operator1] consics [operator2]", "if [concept1] [operator1] [concept2] and [concept3] [operator2] [concept2] then [concept1] [operator1] [concept3]" },
                             "if tree madeof wood and material someare wood then tree madeof material",
                             "alright, madeof now consics someare");  
        }

        private void AddVerbNameListParagraph()
        {
            richTextBoxHelp.Document.Blocks.Add(paragraphVerbNameList);
        }

        /// <summary>
        /// Add a topic to the help
        /// </summary>
        /// <param name="title">generally a command's name</param>
        /// <param name="shortDefinition">short definition on top</param>
        /// <param name="syntax">syntax</param>
        /// <param name="exampleInput">example (input)</param>
        /// <param name="exampleOutput">example (output)</param>
        private void AddOperatorTopic(string title, string shortDefinition, string syntax, string exampleInput, string exampleOutput)
        {
            richTextBoxHelp.Document.Blocks.Add(GetTitleParagraph(title));
            richTextBoxHelp.Document.Blocks.Add(GetShortDefinitionParagraph(shortDefinition));
            richTextBoxHelp.Document.Blocks.Add(GetSyntaxParagraph(syntax));
            richTextBoxHelp.Document.Blocks.Add(GetExampleParagraph(exampleInput, exampleOutput));
        }

        private void AddOperatorTopic(string title, string shortDefinition, string syntax, string exampleInput)
        {
            richTextBoxHelp.Document.Blocks.Add(GetTitleParagraph(title));
            richTextBoxHelp.Document.Blocks.Add(GetShortDefinitionParagraph(shortDefinition));
            richTextBoxHelp.Document.Blocks.Add(GetSyntaxParagraph(syntax));
            richTextBoxHelp.Document.Blocks.Add(GetExampleParagraph(exampleInput));
        }

        private void AddOperatorTopic(string title, string shortDefinition, List<string> syntaxLines, string exampleInput, string exampleOutput)
        {
            richTextBoxHelp.Document.Blocks.Add(GetTitleParagraph(title));
            richTextBoxHelp.Document.Blocks.Add(GetShortDefinitionParagraph(shortDefinition));
            richTextBoxHelp.Document.Blocks.Add(GetSyntaxParagraph(syntaxLines));
            richTextBoxHelp.Document.Blocks.Add(GetExampleParagraph(exampleInput, exampleOutput));
        }

        private void AddOperatorTopic(string title, string shortDefinition, string syntax, string exampleInput, List<string> exampleOutputList)
        {
            richTextBoxHelp.Document.Blocks.Add(GetTitleParagraph(title));
            richTextBoxHelp.Document.Blocks.Add(GetShortDefinitionParagraph(shortDefinition));
            richTextBoxHelp.Document.Blocks.Add(GetSyntaxParagraph(syntax));
            richTextBoxHelp.Document.Blocks.Add(GetExampleParagraph(exampleInput, exampleOutputList));
        }

        private void AddOperatorTopic(string title, string shortDefinition, string syntax, List<string> exampleInputList, string exampleOutput)
        {
            richTextBoxHelp.Document.Blocks.Add(GetTitleParagraph(title));
            richTextBoxHelp.Document.Blocks.Add(GetShortDefinitionParagraph(shortDefinition));
            richTextBoxHelp.Document.Blocks.Add(GetSyntaxParagraph(syntax));
            richTextBoxHelp.Document.Blocks.Add(GetExampleParagraph(exampleInputList, exampleOutput));
        }

        private Paragraph GetTitleParagraph(string title)
        {
            Paragraph titleParagraph = new Paragraph();
            titleParagraph.Inlines.Add(new LineBreak());
            Line line = new Line();
            line.Stroke = lineColor;
            line.StrokeThickness = 2;
            line.X1 = 0;
            line.X2 = 300;
            titleParagraph.Inlines.Add(line);
            titleParagraph.Inlines.Add(new LineBreak());
            Span titleSpan = new Span();
            titleSpan.FontSize = 17;
            titleSpan.Inlines.Add(title);
            titleParagraph.Inlines.Add(titleSpan);
            return titleParagraph;
        }

        private Block GetShortDefinitionParagraph(string shortDefinition)
        {
            Paragraph shortDefinitionParagraph = new Paragraph();
            Span shortDefinitionSpan = new Span();
            shortDefinitionSpan.Inlines.Add(shortDefinition);
            shortDefinitionParagraph.Inlines.Add("Definition: ");
            shortDefinitionParagraph.Inlines.Add(shortDefinitionSpan);

            return shortDefinitionParagraph;
        }

        private Block GetSyntaxParagraph(string syntax)
        {
            Paragraph syntaxParagraph = new Paragraph();
            syntaxParagraph.Inlines.Add("Syntax: ");
            syntaxParagraph.Inlines.Add(syntax);

            return syntaxParagraph;
        }

        private Block GetSyntaxParagraph(List<string> syntaxLines)
        {
            Paragraph syntaxParagraph = new Paragraph();
            syntaxParagraph.Inlines.Add("Syntax: ");
            int counter = 0;
            foreach (string currentLine in syntaxLines)
            {
                syntaxParagraph.Inlines.Add(new LineBreak());
                if (counter != 0)
                {
                    syntaxParagraph.Inlines.Add("or");
                    syntaxParagraph.Inlines.Add(new LineBreak());
                }
                syntaxParagraph.Inlines.Add(currentLine);
                counter++;
            }
            return syntaxParagraph;
        }

        private Block GetExampleParagraph(string exampleInput, string exampleOutput)
        {
            Paragraph exampleParagraph = new Paragraph();
            Italic inputTitle = new Italic();
            inputTitle.Inlines.Add("Considering the following statement example:");
            Bold visibleInput = new Bold();
            visibleInput.Inlines.Add(exampleInput);
            Italic willOutput = new Italic();
            willOutput.Inlines.Add("the AI answer something similar to:");
            Bold visibleOutput = new Bold();
            visibleOutput.Inlines.Add(exampleOutput);

            exampleParagraph.Inlines.Add(inputTitle);
            exampleParagraph.Inlines.Add(new LineBreak());
            exampleParagraph.Inlines.Add(new LineBreak());
            exampleParagraph.Inlines.Add(visibleInput);
            exampleParagraph.Inlines.Add(new LineBreak());
            exampleParagraph.Inlines.Add(new LineBreak());
            exampleParagraph.Inlines.Add(willOutput);
            exampleParagraph.Inlines.Add(new LineBreak());
            exampleParagraph.Inlines.Add(new LineBreak());
            exampleParagraph.Inlines.Add(visibleOutput);

            return exampleParagraph;
        }

        private Block GetExampleParagraph(string exampleInput, List<string> exampleOutputList)
        {
            Paragraph exampleParagraph = new Paragraph();
            Italic inputTitle = new Italic();
            inputTitle.Inlines.Add("Considering the following statement example:");
            Bold visibleInput = new Bold();
            visibleInput.Inlines.Add(exampleInput);
            Italic willOutput = new Italic();
            willOutput.Inlines.Add("the AI answer something similar to:");


            exampleParagraph.Inlines.Add(inputTitle);
            exampleParagraph.Inlines.Add(new LineBreak());
            exampleParagraph.Inlines.Add(new LineBreak());
            exampleParagraph.Inlines.Add(visibleInput);
            exampleParagraph.Inlines.Add(new LineBreak());
            exampleParagraph.Inlines.Add(new LineBreak());
            exampleParagraph.Inlines.Add(willOutput);
            exampleParagraph.Inlines.Add(new LineBreak());
            exampleParagraph.Inlines.Add(new LineBreak());
            foreach (string line in exampleOutputList)
            {
                exampleParagraph.Inlines.Add(line);
                exampleParagraph.Inlines.Add(new LineBreak());
            }

            return exampleParagraph;
        }

        private Block GetExampleParagraph(string exampleInput)
        {
            Paragraph exampleParagraph = new Paragraph();
            Italic inputTitle = new Italic();
            inputTitle.Inlines.Add("Example:");
            Bold visibleInput = new Bold();
            visibleInput.Inlines.Add(exampleInput);
            Italic willOutput = new Italic();
            Bold visibleOutput = new Bold();

            exampleParagraph.Inlines.Add(inputTitle);
            exampleParagraph.Inlines.Add(new LineBreak());
            exampleParagraph.Inlines.Add(new LineBreak());
            exampleParagraph.Inlines.Add(visibleInput);

            return exampleParagraph;
        }

        private Block GetExampleParagraph(List<string> exampleInputList, string exampleOutput)
        {
            Paragraph exampleParagraph = new Paragraph();
            Italic willOutput = new Italic();
            willOutput.Inlines.Add("the AI answer something similar to:");
            Bold visibleOutput = new Bold();
            visibleOutput.Inlines.Add(exampleOutput);

            foreach (string exampleInput in exampleInputList)
            {
                exampleParagraph.Inlines.Add(exampleInput);
                exampleParagraph.Inlines.Add(new LineBreak());
            }
            exampleParagraph.Inlines.Add(new LineBreak());
            exampleParagraph.Inlines.Add(willOutput);
            exampleParagraph.Inlines.Add(new LineBreak());
            exampleParagraph.Inlines.Add(new LineBreak());
            exampleParagraph.Inlines.Add(visibleOutput);

            return exampleParagraph;
        }

        private void AddTitle(string title)
        {
            Paragraph paragraph = new Paragraph();
            Line line = new Line();
            line.Stroke = lineColor;
            line.StrokeThickness = 2;
            line.X1 = 0;
            line.X2 = 700;
            paragraph.Inlines.Add(line);
            paragraph.Inlines.Add(new LineBreak());
            paragraph.Inlines.Add(title);
            paragraph.FontSize = 25;
            paragraph.Margin = new Thickness(0);
            richTextBoxHelp.Document.Blocks.Add(paragraph);
        }
        #endregion

        #region Properties
        public List<string> VerbNameList
        {
            set
            {
                paragraphVerbNameList.Inlines.Clear();
                foreach (string verbName in value)
                {
                    paragraphVerbNameList.Inlines.Add(verbName);
                    paragraphVerbNameList.Inlines.Add(new LineBreak());
                }
            }
        }
        #endregion
    }
}
