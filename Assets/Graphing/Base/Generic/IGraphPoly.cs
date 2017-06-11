namespace Graphing.Generic
{
    /// <summary>
    /// A polygon within a graph. Does not have to be closed, but the polygon's edge must be the start edge (previous is null) and the end edge must have a twin (to include the last node).
    /// </summary>
    /// <typeparam name="PolyT">This</typeparam>
    /// <typeparam name="EdgeT">The Typesafe Edge</typeparam>
    /// <typeparam name="NodeT">The Typesafe Node</typeparam>
    public interface IGraphPoly<PolyT, EdgeT, NodeT> : IGraphPoly
            where PolyT : class, IGraphPoly<PolyT, EdgeT, NodeT>
            where EdgeT : class, IGraphEdge<PolyT, EdgeT, NodeT>
            where NodeT : class, IGraphNode<PolyT, EdgeT, NodeT>
    {
        /// <summary>
        /// An interior Half edge of the polygon.
        /// This should be the first Half edge if the polygon is not closed.
        /// </summary>
        new EdgeT Edge
        {
            get;
        }
    }
}

