using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    abstract class AbstractVisualizer
    {
        /// <summary>
        /// Show a concept
        /// </summary>
        /// <param name="conceptName">concept name</param>
        /// <param name="conceptDefinition">concept definition</param>
        public abstract void ShowConcept(string conceptName, Dictionary<string, List<string>> conceptDefinition);

        /// <summary>
        /// Show a concept
        /// </summary>
        /// <param name="conceptName">concept name</param>
        /// <param name="conceptDefinition">concept definition</param>
        /// <param name="connectionsNoWhy">connenctions for which no why link will be shown (can be null)</param>
        public abstract void ShowConcept(string conceptName, Dictionary<string, List<string>> conceptDefinition, Dictionary<string, List<string>> connectionsNoWhy);

        /// <summary>
        /// Get Visualizer Viewer
        /// </summary>
        public abstract VisualizerViewer VisualizerViewer
        {
            get;
        }
    }
}
