using Graphing.Generic;
namespace Graphing.Position
{
    public sealed class PositionPolygonPartition : GenericPolygonPartition<PositionGraph, PositionPolygonPartition, PositionNodePartition, PositionPolygon, PositionHalfEdge, PositionNode>
    {
        public PositionPolygonPartition() : base()
        {

        }
        public PositionPolygonPartition(PositionGraph graph, int id, params int[] nodes) : base(graph, id, nodes)
        {
        }
    }
}