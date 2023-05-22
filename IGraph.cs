using System;
using System.Collections.Generic;
using System.Security.Permissions;

[assembly: CLSCompliant(true)]
namespace CrawfisSoftware.Collections.Graph
{
    /// <summary>
    /// IEdge provides a standard interface to specify an edge and any data associated
    /// with an edge within a graph.
    /// </summary>
    /// <typeparam name="N">The type of the nodes in the graph.</typeparam>
    /// <typeparam name="E">The type of the data on an edge.</typeparam>
    public interface IEdge<N, E>
    {
        /// <summary>
        /// Get the Node label that this edge emanates from.
        /// </summary>
        N From { get; }
        /// <summary>
        /// Get the Node label that this edge terminates at.
        /// </summary>
        N To { get; }
        /// <summary>
        /// Get the edge label for this edge.
        /// </summary>
        E Value { get; }
    }
    /// <summary>
    /// The Graph interface
    /// </summary>
    /// <typeparam name="N">The type associated at each node. Called a node or node label. These must be unique.</typeparam>
    /// <typeparam name="E">The type associated at each edge. Also called the edge label.</typeparam>   
    public interface IGraph<N, E>
    {
        /// <summary>
        /// Iterator for the nodes in the graoh.
        /// </summary>
        IEnumerable<N> Nodes { get; }
        /// <summary>
        /// Iterator for the children or neighbors of the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An enumerator of nodes.</returns>
        IEnumerable<N> Neighbors(N node);
        /// <summary>
        /// Iterator over the parents or immediate ancestors of a node.
        /// </summary>
        /// <remarks>May not be supported by all graphs.</remarks>
        /// <param name="node">The node.</param>
        /// <returns>An enumerator of nodes.</returns>
        IEnumerable<N> Parents(N node);
        /// <summary>
        /// Iterator over the emanating edges from a node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An enumerator of nodes.</returns>
        IEnumerable<IEdge<N, E>> OutEdges(N node);
        /// <summary>
        /// Iterator over the in-coming edges of a node.
        /// </summary>
        /// <remarks>May not be supported by all graphs.</remarks>
        /// <param name="node">The node.</param>
        /// <returns>An enumerator of edges.</returns>
        IEnumerable<IEdge<N, E>> InEdges(N node);

        /// <summary>
        /// Iterator for the edges in the graph, yielding IEdge's
        /// </summary>
        IEnumerable<IEdge<N, E>> Edges { get; }
        /// <summary>
        /// Tests whether an edge exists between two nodes.
        /// </summary>
        /// <param name="fromNode">The node that the edge emanates from.</param>
        /// <param name="toNode">The node that the edge terminates at.</param>
        /// <returns>True if the edge exists in the graph. False otherwise.</returns>
        bool ContainsEdge(in N fromNode, in N toNode);
        /// <summary>
        /// Gets the label on an edge.
        /// </summary>
        /// <param name="fromNode">The node that the edge emanates from.</param>
        /// <param name="toNode">The node that the edge terminates at.</param>
        /// <returns>The edge.</returns>
        E GetEdgeLabel(in N fromNode, in N toNode);
        /// <summary>
        /// Exception safe routine to get the label on an edge.
        /// </summary>
        /// <param name="fromNode">The node that the edge emanates from.</param>
        /// <param name="toNode">The node that the edge terminates at.</param>
        /// <param name="edge">The resulting edge if the method was successful. A default
        /// value for the type if the edge could not be found.</param>
        /// <returns>True if the edge was found. False otherwise.</returns>
        bool TryGetEdge(in N fromNode, in N toNode, out E edge);
    }
}
