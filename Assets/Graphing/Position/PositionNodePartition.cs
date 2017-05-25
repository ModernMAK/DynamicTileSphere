using Graphing.Generic;
namespace Graphing.Position
{
    public sealed class PositionNodePartition : GenericNodePartition<PositionGraph, PositionPolygonPartition, PositionNodePartition, PositionPolygon, PositionHalfEdge, PositionNode>
    {
        public PositionNodePartition() : base()
        {
        }
        public PositionNodePartition(PositionGraph graph, int id, params int[] nodes) : base(graph, id, nodes)
        {
        }
    }
}

