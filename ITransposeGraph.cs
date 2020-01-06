using System;

namespace CrawfisSoftware.Collections.Graph
{
    /// <summary>
    /// Interface used to allow the transpose of a directed graph.
    /// The transpose of a graph has all of the edge directions reversed.
    /// </summary>
    /// <typeparam name="N">The type of the nodes in the graph.</typeparam>
    /// <typeparam name="E">The type of the data on an edge.</typeparam>
    public interface ITransposeGraph<N, E>
    {
        /// <summary>
        /// Compute the transpose of the graph and return it.
        /// </summary>
        /// <returns>An IGraph with the same type parameters.</returns>
        IGraph<N, E> Transpose();
    }
}
