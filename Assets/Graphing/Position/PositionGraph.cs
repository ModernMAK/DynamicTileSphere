using Graphing.Position.Generic;
namespace Graphing.Position
{
    public class PositionGraph
        : GenericPositionGraph<PositionGraph, PositionPolygonPartition, PositionNodePartition, PositionPolygon, PositionHalfEdge, PositionNode>
    {

        //Mostly to make life easy for everyone
        public class PositionGraphSerializer : GenericPositionGraphSerializer<PositionPolygonPartition.GenericPolygonPartitionSerializer, PositionNodePartition.GenericNodePartitionSerializer, PositionPolygon.PositionPolygonSerializer, PositionHalfEdge.GenericHalfEdgeSerializer, PositionNode.GenericPositionNodeSerializer>
        {
            public PositionGraphSerializer(PositionGraph graph, string fName, string fDirectory = null, string fExtension = null) : base(graph, fName, fDirectory, fExtension)
            {
            }
        }
        //Mostly to make life easy for everyone
        public class PositionGraphBuilder : GenericPositionGraphBuilder<PositionPolygonPartition.GenericPolygonPartitionBuilder, PositionNodePartition.GenericNodePartitionBuilder, PositionPolygon.PositionPolygonBuilder, PositionHalfEdge.GenericHalfEdgeBuilder, PositionNode.GenericPositionNodeBuilder>
        {
        }
        public PositionGraph() : base()
        {
        }
        public PositionGraph(PositionNode[] nodes, PositionPolygon[] polygons, PositionHalfEdge[] edges, PositionNodePartition[] nodePartititons, PositionPolygonPartition[] polyPartititons) : base(nodes, polygons, edges, nodePartititons, polyPartititons)
        {
        }
    }
}