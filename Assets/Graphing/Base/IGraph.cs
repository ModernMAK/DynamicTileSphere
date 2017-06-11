using System.Collections.Generic;

namespace Graphing
{
    public interface IGraph
    {
        /// <summary>
        /// The Nodes within the graph
        /// </summary>
        IEnumerable<IGraphNode> Nodes
        {
            get;
        }
        /// <summary>
        /// The Edges within the graph
        /// </summary>
        IEnumerable<IGraphEdge> Edges
        {
            get;
        }
        /// <summary>
        /// The Polygons within the graph
        /// </summary>
        IEnumerable<IGraphPoly> Polygons
        {
            get;
        }
    }
}