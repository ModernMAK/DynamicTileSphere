namespace Graphing
{
    /// <summary>
    /// A polygon within a graph. Does not have to be closed, but the polygon's edge must be the start edge (previous is null) and the end edge must have a twin (to include the last node).
    /// </summary>
    public interface IGraphPoly
    {
        /// <summary>
        /// An interior Half edge of the polygon.
        /// This should be the first Half edge if the polygon is not closed.
        /// </summary>
        IGraphEdge Edge
        {
            get;
        }
    }
    
}