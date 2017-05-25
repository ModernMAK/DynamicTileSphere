using UnityEngine;
namespace Graphing.Position
{
    using Position.Generic;
    public class PositionNode
        : GenericPositionNode<PositionGraph, PositionPolygonPartition, PositionNodePartition, PositionPolygon, PositionHalfEdge, PositionNode>
    {
        public PositionNode() : base()
        {
        }
        public PositionNode(PositionGraph graph, int id, int partitionId, Vector3 position, int edgeId = -1) : base(graph, id, partitionId, position, edgeId)
        {
        }

    }

}

