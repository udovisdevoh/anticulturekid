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
    /// Interaction logic for MemoryViewer.xaml
    /// </summary>
    public partial class MemoryTreeView : TreeView
    {
        #region Fields
        private NameMapper nameMapper;

        private Memory memory;

        private HashSet<string> rememberedItemNameList;
        #endregion

        #region Constructor
        public MemoryTreeView()
        {
            this.BorderBrush = null;
        }
        #endregion
        
        #region Events
        public event EventHandler<StringEventArgs> OnClickItem;
        #endregion

        #region Public Methods
        public void SetBinding(Memory memory, NameMapper nameMapper)
        {
            this.nameMapper = nameMapper;
            this.memory = memory;

            Items.Clear();
            List<string> conceptNameList = new List<string>(nameMapper);
            conceptNameList.Sort();
            foreach (string subjectName in conceptNameList)
                Items.Add(GetNewItemFromConceptName(subjectName));

            rememberedItemNameList = new HashSet<string>(conceptNameList);
        }

        public void UpdateIfNeeded()
        {
            if (Items.Count < nameMapper.Count)
                Update();
        }
        #endregion

        #region Private Methods
        private List<MemoryTreeViewItem> AddChildVerbList(Concept subject, List<MemoryTreeViewItem> itemsSource)
        {
            int verbId, complementId;
            string verbName, complementName;
            MemoryTreeViewItem verbItem, complementItem;
            Span verbSpan, complementSpan;
            foreach (Concept verb in subject.OptimizedConnectionBranchList.Keys)
            {
                HashSet<Concept> complementList = subject.GetOptimizedConnectionBranch(verb).ComplementConceptList;
                if (complementList.Count > 0)
                {
                    verbItem = new MemoryTreeViewItem();
                    verbId = memory.GetIdFromConcept(verb);
                    verbName = nameMapper.GetConceptNames(verbId)[0];

                    verbSpan = new Span();
                    verbSpan.Inlines.Add(verbName);
                    verbItem.Header = verbSpan;
                    verbItem.Tag = verbName;

                    foreach (Concept complement in complementList)
                    {   
                        complementItem = new MemoryTreeViewItem();
                        complementId = memory.GetIdFromConcept(complement);
                        complementName = nameMapper.GetConceptNames(complementId)[0];

                        complementSpan = new Span();
                        complementSpan.Inlines.Add(complementName);
                        complementSpan.MouseDown += OnClickItemHandler;
                        complementSpan.MouseEnter += OnMouseEnterItemHandler;
                        complementSpan.MouseLeave += OnMouseLeaveItemHandler;
                        complementSpan.Tag = complementName;
                        complementSpan.Cursor = Cursors.Hand;
                        complementItem.Header = complementSpan;
                        complementItem.ItemsSource = new List<object>() { new object() };
                        complementItem.Expanding += OnExpandConcept;
                        complementItem.Tag = complementName;
                        verbItem.Items.Add(complementItem);
                    }

                    itemsSource.Add(verbItem);
                }
            }
            return itemsSource;
        }

        private void Update()
        {
            foreach (string conceptName in nameMapper)
            {
                if (!rememberedItemNameList.Contains(conceptName))
                {
                    rememberedItemNameList.Add(conceptName);
                    Items.Insert(GetInsertionIndexFor(conceptName),GetNewItemFromConceptName(conceptName));
                }
            }
        }

        private int GetInsertionIndexFor(string conceptName)
        {
            return GetInsertionIndexFor(conceptName, 0, Items.Count);
        }

        private int GetInsertionIndexFor(string conceptName, int searchStart, int searchEnd)
        {
            if (searchStart == searchEnd)
                return searchStart;

            int pivotIndex = (searchStart + searchEnd) / 2;
            MemoryTreeViewItem pivotItem = (MemoryTreeViewItem)Items[pivotIndex];
            string pivotName = (string)pivotItem.Tag;

            if (searchStart + 1 == searchEnd)
            {
                MemoryTreeViewItem currentItemAtIndex = (MemoryTreeViewItem)Items[searchStart];
                string currentItemNameAtIndex = (string)currentItemAtIndex.Tag;

                if (conceptName.CompareTo(currentItemNameAtIndex) < 0)
                    return searchStart;
                else
                    return searchEnd;
            }
            else if (conceptName.CompareTo(pivotName) <= 0)
            {
                return GetInsertionIndexFor(conceptName, searchStart, pivotIndex);
            }
            else
            {
                return GetInsertionIndexFor(conceptName, pivotIndex, searchEnd);
            }
        }

        private object GetNewItemFromConceptName(string name)
        {
            MemoryTreeViewItem item = new MemoryTreeViewItem();
            Span span = new Span();
            span.Cursor = Cursors.Hand;
            span.Inlines.Add(name);
            span.MouseDown += OnClickItemHandler;
            span.MouseEnter += OnMouseEnterItemHandler;
            span.MouseLeave += OnMouseLeaveItemHandler;
            span.Tag = name;
            item.Header = span;
            item.ItemsSource = new List<object>() { new object() };
            item.Expanding += OnExpandConcept;
            item.Tag = name;
            return item;
        }
        #endregion

        #region Handlers
        private void OnExpandConcept(object sender, RoutedEventArgs e)
        {
            MemoryTreeViewItem subjectItem = (MemoryTreeViewItem)sender;
            string subjectName = (string)subjectItem.Tag;
            int subjectId = nameMapper.GetOrCreateConceptId(nameMapper.InvertYouAndMePov(subjectName));
            Concept subject = memory.GetOrCreateConcept(subjectId);

            List<MemoryTreeViewItem> itemsSource = new List<MemoryTreeViewItem>();
            AddChildVerbList(subject,itemsSource);
            if (itemsSource.Count < 1)
            {
                MemoryTreeViewItem emptyItem = new MemoryTreeViewItem();
                emptyItem.Header = "Ø";
                itemsSource.Add(emptyItem);
            }

            subjectItem.ItemsSource = itemsSource;
        }

        private void OnClickItemHandler(object sender, RoutedEventArgs e)
        {
            Span subjectSpan = (Span)sender;
            string subjectName = (string)(subjectSpan.Tag);
            if (OnClickItem != null) OnClickItem(sender, new StringEventArgs(subjectName));
        }

        private void OnMouseEnterItemHandler(object sender, RoutedEventArgs e)
        {
            Span subjectSpan = (Span)sender;
            subjectSpan.TextDecorations = TextDecorations.Underline;
        }

        private void OnMouseLeaveItemHandler(object sender, RoutedEventArgs e)
        {
            Span subjectSpan = (Span)sender;
            subjectSpan.TextDecorations = null;
        }
        #endregion
    }
}
