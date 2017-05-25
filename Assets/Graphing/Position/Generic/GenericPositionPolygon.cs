using Graphing.Generic;
using UnityEngine;
namespace Graphing.Position.Generic
{
    public class GenericPositionPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>
        : GenericPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>
        where GraphType : GenericPositionGraph<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonPartitionType : GenericPolygonPartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodePartitionType : GenericNodePartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonType : GenericPositionPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where HalfEdgeType : GenericPositionHalfEdge<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodeType : GenericPositionNode<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
    {
        public GenericPositionPolygon(GraphType graph, int id, int partitionId, int edgeId) : base(graph, id, partitionId, edgeId)
        {
        }
        public GenericPositionPolygon() : base()
        {
        }
        public Vector3 Center
        {
            get
            {
                Vector3 zero = Vector3.zero;
                int counter = 0;
                foreach (HalfEdgeType edge in this)
                {
                    zero += edge.Node.Position;
                    counter++;
                }
                return zero / (counter != 0 ? counter : 1);
            }
        }
    }
}