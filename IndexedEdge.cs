namespace CrawfisSoftware.Collections.Graph
{
    /// <summary>
    /// Struct to hold a graph edge with indices to both nodes and an edge value.
    /// </summary>
    /// <typeparam name="E">The type of the edge value.</typeparam>
    public struct IndexedEdge<E> : IIndexedEdge<E>
    {
        int _from, _to;
        E _edgeLabel;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="from">Node index of originating node.</param>
        /// <param name="to">Node index where following the edge ends up</param>
        /// <param name="edgeLabel">Data or edge weight associated with this edge.</param>
        public IndexedEdge(int from, int to, E edgeLabel)
        {
            _from = from;
            _to = to;
            _edgeLabel = edgeLabel;
        }


        #region IIndexedEdge<E> Members
        /// <inheritdoc/>
        public int From
        {
            get { return _from; }
        }

        /// <inheritdoc/>
        public int To
        {
            get { return _to; }
        }

        /// <inheritdoc/>
        public E Value
        {
            get { return _edgeLabel; }
        }

        #endregion
    }
}
