namespace CrawfisSoftware.Collections.Graph
{
    /// <summary>
    /// Graph interface for graphs with finite size.
    /// </summary>
    /// <typeparam name="N">The type associated at each node. Called a node or node label</typeparam>
    /// <typeparam name="E">The type associated at each edge. Also called the edge label.</typeparam>
    /// <seealso cref="IGraph{N, E}"/>
    public interface IFiniteGraph<N, E> : IGraph<N, E>
    {
        /// <summary>
        /// Get the number of edges in the graph.
        /// </summary>
        int NumberOfEdges { get; }
        /// <summary>
        /// Get the number of nodes in the graph.
        /// </summary>
        int NumberOfNodes { get; }
    }
}
