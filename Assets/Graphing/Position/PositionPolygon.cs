using System.IO;
using Graphing.Position.Generic;

namespace Graphing.Position
{
    public class PositionPolygon
        : GenericPositionPolygon<PositionGraph, PositionPolygonPartition, PositionNodePartition, PositionPolygon, PositionHalfEdge, PositionNode>

    {
        public PositionPolygon() : base()
        {
        }
        public PositionPolygon(PositionGraph graph, int id, int partitionId, int edgeId) : base(graph, id, partitionId, edgeId)
        {
        }

    }
}

