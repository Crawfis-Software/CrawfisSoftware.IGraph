using System.Collections.Generic;

namespace CrawfisSoftware.Collections.Graph
{
    /// <summary>
    /// IEdge provides a standard interface to specify an edge and any data associated
    /// with an edge within a graph.
    /// </summary>
    /// <typeparam name="E">The type of the data on an edge.</typeparam>
    public interface IIndexedEdge<E>
    {
        /// <summary>
        /// The index of the node that the edge or arc emanates from.
        /// </summary>
        int From { get; }
        /// <summary>
        /// The index of the node the edge or arc terminates at.
        /// </summary>
        int To { get; }
        /// <summary>
        /// The label on the edge.
        /// </summary>
        E Value { get; }
    }

    /// <summary>
    /// Interface for a (static) index-based graph.
    /// </summary>
    /// <remarks>Node indices are used and returned whenever possible. Use
    /// the <seealso cref="GetNodeLabel"/> method to access the node's label.</remarks>
    /// <typeparam name="N">The type of the nodes in the graph.</typeparam>
    /// <typeparam name="E">The type of the data on an edge.</typeparam>
    public interface IIndexedGraph<N, E>
    {
        /// <summary>
        /// Get the number of edges in the graph.
        /// </summary>
        int NumberOfEdges { get; }
        /// <summary>
        /// Get the number of nodes in the graph.
        /// </summary>
        int NumberOfNodes { get; }
        /// <summary>
        /// Iterator for the node indices in the graph.
        /// </summary>
        IEnumerable<int> Nodes { get; }
        /// <summary>
        /// Iterator for the edges in the graph, yielding IIndexedEdge's
        /// </summary>
        IEnumerable<IIndexedEdge<E>> Edges { get; }
        /// <summary>
        /// Returns the node label associated with the node at index <paramref name="nodeIndex"/>.
        /// </summary>
        /// <param name="nodeIndex">The index of the node.</param>
        /// <returns>The label associated with the node.</returns>
        N GetNodeLabel(int nodeIndex);
        /// <summary>
        /// Iterator for the children or neighbors of the specified node.
        /// </summary>
        /// <param name="nodeIndex">The index of the node.</param>
        /// <returns>An enumerator of node indices.</returns>
        IEnumerable<int> Neighbors(int nodeIndex);
        /// <summary>
        /// Iterator over the emanating edges from a node.
        /// </summary>
        /// <param name="nodeIndex">The index of the node.</param>
        /// <returns>An enumerator of edges.</returns>
        IEnumerable<IIndexedEdge<E>> OutEdges(int nodeIndex);

        /// <summary>
        /// Iterator over the parents or immediate ancestors of a node.
        /// </summary>
        /// <remarks>May not be supported by all graphs.</remarks>
        /// <param name="nodeIndex">The index of the node.</param>
        /// <returns>An enumerator of node indices.</returns>
        IEnumerable<int> Parents(int nodeIndex);
        /// <summary>
        /// Iterator over the in-coming edges of a node.
        /// </summary>
        /// <remarks>May not be supported by all graphs.</remarks>
        /// <param name="nodeIndex">The index of the node.</param>
        /// <returns>An enumerator of node indices.</returns>
        IEnumerable<IIndexedEdge<E>> InEdges(int nodeIndex);

        /// <summary>
        /// Tests whether an edge exists between two nodes.
        /// </summary>
        /// <param name="fromNode">Index of the node that the edge emanates from.</param>
        /// <param name="toNode">Index of the node that the edge terminates at.</param>
        /// <returns>True if the edge exists in the graph. False otherwise.</returns>
        bool ContainsEdge(int fromNode, int toNode);
        /// <summary>
        /// Gets the label on an edge.
        /// </summary>
        /// <param name="fromNode">Index of the node that the edge emanates from.</param>
        /// <param name="toNode">Index of the node that the edge terminates at.</param>
        /// <returns>The label on the edge.</returns>
        E GetEdgeLabel(int fromNode, int toNode);
        /// <summary>
        /// Exception safe routine to get the label on an edge.
        /// </summary>
        /// <param name="fromNode">Index of the node that the edge emanates from.</param>
        /// <param name="toNode">Index of the node that the edge terminates at.</param>
        /// <param name="edge">The resulting edge label if the method was successful. A default
        /// value for the type if the edge could not be found.</param>
        /// <returns>True if the edge was found. False otherwise.</returns>
        bool TryGetEdgeLabel(int fromNode, int toNode, out E edge);
    }
}
