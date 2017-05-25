using Graphing.Generic;
namespace Graphing.Position.Generic
{
    public class GenericPositionGraph<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>
        : GenericGraph<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>
        where GraphType : GenericPositionGraph<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonPartitionType : GenericPolygonPartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodePartitionType : GenericNodePartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonType : GenericPositionPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where HalfEdgeType : GenericPositionHalfEdge<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodeType : GenericPositionNode<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
    {
        public class GenericPositionGraphSerializer<PolygonPartitionSerializerType, NodePartitionSerializerType, PolygonSerializerType, HalfEdgeSerializerType, NodeSerializerType> : GenericGraphSerializer<PolygonPartitionSerializerType, NodePartitionSerializerType, PolygonSerializerType, HalfEdgeSerializerType, NodeSerializerType>
            where PolygonPartitionSerializerType : GenericPolygonPartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericPolygonPartitionSerializer, new()
            where NodePartitionSerializerType : GenericNodePartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericNodePartitionSerializer, new()
            where PolygonSerializerType : GenericPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericPolygonSerializer, new()
            where HalfEdgeSerializerType : GenericHalfEdge<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericHalfEdgeSerializer, new()
            where NodeSerializerType : GenericNode<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericNodeSerializer, new()
        {
            public GenericPositionGraphSerializer(GraphType graph, string fName, string fDirectory = null, string fExtension = null) : base(graph, fName, fDirectory, fExtension)
            {
            }
        }
        public class GenericPositionGraphBuilder<PolygonPartitionBuilderType, NodePartitionBuilderType, PolygonBuilderType, HalfEdgeBuilderType, NodeBuilderType> : GenericGraphBuilder<PolygonPartitionBuilderType, NodePartitionBuilderType, PolygonBuilderType, HalfEdgeBuilderType, NodeBuilderType>
            where PolygonPartitionBuilderType : GenericPolygonPartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericPolygonPartitionBuilder, new()
            where NodePartitionBuilderType : GenericNodePartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericNodePartitionBuilder, new()
            where PolygonBuilderType : GenericPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericPolygonBuilder, new()
            where HalfEdgeBuilderType : GenericHalfEdge<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericHalfEdgeBuilder, new()
            where NodeBuilderType : GenericNode<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericNodeBuilder, new()
        {
            public GenericPositionGraphBuilder() : base()
            {
            }

        }
        public GenericPositionGraph(NodeType[] nodes, PolygonType[] polygons, HalfEdgeType[] edges, NodePartitionType[] nodePartititons, PolygonPartitionType[] polyPartitions) : base(nodes, polygons, edges, nodePartititons, polyPartitions)
        {
        }

        public GenericPositionGraph() : base()
        {
        }
    }
}