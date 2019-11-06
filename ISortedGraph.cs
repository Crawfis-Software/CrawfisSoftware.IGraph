using System;
using System.Collections.Generic;
using System.Text;

namespace OhioState.Collections.Graph
{
    /// <summary>
    /// Interface for topologically sorted graphs
    /// </summary>
    public interface ISortedGraph
    {
        /// <summary>
        /// Indicates whether the graph is already sorted.
        /// </summary>
        bool IsSorted { get; }
        /// <summary>
        /// Sorts the graph.
        /// </summary>
        void SortTopologically();
    }
}
