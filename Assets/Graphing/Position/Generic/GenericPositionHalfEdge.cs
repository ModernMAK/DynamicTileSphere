using Graphing.Generic;
using UnityEngine;

namespace Graphing.Position.Generic
{
    public class GenericPositionHalfEdge<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>
        : GenericHalfEdge<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>
        where GraphType : GenericPositionGraph<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonPartitionType : GenericPolygonPartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodePartitionType : GenericNodePartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonType : GenericPositionPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where HalfEdgeType : GenericPositionHalfEdge<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodeType : GenericPositionNode<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()

    {
        public GenericPositionHalfEdge(GraphType graph, int id, int nodeId, int polyId = -1, int nextId = -1, int prevId = -1, int twinId = -1) : base(graph, id, nodeId, polyId, nextId, prevId, twinId)
        {
        }
        public GenericPositionHalfEdge() : base()
        {
        }
        public Vector3 Center
        {
            get
            {
                Vector3 center = Node.Position;
                if (Twin != null)
                {
                    center += Twin.Node.Position;
                    center /= 2f;
                }
                return center;
            }
        }
    }
}