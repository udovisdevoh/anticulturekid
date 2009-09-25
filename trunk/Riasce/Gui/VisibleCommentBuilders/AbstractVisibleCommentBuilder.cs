using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;

namespace AntiCulture.Kid
{
    abstract class AbstractVisibleCommentBuilder
    {
        #region Methods
        protected abstract Paragraph ConcreteBuild();

        public abstract Paragraph Build();

        public abstract void ResetConstructionSettings();

        protected void AddLink(Span span, string tag)
        {
            span.Tag = tag;
            span.MouseLeftButtonDown += spanClick;
            span.MouseEnter += spanMouseEnter;
            span.MouseLeave += spanMouseLeave;
            span.Cursor = Cursors.Hand;
        }

        protected Span BlackenParantheses(Span oldSpan, string content)
        {
            content = content.Trim();
            Span newSpan = new Span();

            while (content.StartsWith("("))
            {
                content = content.Substring(1);
                Span paranthese = new Span();
                paranthese.Inlines.Add("(");
                paranthese.Foreground = Brushes.Black;
                newSpan.Inlines.Add(paranthese);
            }

            newSpan.Inlines.Add(content.Replace("(", "").Replace(")", "").Trim());
            newSpan.Foreground = oldSpan.Foreground;

            while (content.EndsWith(")"))
            {
                content = content.Substring(0, content.Length - 1);
                Span paranthese = new Span();
                paranthese.Inlines.Add(")");
                paranthese.Foreground = Brushes.Black;
                newSpan.Inlines.Add(paranthese);
            }

            return newSpan;
        }
        #endregion

        #region Events
        public event EventHandler UserClickConcept;
        #endregion

        #region Handlers
        private void spanClick(object sender, RoutedEventArgs e)
        {
            Span source = (Span)(sender);
            if (UserClickConcept != null) UserClickConcept(source, e);
        }

        private void spanMouseEnter(object sender, RoutedEventArgs e)
        {
            Span source = (Span)(sender);
            source.TextDecorations.Add(TextDecorations.Underline);
        }

        private void spanMouseLeave(object sender, RoutedEventArgs e)
        {
            Span source = (Span)(sender);
            source.TextDecorations.Clear();
        }
        #endregion
    }
}
